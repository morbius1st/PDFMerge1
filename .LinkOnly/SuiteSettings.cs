using System.Runtime.Serialization;


// projname: SettingsManagerV40
// itemname: AppSettingInfo70
// username: jeffs
// Created:      -- ()

namespace SettingsManager
{
	// #region info class
	//
	// [DataContract(Name = "SuiteSettings", Namespace = "")]
	// public class SuiteSettingInfo<T> : SuiteSettingInfoBase<T>
	// 	where T : new()
	// {
	// 	public SuiteSettingInfo()
	// 	{
	// 		DataClassVersion = "0.7.su";
	// 		Description = "Suite setting file for Andy";
	// 	}
	//
	// 	public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	// }
	//
	// #endregion

	#region suite data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SuiteSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "suite 7.4su";

		[IgnoreDataMember]
		public string DataFileDescription => "suite setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "suite / any notes go here";

		[DataMember(Order = 1)]
		public string SiteRootPath { get; set; }
		//			= @"D:\Users\Jeff\OneDrive\Prior Folders\Office Stuff\CAD\Copy Y Drive & Office Standards\AppData" ;

	}

	#endregion
}