﻿#region using

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AndyShared.ClassificationDataSupport.SheetSupport;
using UtilityLibrary;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.MergeSupport;
using AndyShared.SampleFileSupport;
using static AndyShared.ClassificationDataSupport.TreeSupport.LogicalComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.ValueComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations;
using TreeNode = AndyShared.ClassificationDataSupport.TreeSupport.TreeNode;
using static AndyShared.ClassificationDataSupport.SheetSupport.SheetCategory;
using static AndyShared.ClassificationDataSupport.SheetSupport.ItemClassDef;

#endregion

// username: jeffs
// created:  5/2/2020 2:06:09 PM

namespace ClassifySheets.SampleData
{
	public class SampleData
	#if DEBUG
		: INotifyPropertyChanged
	#endif
	{
	#if DEBUG


	#region private fields

		private static string building = "B";

		private static BaseOfTree root;

		private  static TreeNode temp;

	#endregion

	#region ctor

		static SampleData()
		{
			FileNameSheetParser.Instance.CreateSpecialDisciplines(null);
			FileNameSheetParser.Instance.CreateFileNamePattern();

			makeSampleShts();

			TreeBase = new BaseOfTree();
			
			Sample(TreeBase);

			Building = "A";

			TreeBase.Initalize();

			OnPropertyChange_S(nameof(TreeBase));

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

		public static TreeNode Temp
		{
			get => temp;
			set
			{
				temp = value;
				OnPropertyChange_S();
			}
		}

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

		// public static SampleData sd = new SampleData();

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static void Sample(BaseOfTree tn)
		{
			root = tn;

			root.Item.MergeItems = new ObservableCollection<MergeItem>();
			
			MergeItem mi = new MergeItem(0, sht1);
			root.Item.MergeItems.Add(mi);
			
			// mi = new MergeItem(0, sht2);
			// root.Item.MergeItems.Add(mi);
			//
			// mi = new MergeItem(0, sht3);
			// root.Item.MergeItems.Add(mi);

			MakeChildren(root, 0);

			root.CountExtMergeItems();

			// MakeChildren2(root);
			// MakeChildren3();
		}

		public static void SampleFiles(SheetFileList fileList)
		{
			OnPropertyChange_S("Building");

			FilePath<FileNameSheetPdf> sheet;

			sheet = new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A1.0-0 - This is a Test A10x.pdf");
			fileList.AddPath(sheet);

			sheet = new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A-100 - This is a Test A105x.pdf");
			fileList.AddPath(sheet);

			sheet = new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A2.0-0A - This is a Test A20x.pdf");
			fileList.AddPath(sheet);
			//
			// sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A LS3.11-0.1 This is a Test LS30.pdf");
			// fileList.AddPath(sheet);
			//
			// sheet = new FilePath<FileNameSheetPdf>(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A2.1 This is a Test A21.pdf");
			// fileList.AddPath(sheet);

			sheet = new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A GRN.1(left│right) - This is a Test GRN1x.pdf");
			fileList.AddPath(sheet);

			sheet = new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A-101 - This is a Test A101x.pdf");
			fileList.AddPath(sheet);
		}

	#endregion

	#region private methods

		private static void SampleNonApplicableFiles()
		{
			NonApplicableFiles = new Dictionary<string, List<FilePath<FileNameSheetPdf>>>();

			List < FilePath<FileNameSheetPdf>> fileList = new List<FilePath<FileNameSheetPdf>>();

			fileList.Add(new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\B A-101 - This is a Test B A101y.pdf"));
			fileList.Add(new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\B A-102 - This is a Test B A102y.pdf"));
			fileList.Add(new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\B A-103 - This is a Test B A103y.pdf"));

			NonApplicableFiles.Add("B", fileList);
			NonApplicableFiles.Add("C", fileList);
			NonApplicableFiles.Add("D", fileList);
		}


		private static int MAX_DEPTH = 4;
		private static int TOPMAX = 5;
		private static int CHILDMAX = 5;
		private static int BRANCH = 0;

		private static FilePath<FileNameSheetPdf> sht1;

		private static FilePath<FileNameSheetPdf> sht2;

		private static FilePath<FileNameSheetPdf> sht3;


		private static void makeSampleShts()
		{
			sht1 =
				new FilePath<FileNameSheetPdf>(
					@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A1.0-0 - This is a Test A10z.pdf");
			sht2 =
				new FilePath<FileNameSheetPdf>(
					@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A2.0-0A - This is a Test A20z.pdf");
			sht3 =
				new FilePath<FileNameSheetPdf>(
					@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A LS3.11-0.1 - This is a Test LS30z.pdf");
		}


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

				SheetCategory item =
					new SheetCategory($"node title {BRANCH:D2}:{depth:D2}:{i:D2}", $"node description");
				// item.Depth = depth + 1;

				item.ItemClass = Item_Class.IC_BOOKMARK;

				item.MergeItems = new ObservableCollection<MergeItem>();

				if ((i) % 3 != 0)
				{
					MergeItem mi = new MergeItem(0, sht1);
					item.MergeItems.Add(mi);

					mi = new MergeItem(0, sht2);
					item.MergeItems.Add(mi);

					mi = new MergeItem(0, sht3);
					item.MergeItems.Add(mi);

					if (i == 0) item.IsVisible = false;

					item.UpdateMergeProperties();

				}


				node = new TreeNode(parent, item, false);

				int compComponent = depth - (depth  % 2);

				compComponent = compComponent == 0 ? 1 : compComponent;

				if (i == 0)
				{
					// item.CompareOps.Add(new ComparisonOp(ValueCompareOps[(int) STARTS_WITH], "A", true));
					item.IsFixed = true;

					// node = new TreeNode(parent, item, false);
				}
				else if (i == 2)
				{
					// item.CompareOps.Add(new ComparisonOp(null, ValueCompareOps[(int) STARTS_WITH], "A", compComponent));
					item.CompareOps.Add(new ComparisonOp(LOGICAL_NO_OP, STARTS_WITH, "A", compComponent));
					// node = new TreeNode(parent, item, false);

					item.IsFixed = true;
				}
				else if (i == 3 || i == 5)
				{
					item.CompareOps.Add(new ComparisonOp(LOGICAL_NO_OP, EQUALTO, "1", compComponent));
					item.CompareOps.Add(new ComparisonOp(LOGICAL_AND, DOES_NOT_EQUAL, "2", compComponent));

					// node = new TreeNode(parent, item, false);
					item.IsFixed = true;
				}
				else if (i == 4)
				{
					// item.CompareOps.Add(new ComparisonOp(ValueCompareOps[(int) EQUALTO], "1", isFirst: true, ignore: false));
					// item.CompareOps.Add(new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_AND]));
					// item.CompareOps.Add(new ComparisonOp(ValueCompareOps[(int) DOES_NOT_EQUAL], "2"));
					// node = new TreeNode(parent, item, false);
				}
				else if (i == 1)
				{
					item.CompareOps.Add(new ComparisonOp(LOGICAL_NO_OP, EQUALTO, "1", compComponent));
					item.CompareOps.Add(new ComparisonOp(LOGICAL_AND, DOES_NOT_EQUAL, "2", compComponent + 1));

					ComparisonOp v = new ComparisonOp(LOGICAL_OR, DOES_NOT_END_WITH, "A", compComponent);
					v.IsDisabled = true;


					item.CompareOps.Add(v);
					item.CompareOps.Add(new ComparisonOp(LOGICAL_AND, GREATER_THAN_OR_EQUAL, "1", compComponent));
					item.CompareOps.Add(new ComparisonOp(LOGICAL_OR, DOES_NOT_START_WITH, "Z", compComponent));

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

				node.IsExpanded = true;
				node.IsExpandedAlt = true;

				root.AddNode(node);

				root.UpdateProperties();


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
		// 		item.CompareOps.Add(new ComparisonOp(ValueCompareOps[(int) STARTS_WITH], 
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
		// 	item.CompareOps.Add(new ComparisonOp(ValueCompareOps[(int) STARTS_WITH],"A", true));
		//
		// 	TreeNode rootNode = new TreeNode(root, item, false);
		//
		// 	TreeNode node;
		//
		// 	for (int i = 0; i < 5; i++)
		// 	{
		// 		item = new SheetCategory($"node title {0:D2}:{0:D2}:{i:D2}", $"node description");
		//
		// 		item.CompareOps.Add(new ComparisonOp(ValueCompareOps[(int) STARTS_WITH], 
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