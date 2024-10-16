using System.Runtime.Serialization;

// User settings (per user) 
//  - user's settings for a specific app
//	- located in the user's app data folder / app name

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
	// #region info class
	//
	// [DataContract(Name = "UserSettings", Namespace = "")]
	// public class UserSettingInfo<T> : UserSettingInfoBase<T>
	// 	where T : new()
	// {
	// 	public UserSettingInfo()
	// 	{
	// 		// these are specific to this data file
	// 		DataClassVersion = "user 7.2u";
	// 		Description = "user setting file for SettingsManager v7.2";
	// 		Notes = "any notes go here";
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
	public class UserSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "user 7.4u";

		[IgnoreDataMember]
		public string DataFileDescription => "user setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "user / any notes go here";
		[DataMember(Order = 1)]
		public int UserSettingsValue { get; set; } = 7;

		[DataMember(Order = 1)]
		public string LastClassificationFileId { get; set; } = "PdfSample 1";
	}

	#endregion
}