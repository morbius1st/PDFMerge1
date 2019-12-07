using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Tests2.DebugSupport;
using Tests2.FileListManager;
using Tests2.PDFMergeManager;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xaml;
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


		public static FileItems flx { get; private set; } = FileItems.Instance;

		private DebugHelper dh = DebugHelper.Instance;

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

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			WinTreeDesigns w;

			Debug.WriteLine("@debug");

			PDFMergeMgr.Instance.MTree.Add(new FileItem(new Route(@"c:\path\path\file.pdf")));
		}

		private void BtnAddAtBegin_OnClick(object sender, RoutedEventArgs e)
		{
			PDFMergeMgr.Instance.MTree.Add(
				new FileItem(new Route(@"c:\Add At Begin\file.pdf")), 0);
		}
		
		private void BtnAddAtTwo_OnClick(object sender, RoutedEventArgs e)
		{
			PDFMergeMgr.Instance.MTree.Add(
				new FileItem(new Route(@"c:\Add At Begin\file.pdf")), 1);
		}
				
		private void BtnAddAtEnd_OnClick(object sender, RoutedEventArgs e)
		{
			int idx = PDFMergeMgr.Instance.MTree.Count - 1;

			PDFMergeMgr.Instance.MTree.Add(
				new FileItem(new Route(@"c:\Add At Begin\file.pdf")), idx);
		}

		private void MainWin_Loaded(object sender, RoutedEventArgs e)
		{
			FileList = FileItems.Instance;

			OnPropertyChange("FileList");

			MrgTree = PDFMergeTree.Instance;

			OnPropertyChange("MrgTree");
		}

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
			public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				TreeViewItem item = (TreeViewItem)value;
				ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
				return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
			}

			public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				return false;
			}
		}

	}
}
