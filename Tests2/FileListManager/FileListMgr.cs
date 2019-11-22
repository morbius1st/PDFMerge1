#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests2.OutlineManager;
using Tests2.Windows;

using static UtilityLibrary.MessageUtilities;

#endregion


// projname: Tests2.FileListManager
// itemname: SelectFilesMgr
// username: jeffs
// created:  11/16/2019 6:26:29 AM


namespace Tests2.FileListManager
{
	public class FileListMgr
	{
		private static readonly FileListMgr instance = new FileListMgr();

		public static FileListMgr Instance => instance;

		static FileListMgr() { }

		private SelectFolder sFld = new SelectFolder();
		private SelectFiles sFil = new SelectFiles();
		private OutlineMgr olm = new OutlineMgr();

		private static Route baseFolder = Route.Invalid;

		public static Route BaseFolder
		{
			get => baseFolder;
			set => baseFolder = value;
		}

		public bool IsInitialized { get; private set; }

		public bool CreateFileList()
		{
			if (!baseFolder.IsValid)
			{
				baseFolder = new Route("");
			}

			BaseFolder = sFld.GetFolder(baseFolder);

			IsInitialized = baseFolder.IsValid;

			if (!baseFolder.IsValid) return false;

			FileItem.RootPath= baseFolder;

			olm.Initalize();

			return sFil.GetFileList();
		}

	}
}
