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
		public TestFilesCollection TestFileColl { get; private set; }

		public ReadFiles rf;

		public SelectFiles<SheetIdBase> BaseFiles { get; private set; }
		public SelectFiles<SheetIdTest> TestFiles { get; private set; }

		public ICollectionView cv { get; private set; }
		public ICollectionView cv2 { get; private set; }

		SelectFolder.SelectFolder sf = new SelectFolder.SelectFolder();

		private Route baseFolder;
		private Route testFolder;

		private bool ByPass = true;

		public FilesManager()
		{
			Reset();
		}

		public void Reset()
		{
			BaseFiles = new SelectFiles<SheetIdBase>();
			TestFiles = new SelectFiles<SheetIdTest>();

			TestFileColl = new TestFilesCollection();

			rf = new ReadFiles(TestFileColl);

			cv = null;
			cv2 = null;

		}

		public bool Read()
		{
			rf.GetFiles(FolderManager.TestFolder);
			if (!BaseFiles.GetFiles(FolderManager.BaseFolder)) return false;
			return true;
		}

		public bool Process()
		{
			if (!GetFiles()) return false;

			ProcessFiles pf = new ProcessFiles(BaseFiles, TestFiles, TestFileColl);

			pf.Process2();

			cv2.SortDescriptions.Add(new SortDescription(nameof(TestFile.SheetNumber), ListSortDirection.Ascending));

			return pf.Process();
		}

		private bool GetFiles()
		{
			rf.GetFiles(FolderManager.TestFolder);
			cv2 = CollectionViewSource.GetDefaultView(TestFileColl.TestFiles);
			OnPropertyChange("cv2");

			if (!BaseFiles.GetFiles(FolderManager.BaseFolder)) return false;

			if (!TestFiles.GetFiles(FolderManager.TestFolder)) return false;

			cv = CollectionViewSource.GetDefaultView(TestFiles.SheetFiles.Files);
			OnPropertyChange("cv");

			return true;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}