using SettingsManager;

namespace SettingReaderClassifierEditor
{
	public class ClassifierEditorSettings
	{
		public ClassifierEditorSettings()
		{
			UserSettings.Path.settingFilePath = @"C:\Users\jeffs\AppData\Roaming\CyberStudio\Andy\ClassifierEditor\user.setting.xml";
			UserSettings.Admin.Read();
		}

		// actual data saved in the file
		public string LastClassificationFileId => UserSettings.Data.LastClassificationFileId;
		public bool RememberNodeExpandState => UserSettings.Data.RememberNodeExpandState;

		// general / metadata
		public string Path => UserSettings.Path.SettingFilePath;
		public bool PathExists => UserSettings.Path.Exists;

		public string Info_DataClassVersion => UserSettings.Info.DataClassVersion;
		public string Info_Description => UserSettings.Info.Description;
		public string Info_Notes => UserSettings.Info.Notes;

		public string Info_Header_SavedBy => UserSettings.Info.Header.SavedBy;
		public string Info_Header_SavedDateTime => UserSettings.Info.Header.SaveDateTime;

		// template information for when a new file is created
		// public string Data_FileDescription => UserSettings.Data.DataFileDescription;
		// public string Data_FileNotes => UserSettings.Data.DataFileNotes;
		// public string Data_FileVersion => UserSettings.Data.DataFileVersion;

	}
}

namespace SettingsManager
{
	public partial class UserSettingDataFile : IDataFile
	{
		private void initializeWindowLayout(){}
	}
}
