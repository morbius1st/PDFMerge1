﻿#region using directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using AndyConfig.ConfigMgr;
using SettingsManager;
using AndyShared.ClassificationFileSupport;
using AndyShared.ConfigMgrShared;

#endregion

// username: jeffs
// created:  6/19/2020 7:08:28 PM




namespace AndyShared.ConfigMgr
{
	public class ConfigManager : INotifyPropertyChanged
	{
	#region private fields

	#endregion

	#region ctor

	#endregion

	#region public properties

		public bool Initalized { get; private set; }

		/// <summary>
		/// <c>ConfigSuite</c><br/>
		/// SuiteSettingsFileExists   : bool    : &#160;&#160;&#160;does the suite settings file exist<br/>
		/// (rename the below)
		/// SiteSettingsRootPath      : string  : site setting root folder path (from the suite setting file)<br/>
		/// SiteSettingsRootPathIsValid: bool   : (not) is 'SiteRootPath' null or empty<br/>
		/// SuiteSettingFileName      : string  : site settings file name<br/>
		/// Site                      : ConfigSite: config site file object<br/>
		/// </summary>
		public ConfigSuite Suite { get; private set; } = ConfigSuite.Instance;

		/// <summary>
		/// <c>ConfigSite</c><br/>
		/// SiteSettingsFileExists    : bool    : site settings file exists<br/>
		/// SiteSettingsRootPath      : string  : site setting root folder path (from the site settings file)<br/>
		/// SiteSettingsFileName      : string  : site settings file name (from the site settings file)<br/>
		/// SiteSettingsFilePath      : string  : site setting file path (not used)<br/>
		/// Seed                      : ConfigSeed: config seed file object<br/>
		/// </summary>
		public ConfigSite Site { get; private set; } = ConfigSite.Instance;

		/// <summary>
		/// <c>ConfigSeedInstalled</c><br/>
		/// InstallFolder             : string  : app install folder<br/>
		/// InstallSeedFileFolder     : string  : the app install seed folder<br/>
		/// InstalledSeedFileList     : FolderAndFileSupport: collection of site seed files<br/>
		/// InstalledFolderExists     : bool    : site seed folder exists<br/>
		/// InstalledSeedFilesCount   : int     : count of installed seed files<br/>
		/// </summary>
		public ConfigSeedInstalled SeedInstalled { get; private set; } = ConfigSeedInstalled.Instance;

		public ConfigSeedSite SeedSite { get; private set; } = ConfigSeedSite.Instance;

		/// <summary>
		/// <c>ConfigSeedLocal</c><br/>
		/// LocalSeedFileFolder        : FolderAndFileSupport  : folder for the local seed files<br/>
		/// </summary>
		public ConfigSeedLocal SeedLocal { get; private set; } = ConfigSeedLocal.Instance;

		public ClassificationFiles ClassificationFiles { get; private set; } = ClassificationFiles.Instance;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{

			if (Initalized) return;

			Initalized = true;

			// this class is independent / stand alone
			ClassificationFiles.Initialize();

			// the below are interrelated to each other

			// Suite = ConfigSuite.Instance;

			Suite.Initialize();
			Suite.OnSiteRootPathChanged += Site.SuiteOnSiteRootPathChanged;

			// Site = ConfigSite.Instance;
			Site.Initialize(Suite.SiteSettingsRootPath);
			Site.OnInstalledSeedFileCollectionChanged += SeedSite.OnInstalledSeedFileCollectionChanged;


			// SeedInstalled = ConfigSeedInstalled.Instance;
			SeedInstalled.Initialize();
			SeedInstalled.OnInstalledSeedCollectionUpdated += Site.OnInstalledSeedCollectionUpdated;


			// SeedSite = ConfigSeedSite.Instance;
			SeedSite.Initialize();
			SeedSite.OnSeedSiteCollectionUpdated += SeedLocal.OnSeedSiteCollectionUpdated;


			// SeedLocal = ConfigSeedLocal.Instance;
			SeedLocal.Initialize();


		}

	#endregion

	#region private methods

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
			return "this is ConfigManager";
		}

	#endregion
	}
}