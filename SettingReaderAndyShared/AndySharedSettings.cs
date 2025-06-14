using SettingsManager;

namespace SettingReaderAndyShared
{

	// this is an example file only, andy shared is a shared project so it does not have
	// a user setting file
	public class AndySharedSettings
	{
		public AndySharedSettings()
		{
			UserSettings.Path.settingFilePath = "";

			UserSettings.Admin.Read();
		}

		public string LastClassificationFileId => UserSettings.Data.LastClassificationFileId;

		public string Path => UserSettings.Path.SettingFilePath;
		public bool PathExists => UserSettings.Path.Exists;

		public string Info_DataClassVersion => UserSettings.Info.DataClassVersion;
		public string Info_Description => UserSettings.Info.Description;
		public string Info_Notes => UserSettings.Info.Notes;

		public string Info_Header_SavedBy => UserSettings.Admin.Info.Header.SavedBy;
		public string Info_Header_SavedDateTime => UserSettings.Admin.Info.Header.SaveDateTime;

		// template information when a new file is created
		// public string Data_FileDescription => UserSettings.Data.DataFileDescription;
		// public string Data_FileNotes => UserSettings.Data.DataFileNotes;
		// public string Data_FileVersion => UserSettings.Data.DataFileVersion;


	}
}