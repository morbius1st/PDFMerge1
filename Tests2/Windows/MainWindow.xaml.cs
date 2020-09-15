using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Tests2.Settings;
using Tests2.Support;
using Tests2.Windows.MainWinSupport;
using static UtilityLibrary.MessageUtilities;

namespace Tests2.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public FileItems FileList { get; private set; }

		public PDFMergeTree MrgTree { get; private set; }
		public static ObservableCollection<KeyValuePair<string,PDFMergeItem>> Mtx { get; set; } = PDFMergeTree.Instance.MergeTree;

		public ObservableDictionary<string, PDFMergeItem> test { get; set; } = new ObservableDictionary<string,
			PDFMergeItem>();

		public static FileItems flx { get; private set; } = FileItems.Instance;

		public MainWindow()
		{
			InitializeComponent();

			tbkUL.AppendText(nl);

			OnPropertyChange("tbkUL");

		}

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

			FileListMgr.BaseFolder = new Route(UserSettings.Data.PriorFolder);

			FlMgr.CreateFileList();

			Debug.WriteLine("sub-item count| " + FileItems.Instance.SubItemCount);

			if (!FlMgr.IsInitialized) return;

			UserSettings.Data.PriorFolder = FileListMgr.BaseFolder.FullPath;

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