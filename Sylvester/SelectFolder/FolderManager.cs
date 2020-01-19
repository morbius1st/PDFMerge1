#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Sylvester.FileSupport;
using Sylvester.Settings;

#endregion


// projname: Sylvester.SelectFolder
// itemname: FolderManager
// username: jeffs
// created:  1/4/2020 9:30:29 PM



// general folder manager - works with either collection
namespace Sylvester.SelectFolder
{
	public class FolderManager : INotifyPropertyChanged
	{
		private bool hasBaseFolder;
		private bool hasTestFolder;

		public Route BaseFolder { get; set; }

		public Route TestFolder { get; set; }

		private bool ByPass = true;

		private SelectFolder sf = new SelectFolder();

		private FolderPath Fp1;

		private UserSettingData30 usd;



		public FolderManager(FolderPath fp1)
		{
//			string[] favnames = new string[UserSettings.Data.Favorites.Count];
//			UserSettings.Data.Favorites.Keys.CopyTo(favnames, 0);

//			AddFav("new fav", new Route(@"C:\2099-999 Sample Project"));

			Fp1 = fp1;

			Fp1.PathChange += OnPathChangeFp1;
			Fp1.SelectFolder += OnSelectFolderFp1;

			usd = UserSettings.Data;

			GetFolders();
		}

		public void GetFolders()
		{
			GetBaseFolder();
			GetTestFolder();
		}

		public void GetBaseFolder()
		{
			BaseFolder = new Route(usd.PriorBaseFolder);

			Fp1.Path = BaseFolder;
		}

		private bool SelectBaseFolder()
		{
			BaseFolder = sf.GetFolder(BaseFolder);
			if (!BaseFolder.IsValid) return false;

			usd.PriorBaseFolder = BaseFolder.FullPath;
			UserSettings.Admin.Save();

			return true;
		}

		public void GetTestFolder()
		{
			if (!usd.HasPriorTestFolder) return;

			TestFolder = new Route(usd.PriorTestFolder);
		}

		private bool SelectTestFolder()
		{
			TestFolder = sf.GetFolder(TestFolder);
			if (!TestFolder.IsValid) return false;

			UserSettings.Data.PriorTestFolder = TestFolder.FullPath;
			UserSettings.Admin.Save();

			return true;
		}

		public void OnPathChangeFp1(object sender, PathChangeArgs e)
		{
			Debug.WriteLine("folderManager, path changed");
			Debug.WriteLine("folderManager| index     | " + e.Index);
			Debug.WriteLine("folderManager| sel folder| " + e.SelectedFolder);
			Debug.WriteLine("folderManager| sel path  | " + e.SelectedPath.FullPath);

			Fp1.Path = e.SelectedPath;
		}

		private void OnSelectFolderFp1(object sender)
		{
			Debug.WriteLine("folderManager, Select Folder");

			usd.PriorBaseFolder = @"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Base";

			GetBaseFolder();

			Fp1.Path = BaseFolder;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}


//		public void Register(FolderPath fp1)
//		{
//			Fp1 = fp1;
//
//			Fp1.SkewedButton1.InnerButton.Click += ButtonBase_OnClick;
//		}
//
//		public void ButtonBase_OnClick(object sender, RoutedEventArgs e)
//		{
//			Button b = e.OriginalSource as Button;
//			SkewedButton sb = b.Tag as SkewedButton;
//
//			int i = (int) sb.Tag;
//			string s = sb.InnerSp.Tag as string;
//
//			string[] p = FolderManager.BaseFolder.FullPathNames;
//
//			if (i == -1)
//			{
//				Fp1.AddPath(FolderManager.BaseFolder.FullPathNames);
//			}
//		}

}