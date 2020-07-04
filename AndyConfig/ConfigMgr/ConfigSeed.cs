#region using directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AndyShared.ConfigMgr;
using AndyShared.ConfigSupport;

#endregion

// username: jeffs
// created:  6/21/2020 6:34:39 AM

namespace AndyConfig.ConfigMgr
{
	public class ConfigSeed //: INotifyPropertyChanged
	{
	// #region private fields
	//
	// 	private static readonly Lazy<ConfigSeed> instance =
	// 		new Lazy<ConfigSeed>(() => new ConfigSeed());
	//
	// #endregion
	//
	// #region ctor
	//
	// 	private ConfigSeed()
	// 	{
	// 		// SeedInstalled = new ConfigSeedInstalled();
	// 		//
	// 		// SeedSite = new ConfigSeedSite();
	// 		//
	// 		// SeedLocal = ConfigSeedLocal.Instance;
	// 	}
	//
	// #endregion
	//
	// #region public properties
	//
	// 	public static ConfigSeed Instance => instance.Value;
	//
	// 	public bool Initialized { get; set; }
	// 	//
	// 	// public ConfigSeedInstalled SeedInstalled {get; private set; }
	// 	//
	// 	// public ConfigSeedSite SeedSite { get; private set; }
	// 	//
	// 	// public ConfigSeedLocal SeedLocal { get; private set; }
	//
	// #endregion
	//
	// #region private properties
	//
	// #endregion
	//
	// #region public methods
	//
	// 	public void Initialize()
	// 	{
	// 		if (Initialized) return;
	//
	// 		Initialized = true;
	//
	// 		// SeedInstalled.Initialize();
	// 		//
	// 		// SeedSite.Initialize();
	// 		//
	// 		// SeedLocal.Initialize();
	//
	// 		UpdateProperties();
	// 	}
	//
	// #endregion
	//
	// #region private methods
	//
	// 	private void UpdateProperties()
	// 	{
	// 		OnPropertyChange("Initialized");
	// 		OnPropertyChange("SeedInstalled");
	// 		OnPropertyChange("SeedSite");
	// 	}
	//
	// #endregion
	//
	// #region event processing
	//
	// 	public event PropertyChangedEventHandler PropertyChanged;
	//
	// 	private void OnPropertyChange([CallerMemberName] string memberName = "")
	// 	{
	// 		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
	// 	}
	//
	// #endregion
	//
	// #region event handeling
	//
	// #endregion
	//
	// #region system overrides
	//
	// 	public override string ToString()
	// 	{
	// 		return "this is ConfigSeed";
	// 	}
	//
	// #endregion
	}
}