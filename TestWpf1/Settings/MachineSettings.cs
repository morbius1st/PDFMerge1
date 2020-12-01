using System.Runtime.Serialization;

// Mach settings (per suite)
//	- applies to a specific machine / all users on that machine
//	- holds information needed by or shared between all users on the machine
//	- located in the common app data folder (currently c:\program data)		


// ReSharper disable once CheckNamespace

namespace SettingsManager
{
	#region info class

	[DataContract(Name = "MachSettings", Namespace = "")]
#pragma warning disable CS0060 // Inconsistent accessibility: base class 'MachSettingInfoBase<T>' is less accessible than class 'MachSettingInfo<T>'
#pragma warning disable CS0246 // The type or namespace name 'MachSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
	public class MachSettingInfo<T> : MachSettingInfoBase<T>
#pragma warning restore CS0246 // The type or namespace name 'MachSettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0060 // Inconsistent accessibility: base class 'MachSettingInfoBase<T>' is less accessible than class 'MachSettingInfo<T>'
		where T : new()
	{
		public MachSettingInfo()
		{
			// these are specific to this data file
			DataClassVersion = "mach 7.2m";
			Description = "machine setting file for SettingsManager v7.2";
			Notes = "any notes go here";
		}

#pragma warning disable CS0051 // Inconsistent accessibility: parameter type 'SettingInfoBase<T>' is less accessible than method 'MachSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning disable CS0507 // 'MachSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)': cannot change access modifiers when overriding 'internal' inherited member 'SettingInfoBase<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning disable CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
#pragma warning restore CS0246 // The type or namespace name 'SettingInfoBase<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0507 // 'MachSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)': cannot change access modifiers when overriding 'internal' inherited member 'SettingInfoBase<T>.UpgradeFromPrior(SettingInfoBase<T>)'
#pragma warning restore CS0051 // Inconsistent accessibility: parameter type 'SettingInfoBase<T>' is less accessible than method 'MachSettingInfo<T>.UpgradeFromPrior(SettingInfoBase<T>)'
	}

	#endregion

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class MachSettingData
	{
		[DataMember(Order = 1)]
		public int MachSettingsValue { get; set; } = 7;
	}

	#endregion
}