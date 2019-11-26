#region + Using Directives

#endregion


// projname: AODeliverable.FileSelection
// itemname: SelectFolderMgr
// username: jeffs
// created:  11/3/2019 6:48:08 AM


using Tests2.Settings;

namespace Tests2.FileListManager
{
	public class SelectFolderMgr
	{
		private SelectFolder sFld;

		public SelectFolderMgr()
		{
			sFld = new SelectFolder();
		}

//		private Route baseFolder = new Route("");
		private bool isInit = false;

		public bool IsInitialized => isInit;

//		public Route BaseFolder => baseFolder;

		public bool SelectFolder(Route initFolder)
		{
			if (!initFolder.IsValid) 
				return false;

			bool result = sFld.GetFolder();

			if (result)
			{
				baseFolder = new Route(sf.SelectedFolder);

				SettingsMgr.Instance.SetInitFolder(sf.SelectedFolder);

				isInit = true;
			}

			return result;
		}
//
//		public void SetInitFolder(Route initFolder)
//		{
//			sf.InitFolder = initFolder.FullRoute;
//
//			baseFolder = initFolder;
//
//			isInit = false;
//		}

//
//		public string PriorFolder
//		{
//			get { return sf.PriorFolder; }
//			set
//			{
//				if (string.IsNullOrWhiteSpace(value)) return;
//
//				sf.PriorFolder = value;
//			}
//		}



	}
}
