#region + Using Directives
using System.Runtime.Serialization;
using System.Xaml;
using WpfShared.Windows;

#endregion


// projname: Sylvester.Settings
// itemname: UsrSetgWindowMgmt
// username: jeffs
// created:  2/22/2020 11:08:26 AM


namespace SettingsManager
{
	public partial class UserSettingDataFile
	{
		private void initializeWindowLayout()
		{
			// SavedWinLocationInfo = new WindowLayout[(int) WindowId.COUNT];

			for (int i = 0; i < (int) WindowId.COUNT; i++)
			{
				SavedWinLocationInfo[i] = new WindowLayout((WindowId) i);
			}
		}

		[DataMember]
		public WindowLayout[] SavedWinLocationInfo { get; set; } = new WindowLayout[(int) WindowId.COUNT];

		public WindowLayout GetWindowLayout(WindowId id) => SavedWinLocationInfo[(int) id];

		public void SetWindowLayout(WindowId id, WindowLayout layout)
		{
			SavedWinLocationInfo[(int) id] = layout;

			UserSettings.Admin.Write();
		}
	}

	[DataContract(Namespace = "")]
	public class WindowLayout
	{
		public WindowLayout(WindowId id)
		{
			Id = id;
		}

		[IgnoreDataMember]
		public double Min_Width;

		[IgnoreDataMember]
		public double Min_Height;

		[DataMember(Order = 1)]
		public WindowId Id { get; private set; }

		[DataMember(Order = 2)]
		public int Height { get; set; } = -1;
		[DataMember(Order = 3)]
		public int Width { get; set; } = -1;
		[DataMember(Order = 4)]
		public int TopEdge { get; set; } = -1;
		[DataMember(Order = 5)]
		public int LeftEdge { get; set; } = -1;
	}


}
