using System.Runtime.Serialization;

// Suite settings (per user)
//	- applies to a specific app suite (multiple programs)
//	- holds information needed by all programs in the suite, but not all users of the suite
//	- provides the pointer to the site settings file (why here: allows each user to be associated with different site files)
//	- located in the user's app data folder

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
	#region info class

	[DataContract(Name = "SuiteSettings", Namespace = "")]
#pragma warning disable CS0060 // Inconsistent accessibility: base class 'SuiteSettingInfoBase<T>' is less accessible than class 'SuiteSettingInfo<T>'
#pragma warning disable CS0246 // The type or namespace name 'SuiteSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
	public class SuiteSettingInfo<T> : SuiteSettingInfoBase<T>
#pragma warning restore CS0246 // The type or namespace name 'SuiteSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0060 // Inconsistent accessibility: base class 'SuiteSettingInfoBase<T>' is less accessible than class 'SuiteSettingInfo<T>'
		where T : new()
	{
		public SuiteSettingInfo()
		{
			// these are specific to this data file
			DataClassVersion = "suite 7.2su";
			Description = "suite setting file for SettingsManager v7.2";
			Notes = "any notes go here";
		}

#pragma warning disable CS0051 // Inconsistent accessibility: parameter type 'SettingInfoBase<T>' is less accessible than method 'SuiteSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning disable CS0507 // 'SuiteSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)': cannot change access modifiers when overriding 'internal' inherited member 'SettingInfoBase<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning disable CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
#pragma warning restore CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0507 // 'SuiteSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)': cannot change access modifiers when overriding 'internal' inherited member 'SettingInfoBase<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning restore CS0051 // Inconsistent accessibility: parameter type 'SettingInfoBase<T>' is less accessible than method 'SuiteSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)'
	}

	#endregion

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SuiteSettingData
	{
		[DataMember(Order = 1)]
		public int SuiteSettingsValue { get; set; } = 7;

		[DataMember(Order = 2)]
		public string SiteRootPath { get; set; }
			= @"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerV72\SiteSettings";
	}

	#endregion
}