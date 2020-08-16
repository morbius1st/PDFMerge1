using System.Collections.Generic;
using System.Runtime.Serialization;
using ClassifierEditor.ConfigSupport;
#pragma warning disable CS0246 // The type or namespace name 'UtilityLibrary' could not be found (are you missing a using directive or an assembly reference?)
using UtilityLibrary;
#pragma warning restore CS0246 // The type or namespace name 'UtilityLibrary' could not be found (are you missing a using directive or an assembly reference?)

// projname: SettingsManagerV40
// itemname: AppSettingInfo60
// username: jeffs
// Created:      -- ()

namespace SettingsManager
{

#region info class

	[DataContract(Name = "AppSettingInfoInfo")]
#pragma warning disable CS0246 // The type or namespace name 'AppSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
	public class AppSettingInfo<T> : AppSettingInfoBase<T>
#pragma warning restore CS0246 // The type or namespace name 'AppSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
		where T : new ()
	{


		[DataMember]
#pragma warning disable CS0115 // 'AppSettingInfo<T>.DataClassVersion': no suitable method found to override
		public override string DataClassVersion => "7.0a";
#pragma warning restore CS0115 // 'AppSettingInfo<T>.DataClassVersion': no suitable method found to override
#pragma warning disable CS0115 // 'AppSettingInfo<T>.Description': no suitable method found to override
		public override string Description => "app setting file for ClassifierEditor";
#pragma warning restore CS0115 // 'AppSettingInfo<T>.Description': no suitable method found to override
#pragma warning disable CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
#pragma warning restore CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
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