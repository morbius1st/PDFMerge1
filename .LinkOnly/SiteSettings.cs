using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using AndyShared.ConfigSupport;

// Solution:     SettingsManager
// Project:       SettingsManagerV70
// File:             SiteSettings.cs
// Created:      -- ()

namespace SettingsManager
{
	// #region info class
	//
	// [DataContract(Name = "SiteSettings", Namespace = "")]
	// public class SiteSettingInfo<T> : SiteSettingInfoBase<T>
	// 	where T : new()
	// {
	// 	public SiteSettingInfo()
	// 	{
	// 		DataClassVersion = "7.0as";
	// 		Description = "site setting file for SettingsManagerV70";
	// 	}
	//
	//
	// 	public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	// }
	//
	// #endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SiteSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "site 7.4si";

		[IgnoreDataMember]
		public string DataFileDescription => "site setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "site / any notes go here";

		[DataMember(Order = 1)]
		public ObservableCollection<ConfigSeedFile> InstalledSeedFiles { get; set; }
			= new ObservableCollection<ConfigSeedFile>()
			{
				{
					new ConfigSeedFile("Andy", "No User", "Seed Files", "Basic", "", false, false, false,
						SeedFileStatus.IGNORE)
				}
			};

		[DataMember(Order = 2)]
		public ObservableCollection<string> AdminUsers { get; set; } = new ObservableCollection<string>()
		{
			"jeffs"
		};


	}

#endregion
}