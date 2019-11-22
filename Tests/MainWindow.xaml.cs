using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using AODeliverable.FileSelection;
using Tests.Debug;
using Tests.Sequence;
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
//			RouteTest();
//			TvTest2();
			TvTest();
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


			WriteLineToLeft("current directory is| " + (Environment.CurrentDirectory) + nl);


			Route r;

			r = new Route(@"\\CS-004\Documents\Files\021 - Household\MicroStation\0047116612.PDF");
			ListRoute(r);
			r = new Route(@"C:\Documents\Files\021 - Household\MicroStation\0047116612.PDF");
			ListRoute(r);
			r = new Route(@"C:\Documents\Files\021 - Household\MicroStation\.file");
			ListRoute(r);
			r = new Route(@"\Documents\Files\021 - Household\MicroStation\0047116612.PDF");
			ListRoute(r);
			r = new Route(@"\\CS-004\Documents\Files\021 - Household\MicroStation");
			ListRoute(r);
			r = new Route(@"C:\Documents\Files\021 - Household\MicroStation");
			ListRoute(r);
			r = new Route(@"\Documents\Files\021 - Household\MicroStation");
			ListRoute(r);
			r = new Route(@"\\CS-004");
			ListRoute(r);
			r = new Route(@"C:\");
			ListRoute(r);
			r = new Route(@"\\CS-004\Documents");
			ListRoute(r);
			r = new Route(@"C:\Documents");
			ListRoute(r);
			r = new Route(@"\Documents");
			ListRoute(r);
			r = new Route(@"");
			ListRoute(r);
		}

		private void ListRoute(Route r)
		{
			WriteLineToLeft("full route| " + r.FullPath);

			if (r.IsValid == false)
			{
				WriteLineToLeft("init route| " + "** is invalid (" +
					r.FullPath + ")**");
				WriteLineToLeft("---------------------\n\n\n");
				return;
			}

			WriteLineToLeft("isvalid   | " + r.IsValid);
			WriteLineToLeft("levels    | " + (r.Levels));
			WriteLineToLeft("vol name  | " + (r.VolumeName ?? "is null"));
			WriteLineToLeft("isunc     | " + r.IsUnc);
			WriteLineToLeft("isrooted  | " + r.IsRooted);

			WriteLineToLeft("rootpath  | " + (r.RootPath ?? "is null"));
			WriteLineToLeft("root      | " + (r.Root ?? "is null"));
			WriteLineToLeft("path      | " + r.Path);
			WriteLineToLeft("folders   | " + r.Folders);
			WriteLineToLeft("hasfname  | " + r.HasFileName);
			WriteLineToLeft("file&ext  | " + r.FileName);
			WriteLineToLeft("file no ex| " + r.FileWithoutExtension);
			WriteLineToLeft("file ext  | " + r.FileExtension);
			WriteLineToLeft("divide pth| " + string.Join(" :: ",
				r.DividePath(r.Folders) ?? new string[1]));

			WriteLineToLeft("[+]       | ********");
			for (int i = 0; i < r.Levels; i++)
			{
				WriteLineToLeft("[+" + i + "]      | " + r[i]
					+ "  (" + r.GetFolderName(r[i]) + ")");
			}

			int j;
			for (int i = -1; i > (r.Levels * -1); i--)
			{
				WriteLineToLeft("[" + i + "]      | " + r[i]
					+ "  (" + r.GetFolderName(r[i]) + ")");
			}

			WriteLineToLeft("assemble p| " + r.AssemblePath(2));
			WriteLineToLeft("assemble p| " + r.AssemblePath(-1));


//
//			WriteLineToLeft("[0]       | " + r[0]);
//			WriteLineToLeft("[0].name  | " + r.GetFolderName(r[0]));
//			WriteLineToLeft("[2]       | " + r[2]);
//			WriteLineToLeft("[2].name  | " + r.GetFolderName(r[2]));
//			WriteLineToLeft("[-1]      | " + r[-1]);
//			WriteLineToLeft("[-1].name | " + r.GetFolderName(r[-1]));


			WriteLineToLeft("*******\n");

//			WriteLineToLeft("****");
//
//			try
//			{
//				WriteLineToLeft("using Path");
//				WriteLineToLeft("full route| " + r.FullPath);
//				// this will correctly return the root portion of a path
//				// i.e.
//				// c:\
//				// \\cs-004\documents
//				WriteLineToLeft("root      | " + Path.GetPathRoot(r.FullPath));
//				WriteLineToLeft("dir name  | " + Path.GetDirectoryName(r.FullPath));
//				WriteLineToLeft("file name | " + Path.GetFileName(r.FullPath));
//				WriteLineToLeft("fname-no-x| " + Path.GetFileNameWithoutExtension(r.FullPath));
//				WriteLineToLeft("extension | " + Path.GetExtension(r.FullPath));
//				WriteLineToLeft("extension?| " + Path.HasExtension(r.FullPath));
//				WriteLineToLeft("rooted?   | " + Path.IsPathRooted(r.FullPath));
//			}
//			catch (Exception e)
//			{
//				Debug.WriteLine(e);
//				WriteLineToLeft("*** Exception Caught ***");
//			}
//
//			WriteLineToLeft("---------------------\n\n\n");
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