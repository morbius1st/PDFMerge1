using System.Runtime.Serialization;

// App settings (per user)
//	- applies to a specific app in the suite
//	- holds information specific to the app
//	- located in the user's app data folder / app name / AppSettings

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
	#region info class

	[DataContract(Name = "AppSettings", Namespace = "")]
#pragma warning disable CS0060 // Inconsistent accessibility: base class 'AppSettingInfoBase<T>' is less accessible than class 'AppSettingInfo<T>'
#pragma warning disable CS0246 // The type or namespace name 'AppSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
	public class AppSettingInfo<T> : AppSettingInfoBase<T>
#pragma warning restore CS0246 // The type or namespace name 'AppSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0060 // Inconsistent accessibility: base class 'AppSettingInfoBase<T>' is less accessible than class 'AppSettingInfo<T>'
		where T : new()
	{
		public AppSettingInfo()
		{
			// these are specific to this data file
			DataClassVersion = "app 7.2a";
			Description = "app setting file for SettingsManager v7.2";
			Notes = "any notes go here";
		}

#pragma warning disable CS0507 // 'AppSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)': cannot change access modifiers when overriding 'internal' inherited member 'SettingInfoBase<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning disable CS0051 // Inconsistent accessibility: parameter type 'SettingInfoBase<T>' is less accessible than method 'AppSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning disable CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
#pragma warning restore CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0051 // Inconsistent accessibility: parameter type 'SettingInfoBase<T>' is less accessible than method 'AppSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning restore CS0507 // 'AppSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)': cannot change access modifiers when overriding 'internal' inherited member 'SettingInfoBase<T>.UpgradeFromPrior(SettingInfoBase<T>)'
	}

	#endregion

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class AppSettingData
	{
		[DataMember(Order = 1)]
		public int AppSettingsValue { get; set; } = 7;
	}

	#endregion
}