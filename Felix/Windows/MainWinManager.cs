#region + Using Directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Felix.FileListManager;
using Felix.OutlineManager;
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
	public class MainWinManager
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
			if (!Configure()) return;

			flMgr.CreateFileList();
			if (!flMgr.IsInitialized) return;

			UserSettings.Data.PriorFolder = FileListMgr.BaseFolder.FullPath;

			ShowMainWin();
		}

		private void ShowMainWin()
		{
			mainWin.ShowDialog();
		}

		private bool Configure()
		{
			UserSettings.Admin.Read();
			FileListMgr.BaseFolder = new Route(UserSettings.Data.PriorFolder);

			OutlineMgr.Instance.Initialize();

			OutlineMgr o = OutlineMgr.Instance;

			return true;
		}

	#region debug support

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

	#endregion

	}
}
