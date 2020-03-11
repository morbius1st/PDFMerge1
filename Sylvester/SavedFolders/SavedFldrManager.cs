#region + Using Directives

#endregion


// projname: Sylvester.FolderSupport
// itemname: SavedFolderManager
// username: jeffs
// created:  1/20/2020 8:55:27 PM
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sylvester.FileSupport;
using Sylvester.Process;
using Sylvester.Settings;
using UtilityLibrary;

namespace Sylvester.SavedFolders
{
	public enum SavedFolderType
	{
		HISTORY = 0,
		FAVORITES = 1,
		COUNT = 2
	}

	public enum SavedFolderOperation
	{
		MANAGEMENT = 0,
		GET_CURRENT = 1,
		GET_REVISION = 2
	}

	public enum SavedFolderCategory
	{
		FOLDER_PROJECT = 0,
		FOLDER_PAIR = 1
	}

	public class SavedFolderManager : INotifyPropertyChanged
	{
	#region private fields

		// will be two copies 
		// one for history
		// one for favorite
		private static SavedFoldersWin savedWinInstance;

		private SavedFoldersDebugSupport sfds = SavedFoldersDebugSupport.Instance;

		private string title;

		private static SavedFolderManager favoritesMgr;
		private static SavedFolderManager historyMgr;

		private SavedFolderManager( SavedFolderType folderType, string title)
		{
			FolderType = folderType;
			this.title = title;
		}

	#endregion

	#region public methods

//		public static SavedFolderManager GetFavoriteManager
//		{
//			get
//			{
//				if (favoritesMgr == null)
//					favoritesMgr =
//						new SavedFolderManager(SavedFolderType.FAVORITES, TODO, "Favorite Folders");
//
//				return favoritesMgr;
//			}
//		}
//
//		public static SavedFolderManager GetHistoryManager
//		{
//			get
//			{
//				if (historyMgr == null)
//					historyMgr =
//						new SavedFolderManager(SavedFolderType.HISTORY, TODO, "History Folders");
//				return historyMgr;
//			}
//		}

		public SavedFolderType FolderType { get; set; }

		public bool HasSavedFolders => SetgMgr.Instance.HasSavedFolders(FolderType);

		public static SavedFoldersWin SavedWinInstance => savedWinInstance;

	#endregion

	#region public methods

		public static SavedFolderManager GetFavoriteManager()
		{
			return new SavedFolderManager(SavedFolderType.FAVORITES, "Favorite Folders");
		}

		public static SavedFolderManager GetHistoryManager()
		{
			return new SavedFolderManager(SavedFolderType.HISTORY, "History Folders");
		}

		public bool? ShowSavedFolderWin(SavedFolderOperation folderOp = SavedFolderOperation.MANAGEMENT)
		{
			savedWinInstance = new SavedFoldersWin(FolderType, title);
			savedWinInstance.SavedFolderOperation = folderOp;

			return savedWinInstance.ShowDialog();
		}

//		public void test()
//		{
//			savedWinInstance = new SavedFoldersWin(FolderType, title);
//			savedWinInstance.AddFavorite -= SavedWinInstance_AddFavorite;
//			savedWinInstance.AddFavorite += SavedWinInstance_AddFavorite;
//			savedWinInstance.Owner = MainWindow.MainWin;
//
//			bool? result = savedWinInstance.ShowDialog();
//		}
//
//		public bool AddProjectFavorite(
//			FilePath<FileNameSimple> current,
//			FilePath<FileNameSimple> revision
//			)
//		{
//			bool result = AddProject(current, revision);
//
//			if (!result) return false;
//
//			savedWinInstance.CollectionUpdated();
//
//			return true;
//		}
//
//		public bool AddProjectHistory(
//			FilePath<FileNameSimple> current,
//			FilePath<FileNameSimple> revision
//			)
//		{
//			bool result = AddProject(current, revision);
//
//			if (!result) return false;
//
//			savedWinInstance.CollectionUpdated();
//
//			return true;
//		}

	#endregion

	#region private methods

//		private bool AddProject (
//			FilePath<FileNameSimple> current,
//			FilePath<FileNameSimple> revision)
//		{
//			string searchKey = SavedFolderProject.MakeFolderProjectKey(current, FolderType);
//
//			SavedFolderProject sf = SetgMgr.Instance.FindSavedFolder(searchKey, FolderType);
//			SavedFolderPair cfp = new SavedFolderPair(current, revision);
//
//			if (sf == null)
//			{
//				sf = new SavedFolderProject(current, FolderType);
//				SetgMgr.Instance.AddSavedProjectFolder(sf, FolderType);
//			}
//			else
//			{
//				if (SetgMgr.Instance.FindSavedFolderPair(sf, cfp.Name) != null) return false;
//			}
//
//			sf.SavedFolderPairs.Add(cfp);
//
//			UserSettings.Admin.Write();
//
//			return true;
//		}

	#endregion

	#region event processing

//		private void SavedWinInstance_AddFavorite(object sender, EventArgs e)
//		{
//			// add a fav
//
//			sfds.Test_02(this, FolderType);
//		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}
}