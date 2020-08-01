using System.Collections.Generic;
using System.Runtime.Serialization;
using ClassifierEditor.ConfigSupport;
using UtilityLibrary;

// projname: SettingsManagerV40
// itemname: AppSettingInfo60
// username: jeffs
// Created:      -- ()

namespace SettingsManager
{

#region info class

	[DataContract(Name = "AppSettingInfoInfo")]
	public class AppSettingInfo<T> : AppSettingInfoBase<T>
		where T : new ()
	{
		[DataMember]
		public override string DataClassVersion => "7.0a";
		public override string Description => "app setting file for ClassifierEditor";
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "AppSettingData")]
	public class AppSettingData
	{
		[DataMember]
		public string Name { get; set; } = "Andy";

	}

#endregion

}