#region using directives

using System;
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

#endregion

// username: jeffs
// created:  5/2/2020 9:28:15 AM

namespace ClassifierEditor.NumberComponent
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

	[DataContract(Namespace = "", IsReference = true)]
	public class TreeNode : INotifyPropertyChanged
	{
	#region private fields

		// properties
		private string key;
		private NumberComponentItem item;
		private ObservableCollection<TreeNode> children;

		private TreeNode parent;
		private CheckedState checkedState = CheckedState.UNCHECKED;
		private CheckedState triState = CheckedState.UNSET;
		private bool isExpanded;
		private int checkedChildCount = 0;

		// fields

		//										mixed->checked->unchecked->mixed
		//										  0        1        2        3 (0)
		private static readonly bool?[] _boolList = new bool?[] {null,     true,    false,   null};
		private int depth = 0;
		private bool mixesStateBeenTold = false;


	#endregion

	#region ctor

		public TreeNode()
		{
			Children = new ObservableCollection<TreeNode>();

		}

		public void Initialize(TreeNode parent, NumberComponentItem item,
			int depth, bool isBranch, bool isExpanded)
		{
			Parent = parent;
			Item = item;
			Key = item.KeyCode;
			IsExpanded = isExpanded;

			this.depth = depth;

			if (isBranch)
			{
				NodeType = NodeType.BRANCH;
			}
		}

	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public string Key
		{
			get => key;

			set
			{
				key = value;
				OnPropertyChange();
			}
		}

		// the actual tree data item
		[DataMember(Order = 2)]
		public NumberComponentItem Item
		{
			get => item;

			set
			{
				item = value;
				OnPropertyChange();
				item.NotifyChange();
			}
		}

		[DataMember(Order = 3)]
		public NodeType NodeType { get; private set; } = NodeType.LEAF;


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

		[DataMember(Order = 10)]
		public ObservableCollection<TreeNode> Children
		{
			get => children;

			private set
			{
				children = value;
				NotifyChildrenChange();
			}

			}

		public bool HasChildren => ChildCount > 0;

		public int ChildCount => Children?.Count ?? 0;

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


	#endregion

	#region private properties

	#endregion

	#region public methods

		public void ChildrenUpdated()
		{
			OnPropertyChange("Children");
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

		public void AddChild(TreeNode child)
		{
			Children.Add(child);
			NotifyChildrenChange();
		}

		public void RemoveChild(TreeNode child)
		{
			Children.Remove(child);
			NotifyChildrenChange();
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
				foreach (TreeNode node in Children )
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

		private void NotifyChildrenChange()
		{
			OnPropertyChange("Children");
			OnPropertyChange("ChildCount");
			OnPropertyChange("HasChildren");
		}

		private void NotifyParentOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			parent?.StateChangeFromChild(newState, oldState, useTriState);
		}

		private void NotifyChildrenOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			if (Children == null) return;

			foreach (TreeNode node in Children)
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
			return NodeType + "::" + item.Title + "::" + checkedState;
		}

	#endregion
	}
}