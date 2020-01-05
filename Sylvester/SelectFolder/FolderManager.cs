#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sylvester.FileSupport;
using Sylvester.Settings;

#endregion


// projname: Sylvester.SelectFolder
// itemname: FolderManager
// username: jeffs
// created:  1/4/2020 9:30:29 PM


namespace Sylvester.SelectFolder
{
	public class FolderManager
	{
		public static Route BaseFolder { get; set; }
		public static Route TestFolder { get; set; }

		private bool ByPass = true;

		private SelectFolder sf = new SelectFolder();

		public void GetFolders()
		{
			GetBaseFolder();
			GetTestFolder();
		}

		public bool GetBaseFolder()
		{
			BaseFolder = new Route(UserSettings.Data.PriorBaseFolder);

		#if DEBUG
			if (ByPass) return true;
		#endif

			BaseFolder = sf.GetFolder(BaseFolder);
			if (!BaseFolder.IsValid) return false;

			UserSettings.Data.PriorBaseFolder = BaseFolder.FullPath;
			UserSettings.Admin.Save();

			return true;
		}

		public bool GetTestFolder()
		{
			TestFolder = new Route(UserSettings.Data.PriorTestFolder);

		#if DEBUG
			if (ByPass) return true;
		#endif

			TestFolder = sf.GetFolder(TestFolder);
			if (!TestFolder.IsValid) return false;

			UserSettings.Data.PriorTestFolder = TestFolder.FullPath;
			UserSettings.Admin.Save();

			return true;
		}
	}
}