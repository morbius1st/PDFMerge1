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


		internal override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
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

		[DataMember(Order = 100)]
		public WindowPosition MainWinPos{ get; set; } = new WindowPosition(100,100);

		[DataMember(Order = 101)]
		public WindowPosition WinClassifyPos{ get; set; } = new WindowPosition(100,100);
	}

#endregion

	[DataContract(Namespace = "")]
	public class WindowPosition
	{
		[DataMember(Order = 1)]
		public int X { get; set; }

		[DataMember(Order = 2)]
		public int Y { get; set; }
		
		public WindowPosition(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}