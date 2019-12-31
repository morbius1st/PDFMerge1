#region + Using Directives

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using static Test3.MainWindow;
using static Test3.CheckedState;


#endregion


// projname: Test3
// itemname: EventTest5
// username: jeffs
// created:  12/21/2019 2:26:39 PM


namespace Test3
{


	public class TreeNode5 : INotifyPropertyChanged
	{
		public enum CheckedStatus
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

		private CheckedStatus checkState = CheckedStatus.UNCHECKED;
		private CheckedStatus selectTriState = CheckedStatus.UNSET;

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

		public CheckedStatus CheckState
		{
			get => checkState;
//			set
//			{
//				ProcessStateChange(value);
//				OnPropertyChange();
//			}
		}

		private CheckedStatus ChkState
		{
			set
			{
				if (value == CheckedStatus.UNSET)
				{
					checkState = value;
					return;
				}
				else if (value != checkState)
				{
					checkState = value;
					OnPropertyChange("SelectState");
					OnPropertyChange("Checked");

					parent?.AdjustChildCount(checkState);
				}
			}
		}

		public CheckedStatus TriState
		{
			get => selectTriState;
			set
			{
				if (value != selectTriState)
				{
					selectTriState = value;
					OnPropertyChange();
				}
			}
		}

		public bool? Checked
		{
			get
			{
				if (checkState == CheckedStatus.UNSET)
					return null;

				return _boolList[(int) checkState];
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

		public void AdjustChildCount(CheckedStatus childState)
		{
			if (childState == CheckedStatus.MIXED)
			{
				if (!MixedStateBeenTold)
				{
					MixedStateBeenTold = true;
					CheckedChildrenCount++;
					return;
				}
			}

			MixedStateBeenTold = false;

			if (childState == CheckedStatus.CHECKED)
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
			ChkState = CheckedStatus.UNCHECKED;
			TriState = CheckedStatus.UNSET;
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

		public void StateChangeFromParent(CheckedStatus newState, CheckedStatus oldState, bool useTristate)
		{
			if (useTristate)
			{
				if (selectTriState == CheckedStatus.UNSET)
				{
					// first time through - save current
					selectTriState = checkState;
				}

				if (newState == CheckedStatus.MIXED) newState = selectTriState;
			}
			else
			{
				selectTriState = CheckedStatus.UNSET;
			}

			ChkState = newState;

			NotifyChildrenOfStateChange(newState, oldState, useTristate);
		}

		public void StateChangeFromChild(CheckedStatus newState, CheckedStatus oldState, bool useTristate)
		{
			CheckedStatus priorState = checkState;
			CheckedStatus finalState = CheckedStatus.UNSET;


			if (!useTristate)
			{
				selectTriState = CheckedStatus.UNSET;
			}
			else if (selectTriState == CheckedStatus.UNSET)
			{
				// usetristate is true
				// starting tristate process - save the current state
				selectTriState = checkState;
			}

			switch (newState)
			{
			// newstate is checked - child is checked
			case CheckedStatus.CHECKED:
				if (checkedChildrenCount == (Children?.Count ?? 0))
				{
					finalState = CheckedStatus.CHECKED;
				}
				else if (checkState != CheckedStatus.MIXED)
				{
					finalState = CheckedStatus.MIXED;
				}

				break;
			// newstate is unchecked - child is unchecked
			case CheckedStatus.UNCHECKED:
				if (checkedChildrenCount == 0)
				{
					finalState = CheckedStatus.UNCHECKED;
				}
				else if (checkState == CheckedStatus.CHECKED)
				{
					finalState = CheckedStatus.MIXED;
				}

				break;
			// newstate is checked - child is mixed
			case CheckedStatus.MIXED:
				// must evaluate whether coming from checked to mixed or
				// from unchecked to mixed

				// i am checked, flip from checked to
				// mixed.  inform parent.
				if (checkState == CheckedStatus.CHECKED ||
					checkState == CheckedStatus.UNCHECKED)
				{
					finalState = CheckedStatus.MIXED;
				}


				break;
			}

			if (finalState != CheckedStatus.UNSET)
			{
				ChkState = finalState;
				NotifyParentOfStateChange(finalState, priorState, useTristate);
			}
		}

		public void TriStateReset()
		{
			TriState = CheckedStatus.UNSET;
		}

	#if DEBUG
		public string MakeName()
		{
			return "D| " + depth + " :: Id| " + id + " Pid| (" + parent?.id ?? "null" + ")";
		}
	#endif

	#endregion

	#region private methods

		private void NotifyParentOfStateChange(CheckedStatus newState, CheckedStatus oldState, bool useTriState)
		{
			parent?.StateChangeFromChild(newState, oldState, useTriState);
		}

		private void NotifyChildrenOfStateChange(CheckedStatus newState, CheckedStatus oldState, bool useTriState)
		{
			if (Children == null) return;

			foreach (TreeNode5 evt in Children)
			{
				evt.StateChangeFromParent(newState, oldState, useTriState);
			}
		}

		private void ProcessStateChangeLeaf(CheckedStatus newState, CheckedStatus oldState, bool useTriState)
		{
			ChkState = newState;

			NotifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void processStateChangeBranch(CheckedStatus newState, CheckedStatus oldState, bool useTriState)
		{
			ChkState = newState;

			NotifyChildrenOfStateChange(newState, oldState, useTriState);
			NotifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void ProcessStateChange(CheckedStatus newState)
		{
			CheckedStatus oldState = checkState;

			// process when leaf
			if (NodeType == NodeType.LEAF)
			{
				ProcessStateChangeLeaf(newState, checkState, false);

				return;
			}

			// node type is branch
			if (selectTriState == CheckedStatus.UNSET)
			{
				// not currently tristate processing

				if (checkState == CheckedStatus.CHECKED ||
					checkState == CheckedStatus.UNCHECKED)
				{
					processStateChangeBranch(newState, oldState, false);
				}
				// state is mixed // start using tri-state
				else
				{
					selectTriState = checkState;

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
				CheckedStatus proposed =
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

		private CheckedStatus SelectStateFromBool(bool? test)
		{
			return (CheckedStatus) indexInBoolList(test);
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
			return NodeType + "::" + name + "::" + checkState;
		}

	#endregion

	}


}