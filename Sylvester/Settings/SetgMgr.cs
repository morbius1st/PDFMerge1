#region + Using Directives

using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Security;
using Sylvester.FileSupport;

#endregion


// projname: Tests2.Settings
// itemname: SettingsManager
// username: jeffs
// created:  11/16/2019 6:02:58 AM


namespace Sylvester.Settings
{
	public sealed class SetgMgr
	{
		private static readonly SetgMgr instance = new SetgMgr();

		static SetgMgr() { }

		public static SetgMgr Instance => instance;

		public void SetInitFolder(string folder)
		{
			UserSettings.Data.PriorBaseFolder = folder;

			UserSettings.Admin.Save();
		}

		public Route BaseFolder
		{
			get => new Route(UserSettings.Data.PriorBaseFolder);

			set
			{
				UserSettings.Data.PriorBaseFolder = value.FullPath;

				UserSettings.Admin.Save();
			}
		}

		public Route TestFolder
		{
			get => new Route(UserSettings.Data.PriorTestFolder);

			set
			{
				UserSettings.Data.PriorTestFolder = value.FullPath;

				UserSettings.Admin.Save();
			}
		}


	}
}