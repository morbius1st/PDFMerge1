#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sylvester.FileSupport;
using Sylvester.SavedFolders;
using Sylvester.Settings;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;
using static Sylvester.SavedFolders.SavedFolderType;

#endregion


// projname: Sylvester.Support
// itemname: SavedFoldersDebugSupport
// username: jeffs
// created:  1/24/2020 5:52:02 PM


namespace Sylvester.Support
{
	public class SavedFoldersDebugSupport
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

			Test_03(index);
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
			Dictionary<string, SavedFolder> sf = d.SavedFolders[index.Value()];

			savedWin.Append(nl);
			savedWin.AppendLineFmt("project count", sf.Count.ToString());
			
			foreach (KeyValuePair<string, SavedFolder> kvp in sf)
			{
				listSavedFolderInfo(kvp);
			}
		}

		private void listSavedFolderInfo(KeyValuePair<string, SavedFolder> kvp)
		{
			int i = 0;

			savedWin.AppendLineFmt("dict key", kvp.Key);
			savedWin.AppendLineFmt("saved key", kvp.Value.Key);
			savedWin.AppendLineFmt("name", kvp.Value.Name);
			savedWin.AppendLineFmt("Id/Vol", kvp.Value.Identifier.Volume ?? "null volume");
			savedWin.AppendLineFmt("Id/Root Folder", kvp.Value.Identifier.RootFolder ?? "null root folder");
			savedWin.AppendLineFmt("UseCount", kvp.Value.UseCount.ToString());
			savedWin.Append(nl);
			savedWin.AppendLineFmt("current/rev fldf pair");
			
			foreach (KeyValuePair<string, CurrentRevisionFolderPair> kvpair in kvp.Value.FolderPairs)
			{
				savedWin.Append(nl);
				savedWin.AppendLineFmt("item number", i++.ToString());
				savedWin.AppendLineFmt("dict key", kvpair.Key);
				savedWin.AppendLineFmt("saved key", kvpair.Value.Key);
				savedWin.AppendLineFmt("current-full path", kvpair.Value.Current?.FullPath ?? "null current route");
				savedWin.AppendLineFmt("revision-full path", kvpair.Value.Revision?.FullPath ?? "null revision route");
			}
		}

	}
}
