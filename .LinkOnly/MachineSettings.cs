using System.Runtime.Serialization;
using SettingsManager;

// Solution:     SettingsManager
// Project:       SettingsManagerV70
// File:             MachineSettings.cs
// Created:      -- ()

namespace SettingsManager
{
	// #region info class
	//
	// [DataContract(Name = "MachSettings", Namespace = "")]
	// public class MachSettingInfo<T> : MachSettingInfoBase<T>
	// 	where T : new()
	// {
	// 	public MachSettingInfo()
	// 	{
	// 		DataClassVersion = "7.0m";
	// 		Description = "machine setting file for SettingsManagerV70";
	// 	}
	//
	// 	public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	// }
	//
	// #endregion

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class MachSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "mach 7.4m";

		[IgnoreDataMember]
		public string DataFileDescription =>"mach setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "mach / any notes go here";

		[DataMember(Order = 1)]
		public string LastClassificationFileId { get; set; } = "PdfSample 1";
	}

	#endregion
}