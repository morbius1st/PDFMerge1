using AndyShared.ConfigSupport;

using System.Collections.ObjectModel;
using System.Runtime.Serialization;

// Solution:     SettingsManager
// Project:       SettingsManagerV70
// File:             SiteSettings.cs
// Created:      -- ()

namespace SettingsManager
{
	#region info class

	[DataContract(Name = "SiteSettings", Namespace = "")]
	internal class SiteSettingInfo<T> : SiteSettingInfoBase<T>
		where T : new()
	{
		public SiteSettingInfo()
		{
			DataClassVersion = "7.0as";
			Description = "site setting file for SettingsManagerV70";
		}


#pragma warning disable CS0436 // The type 'SettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SettingInfoBase<TData>' in 'WpfShared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
		internal override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
#pragma warning restore CS0436 // The type 'SettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SettingInfoBase<TData>' in 'WpfShared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
	}

	#endregion

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	internal class SiteSettingData
	{
		[DataMember(Order = 1)]
		public ObservableCollection<ConfigSeedFile> InstalledSeedFiles { get; set; }
			= new ObservableCollection<ConfigSeedFile>()
			{
				{
					new ConfigSeedFile("Andy", "No User", "Seed Files", "Basic", "", false, false, false,
						SeedFileStatus.IGNORE)
				}
			};

		// public SortedDictionary<string, ConfigSeedFile> InstalledSeedFiles { get; set; }
		//			= new Dictionary<string, ConfigSeedFile>()
		//			{
		//				{"Andy :: No User",  new ConfigSeedFile("Andy", "No User", "Seed Files", "Basic", "")}
		//			};
		//
		//
		// public string MakeKey(string userName, string id)
		// {
		// 	return userName + " :: " + id;
		// }
	}

	#endregion
}