#region + Using Directives

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SettingManager;
using SettingsManager;
using Sylvester.FileSupport;
using Sylvester.Process;
using Sylvester.SavedFolders;
// using Sylvester.Settings;
using Sylvester.Windows;
using UtilityLibrary;

#endregion


// projname: Sylvester.Settings
// itemname: SettingsManager
// username: jeffs
// created:  11/16/2019 6:02:58 AM

namespace SettingsManager
{
	public sealed class SetgMgr
	{
		private static readonly SetgMgr instance = new SetgMgr();

		private static WindowManager winMgr = new WindowManager();

		static SetgMgr() { }

		public SetgMgr() { }

		public static SetgMgr Instance => instance;

	#region app settings

//		public static bool DebugMode
//		{
//			get => AppSettings.Data.DebugMode;
//			set => AppSettings.Data.DebugMode = value;
//		}

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
			if (UserSettings.Data.PriorFolders == null || UserSettings.Data.PriorFolders.Length <1) return null;

			// SettingsMgr<UserSettingInfo30> u = UserSettings.Admin;

			return UserSettings.Data.PriorFolders[index.Value()];
		}

	#endregion

	#region public methods

	#region misc methods

		public static void WriteUsr()
		{
			UserSettings.Admin.Write();
		}

		public static void WriteApp()
		{
			AppSettings.Admin.Write();
		}

		public List<ObservableCollection<SavedFolderProject>> SavedFolders => UserSettings.Data.SavedFolders;

	#endregion

	#region saved project methods

		public SavedFolderProject NewFolderProject(FilePath<FileNameSimple> current,
			FilePath<FileNameSimple> revision, SavedFolderType folderType)
		{
			SavedFolderProject sfp = new SavedFolderProject(current, folderType);

			SavedFolderPair cfp = new SavedFolderPair(sfp, current, revision);

			sfp.SavedFolderPairs.Add(cfp);

			AddSavedFolderProject(sfp, folderType);

			WriteUsr();

			return sfp;
		}

		public SavedFolderProject CreateFolderProject(SavedFolderType folderType)
		{
			return NewFolderProject(null, null, folderType);
		}
		
		public bool AddSavedFolderProject(SavedFolderProject sf, SavedFolderType folderType)
		{
			if (FindFolderProjectByName(sf.Name, folderType) != null) return false;

			UserSettings.Data.SavedFolders[folderType.Value()].Add(sf);

			return true;
		}

		public bool DeleteSavedProjectFolder(SavedFolderProject sf, SavedFolderType folderType)
		{
			UserSettings.Data.SavedFolders[(int) folderType].Remove(sf);

			return true;
		}

		// find by root folder path
		public SavedFolderProject FindFolderProjectByRootFolder(FilePath<FileNameSimple> current,
			SavedFolderType folderType)
		{
			if (current == null || current== FilePath<FileNameSimple>.Invalid) return null;

			string testKey = current.AssemblePath(1).ToUpper();

			foreach (SavedFolderProject sf in UserSettings.Data.SavedFolders[(int) folderType])
			{
				if (sf == null) continue;

				foreach (SavedFolderPair pair in sf.SavedFolderPairs)
				{
					if (pair.Current == null) continue;

					string rootPath = pair.Current.AssemblePath(1);

					// got it?  return it.
					if (rootPath.ToUpper().Equals(testKey)) return sf;
				}
			}

			// nothing found
			return null;
		}

		// find by name
		public SavedFolderProject FindFolderProjectByKey(string testKey, SavedFolderType folderType)
		{
			foreach (SavedFolderProject fp in UserSettings.Data.SavedFolders[folderType.Value()])
			{
				if (fp.Key.Equals(testKey)) return fp;
			}

			return null;
		}

		public bool ContainsKey(string testKey, SavedFolderType folderType)
		{
			bool result = FindFolderProjectByKey(testKey, folderType) != null;

			return result;
		}

		// find by name
		public SavedFolderProject FindFolderProjectByName(string testName, SavedFolderType folderType)
		{
			foreach (SavedFolderProject sp in UserSettings.Data.SavedFolders[folderType.Value()])
			{
				if (sp.Name.Equals(testName)) return sp;
			}

			return null;
		}

		public bool ContainsName(string testName, SavedFolderType folderType)
		{
			bool result = FindFolderProjectByName(testName, folderType) != null;

			return result;
		}

		public bool HasSavedFolders(SavedFolderType folderType)
		{
			return UserSettings.Data.SavedFolders[folderType.Value()].Count > 0;
		}

	#endregion

	#region saved pair methdods

		public bool AddFolderPair(SavedFolderProject sf,
			FilePath<FileNameSimple> current,
			FilePath<FileNameSimple> revision)
		{
			if (sf == null) return false;

			SavedFolderPair pair = new SavedFolderPair(sf, current, revision);

			sf.SavedFolderPairs.Add(pair);

			return true;
		}

		public SavedFolderPair CopyFolderPair(SavedFolderProject sf, SavedFolderPair pair)
		{
			if (pair == null) return null;

			return pair.Clone(sf);
		}
		
		public bool DeleteFolderPair(SavedFolderProject sf, SavedFolderPair pair)
		{
			if (!ContainsName(sf, pair.Name)) return false;

			sf.SavedFolderPairs.Remove(pair);

			return true;
		}

		public SavedFolderPair FindFolderPairByKey(SavedFolderProject sf, string testKey)
		{
			foreach (SavedFolderPair sfp in sf.SavedFolderPairs)
			{
				if (sfp.Key.Equals(testKey)) return sfp;
			}

			return null;
		}

		public bool ContainsKey(SavedFolderProject sf, string testKey)
		{
			return FindFolderPairByKey(sf, testKey) != null;
		}

		public SavedFolderPair FindFolderPairByName(SavedFolderProject sf, string testName)
		{
			foreach (SavedFolderPair sfp in sf.SavedFolderPairs)
			{
				if (sfp.Name.Equals(testName)) return sfp;
			}

			return null;
		}

		public bool ContainsName(SavedFolderProject sf, string testName)
		{
			return FindFolderPairByName(sf, testName) != null;
		}

		public SavedFolderPair FindSavedProjectByPaths(FilePath<FileNameSimple> Current, 
			FilePath<FileNameSimple> Revision, SavedFolderType folderType, out SavedFolderProject project)
		{
			project = null;

			if (Current == null || Current == FilePath<FileNameSimple>.Invalid ||
				Revision == null || Revision == FilePath<FileNameSimple>.Invalid) return null;

			foreach (SavedFolderProject sf in UserSettings.Data.SavedFolders[(int) folderType])
			{
				if (sf == null) continue;

				foreach (SavedFolderPair pair in sf.SavedFolderPairs)
				{
					if (pair.Current == null) continue;
					if (pair.Revision == null) continue;

					if (pair.Current.Equals(Current))
					{
						if (pair.Revision.Equals(Revision))
						{
							// found
							project = sf;
							return pair;
						}
					}
				}
			}

			return null;
		}
		
	#endregion

	#endregion

	#endregion

	#region window layout

		public static WindowLayout GetMainWindowLayout => GetWindowLayout(WindowId.WINDOW_MAIN);
		public static WindowLayout GetSavedFolderLayout => GetWindowLayout(WindowId.DIALOG_SAVED_FOLDERS);


		public static WindowLayout GetWindowLayout(WindowId id)
		{
			WindowLayout w = UserSettings.Data.SavedWinLocationInfo[(int) id];

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