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

	public class SavedFolderManager : INotifyPropertyChanged
	{
		// will be two copies 
		// one for history
		// one for favorites

		private static string[] titles = new [] {"Historical Folders", "Favorite Folders"};

		private static SavedFoldersWin savedWinInstance;

		private SetgMgr sm;

		private SavedFoldersDebugSupport sfds = SavedFoldersDebugSupport.Instance;

		private string title;

//		public SavedFolderManager() { }

		public SavedFolderManager(SavedFolderType index)
		{
			// before make savedfolderwin
			sm = SetgMgr.Instance;

			Index = index;
			this.title = titles[index.Value()];
		}

	#region public methods

		public SavedFolderType Index { get; set; }

		public bool HasSavedFolders => sm.HasSavedFolders(Index);

		public static SavedFoldersWin SavedWinInstance => savedWinInstance;

	#endregion

	#region public methods

		public void test()
		{
			savedWinInstance = new SavedFoldersWin(Index, title);
			savedWinInstance.AddFavorite -= SavedWinInstance_AddFavorite;
			savedWinInstance.AddFavorite += SavedWinInstance_AddFavorite;
			bool? result = savedWinInstance.ShowDialog();
		}

		public bool AddProjectFavorite(
			FilePath<FileNameSimple> current,
			FilePath<FileNameSimple> revision
			)
		{
//			UserSettings.Data.priorPath = current;


//			SetgMgr.SetPriorFolder(FolderType.CURRENT, current);
//			SetgMgr.SetPriorFolder(FolderType.REVISION, revision);

//			string searchKey = SavedFolderProject.MakeSavedFolderKey(current.AssemblePath(1));
//
//			SavedFolderProject sf = sm.FindSavedFolder(searchKey, Index);
//			SavedFolderPair cfp = new SavedFolderPair(current, revision);
//
//			if (sf == null)
//			{
//				sf = new SavedFolderProject(current);
//				sm.AddSavedFolder(sf, Index);
//			}
//			else
//			{
//				if (sm.FindSavedFolderPair(sf, cfp.Key) != null) return false;
//			}
//
//			sf.SavedFolderPairs.Add(cfp);
//
//			UserSettings.Admin.Write();

			bool result = AddProject(current, revision);

			if (!result) return false;

			savedWinInstance.CollectionUpdated();

			return true;
		}

		public bool AddProjectHistory(
			FilePath<FileNameSimple> current,
			FilePath<FileNameSimple> revision
			)
		{
			bool result = AddProject(current, revision);

			if (!result) return false;

			savedWinInstance.CollectionUpdated();

			return true;
		}

	#endregion

	#region private methods

		private bool AddProject (
			FilePath<FileNameSimple> current,
			FilePath<FileNameSimple> revision)
		{
			string searchKey = SavedFolderProject.MakeSavedFolderKey(current.AssemblePath(1));

			SavedFolderProject sf = sm.FindSavedFolder(searchKey, Index);
			SavedFolderPair cfp = new SavedFolderPair(current, revision);

			if (sf == null)
			{
				sf = new SavedFolderProject(current);
				sm.AddSavedFolder(sf, Index);
			}
			else
			{
				if (sm.FindSavedFolderPair(sf, cfp.Key) != null) return false;
			}

			sf.SavedFolderPairs.Add(cfp);

			UserSettings.Admin.Write();

			return true;
		}

	#endregion

	#region event processing

		private void SavedWinInstance_AddFavorite(object sender, EventArgs e)
		{
			// add a fav

			sfds.Test_02(this, Index);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}
}