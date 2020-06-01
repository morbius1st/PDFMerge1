#region using directives

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Data;

#endregion

// username: jeffs
// created:  5/2/2020 9:28:15 AM

namespace ClassifierEditor.Tree
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
	public class TreeNode : INotifyPropertyChanged, ICloneable
	{
	#region private fields

		// properties
//		private float key;
		protected SheetCategory item;
		protected ObservableCollection<TreeNode> children;
		protected ListCollectionView childrenView;

		protected TreeNode parent;
		private int depth;


		protected CheckedState checkedState = CheckedState.UNCHECKED;
		protected CheckedState triState = CheckedState.UNSET;
		protected bool isExpanded;
		protected bool isSelected;
		protected bool isContextSelected = false;

		protected int checkedChildCount = 0;

		protected int uniqueId = -1;


		// fields

		//										mixed->checked->unchecked->mixed
		//										  0        1        2        3 (0)
		private static readonly bool?[] _boolList = new bool?[] {null,     true,    false,   null};
		private bool mixesStateBeenTold = false;


		// static
		protected static int masterUniqueId = 0;

	#endregion

	#region ctor

		public TreeNode(TreeNode parent, SheetCategory item, bool isExpanded)
		{
			
			Children = new ObservableCollection<TreeNode>();
			this.parent = parent;
			this.depth = parent.depth + 1;
			this.item = item;
			this.isExpanded = isExpanded;

			UniqueId = masterUniqueId++;

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		}

		public TreeNode() : this(null, null, false) { }

		protected  TreeNode(SheetCategory item, bool isExpanded)
		{
			Children = new ObservableCollection<TreeNode>();
			this.item = item;
			this.isExpanded = isExpanded;

			UniqueId = masterUniqueId++;

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;

		}

	#endregion

	#region public properties


		// the actual tree data item
		[DataMember(Order = 2)]
		public SheetCategory Item
		{
			get => item;

			set
			{
				item = value;
				OnPropertyChange();
				item.NotifyChange();
			}
		}

//		[DataMember(Order = 3)]
		public NodeType NodeType
		{
			get
			{
				if (ChildCount == 0) return NodeType.LEAF;

				return NodeType.BRANCH;
			}
		}

		[DataMember(Order = 4)]
		public TreeNode Parent
		{
			get => parent;
			set
			{
				parent = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 5)]
		public int Depth
		{
			get => depth;
			set => depth = value;
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
					return;
				}
				else if (value != checkedState)
				{
					checkedState = value;
					OnPropertyChange("Checked");

					parent?.UpdateChildCount(checkedState);
				}
			}
		}

		[DataMember(Order = 6)]
		public bool? Checked
		{
			get
			{
				if (checkedState == CheckedState.UNSET) return null;

				return _boolList[(int) checkedState];
			}

			set
			{
				ProcessStateChange(SelectStateFromBool(value));
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
				}
			}
		}

		[DataMember(Order = 10, Name = "SubCategories")]
		public ObservableCollection<TreeNode> Children
		{
			get => children;

			private set
			{
				children = value;
				NotifyChildrenChange();
			}
		}

		[IgnoreDataMember]
		public ICollectionView ChildrenView
		{
			get { return childrenView; }
		}

		[IgnoreDataMember]
		public int UniqueId
		{
			get => uniqueId;
			set
			{
				uniqueId = value;
				OnPropertyChange();
			}
		}
		
		public bool HasChildren => ChildCount > 0;

		public int ChildCount => Children?.Count ?? 0;

		public int ExtendedChildCount => ExtendedChildrenCount(this);

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

		[DataMember(Order = 8)]
		public bool IsExpanded
		{
			get => isExpanded;

			set
			{
				if (value != isExpanded)
				{
					isExpanded = value;
					OnPropertyChange();
				}
			}
		}

		[IgnoreDataMember]
		public bool CanExpand => ChildCount > 0;

		[IgnoreDataMember]
		public bool IsSelected
		{
			get => isSelected;
			set
			{
				isSelected = value;
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

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void InitializeAllChildrenView()
		{
			uniqueId = masterUniqueId++;

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
//			childrenView.CustomSort = new ChildrenSorter();

			foreach (TreeNode node in Children)
			{
				

				node.InitializeAllChildrenView();
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

		// adds a child at the end of the list of children
		// for sample data only
//		public void AddNode(TreeNode newNode)
//		{
//			children.Add(newNode);
//		}

		public static TreeNode TempTreeNode(TreeNode parent)
		{
			return new TreeNode(parent, SheetCategory.TempSheetCategory(), false);
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

			NotifyChildrenOfStateChange(newState, oldState, useTriState);
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
				NotifyParentOfStateChange(finalState, priorState, useTristate);
			}
		}

		public void TriStateReset()
		{
			TriState = CheckedState.UNSET;
		}

	#endregion

	#region private methods

		private int ExtendedChildrenCount(TreeNode node)
		{
			if (node.children.Count == 0) return 0;

			int count = node.children.Count;

			foreach (TreeNode child in node.children)
			{
				if (child.children.Count > 0)
				{
					count += ExtendedChildrenCount(child);
				}
			}

			return count;
		}

		public void NotifyChildrenChange()
		{

			OnPropertyChange("ChildrenView");
			OnPropertyChange("Children");
			OnPropertyChange("ChildCount");
			OnPropertyChange("HasChildren");
			OnPropertyChange("ExtendedChildCount");
		}

		private void NotifyParentOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			parent?.StateChangeFromChild(newState, oldState, useTriState);
		}

		private void NotifyChildrenOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			if (ChildrenView == null) return;

			foreach (TreeNode node in ChildrenView)
			{
				node.StateChangeFromParent(newState, oldState, useTriState);
			}
		}

		private void ProcessStateChangeLeaf(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			CheckedState = newState;

			NotifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void processStateChangeBranch(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			CheckedState = newState;

			NotifyChildrenOfStateChange(newState, oldState, useTriState);
			NotifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void ProcessStateChange(CheckedState newState)
		{
			CheckedState oldState = checkedState;

			// process when leaf
			if (NodeType == NodeType.LEAF)
			{
				ProcessStateChangeLeaf(newState, checkedState, false);

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
					SelectStateFromBool(_boolList[(int) oldState + 1]);

				CheckedState = proposed;

				NotifyParentOfStateChange(proposed, oldState, true);
				NotifyChildrenOfStateChange(proposed, oldState, true);
			}
		}

		private int indexInBoolList(bool? test)
		{
			for (var i = 0; i < _boolList.Length; i++)
			{
				if (_boolList[i] == test) return i;
			}

			return 0;
		}

		private CheckedState SelectStateFromBool(bool? test)
		{
			return (CheckedState) indexInBoolList(test);
		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides


		public object Clone()
		{
			TreeNode newNode = new TreeNode(parent, (SheetCategory) item.Clone(), false);

			newNode.checkedState = checkedState;
			newNode.triState = triState;

			return newNode;
		}

		public override string ToString()
		{
			return $"[{UniqueId:D3}] :: " +
				NodeType + "::" + item.Title + "::" + checkedState;
		}


	#endregion
	}

#endregion


//	public class ChildrenSorter : IComparer
//	{
//		public int Compare(object x, object y)
//		{
//			TreeNode a = (TreeNode) x;
//			TreeNode b = (TreeNode) y;
//
//			if (a == null || b == null) return 0;
//
//			return a.Key.CompareTo(b.Key);
//		}
//	}




#region BaseOfTree

	[DataContract(Namespace = "", IsReference = true)]
	public class BaseOfTree : TreeNode, INotifyPropertyChanged
	{
	#region private fields
		private TreeNode selectedNode;

	#endregion

	#region ctor

		public BaseOfTree() : base(new SheetCategory("BaseOfTree", null, null), false)
		{
			masterUniqueId = 0;
		}

		public void Initalize()
		{

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
			contextNode.Children.Add(TempTreeNode(contextNode));

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
			AddAt2(parent, TempTreeNode(parent), parent.Children.IndexOf(contextNode) + 1);

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
			return $"[{UniqueId:D3}] :: " +
				NodeType + ":: ** BaseOfTree ** ::" + CheckedState;
		}

	#endregion
	}
	

#endregion
	
}