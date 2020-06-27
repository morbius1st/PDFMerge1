#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SettingsManager;

#endregion

// username: jeffs
// created:  6/19/2020 7:08:28 PM

namespace AndyConfig.ConfigMgr
{
	public class ConfigManager : INotifyPropertyChanged
	{
	#region private fields

	#endregion

	#region ctor

		public ConfigManager()
		{


		}

	#endregion

	#region public properties

		public ConfigSuite Suite { get; private set; } = new ConfigSuite();

		public ConfigSite Site => Suite?.Site ?? null;

		public ConfigSeed Seed => Site?.Seed ?? null;

		public ConfigSeedInstalled SeedInstalled => Seed?.SeedInstalled ?? null;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void SuiteWrite()
		{
			Suite.Write();

			Site.SiteSettingsRootPath = Suite.SiteRootPath;

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