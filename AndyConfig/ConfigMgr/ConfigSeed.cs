#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using AndyConfig.ConfigMgr;
using AndyShared.ConfigSupport;
using AndyShared.FilesSupport;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  6/21/2020 6:34:39 AM

namespace AndyShared.ConfigMgr
{
	public class ConfigSeed : INotifyPropertyChanged
	{
	#region private fields



	#endregion

	#region ctor

		public ConfigSeed()
		{
			SeedInstalled = new ConfigSeedInstalled();

			SeedSite = new ConfigSeedSite();

			SeedLocal = new ConfigSeedLocal();
		}

	#endregion

	#region public properties

		public bool Initialized { get; set; }

		public ConfigSeedInstalled SeedInstalled {get; private set; }

		public ConfigSeedSite SeedSite { get; private set; }

		public ConfigSeedLocal SeedLocal { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			if (Initialized) return;

			Initialized = true;

			SeedInstalled.Initialize();

			SeedSite.Initialize();

			SeedLocal.Initialize();

			UpdateProperties();
		}

	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("Initialized");
			OnPropertyChange("SeedInstalled");
			OnPropertyChange("SeedSite");
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
			return "this is ConfigSeed";
		}

	#endregion
	}
}