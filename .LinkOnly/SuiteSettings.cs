using System.Runtime.Serialization;


// projname: SettingsManagerV40
// itemname: AppSettingInfo70
// username: jeffs
// Created:      -- ()

namespace SettingsManager
{
	#region info class

	[DataContract(Name = "SuiteSettings", Namespace = "")]
	internal class SuiteSettingInfo<T> : SuiteSettingInfoBase<T>
		where T : new()
	{
		public SuiteSettingInfo()
		{
			DataClassVersion = "0.7.su";
			Description = "Suite setting file for Andy";
		}

#pragma warning disable CS0436 // The type 'SettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SettingInfoBase<TData>' in 'WpfShared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
		internal override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
#pragma warning restore CS0436 // The type 'SettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SettingInfoBase<TData>' in 'WpfShared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
	}

	#endregion

	#region suite data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	internal class SuiteSettingData
	{
		[DataMember(Order = 1)]
		public string SiteRootPath { get; set; }
		//			= @"D:\Users\Jeff\OneDrive\Prior Folders\Office Stuff\CAD\Copy Y Drive & Office Standards\AppData" ;




	}

	#endregion
}