using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using Tests2.DebugSupport;
using Tests2.FileListManager;
using Tests2.PDFMergeManager;
using System.Windows.Controls;
using System.Windows.Data;
using System.Threading.Tasks;
using System.Windows.Threading;

using Tests2.Support;
using Tests2.Windows.MainWinSupport;
using static UtilityLibrary.MessageUtilities;

using SettingsManager;
using Tests2.DataStore;
using Tests2.TestData;
using UtilityLibrary;
using DataSet = Tests2.TestData.DataSet;

namespace Tests2.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private DataManager<DataSet> dm1;
		private DataManager<SavedFileList> dms;

		private FilePath<FileNameSimple> dataFilePath;

		private SampleData sd;
		private SampleData sds;

		public FileItems FileList { get; private set; }

		public PDFMergeTree MrgTree { get; private set; }

		public static ObservableCollection<KeyValuePair<string,PDFMergeItem>> Mtx { get; set; } = PDFMergeTree.Instance.MergeTree;

		public ObservableDictionary<string, PDFMergeItem> test { get; set; } = new ObservableDictionary<string,
			PDFMergeItem>();

		public static FileItems flx { get; private set; } = FileItems.Instance;


		public string testName = "";


		public MainWindow()
		{
			InitializeComponent();

			tbkUL.AppendText(nl);

			OnPropertyChange("tbkUL");

			SampleDataPrepCreate();
			SampleDataPrepCreate2();
			// SampleDataPrepRead();

		}

		private void SampleDataPrepCreate2()
		{

			dataFilePath  =
				new FilePath<FileNameSimple>(@"C:\Users\jeffs\AppData\Roaming\CyberStudio\Andy\Tests2\savedfilelist1.xml");

			dms = new DataManager<SavedFileList>(dataFilePath);

			// dms.Configure(dataFilePath);

			sds = new SampleData(dms);

			dms.Create(dataFilePath, dataFilePath.FileName, null);

			OnPropertyChange("DataMgrDms");

			dms.Admin.Write();

			TestName = dms.Data.SavedClassfFiles[0].Value.Name;

		}

		private void SampleDataPrepCreate()
		{

			// NullTest();

			dataFilePath  =
				new FilePath<FileNameSimple>(@"C:\Users\jeffs\AppData\Roaming\CyberStudio\Andy\Tests2\dataset1.xml");

			dm1 = new DataManager<DataSet>(dataFilePath);

			// dm1.Configure(dataFilePath);

			sd = new SampleData(dm1);
			// dm1.Create(dataFilePath, null, null);
			dm1.Create(dataFilePath, dataFilePath.FileName, null);

			OnPropertyChange("DataMgr");

			dm1.Admin.Write();
		}

		
		// private void SampleDataPrepRead()
		// {
		// 	dm1 = new DataManager<DataSet>();
		//
		// 	dataFilePath  =
		// 		new FilePath<FileNameSimple>(@"C:\Users\jeffs\AppData\Roaming\CyberStudio\Tests2\dataset1.xml");
		//
		// 	dm1.Configure(dataFilePath);
		// 	dm1.Admin.Read();
		//
		// 	OnPropertyChange("DataMgr");
		//
		// 	listDict();
		// }

		private void listDict()
		{
			foreach (KeyValuePair<string, subDataClass> kvp in DataMgr)
			{
				Debug.Write("Key| " + kvp.Key);
				Debug.Write(" subclass| S1| " + kvp.Value.S1);
				Debug.Write(" subclass| D1| " + kvp.Value.D1);
				Debug.WriteLine(" subclass| I1| " + kvp.Value.I1);
			}
		}


		private void NullTest()
		{

			Debug.WriteLine("null check start");

			int a = 0;

			string s1 = "test";
			string s2 = null;
			string s3 = "";

			sd = null;

			bool result;

			result = nullCheck(a);
			Debug.WriteLine("int is null?| " + result);

			result = nullCheck(s1);
			Debug.WriteLine(" s1 is null?| " + result);

			result = nullCheck(s2);
			Debug.WriteLine(" s2 is null?| " + result);

			result = nullCheck(s3);
			Debug.WriteLine(" s3 is null?| " + result);

			result = nullCheck(sd);
			Debug.WriteLine(" sd is null?| " + result);

			result = nullCheck(dm1);
			Debug.WriteLine("dm1 is null?| " + result);


			// Dictionary<string, string> d = new Dictionary<string, string>();
			//
			// d.Add("alpha", "beta");
			//
			// d["beta"] = "alpha";
			//

		}

		private bool nullCheck<T>(T key)
		{
			if (ReferenceEquals(key, null)) return true;

			return false;
		}

		public string TestName
		{
			get => DataMgrDms?[0].Value.Name ?? "name is null";
			set
			{
				DataMgrDms[0].Value.Name = value;

				OnPropertyChange();
			}
	}

		public  ObservableDictionary<string, SavedFile> DataMgrDms => dms?.Data.SavedClassfFiles ?? null;

		public ObservableDictionary<string, subDataClass> DataMgr => dm1?.Data.Od ?? null;

		public void SendMessage(string message)
		{
			tbkUL.AppendText(message);

			OnPropertyChange("tbkUL");
		}

		public void SendClearMessage()
		{
			tbkUL.Clear();

			OnPropertyChange("tbkUL");
		}

		public void SetProgressMax(int max)
		{
			pb1.Maximum = max;

			pbMax.Text = max.ToString();
		}
//
//		public void ResetProgress()
//		{
//			pb1.Maximum = 100;
//			pbStatus.Text = "Idle";
//			pbMax.Text = "0";
//
//		}

		public ProgressBar StatusBar => pb1;

		public void AddToProgress(int i)
		{
			pb1.Dispatcher.Invoke(() => pb1.Value += i, DispatcherPriority.Background);
		}

		public void UpdateProgressStatus(string status)
		{
			pbStatus.Dispatcher.Invoke(() => pbStatus.Text = status, DispatcherPriority.Background);
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
		
		private void BtnRename_OnClick(object sender, RoutedEventArgs e)
		{
			TestName = "Got Name?";
		}
		
		private void BtnFilter_OnClick(object sender, RoutedEventArgs e)
		{
			dms.Data.View = SavedFileListSupport.FilterByProject("2099-001", dms);
		}
				
		private void BtnRemove_OnClick(object sender, RoutedEventArgs e)
		{
			SavedFileListSupport.RemoveByProject("2099-001", dms);
		}
		
		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			string testKey1 = "boar";
			string testKey2 = "horse";
			subDataClass testValue2 = new subDataClass("jumps high", 6.0, 6);
			subDataClass testValue3 = new subDataClass("jumps high", 6.0, 6);


			DataMgr.Add(testKey1, new subDataClass("The ocher boar in the undergrowth", 5.0, 5));
			DataMgr.Add(testKey2,testValue2);


			Debug.WriteLine("Contains key  | "
				+ testKey1 + "?| " + DataMgr.ContainsKey(testKey1));

			Debug.WriteLine("Contains key  | "
				+ testKey2 + "?| " + DataMgr.ContainsKey(testKey2));

			Debug.WriteLine("Contains value2| "
				+ testValue2.S1 + "?| " + DataMgr.ContainsValue(testValue2));

			Debug.WriteLine("Contains value3| "
				+ testValue3.S1 + "?| " + DataMgr.ContainsValue(testValue3));

			Debug.WriteLine("count | " + DataMgr.Count);

		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			WinTreeDesigns w;
			MrgTree.testevent();

			Debug.WriteLine("@debug");
		}

		private void MainWin_Loaded(object sender, RoutedEventArgs e)
		{
			Process();

			FileList = FileItems.Instance;

			OnPropertyChange("FileList");

			MrgTree = PDFMergeTree.Instance;

			OnPropertyChange("MrgTree");
		}

		private void Process()
		{
			FileListMgr FlMgr = FileListMgr.Instance;
			PDFMergeMgr MrgMgr = PDFMergeMgr.Instance;

			FileListMgr.BaseFolder = new Route(@"C:\2099-999 Sample Project\Publish\Bulletins");

			FlMgr.CreateFileList();

			Debug.WriteLine("sub-item count| " + FileItems.Instance.SubItemCount);

			if (!FlMgr.IsInitialized) return;
			//
			// UserSettings.Data.PriorFolder = FileListMgr.BaseFolder.FullPath;

			MrgMgr.CreateMergeList();

		}

//		private async void test(int i)
//		{
//			Thread.Sleep(100);
//
//
//			this.Dispatcher.Invoke(() =>
//			{
//				pb1.Value = i;
//				pbStatus.Text = i.ToString();
//			});
//		}

		private void ListElapsedTime(TimeSpan ts)
		{
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
				ts.Hours, ts.Minutes, ts.Seconds,
				ts.Milliseconds / 10);
			Console.WriteLine("RunTime " + elapsedTime);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		class TreeViewLineConverter : IValueConverter
		{
			public object Convert(object value, Type targetType, object parameter,
				System.Globalization.CultureInfo culture
				)
			{
				TreeViewItem item = (TreeViewItem) value;
				ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
				return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
			}

			public object ConvertBack(object value, Type targetType, object parameter,
				System.Globalization.CultureInfo culture
				)
			{
				return false;
			}
		}

		private void cbxBranch_OnChecked(object sender, RoutedEventArgs e)
		{
			CheckBox cbx = sender as CheckBox;

			PDFMergeItem item = cbx.DataContext as PDFMergeItem;

			tbkUL.Text = "*** branch check box selected| " + item.BookmarkTitle + nl + nl
				+ tbkUL.Text;


		}

		private void CbxLeaf_LostFocus(object sender, RoutedEventArgs e)
		{
			tbkUL.Text = "*** checkbox lost focus|" + nl + nl
				+ tbkUL.Text;
		}
	}
}