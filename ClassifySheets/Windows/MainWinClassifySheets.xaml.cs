#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using UtilityLibrary;
using AndyShared;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.FileSupport.SheetPDF;
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

		private ClassificationFile classificationFile;

		private static TreeNode userSelected;

		private string classfFileArg;

		private string tbx1Message;
		private string tbx2Message;

		private FileNameSheetPdf fp;

		// the list of files to categorize;
		private SheetFileList testFileList;

		// the classification tree 
		private BaseOfTree treeBase;

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
			get => treeBase;
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
				tbx1Message += value;

				OnPropertyChange();
			}
		}
		
		public string Tbx2Message
		{
			get => tbx2Message;

			private set
			{
				tbx2Message += value;

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

			treeBase = classificationFile?.TreeBase ?? null;

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

			classificationFile.Initialize();

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

			tbx1Message = "";

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

		private void ListTreeBase()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(formatNodeDescription(classificationFile.TreeBase));

			sb.Append(listTreeBase(classificationFile.TreeBase));

			Tbx2Message = sb.ToString();
		}

		private string listTreeBase(TreeNode node)
		{
			StringBuilder sb = new StringBuilder();

			foreach (TreeNode childNode in node.Children)
			{
				sb.Append(formatNodeDescription(childNode));

				if (childNode.ChildCount > 0)
				{
					sb.Append(listTreeBase(childNode));
				} 
			}

			return sb.ToString();
		}

		private string formatNodeDescription(TreeNode node)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(node.NodeType.ToString().PadRight(12));
			sb.Append("| Depth| ");
			sb.Append(node.Depth.ToString().PadRight(5));
			sb.Append("|");
			sb.Append(("  ".Repeat(node.Depth) + ">" + node.Item.Title));
			sb.Append("< ");

			sb.Append("compare count| ").Append(node.Item.CompareOps.Count);


			sb.AppendLine();

			return sb.ToString();
		}


	#endregion

	#region event consuming

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