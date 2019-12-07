#region + Using Directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tests2.FileListManager;
using Tests2.PDFMergeManager;
using Tests2.Settings;

using static UtilityLibrary.MessageUtilities;

#endregion


// projname: Tests2
// itemname: MainWinManager
// username: jeffs
// created:  11/12/2019 10:02:36 PM


namespace Tests2.Windows
{
	public class MainWinManager : INotifyPropertyChanged
	{
		public static MainWindow mainWin = null;

		public FileListMgr FileLstMgr = null;

		public PDFMergeMgr MrgMgr = null;

		public MainWinManager()
		{
			Initalize();

			mainWin.tbkUL.Clear();
		}

		private void Initalize()
		{
#pragma warning disable CS0219 // The variable 'result' is assigned but its value is never used
			bool result = false;
#pragma warning restore CS0219 // The variable 'result' is assigned but its value is never used

			mainWin = new MainWindow();

			FileLstMgr = FileListMgr.Instance;

			MrgMgr = PDFMergeMgr.Instance;
		}

		public void Start()
		{
			FileListMgr.BaseFolder = new Route(UserSettings.Data.PriorFolder);

			FileLstMgr.CreateFileList();
			if (!FileLstMgr.IsInitialized) return;

			UserSettings.Data.PriorFolder = FileListMgr.BaseFolder.FullPath;

			MrgMgr.CreateMergeList();

			MrgMgr.ListTree();

			OnPropertyChange("MrgMgr");

			ShowMainWin();
		}

		private void ShowMainWin()
		{
			mainWin.ShowDialog();
		}

		public static void MessageClear()
		{
			mainWin.SendClearMessage();
		}

		public static void MessageAppend(string message)
		{
			mainWin.SendMessage(message);
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
