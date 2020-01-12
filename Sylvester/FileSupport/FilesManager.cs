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

		public FilesCollection<BaseFile>  BaseFileColl { get; private set; }
		public FilesCollection<BaseFile>  TestFileColl { get; private set; }
		public FilesCollection<TestFile>  FinalFileColl { get; private set; }

//		public SelectFiles<BaseFile> BaseFiles { get; private set; }
//		public SelectFiles<SheetIdTest> TestFiles { get; private set; }

		public ReadFiles BaseReadFiles { get; private set; }
		public ReadFiles TestReadFiles { get; private set; }

		public ICollectionView cvBase { get; private set; }
		public ICollectionView cvTest { get; private set; }

		public ICollectionView cvFinal { get; private set; }

		SelectFolder.SelectFolder sf = new SelectFolder.SelectFolder();

		private Route baseFolder;
		private Route testFolder;

		private bool ByPass = true;

		public FilesManager()
		{
			BaseFileColl = new FilesCollection<BaseFile>();
			TestFileColl = new FilesCollection<BaseFile>();
			FinalFileColl = new FilesCollection<TestFile>();

			Reset();

		}

		public void Reset()
		{
			BaseFileColl.Reset();
			BaseFileColl.Name = "Base";

			TestFileColl.Reset();
			TestFileColl.Name = "Test";

			FinalFileColl.Reset();
			FinalFileColl.Name = "Final";
			CollectionViewFinal();

			BaseReadFiles = new ReadFiles();
			TestReadFiles = new ReadFiles();

			cvBase = null;
			cvTest = null;

		}

		public bool Read()
		{
			BaseFileColl.Directory = FolderManager.BaseFolder;

			if (!BaseReadFiles.GetFiles<BaseFile>(FolderManager.BaseFolder, false, BaseFileColl)) return false;

			CollectionViewBase();

			TestFileColl.Directory = FolderManager.TestFolder;

			if (!TestReadFiles.GetFiles<BaseFile>(FolderManager.TestFolder, true, TestFileColl)) return false;

			CollectionViewTest();

			return true;
		}

		public void CollectionViewBase()
		{
			cvBase = CollectionViewSource.GetDefaultView(BaseFileColl.TestFiles);

			cvBase.SortDescriptions.Add(new SortDescription("AdjustedSheetID", ListSortDirection.Ascending));

			OnPropertyChange("cvBase");
		}

		public void CollectionViewTest()
		{
			cvTest = CollectionViewSource.GetDefaultView(TestFileColl.TestFiles);

			cvTest.SortDescriptions.Add(new SortDescription("AdjustedSheetID", ListSortDirection.Ascending));

			OnPropertyChange("cvTest");
		}

		public void CollectionViewFinal()
		{
			cvFinal = CollectionViewSource.GetDefaultView(FinalFileColl.TestFiles);

			cvFinal.SortDescriptions.Add(new SortDescription("AdjustedSheetID", ListSortDirection.Ascending));

			OnPropertyChange("cvFinal");
		}

		public bool Process()
		{
			ProcessFiles p = new ProcessFiles(BaseFileColl, TestFileColl);

			if ((FinalFileColl = p.Process()) == null) return false;

			CollectionViewFinal();
//
//			cvFinal = CollectionViewSource.GetDefaultView(FinalFileColl.TestFiles);
//
//			cvFinal.SortDescriptions.Add(new SortDescription("AdjustedSheetID", ListSortDirection.Ascending));
//		
//			OnPropertyChange("cvFinal");

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