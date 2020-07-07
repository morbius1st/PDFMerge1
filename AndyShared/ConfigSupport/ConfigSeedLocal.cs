using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using AndyShared.FilesSupport;
using SettingsManager;
using UtilityLibrary;

namespace AndyShared.ConfigSupport
{
	// manages the local seed file configuration
	// provides a list of seed files available to the
	// local user via the suite file / suite seed folder
	// this is a combination of the seed files 
	// from the site configuration - and -
	// stored locally
	// must get this list from site
	// must develop a list from local
	// needs the local seed folder: LocalSeedFileFolder
	// does not maintain a list - everything stored
	// locally is available


// ConfigLocalSeedFiles - the Local Seed Folder and Files + 
//	the composite list of seed files (site + local)
// (as a  static instances)
// properties
//  initialized
//  the list of files found in the local seed folder
//  the collection of files in the local seed folder + the selected (and found) site seed file
//  the view of the collection (all files) 
//  the local seed folder path
//  the local seed seed folder name
//  the local seed seed folder exists
//  the count of local seed files 
// methods
//  initialize
//  Read folder
//  GetItem(key)
//  ItemExists(key)
// subscribe events
//  site seed: collection updated


	public class ConfigSeedLocal : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<ConfigSeedLocal> instance =
			new Lazy<ConfigSeedLocal>(() => new ConfigSeedLocal());

		private  ObservableCollection<ConfigSeedFile> seedFiles =
			new ObservableCollection<ConfigSeedFile>();

		private ICollectionView localSeedFileView;

		private FilePath<FileNameSimple> localSeedFolderPath;

	#endregion

	#region ctor

		private ConfigSeedLocal() {}

	#endregion

	#region public properties

		public static ConfigSeedLocal Instance => instance.Value;

		public bool Initialized { get; set; }

		/// <summary>
		/// path to the local seed folder
		/// </summary>
		public string LocalSeedFolderPath => localSeedFolderPath.GetPath;

		public bool LocalSeedFolderPathExists => Directory.Exists(LocalSeedFolderPath);

		public  ObservableCollection<ConfigSeedFile> SeedFiles
		{
			get => seedFiles;
			private set
			{
				seedFiles = value;
				OnPropertyChange();
			}
		}

		public ICollectionView View
		{
			get => localSeedFileView;
			// mst be public
			set
			{
				localSeedFileView = value;

				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			Initialized = true;

			localSeedFolderPath = new FilePath<FileNameSimple>(
				SuiteSettings.Path.SettingFolderPath + ConfigSeedFileSupport.SEED_FOLDER_SUITE);

			UpdateCollection();

			OnPropertyChange("Initialized");

			UpdateProperties();
		}

		public void UpdateCollection()
		{

			// GetInstalledSeedFiles();
			GetSiteSeedFiles();
			GetLocalSeedFiles();

			UpdateView();

			UpdateViewProperties();
		}

	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("LocalSeedFolderPath");
			OnPropertyChange("LocalSeedFolderPathExists");
		}

		private void UpdateViewProperties()
		{
			seedFiles = new ObservableCollection<ConfigSeedFile>();

			OnPropertyChange("SeedFiles");
			OnPropertyChange("View");
		}

		private void UpdateView()
		{
			View = CollectionViewSource.GetDefaultView(SeedFiles);

			View.SortDescriptions.Clear();
			View.SortDescriptions.Add(new SortDescription("FileName", ListSortDirection.Ascending));
		}

		private void GetLocalSeedFiles()
		{
			bool result = ConfigSeedFileSupport.GetFiles(seedFiles,
				localSeedFolderPath.GetPath, ConfigSeedFileSupport.SEED_PATTERN, 
				SearchOption.AllDirectories, false, true);
		}

		private void GetInstalledSeedFiles()
		{
			if (SiteSettings.Data.InstalledSeedFiles.Count == 0) return;

			foreach (ConfigSeedFile installedFile in SiteSettings.Data.InstalledSeedFiles)
			{
				if (installedFile.Selected)
				{
					seedFiles.Add(installedFile);
				}
			}
		}

		private void GetSiteSeedFiles()
		{
			string siteSeedFilesFolderpath =
				SiteSettings.Path.SettingFolderPath + FilePathUtil.PATH_SEPARATOR
				+ ConfigSeedFileSupport.SEED_FOLDER_SITE;

			bool result = ConfigSeedFileSupport.GetFiles(seedFiles, siteSeedFilesFolderpath);
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

		public void OnSeedSiteCollectionUpdated(object sender)
		{
			UpdateCollection();
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ConfigSeedLocal";
		}

	#endregion
	}
}