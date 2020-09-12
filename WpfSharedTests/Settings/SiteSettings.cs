using System.Runtime.Serialization;

// Site settings:
// 	- applies to all machines & all users
// 	- holds information needed by all machines and, therefore, all users
// 	- maybe placed on the local or remote machine (specify location in app setting file)

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
	#region info class

	[DataContract(Name = "SiteSettings", Namespace = "")]
#pragma warning disable CS0436 // The type 'SiteSettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SiteSettingInfoBase<TData>' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
	public class SiteSettingInfo<T> : SiteSettingInfoBase<T>
#pragma warning restore CS0436 // The type 'SiteSettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SiteSettingInfoBase<TData>' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
		where T : new()
	{
		public SiteSettingInfo()
		{
			// these are specific to this data file
			DataClassVersion = "site 7.2as";
			Description = "site setting file for SettingsManager v7.2";
			Notes = "any notes goes here";
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
	public class SiteSettingData
	{
		[DataMember(Order = 1)]
		public int SiteSettingsValue { get; set; } = 7;
	}

	#endregion
}