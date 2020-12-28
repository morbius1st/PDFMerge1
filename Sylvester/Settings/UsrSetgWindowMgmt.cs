#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sylvester.Windows;
using UtilityLibrary;

#endregion


// projname: Sylvester.Settings
// itemname: UsrSetgWindowMgmt
// username: jeffs
// created:  2/22/2020 11:08:26 AM


namespace SettingsManager
{
	public partial class UserSettingData
	{
		private void initializeWindowLayout()
		{
			for (int i = 0; i < (int) WindowId.COUNT; i++)
			{
				SavedWinLocationInfo[i] = new WindowLayout();
			}
		}

		[DataMember]
		public WindowLayout[] SavedWinLocationInfo = new WindowLayout[(int) WindowId.COUNT];
	}

	public class WindowLayout
	{
		[IgnoreDataMember]
		public double Min_Width;

		[IgnoreDataMember]
		public double Min_Height;


		public int Height { get; set; } = -1;
		public int Width { get; set; } = -1;
		public int TopEdge { get; set; } = -1;
		public int LeftEdge { get; set; } = -1;
	}


}
