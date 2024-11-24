#region using directives
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Data;
using AndyShared.ClassificationDataSupport.SheetSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.Support;
using JetBrains.Annotations;
using SettingsManager;
using static AndyShared.ClassificationDataSupport.SheetSupport.ItemClassDef;
using static UtilityLibrary.MessageUtilities;

#endregion

// username: jeffs
// created:  5/2/2020 9:28:15 AM

// nodi
// nodus
// nexus
// point
// thing
// part
// atom
// unit
// component
// object
// element
// item

// TreeComponent
// TreeObject
// TreeItem  


// TreeBase
// TreeNexus
// TreeTrunk
// TreeCore
// TreeStart
// TreeObject

// TreeRoot (the first node) (is also a )

//  (the nodes)

// TreeData (the data element)



namespace AndyShared.ClassificationDataSupport.TreeSupport
{

	public interface ITreeData<TE> : ICloneable, INotifyPropertyChanged
	where TE : class
	{

		string ID { get; set; }

		TE Parent { get; set; }

		string SortCode { get; set; }

		/// <summary>
		/// the depth in the tree of the parent
		/// </summary>
		int Depth { get; set; }

		/// <summary>
		/// can this object cannot be modified (true when IsLocked or IsFixed)
		/// </summary>
		bool CannotModify { get; set; }

		/// <summary>
		/// flag element created and parent assigned
		/// </summary>
		bool IsInitialized { get; set; }

		/// <summary>
		/// the element has been modified
		/// </summary>
		bool IsModified { get; set; }

		/// <summary>
		/// flag that this element meets the search criteria
		/// </summary>
		bool IsMatched { get; set; }

		/// <summary>
		/// flag, element has been locked (user lock)
		/// </summary>
		bool IsLocked { get; set; }

		/// <summary>
		/// flag, element is fixed & cannot be changed (system / admin lock)
		/// </summary>
		bool IsFixed { get; set; }

		/// <summary>
		/// match this item against the provided criteria
		/// </summary>
		bool Match(string criteria);

		/// <summary>
		/// issue property changed events for various properties
		/// </summary>
		void UpdateProperties();

		/// <summary>
		/// issue property changed events for various properties
		/// </summary>
		void UpdateInternalProperties();

		/// <summary>
		/// parent element notifies this item of a change
		/// </summary>
		void GetChangeFromParent(INTERNODE_MESSAGES msg);

		/// <summary>
		/// tell parent element of my changes
		/// </summary>
		void SetChangeToParent(INTERNODE_MESSAGES msg);

		/// <summary>
		/// update the title and description
		/// </summary>
		void Configure(string title, string description);

		/// <summary>
		/// create & provide a temporary item
		/// </summary>
		ITreeData<TE> GetTemp(ITreeData<TE> selected);

		new object Clone();

		ITreeData<TE> Clone(TE parent);

	}


#region TreeElement<TD>

	[DataContract(Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	public class TreeElement<TD> : INotifyPropertyChanged, ICloneable
		where TD : class, ITreeData<TreeElement<TD>>, new()
	{
		private static int index = 1;


	#region private fields

		// properties
		protected TD item;

		/// <summary>
		/// the list of child TreeElements
		/// </summary>
		protected ObservableCollection<TreeElement<TD>> children = new ObservableCollection<TreeElement<TD>>();

		private ListCollectionView childrenView;

		private TreeElement<TD> parent;
		private int depth;

		private CheckedState checkedState = CheckedState.UNCHECKED;
		private CheckedState triState = CheckedState.UNSET;

		private bool isInitialized;

		private bool treeElementModified;
		private bool isExpanded;
		private bool isExpandedAlt;
		private bool isElementSelected;
		private bool isContextTarget;
		private bool isContextSource;
		
		private int checkedChildCount;

		//										                 mixed->checked->unchecked->mixed
		//										                   0        1        2        3 (0)
		private static readonly bool?[] BoolList = new bool?[] {null,     true,    false,   null};
		private bool mixesStateBeenTold;

		private bool childItemModified;
		private bool isLocked;
		private static bool rememberExpCollapseState = false;
		
	#endregion

	#region ctor

		public TreeElement() : this(null, null) { }

		public TreeElement(TreeElement<TD> parent, TD item, bool isExpanded)
		{
			this.parent = parent;
			this.item = item;
			Depth = parent?.depth + 1 ?? 0;
			this.isExpanded = isExpanded;

			OnCreated();

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		}

		public TreeElement(TreeElement<TD> parent, TD item)
		{
			this.parent = parent;
			this.item = item;
			Depth = parent?.depth + 1 ?? 0;

			OnCreated();

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		}

		protected  TreeElement(TD item, bool isExpanded)
		{
			this.item = item;
			this.isExpanded = isExpanded;
			
			OnCreated();

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		}

		private void OnCreated()
		{
			Children.CollectionChanged += ChildrenOnCollectionChanged;

			IsInitialized = true;

			if (item != null)
			{
				item.Parent = this;
				item.IsInitialized = true;
			}

			ID = index++.ToString("X");

			UpdateParentProperties();

			item.UpdateProperties();
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext c)
		{
			Children = new ObservableCollection<TreeElement<TD>>();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext c)
		{
			OnCreated();

			if (!RememberExpCollapseState) isExpanded = false;
		}

	#endregion

	#region public properties

		[IgnoreDataMember]
		public string ID { get; set; }

		// track changes: yes - but indirect
		// the actual tree data item
		[DataMember(Order = 20)]
		public TD Item
		{
			get => item;

			private set
			{
				item = value;
			
				if (item == null) return;
				
				item.Depth = depth;
			
				OnPropertyChanged();
			}
		}

		// track changes: no const
		[IgnoreDataMember]
		public NodeType ElementType
		{
			get
			{
				if (parent == null && ChildCount > 0) return NodeType.ROOT;
				if (ChildCount == 0) return NodeType.LEAF;

				return NodeType.BRANCH;
			}
		}

// track changes: yes
		[DataMember(Order = 2)]
		public TreeElement<TD> Parent
		{
			get => parent;
			set
			{
				if (value?.Equals(parent) ?? false) return;

				parent = value;
				OnPropertyChanged();
				UpdateParentProperties();

				if (isInitialized) ElementChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public TreeElement<TD> RootParent => GetGrandParent();

// track changes: yes
		[DataMember(Order = 3)]
		public int Depth
		{
			get => depth;
			set
			{
				if (value == depth) return;

				depth = value;
				OnPropertyChanged();

				if (item != null) item.Depth = value;

				if (isInitialized) ElementChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
			}
		}

// track changes: yes
		[DataMember(Order = 5)]
		public CheckedState CheckedState
		{
			get => checkedState;

			private set
			{
				if (value == CheckedState.UNSET)
				{
					checkedState = value;
					if (isInitialized) ElementChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
				}
				else if (value != checkedState)
				{
					checkedState = value;
					OnPropertyChanged("Checked");

					parent?.UpdateChildCount(checkedState);

					if (isInitialized) ElementChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
				}
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool? Checked
		{
			get
			{
				if (checkedState == CheckedState.UNSET) return null;

				return BoolList[(int) checkedState];
			}

			set
			{
				processStateChange(selectStateFromBool(value));
				OnPropertyChanged();
			}
		}

// track changes: yes
		[DataMember(Order = 7)]
		public CheckedState TriState
		{
			get => triState;

			set
			{
				if (value != triState)
				{
					triState = value;
					OnPropertyChanged();
					if (isInitialized) ElementChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
				}
			}
		}

// track changes: yes
		[DataMember(Order = 30, Name = "SubCategories")]
		public ObservableCollection<TreeElement<TD>> Children
		{
			get => children;

			private set
			{
				if (value?.Equals(children) ?? false) return;

				children = value;
				NotifyChildrenChange();
				}
		}

		// public ICollectionView ChildrenView
		// track changes: no
		[IgnoreDataMember]
		public ListCollectionView ChildrenView
		{
			get => childrenView;

			private set
			{
				childrenView = value;

				OnPropertyChanged();
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool HasChildren => ChildCount > 0;

		// track changes: no
		[IgnoreDataMember]
		public int ChildCount => Children?.Count ?? 0;

		[IgnoreDataMember]
		// public int ExtendedChildCount => ExtendedChildrenCount(this);
		// track changes: no
		public int ChildCountEx => ChildrenCountEx();

		// track changes: no
		[IgnoreDataMember]
		public int CheckedChildCount
		{
			get => checkedChildCount;

			set
			{
				if (value != checkedChildCount)
				{
					checkedChildCount = value;
					OnPropertyChanged();
				}
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public object ChildrenCollectLock { get; set; }

		// track changes: no
		[IgnoreDataMember]
		public int ChildrenCollectLockIdx { get; set; }


	#region status properties

		[IgnoreDataMember]
		public bool IsLocked
		{
			get => isLocked || !item.CannotModify;
			set
			{
				if (value == isLocked) return;
				isLocked = value;
				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public static bool RememberExpCollapseState
		{
			get => rememberExpCollapseState;
			set
			{
				if (value == rememberExpCollapseState) return;
				rememberExpCollapseState = value;
			}
		}

		/// <summary>
		/// controls whether the element is expanded or not
		/// </summary>
// track changes: yes
		[DataMember(Order = 15)]
		public bool IsExpanded
		{
			get => isExpanded;

			set
			{
				if (value == isExpanded) return;

				isExpanded = value;

				OnPropertyChanged();

				if (RememberExpCollapseState)
				{
					if (isInitialized) ElementChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
				}
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool IsExpandedAlt
		{
			get => isExpandedAlt;
			set
			{
				isExpandedAlt = value;
				OnPropertyChanged();
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool CanExpand => ChildCount > 0;

		// track changes: n/a
		[IgnoreDataMember]
		public bool IsElementSelected
		{
			get => isElementSelected;
			set
			{
				// this is set in the WPF style
				// this occurs before SelectionChanged event

				isElementSelected = value;

				OnPropertyChanged();
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool IsContextTarget
		{
			get => isContextTarget;
			set
			{
				isContextTarget = value;
				OnPropertyChanged();
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool IsContextSource
		{
			get => isContextSource;
			set
			{
				isContextSource = value;
				OnPropertyChanged();
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool IsInitialized
		{
			get => isInitialized;
			set
			{
				if (value == isInitialized) return;
				isInitialized = value;

				OnPropertyChanged();
			}
		}

		// track changes: n/a
		[IgnoreDataMember]
		public virtual bool TreeElementModified
		{
			get
			{
				return treeElementModified; // || Item.TreeElementModified;
			}
			set
			{
				if (!isInitialized || value == treeElementModified) return;

				treeElementModified = value;
				OnPropertyChanged();
			}
		}

		// track changes: n/a
		[IgnoreDataMember]
		public virtual bool TreeElementChildItemModified
		{
			get => childItemModified;
			set
			{
				if (value == childItemModified) return;
				childItemModified = value;
				OnPropertyChanged();
			}
		}

	#endregion

	#endregion

	#region private properties

	#endregion

	#region public methods

		public TreeElement<TD> GetGrandParent()
		{
			TreeElement<TD> grandParent = this;

			for (int i = depth - 1; i >= 0 ; i--)
			{
				if (grandParent.parent.ElementType == NodeType.ROOT) break;

				grandParent = grandParent.parent;

			}

			return grandParent;
		}

		public bool IsChildOf(TreeElement<TD> testParent)
		{
			TreeElement<TD> checkParent = parent;

			for (int i = depth - 1; i >= 0 ; i--)
			{
				if (checkParent.Equals(testParent)) return true;

				checkParent = checkParent.parent;

			}

			return false;
		}

		public void InitializeAllChildrenView()
		{
			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;

			foreach (TreeElement<TD> element in Children)
			{
				element.InitializeAllChildrenView();
				element.IsInitialized = true;
			}
		}

		public void UpdateChildCount(CheckedState childState)
		{
			if (childState == CheckedState.MIXED)
			{
				if (!mixesStateBeenTold)
				{
					mixesStateBeenTold = true;
					CheckedChildCount++;
					return;
				}
			}

			mixesStateBeenTold = false;

			if (childState == CheckedState.CHECKED)
			{
				CheckedChildCount++;
				return;
			}

			CheckedChildCount--;
		}

		public static TreeElement<TD> TempTreeElement(TreeElement<TD> parent, TreeElement<TD> selected)
		{
			TD td = (TD) new TD().GetTemp(selected.item);

			TreeElement<TD> temp = new TreeElement<TD>(parent,td, false);

			return temp;
		}

		public void ResetElement()
		{
			CheckedState = CheckedState.UNCHECKED;
			TriState = CheckedState.UNSET;
		}

		public void ResetTree()
		{
			if (ElementType == NodeType.BRANCH)
			{
				// reset children then myself
				foreach (TreeElement<TD> element in ChildrenView )
				{
					element.ResetTree();
				}
			}

			ResetElement();
		}

		public void TriStateReset()
		{
			TriState = CheckedState.UNSET;
		}

		public void UpdateProperties()
		{
			OnPropertyChanged(nameof(Item));
			OnPropertyChanged(nameof(ChildrenView));
			OnPropertyChanged(nameof(HasChildren));
			OnPropertyChanged(nameof(ChildCount));
			OnPropertyChanged(nameof(ChildCountEx));
		}

		public void UpdateParentProperties()
		{
			OnPropertyChanged(nameof(RootParent));
		}

		// inter element communications routines

		// state changes
		// get notification from parent of a state modification
		public void StateChangeFromParent(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			if (useTriState)
			{
				if (triState == CheckedState.UNSET)
				{
					// first time through - save current
					triState = checkedState;
				}

				if (newState == CheckedState.MIXED) newState = triState;
			}
			else
			{
				triState = CheckedState.UNSET;
			}

			CheckedState = newState;

			notifyChildrenOfStateChange(newState, oldState, useTriState);
		}

		// get notification from child of a state modification
		public void StateChangeFromChild(CheckedState newState, CheckedState oldState, bool useTristate)
		{
			CheckedState priorState = checkedState;
			CheckedState finalState = CheckedState.UNSET;


			if (!useTristate)
			{
				triState = CheckedState.UNSET;
			}
			else if (triState == CheckedState.UNSET)
			{
				// usetristate is true
				// starting tristate process - save the current state
				triState = checkedState;
			}

			switch (newState)
			{
			// newstate is checked - child is checked
			case CheckedState.CHECKED:
				if (checkedChildCount == (Children?.Count ?? 0))
				{
					finalState = CheckedState.CHECKED;
				}
				else if (checkedState != CheckedState.MIXED)
				{
					finalState = CheckedState.MIXED;
				}

				break;
			// newstate is unchecked - child is unchecked
			case CheckedState.UNCHECKED:
				if (checkedChildCount == 0)
				{
					finalState = CheckedState.UNCHECKED;
				}
				else if (checkedState == CheckedState.CHECKED)
				{
					finalState = CheckedState.MIXED;
				}

				break;
			// newstate is checked - child is mixed
			case CheckedState.MIXED:
				// must evaluate whether coming from checked to mixed or
				// from unchecked to mixed

				// i am checked, flip from checked to
				// mixed.  inform parent.
				if (checkedState == CheckedState.CHECKED ||
					checkedState == CheckedState.UNCHECKED)
				{
					finalState = CheckedState.MIXED;
				}

				break;
			}

			if (finalState != CheckedState.UNSET)
			{
				CheckedState = finalState;
				notifyParentOfStateChange(finalState, priorState, useTristate);
			}
		}

		// inform children to modification
		public void ItemChangeFromParent(INTERNODE_MESSAGES modification)
		{
			if (item == null || ChildrenView == null)
			{
				return;
			}

			if (modification == INTERNODE_MESSAGES.IM_CLEAR_ITEM_MODIFICATION)
			{
				if (!TreeElementChildItemModified)
				{
					return;
				}

				TreeElementChildItemModified = false;
			}
			else 
			if (modification == INTERNODE_MESSAGES.IM_PARENT_FIXED_CHANGED && item!= null)
			{
				item.UpdateInternalProperties();
			}
			else
			{
				return;
			}

			item.GetChangeFromParent(modification);

			notifyChildElementOfItemChange(modification);
		}

		// get notification from child of an item modification
		// for this modification can come from a child element or
		// can come from a child item
		public void ItemChangeFromChild(INTERNODE_MESSAGES change)
		{
			TreeElementChildItemModified = true;

			if (change == INTERNODE_MESSAGES.IM_PARENT_FIXED_CHANGED)
			{
				notifyChildElementOfItemChange(change);
			} 
			else
			{
				notifyParentOfItemChange(change);
			}
		}

		public void ElementChangeFromChild(INTERNODE_MESSAGES modification)
		{
			if (modification == INTERNODE_MESSAGES.IM_IS_MODIFIED)
			{
				TreeElementModified = true;

				notifyParentOfElementChange(modification);
			}
			
		}

		public void ElementChangeFromParent(INTERNODE_MESSAGES modification)
		{
			if (ChildrenView == null)
			{
				return;
			}

			if (modification == INTERNODE_MESSAGES.IM_CLEAR_NODE_MODIFICATION)
			{
				if (!TreeElementModified)
				{
					return;
				}

				TreeElementModified = false;
			}
			else
			if (modification == INTERNODE_MESSAGES.IM_EXPAND_ALL)
			{
				IsExpanded = true;
			}
			else
			if (modification == INTERNODE_MESSAGES.IM_COLLAPSE_ALL)
			{
				IsExpanded = false;
			}
			else
			{
				return;
			}

			notifyChildElementsOfElementChange(modification);
		}

		// notify window of changes
		public void NotifyChildrenChange()
		{
			OnPropertyChanged("Children");
			OnPropertyChanged("ChildrenView");
			OnPropertyChanged("ChildCount");
			OnPropertyChanged("HasChildren");

			if (isInitialized) ElementChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
		}

	#endregion

	#region private methods

		/* inter element communications*/

		// notify parent / children of state changes
		private void notifyParentOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			parent?.StateChangeFromChild(newState, oldState, useTriState);
		}

		private void notifyChildrenOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			if (ChildrenView == null) return;

			foreach (TreeElement<TD> element in ChildrenView)
			{
				element.StateChangeFromParent(newState, oldState, useTriState);
			}
		}

		private void notifyParentOfItemChange(INTERNODE_MESSAGES modification)
		{
			parent?.ItemChangeFromChild(modification);
		}

		private void notifyParentOfElementChange(INTERNODE_MESSAGES modification)
		{
			parent?.ElementChangeFromChild(modification);
		}

		private void notifyChildElementOfItemChange(INTERNODE_MESSAGES modification)
		{
			foreach (TreeElement<TD> element in ChildrenView)
			{
				element.ItemChangeFromParent(modification);
			}

		}

		private void notifyChildElementsOfElementChange(INTERNODE_MESSAGES modification)
		{
			foreach (TreeElement<TD> element in ChildrenView)
			{
				element.ElementChangeFromParent(modification);
			}
		}


		// process routines

		private int ChildrenCountEx()
		{
			if (children.Count == 0) return 0;

			int count = children.Count;

			foreach (TreeElement<TD> child in children)
			{
				count += child.ChildCountEx;
			}

			return count;
		}

		private void processStateChangeLeaf(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			CheckedState = newState;

			notifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void processStateChangeBranch(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			CheckedState = newState;

			notifyChildrenOfStateChange(newState, oldState, useTriState);
			notifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void processStateChange(CheckedState newState)
		{
			CheckedState oldState = checkedState;

			// process when leaf
			if (ElementType == NodeType.LEAF)
			{
				processStateChangeLeaf(newState, checkedState, false);

				return;
			}

			// element type is branch
			if (triState == CheckedState.UNSET)
			{
				// not currently tristate processing

				if (checkedState == CheckedState.CHECKED ||
					checkedState == CheckedState.UNCHECKED)
				{
					processStateChangeBranch(newState, oldState, false);
				}
				// state is mixed // start using tri-state
				else
				{
					triState = checkedState;

					processStateChangeBranch(newState, oldState,  true);
				}
			}
			else
				// selectStateOriginal is set
				// processing a tri-state modification
				// mixed to checked
				// checked to unchecked
				// unchecked to mixed

				// current is mixed -> modification to checked
				// current is checked -> modification to unchecked
				// current is unchecked -> modification to mixed
			{
				CheckedState proposed =
					selectStateFromBool(BoolList[(int) oldState + 1]);

				CheckedState = proposed;

				notifyParentOfStateChange(proposed, oldState, true);
				notifyChildrenOfStateChange(proposed, oldState, true);
			}
		}

		private int indexInBoolList(bool? test)
		{
			for (var i = 0; i < BoolList.Length; i++)
			{
				if (BoolList[i] == test) return i;
			}

			return 0;
		}

		private CheckedState selectStateFromBool(bool? test)
		{
			return (CheckedState) indexInBoolList(test);
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event consuming

		private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			TreeElementModified = true;
		}

	#endregion

	#region system overrides

		// creates a partial clone - does not clone the children
		public object Clone()
		{
			TreeElement<TD> newElement = new TreeElement<TD>(parent, (TD) item.Clone(), false);

			newElement.checkedState = checkedState;
			newElement.isExpanded = isExpanded;
			newElement.triState = triState;

			newElement.depth = depth;

			return newElement;
		}

		public TreeElement<TD> Clone(int whichParent)
		{
			TreeElement<TD> newElement = null;

			if (whichParent==0)
			{
				newElement= new TreeElement<TD>(null, false);
				newElement.item = (TD) item.Clone(newElement);
			}
			else
			if (whichParent==1)
			{
				newElement = new TreeElement<TD>(parent, (TD) item.Clone(), false);
			}
			else
			if (whichParent==2)
			{
				newElement = new TreeElement<TD>(this, (TD) item.Clone(), false);
			} 
			else
			{
				return null;
			}

			newElement.checkedState = checkedState;
			newElement.isExpanded = isExpanded;
			newElement.triState = triState;
			newElement.depth = depth;

			return newElement;
		}

		// create a complete clone element + all children
		public TreeElement<TD> CloneEx()
		{
			return Clone(this);
		}

		private TreeElement<TD> Clone(TreeElement<TD> parentElement)
		{
			TreeElement<TD> copy = (TreeElement<TD>) parentElement.Clone();

			foreach (TreeElement<TD> element in parentElement.children)
			{
				copy.children.Add(Clone(element));
			}

			return copy;
		}

		public override string ToString()
		{
			return
				$"{ID} :: {ElementType} :: {checkedState} | expanded = {isExpanded} | alt exp = {isExpandedAlt} | item = {item.ToString()}";
		}

	#endregion
	}

#endregion

#region TreeRoot

	[DataContract(Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
	public class TreeRoot<TD> : TreeElement<TD>
		where TD : class, ITreeData<TreeElement<TD>>, new()
	{
	#region private fields

		private TreeElement<TD> selectedElement;

		private bool treeElementModified;

		private bool treeElementChildItemModified;

	#endregion

	#region ctor

		public TreeRoot() : base(null, false)
		{
			Depth = 0;

			item = new TD();
			item.Configure("Initial Item", "Initial Item");
		}

		public void Initalize()
		{
			IsInitialized = true;

			InitializeAllChildrenView();
		}

	#endregion

	#region public properties

		public TreeElement<TD> SelectedElement
		{
			get => selectedElement;
			set
			{
				selectedElement = value;
				OnPropertyChanged();
				OnPropertyChanged("HasSelection");
			}
		}

		public bool HasSelection => SelectedElement != null;

		public override bool TreeElementModified
		{
			get => treeElementModified;
			set
			{
				if (value == treeElementModified) return;
				treeElementModified = value;
				OnPropertyChanged();

				RaiseTreeModifiedEvent();
			}
		}

		public override bool TreeElementChildItemModified
		{
			get => treeElementChildItemModified;
			set
			{
				if (value == treeElementChildItemModified) return;
				treeElementChildItemModified = value;
				OnPropertyChanged();

				RaiseTreeItemModifiedEvent();
			}
		}

		public bool HasGrandChildren => hasGrandChildren();

		public bool HasGrandChildrenExpanded => hasGrandChildrenExpanded();

	#endregion

	#region public methods

		/*inter element communications */

		public void NotifyChangeFromParent(INTERNODE_MESSAGES mod)
		{
			if ((int) mod >= 0)
			{
				ElementChangeFromParent(mod);
			} 

			if ((int) mod <= 0)
			{
				ItemChangeFromParent(mod);
			}
		}

		/* expansion control */

		public void ExpandAll()
		{
			NotifyChangeFromParent(INTERNODE_MESSAGES.IM_EXPAND_ALL);

			OnPropertyChanged(nameof(HasGrandChildrenExpanded));
		}

		public void CollapseAll()
		{
			NotifyChangeFromParent(INTERNODE_MESSAGES.IM_COLLAPSE_ALL);

			OnPropertyChanged(nameof(HasGrandChildrenExpanded));
		}

		/* element adjustments */

		/// <summary>
		/// remove the element provided - but not if element is locked
		/// </summary>
		/// <param name="element"></param>
		public void RemoveElement2(TreeElement<TD> element)
		{
			if (element.IsLocked) return;

			TreeElement<TD> parent = element.Parent;
			parent.Children.Remove(element);

			NotifyChildrenChange();
		}

		/// <summary>
		/// add a new / temp element to the element provided - but not if element is locked<br/>
		/// return the temp element
		/// </summary>
		public TreeElement<TD> AddNewChild2(TreeElement<TD> element)
		{
			if (element.IsLocked) return null;

			TreeElement<TD> temp = TempTreeElement(element, element);
			
			element.Children.Add(temp);

			element.NotifyChildrenChange();

			return temp;
		}

		/// <summary>
		/// add a new / temp element to the element provided - but not if element is locked
		/// </summary>
		public void AddChild2(TreeElement<TD> element, TreeElement<TD> toAddElement)
		{
			if (element.IsLocked) return ;

			toAddElement.Depth = ++element.Depth;
			element.Children.Add(toAddElement);

			NotifyChildrenChange();
		}

		/// <summary>
		/// adds a new, temp element to the parent of the selected element<br/>
		/// but not if the parent is locked
		/// </summary>
		public TreeElement<TD> AddNewBefore2(TreeElement<TD> element)
		{
			TreeElement<TD> parent = element.Parent;

			if (parent.IsLocked) return null;

			TreeElement<TD> temp = TempTreeElement(parent, parent);

			// temp.IsElementSelected = true;

			AddAt2(parent, temp, parent.Children.IndexOf(element));

			return temp;
		}

		// adds without notifying of the revision
		/// <summary>
		/// adds provided element to the parent of the selected element<br/>
		/// but not if the parent is locked
		/// </summary>
		public void AddBefore2(TreeElement<TD> element, TreeElement<TD> toAddElement)
		{
			toAddElement.Depth = ++element.Depth;

			TreeElement<TD> parent = element.Parent;

			if (parent.IsLocked) return;

			AddAt2(parent, toAddElement, parent.Children.IndexOf(element));
		}

		// adds a new, temp element before the selected element
		/// <summary>
		/// adds a new, temp element to the parent of the selected element<br/>
		/// after the selected element<br/>
		/// but not if the parent is locked
		/// </summary>
		public TreeElement<TD> AddNewAfter2(TreeElement<TD> element)
		{
			TreeElement<TD> parent = element.Parent;

			if (parent.IsLocked) return null;

			TreeElement<TD> temp = TempTreeElement(parent, parent);

			AddAt2(parent, temp, parent.Children.IndexOf(element) + 1);

			return temp;
		}

		// adds without notifying of the revision
		/// <summary>
		/// adds the provided element to the parent of the selected element<br/>
		/// after the selected element<br/>
		/// but not if the parent is locked
		/// </summary>
		public void AddAfter2(TreeElement<TD> element, TreeElement<TD> toAddElement)
		{
			TreeElement<TD> parent = element.Parent;

			if (parent.IsLocked) return;

			toAddElement.Depth = parent.Depth + 1;

			AddAt2(parent, toAddElement, parent.Children.IndexOf(element) + 1);
		}

		/// <summary>
		/// adds the provided element to the parent of the selected element<br/>
		/// after the selected element<br/>
		/// but not if the parent is locked
		/// </summary>
		public void AddElement(TreeElement<TD> element)
		{
			if (element.Parent.IsLocked) return;

			element.Depth = element.Parent.Depth + 1;

			element.Parent.Children.Add(element); 
			
			NotifyChildrenChange();
		}

		/// <summary>
		/// move sourceElement to before targetElement - at the same depth / same parent
		/// </summary>
		/// <param name="targetElement">the selected element</param>
		/// <param name="sourceElement">the existing element being relocated</param>
		public void MoveBefore(TreeElement<TD> targetElement, TreeElement<TD> sourceElement)
		{
			sourceElement.Depth = targetElement.Depth;

			moveElement2(targetElement, sourceElement, NodePlacement.BEFORE);
		}

		/// <summary>
		/// move the existing element after the context selected element
		/// </summary>
		/// <param name="element">the selected element</param>
		/// <param name="existingElement">the existing element being relocated</param>
		public void MoveAfter(TreeElement<TD> element, TreeElement<TD> existingElement)
		{
			existingElement.Depth = element.Depth;

			moveElement2(element, existingElement, NodePlacement.AFTER);
		}

		/// <summary>
		/// move the existing element to be a child of the selected element
		/// </summary>
		/// <param name="element">the selected element</param>
		/// <param name="existingElement">the element being relocated</param>
		public void MoveAsChild(TreeElement<TD> element, TreeElement<TD> existingElement)
		{
			moveAsChild2(element, existingElement);

		}

	#endregion

	#region private methods

		private void CannotAddChildError(IntPtr handle =  default)
		{
			CommonTaskDialogs.CommonErrorDialog(
				"Cannot Add Sub-Category",
				"A sub-category cannot be added",
				"The maximum sheet classification depth has" + nl
				+ "been reached and a sub-category cannot be added",
				handle);
		}

		/// <summary>
		/// inserts a child element at the index provided<br/>
		/// includes notify children of change
		/// </summary>
		private void AddAt2(TreeElement<TD> parent, TreeElement<TD> toAddElement, int index)
		{
			parent.Children.Insert(index, toAddElement);
			
			NotifyChildrenChange();
		}

		/// <summary>
		/// moves withiin a parent's collection - or -
		/// from one parent to another
		/// move the source note to be a child of the target element's parent <br/>
		/// but not if the target element is locked
		/// </summary>
		private void moveElement2(TreeElement<TD> targetElement, TreeElement<TD> sourceElement, NodePlacement how)
		{
			if (targetElement.Parent.IsLocked) return;

			TreeElement<TD> parent = targetElement.Parent;

			int contextIdx = parent.Children.IndexOf(targetElement);

			contextIdx = how == NodePlacement.AFTER ? ++contextIdx : contextIdx;

			if (sourceElement.Parent.Equals(parent))
			{
				int existIdx = parent.Children.IndexOf(sourceElement);

				if (contextIdx < parent.Children.Count )
				{
					// simple move within the same children collection
					parent.Children.Move(existIdx, contextIdx);

				} 
				else
				{ 
					// simple move within the same children collection
					// but gets placed at the end of the collection
					parent.Children.Insert(contextIdx, sourceElement);
					parent.Children.RemoveAt(existIdx);
				}

				NotifyChildrenChange();
			}
			else
			{
				// parents are different - moving from one 
				// parent's collection to another parent's collection

				// however, must check that the new parent is not a child 
				// of the source

				// complex move from one collection to another
				// this means, add then delete

				if (targetElement.IsChildOf(sourceElement))
				{
					return;
				}

				TreeElement<TD> exParent = sourceElement.Parent;

				// update the parent of the existing element
				sourceElement.Parent = targetElement.Parent;

				// add in the parent collection of he selected element
				AddAt2(parent, sourceElement, contextIdx);

				// remove from the original collection
				exParent.Children.Remove(sourceElement);
			}
		}

		// move to be the child of targetElement

		/// <summary>
		/// move to be the child of targetElement
		/// </summary>
		private void moveAsChild2(TreeElement<TD> targetElement, TreeElement<TD> sourceElement)
		{
			if (targetElement.IsLocked) return;

			// save the original parent for later use
			TreeElement<TD> exParent = sourceElement.Parent;

			// update the parent of the existing element
			sourceElement.Parent = targetElement;

			// add as first element
			AddAt2(targetElement, sourceElement, 0);

			exParent.Children.Remove(sourceElement);
		}

		/// <summary>
		/// determine of this element has any grand children
		/// </summary>
		private bool hasGrandChildren()
		{
			foreach (TreeElement<TD> child in children)
			{
				if (child.ChildCount > 0) return true;
			}
			
			return false;
		}

		/// <summary>
		/// determine if any children elements are expanded
		/// </summary>
		private bool hasGrandChildrenExpanded()
		{
			foreach (TreeElement<TD> child in children)
			{
				if (child.ChildCount > 0 && child.IsExpanded) return true;
			}

			return false;
		}


	#endregion

	#region event issuing

		public delegate void TreeModifiedEventHandler(object sender);

		public event TreeRoot<TD>.TreeModifiedEventHandler TreeModified;

		protected virtual void RaiseTreeModifiedEvent()
		{
			TreeModified?.Invoke(this);
		}

		public delegate void TreeItemModifiedEventHandler(object sender);

		public event TreeRoot<TD>.TreeItemModifiedEventHandler TreeItemModified;

		protected virtual void RaiseTreeItemModifiedEvent()
		{
			TreeItemModified?.Invoke(this);
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return ElementType + ":: ** class TreeRoot ** ::" + CheckedState;
		}

	#endregion
	}

#endregion
}