#region + Using Directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AODeliverable.Settings;

#endregion


// projname: AODeliverable.FileSelection
// itemname: SelectFolderMgr
// username: jeffs
// created:  11/3/2019 6:48:08 AM


namespace AODeliverable.FileSelection
{
	public class SelectFolderMgr 
	{
		private static SelectFolderMgr instance = null;

		private SelectFolder sf = null;

		private Route baseFolder = null;

		public static SelectFolderMgr Instance
		{
			get
			{
				if (instance == null) instance = new SelectFolderMgr();

				return instance;
			}
		}

		private SelectFolderMgr()
		{
			sf = SelectFolder.Instance;
		}

		public bool ChoseFolder()
		{
			sf.InitialFolder = UserSettings.Data.PriorFolder;

			bool result = sf.GetFolder();

			if (result)
			{
				baseFolder = new Route(sf.SelectedFolder);

				UserSettings.Data.PriorFolder =
					sf.SelectedFolder;

				UserSettings.Admin.Save();
			}

			return result;
		}

		public bool Initialized => !string.IsNullOrWhiteSpace(baseFolder.path);

		public int FolderCount => baseFolder.FolderCount;

		public string BaseFolder => baseFolder.path;

		public string LeftFolder(int count)
		{
			return baseFolder.LeftFolders(count);
		}


		public string InitialFolder
		{
			get { return sf.InitialFolder; }
			set
			{
				if (string.IsNullOrWhiteSpace(value)) return;

				sf.InitialFolder = value;
			}
		}

	}
}
