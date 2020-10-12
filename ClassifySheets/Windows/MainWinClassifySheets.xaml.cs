#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using UtilityLibrary;
using AndyShared.MergeSupport;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.SampleFileSupport;
using AndyShared.Support;
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

		private bool displayDebugMsgs = true;

		private Orator.ConfRoom.Announcer announcer;

		private ClassificationFile classificationFile;

		private static TreeNode userSelected;

		private string classfFileArg;

		public static string tbx1Message;
		private string tbx2Message;

		private FileNameSheetPdf fp;

		// the list of files to categorize;
		private SheetFileList testFileList;

		// // the classification tree 
		// private BaseOfTree treeBase;

	#endregion

	#region ctor

		public MainWinClassifySheets()
		{
			InitializeComponent();
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

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ MainWinClassifySheets|@ ClassificationFile");

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
				OnPropertyChange("HasSelection");
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

			OnPropertyChange("ClassificationFile");
			OnPropertyChange("BaseOfTree");
			OnPropertyChange("TestFileList");
			OnPropertyChange("UserSelected");
		}


	#endregion

	#region private methods

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

			getCmdLineArgs();

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

			Tbx1Message = "";

			classificationFile.Initialize();

			announcer = Orator.GetAnnouncer(this, "toClassify");

			classify = new Classify(BaseOfTree, "A");

			Orator.Listen("fromClassify", OnGetAnnouncement);

			// tell classify to display debug messages
			announcer.Announce(displayDebugMsgs);
			ListSampleFileList();
			ListTreeBase();

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
						if (args.Length == i+1 || args[i + 1].StartsWith("-"))
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

							Debug.WriteLine("classf file cmd line arg found| " + classfFileArg);
						}
					}
				}
			}

		}

		private void ListSampleFileList()
		{
			int i = 0;

			foreach (FilePath<FileNameSheetPdf> filePath in TestFileList.Files)
			{
				fp = filePath.FileNameObject;

				StringBuilder sb = new StringBuilder();

				sb.Append(i++.ToString("D3"));
				sb.Append(" |  ").Append((fp.SheetComponentType.ToString()).PadRight(7));
				sb.Append(" |  ").Append((fp.PhaseBldg    ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Discipline   ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Category     ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Subcategory  ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Modifier     ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Submodifier  ?? "?").PadRight(3));
				sb.Append(" |  ").Append((fp.Identifier   ?? "?").PadRight(7));
				sb.Append(" |  ").Append((fp.Subidentifier?? "?").PadRight(7));
				sb.Append(" |  ").Append(fp.SheetNumber.PadRight(12));
				sb.Append(" |  ").Append(fp.SheetTitle);

				sb.Append(nl);

				tbx1Message += sb.ToString();
			}

			OnPropertyChange("Tbx1Message");
		}

		private void ListTreeBase(bool showMerge = false)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(formatNodeDescription(classificationFile.TreeBase, showMerge));

			sb.Append(listTreeBase(classificationFile.TreeBase, showMerge));

			Tbx2Message += sb.ToString();
		}

		private string listTreeBase(TreeNode node, bool showMerge)
		{
			StringBuilder sb = new StringBuilder();

			foreach (TreeNode childNode in node.Children)
			{
				sb.Append(formatNodeDescription(childNode, showMerge));

				if (childNode.ChildCount > 0)
				{
					sb.Append(listTreeBase(childNode, showMerge));
				} 
			}

			return sb.ToString();
		}

		private string formatNodeDescription(TreeNode node, bool showMerge)
		{
			StringBuilder sb = new StringBuilder();

			string marginStr = "  ".Repeat(node.Depth);
			int margin = marginStr.Length;


			sb.Append(node.NodeType.ToString().PadRight(12));
			sb.Append("| Depth| ");
			sb.Append(node.Depth.ToString().PadRight(5));
			sb.Append("|");
			sb.Append((marginStr + ">" + node.Item.Title).PadRight(35));
			sb.Append("< ");
			// sb.Append("> ").Append(field1Width).Append(" < ");

			if (!showMerge)
			{
				sb.Append("ops count| ").Append(node.Item.CompareOps.Count.ToString("##0"));

				if (node.ChildCount!=0)
				{
					sb.Append(" child count| ").Append(node.ChildCount.ToString("##0"));
					sb.Append(" ex child count| ").Append(node.ExtChildCount.ToString("##0"));
				}
			}

			if (showMerge)
			{
				sb.Append(" item count| ").Append(node.ItemCount.ToString("##0"));
				sb.Append(" ex item count| ").Append(node.ExtItemCount.ToString("##0"));
				sb.AppendLine();


				if (node.Item.MergeItemCount != 0)
				{
					foreach (MergeItem mi in node.Item.MergeItems)
					{
						sb.Append(" ".Repeat(20)).Append(mi.FilePath.FileNameNoExt);
						sb.AppendLine();
					}
				}
			}

			// sb.Append(" (vs) ").Append(node.ExtendedChildCount.ToString("##0"));

			sb.AppendLine();

			return sb.ToString();
		}

		private void enumerateMergeItems()
		{
			foreach (MergeItem mi in classify.EnumerateMergeItems())
			{
				Tbx2Message += "merge item| " + mi.FilePath.FileNameObject.SheetNumber + " :: "
					+ mi.FilePath.FileNameObject.SheetTitle + "\n";
			}
		}

		private void enumerateMergeNodes()
		{
			foreach (TreeNode node in classify.EnumerateMergeNodes())
			{
				string margin = "  ".Repeat(node.Depth);

				Tbx2Message += margin + "merge node| " +
					node.Item.Title + "\n";

				if (node.ItemCount > 0)
				{
					foreach (MergeItem mi in node.Item.MergeItems)
					{
						Tbx2Message += margin + "   merge item| > " + mi.FilePath.FileNameObject.SheetNumber + " :: "
							+ mi.FilePath.FileNameObject.SheetTitle + "\n";
					}
				}
			}
		}


	#endregion

	#region event consuming

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


		private void BtnShow_OnClick(object sender, RoutedEventArgs e)
		{
			Tbx2Message = "";

			tbx2Message += classify.FormatMergeList(BaseOfTree);

			tbx2Message += "\n\n\n";

			enumerateMergeItems();

			Tbx2Message += "\n";

			enumerateMergeNodes();
		}


		private void BtnGo_OnClick(object sender, RoutedEventArgs e)
		{
			Tbx1Message = "";
			Tbx2Message = "";

			Tbx1Message += "Before classify\n";

			classify.Process(TestFileList);

			Tbx1Message += "\nSheets have been classified\n";

			if (classify.HasNonApplicableFiles)
			{
				Tbx1Message += "ignored files| " +
					classify.NonApplicableFilesTotalCount;
			}

			ListTreeBase(true);

		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@ Debug");
		}

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);

		}
	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MainWindow";
		}

		#endregion

			
	}
}