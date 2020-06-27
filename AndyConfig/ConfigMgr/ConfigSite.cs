#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using SettingsManager;

#endregion

// username: jeffs
// created:  6/20/2020 9:46:37 PM

namespace AndyConfig.ConfigMgr
{
	public class ConfigSite : INotifyPropertyChanged
	{
	#region private fields

	#endregion

	#region ctor

		public ConfigSite() { }

	#endregion

	#region public properties

		// site
		public static bool Initalized { get; private set; }

		public bool SiteSettingsFileExists => SiteSettings.Path.Exists;

		public string SiteSettingsRootPath
		{
			get
			{
				return SiteSettings.Path.RootPath;
			}
			set
			{
				SiteSettings.Path.RootPath = value;
				OnPropertyChange();

				SiteSettings.Admin.Read();
				
			}
		}

		public string SiteSettingsFileName => SiteSettings.Path.FileName;

		public string SiteSettingsFilePath => SiteSettings.Path.SettingPath;

		public SiteSettingInfo70<SiteSettingData70> Info => SiteSettings.Info;

		public ConfigSeed Seed { get; private set; } = new ConfigSeed();

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Read()
		{
			SiteSettings.Admin.Read();

			Initalized = true;

			UpdateProperties();

			Seed.Initialize();
		}

		public void UpdateSeedFileList()
		{

		}

	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("Initalized");
			OnPropertyChange("SiteSettingsFileExists");
			OnPropertyChange("SiteSettingsRootPath");
			OnPropertyChange("SiteSettingsFileName");
			OnPropertyChange("SiteSettingsFilePath");
			OnPropertyChange("Info");

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
			return "this is ConfigSite";
		}

	#endregion
	}
}