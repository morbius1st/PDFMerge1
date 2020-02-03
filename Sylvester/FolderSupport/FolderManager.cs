#region + Using Directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Sylvester.FileSupport;
using Sylvester.SavedFolders;
using Sylvester.Settings;
using Sylvester.UserControls;
using UtilityLibrary;

using static Sylvester.SavedFolders.SavedFolderType;

#endregion


// projname: Sylvester.SelectFolder
// itemname: FolderManager
// username: jeffs
// created:  1/4/2020 9:30:29 PM


// general folder manager - works with either collection
namespace Sylvester.FolderSupport
{
	public class FolderManager : INotifyPropertyChanged //, IDisposable
	{
		private SelectFolder sf = new SelectFolder();

		public SavedFolderManager[] svfMgr = new SavedFolderManager[COUNT.Value()];

		private Route folderPath;

		private int index;

		private HeaderControl hcPath;

		public FolderManager(int index, HeaderControl hcPath)
		{
			this.index = index;

			this.hcPath = hcPath;

			hcPath.SetPathChangeEventHandler(onFolderPathPathChangeEvent);
			hcPath.SetSelectFolderEventHandler(onFolderPathSelectFolderEvent);
			hcPath.SetFavoritesEventHandler(onFolderPathFavoriteEvent);
			hcPath.SetHistoryEventHandler(onFolderPathHistoryEvent);

			svfMgr[HISTORY.Value()] = new SavedFolderManager(HISTORY);  //, SetgMgr.Instance.ProjectSavedFolders);

			svfMgr[FAVORITES.Value()] = new SavedFolderManager(FAVORITES);  //, SetgMgr.Instance.FavoritesSavedFolders);

			SavedFoldersDebugSupport.Instance.
				ConfigSavedFoldersDebugSupport(svfMgr[HISTORY.Value()], svfMgr[FAVORITES.Value()]);

			getPriorFolder();

			configHeader();
		}

		private Route folder;

		public Route Folder
		{
			get => folder;
			set
			{
				folder = value;

				SetgMgr.SetPriorFolder(index, folder);

				RaiseFolderChangeEvent();
			}
		}

		public bool HasPriorFolder => Folder.IsValid;

		private void configHeader()
		{
			int folderPathType;

			// always show select folder
			folderPathType = ObliqueButtonType.SELECTFOLDER.Value();

			if (HasPriorFolder)
			{
				folderPathType += ObliqueButtonType.TEXT.Value();
			}

			folderPathType += svfMgr[HISTORY.Value()].HasSavedFolders ? ObliqueButtonType.HISTORY.Value() : 0;
			folderPathType += svfMgr[FAVORITES.Value()].HasSavedFolders ? ObliqueButtonType.FAVORITES.Value() : 0;

			hcPath.FolderPathType = folderPathType;
		}


		public Route FolderPath
		{
			get => folderPath;
			set
			{
				folderPath = value;
				OnPropertyChange();
			}
		}

		private void tempGetPriorFolder()
		{
			if (index == 0)
			{
				tempPriorCurrentFolder();
			}
			else
			{
				tempPriorRevisionFolder();
			}
		}

		private void tempPriorCurrentFolder()
		{
			SetgMgr.SetPriorFolder(0,
				@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Base");
		}

		private void tempPriorRevisionFolder()
		{
			SetgMgr.SetPriorFolder(1,
				@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Test");
		}


		private void getPriorFolder()
		{
			Folder = SetgMgr.GetPriorFolder(index);

			hcPath.Path = Folder;
		}

		private void SelectFolder()
		{
//			Folder = sf.GetFolder(Folder);
//			if (!Folder.IsValid) return;

			tempGetPriorFolder();

			Folder = SetgMgr.GetPriorFolder(index);

			SetgMgr.SetPriorFolder(index, Folder);

			hcPath.Path = Folder;

			configHeader();

			return;
		}

	#region event handling

		public delegate void FolderChangeEventHandler(object sender, EventArgs e);

		public event FolderManager.FolderChangeEventHandler FolderChange;

		protected virtual void RaiseFolderChangeEvent()
		{
			FolderChange?.Invoke(this, new EventArgs());
		}

		private void onFolderPathPathChangeEvent(object sender, PathChangeArgs e)
		{
			Debug.WriteLine("folderManager, path changed");
			Debug.WriteLine("folderManager| index     | " + e.Index);
			Debug.WriteLine("folderManager| sel folder| " + e.SelectedFolder);
			Debug.WriteLine("folderManager| sel path  | " + e.SelectedPath.FullPath);

			hcPath.Path = e.SelectedPath;
			Folder = hcPath.Path;
		}

		private void onFolderPathSelectFolderEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Select Folder");

			SelectFolder();
		}

		private void onFolderPathFavoriteEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Favorites");

//			SelectFolder();
		}

		private void onFolderPathHistoryEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, History");

//			SelectFolder();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	}
}