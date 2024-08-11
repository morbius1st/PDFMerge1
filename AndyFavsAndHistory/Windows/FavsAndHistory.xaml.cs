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

using AndyShared.Support;

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

		private const string FAV_HIST_FOLDER = "Favs And History";

		private const string FAV_FILE_NAME = "Favorite-List";
		private const string HIST_FILE_NAME = "History-List";

		private string message;
		private string userName;

		// private Orator.ConfRoom.Announcer OnConfigAnnounce;

		private  FavHistoryManager favMgr;
		private  FavHistoryManager histMgr;

	#endregion

	#region ctor

		public FavsAndHistory()
		{
			InitializeComponent();

			WriteLine(logMsgDbS("status", "@ ctor: FavsAndHistory"));

		}


	#endregion

	#region public properties

		public string Message
		{
			get => message;
			set
			{
				message = value;
				OnPropertyChange();
			}
		}

		public string UserName
		{
			get => userName;
			set
			{
				userName = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure(string userName)
		{
			WriteLine(logMsgDbS("status", "@ Configure"));

			UserName = userName;

			configFav();

			histMgr = new FavHistoryManager();
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

		private void configFav()
		{
			favMgr = new FavHistoryManager();

			favMgr.Configure(new FilePath<FileNameSimple>(SuiteSettings.Path.SettingFolderPath), 
				FAV_FILE_NAME, new [] {FAV_HIST_FOLDER, userName});

			Samples.SampleData.FavSamples(favMgr);

		}

		private void showTestInfo()
		{
			WriteLine(logMsgDbS("status", "@ showTestInfo"));

			SuiteSettings.Admin.Read();

			WriteLine(logMsgDbS("Site|         description", SuiteSettings.Info.Description));
			WriteLine(logMsgDbS("Site|    sett'g file path", SuiteSettings.Path.SettingFilePath));
			WriteLine(logMsgDbS("Site|  sett'g folder path", SuiteSettings.Path.SettingFolderPath));
			WriteLine(logMsgDbS("Site| sett'g file exists?", SuiteSettings.Path.Exists.ToString()));

			WriteLine("");
			WriteLine(logMsgDbS("user name", userName ?? "is null"));

			showFavListInfo(favMgr);

			showHistListInfo(histMgr);
		}

		private void showFavListInfo(FavHistoryManager favMgr)
		{
			if (favMgr != null)
			{
				WriteLine("");
			
				favMgr.Configure(new FilePath<FileNameSimple>(SuiteSettings.Path.SettingFolderPath), 
					FAV_FILE_NAME, new [] {FAV_HIST_FOLDER, userName});
			
				WriteLine(logMsgDbS(" fav manager| file exists", favMgr.FilePath.Exists.ToString()));
				WriteLine(logMsgDbS(" fav manager| file path", favMgr.FilePath.FullFilePath));
			}
		}

		private void showHistListInfo(FavHistoryManager histMgr)
		{
			if (histMgr != null)
			{
				WriteLine("");

				histMgr.Configure(new FilePath<FileNameSimple>(SuiteSettings.Path.SettingFolderPath), 
					HIST_FILE_NAME, new [] {FAV_HIST_FOLDER, userName});

				WriteLine(logMsgDbS(" hist manager| file exists", histMgr.FilePath.Exists.ToString()));
				WriteLine(logMsgDbS(" hist manager| file path", histMgr.FilePath.FullFilePath));
			}
		}






	#endregion

	#region event consuming

		private void winFavs_Loaded(object sender, RoutedEventArgs e)
		{
			WriteLine(logMsgDbS("status", "@ winFavs_Loaded"));

			showTestInfo();
		}

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