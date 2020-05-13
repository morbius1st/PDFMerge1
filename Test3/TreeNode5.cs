#region + Using Directives

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using static Test3.MainWindow;


#endregion


// projname: Test3
// itemname: EventTest5
// username: jeffs
// created:  12/21/2019 2:26:39 PM


namespace Test3
{
	public class TreeNode5 : INotifyPropertyChanged
	{
		public enum CheckedState
		{
			UNSET = -1,
			MIXED = 0,
			CHECKED = 1,
			UNCHECKED = 2
		}

	#region fields

		//										mixed->checked->unchecked->mixed
		//										  0        1        2        3 (0)
		private readonly bool?[] _boolList = new bool?[] {null,     true,    false,   null};

		private string name;

		private int depth = 0;
		private int id = 0;
		private int checkedChildrenCount = 0;

		private bool isExpanded;
		private bool MixedStateBeenTold = false;

		private CheckedState checkedState = CheckedState.UNCHECKED;
		private CheckedState triState = CheckedState.UNSET;

		private TreeNode5 parent;

		public ObservableCollection<TreeNode5> Children { get; private set; } = null;

	#endregion

	#region ctor

		public TreeNode5() { }

		public void Configure(TreeNode5 parent,
			int depth, int id, bool isBranch, bool isExpanded
			)
		{
			this.parent = parent;
			this.depth = depth;
			this.id = id;

			IsExpanded = isExpanded;

			if (isBranch)
			{
				NodeType = NodeType.BRANCH;
				Children = new ObservableCollection<TreeNode5>();
			}
		}

	#endregion

	#region properties

		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChange();
			}
		}

		public NodeType NodeType { get; private set; } = NodeType.LEAF;

		public CheckedState CheckState
		{
			get => checkedState;
//			set
//			{
//				ProcessStateChange(value);
//				OnPropertyChange();
//			}
		}

		private CheckedState ChkState
		{
			set
			{
				if (value == CheckedState.UNSET)
				{
					checkedState = value;
					return;
				}
				else if (value != checkedState)
				{
					checkedState = value;
					OnPropertyChange("SelectState");
					OnPropertyChange("Checked");

					parent?.AdjustChildCount(checkedState);
				}
			}
		}

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

		public bool? Checked
		{
			get
			{
				if (checkedState == CheckedState.UNSET)
					return null;

				return _boolList[(int) checkedState];
			}
			set
			{
				ProcessStateChange(SelectStateFromBool(value));

				OnPropertyChange();
			}
		}

		public bool IsExpanded
		{
			get => isExpanded;
			set
			{
				if (isExpanded != value)
				{
					isExpanded = value;
					OnPropertyChange();
				}
			}
		}

		public int ChildCount => Children?.Count ?? 0;

		public int CheckedChildrenCount
		{
			get => checkedChildrenCount;

			set
			{
				if (value != checkedChildrenCount)
				{
					checkedChildrenCount = value;

					OnPropertyChange();
				}
			}
		}

		public TreeNode5 Parent => parent;

	#endregion

	#region public methods

		public void AdjustChildCount(CheckedState childState)
		{
			if (childState == CheckedState.MIXED)
			{
				if (!MixedStateBeenTold)
				{
					MixedStateBeenTold = true;
					CheckedChildrenCount++;
					return;
				}
			}

			MixedStateBeenTold = false;

			if (childState == CheckedState.CHECKED)
			{
				CheckedChildrenCount++;
				return;
			}

			CheckedChildrenCount--;
		}

		public void AddChild(TreeNode5 child)
		{
			Children.Add(child);
		}

		public void RemoveChild(TreeNode5 child)
		{
			Children.Remove(child);
		}

		public void Reset()
		{
			ChkState = CheckedState.UNCHECKED;
			TriState = CheckedState.UNSET;
		}

		// reset children then myself
		// this allows partial reset as this starts 
		// a branch down
		public void ResetTree()
		{
			if (NodeType == NodeType.BRANCH)
			{
				// reset children then myself
				foreach (TreeNode5 ev in Children)
				{
					ev.ResetTree();
				}
			}

			Reset();
		}

		public void StateChangeFromParent(CheckedState newState, CheckedState oldState, bool useTristate)
		{
			if (useTristate)
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

			ChkState = newState;

			NotifyChildrenOfStateChange(newState, oldState, useTristate);
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
				if (checkedChildrenCount == (Children?.Count ?? 0))
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
				if (checkedChildrenCount == 0)
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
				ChkState = finalState;
				NotifyParentOfStateChange(finalState, priorState, useTristate);
			}
		}

		public void TriStateReset()
		{
			TriState = CheckedState.UNSET;
		}

	#if DEBUG
		public string MakeName()
		{
			return "D| " + depth + " :: Id| " + id + " Pid| (" + parent?.id ?? "null" + ")";
		}
	#endif

	#endregion

	#region private methods

		private void NotifyParentOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			parent?.StateChangeFromChild(newState, oldState, useTriState);
		}

		private void NotifyChildrenOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			if (Children == null) return;

			foreach (TreeNode5 evt in Children)
			{
				evt.StateChangeFromParent(newState, oldState, useTriState);
			}
		}

		private void ProcessStateChangeLeaf(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			ChkState = newState;

			NotifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void processStateChangeBranch(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			ChkState = newState;

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

				ChkState = proposed;

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

	#region debug help

	#if DEBUG

		public void AppendLine(string msg)
		{
			Instance.AppendLineTbk1(msg);
		}

		public void AppendMessage(string msg)
		{
			Instance.AppendMessageTbk1(msg);
		}

	#endif

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return NodeType + "::" + name + "::" + checkedState;
		}

	#endregion

	}


}