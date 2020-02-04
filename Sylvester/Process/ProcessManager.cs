#region + Using Directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Sylvester.FileSupport;
using Sylvester.FolderSupport;
using Sylvester.Settings;
using UtilityLibrary;

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

	public enum FolderType
	{
		UNASSIGNED = -1,
		CURRENT  = 0,
		REVISION = 1,
		FINAL = 2
	}

	// adjust to use generics - for (2) or (3)
	// generic class would have
	// T = FileCurrent / FileRevision / FileFinal  where T : SheetNameInfo
	// file collection of <T> 
	// folder manager (folder type) (needs access to header control)
	// ICollectionView of the file collection<T>
	// readfiles
	//
	// will need to move these to each generic:
	// public: reset(), read...()
	// private: create collection view

	// will need to move these to a support class and provide to each
	// change case(), readfail()
	
	// needs multilayered abstract classes:
	// 
	// final only needs:
	// file collection, Icollectionview, reset, config, but not the rest.
	// current & revision needs all except config final

	// stays in the manager class:
	// compare, renamefiles


	public class ProcessManager : INotifyPropertyChanged
	{
	#region private fields

		private readonly FolderManager _fmCurrent;
		private readonly FolderManager _fmRevision;
		private ReadFiles readFilesCurrent;
		private ReadFiles readFilesRevision;

	#endregion

		public ProcessManager() { }

		public ProcessManager(HeaderControl hcCurrent, HeaderControl hcRevision)
		{
			_fmCurrent = new FolderManager(FolderType.CURRENT.Value(), hcCurrent);
			_fmCurrent.FolderChange += OnCurrentFolderChange;

			_fmRevision = new FolderManager(FolderType.REVISION.Value(), hcRevision);
			_fmRevision.FolderChange += OnRevisionFolderChange;

			FileCollectionCurrent = new FilesCollection<FileCurrent>();
			FileCollectionRevision = new FilesCollection<FileRevision>();
			FileCollectionFinal = new FilesCollection<FileFinal>();

			Reset();

		}

	#region public properties

		public FilesCollection<FileCurrent> FileCollectionCurrent { get; private set; } = new FilesCollection<FileCurrent>();
		public FilesCollection<FileRevision> FileCollectionRevision { get; private set; } = new FilesCollection<FileRevision>();
		public FilesCollection<FileFinal> FileCollectionFinal { get; private set; } = new FilesCollection<FileFinal>();


		public ICollectionView CvCurrent { get; private set; }
		public ICollectionView CvRevision { get; private set; }
		public ICollectionView CvFinal { get; private set; }

		public bool UseExistingCaseShtTitle
		{
			get => SetgMgr.SheetTitleCase == SheetTitleCase.OK_AS_IS;
			set
			{
				if (value)
				{
					SetgMgr.SheetTitleCase =
						SheetTitleCase.OK_AS_IS;

					OnPropertyChange();

					caseChange();
				}
			}
		}

		public bool ForceUpperCaseShtTitle
		{
			get => SetgMgr.SheetTitleCase == SheetTitleCase.TO_UPPER_CASE;
			set
			{
				if (value)
				{
					SetgMgr.SheetTitleCase =
						SheetTitleCase.TO_UPPER_CASE;

					OnPropertyChange();

					caseChange();
				}
			}
		}

		public bool ForceWordCapShtTitle
		{
			get => SetgMgr.SheetTitleCase == SheetTitleCase.TO_CAP_EA_WORD;
			set
			{
				if (value)
				{
					SetgMgr.SheetTitleCase =
						SheetTitleCase.TO_CAP_EA_WORD;

					OnPropertyChange();

					caseChange();
				}
			}
		}

		public bool HasCurrentItems => !CvCurrent?.IsEmpty ?? false;

		public bool HasRevisionItems => !CvRevision?.IsEmpty ?? false;

	#endregion

	#region public methods

		public bool Compare()
		{
			ProcessFiles p = new ProcessFiles(FileCollectionCurrent, FileCollectionRevision);

			if ((FileCollectionFinal = p.Process()) == null) return false;

			configFinal();

			collectionViewFinal();

			return true;
		}

		public bool RenameFiles()
		{
			FileRenameManager frm = new FileRenameManager();

			if (!frm.RenameFiles(FileCollectionFinal)) return false;

			Reset();

			Read();

			Compare();

			return true;
		}

//		public void HistorySave()
//		{
//			fmCurrent.svfMgr
//
//
//		}

		public void Reset()
		{
			resetCurrent();
			resetRevision();
			resetFinal();
		}

		public bool Read()
		{
			if (!ReadCurrent()) return false;
			if (!ReadRevision()) return false;

			return true;
		}

		public bool ReadCurrent()
		{
			bool result = readFilesCurrent.GetFiles(_fmCurrent.Folder, false, FileCollectionCurrent);

			collectionViewCurrent();

			if (!result)
			{
				fileReadFail();
				return false;
			}

			return true;
		}

		public bool ReadRevision()
		{
			bool result = readFilesRevision.GetFiles(_fmRevision.Folder, true, FileCollectionRevision);

			collectionViewRevision();

			if (!result)
			{
				fileReadFail();
				return false;
			}

			return true;
		}

	#endregion

	#region private methods

		private void resetCurrent()
		{
			FileCollectionCurrent.Reset();
			FileCollectionCurrent.Name = "Current";
			FileCollectionCurrent.Folder = _fmCurrent.Folder;
			CvCurrent = null;
			readFilesCurrent = new ReadFiles();

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("FileCollectionCurrent");
		}

		private void resetRevision()
		{
			FileCollectionRevision.Reset();
			FileCollectionRevision.Name = "Revision";
			FileCollectionRevision.Folder = _fmRevision.Folder;
			CvRevision = null;
			readFilesRevision = new ReadFiles();

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("FileCollectionCurrent");
		}

		private void resetFinal()
		{
			FileCollectionFinal.Reset();
			configFinal();
			collectionViewFinal();

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("FileCollectionFinal");
		}

		private void configFinal()
		{
			FileCollectionFinal.Name = "Final";
			//			FileCollectionFinal.HideDirectory = true;
		}

		private void collectionViewCurrent()
		{
			CvCurrent = CollectionViewSource.GetDefaultView(FileCollectionCurrent.Files);

			CvCurrent.SortDescriptions.Add(new SortDescription("AdjustedSheetId", ListSortDirection.Ascending));

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("CvCurrent");
		}

		private void collectionViewRevision()
		{
			CvRevision = CollectionViewSource.GetDefaultView(FileCollectionRevision.Files);

			CvRevision.SortDescriptions.Add(new SortDescription("AdjustedSheetId", ListSortDirection.Ascending));

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("CvRevision");
		}

		private void collectionViewFinal()
		{
			CvFinal = CollectionViewSource.GetDefaultView(FileCollectionFinal.Files);

			CvFinal.SortDescriptions.Add(new SortDescription("AdjustedSheetId", ListSortDirection.Ascending));

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("CvFinal");
		}

		private void caseChange()
		{
			// step 1: change the case of base
			// step 2: if comparison done, redo
			foreach (FileCurrent bf in FileCollectionCurrent.Files)
			{
				if (bf.FileType != FileType.SHEET_PDF) continue;

				bf.UpdateSheetTitleCase();
			}

			collectionViewFinal();

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("pm");

		}

		private void fileReadFail()
		{
			CsMessageBox.CsMessageBox b = new CsMessageBox.CsMessageBox("No PDF's have been selected");

			// ReSharper disable once UnusedVariable
			MessageBoxResult r = b.Show();

		}

	#endregion

	#region event handling

		public void OnCurrentFolderChange(object sender, EventArgs e)
		{
			FileCollectionCurrent.Folder = _fmCurrent.Folder;
		}
		
		public void OnRevisionFolderChange(object sender, EventArgs e)
		{
			FileCollectionRevision.Folder = _fmRevision.Folder;
		}

		public event PropertyChangedEventHandler PropertyChanged;


		// ReSharper disable once InconsistentNaming
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}
}