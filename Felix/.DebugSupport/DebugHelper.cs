#region + Using Directives

using Felix.FileListManager;
using Felix.Support;
using Felix.Windows;
using static UtilityLibrary.MessageUtilities;

#endregion


// projname: Tests2.Debug
// itemname: DebugHelper
// username: jeffs
// created:  11/16/2019 7:50:36 AM


namespace Felix.DebugSupport
{
	public class DebugHelper
	{
		private static readonly DebugHelper instance = new DebugHelper();

		public static DebugHelper Prime => instance;

		static DebugHelper() { }

		public void FolderMgrStatus(bool status, FileListMgr sfiMgr)
		{
			if (status)
			{
				MainWinManager.MessageAppend("full route | " + FileListMgr.BaseFolder.FullPath + nl);
				MainWinManager.MessageAppend("initalized?| " + sfiMgr.IsInitialized + nl);
			}
			else
			{
				MainWinManager.MessageAppend("full route | failed / not initialized" + nl);
			}
		}

		public void ListRoute(Route r)
		{
			Route x = r.SubPath(FileItem.RootPath);

			MainWinManager.MessageAppend("\nRoute|");
			MainWinManager.MessageAppend("Route|       FullPath| " + x.FullPath);
			MainWinManager.MessageAppend("Route|       IsRouted| " + x.IsRooted);
			MainWinManager.MessageAppend("Route|       RootPath| " + x.RootPath);

		}

	}
}
