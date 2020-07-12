#region using

using System;
using System.Management;
using System.Diagnostics;
using AndyShared.ConfigMgrShared;
using Microsoft.WindowsAPICodePack.Shell;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  7/11/2020 5:32:32 PM

namespace AndyConfig.ConfigMgr
{
	public class LocalUsers
	{
	#region private fields

		private const string USER_SETTING_SUBFOLDER = "";

		private LocalUserList userList = new LocalUserList();

		private string userProfilesPath;
		private string currentUserProfilePath;
		private string appDataSubFolders;


	#endregion

	#region ctor

		public LocalUsers()
		{
			GetUserList();
		}

	#endregion

	#region public properties

		public string UserProfilesPath => userProfilesPath;

		public string CurrentUserProfilePath => currentUserProfilePath;

	#endregion

	#region private properties

		private string AppDataSubFolders => appDataSubFolders;

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void GetUserList()
		{
			userProfilesPath = KnownFolders.UserProfiles.Path;
			currentUserProfilePath = KnownFolders.Profile.Path;
			appDataSubFolders = KnownFolders.RoamingAppData.Path.Substring(CurrentUserProfilePath.Length);

			// Debug.WriteLine("common desktop| "
			// 	+ Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
			// Debug.WriteLine("User Profiles Path        | " + UserProfilesPath);
			// Debug.WriteLine("Current User Profile Path | " + CurrentUserProfilePath);
			// Debug.WriteLine("AppData sub-folder path   | " + AppDataSubFolders);


			SelectQuery query = new SelectQuery("Win32_UserAccount");
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
			foreach (ManagementObject envVar in searcher.Get())
			{
				bool local = (bool) envVar["LocalAccount"];
				string status = (string) envVar["Status"];
				string name = (string) envVar["Name"];

				if (!local || !status.Equals("OK") || name.Equals("Administrator")) continue;

				// Debug.WriteLine(
				// 	"Username | {0} | is local | {1} | status | {2} | type | {3} | caption | {4} | domain | {5}",
				// 	envVar["Name"],
				// 	((bool) envVar["LocalAccount"]),
				// 	envVar["Status"],
				// 	envVar["SIDType"],
				// 	envVar["Caption"],
				// 	envVar["Domain"]
				// 	);

				string path = UserProfilesPath + FilePathUtil.PATH_SEPARATOR + name + AppDataSubFolders + ConfigFileSupport.USER_STORAGE_FOLDER;

				Debug.WriteLine("user classification files path   | " + path);
				Debug.WriteLine("create setting path              | " + UserSettings.Path.CreateSettingPath());
				Debug.WriteLine("created sub-folders              | " + CsUtilities.SubFolder(
					UserSettings.Path.SubFolders.Length - 2, "", UserSettings.Path.SubFolders));
				
				userList.AddUser(name, path);
			}
		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is LocalUserList";
		}

	#endregion
	}
}