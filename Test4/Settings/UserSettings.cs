using System.Runtime.Serialization;

// User settings (per user) 
//  - user's settings for a specific app
//	- located in the user's app data folder / app name

// ReSharper disable once CheckNamespace

//WPF; SM74,USER_SETTINGS, APP_SETTINGS, SUITE_SETTINGS, MACH_SETTINGS, SITE_SETTINGS

namespace SettingsManager
{

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



		// [DataMember(Order = 1)]
		// public int UserSettingsValue { get; set; } = 7;

		[DataMember(Order = 0)]
		public bool IsRead { get; set; } = false;

		[DataMember(Order = 2)]
		public string LastClassificationFileId { get; set; } //= "Pdf Filter 1";

	}

	#endregion
}