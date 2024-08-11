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
	public class SiteSettingInfo<T> : SiteSettingInfoBase<T>
		where T : new()
	{
		public SiteSettingInfo()
		{
			DataClassVersion = "7.0as";
			Description = "site setting file for SettingsManagerV70";
		}


		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion


	#region site data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SiteSettingData //: IDataFile
	{

		[DataMember(Order = 1)]
		public int SiteSettingsValue { get; set; } = 7;
	}

	#endregion
}