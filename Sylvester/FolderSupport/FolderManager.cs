#region + Using Directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Sylvester.FileSupport;
using Sylvester.SavedFolders;
using Sylvester.Settings;
using Sylvester.Support;
using Sylvester.UserControls;
using UtilityLibrary;

using static Sylvester.SavedFolders.SavedFolderType;

#endregion


// projname: Sylvester.SelectFolder
// itemname: FolderManager
// username: jeffs
// created:  1/4/2020 9:30:29 PM


// general folder manager - works with either collection
namespace Sylvester.FolderSupport
{
	public class FolderManager : INotifyPropertyChanged //, IDisposable
	{

		private bool ByPass = true;

		private SelectFolder sf = new SelectFolder();

		public SavedFolderManager[] sfMgr = new SavedFolderManager[COUNT.Value()];

		private Route folderPath;

		private int index;

		private HeaderControl hcPath;

		public FolderManager(int index, HeaderControl hcPath)
		{
			this.index = index;

			this.hcPath = hcPath;

			hcPath.SetPathChangeEventHandler(onFolderPathPathChangeEvent);
			hcPath.SetSelectFolderEventHandler(onFolderPathSelectFolderEvent);
			hcPath.SetFavoritesEventHandler(onFolderPathFavoriteEvent);

			sfMgr[SAVED.Value()] = new SavedFolderManager(SAVED);  //, SetgMgr.Instance.ProjectSavedFolders);

			sfMgr[FAVORITES.Value()] = new SavedFolderManager(FAVORITES);  //, SetgMgr.Instance.FavoritesSavedFolders);

			SavedFoldersDebugSupport.Instance.
				ConfigSavedFoldersDebugSupport(sfMgr[SAVED.Value()], sfMgr[FAVORITES.Value()]);


			getPriorFolder();

			configHeader();
		}

		// finalizer
//		~FolderManager()
//		{
//			Dispose();
//		}

		public Route Folder { get; set; }

		/*
			1 = ★ favorite alone
			2 = select folder alone
			4 = path alone
			type  -1-  -4-
			 2     no   no  -> show select folder only = 2 + 0 + 0
			 3    yes   no  -> show ★ & select folder = 2 + 1 + 0
			 4     no  yes  -> show path = 0 + 0 + 4
			 5    yes  yes  -> show ★ & path = 0 + 1 + 4
		 */


		public bool HasPriorFolder => Folder.IsValid;

		private void configHeader()
		{
			// always show select folder
			int folderPathType = 2;

			if (HasPriorFolder)
			{
				folderPathType += 4;
			}

			folderPathType += sfMgr[0].HasSavedFolders ? 1 : 0;

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
				SetgMgr.SetPriorFolder(index,
					@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Base");
			}
			else
			{
				SetgMgr.SetPriorFolder(index,
					@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Test");
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
			Folder = sf.GetFolder(Folder);
			if (!Folder.IsValid) return;

			SetgMgr.SetPriorFolder(index, Folder);

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

		private void onFolderPathSelectFolderEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Select Folder");

//			SelectFolder();
		}

		private void onFolderPathFavoriteEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("folderManager, Favorites");

//			SelectFolder();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}
}