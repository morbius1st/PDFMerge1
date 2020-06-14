#region using

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClassifierEditor.FilesSupport;
using ClassifierEditor.Tree;
using UtilityLibrary;

using static ClassifierEditor.Tree.CompareOperations;
using static ClassifierEditor.Tree.ComparisonOp;

#endregion

// username: jeffs
// created:  5/2/2020 2:06:09 PM

namespace ClassifierEditor.SampleData
{
	public class SampleData : INotifyPropertyChanged
	{
	#region private fields

		private BaseOfTree root;

	#endregion

	#region ctor

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Sample(BaseOfTree tn)
		{
			root = tn;

			SheetCategory item = new SheetCategory("Base of Tree", "Base of Tree");

			root.Item = item;

//			MakeChildren(root, 0);
			MakeChildren2(root);
		}

		public void SampleFiles(SampleFileList fileList)
		{
			FilePath<FileNameSheetPdf> sheet;

			sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A1.0-0 This is a Test A10.pdf");
			fileList.AddPath(sheet);

			sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A2.0-0 This is a Test A20.pdf");
			fileList.AddPath(sheet);

			sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A3.0-0 This is a Test A30.pdf");
			fileList.AddPath(sheet);
		}

	#endregion

	#region private methods

		private static int MAX_DEPTH = 4;
		private static int TOPMAX = 5;
		private static int CHILDMAX = 5;
		private static int BRANCH = 0;

		private void MakeChildren(TreeNode parent, int depth)
		{
			TreeNode node;

			int max = depth == 0 ? TOPMAX : CHILDMAX;

			if (depth >= MAX_DEPTH) return;

			for (int i = 0; i < max; i++)
			{
				if (depth == 0)
				{
					BRANCH = i;
				}

				SheetCategory item = new SheetCategory($"node title {BRANCH:D2}:{depth:D2}:{i:D2}", $"node description");
				node = new TreeNode(parent, item, false);

				if (i == 0)
				{
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH], "A", true));
					node = new TreeNode(parent, item, false);
					node.IsFixed = true;
				}
				else if (i == 2)
				{
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH], "A", true));
					node = new TreeNode(parent, item, false);
					node.IsLocked = true;
				}
				else if (i == 3)
				{
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) GREATER_THAN_OR_EQUAL], "1", isFirst: true, ignore: false));
					node = new TreeNode(parent, item, false);
				}
				else if (i == 4)
				{
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) EQUALTO], "1", isFirst: true, ignore: false));
					item.CompareOps.Add(new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_AND]));
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) DOES_NOT_EQUAL], "2"));
					node = new TreeNode(parent, item, false);
				} 
				else if (i == 1)
				{
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) EQUALTO], "1", isFirst: true));
					item.CompareOps.Add(new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_AND]));
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) DOES_NOT_EQUAL], "2"));
					item.CompareOps.Add(new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_AND]));
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) DOES_NOT_EQUAL], "2"));
					item.CompareOps.Add(new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_AND]));
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) DOES_NOT_EQUAL], "2"));
					item.CompareOps.Add(new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_AND]));
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) DOES_NOT_EQUAL], "2"));
					node = new TreeNode(parent, item, false);
				} 

				if (i == 3 || i == 5)
				{
					// make a new branch
					// this branch is still associated with the parent branch
					// but its children are associated with the new branch

//					node.IsExpanded = true;

					MakeChildren(node, depth + 1);
				}
				
				root.AddNode(node);
			}
		}

		
		private void MakeChildren2(TreeNode parent)
		{
			TreeNode node;

			for (int i = 0; i < 5; i++)
			{
				SheetCategory item = new SheetCategory($"node title {0:D2}:{0:D2}:{i:D2}", $"node description");

				item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH], 
					char.ConvertFromUtf32(65 + i), true));
				node = new TreeNode(parent, item, false);
				node.IsFixed = true;

				root.AddNode(node);
			}
		}



	#endregion

	#region event processing

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleData";
		}

	#endregion
	}
}