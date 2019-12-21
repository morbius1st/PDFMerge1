#region + Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UtilityLibrary;
using static Test3.MainWindow;

#endregion


// projname: Test3
// itemname: EventTestMgr
// username: jeffs
// created:  12/12/2019 5:46:43 PM


namespace Test3
{
	public enum NodeType
	{
		BRANCH,
		LEAF
	}

	public enum SelectState
	{
		UNSET = -2,
		MIXED = -1,
		UNCHECKED = 0,
		CHECKED = 1
	}

	public enum SelectMode
	{
		TWO_STATE = 2,
		TRI_STATE = 3
	}


	public class EventTestMgr : INotifyPropertyChanged
	{
		public EventTest3 Root { get; private set; }
		public EventTest4 First { get; private set; }

//		public List<EventTest3> Ev2List { get; private set; } = null;

		public EventTestMgr()
		{
			Root = new EventTest3(0, 0, 0, 0, 0, true);
			Root.Name = "Root (" + Root.Name + ")";

			MakeChildren(Root, 1, 0, Root.childList);


//			OnPropertyChange("Root");
//			OnPropertyChange("childList");

			First = new EventTest4(0, 0, 0, 0, 0, true);
			First.Name = "First  (" + First.Name + ")";

			MakeChildren(First, 1, 0, First.childList);


		}

		public void CheckOne()
		{

			Root.childList[1].SelectState = SelectState.CHECKED;
		}
		
		public void CheckOneOne()
		{
			First.childList[1].childList[1].SelectState = SelectState.CHECKED;
		}
		
		public void UnCheckOneOne()
		{
			First.childList[1].childList[1].SelectState = SelectState.UNCHECKED;
		}
			
		public void CheckOneOneTv2()
		{
			Root.childList[1].childList[1].SelectState = SelectState.CHECKED;
		}
		
		public void UnCheckOneOneTv2()
		{
			Root.childList[1].childList[1].SelectState = SelectState.UNCHECKED;
		}
				
		public void CheckTwo()
		{
			Root.childList[2].SelectState = SelectState.CHECKED;
		}
		
		public void UnCheckTwo()
		{
			Root.childList[2].SelectState = SelectState.UNCHECKED;
		}

		private int MAX = 3;
		private int index = 0;
		private int branch = 0;
		private int MAX_DEPTH = 4;

//
//		private void MakeChildren3( EventTest3 Parent, int depth, int branch, ObservableCollection<EventTest3> Ev2l)
//		{
//			if (depth >= MAX_DEPTH) return;
//
//			for (int j = 0; j < 1; j++)
//			{
//				
//				for (int i = 0; i < MAX; i++)
//				{
//					index++;
//
//					EventTest3 et3;
//
//					if (i == 1 || i == 2)
//					{
//						this.branch++;
//						// make a new branch
//						// this branch is still associated with the parent branch
//						// but its children are associated with the new branch
//						et3 = new EventTest3(depth, branch, this.branch, (MAX * j + i), index, true);
//
//						MakeChildren3(et3, depth + 1, this.branch, et3.childList);
//					}
//					else
//					{
//						et3 = new EventTest3(depth, branch, 0, (MAX * j + i), index, false);
//					}
//
//					Parent.OnStateChangeNotifyChildrenEvent += et3.StateChangeFromParent;
//					et3.OnStateChangeNotifyParentEvent = Parent.StateChangeFromChild;
//
//					Ev2l.Add(et3);
//				}
//			}
//		}

		private void MakeChildren<T>( T Parent, int depth, int branch, ObservableCollection<T> Ev2l) where  T : Evt<T>, new()
		{
			if (depth >= MAX_DEPTH) return;

			for (int j = 0; j < 1; j++)
			{
				
				for (int i = 0; i < MAX; i++)
				{
					index++;

					T et3;

					if (i == 1 || i == 2)
					{
						this.branch++;
						// make a new branch
						// this branch is still associated with the parent branch
						// but its children are associated with the new branch
//						et3 = new T(depth, branch, this.branch, (MAX * j + i), index, true);
						et3 = new T();
						et3.Configure(depth, branch, this.branch, (MAX * j + i), index, true);

						MakeChildren(et3, depth + 1, this.branch, et3.childList);
					}
					else
					{
//						et3 = new T(depth, branch, 0, (MAX * j + i), index, false);
						et3 = new T();
						et3.Configure(depth, branch, this.branch, (MAX * j + i), index, false);
					}

					Parent.OnStateChangeNotifyChildrenEvent += et3.StateChangeFromParent;
					
					et3.OnStateChangeNotifyParentEvent += Parent.StateChangeFromChild;

					Ev2l.Add(et3);
				}
			}
		}

		public void ResetTree()
		{
			resetTree(Root);
			resetTree(First);
		}

		private int depth = 0;
		private int offset = 4;

		public void ListTree()
		{
			ListTree(Root);
		}

		private void ListTree(EventTest3 et3)
		{
			ListItem(et3, et3.Name);
			depth++;

			foreach (EventTest3 ev3 in et3.childList)
			{
				ListItem(ev3, ev3.Name);
			}

			foreach (EventTest3 ev3 in et3.childList)
			{
				if (ev3.NodeType == NodeType.BRANCH)
				{
					depth++;
					ListTree(ev3);
					depth--;
				}
			}

			depth--;
		}

		private void ListItem(EventTest3 et3, string title)
		{
			int offset = 4;

			MainWin.AppendMessageTbk2(nl);

			DebugPrint(CheckedStateSymbol(et3.SelectState) + " " +
				et3.NodeType.ToString().PadRight(7) + "| " + title, "chk count| " + et3.CheckedCount +
				(et3.NodeType == NodeType.BRANCH ? " :: children| " + et3.childList?.Count ?? "none" : "")
				, 0, offset);
			DebugPrint("", et3.SelectState + " :: orig| " 
				+ et3.SelectStateOriginal, offset, 0);
		}

		private void DebugPrint(string title, string msg, int offsetA, int offsetB)
		{ 
			MainWin.AppendMessageTbk2(
			(" ".Repeat(depth * 6 + offsetA) + title.PadRight(20 + offsetB) + " | "));
			MainWin.AppendLineTbk2(msg);
		}

		private char[] CheckedStateSymbols = new [] {'？', '☒', '☐', '☑'};

		private char CheckedStateSymbol(SelectState state)
		{
			return CheckedStateSymbols[(int) state + 2];
		}

		private void resetTree<T>(T et3) where T : Evt<T>
		{
			et3.Reset();

			foreach (T ev3 in et3.childList)
			{
				ev3.Reset();

				if (ev3.NodeType == NodeType.BRANCH
					&& ev3.childList.Count > 0)
				{
					resetTree(ev3);

				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

}
