#region + Using Directives

#endregion


// projname: Sylvester.FolderSupport
// itemname: SavedFolderManager
// username: jeffs
// created:  1/20/2020 8:55:27 PM
	using Sylvester.FileSupport;
using Sylvester.Settings;

	using static Sylvester.SavedFolders.SavedFolderType;

namespace Sylvester.SavedFolders
{
	public enum SavedFolderType
	{
		CURRENT = 0,
		REVISION = 1,
		COUNT = 2
	}

	public class SavedFolderManager
	{
		// will be two versions 
		// one for history
		// one for favorites

		private static SavedFoldersWin savedWinInstance;

		public SavedFolderManager() { }

		public SavedFolderManager(SavedFolderType index)
		{
			savedWinInstance = new SavedFoldersWin();

			Index = index;
		}

		public SavedFolderType Index { get; set; }

		public bool HasSavedFolders => SetgMgr.Instance.HasSavedFolders(Index);

		public static SavedFoldersWin SavedWinInstance => savedWinInstance;



		public void test()
		{
			bool? result = savedWinInstance.ShowDialog();
		}


		public bool AddProject(
			Route current,
			Route revision
			)
		{
			UserSettings.Data.priorPath = current;

			SavedFolder sf = SetgMgr.Instance.FindSavedFolder(current.FolderNames[0], Index);
			CurrentRevisionFolderPair cfp = new CurrentRevisionFolderPair(current, revision);

			if (sf == null)
			{
				sf = new SavedFolder(current.VolumeName, current.FolderNames[0]);
				SetgMgr.Instance.AddSavedFolder(sf, Index);
			}
			else
			{
				if (SetgMgr.Instance.FindCurrentRevisionFolderPair(sf, cfp.Key) != null) return false;
			}

			sf.FolderPairs.Add(cfp.Key, cfp);

			UserSettings.Admin.Write();

			return true;
		}


	}
}
