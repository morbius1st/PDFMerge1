#region + Using Directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using Sylvester.FileSupport;
using Sylvester.Process;
using Sylvester.SavedFolders;
// using Sylvester.Settings;
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
/*
	public class FolderManager : INotifyPropertyChanged
	{
		private SelectFolder sf = new SelectFolder();

		public static SavedFolderManager[] svfMgr = new SavedFolderManager[COUNT.Value()];

		private FolderType index;

		private static HeaderControl hcPath2;

		private HeaderControl hcPath;

//		public FolderManager(FolderType index, HeaderControl hcPath)
//		{
//
//
//			this.index = index;
//
//			this.hcPath = hcPath;
//			hcPath2 = hcPath;
//
//			ConfigSavedFolders();
//
//			SavedFoldersDebugSupport.Instance.
//				ConfigSavedFoldersDebugSupport(svfMgr[HISTORY.Value()], svfMgr[FAVORITES.Value()]);
//
//			getPriorFolder();
//
//			configHeader();
//		}

		private void ConfigSavedFolders()
		{
			if (svfMgr[0] == null)
			{
				svfMgr[HISTORY.Value()] = new SavedFolderManager(HISTORY);

				svfMgr[FAVORITES.Value()] =new SavedFolderManager(FAVORITES);
			}
		}

		private FilePath<FileNameSimple> folder;

		public FilePath<FileNameSimple> Folder
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
			Folder = SetgMgr.GetPriorFolder(index);

//			hcPath.Folder = Folder;
		}

		private void SelectFolder()
		{
//			Folder = sf.GetFolder(Folder);
//			if (!Folder.IsValid) return;

			tempGetPriorFolder();

			Folder = SetgMgr.GetPriorFolder(index);

			SetgMgr.SetPriorFolder(index, Folder);

//			hcPath.Folder = Folder;

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
			Debug.WriteLine("folderManager| sel path  | " + e.SelectedPath.GetFullFilePath);

//			hcPath.Folder = e.SelectedPath;
			Folder = hcPath.Folder;
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
*/
}