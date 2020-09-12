using System.Runtime.Serialization;

// Suite settings (per user)
//	- applies to a specific app suite (multiple programs)
//	- holds information needed by all programs in the suite, but not all users of the suite
//	- provides the pointer to the site settings file (why here: allows each user to be associated with different site files)
//	- located in the user's app data folder

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
	#region info class

	[DataContract(Name = "SuiteSettings", Namespace = "")]
#pragma warning disable CS0436 // The type 'SuiteSettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SuiteSettingInfoBase<TData>' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
	public class SuiteSettingInfo<T> : SuiteSettingInfoBase<T>
#pragma warning restore CS0436 // The type 'SuiteSettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SuiteSettingInfoBase<TData>' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
		where T : new()
	{
		public SuiteSettingInfo()
		{
			// these are specific to this data file
			DataClassVersion = "suite 7.2su";
			Description = "suite setting file for SettingsManager v7.2";
			Notes = "any notes go here";
		}

#pragma warning disable CS0436 // The type 'SettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SettingInfoBase<TData>' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
#pragma warning restore CS0436 // The type 'SettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SettingInfoBase<TData>' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
	}

	#endregion

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SuiteSettingData
	{
		[DataMember(Order = 1)]
		public int SuiteSettingsValue { get; set; } = 7;

		[DataMember(Order = 2)]
		public string SiteRootPath { get; set; }
			= @"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerV72\SiteSettings";
	}

	#endregion
}