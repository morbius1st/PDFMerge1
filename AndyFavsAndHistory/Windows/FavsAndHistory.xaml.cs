#region using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using AndyFavsAndHistory.FavHistoryMgr;
using CSLibraryIo;
using CSLibraryIo.CommonFileFolderDialog;

using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;


using SettingsManager;

#endregion

// projname: AndyFavsAndHistory
// itemname: MainWindow
// username: jeffs
// created:  1/10/2021 6:45:04 PM

namespace AndyFavsAndHistory.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class FavsAndHistory : Window, INotifyPropertyChanged
	{
	#region private fields

		private string message;

		private FilePath<FileNameSimple> location;

		private  FavHistoryManager<SavedFile> favMgr;
		private  FavHistoryManager<SavedFile> histMgr;



	#endregion

	#region ctor

		public FavsAndHistory()
		{
			InitializeComponent();

			Configure();
		}

	#endregion

	#region public properties

		public string Message {
			get => message;
			set
			{
				message = value;
				OnPropertyChange();
			}
	}
		

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure()
		{
			SuiteSettings.Admin.Read();

			WriteLine(logMsgDbS("SiteRootPath", SuiteSettings.Data.SiteRootPath));
			WriteLine(logMsgDbS("Suite Setting Description", SuiteSettings.Info.Description));
			WriteLine(logMsgDbS("Suite Setting Path", SuiteSettings.Path.SettingFilePath));
			WriteLine(logMsgDbS("Location", location.FullFilePath));
			
		}


	#endregion

	#region private methods

		private void Write(string msg)
		{
			Message += msg;
		}

		private void WriteLine(string msg)
		{
			Write(msg + "\n");
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MainWindow";
		}

	#endregion
	}
}