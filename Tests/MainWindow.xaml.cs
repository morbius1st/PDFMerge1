using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using Tests.Debug;
using Tests.PathSupport;
using Tests.Sequence;

using UtilityLibrary;
// using AndyShared.FileSupport;
using static Tests.PathSupport.FileTypeSheetPdf;


// a hierarchical sequence and associated tree view - but using single level codes


// // root element
// SeqPart
// |    
// |    
// |  // these are just to provide the database of items to select
// |  
// |  // a collection of SeqParts for a discipline
// |  SeqParts
// +->Dictionary<string, SeqPart>
// |   |
// |   |   // a collection of discipline SeqParts
// |   |   SeqPartsList
// |   +-->Dictionary<string, SeqParts>
// |  
// |  
// |  // this is the database of items to in the actual record
// |  // the root item which is saved in the actual record
// |  SeqItem
// +->List<SeqPart>
//     |
//     |   // the collection of Named SeqItems just for screen display
//     +-->Dictionary<string, SeqItem>


namespace Tests
{
		public class ClcomboboxBinding : INotifyPropertyChanged
		{
			private string employeeName;
			private string employeeDept;

			public ClcomboboxBinding(string employeeName, string employeeDept)
			{
				this.employeeName = employeeName;
				this.employeeDept = employeeDept;
			}

			public string EmployeeDept
			{
				get => employeeDept;
				set
				{
					employeeDept = value;
					OnPropertyChange();
				}
			}

			public string EmployeeName
			{
				get => employeeName;
				set
				{
					employeeName = value;
					
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			private void OnPropertyChange([CallerMemberName] string memberName = "")
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
			}
		}


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public ObservableCollection<ClcomboboxBinding> comboBoxLoad = new ObservableCollection<ClcomboboxBinding>();




		private bool doNameTest;

		private string nl = System.Environment.NewLine;

		private string s = "this is before the test";

		public OutlineItems os { get; set; } = new OutlineItems();

		public static SeqPartsList Parts2 { get; set; } = new SeqPartsList();
		public static SeqItems Items2 { get; set; } = new SeqItems();
		public SeqItems SqItems { get; set; } = new SeqItems();
		public SeqPartsList Parts { get; set; } = new SeqPartsList();
		public static SeqDisciplines Disciplines2 { get; set; } = SeqDisciplines.Instance;
		public SeqDisciplines Disciplines { get; set; } = SeqDisciplines.Instance;
		public SeqTestData test { get; set; }

		public string tvTestString
		{
			get => s;
			set
			{
				OnPropertyChange();
				s = value;
			}
		}

		private Helpers h;

		public MainWindow()
		{
			InitializeComponent();

			h = new Helpers();

			FilePath<FileNameSimple> f = new FilePath<FileNameSimple>();

			loadComboBox();

		}

		private void loadComboBox()
		{
				ClcomboboxBinding c = new ClcomboboxBinding("John", "dept1");
				comboBoxLoad.Add(c);
				c = new ClcomboboxBinding("Jane", "dept2");
				comboBoxLoad.Add(c);
				c = new ClcomboboxBinding("Bob", "dept3");
				comboBoxLoad.Add(c);
				c = new ClcomboboxBinding("Chuck", "dept4");
				comboBoxLoad.Add(c);
				c = new ClcomboboxBinding("james", "dept5");
				comboBoxLoad.Add(c);
		}

		public  ObservableCollection<ClcomboboxBinding> ComboBoxLoad => comboBoxLoad;


		private void MainWin_Loaded(object sender, RoutedEventArgs e)
		{
			TvTest2();

//			os.Add(OutlineTestData.TestData());
//			os.Sort();

			test = SeqTestData.Instance;

			SqItems = test.si;
			Parts = test.spl;

			OnPropertyChange("test");
			OnPropertyChange("SqItems");
			OnPropertyChange("Parts");
			OnPropertyChange("tx");

//			System.Environment.Exit(0);
		}

		private void BtnGo_Click(object sender, RoutedEventArgs e)
		{
			FilePathTests();
//			TvTest2();
//			TvTest();
		}

		private void BtnSelectArch_Click(object sender, RoutedEventArgs e)
		{
			test.spl.SetFilter("Architectural");
		}

		private void BtnFilterClr_Click(object sender, RoutedEventArgs e)
		{
			test.spl.ClrFilter();
		}

		private void BtnDebug_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("@debug");
		}

		private void BtnDone_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
			System.Environment.Exit(0);
		}

		private void TvTest2()
		{
			TextBox tbx = h.FindNamedVisualChild<TextBox>(Item2, "tbx2") as TextBox;

			Binding b = new Binding();
			b.Path = new PropertyPath("tvTestString");
			b.Mode = BindingMode.TwoWay;
			b.Source = this;

			tbx.SetBinding(TextBox.TextProperty, b);

			tvTestString = "this is a test";
		}

		private void Find()
		{
			var child = h.FindNamedVisualChild<TreeViewItem>(tv01, "Item2");

			var tbx = h.FindNamedVisualChild<TextBox>(Item2, "tbx2");
		}


		private void TvTest()
		{
			TreeViewItem top = new TreeViewItem();
			TreeViewItem child = new TreeViewItem();
			TreeViewItem grandchild = new TreeViewItem();

			top.Header = "top header";
			top.Foreground = Brushes.White;
			top.FontSize = 12;
			top.IsExpanded = true;
			child.Header = "child header";
			grandchild.Header = " grandchild header";

			tv01.Items.Add(top);
			top.Items.Add(child);
			child.Items.Add(grandchild);

			tvTestString = "test string changed";
		}

		private void FilePathTests()
		{

			//P:\FolderName\FolderName\FolderName\New Text Document.txt

			tbxLeft.Text = "";

			// 
			WriteLineToLeft("current directory 1 is| " + (Directory.GetCurrentDirectory()) + nl);
			WriteLineToLeft("current directory 2 is| " + (Environment.CurrentDirectory) + nl);

			FilePath<FileNameSimple> f;
			// FilePath<FileNameAsSheetFile> fx;
			FilePath<FileNameAsSheetPdf> fp;

			// sample for documentation

			// string[] files = new []
			// {
			// 	@"P:\FolderName 1\FolderName 2\FolderName 3\New Text Document.txt",
			// 	@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt",
			// 	@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt\A1.00 Text Document.pdf",
			// 	@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt\.txt",
			// 	@"P:\2015-491 Centercal - Long Beach\CD\00 Primary\New folder\2015-491 Centercal Long Beach Bldg B Architectural.rvt",
			// 	@"P:\2099-999 Sample Project\Publish\9999 Current\A  A2.1-0  - DO NOT REMOVE.pdf",
			// 	@"\\cs-006\P Drive\2099-999 Sample Project\Publish\9999 Current\A  A2.1-0  - DO NOT REMOVE.pdf",
			// 	@"\\cs-006\OneDrive\Prior GetFolders\Office Stuff\CAD\Copy Y Drive & Office Standards\2020-010 TEST FOLDER\Publish\.Current\A  A1.1-0  - DO NOT REMOVE.pdf",
			// 	@"Y:\2020-010 TEST FOLDER\Publish\.Current\A  A1.1-0  - DO NOT REMOVE.pdf",
			// 	@"Y:\2020-010 TEST FOLDER\Publish\.Current\A A11.11A-01A.11  - DO NOT REMOVE.pdf",
			// 	@"\Documents\Files\021 - Household\MicroStation\0047116612.PDF"
			// };
			//
			// foreach (string file in files)
			// {
			// 	try
			// 	{
			// 		fp = new FilePath<FileNameAsSheetPdf>(file);
			// 		ListFilePath(fp);
			// 	}
			// 	catch { }
			// }

			// zFileName Tests SubFolder
			// f = new FilePath<FileNameSimple>(@"C:\2099-999 Sample Project\zFileNameTests_1\zFileNameTests_2\.file");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"C:\2099-999 Sample Project\zFileNameTests_1\zFileNameTests_2\file.");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"C:\2099-999 Sample Project\zFileNameTests_1\zFileNameTests_2\.Folder\.file");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"C:\2099-999 Sample Project\zFileNameTests_1\zFileNameTests_2\.Folder\file.");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"C:\2099-999 Sample Project\zFileNameTests_1\zFileNameTests_2\x.Folder\.file");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"C:\2099-999 Sample Project\zFileNameTests_1\zFileNameTests_2\x.Folder\file.");
			// ListFilePath(f);



			// // special - folder name looks like a file name (this is a folder)
			// f = new FilePath<FileNameSimple>(@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt");
			// ListFilePath(f);
			// // special - file has no extension and folder name has an extension
			// f = new FilePath<FileNameSimple>(
			// 	@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt\A1.00 Text Document.pdf");
			// ListFilePath(f);
			// // special - file has extension but no filename and folder name has an extension
			// f = new FilePath<FileNameSimple>(@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt\.txt");
			// ListFilePath(f);
			// // special - uses the sheet name class
			// f = new FilePath<FileNameSimple>(
			// 	@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt\A1.00 Text Document");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(
			// 	@"P:\2015-491 Centercal - Long Beach\CD\00 Primary\New folder\2015-491 Centercal Long Beach Bldg B Architectural.rvt");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(
			// 	@"P:\2099-999 Sample Project\Publish\9999 Current\A  A2.1-0  - DO NOT REMOVE.pdf");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(
			// 	@"\\cs-006\P Drive\2099-999 Sample Project\Publish\9999 Current\A  A2.1-0  - DO NOT REMOVE.pdf");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(
			// 	@"\\cs-006\OneDrive\Prior GetFolders\Office Stuff\CAD\Copy Y Drive & Office Standards\2020-010 TEST FOLDER\Publish\.Current\A  A1.1-0  - DO NOT REMOVE.pdf");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"Y:\2020-010 TEST FOLDER\Publish\.Current\A  A1.1-0  - DO NOT REMOVE.pdf");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"\Documents\Files\021 - Household\MicroStation\0047116612.PDF");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"C:\Documents\Files\021 - Household\MicroStation");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"\Documents\Files\021 - Household\MicroStation");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"C:\");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"C:\Documents");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"\Documents");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"");
			// ListFilePath(f);
			//
			// return;
			//
			//
			// f = new FilePath<FileNameSimple>(@"\\CS-003");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"\\CS-003\Documents");
			// ListFilePath(f);
			// f = new FilePath<FileNameSimple>(@"\\CS-003\Documents\Files\021 - Household\MicroStation");
			// ListFilePath(f);


			WriteLineToLeft("\n*******\n");
			WriteLineToLeft("Original Path");
			f = new FilePath<FileNameSimple>(@"C:\2099-999 Sample Project\zFileName Tests_1\zFileName Tests_2\File.txt");
			ListFilePath(f);

			// string adjFolderName = f.AssemblePath(-2) + FilePathUtil.PATH_SEPARATOR + f.FileName;

			// f.Up();
			// WriteLineToLeft("\n*******\n");
			// WriteLineToLeft("after up");
			// ListFilePath(f);
			//
			// f.ChangeFileName("File", "txt");
			//
			// f.Down(@"zFileName Tests_2\zFileName Tests_3");
			// WriteLineToLeft("\n*******\n");
			// WriteLineToLeft("after dn");
			// ListFilePath(f);
			//
			// f.Up();
			// WriteLineToLeft("\n*******\n");
			// WriteLineToLeft("after up");

			f.Down(@"Bogus");
			WriteLineToLeft("\n*******\n");
			WriteLineToLeft("after bogus dn");
			ListFilePath(f);

			// WriteLineToLeft("\n*******\n");
			// WriteLineToLeft("Original Path");
			// f = new FilePath<FileNameSimple>(@"C:\2099-999 Sample Project\zFileName Tests_1\zFileName Tests_2");
			// ListFilePath(f);
			//
			// f.ChangeFileName(null);
			//
			// f.Up();
			// WriteLineToLeft("\n*******\n");
			// WriteLineToLeft("after up");
			// ListFilePath(f);


			// WriteLineToLeft("\n*******\n");
			// WriteLineToLeft("adjusted path| " + adjFolderName + nl);
			//
			// f.MakeFilePathInfo(adjFolderName);
			// WriteLineToLeft("adjusted path| " +f.FullFilePath + nl);
			//
			// WriteLineToLeft("\n*******\n");
			// WriteLineToLeft("after adjust");
			// ListFilePath(f);

			doNameTest = false;

			WriteLineToLeft("\n*******\n");
			WriteLineToLeft("Original Path");
			f = new FilePath<FileNameSimple>(@"C:\2099-999 Sample Project\zFileName Tests_1\zFileName Tests_2");
			ListFilePath(f);

			WriteLineToLeft("\n*******\n");
			WriteLineToLeft("after bogus dn");
			f.Down(@"Bogus");
			ListFilePath(f);

			WriteLineToLeft("change filename     | to \"new filename\"");
			WriteLineToLeft("change extension    | to \"extension\"");

			f.ChangeFileName("new filename", "extension"); //File.txt

			WriteLineToLeft("\n*******\n");
			WriteLineToLeft("after name change");
			ListFilePath(f);

			f.Up();
			f.Down(@"Bogus");

			WriteLineToLeft("\n*******\n");
			WriteLineToLeft("after up and");
			WriteLineToLeft("after bogus dn");
			ListFilePath(f);

		}


		private void ListFilePath<T>(FilePath<T> f) where T : AFileName, new()
		{
//			Compare(f);
//			return;

//			f.UseUnc = true;

			Type tx = f.FileNameObject.GetType();

			FileNameAsSheetFile sht = null;
			FileNameAsSheetPdf pdf = null;

			//ruler          x                   |
			WriteLineToLeft("\n*******\n");
			WriteLineToLeft("type name           | " + tx.Name);
			WriteLineToLeft("FullFilePath        | " + f.FullFilePath);
			WriteLineToLeft("isvalid             | " + f.IsValid);


			if (f.IsValid == false)
			{
				//                                   |
				WriteLineToLeft("init filepath       | " + "** is invalid (" +
					f.FullFilePath + ")**");
				WriteLineToLeft("---------------------\n\n\n");
				return;
			}

			string path = FilePathInfo<FileNameSimple>.splitFolderAndFile(f.FullFilePath, out string filename, out string ext);

			WriteLineToLeft("data                | ***************************** ");

			//ruler          x                   |
			WriteLineToLeft("Depth               | " + f.Depth);
			WriteLineToLeft("Length              | " + f.Length);
			WriteLineToLeft("FolderCount         | " + f.FolderCount);

			WriteLineToLeft("status              | ***************************** ");
			WriteLineToLeft("UseUnc              | " + f.UseUnc);
			WriteLineToLeft("HasQualifiedPath    | " + f.HasQualifiedPath);
			WriteLineToLeft("HasUnc              | " + f.HasUnc);
			WriteLineToLeft("HasDrive            | " + f.HasDrive);
			WriteLineToLeft("hasFilename         | " + f.HasFileName);
			WriteLineToLeft("IsFolderPath        | " + f.IsFolderPath);
			WriteLineToLeft("IsFilePath          | " + f.IsFilePath);
			WriteLineToLeft("IsFound             | " + f.IsFound);

			WriteLineToLeft("properties          | ***************************** ");

			WriteLineToLeft("DriveVolume         | " + (f.DriveVolume ?? "is null"));
			WriteLineToLeft("DrivePath           | " + (f.DrivePath ?? "is null"));
			WriteLineToLeft("DriveRoot           | " + (f.DriveRoot ?? "is null"));
											     
			WriteLineToLeft("UncFullFilePath     | " + (f.UncFullFilePath ?? "is null"));
			WriteLineToLeft("UncRoot             | " + (f.UncRoot ?? "is null"));
			WriteLineToLeft("UncVolume           | " + (f.UncVolume ?? "is null"));
			WriteLineToLeft("UncShare            | " + (f.UncShare ?? "is null"));
											     
			WriteLineToLeft("FolderPath          | " + f.FolderPath);
			WriteLineToLeft("UncFolderPath       | " + f.UncFolderPath);
			WriteLineToLeft("Folders             | " + f.Folders);
			WriteLineToLeft("Folders by split    | " + path);

			WriteLineToLeft("FileName            | " + f.FileName);
			WriteLineToLeft("FileName        info| " + f.FilePathInfo.FileName);
			WriteLineToLeft("FileName         obj| " + f.FileNameObject.FileName);
			WriteLineToLeft("FileName-wo-ext     | " + 
				(f.FileNameNoExt.IsVoid() ? "null" : f.FileNameNoExt) 
				+ " Matches (" + (filename.IsVoid() ? "null" : filename) + ")| " 
				+ ((f.FileNameNoExt.IsVoid() ? "null" : f.FileNameNoExt).Equals(filename.IsVoid() ? "" : filename).ToString()));
			WriteLineToLeft("FileName-wo-ext info| " + f.FilePathInfo.FileNameNoExt);
			WriteLineToLeft("FileName-wo-ext  obj| " + f.FileNameObject.FileNameNoExt);

			WriteLineToLeft("FileExtension       | " + f.Extension);
			WriteLineToLeft("FileExtension   info| " + f.FilePathInfo.Extension);
			WriteLineToLeft("FileExtension    obj| does not exist");

			WriteLineToLeft("FileExt-no-sep      | " +
				(f.FileExtensionNoSep.IsVoid() ? "null" : f.FileExtensionNoSep)
				+ " Matches (" + (ext.IsVoid() ? "null" : ext) + ")| "
				+ ((f.FileExtensionNoSep.IsVoid() ? "null" : f.FileExtensionNoSep).Equals(ext.IsVoid() ? "" : 
					ext).ToString()));
			WriteLineToLeft("FileExt-no-sep  info| " + f.FilePathInfo.ExtensionNoSep);
			WriteLineToLeft("FileExt-no-sep   obj| " + f.FileNameObject.ExtensionNoSep);



			if (tx == typeof(FileNameAsSheetFile))
			{
				sht = f.FileNameObject as FileNameAsSheetFile;
				WriteLineToLeft("FileNameAsSheetFile | ***************************** ");
				WriteLineToLeft("SheetNumber         | " + sht?.SheetNumber ?? "is null");
				WriteLineToLeft("SheetName           | " + sht?.SheetName ?? "is null");
			} 
			else if (tx == typeof(FileNameAsSheetPdf))
			{
				pdf = f.FileNameObject as FileNameAsSheetPdf;

				WriteLineToLeft("FileNameAsSheetPdf  | ***************************** ");

				if (pdf == null)
				{ 
					WriteLineToLeft("pdf is              | null");
				} else
				{
					if (pdf.FileType == FileTypeSheetPdf.SHEET_PDF)
					{
						//ruler          x                   |
						WriteLineToLeft("Pdf is              | is a sheet pdf");

						if (pdf.IsValid)
						{
							WriteLineToLeft("Name                | " + pdf?.FileNameNoExt ?? "is null");
							WriteLineToLeft("Extension           | " + pdf?.ExtensionNoSep ?? "is null");
							WriteLineToLeft("FileType            | " + pdf?.FileType ?? "is null");
							WriteLineToLeft("PhaseBldg           | " + pdf?.PhaseBldg ?? "is null");
							WriteLineToLeft("PhaseBldgSep        | " + pdf?.PhaseBldgSep ?? "is null");
							WriteLineToLeft("SheetId             | " + pdf?.SheetId ?? "is null");
							WriteLineToLeft("Separator           | " + pdf?.Separator ?? "is null");
							WriteLineToLeft("SheetNumber         | " + pdf?.SheetNumber ?? "is null");
							WriteLineToLeft("SheetTitle          | " + pdf?.SheetTitle ?? "is null");
							WriteLineToLeft("OriginalSheetTitle  | " + pdf?.OriginalSheetTitle ?? "is null");

							if (pdf.SheetIdIdsMatch)
							{
								//ruler          x                   |
								WriteLineToLeft("SheetIdByComponent  | " + pdf?.SheetIdByComponent ?? "is null");
								WriteLineToLeft("Discipline          | " + pdf?.Discipline  ?? "is null");
								WriteLineToLeft("Category            | " + pdf?.Category    ?? "is null");
								WriteLineToLeft("Seperator1          | " + pdf?.Seperator1  ?? "is null");
								WriteLineToLeft("Subcategory         | " + pdf?.Subcategory ?? "is null");
								WriteLineToLeft("Seperator2          | " + pdf?.Seperator2  ?? "is null");
								WriteLineToLeft("Modifier            | " + pdf?.Modifier    ?? "is null");
								WriteLineToLeft("Seperator3          | " + pdf?.Seperator3  ?? "is null");
								WriteLineToLeft("Submodifier         | " + pdf?.Submodifier ?? "is null");

							}
							else
							{
								//ruler          x                   |
								WriteLineToLeft("SheetIdsMatch       | do not match");
								WriteLineToLeft("SheetIdByComponent  | " + pdf?.SheetIdByComponent ?? "is null");
							}
						}
						else
						{
							WriteLineToLeft("pdf is              | not valid");
						}
					}
					else
					{
						//ruler          x                   |
						WriteLineToLeft("Pdf is              | not a sheet pdf");
					}
				}

			}

			//                                   |
			WriteLineToLeft("------------------- |");
			// WriteLineToLeft("[+]                 | *****  Assembly Folders");
			for (int l = 0; l < 2; l++)
			{
				int forward = l % 2 == 0 ? 1 : -1;

				for (int k = 0; k < 2; k++)
				{
					bool separatorPreface = k % 2 == 1;

					WriteLineToLeft("[+]                 | *****  Assembly Folders | direction| "
						+ (forward == 1 ? "forward" : "reverse") 
						+ " | provide separator preface?| "
						+ (separatorPreface ? "yes" : "no")
						);

					for (int i = 0 ; i < f.FolderCount + 2; i++)
					{
						int m = i * forward;

						string sign = "+";

						if (m < 0)
						{
							sign = "";
						}

						//                             [+0]                | x
						WriteLineToLeft("(" + sign + m + ")                | " + f.AssembleFolders(m, separatorPreface));
					}
				}
			}


			// WriteLineToLeft("Assembly Folders(0) | " + f.AssembleFolders(0));
			// WriteLineToLeft("Assembly Folders(1) | " + f.AssembleFolders(1));
			// WriteLineToLeft("Assembly Folders(2) | " + f.AssembleFolders(2));
			// WriteLineToLeft("Assembly Folders(3) | " + f.AssembleFolders(3));
			// WriteLineToLeft("Assembly Folders(4) | " + f.AssembleFolders(4));
			// WriteLineToLeft("Assembly Folders(-1)| " + f.AssembleFolders(-1));
			// WriteLineToLeft("Assembly Folders(-2)| " + f.AssembleFolders(-2));
			// WriteLineToLeft("Assembly Folders(-3)| " + f.AssembleFolders(-3));
			// WriteLineToLeft("Assembly Folders(-4)| " + f.AssembleFolders(-4));


			//                                   |
			WriteLineToLeft("------------------- |");
			WriteLineToLeft("[+]                 | ***** indexer [i] without the back slash prefix");
			for (int i = (f.Depth + 1) * -1; i < f.Depth + 2; i++)
			{
				string sign = "+";

				if (i < 0)
				{
					sign = "";
				} 

				//                             [+0]                | x
				WriteLineToLeft("[" + sign + i + "]                | " + f[i] ?? "is null");
			}


			double j;

			//               [+0.1]              | x
			//               [+0]                | x
			WriteLineToLeft("------------------- |");
			WriteLineToLeft("[+]                 | ***** with a back slash prefix (add 0.x)");
			for (double i = (f.Depth + 1) * - 1; i < f.Depth + 2; i++)
			{
				j = i;

				string value = "";

				if (i < 0)
				{
					j -= 0.1;

					value = "[" + j + "]";
				}
				// else if (i == 0)
				// {
				// 	j += 0.1;
				//
				// 	value = "[+0]  ";
				// }
				else if (i >= 0)
				{
					j += 0.1;

					value = "[+" + j + "]";
				}

				//                 [+0.1]              | x
				WriteLineToLeft(value + "              | " + f[j] ?? " is null");
			}

			string[] names;

			names =  f.GetPathNames;

			WriteLineToLeft("------------------- |");
			WriteLineToLeft("getpathnames as []  |");
			for (int i = 0; i < (names?.Length ?? 0); i++)
			{

				//                       [0]                 | x	
				WriteLineToLeft("[" + i + "]                 | "
					+ names[i]);
			}

			names =  f.GetPathNamesAlt;

			WriteLineToLeft("------------------- |");
			WriteLineToLeft("getpathnamesalt as []");
			for (int i = 0; i < (names?.Length ?? 0); i++)
			{

				//                       [0]                 | x
				WriteLineToLeft("[" + i + "]                 | "
					+ names[i]);
			}

			//               x                   |
			// WriteLineToLeft("AssemblePath(4)     | " + f.AssemblePath(4));
			// WriteLineToLeft("AssemblePath(3)     | " + f.AssemblePath(3));
			// WriteLineToLeft("AssemblePath(2)     | " + f.AssemblePath(2));
			// WriteLineToLeft("AssemblePath(1)     | " + f.AssemblePath(1));
			// WriteLineToLeft("AssemblePath(0)     | " + f.AssemblePath(0));
			// WriteLineToLeft("AssemblePath(-4)    | " + f.AssemblePath(-4));
			// WriteLineToLeft("AssemblePath(-3)    | " + f.AssemblePath(-3));
			// WriteLineToLeft("AssemblePath(-2)    | " + f.AssemblePath(-2));
			// WriteLineToLeft("AssemblePath(-1)    | " + f.AssemblePath(-1));

			WriteLineToLeft("------------------- |");
			for (int i = f.Depth * -1; i <= f.Depth; i++)
			{
				string space = " ";

				if (i < 0) space = "";


				//                       [0]      |
				//                       x                  |
				WriteLineToLeft("AssemblePath(" + i + ")" + space + "    | "
					+ f.AssemblePath(i));
			}

			if (doNameTest)
			{
				WriteLineToLeft("------------------- |");
				WriteLineToLeft("change routines     | ");

				WriteLineToLeft("change filename     | to \"new filename\"");
				WriteLineToLeft("change extension    | to \"extension\"");

				f.ChangeFileName("new filename", "extension"); //File.txt

				WriteLineToLeft("GetFileName         | " + f.FileName);
				WriteLineToLeft("GetFileName-wo-ext  | " + f.FileNameNoExt);
				WriteLineToLeft("GetFileExtension    | " + f.Extension);
			}


			WriteLineToLeft("\n*******\n");
		}

		private void Compare<T>(FilePath<T> r) where T : AFileName, new ()

		{
			Type tx = r.FileNameObject.GetType();

			WriteLineToLeft("\n*******\n");
			WriteLineToLeft("type name           | " + tx.Name);
			WriteLineToLeft("isvalid             | " + r.IsValid);

			if (!r.IsValid) return;

			string p = r.FullFilePath;


			// note - cannot use system Path class - it does not get file names / file extensions
			// correct when a folder name includes a period '.'
			WriteLineToLeft("GetFullFilePath        (FilePath) | " + r.FullFilePath);
			WriteLineToLeft("GetFullFilePath            (Path) | " + Path.GetFullPath(p));
			WriteLineToLeft("GetDriveRoot       (FilePath) | " + (r.DriveRoot ?? "is null"));
			WriteLineToLeft("GetPathRoot            (Path) | " + Path.GetPathRoot(p));

			WriteLineToLeft("GetPath            (FilePath) | " + r.FolderPath);
			WriteLineToLeft("GetFolders         (FilePath) | " + r.Folders);
			WriteLineToLeft("GetDirectoryName       (Path) | " + Path.GetDirectoryName(p));

			WriteLineToLeft("GetFileName        (FilePath) | " + r.FileName);
			WriteLineToLeft("GetFileName            (Path) | " + Path.GetFileName(p));
			WriteLineToLeft("GetFileName-wo-ext (FilePath) | " + r.FileNameNoExt);
			WriteLineToLeft("GetFileName-wo-ext     (Path) | " + Path.GetFileNameWithoutExtension(p));
			WriteLineToLeft("GetFileExtension   (FilePath) | " + r.Extension);
			WriteLineToLeft("GetFileExtension       (Path) | " + Path.GetExtension(p));

		}

		private void WriteToLeft(string message)
		{
			ThreadPool.QueueUserWorkItem((o) =>
			{
				Dispatcher.Invoke((Action) (() =>
						tbxLeft.Text += message
					));
			});
		}

		private void WriteLineToLeft(string message)
		{
			WriteToLeft(message + nl);
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		private void BtnRowDone_OnClick(object sender, RoutedEventArgs e)
		{
			dataGridB.SelectedIndex = -1;
		}

		public void Connect(int connectionId, object target) { }
	}
}