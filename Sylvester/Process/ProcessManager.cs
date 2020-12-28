#region + Using Directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using SettingsManager;
using Sylvester.FileSupport;
using Sylvester.FolderSupport;
using Sylvester.SavedFolders;
// using Sylvester.Settings;
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

	// functional where and what
	// at mainwin | press history (either type) ->
	//		| user selects history entry -> update folder routes
	//
	// at mainwin | press saved folder (either type) ->
	//		| user selects a saved folder -> update folder routes
	// 
	// at mainwin | press add saved folder ->
	//		| user selects a history entry -> save to saved folders
	//		| user selects new saved folder -> create and save a new saved folder
	//
	// at mainwin | press rename ->
	//		| have saved folder create and save current as a history entry


	public class ProcessManager : INotifyPropertyChanged
	{

	#region private fields
		private ICollectionView cvRevision;
		private ICollectionView cvCurrent;

		private ReadFiles readFilesCurrent;
		private ReadFiles readFilesRevision;

		private SavedFolderManager history;
		private SavedFolderManager favorites;

		private FilesCollection<FileFinal> fileCollectionFinal = new FilesCollection<FileFinal>();

	#endregion

		public ProcessManager() { //}

			FileCollectionCurrent = new FilesCollection<FileCurrent>();
			FileCollectionRevision = new FilesCollection<FileRevision>();
			FileCollectionFinal = new FilesCollection<FileFinal>();

			history = SavedFolderManager.GetHistoryManager();
			favorites = SavedFolderManager.GetFavoriteManager();

			Reset();
		}

	#region public properties

		public FilesCollection<FileCurrent> FileCollectionCurrent { get; private set; } = new FilesCollection<FileCurrent>();
		public FilesCollection<FileRevision> FileCollectionRevision { get; private set; } = new FilesCollection<FileRevision>();


		public FilesCollection<FileFinal> FileCollectionFinal
		{
			get => fileCollectionFinal;
			private set
			{
				fileCollectionFinal = value;
				OnPropertyChange();
				fileCollectionFinal.Update();
			}
		}


		public ICollectionView CvCurrent
		{
			get => cvCurrent;
			private set
			{
				cvCurrent = value;
				OnPropertyChange();
				OnPropertyChange("HasCurrentItems");
			}
		}

		public ICollectionView CvRevision
		{
			get => cvRevision;
			private set
			{
				cvRevision = value;
				OnPropertyChange();
				OnPropertyChange("HasRevisionItems");
			}
		}

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

		public void FolderChanged(FolderType ft, FilePath<FileNameSimple> folder)
		{
			switch (ft)
			{
			case FolderType.CURRENT:
				{
					FileCollectionCurrent.Folder = folder;
					resetCurrent();
					resetFinal();
					break;
				}
			case FolderType.REVISION:
				{
					FileCollectionRevision.Folder  = folder;
					resetRevision();
					resetFinal();
					break;
				}
			}
		}

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

			history.AddToSavedFolders(FileCollectionCurrent.Folder, FileCollectionRevision.Folder);

			Reset();

			Read();

			Compare();

			return true;
		}

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
			bool result = readFilesCurrent.GetFiles( FileCollectionCurrent, false, FileCollectionCurrent.Folder);

			collectionViewCurrent();

			if (!result)
			{
//				fileReadFail();
				return false;
			}

			return true;
		}

		public bool ReadRevision()
		{
			bool result = readFilesRevision.GetFiles( FileCollectionRevision, true, FileCollectionRevision.Folder);

			collectionViewRevision();

			if (!result)
			{
//				fileReadFail();
				return false;
			}

			return true;
		}

		public bool SaveToFavorites()
		{
			return favorites.AddToSavedFolders(FileCollectionCurrent.Folder, FileCollectionRevision.Folder);
		}

	#endregion

	#region private methods

		private void resetCurrent()
		{
			FileCollectionCurrent.Initialize();
			FileCollectionCurrent.Name = "Current";
//			FileCollectionCurrent.Folder = _fmCurrent.Folder;
			CvCurrent = null;
			readFilesCurrent = new ReadFiles();

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("HasCurrentItems");
			OnPropertyChange("FileCollectionCurrent");
		}

		private void resetRevision()
		{
			FileCollectionRevision.Initialize();
			FileCollectionRevision.Name = "Revision";
//			FileCollectionRevision.Folder = _fmRevision.Folder;
			CvRevision = null;
			readFilesRevision = new ReadFiles();

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("HasRevisionItems");
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

//		public void OnCurrentFolderChange(object sender, EventArgs e)
//		{
//			FileCollectionCurrent.Folder = _fmCurrent.Folder;
//		}
//		
//		public void OnRevisionFolderChange(object sender, EventArgs e)
//		{
//			FileCollectionRevision.Folder = _fmRevision.Folder;
//		}

		public event PropertyChangedEventHandler PropertyChanged;


		// ReSharper disable once InconsistentNaming
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}
}