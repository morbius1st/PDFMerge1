using System.Runtime.Serialization;

// projname: SettingsManagerV40
// itemname: UserSettingInfoInfo60
// username: jeffs
// created:  12/23/2018 1:14:35 PM

namespace SettingsManager
{
#region info class

	[DataContract(Name = "UserSetting", Namespace = "")]
	internal class UserSettingInfo<T> : UserSettingInfoBase<T>
		where T : new ()
	{
		public UserSettingInfo()
		{
			DataClassVersion = "7.0u";
			Description = "user setting file for ClassifierEditor";
		}


		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	internal class UserSettingData
	{
		[DataMember(Order = 1)]
		public string LastClassificationFileId { get; set; } = "PdfSample 1";
		
		[DataMember(Order = 2)]
		public bool RememberNodeExpandState { get; set; } = false;



		// public Dictionary<string, string>
		// 	UserClassfigCfg { get; set; } = new Dictionary<string, string>()
		// {
		// 	{"jeffs", "PdfSample 2"}
		// };

		// // the name of the organization configuration file
		// [DataMember(Order = 1)]
		// public string OrgConfigFileName { get; private set; } =
		// 	"Test File 1 :: jeffs";

		// // the name of the organization configuration file
		// [DataMember(Order = 1)]
		// public string OrgConfigFileName { get; private set; } =
		// 	"Test File 1 :: jeffs";
		//
		// // the folder where the FileName config files are stored
		// [DataMember(Order = 2)]
		// public string CatConfigFolder { get; private set; } =
		// 	@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\.sample";
		//
		// // the name of the FileName config file
		// [DataMember(Order = 3)]
		// public string CatConfigFile { get; private set; } =
		// 	"SheetCategories.xml";
		//
		// // the folder with the sample PDF files to categorize
		// [DataMember(Order = 4)]
		// public string CatConfigSampleFolder { get; private set; } =
		// 	@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\.sample";
		//
		// // the folder with the sample PDF files to categorize
		// [DataMember(Order = 5)]
		// public string CatConfigSampleFile { get; private set; } =
		// 	@"PdfSampleFileList.dat";
	}

#endregion
}