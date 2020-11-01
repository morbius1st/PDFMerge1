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
using UtilityLibrary;

#endregion

// username: jeffs
// created:  5/2/2020 9:28:15 AM

namespace AndyShared.ClassificationDataSupport.TreeSupport
{
	public enum CheckedState
	{
		UNSET = -1,
		MIXED = 0,
		CHECKED = 1,
		UNCHECKED = 2
	}

	public enum NodeType
	{
		BRANCH,
		LEAF
	}

	public enum SelectState
	{
		UNSET = -1,
		UNCHECKED = 0,
		CHECKED = 1,
		MIXED = 2
	}

	public enum SelectMode
	{
		TWO_STATE = 2,
		TRI_STATE = 3
	}

	public enum NodePlacement
	{
		BEFORE = -1,
		AFTER = 1
	}

#region TreeNode

	[DataContract(Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	public class TreeNode : INotifyPropertyChanged, ICloneable
	{
	#region private fields

		// properties
		private SheetCategory item;

		/// <summary>
		/// the list of child TreeNodes
		/// </summary>
		private ObservableCollection<TreeNode> children = new ObservableCollection<TreeNode>();

		private ListCollectionView childrenView;

		private TreeNode parent;
		private int depth;
		private static int maxDepth = 7;

		private int extItemCountLast;

		private CheckedState checkedState = CheckedState.UNCHECKED;
		private CheckedState triState = CheckedState.UNSET;

		private bool isInitialized;

		private bool isModified;
		private bool isExpanded;
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

		private Orator.ConfRoom.Announcer onModifiedAnnouncer;

	#endregion

	#region ctor

		public TreeNode(TreeNode parent, SheetCategory item, bool isExpanded)
		{
			this.parent = parent;
			this.item = item;
			Depth = parent.depth + 1;
			this.isExpanded = isExpanded;

			// Children = new ObservableCollection<TreeNode>();
			OnCreated();

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		}

		public TreeNode() : this(null, null, false) { }

		protected  TreeNode(SheetCategory item, bool isExpanded)
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
			Orator.Listen(OratorRooms.TN_INIT, OnAnnounceTnInit);

			// listen to parent, changes have been saved
			Orator.Listen(OratorRooms.SAVED, OnAnnounceSaved);

			// listen to parent, changes have been saved
			Orator.Listen(OratorRooms.SAVING, OnSavingAnnounce);

			// listen to parent, changes have been saved
			Orator.Listen(OratorRooms.TN_REM_EXCOLLAPSE_STATE, OnAnnounceRemExCollapseState);


			onModifiedAnnouncer = Orator.GetAnnouncer(this, OratorRooms.MODIFIED);

			IsInitialized = true;
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext c)
		{
			Children = new ObservableCollection<TreeNode>();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext c)
		{
			OnCreated();

			if (!rememberExpCollapseState) isExpanded = false;
		}

		// [OnSerializing]
		// private void Onerializing(StreamingContext c)
		// {
		// 	if (!rememberExpCollapseState) isExpanded = false;
		// }

	#endregion

	#region public properties

		// the actual tree data item
		[DataMember(Order = 20)]
		public SheetCategory Item
		{
			get => item;
			set
			{
				item = value;

				if (item == null) return;

				item.Depth = depth;

				OnPropertyChange();
				item.UpdateProperties();
			}
		}

		[IgnoreDataMember]
		public NodeType NodeType
		{
			get
			{
				if (ChildCount == 0) return NodeType.LEAF;

				return NodeType.BRANCH;
			}
		}

		[DataMember(Order = 2)]
		public TreeNode Parent
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
		public CheckedState CheckedState
		{
			get => checkedState;

			private set
			{
				if (value == CheckedState.UNSET)
				{
					checkedState = value;
					IsModified = true;
				}
				else if (value != checkedState)
				{
					checkedState = value;
					OnPropertyChange("Checked");

					parent?.UpdateChildCount(checkedState);

					IsModified = true;
				}
			}
		}

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
				OnPropertyChange();
			}
		}

		[DataMember(Order = 7)]
		public CheckedState TriState
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
		public ObservableCollection<TreeNode> Children
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

		[IgnoreDataMember]
		// public ICollectionView ChildrenView
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

	#region item properties

		[IgnoreDataMember]
		public int ItemCount => item.Count;

		[IgnoreDataMember]
		public int ExtItemCountLast
		{
			get => extItemCountLast;
			set
			{
				extItemCountLast = value;
				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public int ExtItemCountCurrent
		{
			get
			{
				int count = item.MergeItemCount;

				foreach (TreeNode childNode in children)
				{
					count += childNode.ExtItemCountCurrent;
				}

				return count;
			}

		}



	#endregion

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

				if (rememberExpCollapseState) IsModified = true;
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

				Debug.WriteLine("@ is node selected| " + item.Title);


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

				if (isInitialized)
				{
					onModifiedAnnouncer.Announce(null);
				}
			}
		}

		[IgnoreDataMember]
		public bool IsMaxDepth => depth >= maxDepth;

	#endregion

	#endregion

	#region private properties

	#endregion

	#region public methods

		public int CountExtItems()
		{
			int count = item.MergeItemCount;

			foreach (TreeNode childNode in children)
			{
				count += childNode.CountExtItems();
			}

			ExtItemCountLast = count;

			return count;
		}

		public void InitializeAllChildrenView()
		{
			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;

			foreach (TreeNode node in Children)
			{
				node.InitializeAllChildrenView();
				node.IsInitialized = true;
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

		public static TreeNode TempTreeNode(TreeNode parent)
		{
			TreeNode temp = new TreeNode(parent, SheetCategory.TempSheetCategory(), false);

			return temp;
		}

		public void ResetNode()
		{
			CheckedState = CheckedState.UNCHECKED;
			TriState = CheckedState.UNSET;
		}

		public void ResetTree()
		{
			if (NodeType == NodeType.BRANCH)
			{
				// reset children then myself
				foreach (TreeNode node in ChildrenView )
				{
					node.ResetTree();
				}
			}

			ResetNode();
		}

		public void StateChangeFromParent(CheckedState newState,
			CheckedState oldState, bool useTriState)
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

		public void TriStateReset()
		{
			TriState = CheckedState.UNSET;
		}

		public void UpdateProperties()
		{
			OnPropertyChange("Item");
			OnPropertyChange("ChildrenView");
			OnPropertyChange("HasChildren");
			OnPropertyChange("ChildCount");
			OnPropertyChange("ExtChildCount");
			OnPropertyChange("ItemCount");
			OnPropertyChange("ExtItemCount");
		}

	#endregion

	#region private methods

		// private int ExtendedChildrenCount(TreeNode node)
		// {
		// 	if (node.children.Count == 0) return 0;
		//
		// 	int count = node.children.Count;
		//
		// 	foreach (TreeNode child in node.children)
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

			foreach (TreeNode child in children)
			{
				count += child.ExtChildCount;
			}

			return count;
		}

		// private void ExtMergeItemCount(TreeNode node)
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
		// 	foreach (TreeNode child in node.children)
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

		private void notifyParentOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			parent?.StateChangeFromChild(newState, oldState, useTriState);
		}

		private void notifyChildrenOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			if (ChildrenView == null) return;

			foreach (TreeNode node in ChildrenView)
			{
				node.StateChangeFromParent(newState, oldState, useTriState);
			}
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
			if (NodeType == NodeType.LEAF)
			{
				processStateChangeLeaf(newState, checkedState, false);

				return;
			}

			// node type is branch
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
				// processing a tri-state change
				// mixed to checked
				// checked to unchecked
				// unchecked to mixed

				// current is mixed -> change to checked
				// current is checked -> change to unchecked
				// current is unchecked -> change to mixed
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

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event consuming

		private void OnSavingAnnounce(object sender, object value)
		{
			// isSaving = true;
		}

		private void OnAnnounceRemExCollapseState(object sender, object value)
		{
			rememberExpCollapseState = (bool) (value ?? false);
		}

		private void OnAnnounceTnInit(object sender, object value)
		{
			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ treenode|@ onann-tninit| received");
			isInitialized = true;
			isModified = false;
		}

		private void OnAnnounceSaved(object sender, object value)
		{
			if (Common.SHOW_DEBUG_MESSAGE1)
				Debug.WriteLine("@     treenode|@ onann-saved| received| isinitialized| "
					+ isInitialized + " | ismodified| " + IsModified + " | who| " + this.ToString());
			isModified = false;
			// isSaving = false;
		}

		private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			IsModified = true;
		}

	#endregion

	#region system overrides

		// creates a partial clone - does not clone the children
		public object Clone()
		{
			TreeNode newNode = new TreeNode(parent, (SheetCategory) item.Clone(), false);

			newNode.checkedState = checkedState;
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
			return NodeType + "::" + item.Title + "::" + checkedState;
		}

	#endregion
	}

#endregion


#region BaseOfTree

	[DataContract(Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
	public class BaseOfTree : TreeNode
	{
	#region private fields

		private TreeNode selectedNode;

	#endregion

	#region ctor

		public BaseOfTree() : base(null, false)
		{
			Depth = 0;
		}

		public void Initalize()
		{
			IsInitialized = true;
			InitializeAllChildrenView();
		}

	#endregion

	#region public properties

		public TreeNode SelectedNode
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

		public void RemoveNode2(TreeNode contextNode)
		{
			TreeNode parent = contextNode.Parent;
			parent.Children.Remove(contextNode);

			NotifyChildrenChange();
		}

		public void AddNewChild2(TreeNode contextNode)
		{
			TreeNode temp = TempTreeNode(contextNode);

			temp.IsNodeSelected = true;

			contextNode.Children.Add(temp);

			contextNode.NotifyChildrenChange();
		}

		public void AddChild2(TreeNode contextNode, TreeNode toAddNode)
		{
			toAddNode.Depth = ++contextNode.Depth;
			contextNode.Children.Add(toAddNode);

			NotifyChildrenChange();
		}

		// adds a new, temp node before the selected node
		public void AddNewBefore2(TreeNode contextNode)
		{
			TreeNode parent = contextNode.Parent;
			AddAt2(parent, TempTreeNode(parent), parent.Children.IndexOf(contextNode));

			NotifyChildrenChange();
		}

		// adds without notifying of the revision
		public void AddBefore2(TreeNode contextNode, TreeNode toAddNode)
		{
			toAddNode.Depth = ++contextNode.Depth;
			TreeNode parent = contextNode.Parent;
			AddAt2(parent, toAddNode, parent.Children.IndexOf(contextNode));
		}

		// adds a new, temp node before the selected node
		public void AddNewAfter2(TreeNode contextNode)
		{
			TreeNode parent = contextNode.Parent;

			TreeNode temp = TempTreeNode(parent);

			AddAt2(parent, temp, parent.Children.IndexOf(contextNode) + 1);

			NotifyChildrenChange();
		}

		// adds without notifying of the revision
		public void AddAfter2(TreeNode contextNode, TreeNode toAddNode)
		{
			TreeNode parent = contextNode.Parent;
			toAddNode.Depth = parent.Depth + 1;
			AddAt2(parent, toAddNode, parent.Children.IndexOf(contextNode) + 1);
		}

		public void AddNode(TreeNode node)
		{
			node.Depth = node.Parent.Depth + 1;

			node.Parent.Children.Add(node);
		}

		/// <summary>
		/// move the existing node before the context selected node
		/// </summary>
		/// <param name="contextNode">the selected node</param>
		/// <param name="existingNode">the existing node being relocated</param>
		public void MoveBefore(TreeNode contextNode, TreeNode existingNode)
		{
			existingNode.Depth = contextNode.Depth;

			moveNode2(contextNode, existingNode, NodePlacement.BEFORE);

			NotifyChildrenChange();
		}

		/// <summary>
		/// move the existing node after the context selected node
		/// </summary>
		/// <param name="contextNode">the selected node</param>
		/// <param name="existingNode">the existing node being relocated</param>
		public void MoveAfter(TreeNode contextNode, TreeNode existingNode)
		{
			existingNode.Depth = contextNode.Depth;

			moveNode2(contextNode, existingNode, NodePlacement.AFTER);

			NotifyChildrenChange();
		}

		/// <summary>
		/// move the existing node to be a child of the selected node
		/// </summary>
		/// <param name="contextNode">the selected node</param>
		/// <param name="existingNode">the node being relocated</param>
		public void MoveAsChild(TreeNode contextNode, TreeNode existingNode)
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


		private void AddAt2(TreeNode parent, TreeNode toAddNode, int index)
		{
			parent.Children.Insert(index, toAddNode);
		}

		// parent is the parent of the new location
		private void moveNode2(TreeNode contextNode, TreeNode existingNode, NodePlacement how)
		{
			TreeNode parent = contextNode.Parent;

			int contextIdx = parent.Children.IndexOf(contextNode);

			contextIdx = how == NodePlacement.AFTER ? ++contextIdx : contextIdx;

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

				TreeNode exParent = existingNode.Parent;

				// update the parent of the existing node
				existingNode.Parent = contextNode.Parent;

				// add in the parent collection of he selected node
				AddAt2(parent, existingNode, contextIdx);

				// remove from the original collection
				exParent.Children.Remove(existingNode);
			}
		}

		// move to be the child of contextNode
		private void moveAsChild2(TreeNode contextNode, TreeNode existingNode)
		{
			// save the original parent for later use
			TreeNode exParent = existingNode.Parent;

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
			return NodeType + ":: ** class BaseOfTree ** ::" + CheckedState;
		}

	#endregion
	}

#endregion
}