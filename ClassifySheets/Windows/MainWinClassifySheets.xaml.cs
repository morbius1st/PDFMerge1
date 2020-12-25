#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UtilityLibrary;
using AndyShared.MergeSupport;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.SampleFileSupport;
using AndyShared.Support;
using ClassifySheets.Tests;
using static UtilityLibrary.MessageUtilities;

#endregion

// projname: ClassifySheets
// itemname: MainWindow
// username: jeffs
// created:  9/24/2020 1:11:53 PM

namespace ClassifySheets.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWinClassifySheets : Window, INotifyPropertyChanged
	{
	#region private fields

		private Classify classify;

		private ClassificationFile classificationFile;

		// the list of files to categorize;
		private SheetFileList testFileList;

		private Orator.ConfRoom.Announcer announcer;

		private FileNameSheetPdf fp;
		private static TreeNode userSelected;


		private string classfFileArg;

		private bool displayDebugMsgs = true;

		private string tbx1Message;
		private string tbx2Message;
		private string tbx1Progress;

		private Progress<double> p1Double;
		private Progress<string> p1String;
		private double pb1MaximumValue;
		private double pb1Value;
		private string lbl1Content;

		private Progress<double> p2Double;
		private Progress<string> p2String;
		private double pb2MaximumValue;
		private double pb2Value;
		private string lbl2Content;

		private double pbProgVal;
#pragma warning disable CS0169 // The field 'MainWinClassifySheets.pbProgMax' is never used
		private double pbProgMax;
#pragma warning restore CS0169 // The field 'MainWinClassifySheets.pbProgMax' is never used
		private Progress<double> pbProgValue;

	#endregion

	#region ctor

		public MainWinClassifySheets()
		{

			InitializeComponent();

			p1Double = new Progress<double>(value => Pb1Value = value);
			p1String = new Progress<string>(value => Lbl1Content = value);

			p2Double = new Progress<double>(value => Pb2.Value = value);
			// p2Double2 = new Progress<double>(value => Pb2Value = value);
			p2String = new Progress<string>(value => lbl2.Content = value);
			// p2msg = new Progress<string>(value => Tbx2Message += value);

			// pbProgValue = new Progress<double>(value => PbProgValue = value);
			pbProgValue = new Progress<double>(value => Pb2.Value = value);

		}

	#endregion

	#region public properties

		public SheetFileList TestFileList => testFileList;

		public ClassificationFile ClassificationFile
		{
			get => classificationFile;
			private set
			{
				if (value == null) return;

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("win| @ MainWinClassifySheets|@ ClassificationFile");

				InitClassfFile(value);
			}
		}

		public BaseOfTree BaseOfTree
		{
			get => classificationFile?.TreeBase ?? null;
		}

		public TreeNode UserSelected
		{
			get => userSelected;
			set
			{
				userSelected = value;

				OnPropertyChange();
				// OnPropertyChange(nameof(HasSelection));
			}
		}

		public Classify Classify
		{
			get => classify;
			private set
			{
				classify = value;

				OnPropertyChange();
			}
		}

		public string Tbx1Message
		{
			get => tbx1Message;

			private set
			{
				tbx1Message = value;

				OnPropertyChange();
			}
		}

		public string Tbx2Message
		{
			get => tbx2Message;

			private set
			{
				tbx2Message = value;

				OnPropertyChange();
			}
		}

		public string Tbx1Progress
		{
			get => tbx1Progress;

			private set
			{
				tbx1Progress = value;

				OnPropertyChange();
			}
		}

		public string Lbl1Content
		{
			get => lbl1Content;
			set
			{
				lbl1Content = value;
				OnPropertyChange();
			}
		}

		public double Pb1MaximumValue
		{
			get => pb1MaximumValue;
			set
			{
				pb1MaximumValue = value;

				OnPropertyChange();
			}
		}

		public double Pb1Value
		{
			get => pb1Value;
			set
			{
				pb1Value = value;

				OnPropertyChange();
			}
		}

		public string Lbl2Content
		{
			get => lbl2Content;
			set
			{
				lbl2Content = value;
				OnPropertyChange();
			}
		}

		public double Pb2MaximumValue
		{
			get => pb2MaximumValue;
			set
			{
				pb2MaximumValue = value;

				OnPropertyChange();
			}
		}

		public double Pb2Value
		{
			get => pb2Value;
			set
			{
				pb2Value = value;

				OnPropertyChange();
				OnPropertyChange(nameof(BaseOfTree));
			}
		}

		public double PbProgValue
		{
			get => pbProgVal;
			set
			{
				pbProgVal = value;

				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void InitClassfFile(ClassificationFile classfFile)
		{
			classificationFile = classfFile;

			classificationFile.Initialize();

			// treeBase = classificationFile?.TreeBase ?? null;

			if (!classificationFile.SampleFilePath.IsVoid())
			{
				testFileList = new SheetFileList();
				testFileList.ReadSampleSheetFileList(classificationFile.SampleFilePath);
			}

			OnPropertyChange(nameof(ClassificationFile));
			OnPropertyChange(nameof(BaseOfTree));
			OnPropertyChange(nameof(TestFileList));
			OnPropertyChange(nameof(UserSelected));
		}

		public void UpdateProperties()
		{
			OnPropertyChange(nameof(BaseOfTree));
		}

	#endregion

	#region private methods

		private void configClassfFile()
		{
			try
			{
				if (classfFileArg.IsVoid())
				{
					ClassificationFile = ClassificationFileAssist.GetUserClassfFile("PdfSample 1");
				}
				else
				{
					ClassificationFile = ClassificationFileAssist.GetUserClassfFile(classfFileArg);
				}
			}
			catch (Exception ex)
			{
				Debug.Write("Outer Exception| ");
				Debug.WriteLine(ex);

				Debug.Write("Inner Exception| ");
				Debug.WriteLine(ex.InnerException?.Message ?? "None");

				Environment.Exit(1);
			}

			classificationFile.Initialize();
		}

		private async Task WinStartAsync()
		{
			Tbx1Message = "Building is| " + testFileList.Building + "\n\n";
			Tbx2Message = "";

			await Task.Run(() => { ListSampleFileList(); } );
			await Task.Run(() => { ListTreeBase(true); } );
		}

		private void getCmdLineArgs()
		{
			Dictionary<string, string> cmdLineArgs = new Dictionary<string, string>();

			string[] args = Environment.GetCommandLineArgs();

			int count = args.Length;

			if (count >= 3)
			{
				count -= 1;

				if (count % 2 == 0)
				{
					for (var i = 1; i < args.Length; i += 2)
					{
						if (args.Length == i + 1 || args[i + 1].StartsWith("-"))
						{
							cmdLineArgs.Add(args[i], string.Empty);
						}
						else
						{
							cmdLineArgs.Add(args[i], args[i + 1]);
						}
					}

					foreach (KeyValuePair<string, string> kvp in cmdLineArgs)
					{
						if (kvp.Key == "-classification_file")
						{
							classfFileArg = kvp.Value;

							Debug.WriteLine("win| classf file cmd line arg found| " + classfFileArg);
						}
					}
				}
			}
		}

		private void go()
		{
			prepClassify();

			classify.PreProcess();

			classify.Process3();

		}

		private void prepClassify()
		{
			Pb2Value = 0;
			Pb2MaximumValue = testFileList.Files.Count;

			classify.Configure(BaseOfTree, TestFileList);
			classify.ConfigureAsyncReporting(pbProgValue);
		}


	#endregion

	#region event consuming

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			getCmdLineArgs();

			configClassfFile();

			try
			{
				if (classfFileArg.IsVoid())
				{
					ClassificationFile = ClassificationFileAssist.GetUserClassfFile("PdfSample 1");
				}
				else
				{
					ClassificationFile = ClassificationFileAssist.GetUserClassfFile(classfFileArg);
				}
			}
			catch (Exception ex)
			{
				Debug.Write("Outer Exception| ");
				Debug.WriteLine(ex);

				Debug.Write("Inner Exception| ");
				Debug.WriteLine(ex.InnerException?.Message ?? "None");

				Environment.Exit(1);
			}

			classificationFile.Initialize();

			announcer = Orator.GetAnnouncer(this, "toClassify");

			Classify = new Classify();

			classify.OnFileChange += classify_OnFileChange;
			classify.OnTreeNodeChange += classify_OnTreeNodeChange;
#pragma warning disable CS1061 // 'Classify' does not contain a definition for 'OnClassifyCompletion' and no accessible extension method 'OnClassifyCompletion' accepting a first argument of type 'Classify' could be found (are you missing a using directive or an assembly reference?)
			classify.OnClassifyCompletion += classify_OnClassifyCompletion;
#pragma warning restore CS1061 // 'Classify' does not contain a definition for 'OnClassifyCompletion' and no accessible extension method 'OnClassifyCompletion' accepting a first argument of type 'Classify' could be found (are you missing a using directive or an assembly reference?)

			Orator.Listen("fromClassify", OnGetAnnouncement);

			// tell classify to display debug messages
			announcer.Announce(displayDebugMsgs);
		}

		private async void MainWin_ContentRendered(object sender, EventArgs e)
		{
			Thread.Sleep(300);

			await WinStartAsync();
		}

		// classify the data
		private void BtnGo_OnClick(object sender, RoutedEventArgs e)
		{
			go();
		}

		private void OnGetAnnouncement(object sender, object value)
		{
			if (displayDebugMsgs)
			{
				if (value is string)
				{
					Tbx1Message += (string) value;
				}
			}
		}

		// list the contents of treebase
		// run async
		private async void BtnListBase_OnClick(object sender, RoutedEventArgs e)
		{
			Tbx2Message = "Start";

			await Task.Run(() => { ListTreeBase(true); } );
		}

		private async void BtnShow_OnClick(object sender, RoutedEventArgs e)
		{
			Tbx2Message = "";
			tbx2Message += classify.FormatMergeList(BaseOfTree);
			tbx2Message += "\n\n\n";
			await Task.Run(() => { enumerateMergeItems(); } );

			Tbx2Message += "\n";
			await Task.Run(() => { enumerateMergeNodes(); } );
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("win| @ Debug");
		}

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}

		private void classify_OnTreeNodeChange(object sender, TreeNodeChangeEventArgs e)
		{
			Tbx2Message += "classify / node changed| " + e.TreeNode.Item.Title + "\n";
		}

		private void classify_OnFileChange(object sender, FileChangeEventArgs e)
		{
			if (e.SheetFile == null) return;
			Tbx2Message += "classify / file changed| " + e.SheetFile.FileNameObject.SheetNumber + "\n";
		}

		private void classify_OnClassifyCompletion(object sender)
		{
			if (classify.Status != ClassifyStatus.SUCESSFUL) return;

			BaseOfTree.CountExtMergeItems();

			BaseOfTree.UpdateProperties();
			
			UpdateProperties();
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this,  new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MainWindow";
		}

	#endregion

		private int pb2Count;

		private void ListTreeBase(bool showMerge = false)
		{
			Pb2Value = 0;
			Lbl2Content = "";
			Pb2MaximumValue = BaseOfTree.ExtChildCount;
			Tbx2Message = "";
			pb2Count = 0;

			((IProgress<string>) p2String).Report("Starting");

			// StringBuilder sb = new StringBuilder();

			// Tbx2MsgConcat("***** List Tree Base ***\n\n");
			Tbx2Message += ("***** List Tree Base ***\n\n");

			formatNodeDescription(classificationFile.TreeBase, showMerge);

			((IProgress<string>) p2String).Report("Starting");

			((IProgress<double>) p2Double).Report(pb2Count);

			listTreeBase(BaseOfTree, showMerge);

			Tbx2Message += ("\n***** End of Listing ***\n\n");
		}

		private void listTreeBase(TreeNode node, bool showMerge)
		{
			((IProgress<double>) p2Double).Report(++pb2Count);
			((IProgress<string>) p2String).Report(node.Item.Title + " (" + pb2Count + ")");
			// Tbx2Message += pb2Count.ToString("000 ") + node.Item.Title + "\n";
			// Thread.Sleep(50);

			// StringBuilder sb = new StringBuilder();

			foreach (TreeNode childNode in node.Children)
			{
				formatNodeDescription(childNode, showMerge);

				if (childNode.ChildCount > 0)
				{
					listTreeBase(childNode, showMerge);
				}
				else
				{
					((IProgress<double>) p2Double).Report(++pb2Count);
					((IProgress<string>) p2String).Report(childNode.Item.Title
						+ " (" + pb2Count + ") ");
				}
			}
		}

		private void formatNodeDescription(TreeNode node, bool showMerge)
		{
			string marginStr = "  ".Repeat(node.Depth);
			int margin = marginStr.Length;


			Tbx2Message += (node.NodeType.ToString().PadRight(12));
			Tbx2Message += ("| Depth| ");
			Tbx2Message += (node.Depth.ToString().PadRight(5));
			Tbx2Message += ("|");
			Tbx2Message += ((marginStr + ">" + node.Item.Title).PadRight(45));
			Tbx2Message += (" <");

			if (!showMerge)
			{
				Tbx2Message += ("ops count| ");
				Tbx2Message += (node.Item.CompareOps.Count.ToString("##0"));

				if (node.ChildCount != 0)
				{
					Tbx2Message += (" child count| ");
					Tbx2Message += (node.ChildCount.ToString("##0"));
					Tbx2Message += (" ex child count| ");
					Tbx2Message += (node.ExtChildCount.ToString("##0"));
				}
			}

			if (showMerge)
			{
				Tbx2Message += (" item count | ");
				Tbx2Message += (node.ItemCount.ToString("##0"));
				Tbx2Message += (" ex last item count| ");
				Tbx2Message += (node.ExtMergeItemCount.ToString("##0"));
				Tbx2Message += ("\n");


				if (node.Item.MergeItemCount != 0)
				{
					foreach (MergeItem mi in node.Item.MergeItems)
					{
						Tbx2Message += ("SHEET".PadRight(26));
						Tbx2Message += ("| ");
						Tbx2Message += ((marginStr + "   " + mi.FilePath.FileNameNoExt).PadRight(44));
						Tbx2Message += (" < page number| " + mi.PageNumber + " \n");
					}
				}
			}
			else
			{
				Tbx2Message += ("\n");
			}
		}

		private void enumerateMergeItems()
		{
			Pb2Value = 0;
			Lbl2Content = "";
			Pb2MaximumValue = BaseOfTree.ExtMergeItemCount;

			pb2Count = 0;

			Tbx2Message += "Report: *** enumerateMergeItems ***\n";

			int count = 0;

			foreach (MergeItem mi in classify.EnumerateMergeItems())
			{
				count++;
				Tbx2Message += ( "merge item| " + mi.FilePath.FileNameObject.SheetNumber + " :: "
					+ mi.FilePath.FileNameObject.SheetTitle + "\n");

				((IProgress<string>) p2String).Report(mi.FilePath.FileNameObject.SheetNumber);
				((IProgress<double>) p2Double).Report(++pb2Count);
			}

			Tbx2Message += "Report: *** enumerateMergeItems - complete (total of| " + count
				+ ") merge items ***\n";
		}

		private void enumerateMergeNodes()
		{
			Pb2Value = 0;
			Lbl2Content = "";
#pragma warning disable CS1061 // 'BaseOfTree' does not contain a definition for 'ExtItemMergeCount' and no accessible extension method 'ExtItemMergeCount' accepting a first argument of type 'BaseOfTree' could be found (are you missing a using directive or an assembly reference?)
			Pb2MaximumValue = BaseOfTree.ExtMergeItemCount;
#pragma warning restore CS1061 // 'BaseOfTree' does not contain a definition for 'ExtItemMergeCount' and no accessible extension method 'ExtItemMergeCount' accepting a first argument of type 'BaseOfTree' could be found (are you missing a using directive or an assembly reference?)
			pb2Count = 0;

			Tbx2Message += "Report: *** enumerateMergeNodes ***\n";

			foreach (TreeNode node in classify.EnumerateMergeNodes())
			{
				string margin = "  ".Repeat(node.Depth);

				Tbx2Message += (margin + "merge node| " +
					node.Item.Title + "\n");

				if (node.ItemCount > 0)
				{
					foreach (MergeItem mi in node.Item.MergeItems)
					{
						Tbx2Message += ( margin + "   merge item| > " + mi.FilePath.FileNameObject.SheetNumber + " :: "
							+ mi.FilePath.FileNameObject.SheetTitle + "\n");

						((IProgress<string>) p2String).Report(mi.FilePath.FileNameObject.SheetNumber);
						((IProgress<double>) p2Double).Report(++pb2Count);
					}
				}
			}
		}

		private void ListSampleFileList()
		{
			Pb2Value = 0;
			Lbl2Content = "";
			Pb2MaximumValue = BaseOfTree.ExtChildCount;
			Tbx2Message = "";
			StringBuilder sb;

			int i = 0;

			((IProgress<string>) p1String).Report("Starting");
			((IProgress<double>) p1Double).Report(i);


			foreach (FilePath<FileNameSheetPdf> filePath in TestFileList.Files)
			{
				fp = filePath.FileNameObject;

				sb = new StringBuilder();

				((IProgress<string>) p1String).Report(fp.SheetNumber);

				sb.Append(i++.ToString("D3"));
				sb.Append(" |  ").Append((fp.SheetComponentType.ToString()).PadRight(7));
				sb.Append(" |  ").Append((fp.PhaseBldg    ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Discipline   ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Category     ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Subcategory  ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Modifier     ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Submodifier  ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Identifier   ?? "?").PadRight(7));
				sb.Append(" |  ").Append((fp.Subidentifier ?? "?").PadRight(7));
				sb.Append(" |  ").Append(fp.SheetNumber.PadRight(12));
				sb.Append(" |  ").Append(fp.SheetTitle);

				sb.Append(nl);

				Tbx1Message += sb.ToString();

				((IProgress<double>) p1Double).Report(i);
			}

			// OnPropertyChange("Tbx1Message");
		}


	}

}