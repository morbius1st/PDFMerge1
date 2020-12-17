using System.Runtime.Serialization;

// User settings (per user) 
//  - user's settings for a specific app
//	- located in the user's app data folder / app name

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
	#region info class

	[DataContract(Name = "UserSettings", Namespace = "")]
#pragma warning disable CS0060 // Inconsistent accessibility: base class 'UserSettingInfoBase<T>' is less accessible than class 'UserSettingInfo<T>'
#pragma warning disable CS0246 // The type or namespace name 'UserSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
	public class UserSettingInfo<T> : UserSettingInfoBase<T>
#pragma warning restore CS0246 // The type or namespace name 'UserSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0060 // Inconsistent accessibility: base class 'UserSettingInfoBase<T>' is less accessible than class 'UserSettingInfo<T>'
		where T : new()
	{
		public UserSettingInfo()
		{
			// these are specific to this data file
			DataClassVersion = "user 7.2u";
			Description = "user setting file for SettingsManager v7.2";
			Notes = "any notes go here";
		}

#pragma warning disable CS0051 // Inconsistent accessibility: parameter type 'SettingInfoBase<T>' is less accessible than method 'UserSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning disable CS0507 // 'UserSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)': cannot change access modifiers when overriding 'internal' inherited member 'SettingInfoBase<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning disable CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
#pragma warning restore CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0507 // 'UserSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)': cannot change access modifiers when overriding 'internal' inherited member 'SettingInfoBase<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning restore CS0051 // Inconsistent accessibility: parameter type 'SettingInfoBase<T>' is less accessible than method 'UserSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)'
	}

	#endregion

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class UserSettingData
	{
		[DataMember(Order = 1)]
		public int UserSettingsValue { get; set; } = 7;
	}

	#endregion
}