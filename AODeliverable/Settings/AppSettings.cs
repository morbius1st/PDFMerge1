using System.Collections.Generic;
using System.Runtime.Serialization;
using UtilityLibrary;

// projname: SettingsManagerV40
// itemname: AppSettingInfo60
// username: jeffs
// Created:      -- ()

namespace SettingsManager
{
// #region info class
//
// 	[DataContract(Name = "AppSettings", Namespace = "")]
// 	public class AppSettingInfo<T> : AppSettingInfoBase<T>
// 		where T : new ()
// 	{
// 		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
//
// 		public AppSettingInfo()
// 		{
// 			DataClassVersion = "7.0a";
// 			Description = "app setting file for WpfShared";
// 		}
//
// 	}
//
// #endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class AppSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "app 7.4a";

		[IgnoreDataMember]
		public string DataFileDescription => "app setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "app / any notes go here";

		[DataMember]
		public string Name { get; set; } = "Andy";

		[DataMember(Order = 2)]
		public bool AllowPropertyEditing { get; set; } = false;
	}

#endregion
}