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
		public Route Folder { get; set; }

		private bool ByPass = true;

		private SelectFolder sf = new SelectFolder();

		private FolderPath fpPath;

		private UserSettingData30 usd;

		private int index;

		public FolderManager(int index, HeaderControl hcPath)
		{
			this.index = index;

			fpPath = hcPath.FpPath;

			hcPath.FolderPathType = 7;

			fpPath.PathChange += OnPathChangeFolderPath;
			fpPath.SelectFolder += onSelectFolderFolderPath;


			usd = UserSettings.Data;

			GetFolder();
		}

		public void GetFolder()
		{
			SetFolder(usd.PriorFolders[index]);
		}

		private void SetFolder(string path)
		{
			Folder = new Route(path);

			fpPath.Path = Folder;
		}

		private bool SelectFolder()
		{

			if (index == 0)
			{
				usd.PriorFolders[index] = @"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Base";
			}
			else
			{
				usd.PriorFolders[index] = @"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Test";
			}

			GetFolder();

			return true;

			Folder = sf.GetFolder(Folder);
			if (!Folder.IsValid) return false;

			usd.PriorFolders[index] = Folder.FullPath;
			UserSettings.Admin.Save();

			fpPath.Path = Folder;

			return true;
		}

		public void OnPathChangeFolderPath(object sender, PathChangeArgs e)
		{
			Debug.WriteLine("folderManager, path changed");
			Debug.WriteLine("folderManager| index     | " + e.Index);
			Debug.WriteLine("folderManager| sel folder| " + e.SelectedFolder);
			Debug.WriteLine("folderManager| sel path  | " + e.SelectedPath.FullPath);

			fpPath.Path = e.SelectedPath;
		}

		private void onSelectFolderFolderPath(object sender)
		{
			Debug.WriteLine("folderManager, Select Folder");

			SelectFolder();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}


//		public void Register(FolderPath fpPath)
//		{
//			Fp1 = fpPath;
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
//			string[] p = FolderManager.Folder.FullPathNames;
//
//			if (i == -1)
//			{
//				Fp1.AddPath(FolderManager.Folder.FullPathNames);
//			}
//		}

}