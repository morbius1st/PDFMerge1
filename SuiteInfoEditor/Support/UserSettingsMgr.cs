
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettingReaderClassifySheets;
using SettingsManager;
using SettingReaderClassifierEditor;
using SuiteInfoEditor.Windows;
using UtilityLibrary;


// user name: jeffs
// created:   6/7/2025 8:16:48 AM

namespace SuiteInfoEditor.Support
{
	public class UserSettingsMgr
	{
		private ClassifierEditorSettings classifierEditorSetg;
		private ClassifySheetsSettings classifiySheetsSetg;

		private CsFlowDocManager w = CsFlowDocManager.Instance;

		public UserSettingsMgr()
		{
			this.classifierEditorSetg = new ClassifierEditorSettings();
			this.classifiySheetsSetg = new ClassifySheetsSettings();
		}

		public void ShowUserSettings()
		{
			w.AddDescTextLineTb("<repeat text='-' quantity='85'/>");
			w.AddTextLineTb("User Settings for each App");
			
			w.StartTb("<indent spaces='4'/>");

			ShowClassifierEditorSettings();

			ShowClassifySheetsSettings();

			w.AddDescTextTb("<indent/>");

			w.AddLineBreaksTb(1);
			w.AddDescTextLineTb("<repeat text='-' quantity='85'/>");
		}

		public void ShowClassifierEditorSettings()
		{
			GeneralSettingsMgr.ShowSettingFilePreface("ClassifierEditor User Settings",classifierEditorSetg.Path,classifierEditorSetg.PathExists);

			GeneralSettingsMgr.ShowSetting("LastClassificationFileId", classifierEditorSetg.LastClassificationFileId);
			GeneralSettingsMgr.ShowSetting("RememberNodeExpandState", classifierEditorSetg.RememberNodeExpandState.ToString());

			w.AddLineBreaksTb(1);

			w.AddDescTextLineTb("<lawngreen>ClassifierEditor User (Data & Info) Settings</lawngreen>");
			w.AddLineBreaksTb(1);

			GeneralSettingsMgr.ShowSetting("Info / Header / SavedBy", classifierEditorSetg.Info_Header_SavedBy);
			GeneralSettingsMgr.ShowSetting("Info / Header / SavedDateTime", classifierEditorSetg.Info_Header_SavedDateTime);
			GeneralSettingsMgr.ShowSetting("Info / DataClassVersion", classifierEditorSetg.Info_DataClassVersion);
			GeneralSettingsMgr.ShowSetting("Info / Description", classifierEditorSetg.Info_Description);
			GeneralSettingsMgr.ShowSetting("Info / Notes", classifierEditorSetg.Info_Notes);

			// template information - replaced when a new file is created
			// GeneralSettingsMgr.ShowSetting("Data / FileDescription", classifierEditorSetg.Data_FileDescription);
			// GeneralSettingsMgr.ShowSetting("Data / FileNotes", classifierEditorSetg.Data_FileNotes);
			// GeneralSettingsMgr.ShowSetting("Data / FileVersion", classifierEditorSetg.Data_FileVersion);
			
			w.AddDescTextTb("<indent/>");

			w.AddLineBreaksTb(1);
		}

		

		public void ShowClassifySheetsSettings()
		{
			GeneralSettingsMgr.ShowSettingFilePreface("ClassifySheets User Settings",classifiySheetsSetg.Path, classifiySheetsSetg.PathExists);
			
			GeneralSettingsMgr.ShowSetting("LastClassificationFileId", classifiySheetsSetg.LastClassificationFileId);
		
			w.AddLineBreaksTb(1);
		
			w.AddDescTextLineTb("<lawngreen>AndyShared User (Data & Info) Settings</lawngreen>");
			w.AddLineBreaksTb(1);
		
			GeneralSettingsMgr.ShowSetting("Info / Header / SavedBy", classifiySheetsSetg.Info_Header_SavedBy);
			GeneralSettingsMgr.ShowSetting("Info / Header / SavedDateTime", classifiySheetsSetg.Info_Header_SavedDateTime);
			GeneralSettingsMgr.ShowSetting("Info / DataClassVersion", classifiySheetsSetg.Info_DataClassVersion);
			GeneralSettingsMgr.ShowSetting("Info / Description", classifiySheetsSetg.Info_Description);
			GeneralSettingsMgr.ShowSetting("Info / Notes", classifiySheetsSetg.Info_Notes);
			
		
			w.AddDescTextTb("<indent/>");
		
			w.AddLineBreaksTb(1);
		}







		public override string ToString()
		{
			return $"this is {nameof(UserSettingsMgr)}";
		}


		
		// andyshared is a shared project - it does not have a user setting file
		// public void ShowAndySharedSettings()
		// {
		// 	GeneralSettingsMgr.ShowSettingFilePreface("AndyShared User Settings",andySharedSetg.Path, andySharedSetg.PathExists);
		//
		// 	// w.AddLineBreaksTb(1);
		// 	//
		// 	// w.StartTb("<indent spaces='4'/>");
		// 	//
		// 	// w.AddDescTextLineTb("<repeat text='-' quantity='85'/>");
		// 	// w.AddDescTextLineTb("<lawngreen>AndyShared User Settings</lawngreen>");
		// 	//
		// 	// w.AddLineBreaksTb(1);
		// 	// w.StartTb("<indent spaces='8'/>");
		// 	//
		// 	// w.AddDescTextLineTb($"<darkgray>Path | </darkgray><dimgray>{andySharedSetg.Path}</dimgray>");
		// 	//
		// 	// w.AddLineBreaksTb(1);
		//
		// 	GeneralSettingsMgr.ShowSetting("LastClassificationFileId", andySharedSetg.LastClassificationFileId);
		//
		// 	w.AddLineBreaksTb(1);
		//
		// 	w.AddDescTextLineTb("<lawngreen>AndyShared User (Data & Info) Settings</lawngreen>");
		// 	w.AddLineBreaksTb(1);
		//
		// 	GeneralSettingsMgr.ShowSetting("Info / Header / SavedBy", andySharedSetg.Info_Header_SavedBy);
		// 	GeneralSettingsMgr.ShowSetting("Info / Header / SavedDateTime", andySharedSetg.Info_Header_SavedDateTime);
		// 	GeneralSettingsMgr.ShowSetting("Info / Description", andySharedSetg.Info_Description);
		// 	GeneralSettingsMgr.ShowSetting("Info / DataClassVersion", andySharedSetg.Info_DataClassVersion);
		//
		// 	GeneralSettingsMgr.ShowSetting("Data / FileDescription", andySharedSetg.Data_FileDescription);
		// 	GeneralSettingsMgr.ShowSetting("Data / FileNotes"      , andySharedSetg.Data_FileNotes);
		// 	GeneralSettingsMgr.ShowSetting("Data / FileVersion"    , andySharedSetg.Data_FileVersion);
		//
		//
		// 	w.AddDescTextTb("<indent/>");
		//
		// 	w.AddLineBreaksTb(1);
		// }

	}
}
