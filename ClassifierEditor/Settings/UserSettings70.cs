using System.Runtime.Serialization;

// projname: SettingsManagerV40
// itemname: UserSettingInfoInfo60
// username: jeffs
// created:  12/23/2018 1:14:35 PM

namespace SettingsManager
{

#region info class

	[DataContract(Name = "UserSettingInfoInfo")]
	public class UserSettingInfo70<T> : UserSettingInfoBase<T>
		where T : new ()
	{
		[DataMember]
		public override string DataClassVersion => "7.0u";
		public override string Description => "user setting file for ClassifierEditor";
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }

	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "UserSettingData")]
	public class UserSettingData70
	{
		[DataMember(Order = 1)]
		public int UserSettingsValue { get; set; } = 6;

		[DataMember(Order = 2)]
		public string FileNameCategoryFolder { get; private set; } =
			@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\.sample";

		[DataMember(Order = 3)]
		public string FileNameCategoryFile { get; private set; } =
			"SheetCategories.xml";

		[DataMember(Order = 4)]
		public string FileNameTestFolder { get; private set; } =
			@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\.sample";
	}

#endregion


}