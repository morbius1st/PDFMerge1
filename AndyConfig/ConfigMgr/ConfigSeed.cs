#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using AndyShared.FilesSupport;
using ClassifierEditor.ConfigSupport;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  6/21/2020 6:34:39 AM

namespace AndyConfig.ConfigMgr
{
	public class ConfigSeed : INotifyPropertyChanged
	{
	#region private fields

		public const string SEED_FOLDER = @"Seed Files";

	#endregion

	#region ctor

		public ConfigSeed() { }

	#endregion

	#region public properties

		public bool Initialized { get; set; }

		// seed folder

		public bool SiteSettingsSeedFolderExists
		{
			get => Directory.Exists(SiteSettingsSeedFolderPath);
		}

		public string SiteSettingsSeedFolderPath => SiteSettings.Path.SettingPath + "\\" + SEED_FOLDER;


		// seed files

		public bool HasSeedFileSetting => (SiteSettings.Data.InstSeedFiles != null &&
			SiteSettings.Data.InstSeedFiles.Count > 0);


		// seed files installed
		public ConfigSeedInstalled SeedInstalled {get; private set; } = new ConfigSeedInstalled();


	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			Initialized = true;

			SeedInstalled.Initialize();

			UpdateProperties();

			UpdateSelectedSeedFile();
		}

		public void SaveSeedFileList()
		{
			SiteSettings.Data.InstSeedFiles = new Dictionary<string, ConfigSeedFileSetting>();

			foreach (FilePath<FileNameSimpleSelectable> file in 
				SeedInstalled.InstalledSeedFiles.FoundFiles)
			{
				string sampleFile =
					file.GetPath + @"\" + file.GetFileNameObject.Name + @".dat";

				bool exists = File.Exists(sampleFile);

				string key = makeKey(file);

				if (!exists)
				{
					sampleFile = null;
				}

				ConfigSeedFileSetting seedFile =
					new ConfigSeedFileSetting(
						file.GetFileNameWithoutExtension,
						Heading.SuiteName,
						false, 
						file.GetFileNameObject.Selected,
						file.GetPath,
						file.GetFileName,
						sampleFile);

				SiteSettings.Data.InstSeedFiles.Add(key, seedFile);
			}

			SiteSettings.Admin.Write();
		}

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
			OnPropertyChange("SiteSettingsSeedFolderExists");
			OnPropertyChange("SiteSettingsSeedFolderPath");
			OnPropertyChange("HasSeedFileSetting");
		}

		private void UpdateSelectedSeedFile()
		{
			if (SiteSettings.Data.InstSeedFiles == null ||
				SiteSettings.Data.InstSeedFiles.Count == 0) return;

			foreach (FilePath<FileNameSimpleSelectable> file in
				SeedInstalled.InstalledSeedFiles.FoundFiles)
			{
				string key = makeKey(file);

				ConfigSeedFileSetting seed =
					SiteSettings.Data.InstSeedFiles[key];

				if (seed == null) continue;

				file.GetFileNameObject.Selected =
					seed.Selected;
			}
		}

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