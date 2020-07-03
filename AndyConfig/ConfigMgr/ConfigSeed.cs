#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using AndyShared.ConfigSupport;
using AndyShared.FilesSupport;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  6/21/2020 6:34:39 AM

namespace AndyShared.ConfigMgr
{
	public class ConfigSeed : INotifyPropertyChanged
	{
	#region private fields

		public static string SEED_PATTERN = @"*.seed.xml";
		public static string SEED_FOLDER_NAME = @"Seed Files";
		public static string SEED_FOLDER = @"\"+ SEED_FOLDER_NAME;

	#endregion

	#region ctor

		public ConfigSeed()
		{
			SeedInstalled = new ConfigSeedInstalled();

			SeedLocal = new ConfigSeedLocal();
		}

	#endregion

	#region public properties

		public bool Initialized { get; set; }

		// seed folder

		public bool SiteSettingsSeedFolderExists
		{
			get => Directory.Exists(SiteSettingsSeedFolderPath);
		}

		public string SiteSettingsSeedFolderPath => SiteSettings.Path.SettingPath + SEED_FOLDER;

		// seed files
		public bool HasSeedFileSetting => (SiteSettings.Data.InstalledSeedFiles != null &&
			SiteSettings.Data.InstalledSeedFiles.Count > 0);

		// seed files installed
		public ConfigSeedInstalled SeedInstalled {get; private set; } 

		public ConfigSeedLocal SeedLocal { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			if (Initialized) return;

			Initialized = true;

			SeedInstalled.Initialize();

			SeedLocal.Initialize();

			UpdateProperties();

			// UpdateSelectedSeedFile();
		}

		// public void SaveSeedFileList()
		// {
		// 	SiteSettings.Data.InstalledSeedFiles = new SortedDictionary<string, ConfigSeedFileSetting>();
		//
		// 	foreach (FilePath<FileNameSimpleSelectable> file in 
		// 		SeedInstalled.InstalledSeedFileList.FoundFiles)
		// 	{
		// 		string sampleFile =
		// 			file.GetPath + @"\" + file.GetFileNameObject.Name + @".dat";
		//
		// 		bool exists = File.Exists(sampleFile);
		//
		// 		string key = makeKey(file);
		//
		// 		if (!exists)
		// 		{
		// 			sampleFile = null;
		// 		}
		//
		// 		ConfigSeedFileSetting seedFile =
		// 			new ConfigSeedFileSetting(
		// 				file.GetFileNameWithoutExtension,
		// 				Heading.SuiteName,
		// 				false, 
		// 				false,
		// 				file.GetFileNameObject.Selected,
		// 				file.GetPath,
		// 				file.GetFileName,
		// 				sampleFile);
		//
		// 		SiteSettings.Data.InstalledSeedFiles.Add(key, seedFile);
		// 	}
		//
		// 	SiteSettings.Admin.Write();
		// }

	#endregion

	#region private methods

		private string makeKey(FilePath<FileNameSimpleSelectable> file)
		{
			return SiteSettings.Data.MakeKey(Heading.SuiteName,
				file.GetFileNameWithoutExtension);
		}

		private void UpdateProperties()
		{
			OnPropertyChange("Initialized");
			OnPropertyChange("SeedInstalled");
			OnPropertyChange("SeedLocal");
			OnPropertyChange("SiteSettingsSeedFolderExists");
			OnPropertyChange("SiteSettingsSeedFolderPath");
			OnPropertyChange("HasSeedFileSetting");
		}

		// read the files found in the folder and update the
		// list in the config file
		// cross ref with current config file list to determine
		// if a file is selected
		// this updates with any added
		// private void UpdateSelectedSeedFile()
		// {
		// 	if (SiteSettings.Data.InstalledSeedFiles == null ||
		// 		SiteSettings.Data.InstalledSeedFiles.Count == 0) return;
		//
		// 	foreach (FilePath<FileNameSimpleSelectable> file in
		// 		SeedInstalled.InstalledSeedFileList.FoundFiles)
		// 	{
		// 		string key = makeKey(file);
		//
		// 		ConfigSeedFileSetting seed =
		// 			SiteSettings.Data.InstalledSeedFiles[key];
		//
		// 		if (seed == null) continue;
		//
		// 		file.GetFileNameObject.Selected =
		// 			seed.Selected;
		// 	}
		// }

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ConfigSeed";
		}

	#endregion
	}
}