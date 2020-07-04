#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using AndyShared.ConfigSupport;
using AndyShared.FilesSupport;
using SettingsManager;

#endregion

// username: jeffs
// created:  7/3/2020 8:21:22 AM

// ConfigSite - the SiteSettingsFile
// (as a  static instances)
// properties
//  initialized
//  the list of files found in the site seed folder
//  the collection of files in the site seed folder (need combine with the above)
//  the view of the collection (all files)
//  the site seed folder path
//  the site seed seed folder name
//  the site seed seed folder exists
//  the count of site seed files
// methods
//  initialize
//  Read folder
//  filter collection
// events
//  site seed file Collection updated


namespace AndyConfig.ConfigMgr
{
	public class ConfigSeedSite : INotifyPropertyChanged
	{
	#region private fields


		private static readonly Lazy<ConfigSeedSite> instance =
			new Lazy<ConfigSeedSite>(() => new ConfigSeedSite());

	#endregion

	#region ctor

		private ConfigSeedSite() { }

	#endregion

	#region public properties

		public static ConfigSeedSite Instance => instance.Value;

		public bool Initialized { get; set; }

		public bool SiteSeedFolderExists
		{
			get => Directory.Exists(SiteSeedFolderPath);
		}

		public string SiteSeedFolderPath => SiteSettings.Path.SettingPath + ConfigSeedFileSupport.SEED_FOLDER;

		public bool HasSeedFileSetting => (SiteSettings.Data.InstalledSeedFiles != null &&
			SiteSettings.Data.InstalledSeedFiles.Count > 0);

		public ICollectionView View { get; private set; }

		public FolderAndFileSupport SiteSeedFileList { get; private set; }

		public bool SiteSeedFilesExist => SiteSeedFilesCount > 0;

		public int SiteSeedFilesCount => SiteSeedFileList?.Count ?? 0;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			if (Initialized) return;

			Initialized = true;

			View = CollectionViewSource.GetDefaultView(SiteSettings.Data.InstalledSeedFiles);

			SiteSeedFileList =
				new FolderAndFileSupport(SiteSeedFolderPath, ConfigSeedFileSupport.SEED_PATTERN);

			UpdateProperties();
		}

	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("Initialized");
			OnPropertyChange("View");
			OnPropertyChange("SiteSeedFileList");
			OnPropertyChange("SiteSeedFolderExists");
			OnPropertyChange("SiteSeedFolderPath");
			OnPropertyChange("HasSeedFileSetting");
		}

		private bool ValidateSiteSeedFileExist()
		{
			foreach (ConfigSeedFile file in SiteSettings.Data.InstalledSeedFiles)
			{
				if (!file.FilePath.IsFound) return false;
			}

			return true;
		}

		private void FilterInstalledSeedFiles()
		{
			
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
			return "this is ConfigSeedSite";
		}

	#endregion
	}
}