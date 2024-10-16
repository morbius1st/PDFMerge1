using System.Runtime.Serialization;

// projname: SettingsManagerV40
// itemname: UserSettingInfoInfo60
// username: jeffs
// created:  12/23/2018 1:14:35 PM

namespace SettingsManager
{
// #region info class
//
// 	[DataContract(Name = "UserSetting", Namespace = "")]
// 	public class UserSettingInfo<T> : UserSettingInfoBase<T>
// 		where T : new ()
// 	{
// 		public UserSettingInfo()
// 		{
// 			DataClassVersion = "7.0u";
// 			Description = "user setting file for ClassifierEditor";
// 		}
//
//
// 		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
// 	}
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
		public string LastClassificationFileId { get; set; } = "PdfSample 1";

	}

#endregion
}