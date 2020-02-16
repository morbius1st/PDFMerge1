#region + Using Directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
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
//	public class FolderManager2
//	{
//		private SelectFolder sf = new SelectFolder();
//
//		private FolderInfo[] folders = new FolderInfo[2];
//
//		public FolderManager2(HeaderControl hcCurrent, HeaderControl hcRevision)
//		{
//			folders[0] = new FolderInfoCurrent(hcCurrent);
//			folders[1] = new FolderInfoRevision(hcRevision);
//		}
//
//		public FolderInfo CurrentFolderInfo => folders[FolderType.CURRENT.Value()];
//		public FolderInfo RevisionFolderInfo => folders[FolderType.CURRENT.Value()];
//
//		public Route CurrentFolder => CurrentFolderInfo.Folder;
//		public Route RevisionFolder => RevisionFolderInfo.Folder;
//
//	}


	public class FolderManager : INotifyPropertyChanged
	{
		private SelectFolder sf = new SelectFolder();

		public static SavedFolderManager[] svfMgr = new SavedFolderManager[COUNT.Value()];

		private int index;

		private static HeaderControl hcPath2;

		private HeaderControl hcPath;

		public FolderManager(int index, HeaderControl hcPath)
		{
			var fldrRoute = 


			this.index = index;

			this.hcPath = hcPath;
			hcPath2 = hcPath;

//			hcPath.SetPathChangeEventHandler(onPathPathChangeEvent);
//			hcPath.SetSelectFolderEventHandler(onPathSelectFolderEvent);
//			hcPath.SetFavoritesEventHandler(onPathFavoriteEvent);
//			hcPath.SetHistoryEventHandler(onPathHistoryEvent);
//
//			hcPath.FolderRoute.MyCustomEvent += FldrRoute_MyCustomEvent;


			ConfigSavedFolders();

			SavedFoldersDebugSupport.Instance.
				ConfigSavedFoldersDebugSupport(svfMgr[HISTORY.Value()], svfMgr[FAVORITES.Value()]);

			getPriorFolder();

			configHeader();
		}

		private void FldrRoute_MyCustomEvent(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("custom event received");
		}

		private void ConfigSavedFolders()
		{
			if (svfMgr[0] == null)
			{
				svfMgr[HISTORY.Value()] = new SavedFolderManager(HISTORY);

				svfMgr[FAVORITES.Value()] =new SavedFolderManager(FAVORITES);
			}
		}

		private FilePath<FileNameAsSheet> folder;

		public FilePath<FileNameAsSheet> Folder
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

		internal virtual void RaiseFolderChangeEvent()
		{
			FolderChange?.Invoke(this, new EventArgs());
		}

		internal void onPathPathChangeEvent(object sender, PathChangeArgs e)
		{
			Debug.WriteLine("folderManager, path changed");
			Debug.WriteLine("folderManager| index     | " + e.Index);
			Debug.WriteLine("folderManager| sel folder| " + e.SelectedFolder);
			Debug.WriteLine("folderManager| sel path  | " + e.SelectedPath.GetFullPath);

			hcPath.Path = e.SelectedPath;
			Folder = hcPath.Path;
		}

		internal void onPathSelectFolderEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Select Folder");

			SelectFolder();
		}

		internal void onPathFavoriteEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Favorites");

//			SelectFolder();
		}

		internal void onPathHistoryEvent(object sender, EventArgs e)
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