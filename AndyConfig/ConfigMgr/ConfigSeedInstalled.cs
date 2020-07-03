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

		private ICollectionView installedSeedFileView;


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

		public ICollectionView View 
		{
			get => installedSeedFileView;
			set => installedSeedFileView = value;

		}

		public string InstallSeedFileFolder => InstallFolder + ConfigSeed.SEED_FOLDER;

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

			installedSeedFileView = CollectionViewSource.GetDefaultView(SiteSettings.Data.InstalledSeedFiles);

			InstalledSeedFileList =
				new FolderAndFileSupport(InstallSeedFileFolder, ConfigSeed.SEED_PATTERN);

			InstalledSeedFileList.GetFiles();

			UpdateInstalledSeedFileDict();

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
		// the folder and remove from the list any that no longer exist
		// and add any new files
		// cross reference with current list to determine which
		// files are selected
		private void UpdateInstalledSeedFileDict()
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

				// flag to remove all entries - then, un-flag below
				FlagRemoveSeedFileDict();

				// scan through the list of found files and configure
				// the list
				foreach (FilePath<FileNameSimpleSelectable> file in InstalledSeedFileList.FoundFiles)
				{
					string key = ConfigSeedFileSetting.MakeKey(file);

					ConfigSeedFileSetting found = SiteSettings.Data.InstalledSeedFiles.Find(key);

					if (found != null)
					{
						// existing entry found
						// un-flag remove
						found.Remove = false;
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

				ProcessRemovedSeedFileDict();
			}

			SiteSettings.Admin.Write();
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

		private void FlagRemoveSeedFileDict()
		{
			if (SiteSettings.Data.InstalledSeedFiles.Count == 0) return;

			foreach (ConfigSeedFileSetting fileSetting in SiteSettings.Data.InstalledSeedFiles)
			{
				fileSetting.Remove = true;
			}
		}

		private void ProcessRemovedSeedFileDict()
		{
			for (var i = SiteSettings.Data.InstalledSeedFiles.Count - 1; i >= 0; i--)
			{
				if (SiteSettings.Data.InstalledSeedFiles[i].Remove == true)
				{
					SiteSettings.Data.InstalledSeedFiles.RemoveAt(i);
				}
			}


			// foreach (ConfigSeedFileSetting fileSetting in SiteSettings.Data.InstalledSeedFiles)
			// {
			// 	if (fileSetting.Remove == true)
			// 	{
			// 		keysToRemove.Add(fileSetting.Key);
			// 	}
			// }
			//
			// if (keysToRemove.Count != 0 )
			// {
			// 	foreach (string key in keysToRemove)
			// 	{
			// 		ConfigSeedFileSetting fileSetting =
			// 			SiteSettings.Data.InstalledSeedFiles.Find(key);
			//
			// 		SiteSettings.Data.InstalledSeedFiles.Remove(fileSetting);
			//
			// 		SiteSettings.Data.InstalledSeedFiles.
			// 	}
			// }
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