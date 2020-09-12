#region using

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using ClassifierEditor.SampleData;
// using ClassifierEditor.Tree;
using UtilityLibrary;

// using static ClassifierEditor.Tree.CompareOperations;
// using static ClassifierEditor.Tree.ComparisonOp;



using static AndyShared.ClassificationDataSupport.TreeSupport.ComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FilesSupport;

#endregion

// username: jeffs
// created:  5/2/2020 2:06:09 PM

namespace ClassifierEditor.SampleData
{
	public class SampleData : INotifyPropertyChanged
	{
	#if DEBUG
		

	#region private fields

		private static BaseOfTree root;

	#endregion

	#region ctor

		static SampleData()
		{
			// sd = new SampleData();
			SampleData.Sample(TreeBase);
		
			SampleData.SampleFiles(FileList2);
		
		}

		public SampleData()
		{
			// sd = new SampleData();
			SampleData.Sample(TreeBase);

			SampleData.SampleFiles(FileList2);
		}

	#endregion

	#region public properties

		// this is only for design time sample data
		public static BaseOfTree TreeBase { get; set; } = new BaseOfTree();

		public static SampleFileList FileList2 { get; private set; } = new SampleFileList();

		public static TreeNode Temp { get; set; }

		public static string FullFilePath { get; set; } = "this is a file path";

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static void Sample(BaseOfTree tn)
		{
			root = tn;

			SheetCategory item = new SheetCategory("Base of Tree", "Base of Tree");
			
			root.Item = item;

			MakeChildren(root, 0);
			// MakeChildren2(root);
			// MakeChildren3();
		}

		public static void SampleFiles(SampleFileList fileList)
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

		private static void MakeChildren(TreeNode parent, int depth)
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
				item.Depth = depth + 1;
				node = new TreeNode(parent, item, false);


				if (i == 0)
				{
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH], "A", true));
					item.IsFixed = true;

					node = new TreeNode(parent, item, false);

				}
				else if (i == 2)
				{
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH], "A", true));
					node = new TreeNode(parent, item, false);

					item.IsFixed = true;
				}
				else if (i == 3)
				{
					item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) GREATER_THAN_OR_EQUAL], "1", isFirst: true, ignore: false));
					node = new TreeNode(parent, item, false);
					item.IsFixed = true;
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

					Temp = node;

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
				item.IsFixed = true;

				root.AddNode(node);
			}
		}
		
		private void MakeChildren3()
		{
			SheetCategory item = new SheetCategory($"node title {0:D2}:{0:D2}:{0:D2}", $"node description");
			item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH],"A", true));

			TreeNode rootNode = new TreeNode(root, item, false);

			TreeNode node;

			for (int i = 0; i < 5; i++)
			{
				item = new SheetCategory($"node title {0:D2}:{0:D2}:{i:D2}", $"node description");

				item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH], 
					char.ConvertFromUtf32(65 + i), true));
				node = new TreeNode(rootNode, item, false);
				item.IsFixed = true;

				root.AddNode(node);
			}
			root.AddNode(rootNode);
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

		public static string ToSTring() => "This is a test";

	#endregion

	#endif
	}
}