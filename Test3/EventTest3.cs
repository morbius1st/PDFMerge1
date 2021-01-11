#region + Using Directives

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static Test3.MainWindow;

#endregion


// projname: Test3
// itemname: EventTest2
// username: jeffs
// created:  12/11/2019 8:42:22 PM


// need to deal with two event systems
// event system for state changes
// one is from this to child:   OnStateChangeNotifyChild(state, useTriState)
// one is from this to parent:  OnStateChangeNotifyParent(state)
// states: unchecked, checked, undetermined, complete (reset tristate)
// this means that all events starts here


namespace Test3
{
	public class EventTest3 : Evt<EventTest3>, INotifyPropertyChanged
	{
	#region fields

		
#pragma warning disable CS0108 // 'EventTest3.nodeType' hides inherited member 'Evt<EventTest3>.nodeType'. Use the new keyword if hiding was intended.
		private NodeType nodeType = NodeType.LEAF;
#pragma warning restore CS0108 // 'EventTest3.nodeType' hides inherited member 'Evt<EventTest3>.nodeType'. Use the new keyword if hiding was intended.
		private SelectState selectTriStateSaved = SelectState.UNSET;

		private int depth = 0;
		private int parentBranchId = 0;
		private int branchId = 0;
		private int leafId = 0;
		private int uniqueId = 0;

#pragma warning disable CS0169 // The field 'EventTest3._isExpanded' is never used
		private bool _isExpanded;
#pragma warning restore CS0169 // The field 'EventTest3._isExpanded' is never used

		private bool? checkedStatus = false;
		private int childCheckCount = 0;

	#endregion

	#region ctor

		public EventTest3() { }

		public EventTest3(int depth, int parentBranchId, int branchId, int LeafId, int uniqueId,
			bool asBranch = false
			)
		{
			this.depth = depth;
			this.parentBranchId = parentBranchId;
			this.branchId = branchId;
			this.leafId = LeafId;
			this.uniqueId = uniqueId;


			if (asBranch)
			{
				nodeType = NodeType.BRANCH;
				childList = new ObservableCollection<EventTest3>();
			}

			this.name = MakeName();
		}

		public override void Configure(int depth, int parentBranchId, int branchId, int LeafId, int uniqueId,
			bool asBranch = false
			)
		{
			this.depth = depth;
			this.parentBranchId = parentBranchId;
			this.branchId = branchId;
			this.leafId = LeafId;
			this.uniqueId = uniqueId;

			if (asBranch)
			{
				nodeType = NodeType.BRANCH;
				childList = new ObservableCollection<EventTest3>();
			}

			this.name = MakeName();
		}

		private string MakeName()
		{
			if (nodeType == NodeType.LEAF)
			{
				return $"LfId# {leafId,-2}:ParentBr# {parentBranchId,-2}:Depth {depth,-2}:Unique {uniqueId}";
			}

			return $"BrId# {branchId,-2}:ParentBr# {parentBranchId,-2}:Depth {depth,-2}:Unique {uniqueId}";

		}

	#endregion

	#region properties

		public string Name {
			get  => name;

			set => name = value;
		}

		public int CheckedCount {
			get => childCheckCount;
			private set
			{
				childCheckCount = value;
				OnPropertyChange();
			}
		}

		public new NodeType NodeType
		{
			get => nodeType;
			set
			{
				nodeType = value;

				OnPropertyChange();
			}
		}

		public SelectState SelectStateOriginal
		{
			get => selectTriStateSaved;

			set
			{
				selectTriStateSaved = value;
				OnPropertyChange();
			}
		}

		// convenience property - allows changing the 
		// selectstate and sending a notification without
		// running the processing
		private SelectState SelState
		{
			set
			{
				if (selectState == value) return;

				selectState = value;
				OnPropertyChange("SelectState");
				OnPropertyChange("CheckedStatus");
			}
		}

		// the property associated with the current 
		// selected state of this item
		public SelectState SelectState
		{
			get => selectState;
			set
			{
				processStateChange(value); 
				OnPropertyChange();
			}
		}

		// the property attached to the checkbox
		public bool? CheckedStatus
		{
			get
			{
				if (selectState == SelectState.CHECKED)
				{
					checkedStatus = true;

				} else if (selectState == SelectState.UNCHECKED)
				{
					checkedStatus = false;
				}
				else
				{

					checkedStatus = null;
				}

				return checkedStatus;
			}

			set
			{
				checkedStatus = value;
				OnPropertyChange();

				if (value == true)
				{
					processStateChange(SelectState.CHECKED);
				}
				else if (value == false)
				{
					processStateChange(SelectState.UNCHECKED);
				}
				else
				{
					processStateChange(SelectState.MIXED);
				}
			}

		}

	#endregion

		public override void Reset()
		{
			childCheckCount = 0;
			selectState = SelectState.UNCHECKED;
			selectTriStateSaved = SelectState.UNSET;

			OnPropertyChange("CheckedCount");
			OnPropertyChange("CheckedStatus");
			OnPropertyChange("SelectState");
			OnPropertyChange("SelectStateOriginal");

		}

		public void TriStateReset()
		{
			SelectStateOriginal = SelectState.UNSET;
		}

		private void processStateChange(SelectState newValue)
		{
			// for leaf, just notify parent
			if (nodeType == NodeType.LEAF)
			{
				SelState = newValue;

				StateChangeMessage(newValue);

				NotifyParentOfStateChange(selectState, SelectState.UNSET, false);

				return;
			}

			// node type is branch

//			if (selectTriStateSaved == SelectState.UNSET)
//			{

			if (selectState == SelectState.CHECKED ||
				selectState == SelectState.UNCHECKED)
			{
				SelState = newValue;
				StateChangeMessage(newValue);

				NotifyChildrenOfStateChange(selectState, false);
				NotifyParentOfStateChange(selectState, SelectState.UNSET, false);

				// children are all being unchecked
				// reset checked count to 0
				if (selectState == SelectState.UNCHECKED)
				{
					CheckedCount = 0;
				}
				else
				{
					CheckedCount = childList.Count;
				}

				return;
			} // state is mixed // starting tri-state
			else
			{
				selectTriStateSaved = selectState;

				SelState = newValue;

				StateChangeMessage(newValue);

				NotifyChildrenOfStateChange(selectState, true);
				NotifyParentOfStateChange(selectState, SelectState.UNSET, false);

				if (selectState == SelectState.UNCHECKED)
				{
					CheckedCount = 0;
				}
				else
				{
					CheckedCount = childList.Count;
				}

				return;
			}
//			}
//
//			// selectStateOriginal is set
//			// processing a tri-state change
//
//			switch (selectState)
//			{
//			case SelectState.MIXED:
//				{
//					SelState = newValue;
//					StateChangeMessage(SelectState.CHECKED);
//
//					NotifyChildrenOfStateChange(selectState, true);
//					NotifyParentOfStateChange(selectState);
//
//					return;
//				}
//
//			case SelectState.CHECKED:
//				{
//					SelState = newValue;
//					StateChangeMessage(SelectState.UNCHECKED);
//
//					NotifyChildrenOfStateChange(selectState, true);
//					NotifyParentOfStateChange(selectState);
//					return;
//				}
//
//			case SelectState.UNCHECKED:
//				{
//					SelState = newValue;
//					StateChangeMessage(SelectState.MIXED);
//
//					NotifyChildrenOfStateChange(selectState, true);
//					NotifyParentOfStateChange(selectState);
//					return;
//				}
//			}
		}

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		// statechangefrom parent
		// update based on parent
		// pass the state change to children - no additional processing
		public override void StateChangeFromParent(SelectState toThis, bool useTriState)
		{
			SelState = toThis;

			if (nodeType == NodeType.BRANCH)
			{
				if (toThis == SelectState.CHECKED)
				{
					CheckedCount = childList.Count;
				}
				else if (toThis == SelectState.UNCHECKED)
				{
					CheckedCount = 0;
				}
			}


			// pass the state change to children
			NotifyChildrenOfStateChange(toThis, useTriState);

			StateChangeMessage("From Parent");
		}

		// statechangefrom child
		// only goes to a branch
		// need to keep count of number of checked versus unchecked
		// children - but only children
		// checked and mixed count as one but only once

		// from child: child has been checked
//		//  +-> if currently not mixed -> childcheckcount +1   // why??
		//  +-> reached max count -> count = list.count
		//  |  +-> yes -> state to checked -> notify parent
		//  +-> no -> state to mixed - notify parent

		// from child: child has been unchecked
		//  +-> childcheckcount -1
		//  +-> reached 0 count -> count = 0
		//  |  +-> yes -> state to unchecked -> notify parent
		//  +-> no -> state to mixed - notify parent

		// from child: child is mixed
		//  +-> childcheckcount +1
		//  +-> reached 0 count -> count = 0
		//  |  +-> yes -> state to unchecked -> notify parent
		//  +-> no -> state to mixed - notify parent
		public override void StateChangeFromChild(SelectState newState, SelectState fromState, bool useTriState)
		{
			// evaluate the new state and pass along as needed
			SelectState priorState = selectState;

			switch (newState)
			{
			case SelectState.CHECKED:
				{
					if (fromState == SelectState.UNCHECKED || fromState == SelectState.UNSET)
					{
						CheckedCount++;
					}

					if (childCheckCount == childList.Count)
					{
						SelState = SelectState.CHECKED;
						NotifyParentOfStateChange(SelectState.CHECKED, priorState, useTriState);
					}
					else
					{
						// only inform parent of mixed once
						if (selectState != SelectState.MIXED)
						{
							SelState = SelectState.MIXED;
							NotifyParentOfStateChange(SelectState.MIXED, priorState, useTriState);
						}
					}

					break;
				}
			case SelectState.UNCHECKED:
				{
					if (fromState == SelectState.CHECKED || fromState == SelectState.MIXED
						|| fromState == SelectState.UNSET)
					{
						CheckedCount--;
					}

					if (childCheckCount <= 0)
					{
						childCheckCount = 0;
						SelState = SelectState.UNCHECKED;
						NotifyParentOfStateChange(SelectState.UNCHECKED, priorState, useTriState);
					}
					else
					{
						if (selectState == SelectState.CHECKED)
						{
							SelState = SelectState.MIXED;
							NotifyParentOfStateChange(SelectState.MIXED, priorState, useTriState);
						}
					}

					break;
				}
			case SelectState.MIXED:
				{
					// must evaluate whether coming from checked to mixed or
					// from unchecked to mixed

					// i am checked, flip from checked to
					// mixed.  inform parent.
					if (selectState == SelectState.CHECKED)
					{
						SelState = SelectState.MIXED;
						NotifyParentOfStateChange(SelectState.MIXED, priorState, useTriState);
					}
					// since i am unchecked, flip from
					// unchecked to mixed. add one to count
					else if (selectState == SelectState.UNCHECKED)
					{
						CheckedCount++;
						SelState = SelectState.MIXED;
						NotifyParentOfStateChange(SelectState.MIXED, priorState, useTriState);
					}
					// i am mixed, so parent must already be mixed
					// do nothing

					break;
				}
			}

			StateChangeMessage("From Child");
		}

	#endregion

	#region debug support

	#if DEBUG
		private void StateChangeMessage(string dir)
		{
			AppendLine("i am " + name + ". Direction| " + dir
				+ nl + "       status is| " + selectState
				+ nl + "     orig status| " + selectTriStateSaved
				+ nl + "       chk count| " + childCheckCount
				+ nl
				);
		}

		private void StateChangeMessage(SelectState value)
		{
			AppendLine("**** i am " + name + ".  checkbox checked"
				+ nl + "       status is| " + value
				+ nl + "     orig status| " + selectTriStateSaved
				+ nl
				);
		}


	#endif

	#endregion

	}
}