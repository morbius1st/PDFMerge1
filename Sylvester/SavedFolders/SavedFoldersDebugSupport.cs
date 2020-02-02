#region + Using Directives

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sylvester.FileSupport;
using Sylvester.Settings;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

#endregion


// projname: Sylvester.Support
// itemname: SavedFoldersDebugSupport
// username: jeffs
// created:  1/24/2020 5:52:02 PM


namespace Sylvester.SavedFolders
{
	public class SavedFoldersDebugSupport :INotifyPropertyChanged
	{
		private SavedFoldersWin savedWin;

		private SavedFolderManager[] sfMgr = new SavedFolderManager[2];

		public Route CurrentFolder =
			new Route(@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Base");

		public Route TestFolder = new Route(@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Test");

		public static SavedFoldersDebugSupport Instance = new SavedFoldersDebugSupport();

		private SavedFoldersDebugSupport() { }

		public void ConfigSavedFoldersDebugSupport(SavedFolderManager current, SavedFolderManager revision)
		{
			savedWin = SavedFolderManager.SavedWinInstance;

			sfMgr[0] = current;
			sfMgr[1] = revision;
		}

		public void Test_01()
		{
			Route r =  new Route(
				@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Base");

			string[] names = UserSettings.Data.priorPath.FullPathNames;

			foreach (string name in names)
			{
				savedWin.AppendLineFmt("name",  name + nl);

			}
		}

		public void Test_02(SavedFolderType index)
		{
			UserSettingData30 d = UserSettings.Data;

			bool result = sfMgr[0].AddProject(CurrentFolder, TestFolder);

			savedWin.AppendLineFmt("make project", (result ? ";worked" : "failed"));

//			Test_03(index);
		}

		public void Test_03(SavedFolderType index)
		{
			UserSettingData30 d = UserSettings.Data;

			ListInfo("before", index);

			UserSettings.Admin.Reset();

			ListInfo("after reset", index);

			d = UserSettings.Data;

			UserSettings.Admin.Read();

			ListInfo("after", index);

			d = UserSettings.Data;
		}

		public void ListInfo(string title, SavedFolderType index)
		{
			savedWin.AppendLine(nl);
			savedWin.AppendLineFmt("info", title);
			ListUserSettingInfo();
			ListSavedFoldersInfo(index);
		}

		private void ListUserSettingInfo()
		{
			int i = 0;
			UserSettingData30 d =  UserSettings.Data;

			savedWin.AppendLine(nl);
			savedWin.AppendLineFmt("default volume", d.DefaultVolume);
			savedWin.AppendLineFmt("sheet title case", d.SheetTitleCase.Name());
			savedWin.AppendLineFmt("prior path", d.priorPath?.FullPath ?? "null prior path");
			
			foreach (string s in d.PriorFolders)
			{
				savedWin.AppendLineFmt("prior folder #" + i++, s ?? "is null");
			}
		}



		private void ListSavedFoldersInfo(SavedFolderType index)
		{
			UserSettingData30 d =  UserSettings.Data;
			ObservableCollection<SavedFolderProject> sf = d.SavedFolders[index.Value()];

			savedWin.Append(nl);
			savedWin.AppendLineFmt("project count", sf.Count.ToString());
			
			foreach (SavedFolderProject kvp in sf)
			{
				listSavedFolderInfo(kvp);
			}
		}

		private void listSavedFolderInfo(SavedFolderProject kvp)
		{
			int i = 0;

			savedWin.AppendLineFmt("saved key", kvp.Key);
			savedWin.AppendLineFmt("name", kvp.Name);
			savedWin.AppendLineFmt("Id/Vol", kvp.Identifier.Volume ?? "null volume");
			savedWin.AppendLineFmt("Id/Root Folder", kvp.Identifier.RootFolder ?? "null root folder");
			savedWin.AppendLineFmt("UseCount", kvp.UseCount.ToString());
			savedWin.Append(nl);
			savedWin.AppendLineFmt("current/rev fldf pair");

			foreach (SavedFolderPair kvpair in kvp.SavedFolderPairs)
			{
				savedWin.Append(nl);
				savedWin.AppendLineFmt("item number", i++.ToString());
				savedWin.AppendLineFmt("saved key", kvpair.Key);
				savedWin.AppendLineFmt("current-full path", kvpair.Current?.FullPath ?? "null current route");
				savedWin.AppendLineFmt("revision-full path", kvpair.Revision?.FullPath ?? "null revision route");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}
}
