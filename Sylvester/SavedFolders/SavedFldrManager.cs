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
using System.Windows;
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

//		private SavedFoldersDebugSupport sfds = SavedFoldersDebugSupport.Instance;

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

		public SavedFolderType FolderType { get; set; }

		public bool HasSavedFolders => SetgMgr.Instance.HasSavedFolders(FolderType);

		public static SavedFoldersWin SavedWinInstance => savedWinInstance;

		public static Window Parent { get; set; } 

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
			savedWinInstance.Owner = Parent;
			savedWinInstance.SavedFolderOperation = folderOp;

			bool? result = savedWinInstance.ShowDialog();

			return result;
		}

		public FilePath<FileNameSimple> Current => savedWinInstance.SelectedFolderPair?.Current ?? null;
		public FilePath<FileNameSimple> Revision => savedWinInstance.SelectedFolderPair?.Revision ?? null;

		public void AddToHistory(FilePath<FileNameSimple> current, FilePath<FileNameSimple> revision)
		{
			SavedFolderProject sf;

			SavedFolderPair pair = SetgMgr.Instance.
				FindSavedProjectByPaths(current, revision, 
					SavedFolderType.HISTORY, out sf);

			// not found - add
			if (sf != null)
			{
				string name = SavedFolderProject.MakeName(current, SavedFolderType.HISTORY);

				// verify if this name matches
				sf = SetgMgr.Instance.FindFolderProjectByName(name, SavedFolderType.HISTORY);

				if (sf != null)
				{
					SetgMgr.Instance.AddFolderPair(sf, current, revision);

				}
				else
				{
					// add project and folder pair
					// prevent duplicate / extra work - save saved folder selected and note
					// if not modified, do not resave
					// same for project selected

					
				}

			}
		}


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