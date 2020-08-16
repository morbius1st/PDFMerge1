#region using directives
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AndyConfig.ConfigMgr;
using SettingsManager;
using AndyShared.ConfigSupport;

#endregion

// username: jeffs
// created:  6/20/2020 9:46:37 PM

// ConfigSite - the SiteSettingsFile configuration routines
// (as a  static instances)
// properties
//  initialized
//  the name of the site setting file (actual)
//  the root path to the site setting file (actual)
//  the full path to the site setting file (actual)
//  flag: the site settings file exists
//  SiteSettingsFile.Info
// methods
//  initialize
//  read()
//  write()
//  Write seed file list (** add)
// subscribed event
//  from suite: site folder changed
//
// ** do not include the seed object(s) (remove)
// 


namespace AndyShared.ConfigMgr
{
	public class ConfigSite : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<ConfigSite> instance =
			new Lazy<ConfigSite>(() => new ConfigSite());

	#endregion

	#region ctor

		private ConfigSite() { }

	#endregion

	#region public properties

		public static ConfigSite Instance => instance.Value;

		// site
		public static bool Initalized { get; private set; }

		public string SiteSettingsRootPath
		{
			get
			{
				return SiteSettings.Path.RootFolderPath;
			}
			set
			{
				SiteSettings.Path.RootFolderPath = value;
				OnPropertyChange();

				SiteSettings.Admin.Read();
				
			}
		}

		public string SiteSettingsFileName => SiteSettings.Path.FileName;

		public string SiteSettingsFolderPath => SiteSettings.Path.SettingFolderPath;

		public bool SiteSettingsFileExists => SiteSettings.Path.Exists;

		public SiteSettingInfo<SiteSettingData> Info => SiteSettings.Info;

		public ObservableCollection<ConfigSeedFile> SiteInstalledSeedFiles {
			get => SiteSettings.Data.InstalledSeedFiles;

			set
			{
				
				SiteSettings.Data.InstalledSeedFiles = value;
				OnPropertyChange();
			}
	}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize(string rootPath)
		{
			if (Initalized) return;

			Initalized = true;

			RootPathChanged(rootPath);

			OnPropertyChange("Initalized");
		}

		public void SuiteOnOnSiteRootPathChanged(object sender, PathChangedEventArgs e)
		{
			RootPathChanged(e.Path);
		}

		public void Read()
		{
			SiteSettings.Admin.Read();
		}

		public void Write()
		{
			SiteSettings.Admin.Write();
		}


	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("SiteSettingsFileExists");
			OnPropertyChange("SiteSettingsRootPath");
			OnPropertyChange("SiteSettingsFileName");
			OnPropertyChange("SiteSettingsFilePath");
			OnPropertyChange("Info");
		}


		private void RootPathChanged(string rootPath)
		{
			SiteSettings.Path.RootFolderPath = rootPath;

			if (!SiteSettings.Path.SettingFolderPathIsValid) return;

			Read();

			UpdateProperties();

		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public delegate void OnInstalledSeedFileCollectionChangedEventHandler(object sender);
		
		public event ConfigSite.OnInstalledSeedFileCollectionChangedEventHandler OnInstalledSeedFileCollectionChanged;
		
		protected virtual void RaiseOnInstalledSeedFileCollectionChangedEvent()
		{
			OnInstalledSeedFileCollectionChanged?.Invoke(this);
		}


	#endregion

	#region event handeling

		public void OnInstalledSeedCollectionUpdated(object sender)
		{
			OnPropertyChange("SiteInstalledSeedFiles");

			Write();

			RaiseOnInstalledSeedFileCollectionChangedEvent();
		}
		

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ConfigSite";
		}

	#endregion
	}
}