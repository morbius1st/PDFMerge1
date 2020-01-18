#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
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


namespace Sylvester.SelectFolder
{
	public class FolderManager
	{
		public static Route BaseFolder { get; set; }
		public static Route TestFolder { get; set; }

		private bool ByPass = true;

		private SelectFolder sf = new SelectFolder();

		private FolderPath Fp1;

//		public FolderManager()
//		{
//			string[] favnames = new string[UserSettings.Data.Favorites.Count];
//			UserSettings.Data.Favorites.Keys.CopyTo(favnames, 0);
//
//			AddFav("new fav", new Route(@"C:\2099-999 Sample Project"));
//		}
//
//		public void AddFav(string name, Route path)
//		{
//			FavFolder fav = new FavFolder(name, path.FullPath);
//			
//			UserSettings.Data.Favorites.Add(FavKey(0, name), fav);
//
//			UserSettings.Admin.Save();
//		}

		private string FavKey(int useCount, string name)
		{
			return $"{useCount:D5} {name}";
		}

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

		public void Register(FolderPath fp1)
		{
			Fp1 = fp1;

			Fp1.SkewedButton1.InnerButton.Click += ButtonBase_OnClick;
		}

		public void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			Button b = e.OriginalSource as Button;
			SkewedButton sb = b.Tag as SkewedButton;

			int i = (int) sb.Tag;
			string s = sb.InnerSp.Tag as string;

			string[] p = FolderManager.BaseFolder.FullPathNames;

			if (i == -1)
			{
				Fp1.AddPath(FolderManager.BaseFolder.FullPathNames);
			}
		}
	}
}