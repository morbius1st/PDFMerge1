#region + Using Directives

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Sylvester.FileSupport;
using Sylvester.Settings;

#endregion


// projname: Sylvester.SelectFolder
// itemname: FolderManager
// username: jeffs
// created:  1/4/2020 9:30:29 PM



// general folder manager - works with either collection
namespace Sylvester.FolderSupport
{
	public class FolderManager : INotifyPropertyChanged
	{
		private bool ByPass = true;

		private SelectFolder sf = new SelectFolder();

//		private FolderPath fpPath;

		private Route folderPath;

		private int index;

		private HeaderControl hcPath;

		private FavoritesManager favMgr;

		public FolderManager(int index, HeaderControl hcPath)
		{
			this.index = index;

			this.hcPath = hcPath;

			hcPath.SetPathChangeEventHandler(onFolderPathPathChangeEvent);
			hcPath.SetSelectFolderEventHandler(onFolderPathSelectFolderEvent);
			hcPath.SetFavoritesEventHandler(onFolderPathFavoriteEvent);

			favMgr = new FavoritesManager(index);

			getPriorFolder();

			configHeader();
		}

		public Route Folder { get; set; }

		public bool HasPriorFolder => Folder.IsValid;

		private void configHeader()
		{
			int folderPathType = 2;

			if (HasPriorFolder)
			{
				folderPathType = 4;
			}

			folderPathType += favMgr.HasFavorites ? 1 : 0;

			hcPath.FolderPathType = folderPathType;
		}


		public Route FolderPath
		{
			get => folderPath;
			set
			{
				folderPath = value;
				OnPropertyChange();
			}
		}

		private void tempGetPriorFolder()
		{
			if (index == 0)
			{
				UserSettings.Data.PriorFolders[index] = @"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Base";
			}
			else
			{
				UserSettings.Data.PriorFolders[index] = @"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Test";
			}
		}


		private void getPriorFolder()
		{
			tempGetPriorFolder();

			Folder = new Route(UserSettings.Data.PriorFolders[index]);

			hcPath.Path = Folder;
		}

		private void SelectFolder()
		{

//			getPriorFolder();
//
//			return true;

			Folder = sf.GetFolder(Folder);
			if (!Folder.IsValid) return;

//			UserSettings.Data.PriorFolders[index] = Folder.FullPath;
			UserSettings.Data.PriorFolders[index] = Folder.FullPath;
			UserSettings.Admin.Write();

			hcPath.Path = Folder;

			return;
		}

		private void onFolderPathPathChangeEvent(object sender, PathChangeArgs e)
		{
			Debug.WriteLine("folderManager, path changed");
			Debug.WriteLine("folderManager| index     | " + e.Index);
			Debug.WriteLine("folderManager| sel folder| " + e.SelectedFolder);
			Debug.WriteLine("folderManager| sel path  | " + e.SelectedPath.FullPath);

			hcPath.Path = e.SelectedPath;
			Folder = hcPath.Path;
		}

		private void onFolderPathSelectFolderEvent(object sender)
		{
			Debug.WriteLine("folderManager, Select Folder");

			SelectFolder();
		}
		
		private void onFolderPathFavoriteEvent(object sender)
		{
//			Debug.WriteLine("folderManager, Favorites");

			SelectFolder();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}

}