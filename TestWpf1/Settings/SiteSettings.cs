using System.Runtime.Serialization;

// Site settings:
// 	- applies to all machines & all users
// 	- holds information needed by all machines and, therefore, all users
// 	- maybe placed on the local or remote machine (specify location in app setting file)

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
	#region info class

	[DataContract(Name = "SiteSettings", Namespace = "")]
#pragma warning disable CS0060 // Inconsistent accessibility: base class 'SiteSettingInfoBase<T>' is less accessible than class 'SiteSettingInfo<T>'
#pragma warning disable CS0246 // The type or namespace name 'SiteSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
	public class SiteSettingInfo<T> : SiteSettingInfoBase<T>
#pragma warning restore CS0246 // The type or namespace name 'SiteSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0060 // Inconsistent accessibility: base class 'SiteSettingInfoBase<T>' is less accessible than class 'SiteSettingInfo<T>'
		where T : new()
	{
		public SiteSettingInfo()
		{
			// these are specific to this data file
			DataClassVersion = "site 7.2as";
			Description = "site setting file for SettingsManager v7.2";
			Notes = "any notes goes here";
		}

#pragma warning disable CS0051 // Inconsistent accessibility: parameter type 'SettingInfoBase<T>' is less accessible than method 'SiteSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning disable CS0507 // 'SiteSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)': cannot change access modifiers when overriding 'internal' inherited member 'SettingInfoBase<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning disable CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
#pragma warning restore CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0507 // 'SiteSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)': cannot change access modifiers when overriding 'internal' inherited member 'SettingInfoBase<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning restore CS0051 // Inconsistent accessibility: parameter type 'SettingInfoBase<T>' is less accessible than method 'SiteSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)'
	}

	#endregion

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SiteSettingData
	{
		[DataMember(Order = 1)]
		public int SiteSettingsValue { get; set; } = 7;
	}

	#endregion
}