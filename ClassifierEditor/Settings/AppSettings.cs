using System.Collections.Generic;
using System.Runtime.Serialization;

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
// 		public AppSettingInfo()
// 		{
// 			DataClassVersion = "7.0a";
// 			Description = "app setting file for ClassifierEditor";
// 		}
//
// 		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
// 	}
//
// #endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public partial class AppSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "app 7.4a";

		[IgnoreDataMember]
		public string DataFileDescription => "app setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "app / any notes go here";
		
		// [DataMember(Order = 1)]
		// public string Name { get; set; } = "Andy";

		[DataMember(Order = 1)]
		public List<string> AdminUsers { get; set; } = new List<string>();

		// [DataMember(Order = 2)]
		// public bool AllowPropertyEditing { get; set; } = false;
	}

#endregion
}