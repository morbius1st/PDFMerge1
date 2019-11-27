using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Tests2.DebugSupport;
using Tests2.FileListManager;

using static UtilityLibrary.MessageUtilities;


namespace Tests2.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public static string futureMsg { get; set; } = "Future message";

		public FileList filLst { get; private set; } 

		public static FileList flx { get; private set; } = FileList.Instance;

		private DebugHelper dh = DebugHelper.Prime;

		public MainWindow()
		{
			InitializeComponent();

			tbkUL.AppendText(nl);
			tbkUL.AppendText(futureMsg);
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
			Debug.WriteLine("@debug");
		}

		private void MainWin_Loaded(object sender, RoutedEventArgs e)
		{
//			Stopwatch stopwatch = new Stopwatch();
//
//			stopwatch.Start();

			filLst = FileList.Instance;

//			stopwatch.Stop();
//
//			ListElapsedTime(stopwatch.Elapsed);
//
//			stopwatch.Start();

			OnPropertyChange("filLst");

//			stopwatch.Stop();
//
//			ListElapsedTime(stopwatch.Elapsed);


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
	}
}
