#region + Using Directives

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using SettingManager;
using Sylvester.FileSupport;
using Sylvester.Process;
using Sylvester.SavedFolders;
using UtilityLibrary;

using static Sylvester.SavedFolders.SavedFolderType;

#endregion


// projname: Tests2.Settings
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

		public static void SetPriorFolder(int index, Route folder)
		{
			UserSettings.Data.PriorFolders[index] = folder.FullPath;
			UserSettings.Admin.Write();
		}

		public static void SetPriorFolder(int index, string folder)
		{

			SettingsMgr<UserSettingInfo30> admin = UserSettings.Admin;

			UserSettingData30 a = UserSettings.Data;

			UserSettings.Data.PriorFolders[index] = folder;
			UserSettings.Admin.Write();
		}

		public static Route GetPriorFolder(int index)
		{
			return new Route(UserSettings.Data.PriorFolders[index]);
		}

	#endregion

		public List<ObservableCollection<SavedProject>> SavedFolders => UserSettings.Data.SavedFolders;

		public bool HasSavedFolders(SavedFolderType index)
		{
			return UserSettings.Data.SavedFolders[index.Value()].Count > 0;
		}

		public bool AddSavedFolder(SavedProject sf, SavedFolderType index)
		{
			if (FindSavedFolder(sf.Identifier.RootFolder, index) != null) return false;

			UserSettings.Data.SavedFolders[index.Value()].Add(sf);

			return true;
		}

		public SavedProject FindSavedFolder(string testkey, SavedFolderType index)
		{
			foreach (SavedProject sp in UserSettings.Data.SavedFolders[index.Value()])
			{
				if (sp.Key.Equals(testkey)) return sp;
			}

			return null;
		}

		public SavedFolderPair 
			FindCurrentRevisionFolderPair(SavedProject sf, string testKey)
		{

			foreach (SavedFolderPair sfp in sf.SavedFolderPairs)
			{
				if (sfp.Key.Equals(testKey)) return sfp;
			}

			return null;
		}


	}
}