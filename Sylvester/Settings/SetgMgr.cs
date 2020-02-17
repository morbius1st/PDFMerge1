#region + Using Directives

using System.Collections.Generic;
using System.Collections.ObjectModel;
using SettingManager;
using Sylvester.FileSupport;
using Sylvester.Process;
using Sylvester.SavedFolders;
using UtilityLibrary;

#endregion


// projname: Sylvester.Settings
// itemname: SettingsManager
// username: jeffs
// created:  11/16/2019 6:02:58 AM


namespace Sylvester.Settings
{
	public sealed class SetgMgr
	{
		private static readonly SetgMgr instance = new SetgMgr();

		static SetgMgr() { }

		public SetgMgr()
		{
		}

		public static SetgMgr Instance => instance;

	#region configuration settings

		public static SheetTitleCase SheetTitleCase
		{
			get { return UserSettings.Data.SheetTitleCase; }
			set
			{
				UserSettings.Data.SheetTitleCase = value;
				UserSettings.Admin.Write();
			}
		}

	#endregion

	#region configuration data - prior folders

		public static string[] test = new string[3];

		public static void SetPriorFolder(FolderType index, FilePath<FileNameSimple> folder)
		{
			UserSettings.Data.PriorFolders[index.Value()] = folder.GetFullPath;
			UserSettings.Admin.Write();
		}

		public static void SetPriorFolder(FolderType index, string folder)
		{

			SettingsMgr<UserSettingInfo30> admin = UserSettings.Admin;

			UserSettingData30 a = UserSettings.Data;

			UserSettings.Data.PriorFolders[index.Value()] = folder;
			UserSettings.Admin.Write();
		}

		public static FilePath<FileNameSimple> GetPriorFolder(FolderType index)
		{
			return new FilePath<FileNameSimple>(UserSettings.Data.PriorFolders[index.Value()]);
		}

	#endregion

		public List<ObservableCollection<SavedFolderProject>> SavedFolders => UserSettings.Data.SavedFolders;

		public bool HasSavedFolders(SavedFolderType index)
		{
			return UserSettings.Data.SavedFolders[index.Value()].Count > 0;
		}

		public bool AddSavedFolder(SavedFolderProject sf, SavedFolderType index)
		{
			if (FindSavedFolder(sf.Identifier.RootFolder, index) != null) return false;

			UserSettings.Data.SavedFolders[index.Value()].Add(sf);

			return true;
		}

		public SavedFolderProject FindSavedFolder(string testkey, SavedFolderType index)
		{
			foreach (SavedFolderProject sp in UserSettings.Data.SavedFolders[index.Value()])
			{
				if (sp.Key.Equals(testkey)) return sp;
			}

			return null;
		}

		public SavedFolderPair 
			FindCurrentRevisionFolderPair(SavedFolderProject sf, string testKey)
		{

			foreach (SavedFolderPair sfp in sf.SavedFolderPairs)
			{
				if (sfp.Key.Equals(testKey)) return sfp;
			}

			return null;
		}


	}
}