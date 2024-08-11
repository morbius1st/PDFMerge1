using System.Collections.Generic;
using System.Runtime.Serialization;
using Tests2.OutlineManager;

// User settings (per user) 
//  - user's settings for a specific app
//	- located in the user's app data folder / app name


// ReSharper disable once CheckNamespace


namespace SettingsManager
{
#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class UserSettingData //: HeaderData
	{
		[IgnoreDataMember]
		public string DataFileVersion => "user 7.4u";

		[IgnoreDataMember]
		public string DataFileDescription => "user setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "user / any notes go here";

		[DataMember]
		public string PriorFolder = @"C:\2099-999 Sample Project\Publish\Bulletins";

		[DataMember]
		public bool AllowOverwriteOutputFile = true;

		[DataMember]
		public List<OutlineItem> OutlineItems = new List<OutlineItem>();
	}

	[DataContract(Name = "UserSetting", Namespace = "")]
	public class UserSettingInfo<T> : UserSettingInfoBase<T>
		where T : new ()
	{
		[IgnoreDataMember]
		public string DataFileVersion => "user 7.4u";

		[IgnoreDataMember]
		public string DataFileDescription => "user setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "user / any notes go here";

		[DataMember]
		public string PriorFolder = @"C:\2099-999 Sample Project\Publish\Bulletins";

		[DataMember]
		public bool AllowOverwriteOutputFile = true;

		[DataMember]
		public List<OutlineItem> OutlineItems = new List<OutlineItem>();

		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion
}


// , APP_SETTINGS, SUITE_SETTINGS, MACH_SETTINGS, SITE_SETTINGS