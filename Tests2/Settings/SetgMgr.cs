#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests2.FileListManager;

#endregion


// projname: Tests2.Settings
// itemname: SettingsManager
// username: jeffs
// created:  11/16/2019 6:02:58 AM


namespace Tests2.Settings
{
	public sealed class SetgMgr
	{
		private static readonly SetgMgr instance = new SetgMgr();

		static SetgMgr() { }

		public static SetgMgr Instance => instance;

		public void SetInitFolder(string folder)
		{
			UserSettings.Data.PriorFolder = folder;

			UserSettings.Admin.Save();
		}

		public void SetInitFolder(Route folder) => 
			SetInitFolder(folder.FullPath);

		public Route GetInitFolder() => 
			new Route(UserSettings.Data.PriorFolder);

	}
}
