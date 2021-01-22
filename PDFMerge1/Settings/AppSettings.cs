using System.Runtime.Serialization;
using SettingsManager;

// projname: SettingsManagerV40
// itemname: AppSettingInfo60
// username: jeffs
// Created:      -- ()

namespace SettingsManager
{
#region info class

	[DataContract(Name = "AppSettings", Namespace = "")]
	public class AppSettingInfo<T> : AppSettingInfoBase<T>
		where T : new ()
	{
		public AppSettingInfo()
		{
			DataClassVersion = "7.0a";
			Description = "app setting file for ClassifierEditor";
		}

		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class AppSettingData
	{
		[DataMember]
		public string Name { get; set; } = "Andy";
	}

#endregion
}