using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Tests.Debug;
using Tests.Sequence;
using UtilityLibrary;
using static Tests.Helpers;

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
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
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
		}

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
			RouteTest();
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

		private void RouteTest()
		{
			//P:\FolderName\FolderName\FolderName\New Text Document.txt

			tbxLeft.Text = "";

			//                         | 
			WriteLineToLeft("current directory 1 is| " + (Directory.GetCurrentDirectory()) + nl);
			WriteLineToLeft("current directory 2 is| " + (Environment.CurrentDirectory) + nl);

			Route<FileNameSimple> r;
			Route<FileNameAsSheet> rx;

			// sample for documentation
			ListRoute(new Route<FileNameSimple>(@"P:\FolderName 1\FolderName 2\FolderName 3\New Text Document.txt"));

			// special - folder name looks like a file name (this is a folder)
			r = new Route<FileNameSimple>(@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt");
			ListRoute(r);
			// special - file has no extension and folder name has an extension
			rx = new Route<FileNameAsSheet>(@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt\A1.00 Text Document.pdf");
			ListRoute(rx);
			// special - file has extension but no filename and folder name has an extension
			r = new Route<FileNameSimple>(@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt\.txt");
			ListRoute(r);
			// special - uses the sheet name class
			r = new Route<FileNameSimple>(@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt\A1.00 Text Document");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"P:\2015-491 Centercal - Long Beach\CD\00 Primary\New folder\2015-491 Centercal Long Beach Bldg B Architectural.rvt");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"P:\2099-999 Sample Project\Publish\9999 Current\A  A2.1-0  - DO NOT REMOVE.pdf");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"\\cs-006\P Drive\2099-999 Sample Project\Publish\9999 Current\A  A2.1-0  - DO NOT REMOVE.pdf");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"\\cs-006\OneDrive\Prior GetFolders\Office Stuff\CAD\Copy Y Drive & Office Standards\2020-010 TEST FOLDER\Publish\.Current\A  A1.1-0  - DO NOT REMOVE.pdf");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"Y:\2020-010 TEST FOLDER\Publish\.Current\A  A1.1-0  - DO NOT REMOVE.pdf");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"C:\Documents\Files\021 - Household\MicroStation\.file");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"\Documents\Files\021 - Household\MicroStation\0047116612.PDF");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"C:\Documents\Files\021 - Household\MicroStation");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"\Documents\Files\021 - Household\MicroStation");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"C:\");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"C:\Documents");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"\Documents");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"");
			ListRoute(r);

			return;

			r = new Route<FileNameSimple>(@"\\CS-003");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"\\CS-003\Documents");
			ListRoute(r);
			r = new Route<FileNameSimple>(@"\\CS-003\Documents\Files\021 - Household\MicroStation");
			ListRoute(r);

		}


		private void ListRoute<T>(Route<T> r) where T : AFileName, new()
		{
//			r.UseUnc = true;

			Type tx = r.GetFileNameObject.GetType();

			FileNameAsSheet sht = null;

			WriteLineToLeft("\n*******\n");
			WriteLineToLeft("type name           | " + tx.Name);
			WriteLineToLeft("GetFullPath         | " + r.GetFullPath);

			if (r.IsValid == false)
			{
				WriteLineToLeft("init route          | " + "** is invalid (" +
					r.GetFullPath + ")**");
				WriteLineToLeft("---------------------\n\n\n");
				return;
			}

			WriteLineToLeft("isvalid             | " + r.IsValid);

			if (r.IsValid)
			{
				//               x                   |
				WriteLineToLeft("Depth               | " + r.Depth);
				WriteLineToLeft("Length              | " + r.Length);
				WriteLineToLeft("GetFolderCount      | " + r.GetFolderCount);

				WriteLineToLeft("GetDriveVolume      | " + (r.GetDriveVolume ?? "is null"));
				WriteLineToLeft("GetDrivePath        | " + (r.GetDrivePath ?? "is null"));
				WriteLineToLeft("GetDriveRoot        | " + (r.GetDriveRoot ?? "is null"));

				WriteLineToLeft("GetUncVolume        | " + (r.GetUncVolume ?? "is null"));
				WriteLineToLeft("GetUncPath          | " + (r.GetUncPath ?? "is null"));
				WriteLineToLeft("GetUncShare         | " + (r.GetUncShare ?? "is null"));

				WriteLineToLeft("GetPath             | " + r.GetPath);
				WriteLineToLeft("GetPathUnc          | " + r.GetPathUnc);
				WriteLineToLeft("GetFolders          | " + r.GetFolders);
				WriteLineToLeft("GetFileName         | " + r.GetFileName);
				WriteLineToLeft("GetFileName-wo-ext  | " + r.GetFileNameWithoutExtension);
				WriteLineToLeft("GetFileExtension    | " + r.GetFileExtension);

				if (tx == typeof(FileNameAsSheet))
				{
					sht = r.GetFileNameObject as FileNameAsSheet;
					WriteLineToLeft("SheetNumber         | " + sht?.SheetNumber ?? "is null");
					WriteLineToLeft("SheetName           | " + sht?.SheetName ?? "is null");
				}

				WriteLineToLeft("status              | ********* ");
				WriteLineToLeft("UseUnc              | " + r.UseUnc);
				WriteLineToLeft("HasQualifiedPath    | " + r.HasQualifiedPath);
				WriteLineToLeft("HasUnc              | " + r.HasUnc);
				WriteLineToLeft("HasDrive            | " + r.HasDrive);
				WriteLineToLeft("hasFilename         | " + r.HasFileName);
				WriteLineToLeft("IsFolderPath        | " + r.IsFolderPath);
				WriteLineToLeft("IsFilePath          | " + r.IsFilePath);
				WriteLineToLeft("IsFound             | " + r.IsFound);

//				WriteLineToLeft("divide pth| " + string.Join(" :: ",
//					r.DividePath(r.GetFolders) ?? new string[1]));

				//                                   |
				WriteLineToLeft("[+]                 | *****  without the back slash prefix");
				for (int i = 0; i < r.Depth; i++)
				{
					//                       x                   |
					//                       [+0]      |
					WriteLineToLeft("[+" + i + "]                | " + r[i]
//						+ "  (" + r.GetFolderName(i) + ")"
						);
				}

				for (int i = -1; i > (r.Depth * -1 - 1); i--)
				{
					//                      x                   |
					//                      [+0]      |
					WriteLineToLeft("[" + i + "]                | " + r[i]
//						+ "  (" + r.GetFolderName(i) + ")"
						);
				}

				//               x                   |
				WriteLineToLeft("[+]                 | ***** with a back slash prefix (add 0.x)");
				for (double i = 0.1; i < r.Depth; i++)
				{
					//                       x                   |
					//                       [+0]      |
					WriteLineToLeft("[+" + i + "]              | " + r[i]);
				}

				for (double i = -1.1; i > (r.Depth * -1 - 1); i--)
				{
					//                      x                   |
					//                      [+0]      |
					WriteLineToLeft("[" + i + "]              | " + r[i]);
				}

				string[] names;

				names =  r.GetPathNames;

				WriteLineToLeft("getpathnames as []");
				for (int i = 0; i < (names?.Length ?? 0); i++)
				{
					//                       x                   |
					//                       [0]      |
					WriteLineToLeft("[" + i + "]                 | "
						+ names[i]);
				}

				names =  r.GetPathNamesAlt;

				WriteLineToLeft("getpathnamesalt as []");
				for (int i = 0; i < (names?.Length ?? 0); i++)
				{
					//                       x                   |
					//                       [0]      |
					WriteLineToLeft("[" + i + "]                 | "
						+ names[i]);
				}

				//               x                   |
				WriteLineToLeft("AssemblePath(2)     | " + r.AssemblePath(2));
				WriteLineToLeft("AssemblePath(-1)    | " + r.AssemblePath(-1));

				WriteLineToLeft("change routines     | ");
				WriteLineToLeft("change filename     | to \"this is a test\"");
				WriteLineToLeft("change extension    | to \"ext\"");

				r.ChangeFileName("this is a test");
				r.ChangeExtension("ext");

				WriteLineToLeft("GetFileName         | " + r.GetFileName);
				WriteLineToLeft("GetFileName-wo-ext  | " + r.GetFileNameWithoutExtension);
				WriteLineToLeft("GetFileExtension    | " + r.GetFileExtension);






//				bool result = PathWay.separate(r.GetFullPath);
//
//				WriteLineToLeft("");
//				WriteLineToLeft("new route tests | worked| " + result.ToString());
//				WriteLineToLeft("");
//				WriteLineToLeft("full route| " + r.GetFullPath);
//
//				if (result)
//				{
//					//                         |
//					WriteLineToLeft("unc vol   | " + );
//					WriteLineToLeft("unc path  | " + Route.xUncPath);
//					WriteLineToLeft("drv vol   | " + Route.xDrvVolume);
//					WriteLineToLeft("file name | " + Route.xFileName);
//					WriteLineToLeft("file ext  | " + Route.xFileExt);
//					WriteToLeft("folders   | " );
//					
//					foreach (string s in Route.xFolders)
//					{
//						WriteToLeft(s + " :: ");
//					}
//
//					WriteLineToLeft("\n");
//					WriteLineToLeft("isqualpath| " + Route.IsQualifiedPath);
//					WriteLineToLeft("isfldrpath| " + Route.IsFolderPath);
//					WriteLineToLeft("isfilepath| " + Route.IsFilePath);
//					WriteLineToLeft("isfound   | " + Route.IsFound);
//					WriteLineToLeft("hasdrive  | " + Route.HasDrive);
//					WriteLineToLeft("hasunc    | " + Route.HasUnc);
//				}
			}
			else
			{
				//                         | 
				WriteLineToLeft("is not    | valid");
			}

			WriteLineToLeft("\n*******\n");
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

	}



}