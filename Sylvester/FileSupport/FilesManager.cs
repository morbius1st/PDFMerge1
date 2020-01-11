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
//		public static FilesCollection<TestFile> tfc { get; private set; }

		public FilesCollection<TestFile> TestFileColl { get; private set; }
		public FilesCollection<BaseFile>  BaseFileColl { get; private set; }

//		public SelectFiles<BaseFile> BaseFiles { get; private set; }
//		public SelectFiles<SheetIdTest> TestFiles { get; private set; }

		public ReadFiles BaseReadFiles { get; private set; }
		public ReadFiles TestReadFiles { get; private set; }

		public ICollectionView cvTest { get; private set; }
		public ICollectionView cvBase { get; private set; }

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
//			BaseFiles = new SelectFiles<BaseFile>();
//			TestFiles = new SelectFiles<SheetIdTest>();

			TestFileColl = new FilesCollection<TestFile>();
			TestFileColl.Name = "Test";

			BaseFileColl = new FilesCollection<BaseFile>();
			BaseFileColl.Name = "Test";

			BaseReadFiles = new ReadFiles();
			TestReadFiles = new ReadFiles();

			cvTest = null;
			cvBase = null;

		}

		public bool Read()
		{
			TestFileColl.Directory = FolderManager.TestFolder;

			if (!TestReadFiles.GetFiles<TestFile>(FolderManager.TestFolder, TestFileColl)) return false;

			BaseFileColl.Directory = FolderManager.BaseFolder;

			if (!BaseReadFiles.GetFiles<BaseFile>(FolderManager.BaseFolder, BaseFileColl)) return false;

			CollectionViews();

			return true;
		}

		public void CollectionViews()
		{
			cvTest = CollectionViewSource.GetDefaultView(TestFileColl.TestFiles);
			OnPropertyChange("cvTest");

			cvBase = CollectionViewSource.GetDefaultView(BaseFileColl.TestFiles);
			OnPropertyChange("cvBase");
		}

		public bool Process()
		{
			ProcessFiles p = new ProcessFiles(BaseFileColl, TestFileColl);

			return true;
		}


//		public bool Process()
//		{
//			if (!GetFiles()) return false;
//
//			ProcessFiles pf = new ProcessFiles(BaseFiles, TestFiles, TestFileColl);
//
//			pf.Process2();
//
//			cvBase.SortDescriptions.Add(new SortDescription(nameof(TestFile.SheetNumber), ListSortDirection.Ascending));
//
//			return pf.Process();
//		}
//
//		private bool GetFiles()
//		{
//			ReadFiles rf = new ReadFiles();
//
//			rf.GetFiles<TestFile>(FolderManager.TestFolder, TestFileColl);
//			cvBase = CollectionViewSource.GetDefaultView(TestFileColl.TestFiles);
//			OnPropertyChange("cvBase");
//
//			if (!BaseFiles.GetFiles(FolderManager.BaseFolder)) return false;
//
//			if (!TestFiles.GetFiles(FolderManager.TestFolder)) return false;
//
//			cvTest = CollectionViewSource.GetDefaultView(TestFiles.SheetFiles.Files);
//			OnPropertyChange("cvTest");
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