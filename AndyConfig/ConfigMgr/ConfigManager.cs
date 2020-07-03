#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AndyShared.ConfigSupport;
using SettingsManager;

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

		public ConfigManager()
		{
			Suite = new ConfigSuite();

		}

	#endregion

	#region public properties

		/// <summary>
		/// <c>ConfigSuite</c><br/>
		/// SuiteSettingsFileExists   : bool    : does the suite settings file exist<br/>
		/// (rename the below)
		/// SiteSettingsRootPath      : string  : site setting root folder path (from the suite setting file)<br/>
		/// SiteSettingsRootPathIsValid: bool   : (not) is 'SiteRootPath' null or empty<br/>
		/// SuiteSettingFileName      : string  : site settings file name<br/>
		/// Site                      : ConfigSite: config site file object<br/>
		/// </summary>
		public ConfigSuite Suite { get; private set; } 

		/// <summary>
		/// <c>ConfigSite</c><br/>
		/// SiteSettingsFileExists    : bool    : site settings file exists<br/>
		/// SiteSettingsRootPath      : string  : site setting root folder path (from the site settings file)<br/>
		/// SiteSettingsFileName      : string  : site settings file name (from the site settings file)<br/>
		/// SiteSettingsFilePath      : string  : site setting file path (not used)<br/>
		/// Seed                      : ConfigSeed: config seed file object<br/>
		/// </summary>
		public ConfigSite Site => Suite?.Site ?? null;

		/// <summary>
		/// <c>ConfigSeed</c><br/>
		/// SiteSettingsSeedFolderPath: string  : site seed folder path<br/>
		/// HasSeedFileSetting        : bool    : has site installed seed files<br/>
		/// SeedInstalled             : ConfigSeedInstalled: config installed seed file object<br/>
		/// </summary>
		public ConfigSeed Seed => Site?.Seed ?? null;

		/// <summary>
		/// <c>ConfigSeedInstalled</c><br/>
		/// InstallFolder             : string  : app install folder<br/>
		/// InstallSeedFileFolder     : string  : the app install seed folder<br/>
		/// InstalledSeedFileList     : FolderAndFileSupport: collection of site seed files<br/>
		/// InstalledFolderExists     : bool    : site seed folder exists<br/>
		/// InstalledSeedFilesCount   : int     : count of installed seed files<br/>
		/// </summary>
		public ConfigSeedInstalled SeedInstalled => Seed?.SeedInstalled ?? null;

		/// <summary>
		/// <c>ConfigSeedLocal</c><br/>
		/// LocalSeedFileFolder        : FolderAndFileSupport  : folder for the local seed files<br/>
		/// </summary>
		public ConfigSeedLocal SeedLocal => Seed?.SeedLocal ?? null;

	#endregion

	#region private properties

	#endregion

	#region public methods

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