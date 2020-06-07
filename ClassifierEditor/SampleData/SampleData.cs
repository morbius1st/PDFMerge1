#region using

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClassifierEditor.FilesSupport;
using ClassifierEditor.Tree;
using UtilityLibrary;

using static ClassifierEditor.Tree.CompareConditions;
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

			SheetCategory item = new SheetCategory("Base of Tree", "Base of Tree", null);

			root.Item = item;

			MakeChildren(root, 0);
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
			int max = depth == 0 ? TOPMAX : CHILDMAX;

			if (depth >= MAX_DEPTH) return;

			for (int i = 0; i < max; i++)
			{
				if (depth == 0)
				{
					BRANCH = i;
				}

				SheetCategory item = new SheetCategory($"node title {BRANCH:D2}:{depth:D2}:{i:D2}", $"node description", @"(?<=[A-Z])([ -]+)(?=[0-9])");

//				item.CompareOps = new ObservableCollection<ComparisonOperation>();
				if (i == 1)
				{
					item.CompareOps.Add(new ComparisonOperation(ValueConditionList[(int) EQUALTO], "1"));
					item.CompareOps.Add(new ComparisonOperation(LogicalConditionList[(int) LOGICAL_AND]));
					item.CompareOps.Add(new ComparisonOperation(ValueConditionList[(int) DOES_NOT_EQUAL], "2"));
					item.CompareOps.Add(new ComparisonOperation(LogicalConditionList[(int) LOGICAL_OR]));
					item.CompareOps.Add(new ComparisonOperation(ValueConditionList[(int) DOES_NOT_MATCH], "@(?<=[A-Z])([ -]+)(?=[0-9])"));

				} 
				else if (i == 4)
				{
					item.CompareOps.Add(new ComparisonOperation(ValueConditionList[(int) EQUALTO], "1"));
					item.CompareOps.Add(new ComparisonOperation(LogicalConditionList[(int) LOGICAL_AND]));
					item.CompareOps.Add(new ComparisonOperation(ValueConditionList[(int) DOES_NOT_EQUAL], "2"));
				} 
				else if (i == 2)
				{
					item.CompareOps.Add(new ComparisonOperation(ValueConditionList[(int) EQUALTO], "1"));
				}
				else if (i == 3)
				{
					item.CompareOps.Add(new ComparisonOperation(ValueConditionList[(int) NO_OP], "1"));
				}

//				item.CompareOps = new ObservableCollection<ComparisonOperation>();
//				if (i == 1 || i == 4)
//				{
//					item.CompareOps.Add(new ComparisonOperation(ComparisonOp.EQUALS, "1"));
//					item.CompareOps.Add(new ComparisonOperation(ComparisonOp.LOGICAL_AND, ""));
//					item.CompareOps.Add(new ComparisonOperation(ComparisonOp.DOES_NOT_EQUAL, "2"));
//				} else if (i == 2)
//				{
//					item.CompareOps.Add(new ComparisonOperation(ComparisonOp.EQUALS, "1"));
//				}
//				else if (i == 3)
//				{
//					item.CompareOps.Add(new ComparisonOperation(ComparisonOp.NO_OP, ""));
//				}

				


				TreeNode node = new TreeNode(parent, item, false);

				if (i == 3 || i == 5)
				{
					// make a new branch
					// this branch is still associated with the parent branch
					// but its children are associated with the new branch

					MakeChildren(node, depth + 1);
				}
				
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