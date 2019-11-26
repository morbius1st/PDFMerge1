#region + Using Directives

using Felix.Support;

#endregion


// projname: Tests2.Settings
// itemname: SettingsManager
// username: jeffs
// created:  11/16/2019 6:02:58 AM


namespace Felix.Settings
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
