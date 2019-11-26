#region + Using Directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Felix.OutlineManager;
using Felix.Support;
using Felix.Windows;

#endregion


// projname: Tests2.FileListManager
// itemname: SelectFilesMgr
// username: jeffs
// created:  11/16/2019 6:26:29 AM


namespace Felix.FileListManager
{
	public class FileListMgr : INotifyPropertyChanged
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

			sFil.GetFileList();

			olm.Initalize();

			int unMatched = olm.ApplyOutlineSettings();

			if (unMatched > 0)
			{
				MainWinManager.MessageAppendLine("");

				foreach (string s in olm.UnMatched)
				{
					MainWinManager.MessageAppendLine("unmatched| " + s);
				}
			}

			return true;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}
}
