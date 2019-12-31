#region + Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



#endregion


// projname: Test3
// itemname: Branch
// username: jeffs
// created:  12/23/2019 11:16:06 PM


namespace Test3
{
	public class Branch : ATreeNode
	{
	#region fields

		private int checkedChildrenCount;

	#endregion

	#region properties

		public ObservableCollection<ATreeNode> Children { get; private set; }

		public int ChildCount => Children.Count;

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

		public override bool IsBranch => true;
		public override bool IsLeaf => false;
		
	#endregion

	#region public methods

		public void AdjustChildCount(CheckedState childState)
		{
			if (childState.IsMixed)
			{
				if (!informedOfMixedState)
				{
					informedOfMixedState = true;
					CheckedChildrenCount++;
					return;
				}
			}

			informedOfMixedState = false;

			if (childState.IsChecked)
			{
				CheckedChildrenCount++;
				return;
			}

			CheckedChildrenCount--;
		}

		public void AddChild(ATreeNode child)
		{
			Children.Add(child);
		}

		public void ResetTree()
		{
			// reset children, then myself
			if (NodeType == NodeType.BRANCH)
			{
				// reset children then myself
				foreach (ATreeNode node in Children)
				{
					if (node.IsBranch)

						((Branch) node).ResetTree();
				}
			}

			Reset();
		}

		public override void StateChangeFromParent(CheckedState newState, CheckedState oldState, bool useTristate)
		{
			if (useTristate)
			{
				if (triState.IsUnset)
				{
					// first time through - save current
					triState = checkedState;
				}

				if (newState.IsMixed) newState = triState;
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

			switch (newState.State)
			{
			// newstate is checked - child is checked
			case CheckedState.Checked.CHECKED:
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
			case CheckedState.Checked.UNCHECKED:
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
			case CheckedState.Checked.MIXED:
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

	#if DEBUG
		public override string MakeName()
		{
			return "D| " + depth + " :: Id| " + id + " Pid| (" + parent?.Id ?? "null" + ")";
		}
	#endif

	#endregion

	#region protected methods

		protected override void ConfigureNodeType()
		{
			NodeType = NodeType.BRANCH;
			Children = new ObservableCollection<ATreeNode>();
		}

		protected override void NotifyParentOfStateChange(CheckedState newState, 
			CheckedState oldState, bool useTriState
			)
		{
//			parent?.StateChangeFromChild(newState, oldState, useTriState);
		}

		protected override void ProcessStateChange(CheckedState newState) { }

	#endregion

	#region private methods

		private void NotifyChildrenOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState) { }

		private void ProcessStateChangeLeaf(CheckedState newState, CheckedState oldState, bool useTriState) { }

		private void processStateChangeBranch(CheckedState newState, CheckedState oldState, bool useTriState) { }


	#endregion



		
	}
}
