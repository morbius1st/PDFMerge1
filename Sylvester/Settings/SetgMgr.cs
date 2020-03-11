#region + Using Directives

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using SettingManager;
using Sylvester.FileSupport;
using Sylvester.Process;
using Sylvester.SavedFolders;
using Sylvester.Windows;
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

		private static WindowManager winMgr = new WindowManager();

		static SetgMgr() { }

		public SetgMgr() { }

		public static SetgMgr Instance => instance;

	#region app settings

		public static bool DebugMode
		{
			get => AppSettings.Data.DebugMode;
			set => AppSettings.Data.DebugMode = value;
		}

		public static bool AllowPropertyEditing => AppSettings.Data.AllowPropertyEditing;

	#endregion


	#region user settings

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

		public static void SetPriorFolder(FolderType index, FilePath<FileNameSimple> folder)
		{
			UserSettings.Data.PriorFolders[index.Value()] = folder;
			UserSettings.Admin.Write();
		}

		public static FilePath<FileNameSimple> GetPriorFolder(FolderType index)
		{
			return UserSettings.Data.PriorFolders[index.Value()];
		}

	#endregion

	#region public methods

		public static void WriteUsr()
		{
			UserSettings.Admin.Write();
		}

		public static void WriteApp()
		{
			AppSettings.Admin.Write();
		}

		public List<ObservableCollection<SavedFolderProject>> SavedFolders => UserSettings.Data.SavedFolders;

		// saved project methods

		// create a blank project - with temp name and null folder pairs
		public SavedFolderProject CreateSavedProject(SavedFolderType folderType)
		{
			SavedFolderProject sfp = new SavedFolderProject(null, folderType);

			SavedFolderPair cfp = new SavedFolderPair(null, null, "Folder Pair Name");

			sfp.SavedFolderPairs.Add(cfp);

			AddSavedProjectFolder(sfp, folderType);

			WriteUsr();

			return sfp;
		}
		
		public bool AddSavedProjectFolder(SavedFolderProject sf, SavedFolderType folderType)
		{
			if (FindSavedFolder(sf.Name, folderType) != null) return false;

			UserSettings.Data.SavedFolders[folderType.Value()].Add(sf);

			return true;
		}

		public bool DeleteSavedProjectFolder(SavedFolderProject sf, SavedFolderType folderType)
		{
			UserSettings.Data.SavedFolders[(int) folderType].Remove(sf);

			return true;
		}


		public SavedFolderProject FindSavedFolder(string testkey, SavedFolderType folderType)
		{
			foreach (SavedFolderProject sp in UserSettings.Data.SavedFolders[folderType.Value()])
			{
				if (sp.Name.Equals(testkey)) return sp;
			}

			return null;
		}

		public bool ContainsSavedFolder(string testkey, SavedFolderType folderType)
		{
			bool result = FindSavedFolder(testkey, folderType) != null;

			return result;
		}

		public bool HasSavedFolders(SavedFolderType index)
		{
			return UserSettings.Data.SavedFolders[index.Value()].Count > 0;
		}

		// end
		 
		// folder pair methods

		public bool AddFolderPair(SavedFolderProject sf,
			FilePath<FileNameSimple> current,
			FilePath<FileNameSimple> revision)
		{
			if (sf == null) return false;

			SavedFolderPair pair = new SavedFolderPair(current, revision,
				SavedFolderPair.MakeFolderPairkey(sf, current, revision));

			sf.SavedFolderPairs.Add(pair);

			return true;
		}
		
		public bool DeleteFolderPair(SavedFolderProject sf, SavedFolderPair pair)
		{
			if (!ContainsFolderPair(sf, pair.Name)) return false;

			sf.SavedFolderPairs.Remove(pair);

			return true;
		}


		public SavedFolderPair FindSavedFolderPair(SavedFolderProject sf, string testKey)
		{
			foreach (SavedFolderPair sfp in sf.SavedFolderPairs)
			{
				if (sfp.Name.Equals(testKey)) return sfp;
			}

			return null;
		}

		public bool ContainsFolderPair(SavedFolderProject sf, string testKey)
		{
			return FindSavedFolderPair(sf, testKey) != null;
		}



		// end



	#endregion

	#endregion

	#region window layout

		public static WindowLayout GetMainWindowLayout => GetWindowLayout(WindowId.WINDOW_MAIN);
		public static WindowLayout GetSavedFolderLayout => GetWindowLayout(WindowId.DIALOG_SAVED_FOLDERS);


		public static WindowLayout GetWindowLayout(WindowId id)
		{
			return UserSettings.Data.SavedWinLocationInfo[(int) id];
		}

		public static void SetWindowLayout(WindowId id, WindowLayout layout)
		{
			UserSettings.Data.SavedWinLocationInfo[(int) id] = layout;

			UserSettings.Admin.Write();
		}


		public static void RestoreWindowPosition(WindowId id, Window win)
		{
			winMgr.RestoreWinPosition(win, GetWindowLayout(id));
		}


		public static void RestoreWindowLayout(WindowId id, Window win)
		{
			winMgr.RestoreWinLayout(win, GetWindowLayout(id));
		}

		public static void SaveWindowLayout(WindowId id, Window win)
		{
			SetWindowLayout(id,
				winMgr.SaveWinLayout(win, GetWindowLayout(id))
				);
		}

	#endregion
	}
}