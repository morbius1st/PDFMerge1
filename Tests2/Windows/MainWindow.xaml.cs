using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tests2.DebugSupport;
using Tests2.FileListManager;
using Tests2.OutlineManager;


namespace Tests2.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public FileList filLst { get; private set; } 

		public static FileList flx { get; private set; } = FileList.Instance;

		private DebugHelper dh = DebugHelper.Prime;

		public MainWindow()
		{
			InitializeComponent();
		}

		public void SendMessage(string message)
		{
			tbkUL.AppendText(message);
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
