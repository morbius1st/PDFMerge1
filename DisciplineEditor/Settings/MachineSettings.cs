using System.Runtime.Serialization;

// Mach settings (per suite)
//	- applies to a specific machine / all users on that machine
//	- holds information needed by or shared between all users on the machine
//	- located in the common app data folder (currently c:\program data)		


// ReSharper disable once CheckNamespace

namespace SettingsManager
{
	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class MachSettingDataFile: IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "mach 7.4m";

		[IgnoreDataMember]
		public string DataFileDescription =>"mach setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "mach / any notes go here";

		[DataMember(Order = 1)]
		public int MachSettingsValue { get; set; } = 7;
	}



//
// #region info class
//
// 	[DataContract(Name = "MachSettings", Namespace = "")]
// 	public class MachSettingInfo<T> : MachSettingInfoBase<T>
// 		where T : new()
// 	{
// 		public MachSettingInfo()
// 		{
// 			DataClassVersion = "7.0m";
// 			Description = "machine setting file for SettingsManagerV70";
// 		}
//
// 		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
// 	}
//
// #endregion
//
// 	#region user mach class
//
// 	// this is the actual data set saved to the user's configuration file
// 	// this is unique for each program
// 	[DataContract(Namespace = "")]
// 	public class MachSettingData //: IDataFile
// 	{
//
// 		[DataMember(Order = 1)]
// 		public int MachSettingsValue { get; set; } = 7;
// 	}
//
// 	#endregion
}