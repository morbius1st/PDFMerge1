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

		public bool HasSavedFolders(SavedFolderType index)
		{
			return UserSettings.Data.SavedFolders[index.Value()].Count > 0;
		}

		public bool AddSavedFolder(SavedFolderProject sf, SavedFolderType index)
		{
			if (FindSavedFolder(sf.Name, index) != null) return false;

			UserSettings.Data.SavedFolders[index.Value()].Add(sf);

			return true;
		}

		public SavedFolderProject FindSavedFolder(string testkey, SavedFolderType index)
		{
			foreach (SavedFolderProject sp in UserSettings.Data.SavedFolders[index.Value()])
			{
				if (sp.Name.Equals(testkey)) return sp;
			}

			return null;
		}

		public SavedFolderPair FindSavedFolderPair(SavedFolderProject sf, string testKey)
		{
			foreach (SavedFolderPair sfp in sf.SavedFolderPairs)
			{
				if (sfp.Name.Equals(testKey)) return sfp;
			}

			return null;
		}

	#endregion

	#endregion

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
	}
}