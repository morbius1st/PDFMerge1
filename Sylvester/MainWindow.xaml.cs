using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Sylvester.FileSupport;
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

		public FilesManager fm { get; private set; }

		public FolderManager fldm { get; private set; }


		public bool Compare
		{
			get => compare;
			private set
			{
				compare = value;
				OnPropertyChange();
			}
		}

		public MainWindow()
		{
			InitializeComponent();

			fm= new FilesManager();
			OnPropertyChange("fm");
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@debug");
		}
		
		private void BtnReset_OnClick(object sender, RoutedEventArgs e)
		{
			fm.Reset();

			OnPropertyChange("fm");
		}

		private void BtnRead_OnClick(object sender, RoutedEventArgs e)
		{
			Compare = fm.Read();

			OnPropertyChange("fm");
		}

		private void BtnGo_OnClick(object sender, RoutedEventArgs e)
		{
			if (!fm.Process()) return;

			OnPropertyChange("fm");
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		private void Mainwin_Loaded(object sender, RoutedEventArgs e)
		{

			fm = new FilesManager();

			fldm = new FolderManager();

			fldm.GetFolders();


		}
	}
}