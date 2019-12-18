#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using static Test3.MainWindow.SelectStatus;
using static Test3.MainWindow;
using static Test3.EventTestMgr;

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
	public class EventTest3 : INotifyPropertyChanged
	{
		private string name;
		private NodeType nodeType = NodeType.LEAF;
		private SelectState selectState = SelectState.UNCHECKED;
		private SelectState selectTriStateSaved = SelectState.UNSET;

		private int depth = 0;
		private int parentBranchId = 0;
		private int branchId = 0;
		private int leafId = 0;
		private int uniqueId = 0;

		private bool? checkedStatus = false;
		private int childCheckCount = 0;

		public ObservableCollection<EventTest3> childList { get; private set; } = new ObservableCollection<EventTest3>();

		public EventTest3(     int depth, int parentBranchId, int branchId, int LeafId, int uniqueId,
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

//				OnPropertyChange("childList");
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

		public NodeType NodeType
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
//					SelState = SelectState.CHECKED;
					processStateChange(SelectState.CHECKED);
				}
				else if (value == false)
				{
//					SelState = SelectState.UNCHECKED;
					processStateChange(SelectState.UNCHECKED);
				}
				else
				{
//					SelState = SelectState.MIXED;
					processStateChange(SelectState.MIXED);
				}
			}

		}

		private void processStateChange(SelectState newValue)
		{
			// for leaf, just notify parent
			if (nodeType == NodeType.LEAF)
			{
				SelState = newValue;

				StateChangeMessage(newValue);

				NotifyParentOfStateChange(selectState, SelectState.UNSET);

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
					NotifyParentOfStateChange(selectState, SelectState.UNSET);

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
					NotifyParentOfStateChange(selectState, SelectState.UNSET);

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

		// checking process - from this items checkbox

		// is leaf
		//   +-> stateoriginal -> unset
		//   +-> set new state
		//   +-> onpropertychange
		//   +-> notifyparent
		//   +-> done


		// is branch

		// stateoriginal is not set
		// current is checked or unchecked
		//  +-> set new state
		//  +-> onpropertychange
		//  +-> notifyparent
		//  +-> notifychildren
		//  +-> done


		// stateoriginal is not set  (starting tri-state)
		// current is mixed
		//  +-> stateoriginal = currentstate
		//  +-> set new state
		//  +-> onpropertychange
		//  +-> notifyparent  (current state)
		//  +-> notifychildren  (current state, true)
		//  +-> done


		// stateoriginal is set
		// | | +-> current is mixed - use tristate
		// | |     +-> current becomes checked
		// | |     +-> onpropertychange
		// | |     +-> notifyparent (current state)
		// | |     +-> notifychildren (current state, true)
		// | |     +-> done
		// | +-> current is checked (using tristate [stateoriginal is set])
		// |     +-> current becomes unchecked
		// |     +-> orpropertychange
		// |     +-> notifyparent (current state)
		// |     +-> notifychildren (current state, true)
		// |     +-> done
		// +-> current is unchecked  (using tristate [stateoriginal is set])
		//     +-> current becomes mixed
		//     +-> orpropertychange
		//     +-> notifyparent (current state)
		//     +-> notifychildren (current state, true)
		//     +-> done

		// must notify that tri-state is finished
		// focus lost -> TriStateReset()

		public void TriStateReset()
		{
			SelectStateOriginal = SelectState.UNSET;
		}

		public void Reset()
		{
			childCheckCount = 0;
			selectState = SelectState.UNCHECKED;
			selectTriStateSaved = SelectState.UNSET;

			OnPropertyChange("CheckedCount");
			OnPropertyChange("CheckedStatus");
			OnPropertyChange("SelectState");
			OnPropertyChange("SelectStateOriginal");

		}


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

	#region system overrides

		public override string ToString()
		{
			return nodeType + "::" + name + "::" + selectState;
		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		// statechangefrom parent
		// update based on parent
		// pass the state change to children - no additional processing
		public void StateChangeFromParent(SelectState toThis, bool useTriState)
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
		public void StateChangeFromChild(SelectState newState, SelectState fromState)
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
						NotifyParentOfStateChange(SelectState.CHECKED, priorState);
					}
					else
					{
						// only inform parent of mixed once
						if (selectState != SelectState.MIXED)
						{
							SelState = SelectState.MIXED;
							NotifyParentOfStateChange(SelectState.MIXED, priorState);
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
						NotifyParentOfStateChange(SelectState.UNCHECKED, priorState);
					}
					else
					{
						if (selectState == SelectState.CHECKED)
						{
							SelState = SelectState.MIXED;
							NotifyParentOfStateChange(SelectState.MIXED, priorState);
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
						NotifyParentOfStateChange(SelectState.MIXED, priorState);
					}
					// since i am unchecked, flip from
					// unchecked to mixed. add one to count
					else if (selectState == SelectState.UNCHECKED)
					{
						CheckedCount++;
						SelState = SelectState.MIXED;
						NotifyParentOfStateChange(SelectState.MIXED, priorState);
					}
					// i am mixed, so parent must already be mixed
					// do nothing

					break;
				}
			}

			StateChangeMessage("From Child");
		}


	#region event one - from this to child

		// events
		// event one, from this to child

		// create the delegate
		public delegate void StateChangedNotifyChild(SelectState newState, bool useTriState);

		// create the property / event list holder
		public StateChangedNotifyChild OnStateChangeNotifyChildrenEvent { get; set; }

		public void NotifyChildrenOfStateChange(SelectState newState, bool useTriState)
		{
			if (OnStateChangeNotifyChildrenEvent != null) OnStateChangeNotifyChildrenEvent(newState, useTriState);
		}

	#endregion

	#region event two - from this to parent

		// event two, from this to parent
		// from parent's viewpoint, from child
		// delegate
		public delegate void StateChangeNotifyParent(SelectState newState, SelectState fromState);

		// create the property / event list holder
		public StateChangeNotifyParent OnStateChangeNotifyParentEvent { get; set; }

		// create the event
		public void NotifyParentOfStateChange(SelectState newState, SelectState fromState)
		{
			if (OnStateChangeNotifyParentEvent != null)
				OnStateChangeNotifyParentEvent(newState, fromState);
		}

	#endregion

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