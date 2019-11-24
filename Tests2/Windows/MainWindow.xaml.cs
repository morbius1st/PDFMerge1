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

	#if DEBUG
		public OutlineMgr olm { get; private set; } = new OutlineMgr();
	#endif

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
			filLst = FileList.Instance;

			OnPropertyChange("filLst");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
