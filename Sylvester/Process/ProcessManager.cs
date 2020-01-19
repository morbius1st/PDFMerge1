#region + Using Directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Sylvester.FileSupport;
using Sylvester.SelectFolder;
using Sylvester.Settings;

#endregion


// projname: Sylvester.FileSupport
// itemname: ProcessManager
// username: jeffs
// created:  12/31/2019 10:57:47 PM


namespace Sylvester.Process
{
	public enum SheetTitleCase
	{
		TO_UPPER_CASE,
		TO_CAP_EA_WORD,
		OK_AS_IS
	}

	public class ProcessManager : INotifyPropertyChanged
	{
		public FolderManager fmBase { get; private set; }

		public ProcessManager(FolderPath fp1)
		{
			fmBase = new FolderManager(fp1);

			BaseFileColl = new FilesCollection<BaseFile>();
			TestFileColl = new FilesCollection<TestFile>();
			FinalFileColl = new FilesCollection<FinalFile>();

			Reset();
		}

		public FilesCollection<BaseFile>  BaseFileColl { get; private set; }
		public FilesCollection<TestFile>  TestFileColl { get; private set; }
		public FilesCollection<FinalFile>  FinalFileColl { get; private set; }

		public ReadFiles BaseReadFiles { get; private set; }
		public ReadFiles TestReadFiles { get; private set; }

		public ICollectionView cvBase { get; private set; }
		public ICollectionView cvTest { get; private set; }

		public ICollectionView cvFinal { get; private set; }

		public bool UseExistingCaseShtTitle
		{
			get => UserSettings.Data.SheetTitleCase == SheetTitleCase.OK_AS_IS;
			set
			{
				if (value)
				{
					UserSettings.Data.SheetTitleCase =
						SheetTitleCase.OK_AS_IS;
					UserSettings.Admin.Save();

					OnPropertyChange();

					CaseChange();
				}
			}
		}

		public bool ForceUpperCaseShtTitle
		{
			get => UserSettings.Data.SheetTitleCase == SheetTitleCase.TO_UPPER_CASE;
			set
			{
				if (value)
				{
					UserSettings.Data.SheetTitleCase =
						SheetTitleCase.TO_UPPER_CASE;
					UserSettings.Admin.Save();

					OnPropertyChange();

					CaseChange();
				}
			}
		}

		public bool ForceWordCapShtTitle
		{
			get => UserSettings.Data.SheetTitleCase == SheetTitleCase.TO_CAP_EA_WORD;
			set
			{
				if (value)
				{
					UserSettings.Data.SheetTitleCase =
						SheetTitleCase.TO_CAP_EA_WORD;
					UserSettings.Admin.Save();

					OnPropertyChange();

					CaseChange();
				}
			}
		}

		public void Reset()
		{
			ResetBase();
			ResetTest();
			ResetFinal();
		}

		private void ResetBase()
		{
			BaseFileColl.Reset();
			BaseFileColl.Name = "Base";
			BaseReadFiles = new ReadFiles();
			BaseFileColl.Folder = fmBase.BaseFolder;
			cvBase = null;
		}

		private void ResetTest()
		{
			TestFileColl.Reset();
			TestFileColl.Name = "Test";
			TestReadFiles = new ReadFiles();
			TestFileColl.Folder = fmBase.TestFolder;
			cvTest = null;
		}

		private void ResetFinal()
		{
			FinalFileColl.Reset();
			ConfigFinal();
			CollectionViewFinal();
		}

		private void ConfigFinal()
		{
			FinalFileColl.Name = "Final";
			FinalFileColl.HideDirectory = true;
		}

		public bool Read()
		{
			if (!ReadBase()) return false;
			if (!ReadTest()) return false;

			return true;
		}

		public bool ReadBase()
		{
			

			if (!BaseReadFiles.GetFiles(fmBase.BaseFolder, false, BaseFileColl)) return false;

			CollectionViewBase();

			return true;
		}

		public bool ReadTest()
		{
			

			if (!TestReadFiles.GetFiles(fmBase.TestFolder, true, TestFileColl)) return false;

			CollectionViewTest();

			return true;
		}

		public void CollectionViewBase()
		{
			cvBase = CollectionViewSource.GetDefaultView(BaseFileColl.TestFiles);

			cvBase.SortDescriptions.Add(new SortDescription("AdjustedSheetId", ListSortDirection.Ascending));

			OnPropertyChange("cvBase");
		}

		public void CollectionViewTest()
		{
			cvTest = CollectionViewSource.GetDefaultView(TestFileColl.TestFiles);

			cvTest.SortDescriptions.Add(new SortDescription("AdjustedSheetId", ListSortDirection.Ascending));

			OnPropertyChange("cvTest");
		}

		public void CollectionViewFinal()
		{
			cvFinal = CollectionViewSource.GetDefaultView(FinalFileColl.TestFiles);

			cvFinal.SortDescriptions.Add(new SortDescription("AdjustedSheetId", ListSortDirection.Ascending));

			OnPropertyChange("cvFinal");
		}

		public bool Process()
		{
			ProcessFiles p = new ProcessFiles(BaseFileColl, TestFileColl);

			if ((FinalFileColl = p.Process()) == null) return false;
			ConfigFinal();
			CollectionViewFinal();

			return true;
		}

		public bool RenameFiles()
		{
			FileRenameManager frm = new FileRenameManager();

			if (!frm.RenameFiles(FinalFileColl)) return false;

			Reset();

			Read();

			Process();

			return true;
		}

		private void CaseChange()
		{
			// step 1: change the case of base
			// step 2: if comparison done, redo
			foreach (BaseFile bf in BaseFileColl.TestFiles)
			{
				if (bf.FileType != FileType.SHEET_PDF) continue;

				bf.UpdateSheetTitleCase();
			}

			CollectionViewFinal();

			OnPropertyChange("pm");

			

		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}