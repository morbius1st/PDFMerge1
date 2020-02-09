#region + Using Directives
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Sylvester.FileSupport;
using Sylvester.Process;
using Sylvester.SavedFolders;
using Sylvester.Settings;
using Sylvester.UserControls;
using UtilityLibrary;

using static Sylvester.SavedFolders.SavedFolderType;
using static Sylvester.Process.FolderType;
#endregion


// projname: Sylvester.FolderSupport
// itemname: FolderManager2
// username: jeffs
// created:  2/4/2020 10:39:15 PM






namespace Sylvester.FolderSupport
{
	public class FolderManager2
	{
		private SelectFolder sf = new SelectFolder();

		public SavedFolderManager[] svfMgr = new SavedFolderManager[COUNT.Value()];

		private HeaderControl[] hc;
		private Route[] folder;

		private HeaderControl hcCurrent;
		private HeaderControl hcRevision;

		private Route folderCurrent;
		private Route folderRevision;

		public FolderManager2(HeaderControl hcCurrent, HeaderControl hcRevision)
		{
			hc[CURRENT.Value()] = hcCurrent;
			hc[REVISION.Value()] = hcRevision;

			hc[CURRENT.Value()].FolderRoute.PathChange += OnCurrentPathChangeEvent;

			this.hcCurrent = hcCurrent;
			this.hcRevision = hcRevision;

			hcCurrent.FolderRoute.PathChange += OnCurrentPathChangeEvent;
		
//			hcCurrent.SetPathChangeEventHandler(OnRevisionPathChangeEvent);
//			hcCurrent.SetSelectFolderEventHandler(OnRevisionSelectFolderEvent);
//			hcCurrent.SetFavoritesEventHandler(OnRevisionFavoriteEvent);
//			hcCurrent.SetHistoryEventHandler(OnRevisionHistoryEvent);
//
//			hcRevision.SetPathChangeEventHandler(OnRevisionPathChangeEvent);
//			hcRevision.SetSelectFolderEventHandler(OnRevisionSelectFolderEvent);
//			hcRevision.SetFavoritesEventHandler(OnRevisionFavoriteEvent);
//			hcRevision.SetHistoryEventHandler(OnRevisionHistoryEvent);

			ConfigSavedFolders();

			SavedFoldersDebugSupport.Instance.
				ConfigSavedFoldersDebugSupport(svfMgr[HISTORY.Value()], svfMgr[FAVORITES.Value()]);

			getPriorFolder();

			configHeader(this.hcCurrent);
			configHeader(this.hcRevision);
		}

		private void ConfigSavedFolders()
		{
			if (svfMgr[0] == null)
			{
				svfMgr[HISTORY.Value()] = new SavedFolderManager(HISTORY);

				svfMgr[FAVORITES.Value()] =new SavedFolderManager(FAVORITES);
			}
		}

		public Route CurrentFolder
		{
			get => folderCurrent;
			set
			{
				folderCurrent = value;

				SetgMgr.SetPriorFolder(FolderType.CURRENT.Value(), folderCurrent);

				RaiseFolderChangeEvent();
			}
		}

		public Route RevisionFolder
		{
			get => folderRevision;
			set
			{
				folderRevision = value;

				SetgMgr.SetPriorFolder(FolderType.CURRENT.Value(), folderRevision);

				RaiseFolderChangeEvent();
			}
		}

		public bool HasPriorFolderCurrent => CurrentFolder.IsValid;
		
		public bool HasPriorFolderRevision => RevisionFolder.IsValid;

		private void configHeader(HeaderControl hc)
		{
			int folderPathType;

			// always show select folder
			folderPathType = ObliqueButtonType.SELECTFOLDER.Value();

			if (HasPriorFolderCurrent)
			{
				folderPathType += ObliqueButtonType.TEXT.Value();
			}

			folderPathType += svfMgr[HISTORY.Value()].HasSavedFolders ? ObliqueButtonType.HISTORY.Value() : 0;
			folderPathType += svfMgr[FAVORITES.Value()].HasSavedFolders ? ObliqueButtonType.FAVORITES.Value() : 0;

			hc.FolderPathType = folderPathType;
		}

		private void tempGetPriorFolder()
		{
				tempPriorCurrentFolder();
				tempPriorRevisionFolder();
//
//			if (index == 0)
//			{
//			}
//			else
//			{
//			}
		}

		private void tempPriorCurrentFolder()
		{
			SetgMgr.SetPriorFolder(FolderType.CURRENT.Value(),
				@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Base");
		}

		private void tempPriorRevisionFolder()
		{
			SetgMgr.SetPriorFolder(FolderType.REVISION.Value(),
				@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Test");
		}


		private void getPriorFolder()
		{
			CurrentFolder = SetgMgr.GetPriorFolder(hcCurrent.FolderPathType);

			hcCurrent.Path = CurrentFolder;

			RevisionFolder = SetgMgr.GetPriorFolder(hcRevision.FolderPathType);

			hcRevision.Path = CurrentFolder;


		}

		private void SelectFolder()
		{
//			Folder = sf.GetFolder(Folder);
//			if (!Folder.IsValid) return;

			tempGetPriorFolder();

			CurrentFolder = SetgMgr.GetPriorFolder(FolderType.CURRENT.Value());
			SetgMgr.SetPriorFolder(FolderType.CURRENT.Value(), CurrentFolder);
			hcCurrent.Path = CurrentFolder;
			configHeader(hcCurrent);

			RevisionFolder = SetgMgr.GetPriorFolder(FolderType.REVISION.Value());
			SetgMgr.SetPriorFolder(FolderType.REVISION.Value(), RevisionFolder);
			hcRevision.Path = RevisionFolder;
			configHeader(hcRevision);

		}

	#region event handling




		public delegate void FolderChangeEventHandler(object sender, EventArgs e);

		public event FolderManager.FolderChangeEventHandler FolderChange;

		protected virtual void RaiseFolderChangeEvent()
		{
			FolderChange?.Invoke(this, new EventArgs());
		}

		private void OnCurrentPathChangeEvent(object sender, PathChangeArgs e)
		{
			Debug.WriteLine("folderManager| Current path changed");
			Debug.WriteLine("folderManager| index     | " + e.Index);
			Debug.WriteLine("folderManager| sel folder| " + e.SelectedFolder);
			Debug.WriteLine("folderManager| sel path  | " + e.SelectedPath.FullPath);

			hcCurrent.Path = e.SelectedPath;
			CurrentFolder = hcCurrent.Path;
		}

		private void OnCurrentSelectFolderEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Current Select Folder");

			SelectFolder();
		}

		private void OnCurrentFavoriteEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Current Favorites");

		}

		private void OnCurrentHistoryEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Current History");
		}


		private void OnRevisionPathChangeEvent(object sender, PathChangeArgs e)
		{
			Debug.WriteLine("folderManager, Revision path changed");
			Debug.WriteLine("folderManager| index     | " + e.Index);
			Debug.WriteLine("folderManager| sel folder| " + e.SelectedFolder);
			Debug.WriteLine("folderManager| sel path  | " + e.SelectedPath.FullPath);

			hcCurrent.Path = e.SelectedPath;
			CurrentFolder = hcCurrent.Path;
		}

		private void OnRevisionSelectFolderEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Revision Select Folder");

			SelectFolder();
		}

		private void OnRevisionFavoriteEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Revision Favorites");

		}

		private void OnRevisionHistoryEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Revision History");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	}
}
