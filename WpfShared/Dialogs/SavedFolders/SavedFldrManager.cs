#region + Using Directives

#endregion
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SettingsManager;

// using Sylvester.Settings;
using UtilityLibrary;

namespace WpfShared.Dialogs.SavedFolders
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

#pragma warning disable CS0169 // The field 'SavedFolderManager.favoritesMgr' is never used
		private static SavedFolderManager favoritesMgr;
#pragma warning restore CS0169 // The field 'SavedFolderManager.favoritesMgr' is never used
#pragma warning disable CS0169 // The field 'SavedFolderManager.historyMgr' is never used
		private static SavedFolderManager historyMgr;
#pragma warning restore CS0169 // The field 'SavedFolderManager.historyMgr' is never used


	#endregion

		private SavedFolderManager( SavedFolderType savedFolderType, string title)
		{
			SavedFolderType = savedFolderType;
			this.title = title;
		}

	#region public methods

		public SavedFolderType SavedFolderType { get; set; }

		public bool HasSavedFolders => SetgMgr.Instance.HasSavedFolders(SavedFolderType);

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
			savedWinInstance = new SavedFoldersWin(SavedFolderType, title);
			savedWinInstance.Owner = Parent;
			savedWinInstance.SavedFolderOperation = folderOp;

			savedWinInstance.AddFavorite += OnAddFavorite;

			bool? result = savedWinInstance.ShowDialog();

			return result;
		}

		public FilePath<FileNameSimple> Current => savedWinInstance.SelectedFolderPair?.Current ?? null;

		public FilePath<FileNameSimple> Revision => savedWinInstance.SelectedFolderPair?.Revision ?? null;

		public bool AddToSavedFolders(FilePath<FileNameSimple> current, FilePath<FileNameSimple> revision)
		{
			SavedFolderProject sf;

			SavedFolderPair sfp =
				SetgMgr.Instance.FindSavedProjectByPaths(current, revision, SavedFolderType, out sf);

			// already in history
			if (sfp != null) return true;

			// combo of current and revision is not in history
			// check one - is project in history 

			sf = SetgMgr.Instance.FindFolderProjectByRootFolder(current, SavedFolderType);

			bool result;

			// already exists?
			if (sf == null)
			{
				// add full boat
				result = SetgMgr.Instance.NewFolderProject(current, revision, SavedFolderType) == null;
			}
			else
			{
				result = SetgMgr.Instance.AddFolderPair(sf, current, revision);
			}

			SetgMgr.WriteUsr();

			return result;
		}

	#endregion

	#region event processing

		private void OnAddFavorite(object sender, EventArgs e)
		{
			// add a fav

			SavedFolderProject sf = SetgMgr.Instance.FindFolderProjectByKey(
				savedWinInstance.SelectedFolderProject.Key, SavedFolderType.FAVORITES);

			FilePath < FileNameSimple > current =
				savedWinInstance.SelectedFolderProject.SavedFolderPairs[0]?.Current;

			FilePath < FileNameSimple > revision =
				savedWinInstance.SelectedFolderProject.SavedFolderPairs[0]?.Revision;

			if (sf == null)
			{
				sf = SetgMgr.Instance.NewFolderProject(current, revision, SavedFolderType.FAVORITES);
			}
			else
			{
				SetgMgr.Instance.AddFolderPair(sf, current, revision);
			}

			if (savedWinInstance.SelectedFolderProject.SavedFolderPairs.Count > 1)
			{

				for (int i = 1; i < savedWinInstance.SelectedFolderProject.SavedFolderPairs.Count; i++)
				{
					current =
						savedWinInstance.SelectedFolderProject.SavedFolderPairs[i]?.Current;

					revision =
						savedWinInstance.SelectedFolderProject.SavedFolderPairs[i]?.Revision;

					SetgMgr.Instance.AddFolderPair(sf, current, revision);
				}

			}


			SetgMgr.WriteUsr();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	}
}