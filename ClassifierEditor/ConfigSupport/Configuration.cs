#region using directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClassifierEditor.DataRepo;
using SettingsManager;

#endregion

// username: jeffs
// created:  6/11/2020 6:14:19 AM

namespace ClassifierEditor.ConfigSupport
{
	// configures the location of the information (folders and file names)
	// and handles creating a default setup

	public class Configuration : INotifyPropertyChanged
	{
	#region private fields

	#endregion

	#region ctor

		public Configuration()
		{
			SuiteSettings.Admin.Read();

			SiteSettings.Path.RootPath = SuiteSettings.Data.SiteRootPath;
//			SiteSettings.Path.SubFolders = new [] {"CyberStudio", "Andy"};
//			SiteSettings.Path.FileName = "Andy.site.settings";

			SiteSettings.Admin.Read();

			AppSettings.Admin.Read();

			UserSettings.Admin.Read();

			AddUserOrgFile("Test File 1", "jeffs");

		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void AddUserOrgFile(string name, string user)
		{
			string file = name + " :: " + user;

			ConfigFileSetting cfs =
				new ConfigFileSetting(
					name,
					user,
					SuiteSettings.Admin.Path.RootPath,
					file);

			SuiteSettings.Data.UsersOrganizationFiles.Add(cfs);
			SuiteSettings.Admin.Write();
		}

		public void ConfigureCategories( SheetCategoryDataManager dm)
		{
			dm.Configure(UserSettings.Data.CatConfigFolder,
				UserSettings.Data.CatConfigFile);
		}

	#endregion

	#region private methods

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is Configuration";
		}

	#endregion
	}
}