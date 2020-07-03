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

namespace AndyShared.ConfigMgr
{
	public class ConfigSeedInstalled : INotifyPropertyChanged
	{
	#region private fields

	#endregion

	#region ctor

		public ConfigSeedInstalled() { }

	#endregion

	#region public properties

		public bool Initialized { get; set; }

		public string InstallFolder =>
		#if DEBUG
			@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\.sample";
	#else
			Assembly.GetExecutingAssembly().Location;
	#endif

		public FolderAndFileSupport InstalledSeedFileList { get; private set; }

		public ObservableCollection<ConfigSeedFileSetting> InstalledSeedFiles => SiteSettings.Data.InstalledSeedFiles;

		public ICollectionView View { get; private set; }

		public string InstallSeedFileFolder => InstallFolder + ConfigSeedSite.SEED_FOLDER;

		public bool InstalledFolderExists => InstalledSeedFileList?.FolderExists ?? false;

		public bool InstalledSeedFilesExist => InstalledSeedFilesCount > 0;

		public int InstalledSeedFilesCount => InstalledSeedFileList?.Count ?? 0;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			Initialized = true;

			View = CollectionViewSource.GetDefaultView(SiteSettings.Data.InstalledSeedFiles);

			InstalledSeedFileList =
				new FolderAndFileSupport(InstallSeedFileFolder, ConfigSeedSite.SEED_PATTERN);

			InstalledSeedFileList.GetFiles();

			UpdateInstalledSeedFileList();

			Update();

			UpdateProperties();
		}

		public void Update()
		{
			SiteSettings.Admin.Write();
		}


	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("Initialized");
			OnPropertyChange("View");
			OnPropertyChange("InstalledFolderExists");
			OnPropertyChange("InstalledSeedFilesExist");
			OnPropertyChange("InstalledSeedFilesCount");
			OnPropertyChange("InstalledSeedFileList");
			OnPropertyChange("InstalledSeedFiles");
		}


		// adjust the config file's list based on the actual list in
		// the folder and keep from the list any that no longer exist
		// and add any new files
		// cross reference with current list to determine which
		// files are selected
		private void UpdateInstalledSeedFileList()
		{
			if (InstalledSeedFileList.Folder == FilePath<FileNameSimpleSelectable>.Invalid
				|| InstalledSeedFileList.Count == 0)
			{
				// empty the list - if found files is none, the
				// actual list will be empty
				SiteSettings.Data.InstalledSeedFiles = new ObservableCollection<ConfigSeedFileSetting>();
				
				OnPropertyChange("InstalledSeedFiles");
			}
			else
			{
				if (SiteSettings.Data.InstalledSeedFiles == null)
				{
					SiteSettings.Data.InstalledSeedFiles = new ObservableCollection<ConfigSeedFileSetting>();
				}

				// flag to keep all entries - then, un-flag below
				// FlagRemoveSeedFiles();

				// scan through the list of found files and configure
				// the list
				foreach (FilePath<FileNameSimpleSelectable> file in InstalledSeedFileList.FoundFiles)
				{
					string key = ConfigSeedFileSetting.MakeKey(file);

					ConfigSeedFileSetting found = SiteSettings.Data.InstalledSeedFiles.Find(key);

					if (found != null)
					{
						// existing entry found
						// un-flag keep
						found.Keep = true;
					}
					else
					{
						// existing entry not found
						// add item
						SiteSettings.Data.InstalledSeedFiles.Add(
							ConfigSeedFileSetting.MakeSeedItem(
								file,
								Heading.SuiteName,
								GetInstalledSampleFile(file)));
					}
				}

				ProcessNotKeptSeedFiles();
			}
		}

		private string GetInstalledSampleFile(FilePath<FileNameSimpleSelectable> file)
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

		private void FlagRemoveSeedFiles()
		{
			if (SiteSettings.Data.InstalledSeedFiles.Count == 0) return;

			foreach (ConfigSeedFileSetting fileSetting in SiteSettings.Data.InstalledSeedFiles)
			{
				fileSetting.Keep = true;
			}
		}

		private void ProcessNotKeptSeedFiles()
		{
			for (var i = SiteSettings.Data.InstalledSeedFiles.Count - 1; i >= 0; i--)
			{
				if (SiteSettings.Data.InstalledSeedFiles[i].Keep == false)
				{
					SiteSettings.Data.InstalledSeedFiles.RemoveAt(i);
				}

				SiteSettings.Data.InstalledSeedFiles[i].Keep = false;
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
			return "this is ConfigSeedInstalled";
		}

	#endregion
	}
}