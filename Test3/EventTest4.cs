#region + Using Directives

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static Test3.MainWindow;

#endregion


// projname: Test3
// itemname: EventTest4
// username: jeffs
// created:  12/18/2019 9:24:59 PM


namespace Test3
{
	public class EventTest4 : Evt<EventTest4>, INotifyPropertyChanged
	{
	#region fields

		private SelectState selectTriStateSaved = SelectState.UNSET;

		private int depth = 0;
		private int parentBranchId = 0;
		private int branchId = 0;
		private int leafId = 0;
		private int uniqueId = 0;

		private bool _isSelected;
		private bool _isExpanded;

		private bool? checkedStatus = false;
		private int childCheckCount = 0;

	#endregion

	#region ctor

		public EventTest4() { }

		public EventTest4(int depth, int parentBranchId, int branchId, int LeafId, int uniqueId,
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
				childList = new ObservableCollection<EventTest4>();
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
				childList = new ObservableCollection<EventTest4>();
			}

			this.name = MakeName();
		}

//		public override void  AsBranch()
//		{
//			nodeType = NodeType.BRANCH;
//			childList = new ObservableCollection<EventTest4>();
//		}

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

		public string Name
		{
			get  => name;

			set => name = value;
		}

		public int CheckedCount
		{
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
				}
				else if (selectState == SelectState.UNCHECKED)
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

	#region public methods

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

	#endregion

	#region state change

		private void processStateChangeLeaf(SelectState newValue, bool useTriState)
		{
			SelState = newValue;

			NotifyParentOfStateChange(selectState, SelectState.UNSET, useTriState);
		}

		private void processStateChangeBranch(SelectState newValue, bool useTriState)
		{
			SelState = newValue;

			NotifyChildrenOfStateChange(selectState, useTriState);
			NotifyParentOfStateChange(selectState, SelectState.UNSET, useTriState);

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
		}

		private void processStateChange(SelectState newValue)
		{
			// for leaf, just notify parent
			if (nodeType == NodeType.LEAF)
			{
				processStateChangeLeaf(newValue, false);

				return;
			}

			// node type is branch

			if (selectTriStateSaved == SelectState.UNSET)
			{
				if (selectState == SelectState.CHECKED ||
					selectState == SelectState.UNCHECKED)
				{
					processStateChangeBranch(newValue, false);
				}
				// state is mixed // starting tri-state
				else
				{
					selectTriStateSaved = selectState;

					processStateChangeBranch(newValue, true);
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
				SelectState current = selectState;
				SelectState proposed = newValue;

				// selectstate is the current state before the newvalue
				switch (selectState)
				{
				case SelectState.MIXED:
					{
						proposed = SelectState.UNCHECKED;
						break;
					}

				case SelectState.CHECKED:
					{
						proposed = SelectState.MIXED;
						break;
					}

				case SelectState.UNCHECKED:
					{
						proposed = SelectState.CHECKED;
						break;
					}
				}

				SelState = proposed;
				NotifyParentOfStateChange(proposed, current, true);
				NotifyChildrenOfStateChange(proposed, true);
				StateChangeMessage(proposed);
			}
		}

	#endregion

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
		public override void StateChangeFromParent(SelectState toThis, bool useTriState)
		{
			if (useTriState)
			{
				if (selectTriStateSaved == SelectState.UNSET)
				{
					selectTriStateSaved = selectState;
				}

				if (toThis == SelectState.MIXED) toThis = selectTriStateSaved;
			}
			else
			{
				selectTriStateSaved = SelectState.UNSET;
			}

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

		// statechangefrom child to parent
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
			SelectState finalState = SelectState.UNSET;

			if (!useTriState)
			{
				selectTriStateSaved = SelectState.UNSET;
			}
			else if (selectTriStateSaved == SelectState.UNSET)
			{
				// usetristate must be true
				// starting tristate process - save the current state
				selectTriStateSaved = selectState;
			}

			switch (newState)
			{
			// newstate is checked - child is checked.
			case SelectState.CHECKED:
				{
					if (fromState == SelectState.UNCHECKED || fromState == SelectState.UNSET)
					{
						CheckedCount++;
					}

					if (childCheckCount == childList.Count)
					{
						finalState = SelectState.CHECKED;
					}
					else
					{
						// only inform parent of mixed once
						if (selectState != SelectState.MIXED)
						{
							finalState = SelectState.MIXED;
						}
					}

					break;
				}
			// newstate is unchecked - child is unchecked
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
						finalState = SelectState.UNCHECKED;
					}
					else
					{
						if (selectState == SelectState.CHECKED)
						{
							finalState = SelectState.MIXED;
						}
					}

					break;
				}
			// newstate is mixed - child is mixed
			case SelectState.MIXED:
				{
					// must evaluate whether coming from checked to mixed or
					// from unchecked to mixed

					// i am checked, flip from checked to
					// mixed.  inform parent.
					if (selectState == SelectState.CHECKED)
					{
						finalState = SelectState.MIXED;
					}
					// since i am unchecked, flip from
					// unchecked to mixed. add one to count
					else if (selectState == SelectState.UNCHECKED)
					{
						CheckedCount++;
						finalState = SelectState.MIXED;
					}
					// i am mixed, so parent must already be mixed
					// do nothing

					break;
				}
			}

			if (finalState != SelectState.UNSET)
			{
				SelState = finalState;
				NotifyParentOfStateChange(finalState, priorState, useTriState);
			}

			StateChangeMessage("From Child");
		}

	#endregion

	#region debug support

	#if DEBUG
		private void StateChangeMessage(string dir)
		{
			if (!MainWindow.ShowEventMesssage) return;

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