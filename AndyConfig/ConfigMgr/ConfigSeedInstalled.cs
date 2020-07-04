#region using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using AndyConfig.ConfigMgr;
using AndyShared.FilesSupport;
using SettingsManager;
using UtilityLibrary;
using AndyShared.ConfigSupport;
using AndyShared.Support;

#endregion

// username: jeffs
// created:  6/21/2020 3:18:01 PM

// ConfigInstalledSeedFiles - the Installed Seed Folder and Files
// (as a  static instances)
// properties
//  initialized
//  list of files found in the seed installed folder
//  the collection of files found in the seed installed folder (need to combine with the above)
//  the view of the collection
//  the install folder path
//  the installed seed folder name
//  the installed seed folder exists
//  the count of installed seed files
// methods
//  initialize
//  Read folder
//  filter collection
// events
//  installed seed file Collection updated
//  installed seed file collection view updated


namespace AndyShared.ConfigMgr
{
	public class ConfigSeedInstalled : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<ConfigSeedInstalled> instance =
			new Lazy<ConfigSeedInstalled>(() => new ConfigSeedInstalled());

		private  ObservableCollection<ConfigSeedFile> installedSeedFiles =
			new ObservableCollection<ConfigSeedFile>();

		private ICollectionView installedSeedFileView;

		private FilePath<FileNameSimpleSelectable> installedSeedFilePath;

	#endregion

	#region ctor

		private ConfigSeedInstalled() { }

	#endregion

	#region public properties

		public static ConfigSeedInstalled Instance => instance.Value;

		public bool Initialized { get; set; }

		public string InstallFolder =>
		#if DEBUG
			@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\.sample";
	#else
			Assembly.GetExecutingAssembly().Location;
	#endif

		public string InstallSeedFileFolder => installedSeedFilePath.GetPath;

		public bool InstalledFolderExists => installedSeedFilePath.IsFound;

		public ObservableCollection<ConfigSeedFile> InstalledSeedFiles 
		{
			get => installedSeedFiles;
			private set
			{
				installedSeedFiles = value;
				OnPropertyChange();
			}
	}

		public ICollectionView View
		{
			get => installedSeedFileView;

			private set
			{
				installedSeedFileView = value;

				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			if (Initialized) return;

			Initialized = true;

			installedSeedFilePath = new FilePath<FileNameSimpleSelectable>(
				InstallFolder + ConfigSeedFileSupport.SEED_FOLDER);

			// update the seed file list held by the site setting folder
			UpdateInstalledSeedFileList();

			UpdateView();

			UpdateProperties();
		}

		public void Apply()
		{
			RaiseOnInstalledSeedCollectionUpdatedEvent();
		}

	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("Initialized");
			OnPropertyChange("InstallSeedFileFolder");
			OnPropertyChange("InstalledFolderExists");
			OnPropertyChange("InstalledSeedFiles");
		}

		private void UpdateView()
		{
			View = CollectionViewSource.GetDefaultView(installedSeedFiles);
		}

		// adjust the config file's list based on the actual list in
		// the folder and keep from the list any that no longer exist
		// and add any new files
		// cross reference with current list to determine which
		// files are selected
		private void UpdateInstalledSeedFileList()
		{
			if (InstallSeedFileFolder.IsVoid()) return;

			if (installedSeedFiles == null)
			{
				// empty the list - if found files is none, the
				// actual list will be empty
				installedSeedFiles = new ObservableCollection<ConfigSeedFile>();
			}

			foreach (string file in Directory.EnumerateFiles(InstallSeedFileFolder,
				ConfigSeedFileSupport.SEED_PATTERN, SearchOption.AllDirectories))
			{
				FilePath < FileNameSimpleSelectable > seedFile =
					new FilePath<FileNameSimpleSelectable>(file);

				string key = ConfigFile.MakeKey(Heading.SuiteName,
					seedFile.GetFileNameWithoutExtension);

				ConfigSeedFile found = SiteSettings.Data.InstalledSeedFiles.Find(key);

				if (found != null)
				{
					found.Keep = true;
				}
				else
				{
					ConfigSeedFile seed =
						ConfigSeedFileSupport.MakeConfigSeedFileItem(seedFile, Heading.SuiteName,
							ConfigSeedFileSupport.GetSampleFile(seedFile));

					installedSeedFiles.Add(seed);
				}
			}

			ProcessNotKeptSeedFiles();

			RaiseOnInstalledSeedCollectionUpdatedEvent();
		}

		private void ProcessNotKeptSeedFiles()
		{
			for (int i = installedSeedFiles.Count - 1; i >= 0; i--)
			{
				if (installedSeedFiles[i].Keep == false)
				{
					installedSeedFiles.RemoveAt(i);
				}

				installedSeedFiles[i].Keep = false;
			}
		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public delegate void OnInstalledSeedCollectionUpdatedEventHandler(object sender);

		public event OnInstalledSeedCollectionUpdatedEventHandler OnInstalledSeedCollectionUpdated;

		protected virtual void RaiseOnInstalledSeedCollectionUpdatedEvent()
		{
			OnInstalledSeedCollectionUpdated?.Invoke(this);
		}


	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ConfigSeedInstalled";
		}

	#endregion
	}
}