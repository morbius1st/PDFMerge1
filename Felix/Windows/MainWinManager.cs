#region + Using Directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Felix.FileListManager;
using Felix.Settings;
using Felix.Support;
using SettingManager;
using static UtilityLibrary.MessageUtilities;

#endregion


// projname: Tests2
// itemname: MainWinManager
// username: jeffs
// created:  11/12/2019 10:02:36 PM


namespace Felix.Windows
{
	public class MainWinManager : INotifyPropertyChanged
	{
		public static MainWindow mainWin = null;

		public FileListMgr flMgr = null;


		public MainWinManager()
		{
			Initalize();

			MessageClear();
		}

		private void Initalize()
		{
			bool result = false;

			mainWin = new MainWindow();

			flMgr = new FileListMgr();
		}

		public void Start()
		{
			FileListMgr.BaseFolder = new Route(UserSettings.Data.PriorFolder);

			flMgr.CreateFileList();
			if (!flMgr.IsInitialized) return;

			UserSettings.Data.PriorFolder = FileListMgr.BaseFolder.FullPath;

			ShowMainWin();
		}

		private void ShowMainWin()
		{
			mainWin.ShowDialog();
		}

		public static void MessageClear()
		{
			mainWin.tbxMsg.Clear();
		}

		public static void MessageAppend(string message)
		{
			mainWin.tbxMsg.AppendText(message);
		}

		public static void MessageAppendLine(string message)
		{
			MessageAppend(message + nl);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
