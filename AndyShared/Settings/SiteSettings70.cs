using System.Collections.Generic;
using System.Runtime.Serialization;
using ClassifierEditor.ConfigSupport;

// Solution:     SettingsManager
// Project:       SettingsManagerV70
// File:             SiteSettings.cs
// Created:      -- ()

namespace SettingsManager
{

#region info class

	[DataContract(Name = "SiteSettingInfo", Namespace = "")]
	public class SiteSettingInfo70<T> : SiteSettingInfoBase<T>
		where T : new ()
	{
		[DataMember]
		public override string DataClassVersion => "7.0as";
		public override string Description => "site setting file for SettingsManagerV70";
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }

	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "SiteSettingData", Namespace = "")]
	public class SiteSettingData70
	{
		[DataMember(Order = 1)]
		public Dictionary<string, ConfigSeedFileSetting> InstSeedFiles { get; set; }
//			= new Dictionary<string, ConfigSeedFileSetting>()
//			{
//				{"Andy :: No User",  new ConfigSeedFileSetting("Andy", "No User", "Seed Files", "Basic", "")}
//			};


		public string MakeKey(string userName, string id)
		{
			return userName + " :: " + id;
		}
	}

#endregion

}