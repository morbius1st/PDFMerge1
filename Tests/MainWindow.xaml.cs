using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
			tbxLeft.Text = "";

			//                         | 
			WriteLineToLeft("current directory 1 is| " + (Directory.GetCurrentDirectory()) + nl);
			WriteLineToLeft("current directory 2 is| " + (Environment.CurrentDirectory) + nl);

			Route r;

			// special - folder name looks like a file name (this is a folder)
			r = new Route(@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt");
			ListRoute(r);
			// special - file has no extension and folder name has an extension
			r = new Route(@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt\1 New Text Document");
			ListRoute(r);
			// special - file has extension but no filename and folder name has an extension
			r = new Route(@"P:\2099-999 Sample Project\Publish\9999 Current\Test Folder.txt\.txt");
			ListRoute(r);
			r = new Route(@"P:\2015-491 Centercal - Long Beach\CD\00 Primary\New folder\2015-491 Centercal Long Beach Bldg B Architectural.rvt");
			ListRoute(r);
			r = new Route(@"P:\2099-999 Sample Project\Publish\9999 Current\A  A2.1-0  - DO NOT REMOVE.pdf");
			ListRoute(r);
			r = new Route(@"\\cs-006\P Drive\2099-999 Sample Project\Publish\9999 Current\A  A2.1-0  - DO NOT REMOVE.pdf");
			ListRoute(r);
			r = new Route(@"\\cs-006\OneDrive\Prior Folders\Office Stuff\CAD\Copy Y Drive & Office Standards\2020-010 TEST FOLDER\Publish\.Current\A  A1.1-0  - DO NOT REMOVE.pdf");
			ListRoute(r);
			r = new Route(@"Y:\2020-010 TEST FOLDER\Publish\.Current\A  A1.1-0  - DO NOT REMOVE.pdf");
			ListRoute(r);
			r = new Route(@"C:\Documents\Files\021 - Household\MicroStation\.file");
			ListRoute(r);
			r = new Route(@"\Documents\Files\021 - Household\MicroStation\0047116612.PDF");
			ListRoute(r);
			r = new Route(@"C:\Documents\Files\021 - Household\MicroStation");
			ListRoute(r);
			r = new Route(@"\Documents\Files\021 - Household\MicroStation");
			ListRoute(r);
			r = new Route(@"C:\");
			ListRoute(r);
			r = new Route(@"C:\Documents");
			ListRoute(r);
			r = new Route(@"\Documents");
			ListRoute(r);
			r = new Route(@"");
			ListRoute(r);

			return;

			r = new Route(@"\\CS-003");
			ListRoute(r);
			r = new Route(@"\\CS-003\Documents");
			ListRoute(r);
			r = new Route(@"\\CS-003\Documents\Files\021 - Household\MicroStation");
			ListRoute(r);

		}


		private void ListRoute(Route r)
		{
			

			WriteLineToLeft("full route| " + r.FullPath);
//			WriteLineToLeft("is qual'd'| " + System.IO.Path.IsPathFullyQualified(r.FullPath));


			if (r.IsValid == false)
			{
				WriteLineToLeft("init route| " + "** is invalid (" +
					r.FullPath + ")**");
				WriteLineToLeft("---------------------\n\n\n");
				return;
			}

			WriteLineToLeft("isvalid   | " + r.IsValid);

			if (r.IsValid)
			{
				


				WriteLineToLeft("depth     | " + (r.Depth));

				WriteLineToLeft("drv vol   | " + (r.DriveVolume ?? "is null"));
				WriteLineToLeft("drv path  | " + (r.DrivePath ?? "is null"));

				WriteLineToLeft("unc vol   | " + (r.UncVolume ?? "is null"));
				WriteLineToLeft("unc path  | " + (r.UncPath ?? "is null"));
				WriteLineToLeft("unc share | " + (r.UncShare ?? "is null"));
//
//				WriteLineToLeft("unc / path| " + (PathWay.UncVolumeFromPath(r.FullPath) ?? "is null"));
//				WriteLineToLeft("drv / path| " + (PathWay.DriveVolumeFromPath(r.FullPath) ?? "is null"));
//
//				WriteLineToLeft("find unc  | " + (PathWay.findUncFromUncPath(r.FullPath) ?? "is null"));
//				WriteLineToLeft("find drv  | " + (PathWay.findDriveFromUncPath(r.FullPath) ?? "is null"));

				WriteLineToLeft("path      | " + r.Path);
				WriteLineToLeft("folders   | " + r.Folders);
				WriteLineToLeft("hasfname  | " + r.HasFileName);
				WriteLineToLeft("file&ext  | " + r.FileNameAndExtension);
				WriteLineToLeft("file no ex| " + r.FileName);
				WriteLineToLeft("file ext  | " + r.FileExtension);

				WriteLineToLeft("qual'd nam| " + r.HasQualifiedPath);
				WriteLineToLeft("has unc   | " + r.HasUnc);
				WriteLineToLeft("has drv   | " + r.HasDrive);
				WriteLineToLeft("hasFilenam| " + r.HasFileName);
				WriteLineToLeft("is folder | " + r.IsFolderPath);
				WriteLineToLeft("is file   | " + r.IsFilePath);
				WriteLineToLeft("is found  | " + r.IsFound);

//				WriteLineToLeft("divide pth| " + string.Join(" :: ",
//					r.DividePath(r.Folders) ?? new string[1]));

				WriteLineToLeft("[+]       | ********");
				for (int i = 0; i < r.Depth; i++)
				{
					WriteLineToLeft("[+" + i + "]      | " + r[i]
						+ "  (" + r.GetFolder(i) + ")");
				}

				int j;
				for (int i = -1; i > (r.Depth * -1 - 1); i--)
				{
					WriteLineToLeft("[" + i + "]      | " + r[i]
						+ "  (" + r.GetFolder(i) + ")");
				}

				string[] names;

				names =  r.FolderNames;

				WriteLineToLeft("foldernames() as []");
				for (int i = 0; i < (names?.Length ?? 0); i++)
				{
					WriteLineToLeft("[" + i + "]       | "
						+ names[i]);
				}

				WriteLineToLeft("GetFolder(i)");
				for (int i = 0 ; i < r.Depth; i++)
				{
					WriteLineToLeft("(" + $"{i,2:#0}" + ")      | "
						+ r.GetFolder(i));
				}
				for (int i = -1 ; i > -1 * r.Depth -1; i--)
				{
					WriteLineToLeft("(" + $"{i,2:#0}" + ")      | "
						+ r.GetFolder(i));
				}

				//                         |
				WriteLineToLeft("assemble p| " + r.AssemblePath(2));
				WriteLineToLeft("assemble p| " + r.AssemblePath(-1));


//				bool result = PathWay.separate(r.FullPath);
//
//				WriteLineToLeft("");
//				WriteLineToLeft("new route tests | worked| " + result.ToString());
//				WriteLineToLeft("");
//				WriteLineToLeft("full route| " + r.FullPath);
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


			WriteLineToLeft("*******\n");


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