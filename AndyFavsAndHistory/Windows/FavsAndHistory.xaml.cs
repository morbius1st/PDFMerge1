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


		private const string FAV_HIST_FILE_EXT = "xml";
		private const string FAV_FILE_NAME = "Favorite-List";
		private const string HIST_FILE_NAME = "History-List";

		private string message;

		private  FavHistoryManager favMgr;
		private  FavHistoryManager histMgr;

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

			favMgr.Configure(FilePathUtil.AssembleFilePath(FAV_FILE_NAME, FilePathUtil.XML_EXTNOSEP, SuiteSettings.Data.SiteRootPath));
			histMgr.Configure(FilePathUtil.AssembleFilePath(HIST_FILE_NAME, FilePathUtil.XML_EXTNOSEP, SuiteSettings.Data.SiteRootPath));

			WriteLine(logMsgDbS(" fav manager| file path| ", favMgr.FilePath.FullFilePath));
			WriteLine(logMsgDbS(" fav manager| file exists| ", favMgr.FilePath.FullFilePath));

			WriteLine(logMsgDbS("hist manager| file path| ", histMgr.FilePath.FullFilePath));
			WriteLine(logMsgDbS("hist manager| file exists| ", histMgr.FilePath.FullFilePath));

			
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