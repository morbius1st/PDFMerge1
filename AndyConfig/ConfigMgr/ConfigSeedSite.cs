#region using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using AndyShared.ClassificationFileSupport;
using AndyShared.ConfigMgr;
using AndyShared.ConfigMgrShared;
using AndyShared.ConfigSupport;
using AndyShared.FilesSupport;
using AndyShared.Support;
using SettingsManager;
using UtilityLibrary;

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

		/// <summary>
		/// the complete list of and disposition of site seed files<br/>
		/// that is, if listed as a selected installed seed file - keep<br/>
		/// if listed as a un-selected installed seed file - delete<br/>
		/// if other, keep
		/// </summary>
		private  ObservableCollection<ConfigSeedFile> siteSeedFiles =
			new ObservableCollection<ConfigSeedFile>();

		private ICollectionView siteSeedFileView;

		private FilePath<FileNameSimple> siteSeedFolderPath;

		private bool siteSeedFilesFound;

	#endregion

	#region ctor

		private ConfigSeedSite() { }

	#endregion

	#region public properties

		public static ConfigSeedSite Instance => instance.Value;

		public bool Initialized { get; set; }

		/// <summary>
		/// path to the site settings folder
		/// </summary>
		public string SiteSettingsFolderPath => ConfigSite.Instance.SiteSettingsFolderPath;

		/// <summary>
		/// path of the site seed folder
		/// </summary>
		public string SiteSeedFolderPath => siteSeedFolderPath?.GetPath ?? null;

		/// <summary>
		/// verify that the site seed folder exists
		/// </summary>
		public bool SiteSeedFolderPathExists => Directory.Exists(SiteSeedFolderPath);

		public bool HasSeedFileSetting => (ConfigSite.Instance.SiteInstalledSeedFiles != null &&
			ConfigSite.Instance.SiteInstalledSeedFiles.Count > 0);

		public ICollectionView View
		{
			get => siteSeedFileView;
			// must be public
			set
			{
				siteSeedFileView = value;

				OnPropertyChange();
			}
		}

		public bool SiteSeedFilesFound
		{
			get => siteSeedFilesFound;
			private set
			{
				siteSeedFilesFound = value;

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

			siteSeedFolderPath = new FilePath<FileNameSimple>(
				SiteSettingsFolderPath + ConfigSeedFileSupport.SEED_FOLDER_SITE);

			UpdateCollection();
		}

		public void Apply()
		{
			// run through the list: siteSeedFiles and process as needed
			// (3) possibilities:
			// status == remove - erase the seed file and associated data file
			//    from the site seed folder
			// status == copy - copy the seed file and associated data file
			//    to the site seed folder
			// status == ok as is - do nothing

			// copy or erase from this folder: SiteSettingsFolderPath

			foreach (ConfigSeedFile seed in siteSeedFiles)
			{
				if (seed.Status != SeedFileStatus.COPY &&
					seed.Status != SeedFileStatus.REMOVE) continue;

				string target1 =
					SiteSeedFolderPath + FilePathUtil.PATH_SEPARATOR +
					seed.FilePathLocal.GetFileName;

				string target2 =
					ClassificationFileAssist.GetSampleFile(SiteSeedFolderPath,
						seed.FilePathLocal.GetFileNameWithoutExtension, true);

				if (seed.Status == SeedFileStatus.COPY)
				{
					File.Copy(seed.FilePathLocal.GetFullFilePath, target1);
					if (target2 != null)
					{
						File.Copy(seed.SampleFile.GetFullFilePath, target2);
					}

					// #if DEBUG
					// 	Debug.WriteLine(">>>");
					// 	Debug.WriteLine(
					// 		"copy | " +
					// 		seed.FilePath.GetFullFilePath +
					// 		"  > to > " + target1);
					// 	Debug.WriteLine(
					// 		"copy | " +
					// 		seed.SampleFile.GetFullFilePath +
					// 		"  > to > " + target2 ?? "is null");
					// 	Debug.WriteLine("<<<\n");
					// #endif

				}
				else if (seed.Status == SeedFileStatus.REMOVE)
				{
					File.Delete(target1);
					if (target2 != null)
					{
						File.Delete(target2);
					}

					// #if DEBUG
					// 	Debug.WriteLine(">>>");
					// 	Debug.WriteLine(
					// 		"remove | " + target1);
					// 	Debug.WriteLine(
					// 		"remove | " + target2 ?? "is null");
					// 	Debug.WriteLine("<<<\n");
					// #endif
					// 	
				}
			}

			UpdateCollection();

			RaiseOnSeedSiteCollectionUpdatedEvent();
		}

	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("Initialized");
			OnPropertyChange("SiteSettingsFolderPath");
			OnPropertyChange("SiteSeedFolderPath");
			OnPropertyChange("SiteSeedFolderPathExists");
			OnPropertyChange("HasSeedFileSetting");
		}

		private void UpdateCollection()
		{
			updateSiteSeedFiles();

			UpdateView();

			UpdateProperties();

		}

		private void updateSiteSeedFiles()
		{
			if (!SiteSeedFolderPathExists) return;

			siteSeedFiles = new ObservableCollection<ConfigSeedFile>();

			// part 1: enumerate the files in the site seed file folder
			// add to site seed file collection, all of the
			// seed files in the site seed folder
			CreateListOfAllSiteSeedFiles();

			// get the index of the last 
			// item in the list- for this exercise, don't
			// sort or re-order the list after this point
			int last = siteSeedFiles.Count - 1;

			ValidateListOfAllSiteSeedFiles(last);
		}

		private void ValidateListOfAllSiteSeedFiles(int last)
		{
			// scan through the list of installed & selected seed
			// files and validate / update the collection:
			// if the installed list:
			// if the file is selected
			//   if the file is not in the current list - add & flag keep & selected
			//   if the file is in the current list - nothing - flag keep & selected
			// if the file is not selected
			//   if the file is in the current list - flag keep = false;
			//   if the file is not in the list - nothing
			// re-purpose selected as copy to the folder
			// flag keep == no purpose
			// flag local == no purpose

			ConfigSeedFile found;

			foreach (ConfigSeedFile seed in ConfigSite.Instance.SiteInstalledSeedFiles)
			{
				found = siteSeedFiles.Find(seed.Key, last);

				// if installed is selected
				if (seed.SelectedSeed)
				{
					if (found == null)
					{
						found = seed.Clone() as ConfigSeedFile;
						// found.Keep = true;

						// allow the user to make the final determination
						// found.Copy = true;
						found.SelectedSeed = true;
						found.Status = SeedFileStatus.COPY;

						siteSeedFiles.Add(found);
					}


					// else
					// {
					// 	// found is not null - do nothing
					// 	// found.Keep = true;  // redundant - flagged below
					// 	// found.Copy = false; // redundant - flagged below
					// }
				}
				else
				{
					// installed seed file is not selected
					if (found != null)
					{
						// but it is in the folder
						// need to flag remove
						// found.Keep = false;
						found.SelectedSeed = true;
						found.Status = SeedFileStatus.REMOVE;
						// found.Copy = false; // redundant - flagged below
					}

					// else
					// {
					//  // the file is not in the list - do nothing
					// }
				}
			}
		}

		private void CreateListOfAllSiteSeedFiles()
		{
			foreach (string file in Directory.GetFiles(SiteSeedFolderPath, ConfigSeedFileSupport.SEED_PATTERN,
				SearchOption.AllDirectories))
			{
				FilePath<FileNameSimpleSelectable> filePath =
					new FilePath<FileNameSimpleSelectable>(file);

				ConfigSeedFile seed  = ConfigSeedFileSupport.MakeConfigSeedFileItem(
					filePath, Heading.SuiteName, ClassificationFileAssist.GetSampleFile(filePath));

				// changes to these means the above must be adjusted
				// pre-select to keep
				// seed.Keep = true;
				// re-purpose selected as copy to the folder
				// so all of these are false
				// seed.Copy = false;

				seed.SelectedSeed = false;
				seed.Local = true;
				seed.Status = SeedFileStatus.OK_AS_IS;

				ConfigSeedFile found =
					ConfigSite.Instance.SiteInstalledSeedFiles.Find(seed.Key);

				siteSeedFiles.Add(seed);
			}
		}

		private void UpdateView()
		{
			View = CollectionViewSource.GetDefaultView(siteSeedFiles);

			View.SortDescriptions.Clear();
			View.SortDescriptions.Add(new SortDescription("FileName", ListSortDirection.Ascending));
		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public delegate void OnSeedSiteCollectionUpdatedEventHandler(object sender);

		public event ConfigSeedSite.OnSeedSiteCollectionUpdatedEventHandler OnSeedSiteCollectionUpdated;

		protected virtual void RaiseOnSeedSiteCollectionUpdatedEvent()
		{
			OnSeedSiteCollectionUpdated?.Invoke(this);
		}

	#endregion

	#region event handeling

		public void OnInstalledSeedFileCollectionChanged(object sender)
		{
			UpdateCollection();
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ConfigSeedSite";
		}

	#endregion
	}
}