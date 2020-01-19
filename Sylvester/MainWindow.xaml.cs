using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Sylvester.FileSupport;
using Sylvester.Process;
using Sylvester.SelectFolder;
using Sylvester.Settings;

namespace Sylvester
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private bool compare = false;
		private bool go = false;

		public ProcessManager pm { get; private set; }

//		public FolderManager fm { get; private set; }

		public MainWindow()
		{
			InitializeComponent();

			UserSettings.Admin.Read();

		}

		public bool SetFocus
		{
			set { SetFocusComparison(); }
		}

		public bool Compare
		{
			get => compare;
			private set
			{
				compare = value;
				OnPropertyChange();
			}
		}

		public bool Go
		{
			get => go;
			private set
			{
				go = value;
				OnPropertyChange();
			}
		}

		public void SetFocusComparison()
		{
			lvComparison.Focus();
		}

		private void BtnTest1_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@test1");

		}
		
		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@debug");
		}

		private void BtnReset_OnClick(object sender, RoutedEventArgs e)
		{
			pm.Reset();

			OnPropertyChange("pm");
		}

		private void BtnRead_OnClick(object sender, RoutedEventArgs e)
		{
			Compare = pm.Read();

			OnPropertyChange("pm");
		}
		
		private void BtnReadBase_OnClick(object sender, RoutedEventArgs e)
		{
			Compare = pm.ReadBase();

			OnPropertyChange("pm");
		}

		private void BtnCompare_OnClick(object sender, RoutedEventArgs e)
		{
			Go = pm.Process();

			SetFocusComparison();

			OnPropertyChange("pm");
		}

		private void BtnGo_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@go");
			pm.RenameFiles();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void rbtn_OnClick(object sender, RoutedEventArgs e)
		{
			SetFocusComparison();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		private void Mainwin_Loaded(object sender, RoutedEventArgs e)
		{
			pm = new ProcessManager(FpBase, FpTest);
			OnPropertyChange("pm");

//			fm = new FolderManager();
//
//			fm.GetFolders();
		}

		//		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		//		{
		//			Button b = e.OriginalSource as Button;
		//			SkewedButton sb = b.Tag as SkewedButton;
		//
		//			int i = (int) sb.Tag;
		//			string s = sb.InnerSp.Tag as string;
		//
		//			if (i == -1)
		//			{
		//				FpBase.AddPath(FolderManager.BaseFolder.FolderNames);
		//			}
		//		}
	}
}