﻿#region + Using Directives

using Tests2.DebugSupport;
using Tests2.FileListManager;
using Tests2.OutlineManager;
using Tests2.Settings;

using static UtilityLibrary.MessageUtilities;

#endregion


// projname: Tests2
// itemname: MainWinManager
// username: jeffs
// created:  11/12/2019 10:02:36 PM


namespace Tests2.Windows
{
	public class MainWinManager
	{
		public static MainWindow mainWin = null;

		public FileListMgr flMgr = null;



		public MainWinManager()
		{
			Initalize();

			mainWin.tbkUL.Clear();
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

		#if DEBUG
			flMgr.CreateFileList();
//			DebugHelper.Prime.FolderMgrStatus(flMgr.IsInitialized, flMgr);
			if (!flMgr.IsInitialized) return;
		#else
			if (!sFilMgr.SelectFiles(SettingsMgr.Instance.GetInitFolder())) return;
		#endif

			UserSettings.Data.PriorFolder = FileListMgr.BaseFolder.FullPath;

			ShowMainWin();
		}

		private void ShowMainWin()
		{
			mainWin.ShowDialog();
		}

		public static void MessageClear()
		{
			mainWin.tbkUL.Clear();
		}

		public static void MessageAppend(string message)
		{
			mainWin.tbkUL.AppendText(message);
		}

		public static void MessageAppendLine(string message)
		{
			MessageAppend(message + nl);
		}
	}
}