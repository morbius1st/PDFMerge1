using System.ComponentModel;
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
		public FilesManager fm { get; private set; }

		public FolderManager fldm { get; private set; }

		public MainWindow()
		{
			InitializeComponent();

			fm= new FilesManager();
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