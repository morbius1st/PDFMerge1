#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tests2.FileListManager;
using Tests2.Windows;

using static UtilityLibrary.MessageUtilities;

#endregion


// projname: Tests2.Debug
// itemname: DebugHelper
// username: jeffs
// created:  11/16/2019 7:50:36 AM


namespace Tests2.DebugSupport
{
	public class DebugHelper
	{
		private static readonly DebugHelper instance = new DebugHelper();

		public static DebugHelper Instance => instance;

		static DebugHelper() { }

		public void FolderMgrStatus(bool status, FileListMgr sfiMgr)
		{
			if (status)
			{
				MainWinManager.mainWin.SendMessage("full route | " + FileListMgr.BaseFolder.FullPath + nl);
				MainWinManager.mainWin.SendMessage("initalized?| " + sfiMgr.IsInitialized + nl);
			}
			else
			{
				MainWinManager.mainWin.SendMessage("full route | failed / not initialized" + nl);
			}
		}

		public void ListRoute(Route r)
		{
			MainWindow w = MainWinManager.mainWin;

			Route x = r.SubPath(FileItem.RootPath);

			w.SendMessage("\nRoute|");
			w.SendMessage("Route|       FullPath| " + x.FullPath);
			w.SendMessage("Route|       IsRouted| " + x.IsRooted);
			w.SendMessage("Route|       RootPath| " + x.RootPath);

		}

	}
}
