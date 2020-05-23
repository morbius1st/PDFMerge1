#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
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

	[DataContract(Namespace = "", IsReference = true)]
	public class TreeNode : INotifyPropertyChanged
	{
	#region private fields

		// properties
		private float key;
		private SheetCategory item;
		private ObservableCollection<TreeNode> children;
		private ListCollectionView childrenView;

		private TreeNode parent;
		private CheckedState checkedState = CheckedState.UNCHECKED;
		private CheckedState triState = CheckedState.UNSET;
		private bool isExpanded;
		private bool isSelected;
		private bool isContextSelected = false;

		private int checkedChildCount = 0;

		private int ID = -1;

		// fields

		//										mixed->checked->unchecked->mixed
		//										  0        1        2        3 (0)
		private static readonly bool?[] _boolList = new bool?[] {null,     true,    false,   null};
		private bool mixesStateBeenTold = false;



		// static
		private static int idx = 0;

	#endregion

	#region ctor

		public TreeNode(TreeNode parent, string key, SheetCategory item,
			bool isExpanded)
		{
			Children = new ObservableCollection<TreeNode>();
			this.parent = parent;
			this.item = item;
			this.key = -1.0f;
			this.isExpanded = isExpanded;

			ID = idx++;

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
			childrenView.CustomSort = new ChildrenSorter();

			
		}

		public TreeNode() : this(null, null, null, false) { }

		public TreeNode(TreeNode parent, SheetCategory item,
			bool isExpanded) : this(parent, "!!", item, isExpanded) { }

	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public float Key
		{
			get => key;

			private set
			{
				key = value;
				OnPropertyChange();
			}
		}

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
			private set => parent = value;
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

		public ICollectionView ChildrenView
		{
			get
			{
				return childrenView;
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



//		[IgnoreDataMember]
//		public bool IsModified
//		{
//			get => isModified;
//			set
//			{
//				isModified = value;
//				OnPropertyChange();
//			}
//		}


	#endregion

	#region private properties

	#endregion

	#region public methods

		public void ChildrenUpdated()
		{
			OnPropertyChange("ChildrenView");
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

		// for the below
		// i am the selected node - the basis of the adjustment

		// adds a child at the end of the list of children
		// for sample data only
		public bool AddNode(TreeNode newNode)
		{
//			float idx = 0.0f;
//
//			if (children.Count == 1)
//			{
//				idx = children[0].Key;
//			}
//			else if (children.Count > 1)
//			{
//				childrenView.MoveCurrentToLast();
//
//				idx = ((TreeNode) childrenView.CurrentItem).key;
//			}

//			addNode(newNode, idx, NodePlacement.AFTER);

			children.Add(newNode);

			return true;
		}

		public bool AddChild(TreeNode newNode)
		{
			return addNode(newNode, this.key, NodePlacement.AFTER);
		}

		public bool AddBefore(TreeNode newNode)
		{
			return parent.addNode(newNode, this.key, NodePlacement.BEFORE);
		}
		
		public bool AddAfter(TreeNode newNode)
		{
			return parent.addNode(newNode, this.key, NodePlacement.AFTER);
		}

		public bool MoveBefore(TreeNode existingNode, TreeNode selectedNode)
		{
			return moveNode(existingNode, selectedNode, NodePlacement.BEFORE);
		}
		
		public bool MoveAfter(TreeNode existingNode, TreeNode selectedNode)
		{
			return moveNode(existingNode, selectedNode, NodePlacement.AFTER);
		}

		public bool DeleteNode(TreeNode selectedNode)
		{
			TreeNode parent = selectedNode.Parent;

			string childCountDesc;

			int totalChildCount = selectedNode.ExtendedChildCount;

			if (totalChildCount > 0)
			{
				childCountDesc = "a total of " + totalChildCount + " categories "
					+ "and sub-categories.";
			}
			else
			{
				childCountDesc = "no categories or sub-categories.";
			}

			MessageBoxResult result = MessageBox.Show(
				"You are about to delete the category\n"
				+ selectedNode.Item.Title
				+ "\nwith " + childCountDesc
				+ "\nIs this correct?",
				"Classifier Editor", MessageBoxButton.YesNo,
				MessageBoxImage.Warning );

			if (result != MessageBoxResult.Yes) return false;

			return parent.RemoveChild(selectedNode);
		}

		public static TreeNode TempTreeNode(TreeNode parent)
		{
			return new TreeNode(parent, SheetCategory.TempSheetCategory(), false);
		}

		public void ResequenceChildNodes()
		{
			object selNode = childrenView.CurrentItem;

			childrenView.Refresh();

			childrenView.MoveCurrentTo(selNode);

			float idx = 1.0f;

			foreach (TreeNode child in childrenView)
			{
				child.Key = idx++;
			}
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

		private bool RemoveChild(TreeNode existingNode)
		{
			if (children.Contains(existingNode))
			{
				children.Remove(existingNode);
				NotifyChildrenChange();
				return true;
			}
			return false;
		}

		// case where existing and selected have different parents
		// need to add in the new location and
		// delete the old location
//		private bool moveNodeComplex(TreeNode existingNode, TreeNode selectedNode, NodePlacement where)
//		{
//			selectedNode.parent.addNodeQuite(existingNode, selectedNode.key, where);
//
//			TreeNode parent = existingNode.parent;
//
//			parent.RemoveChild(existingNode);
//
//			existingNode = null;
//
//			NotifyChildrenChange();
//
//			return true;
//		}

		
		// case where both existing and selected have the same parent - just revise the index
		// and re-sequence
		private bool moveNode(TreeNode existingNode, TreeNode selectedNode, NodePlacement where)
		{
			
			if (existingNode.parent.Equals(selectedNode.parent))
			{
				float idx = where == NodePlacement.AFTER ? 0.5f : -0.5f;

				existingNode.key = selectedNode.key + idx;

				selectedNode.parent.ResequenceChildNodes();
			}
			else
			{
				selectedNode.parent.addNodeQuite(existingNode, selectedNode.key, where);

				TreeNode parent = existingNode.parent;

				parent.RemoveChild(existingNode);
			}

			NotifyChildrenChange();

			return true;
		}

		private bool addNode(TreeNode newNode, float selectedIdx, NodePlacement where)
		{
			addNodeQuite(newNode, selectedIdx, where);

//			float offset = 0.5f; // place after;
//
//			if (where == NodePlacement.BEFORE) offset = -0.5f;
//
//			newNode.key = selectedIdx + offset;
//
//			children.Add(newNode);

			

			ResequenceChildNodes();

			NotifyChildrenChange();

			return true;
		}

		public void addNodeQuite(TreeNode newNode, float selectedIdx, NodePlacement where)
		{
			float offset = 0.5f; // place after;

			if (where == NodePlacement.BEFORE) offset = -0.5f;

			newNode.key = selectedIdx + offset;

			children.Add(newNode);
		}


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

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"[{ID:D3}] :: " +
				NodeType + "::" + item.Title + "::" + checkedState;
		}

	#endregion
	}

	public class ChildrenSorter : IComparer
	{
		public int Compare(object x, object y)
		{
			TreeNode a = (TreeNode) x;
			TreeNode b = (TreeNode) y;

			if (a == null || b == null) return 0;

			return a.Key.CompareTo(b.Key);
		}
	}
}