#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Sylvester.Process;
using Sylvester.SelectFolder;
using Sylvester.Settings;

#endregion


// projname: Sylvester.FileSupport
// itemname: FilesManager
// username: jeffs
// created:  12/31/2019 10:57:47 PM


namespace Sylvester.FileSupport
{
	public class FilesManager : INotifyPropertyChanged
	{
		public SelectFiles<SheetIdBase> BaseFiles { get; private set; }
		public SelectFiles<SheetIdTest> TestFiles { get; private set; }

		public ICollectionView cv { get; private set; }

		SelectFolder.SelectFolder sf = new SelectFolder.SelectFolder();

		private Route baseFolder;
		private Route testFolder;

		private bool ByPass = true;

		public FilesManager()
		{
			BaseFiles = new SelectFiles<SheetIdBase>();
			TestFiles = new SelectFiles<SheetIdTest>();
		}

		public bool Process()
		{
			if (!GetFiles()) return false;

			return (new ProcessFiles(BaseFiles, TestFiles)).Process();
		}

		private bool GetFiles()
		{
//			if (!GetBaseFolder()) return false;

			if (!BaseFiles.GetFiles(FolderManager.BaseFolder)) return false;

//			if (!GetTestFolder()) return false;

			if (!TestFiles.GetFiles(FolderManager.TestFolder)) return false;

//			int count = BaseFiles.SheetFiles.Files.Count >
//				TestFiles.SheetFiles.Files.Count
//					? TestFiles.SheetFiles.Files.Count
//					: BaseFiles.SheetFiles.Files.Count;
//
//			for (int i = 0; i < count; i++)
//			{
//				BaseFiles.SheetFiles.Files[i].MatchedSheetNumber =
//					TestFiles.SheetFiles.Files[i].SheetNumber;
//				BaseFiles.SheetFiles.Files[i].MatchedSheetSeparation =
//					TestFiles.SheetFiles.Files[i].Separator;
//				BaseFiles.SheetFiles.Files[i].MatchedSheetName =
//					TestFiles.SheetFiles.Files[i].SheetName;
//			}

			cv = CollectionViewSource.GetDefaultView(TestFiles.SheetFiles.Files);

			return true;
		}
//
//		private bool GetBaseFolder()
//		{
//			baseFolder = new Route(UserSettings.Data.PriorBaseFolder);
//
//			if (!ByPass)
//			{
//				baseFolder = sf.GetFolder(baseFolder);
//				if (!baseFolder.IsValid) return false;
//
//				UserSettings.Data.PriorBaseFolder = baseFolder.FullPath;
//				UserSettings.Admin.Save();
//			}
//
//			return true;
//		}
//
//		private bool GetTestFolder()
//		{
//			testFolder = new Route(UserSettings.Data.PriorTestFolder);
//
//			if (!ByPass)
//			{
//				testFolder = sf.GetFolder(testFolder);
//				if (!testFolder.IsValid) return false;
//
//				UserSettings.Data.PriorTestFolder = testFolder.FullPath;
//				UserSettings.Admin.Save();
//			}
//
//			return true;
//		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}