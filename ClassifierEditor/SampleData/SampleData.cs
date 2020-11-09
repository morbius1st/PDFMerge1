#region using

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using UtilityLibrary;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.MergeSupport;
using AndyShared.SampleFileSupport;
using static AndyShared.ClassificationDataSupport.TreeSupport.LogicalComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.ValueComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations;
using TreeNode = AndyShared.ClassificationDataSupport.TreeSupport.TreeNode;

#endregion

// username: jeffs
// created:  5/2/2020 2:06:09 PM

namespace ClassifierEditor.SampleData
{
	public class SampleData : INotifyPropertyChanged
	{
	#if DEBUG
		

	#region private fields

		private static string building = "B";

		private static BaseOfTree root;

	#endregion

	#region ctor

		static SampleData()
		{
			TreeBase = new BaseOfTree();

			Sample(TreeBase);

			Building = "A";

			TreeBase.Initalize();

			SampleFiles(FileList2);

			SampleNonApplicableFiles();
		}

		public SampleData()
		{
			// sd = new SampleData();
			// SampleData.Sample(TreeBase);
			//
			// SampleData.SampleFiles(FileList2);
		}

	#endregion

	#region public properties

		// this is only for design time sample data
		public static BaseOfTree TreeBase { get; set; } // = new BaseOfTree();

		public static SheetFileList FileList2 { get; private set; } = new SheetFileList();

		public static TreeNode Temp { get; set; }

		public static string FullFilePath { get; set; } = "this is a file path";

		public static Dictionary<string, List<FilePath<FileNameSheetPdf>>> NonApplicableFiles { get; set; }

		public static string test => "test";

		public static string Building
		{
			get => building; 
			private set
			{
				building = value;

				OnPropertyChange_S();
			}
		}

		public static SampleData sd = new SampleData();

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static void Sample(BaseOfTree tn)
		{
			root = tn;

			SheetCategory item = new SheetCategory("Base of Tree", "Base of Tree");

			item.MergeItems = new ObservableCollection<MergeItem>();

			MergeItem mi = new MergeItem(0, sht1);
			item.MergeItems.Add(mi);

			mi = new MergeItem(0, sht2);
			item.MergeItems.Add(mi);

			mi = new MergeItem(0, sht3);
			item.MergeItems.Add(mi);
			
			root.Item = item;

			MakeChildren(root, 0);
			// MakeChildren2(root);
			// MakeChildren3();
		}

		public static void SampleFiles(SheetFileList fileList)
		{
			OnPropertyChange_S("Building");

			FilePath<FileNameSheetPdf> sheet;

			sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A1.0-0 This is a Test A10.pdf");
			fileList.AddPath(sheet);

			sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A-100 This is a Test A105.pdf");
			fileList.AddPath(sheet);

			sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A2.0-0A This is a Test A20.pdf");
			fileList.AddPath(sheet);
			//
			// sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A LS3.11-0.1 This is a Test LS30.pdf");
			// fileList.AddPath(sheet);
			//
			// sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A2.1 This is a Test A21.pdf");
			// fileList.AddPath(sheet);

			sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A GRN.1(left│right) This is a Test GRN1.pdf");
			fileList.AddPath(sheet);

			sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A-101 This is a Test A101.pdf");
			fileList.AddPath(sheet);

			
		}

	#endregion

	#region private methods

		private static void SampleNonApplicableFiles()
		{
			NonApplicableFiles = new Dictionary<string, List<FilePath<FileNameSheetPdf>>>();

			List < FilePath<FileNameSheetPdf>> fileList = new List<FilePath<FileNameSheetPdf>>();

			fileList.Add(new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\B A-101 This is a Test B A101.pdf"));
			fileList.Add(new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\B A-102 This is a Test B A102.pdf"));
			fileList.Add(new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\B A-103 This is a Test B A103.pdf"));

			NonApplicableFiles.Add("B", fileList);
			NonApplicableFiles.Add("C", fileList);
			NonApplicableFiles.Add("D", fileList);

		}


		private static int MAX_DEPTH = 4;
		private static int TOPMAX = 5;
		private static int CHILDMAX = 5;
		private static int BRANCH = 0;

		private static FilePath<FileNameSheetPdf> sht1 = 
			new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A1.0-0 This is a Test A10.pdf");
		private static FilePath<FileNameSheetPdf> sht2 =
			new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A2.0-0A This is a Test A20.pdf");
		private static FilePath<FileNameSheetPdf> sht3 =
			new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A LS3.11-0.1 This is a Test LS30.pdf");

		private static void MakeChildren(TreeNode parent, int depth)
		{
			TreeNode node;

			int max = depth == 0 ? TOPMAX : CHILDMAX;

			if (depth >= MAX_DEPTH) return;

			for (int i = 0; i < max; i++)
			{
				// Debug.WriteLine("@index = " + i);

				if (depth == 0)
				{
					BRANCH = i;
				}

				SheetCategory item = new SheetCategory($"node title {BRANCH:D2}:{depth:D2}:{i:D2}", $"node description");
				item.Depth = depth + 1;

				item.MergeItems = new ObservableCollection<MergeItem>();

				MergeItem mi = new MergeItem(0, sht1);
				item.MergeItems.Add(mi);

				mi = new MergeItem(0, sht2);
				item.MergeItems.Add(mi);

				mi = new MergeItem(0, sht3);
				item.MergeItems.Add(mi);

				node = new TreeNode(parent, item, false);

				if (i == 0)
				{
					// item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH], "A", true));
					item.IsFixed = true;

					// node = new TreeNode(parent, item, false);

				}
				else if (i == 2)
				{
					item.CompareOps.Add(new ValueCompOp(null, ValueCompareOps[(int) STARTS_WITH], "A"));
					// node = new TreeNode(parent, item, false);

					item.IsFixed = true;
				}
				else if (i == 3 || i == 5)
				{
					item.CompareOps.Add(new ValueCompOp(null, ValueCompareOps[(int) EQUALTO], "1"));
					item.CompareOps.Add(new ValueCompOp(LogicalCompareOps[(int) LOGICAL_AND], 
						ValueCompareOps[(int) DOES_NOT_EQUAL], "2"));

					// node = new TreeNode(parent, item, false);
					item.IsFixed = true;
				}
				else if (i == 4)
				{
					// item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) EQUALTO], "1", isFirst: true, ignore: false));
					// item.CompareOps.Add(new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_AND]));
					// item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) DOES_NOT_EQUAL], "2"));
					// node = new TreeNode(parent, item, false);

				} 
				else if (i == 1)
				{

					item.CompareOps.Add(new ValueCompOp(null, ValueCompareOps[(int) EQUALTO], "1"));
					item.CompareOps.Add(new ValueCompOp(LogicalCompareOps[(int) LOGICAL_AND], ValueCompareOps[(int) DOES_NOT_EQUAL], "2"));

					ValueCompOp v = new ValueCompOp(LogicalCompareOps[(int) LOGICAL_OR],
						ValueCompareOps[(int) DOES_NOT_END_WITH], "A");
					v.IsDisabled = true;


					item.CompareOps.Add(v);
					item.CompareOps.Add(new ValueCompOp(LogicalCompareOps[(int) LOGICAL_AND], ValueCompareOps[(int) GREATER_THAN_OR_EQUAL], "1"));
					item.CompareOps.Add(new ValueCompOp(LogicalCompareOps[(int) LOGICAL_OR], ValueCompareOps[(int) DOES_NOT_START_WITH], "Z"));

					// node = new TreeNode(parent, item, false);

					if (depth == 0) Temp = node;

				} 

				if (i == 3)
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

		
		// private void MakeChildren2(TreeNode parent)
		// {
		// 	TreeNode node;
		//
		// 	for (int i = 0; i < 5; i++)
		// 	{
		// 		SheetCategory item = new SheetCategory($"node title {0:D2}:{0:D2}:{i:D2}", $"node description");
		//
		// 		item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH], 
		// 			char.ConvertFromUtf32(65 + i), true));
		//
		// 		node = new TreeNode(parent, item, false);
		// 		item.IsFixed = true;
		//
		// 		root.AddNode(node);
		// 	}
		// }
		//
		// private void MakeChildren3()
		// {
		// 	SheetCategory item = new SheetCategory($"node title {0:D2}:{0:D2}:{0:D2}", $"node description");
		// 	item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH],"A", true));
		//
		// 	TreeNode rootNode = new TreeNode(root, item, false);
		//
		// 	TreeNode node;
		//
		// 	for (int i = 0; i < 5; i++)
		// 	{
		// 		item = new SheetCategory($"node title {0:D2}:{0:D2}:{i:D2}", $"node description");
		//
		// 		item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) STARTS_WITH], 
		// 			char.ConvertFromUtf32(65 + i), true));
		// 		node = new TreeNode(rootNode, item, false);
		// 		item.IsFixed = true;
		//
		// 		root.AddNode(node);
		// 	}
		// 	root.AddNode(rootNode);
		// }

	#endregion

	#region event processing

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
		
		public static event PropertyChangedEventHandler PropertyChanged_S;

		private static void OnPropertyChange_S([CallerMemberName] string memberName = "")
		{
			PropertyChanged_S?.Invoke(null, new PropertyChangedEventArgs(memberName));
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