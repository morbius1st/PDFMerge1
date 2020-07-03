using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;
using AndyShared.ConfigMgr;
using AndyShared.FilesSupport;
using AndyShared.Resources.XamlResources;

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


	public class ConfigSeedLocal : INotifyPropertyChanged
	{
	#region private fields

		private static int idx;

		private  ObservableCollection<ConfigSeedFileSetting> seedFiles = new ObservableCollection<ConfigSeedFileSetting>();

		private ICollectionView localSeedFileView;


		public int index { get; set; }
	#endregion

	#region ctor

	#endregion

	#region public properties

		public bool Initialized { get; set; }

		public string LocalSeedFileFolder => SuiteSettings.Path.SettingPath + ConfigSeed.SEED_FOLDER;

		public FolderAndFileSupport LocalSeedFilesList { get; private set; }

		public  ObservableCollection<ConfigSeedFileSetting> LocalSeedFiles
		{
			get => seedFiles;
			set
			{
				seedFiles = value;
				OnPropertyChange();
			}
		}

		public ICollectionView View
		{
			get => localSeedFileView;
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

			GetSeedFileList();
		}

		public void GetSeedFileList()
		{
			GetInstalledSeedFiles();
			GetLocalSeedFiles();

			View = CollectionViewSource.GetDefaultView(LocalSeedFiles);

			UpdateProperties();
		}

	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("Initialized");
			OnPropertyChange("LocalSeedFileFolder");
			OnPropertyChange("LocalSeedFilesList");
			OnPropertyChange("LocalSeedFiles");
		}

		private void GetLocalSeedFiles()
		{
			LocalSeedFilesList =
				new FolderAndFileSupport(LocalSeedFileFolder, ConfigSeed.SEED_PATTERN);

			LocalSeedFilesList.GetFiles();

			if (LocalSeedFilesList.Count == 0) return;

			foreach (FilePath<FileNameSimpleSelectable> file in LocalSeedFilesList.FoundFiles)
			{
				string key = ConfigSeedFileSetting.MakeKey(file);

				ConfigSeedFileSetting seed = 
					ConfigSeedFileSetting.MakeSeedItem(file, Heading.SuiteName,
						getLocalSampleFile(file));

				seed.Local = true;

				seedFiles.Add(seed);
			}

		}

		private void GetInstalledSeedFiles()
		{
			if (SiteSettings.Data.InstalledSeedFiles.Count == 0) return;

			foreach (ConfigSeedFileSetting installedFile in SiteSettings.Data.InstalledSeedFiles)
			{
				if (installedFile.Selected)
				{
					// ConfigSeedFileSetting copy = installedFile.Clone() as ConfigSeedFileSetting;

					seedFiles.Add(installedFile);
				}
			}
		}

		private string getLocalSampleFile(FilePath<FileNameSimpleSelectable> file)
		{
			string sampleFile =
				file.GetPath + @"\" + file.GetFileNameObject.Name + @".dat";

			bool exists = File.Exists(sampleFile);

			if (!exists)
			{
				sampleFile = null;
			}

			return sampleFile;
		}


	#endregion

	#region event processing

		public ConfigSeedLocal()
		{
			index = idx++;


		}

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
			return "this is ConfigSeedLocal";
		}

	#endregion
	}
}
