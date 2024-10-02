#region + Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using Test3.SheetMgr;
using Test3.TreeNoteTests;
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


	public class EventTestMgr : INotifyPropertyChanged
	{
		private TriStateTreeSupport triTreeSupport;

		public EventTest3 Root { get; private set; }
		public EventTest4 First { get; private set; }
		public TreeNode5 Origin { get; private set; }
		public Branch Trunk { get; private set; }


		private TriStateTree<TreeNodeItem> root;
		public TriStateTree<TreeNodeItem> TriTree { get; private set; }

		public ClassificationFile ClassificationFile { get; set; }

//		public List<EventTest3> Ev2List { get; private set; } = null;

		public EventTestMgr()
		{
			triTreeSupport = new TriStateTreeSupport();

			/*
			Root = new EventTest3(0, 0, 0, 0, 0, true);
			Root.Name = "Root (" + Root.Name + ")";

			MakeChildren(Root, 1, 0, Root.childList);

			First = new EventTest4(0, 0, 0, 0, 0, true);
			First.Name = "First  (" + First.Name + ")";

			MakeChildren(First, 1, 0, First.childList);
			*/

			Origin = new TreeNode5();
			Origin.Configure(null, 0, 0, true, true);
			Origin.Name = "Origin";

			TreeNode5 Top = new TreeNode5();
			Top.Configure(Origin, 1, 1, true, true);
			Top.Name = "First Element";

			Origin.AddChild(Top);

			MakeChildren(Top, 1, Top.Children);

			Trunk = new Branch();

			MakeTreeBase();

			getClassificationFile("PdfSample 1A");

			Tests();

		}

		private void getClassificationFile(string fileid)
		{
			ClassificationFile = ClassificationFileAssist.GetUserClassfFile(fileid);

			ClassificationFile.Initialize();

			OnPropertyChange("ClassificationFile");
		}

		private void MakeTreeBase()
		{
			root = triTreeSupport.MakeTriStateTree<TreeNodeItem>();
			TriTree = root;

			MakeChildren(root, 0);
		}

		private void Tests()
		{

			// SheetUtility.test();

			// SheetPdfManager.Instance.ParseSheetNames3();
			// SheetPdfManager.Instance.ShowShtNameResults3();

			// voided
			SheetPdfManager.Instance.ParseSheets1();
			SheetPdfManager.Instance.ShowSheetsResults1();
			//
			// SheetPdfManager.Instance.ParseSheetNumbers2();
			// SheetPdfManager.Instance.ShowShtNumberResults2();





// sequence code tests

//			SequenceCode.Initialize(2, SequenceCode.GENERALSORTCODEPREFIX,
//				SequenceCode.BRANCHSORTFIRSTPREFIX);
//
//			SequenceCode sqA = new SequenceCode("A", null, true);   // branch =	'--A'
//			SequenceCode sqAA = new SequenceCode("A", sqA, true);   // branch = '--A│--A'
//			SequenceCode sqA1 = new SequenceCode("1", sqA);         // leaf	=	'--A│!-1'
//			SequenceCode sqAAA = new SequenceCode("A", sqAA, true); // branch = '--A│--A│--A'
//			SequenceCode sqAA1 = new SequenceCode("1", sqAA);       // leaf =	'--A│--A│!-1'
//
//			SequenceCode sqB = new SequenceCode("B"); // leaf =	'!-B'
//
//			sqAA.Rename("C");
//
//			SequenceCode sqAx2 = new SequenceCode("AA", null, true); // branch ='-AA'

//			SequenceCode sqNull = new SequenceCode(null, null, true); // should fail - does fail

//			SequenceCode sqAx3 = new SequenceCode("AAA", null, true); // should fail - does fail

//			SequenceCode sqBB = new SequenceCode("B", sqB, true);	// should fail - does fail


// sort code tests


//			SortCode.BranchSortOrder = BranchSortOrder.SORTFIRST;
//			
//			SortCode scA = new SortCode("A");
//			SortCode scB = new SortCode("B");
//
//			SortCode scA1 = scA.Append("1");
//
//			SortCode scA2 = scA.Append("2");
//
//			SortCode scA1A = scA1.Append("A");
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

		private int MAX = 4;
		private int MAX5 = 5;

		private int index = 1000;
		private int branch = 0;
		private int MAX_DEPTH = 5;
		private int MAX_DEPTH5 = 4;


		private void MakeChildren(TreeNode2<TreeNodeItem> parent, int depth)
		{
			if (depth >= MAX_DEPTH) return;

			TreeNodeItem item;
			TreeNode2<TreeNodeItem> node;

			for (int i = 0; i < MAX; i++)
			{
				item = new TreeNodeItem($"item_{depth}_{index++}", false, false);

				node = new TreeNode2<TreeNodeItem>(parent, item, false);

				if (i==1 || i == 3) MakeChildren(node, depth+1);

				node.IsExpanded = true;
				node.IsExpandedAlt = true;

				root.AddNode(node);

			}

		}

		private void MakeChildren(TreeNode5 parent, int depth, ObservableCollection<TreeNode5> children)
		{
			if (depth >= MAX_DEPTH) return;


			for (int i = 0; i < MAX5; i++)
			{
				index++;

				TreeNode5 Evt = new TreeNode5();

				if (i == 1 || i == 2)
				{
					// make a new branch
					// this branch is still associated with the parent branch
					// but its children are associated with the new branch
					Evt.Configure(parent, depth, index, true, false);
					Evt.Name = Evt.MakeName();

					MakeChildren(Evt, depth + 1, Evt.Children);
				}
				else
				{
					Evt.Configure(parent, depth, index, false, false);
					Evt.Name = Evt.MakeName();
				}

				children.Add(Evt);
			}

		}

		private void MakeChildren<T>( T Parent, int depth, int branch, ObservableCollection<T> Ev2l) where  T : Evt<T>, new()
		{
			if (depth >= MAX_DEPTH) return;

			for (int j = 0; j < 3; j++)
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
			resetTree(Origin);
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

		private void resetTree(TreeNode5 evt)
		{
			evt.ResetTree();

//			foreach (EventTest5 ev in evt.Children)
//			{
//				ev.Reset();
//
//				if (ev.NodeType == NodeType.BRANCH
//					&& ev.ChildCount > 0)
//				{
//					resetTree(ev);
//				}
//			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

}
