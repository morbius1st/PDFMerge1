#region using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Data;
using static UtilityLibrary.MessageUtilities;
using AndyShared.Support;
using DebugCode;
using UtilityLibrary;
using Test3.TreeNoteTests;

#endregion

// username: jeffs
// created:  5/2/2020 9:28:15 AM

namespace AndyShared.ClassificationDataSupport.TreeSupport
{
	public enum CheckedState2
	{
		UNSET = -1,
		MIXED = 0,
		CHECKED = 1,
		UNCHECKED = 2
	}

	public enum NodeType2
	{
		BRANCH,
		LEAF
	}

	public enum NodePlacement2
	{
		BEFORE = -1,
		AFTER = 1
	}

	public enum NodeStatus
	{
		NS_TREESAVED = 0,
		NS_EXPANDCHILDREN = 1,
		NS_COLLAPSECHILDREN = 2,
		NS_EXPANDTREE=3,
		NS_COLLAPSETREE=4,
	}


	// public enum SelectState2
	// {
	// 	UNSET = -1,
	// 	UNCHECKED = 0,
	// 	CHECKED = 1,
	// 	MIXED = 2
	// }
	//
	// public enum SelectMode2
	// {
	// 	TWO_STATE = 2,
	// 	TRI_STATE = 3
	// }



#region TreeNode2

	[DataContract(Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	public class TreeNode2<T> : INotifyPropertyChanged, ICloneable
	where T : class, INodeItem2
	{
	#region private fields

		// properties
		protected T item;

		/// <summary>
		/// the list of child TreeNodes
		/// </summary>
		protected ObservableCollection<TreeNode2<T>> children = new ObservableCollection<TreeNode2<T>>();

		private ListCollectionView childrenView;

		private TreeNode2<T> parent;
		private int depth;
		private static int maxDepth = 7;

		// private int extMergeItemCount;
		// protected int extItemCountCurrent;
		// private int extMgrCountCurr;

		private CheckedState2 checkedState2 = CheckedState2.UNCHECKED;
		private CheckedState2 triState = CheckedState2.UNSET;

		private bool isInitialized;

		private bool isModified;
		private bool isExpanded;
		private bool isExpandedAlt;
		private bool isNodeSelected;
		private bool isContextSelected;
		private int checkedChildCount;

		private bool rememberExpCollapseState;

		// private bool isSaving;

		// fields

		//										                 mixed->checked->unchecked->mixed
		//										                   0        1        2        3 (0)
		private static readonly bool?[] BoolList = new bool?[] {null,     true,    false,   null};
		private bool mixesStateBeenTold;

		// private Orator.ConfRoom.Announcer onModifiedAnnouncer;

	#endregion

	#region ctor

		public TreeNode2(TreeNode2<T> parent, T item, bool isExpanded)
		{
			// Debug.WriteLine("@TreeNode2<T> ctor| item.title| " + parent.Item.Title 
			// 	+ " parent depth| " + parent.depth );

			this.parent = parent;
			this.item = item;
			Depth = parent.depth + 1;
			this.isExpanded = isExpanded;

			// Children = new ObservableCollection<TreeNode2<T>>();
			OnCreated();

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		}

		public TreeNode2() : this(null, null, false)
		{
			// Debug.WriteLine("@TreeNode2<T> ctor| empty treenode2 created");

		}

		protected  TreeNode2(T item, bool isExpanded)
		{
			this.item = item;
			this.isExpanded = isExpanded;

			OnCreated();

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		}

		private void OnCreated()
		{
			Children.CollectionChanged += ChildrenOnCollectionChanged;

			// childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;	

			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ treenode|@ oncreated");

			// listen to parent, initialize
			// Orator.Listen(OratorRooms.TN_INIT, OnAnnouncedTnInit);

			// listen to parent, changes have been saved
			// Orator.Listen(OratorRooms.SAVED, OnAnnouncedSaved);

			// listen to parent, changes have been saved
			// Orator.Listen(OratorRooms.SAVING, OnSavingAnnounce);

			// RemExCollapseListenConfig();

			// onModifiedAnnouncer = Orator.GetAnnouncer(this, OratorRooms.MODIFIED);

			IsInitialized = true;
			isModified = false;
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext c)
		{
			Children = new ObservableCollection<TreeNode2<T>>();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext c)
		{
			OnCreated();

			// Debug.WriteLine("@TreeNode2<T> onDeserialized| is expanded?");
			if (!rememberExpCollapseState) isExpanded = false;
		}

		// [OnSerializing]
		// private void Onerializing(StreamingContext c)
		// {
		// 	if (!rememberExpCollapseState) isExpanded = false;
		// }

		// private void RemExCollapseListenConfig()
		// {
		// 	// listen to parent, changes have been saved
		// 	Orator.Listen(OratorRooms.TN_REM_EXCOLLAPSE_STATE, OnAnnouncedRemExCollapseState);
		//
		// 	RememberExpCollapseState = (bool) (Orator.GetLastValue(OratorRooms.TN_REM_EXCOLLAPSE_STATE) ?? false);
		//
		// }

	#endregion

	#region public properties

		// public static int Ct = 0;

		// the actual tree data item
		[DataMember(Order = 20)]
		public T Item
		{
			get => item;
			set
			{
				item = value;

				if (item == null) return;

				// Ct++;

				item.Depth = depth;

				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public NodeType2 NodeType2
		{
			get
			{
				if (ChildCount == 0) return NodeType2.LEAF;

				return NodeType2.BRANCH;
			}
		}

		[DataMember(Order = 2)]
		public TreeNode2<T> Parent
		{
			get => parent;
			set
			{
				if (value?.Equals(parent) ?? false) return;

				parent = value;
				OnPropertyChange();
				IsModified = true;
			}
		}

		[DataMember(Order = 3)]
		public int Depth
		{
			get => depth;
			set
			{
				if (value == depth) return;

				depth = value;
				OnPropertyChange();

				if (item != null) item.Depth = value;

				IsModified = true;
			}
		}

		[DataMember(Order = 5)]
		public CheckedState2 CheckedState2
		{
			get => checkedState2;

			private set
			{
				if (value == CheckedState2.UNSET)
				{
					checkedState2 = value;
					IsModified = true;
				}
				else if (value != checkedState2)
				{
					checkedState2 = value;
					OnPropertyChange("Checked");

					parent?.UpdateChildCount(checkedState2);

					IsModified = true;
				}
			}
		}

		[IgnoreDataMember]
		public bool? Checked
		{
			get
			{
				if (checkedState2 == CheckedState2.UNSET) return null;

				return BoolList[(int) checkedState2];
			}

			set
			{
				processStateChange(selectStateFromBool(value));
				OnPropertyChange();
			}
		}

		[DataMember(Order = 7)]
		public CheckedState2 TriState
		{
			get => triState;

			set
			{
				if (value != triState)
				{
					triState = value;
					OnPropertyChange();
					IsModified = true;
				}
			}
		}

		[DataMember(Order = 30, Name = "SubCategories")]
		public ObservableCollection<TreeNode2<T>> Children
		{
			get => children;

			private set
			{
				if (value?.Equals(children) ?? false) return;

				children = value;
				NotifyChildrenChange();
				IsModified = true;
			}
		}

		// public ICollectionView ChildrenView
		[IgnoreDataMember]
		public ListCollectionView ChildrenView
		{
			get => childrenView;

			private set
			{
				childrenView = value;

				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public bool HasChildren => ChildCount > 0;

		[IgnoreDataMember]
		public int ChildCount => Children?.Count ?? 0;

		// [IgnoreDataMember]
		// public int ExtendedChildCount => ExtendedChildrenCount(this);

		public int ExtChildCount => ExtChildrenCount();

		[IgnoreDataMember]
		public int CheckedChildCount
		{
			get => checkedChildCount;

			set
			{
				if (value != checkedChildCount)
				{
					checkedChildCount = value;
					OnPropertyChange();
				}
			}
		}

		[IgnoreDataMember]
		public object ChildrenCollectLock { get; set; }

		[IgnoreDataMember]
		public int ChildrenCollectLockIdx { get; set; }


		[DataMember(Order = 8)]
		public bool RememberExpCollapseState
		{
			get => rememberExpCollapseState;
			set
			{
				if (value == rememberExpCollapseState) return;
				rememberExpCollapseState = value;
				OnPropertyChange();
			}
		}



	// #region item properties

		// [IgnoreDataMember]
		// public int ItemCount => item.Count;
		//
		// [IgnoreDataMember]
		// public int ExtMergeItemCount
		// {
		// 	get => extMergeItemCount;
		// 	set
		// 	{
		// 		extMergeItemCount = value;
		// 		OnPropertyChange();
		// 	}
		// }

		// [IgnoreDataMember]
		// public virtual int ExtMergeItemCountCurrent
		// {
		// 	get => extItemCountCurrent;
		// 	// {
		// 	// 	int count = item.MergeItemCount;
		// 	//
		// 	// 	foreach (TreeNode2<T> childNode in children)
		// 	// 	{
		// 	// 		count += childNode.ExtMergeItemCountCurrent;
		// 	// 	}
		// 	//
		// 	// 	return count;
		// 	// }
		// 	
		// 	set
		// 	{
		// 		if (value != -1) return;
		//
		// 			int count = item.MergeItemCount;
		// 		
		// 			foreach (TreeNode2<T> childNode in children)
		// 			{
		// 				childNode.ExtMergeItemCountCurrent = -1;
		// 				count += childNode.ExtMergeItemCountCurrent;
		// 			}
		//
		// 			extItemCountCurrent = count;
		//
		// 			OnPropertyChange();
		// 	}
		//
		// }

	// #endregion

	#region status properties

		/// <summary>
		/// controls whether the node is expanded or not
		/// </summary>
		[DataMember(Order = 15)]
		public bool IsExpanded
		{
			get => isExpanded;

			set
			{
				if (value == isExpanded) return;

				isExpanded = value;

				OnPropertyChange();

				// Debug.WriteLine("isexpanded set| " + value.ToString());

				if (rememberExpCollapseState) IsModified = true;
			}
		}

		[IgnoreDataMember]
		public bool IsExpandedAlt
		{
			get => isExpandedAlt;
			set
			{
				isExpandedAlt = value;
				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public bool CanExpand => ChildCount > 0;

		[IgnoreDataMember]
		public bool IsNodeSelected
		{
			get => isNodeSelected;
			set
			{
				if (item.IsFixed || item.IsLocked)
				{
					isNodeSelected = false;
					OnPropertyChange();
					return;
				}

				Debug.WriteLine("@ is node selected| " + item.Id);


				isNodeSelected = value;
				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public bool IsContextSelected
		{
			get => isContextSelected;
			set
			{
				isContextSelected = value;
				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public bool IsContextHighlighted
		{
			get => isContextSelected;
			set
			{
				isContextSelected = value;
				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public bool IsInitialized
		{
			get => isInitialized;
			set { isInitialized = value; }
		}

		[IgnoreDataMember]
		public bool IsModified
		{
			get
			{
				return isModified; // || Item.IsModified;
			}
			set
			{
				if (!isInitialized || value == isModified) return;

				isModified = value;
				OnPropertyChange();

				// if (isInitialized)
				// {
				// 	onModifiedAnnouncer.Announce(null);
				// }
			}
		}

		[IgnoreDataMember]
		public bool IsMaxDepth => depth >= maxDepth;


	#endregion

	#endregion

	#region private properties

	#endregion

	#region public methods

		// public int CountExtMergeItems()
		// {
		// 	int count = item.Count;
		//
		// 	foreach (TreeNode2<T> childNode in children)
		// 	{
		// 		count += childNode.CountExtMergeItems();
		// 	}
		//
		// 	ExtMergeItemCount = count;
		//
		// 	return count;
		// }

		public void InitializeAllChildrenView()
		{
			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;

			foreach (TreeNode2<T> node in Children)
			{
				node.InitializeAllChildrenView();
				node.IsInitialized = true;
			}
		}

		public void UpdateChildCount(CheckedState2 childState)
		{
			if (childState == CheckedState2.MIXED)
			{
				if (!mixesStateBeenTold)
				{
					mixesStateBeenTold = true;
					CheckedChildCount++;
					return;
				}
			}

			mixesStateBeenTold = false;

			if (childState == CheckedState2.CHECKED)
			{
				CheckedChildCount++;
				return;
			}

			CheckedChildCount--;
		}

		public static TreeNode2<T> TempTreeNode(TreeNode2<T> parent)
		{
			TreeNode2<T> temp = new TreeNode2<T>(parent, (T) parent.Item.TempNodeItem(parent.depth), false);

			return temp;
		}

		public void ResetNode()
		{
			CheckedState2 = CheckedState2.UNCHECKED;
			TriState = CheckedState2.UNSET;
		}

		public void ResetTree()
		{
			if (NodeType2 == NodeType2.BRANCH)
			{
				// reset children then myself
				foreach (TreeNode2<T> node in ChildrenView )
				{
					node.ResetTree();
				}
			}

			ResetNode();
		}

		public void StateChangeFromParent(CheckedState2 newState, CheckedState2 oldState, bool useTriState)
		{
			if (useTriState)
			{
				if (triState == CheckedState2.UNSET)
				{
					// first time through - save current
					triState = checkedState2;
				}

				if (newState == CheckedState2.MIXED) newState = triState;
			}
			else
			{
				triState = CheckedState2.UNSET;
			}

			CheckedState2 = newState;

			notifyChildrenOfStateChange(newState, oldState, useTriState);
		}

		public void StateChangeFromChild(CheckedState2 newState, CheckedState2 oldState, bool useTristate)
		{
			CheckedState2 priorState = checkedState2;
			CheckedState2 finalState = CheckedState2.UNSET;


			if (!useTristate)
			{
				triState = CheckedState2.UNSET;
			}
			else if (triState == CheckedState2.UNSET)
			{
				// usetristate is true
				// starting tristate process - save the current state
				triState = checkedState2;
			}

			switch (newState)
			{
			// newstate is checked - child is checked
			case CheckedState2.CHECKED:
				if (checkedChildCount == (Children?.Count ?? 0))
				{
					finalState = CheckedState2.CHECKED;
				}
				else if (checkedState2 != CheckedState2.MIXED)
				{
					finalState = CheckedState2.MIXED;
				}

				break;
			// newstate is unchecked - child is unchecked
			case CheckedState2.UNCHECKED:
				if (checkedChildCount == 0)
				{
					finalState = CheckedState2.UNCHECKED;
				}
				else if (checkedState2 == CheckedState2.CHECKED)
				{
					finalState = CheckedState2.MIXED;
				}

				break;
			// newstate is checked - child is mixed
			case CheckedState2.MIXED:
				// must evaluate whether coming from checked to mixed or
				// from unchecked to mixed

				// i am checked, flip from checked to
				// mixed.  inform parent.
				if (checkedState2 == CheckedState2.CHECKED ||
					checkedState2 == CheckedState2.UNCHECKED)
				{
					finalState = CheckedState2.MIXED;
				}

				break;
			}

			if (finalState != CheckedState2.UNSET)
			{
				CheckedState2 = finalState;
				notifyParentOfStateChange(finalState, priorState, useTristate);
			}
		}

		public void StatusChangeFromParent(NodeStatus newStat)
		{
			bool propagate = false;

			switch (newStat)
			{
			case NodeStatus.NS_TREESAVED:
				{
					IsModified = false;
					propagate = true;
					break;
				}
			case NodeStatus.NS_EXPANDCHILDREN:
				{
					IsExpanded = true;
					break;
				}
			case NodeStatus.NS_EXPANDTREE:
				{
					IsExpanded = true;
					propagate = true;
					break;
				}
			case NodeStatus.NS_COLLAPSECHILDREN:
				{
					IsExpanded = false;
					break;
				}
			case NodeStatus.NS_COLLAPSETREE:
				{
					IsExpanded = false;
					propagate = true;
					break;
				}
			}

			if (propagate) notifyChildrenOfStatusChange(newStat);

		}

		public void TriStateReset()
		{
			TriState = CheckedState2.UNSET;
		}

		public void UpdateProperties()
		{
			OnPropertyChange(nameof(Item));
			OnPropertyChange(nameof(ChildrenView));
			OnPropertyChange(nameof(HasChildren));
			OnPropertyChange(nameof(ChildCount));
			OnPropertyChange(nameof(ExtChildCount));
			// OnPropertyChange(nameof(ExtMergeItemCount));
			// OnPropertyChange(nameof(ExtMergeItemCountCurrent));

			// OnPropertyChange("ExtItemCount");
		}

	#endregion

	#region private methods

		// private int ExtendedChildrenCount(TreeNode2<T> node)
		// {
		// 	if (node.children.Count == 0) return 0;
		//
		// 	int count = node.children.Count;
		//
		// 	foreach (TreeNode2<T> child in node.children)
		// 	{
		// 		if (child.children.Count > 0)
		// 		{
		// 			count += ExtendedChildrenCount(child);
		// 		}
		// 	}
		//
		// 	return count;
		// }

		private int ExtChildrenCount()
		{
			if (children.Count == 0) return 0;

			int count = children.Count;

			foreach (TreeNode2<T> child in children)
			{
				count += child.ExtChildCount;
			}

			return count;
		}

		// private void ExtMergeItemCount(TreeNode2<T> node)
		// {
		// 	// count my merge items
		// 	int count = node.item.MergeItems.Count;
		//
		// 	Debug.Write("  ".Repeat(tabDepth));
		// 	Debug.WriteLine("@ ext count start | " + count.ToString("##0") + "  " + item.Title );
		//
		// 	tabDepth++;
		//
		// 	// sum the count from each child recursevly
		// 	foreach (TreeNode2<T> child in node.children)
		// 	{
		// 		count += child.ExtMergeItemCount(child);
		// 	}
		//
		// 	tabDepth--;
		//
		// 	Debug.Write("  ".Repeat(tabDepth));
		// 	Debug.WriteLine("@ ext count end   | " + count.ToString("###") + "  " + item.Title);
		//
		//
		// 	ExtMergeItemCount = count;
		// }

		public void NotifyChildrenChange()
		{
			OnPropertyChange("Children");
			OnPropertyChange("ChildrenView");
			OnPropertyChange("ChildCount");
			OnPropertyChange("HasChildren");
			OnPropertyChange("ExtendedChildCount");
		}

		private void notifyParentOfStateChange(CheckedState2 newState, CheckedState2 oldState, bool useTriState)
		{
			parent?.StateChangeFromChild(newState, oldState, useTriState);
		}

		private void notifyChildrenOfStateChange(CheckedState2 newState, CheckedState2 oldState, bool useTriState)
		{
			if (ChildrenView == null) return;

			foreach (TreeNode2<T> node in ChildrenView)
			{
				node.StateChangeFromParent(newState, oldState, useTriState);
			}
		}

		private void notifyChildrenOfStatusChange(NodeStatus newStat)
		{
			if (ChildrenView == null) return;

			foreach (TreeNode2<T> node in ChildrenView)
			{
				node.StatusChangeFromParent(newStat);
			}
		}

		private void processStateChangeLeaf(CheckedState2 newState, CheckedState2 oldState, bool useTriState)
		{
			CheckedState2 = newState;

			notifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void processStateChangeBranch(CheckedState2 newState, CheckedState2 oldState, bool useTriState)
		{
			CheckedState2 = newState;

			notifyChildrenOfStateChange(newState, oldState, useTriState);
			notifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void processStateChange(CheckedState2 newState)
		{
			CheckedState2 oldState = checkedState2;

			// process when leaf
			if (NodeType2 == NodeType2.LEAF)
			{
				processStateChangeLeaf(newState, checkedState2, false);

				return;
			}

			// node type is branch
			if (triState == CheckedState2.UNSET)
			{
				// not currently tristate processing

				if (CheckedState2 == CheckedState2.CHECKED ||
					CheckedState2 == CheckedState2.UNCHECKED)
				{
					processStateChangeBranch(newState, oldState, false);
				}
				// state is mixed // start using tri-state
				else
				{
					triState = checkedState2;

					processStateChangeBranch(newState, oldState,  true);
				}
			}
			else
				// selectStateOriginal is set
				// processing a tri-state change
				// mixed to checked
				// checked to unchecked
				// unchecked to mixed

				// current is mixed -> change to checked
				// current is checked -> change to unchecked
				// current is unchecked -> change to mixed
			{
				CheckedState2 proposed =
					selectStateFromBool(BoolList[(int) oldState + 1]);

				CheckedState2 = proposed;

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

		private CheckedState2 selectStateFromBool(bool? test)
		{
			return (CheckedState2) indexInBoolList(test);
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event consuming

		// private void OnSavingAnnounce(object sender, object value)
		// {
		// 	// isSaving = true;
		// }

		// private void OnAnnouncedRemExCollapseState(object sender, object value)
		// {
		// 	
		// 	rememberExpCollapseState = (bool) (value ?? false);
		//
		// 	Debug.WriteLine("@TreeNode2<T> onAnnounced| remember?| " + rememberExpCollapseState.ToString());
		// }

		// private void OnAnnouncedTnInit(object sender, object value)
		// {
		// 	if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ treenode|@ onann-tninit| received");
		// 	isInitialized = true;
		// 	isModified = false;
		// }

		// private void OnAnnouncedSaved(object sender, object value)
		// {
		// 	if (Common.SHOW_DEBUG_MESSAGE1)
		// 		Debug.WriteLine("@     treenode|@ onann-saved| received| isinitialized| "
		// 			+ isInitialized + " | ismodified| " + IsModified + " | who| " + this.ToString());
		// 	isModified = false;
		// 	// isSaving = false;
		// }

		private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			IsModified = true;
		}

	#endregion

	#region system overrides

		// creates a partial clone - does not clone the children
		public object Clone()
		{
			TreeNode2<T> newNode = new TreeNode2<T>(parent, (T) item.Clone(), false);

			newNode.CheckedState2 = checkedState2;
			newNode.isExpanded = isExpanded;
			newNode.triState = triState;
			// newNode.isLocked = false;
			// newNode.isFixed = false;
			newNode.isContextSelected = false;

			newNode.depth = depth;

			return newNode;
		}

		public override string ToString()
		{
			return NodeType2 + "::" + item.Id + "::" + checkedState2;
		}

	#endregion
	}

#endregion

#region TriStateTree

	[DataContract(Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
	public class TriStateTree<T> : TreeNode2<T>
	where T : class, INodeItem2
	{
	#region private fields

		private TreeNode2<T> selectedNode;
		// private int extMergeItemCountCurrent;

	#endregion

	#region ctor

		public TriStateTree() : base(null, false)
		{
			Depth = 0;
		}

		public void Initalize()
		{
		#if DML1
			DM.Start0();
		#endif
			IsInitialized = true;

		#if DML1
			DM.Stat0("InitializeAllChildrenView");
		#endif
			InitializeAllChildrenView();

		#if DML1
			DM.End0();
		#endif
		}

	#endregion

	#region public properties

		public TreeNode2<T> SelectedNode
		{
			get => selectedNode;
			set
			{
				selectedNode = value;
				OnPropertyChange();
				OnPropertyChange("HasSelection");
			}
		}

		public bool HasSelection => SelectedNode != null;

	#endregion

	#region private properties

	#endregion

	#region public methods
		//
		// public int ExtMergeItemCountCurrent
		// {
		// 	get => extMergeItemCountCurrent;
		// 	set
		// 	{
		// 		if (value != -1) return;
		//
		// 		int count = item.MergeItemCount;
		// 		
		// 		foreach (TreeNode2<T> childNode in children)
		// 		{
		// 			childNode.ExtMergeItemCountCurrent = -1;
		// 			count += childNode.ExtMergeItemCountCurrent;
		// 		}
		//
		// 		extMergeItemCountCurrent = count;
		//
		// 		OnPropertyChange();
		// 	}
		// }

		public void RemoveNode2(TreeNode2<T> contextNode)
		{
			TreeNode2<T> parent = contextNode.Parent;
			parent.Children.Remove(contextNode);

			NotifyChildrenChange();
		}

		public void AddNewChild2(TreeNode2<T> contextNode)
		{
			TreeNode2<T> temp = TempTreeNode(contextNode);
			
			temp.IsNodeSelected = true;

			contextNode.Children.Add(temp);

			contextNode.NotifyChildrenChange();
		}

		public void AddChild2(TreeNode2<T> contextNode, TreeNode2<T> toAddNode)
		{
			toAddNode.Depth = ++contextNode.Depth;
			contextNode.Children.Add(toAddNode);

			NotifyChildrenChange();
		}

		// adds a new, temp node before the selected node
		public void AddNewBefore2(TreeNode2<T> contextNode)
		{
			TreeNode2<T> parent = contextNode.Parent;
			TreeNode2<T> temp = TempTreeNode(parent);
			temp.IsNodeSelected = true;
			AddAt2(parent, temp, parent.Children.IndexOf(contextNode));

			NotifyChildrenChange();
		}

		// adds without notifying of the revision
		public void AddBefore2(TreeNode2<T> contextNode, TreeNode2<T> toAddNode)
		{
			toAddNode.Depth = ++contextNode.Depth;
			TreeNode2<T> parent = contextNode.Parent;

			AddAt2(parent, toAddNode, parent.Children.IndexOf(contextNode));
		}

		// adds a new, temp node before the selected node
		public void AddNewAfter2(TreeNode2<T> contextNode)
		{
			TreeNode2<T> parent = contextNode.Parent;

			TreeNode2<T> temp = TempTreeNode(parent);

			temp.IsNodeSelected = true;

			AddAt2(parent, temp, parent.Children.IndexOf(contextNode) + 1);

			NotifyChildrenChange();
		}

		// adds without notifying of the revision
		public void AddAfter2(TreeNode2<T> contextNode, TreeNode2<T> toAddNode)
		{
			TreeNode2<T> parent = contextNode.Parent;
			toAddNode.Depth = parent.Depth + 1;

			AddAt2(parent, toAddNode, parent.Children.IndexOf(contextNode) + 1);
		}

		public void AddNode(TreeNode2<T> node)
		{
			node.Depth = node.Parent.Depth + 1;

			node.Parent.Children.Add(node);
		}

		/// <summary>
		/// move the existing node before the context selected node
		/// </summary>
		/// <param name="contextNode">the selected node</param>
		/// <param name="existingNode">the existing node being relocated</param>
		public void MoveBefore(TreeNode2<T> contextNode, TreeNode2<T> existingNode)
		{
			existingNode.Depth = contextNode.Depth;

			moveNode2(contextNode, existingNode, NodePlacement2.BEFORE);

			NotifyChildrenChange();
		}

		/// <summary>
		/// move the existing node after the context selected node
		/// </summary>
		/// <param name="contextNode">the selected node</param>
		/// <param name="existingNode">the existing node being relocated</param>
		public void MoveAfter(TreeNode2<T> contextNode, TreeNode2<T> existingNode)
		{
			existingNode.Depth = contextNode.Depth;

			moveNode2(contextNode, existingNode, NodePlacement2.AFTER);

			NotifyChildrenChange();
		}

		/// <summary>
		/// move the existing node to be a child of the selected node
		/// </summary>
		/// <param name="contextNode">the selected node</param>
		/// <param name="existingNode">the node being relocated</param>
		public void MoveAsChild(TreeNode2<T> contextNode, TreeNode2<T> existingNode)
		{
			moveAsChild2(contextNode, existingNode);

			NotifyChildrenChange();
		}

	#endregion

	#region private methods

		private void CannotAddChildError()
		{
			CommonTaskDialogs.CommonErrorDialog(
				"Cannot Add Sub-Category",
				"A sub-category cannot be added",
				"The maximum sheet classification depth has" + nl
				+ "been reached and a sub-category cannot be added"
				);
		}


		private void AddAt2(TreeNode2<T> parent, TreeNode2<T> toAddNode, int index)
		{
			parent.Children.Insert(index, toAddNode);
		}

		// parent is the parent of the new location
		private void moveNode2(TreeNode2<T> contextNode, TreeNode2<T> existingNode, NodePlacement2 how)
		{
			TreeNode2<T> parent = contextNode.Parent;

			int contextIdx = parent.Children.IndexOf(contextNode);

			contextIdx = how == NodePlacement2.AFTER ? ++contextIdx : contextIdx;

			if (existingNode.Parent.Equals(parent))
			{
				// simple move within the same children collection
				int existIdx = parent.Children.IndexOf(existingNode);

				parent.Children.Move(existIdx, contextIdx);
			}
			else
			{
				// complex move from one collection to another
				// this means, add then delete

				TreeNode2<T> exParent = existingNode.Parent;

				// update the parent of the existing node
				existingNode.Parent = contextNode.Parent;

				// add in the parent collection of he selected node
				AddAt2(parent, existingNode, contextIdx);

				// remove from the original collection
				exParent.Children.Remove(existingNode);
			}
		}

		// move to be the child of contextNode
		private void moveAsChild2(TreeNode2<T> contextNode, TreeNode2<T> existingNode)
		{
			// save the original parent for later use
			TreeNode2<T> exParent = existingNode.Parent;

			// update the parent of the existing node
			existingNode.Parent = contextNode;

			// add as first node
			AddAt2(contextNode, existingNode, 0);

			exParent.Children.Remove(existingNode);
		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return NodeType2 + ":: ** class BaseOfTree2 ** ::" + CheckedState2;
		}

	#endregion
	}

#endregion


}