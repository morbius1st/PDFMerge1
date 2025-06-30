
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndyShared.ConfigSupport;
using SettingsManager;
using SuiteInfoEditor.Windows;
using UtilityLibrary;


// user name: jeffs
// created:   6/7/2025 9:03:38 AM

namespace SuiteInfoEditor.Support
{
	public class GeneralSettingsMgr
	{
		private string[] preFmt = ["<textformat background='40, 40, 40'/>", null];
		private static int showSetgColumn = 34;

		public GeneralSettingsMgr()
		{
			SuiteSettings.Admin.Read();
		}


		private static CsFlowDocManager w = CsFlowDocManager.Instance;

		public static int ShowSetgColumn
		{
			get => showSetgColumn;
			set => showSetgColumn = value;
		}

		
		public static string ShowIntro(string line1, string line2, int len, string[] fmt, string[] l1Fmt, string[] l2Fmt)
		{
			StringBuilder sb = new StringBuilder(fmt[0]);

			sb.Append("\u256d").Append("\u2500".Repeat(len-2)).AppendLine("\u256e");
			sb.Append("\u2502").Append(" ".Repeat(len-2)).AppendLine("\u2502");
			sb.Append("\u2502").Append(l1Fmt[0]).Append(CsStringUtil.JustifyString(line1, CsStringUtil.JustifyHoriz.CENTER, len-2)).Append(l1Fmt[1]).AppendLine("\u2502");
			sb.Append("\u2502").Append(" ".Repeat(len-2)).AppendLine("\u2502");

			if (!line2.IsVoid())
			{
				sb.Append("\u2502").Append(l2Fmt[0]).Append(CsStringUtil.JustifyString(line2, CsStringUtil.JustifyHoriz.CENTER, len-2)).Append(l2Fmt[1]).AppendLine("\u2502");
				sb.Append("\u2502").Append("".Repeat(len-2)).AppendLine("\u2502");
			}

			sb.Append("\u2570").Append("\u2500".Repeat(len-2)).AppendLine("\u256f");

			sb.Append(fmt[1]);

			return sb.ToString();
		}

		public void ShowCommonSettings()
		{
			w.AddDescTextLineTb("<repeat text='-' quantity='85'/>");
			w.AddTextLineTb("General Settings for all Apps");
			
			ShowHeaderSettings();

			ShowSuiteSettings();

			ShowMachSettings();

			ShowSiteSettings();
			
			w.AddLineBreaksTb(1);
			w.AddDescTextLineTb("<repeat text='-' quantity='85'/>");
		}

		public void ShowHeaderSettings()
		{
			w.AddLineBreaksTb(1);
			w.AddDescTextLineTb("<repeat text='-' quantity='85'/>");
			w.AddDescTextLineTb("<lawngreen>SettingsManager.Heading Settings</lawngreen>");
			w.AddLineBreaksTb(1);

			w.StartTb("<indent spaces='4'/>");

			ShowSetting("SuiteName", SettingsManager.Heading.SuiteName);

			// not used for some reason
			ShowSetting("ClassVersionName", $"{SettingsManager.Heading.ClassVersionName} (not used)");
			ShowSetting("SettingsVersionName", $"{SettingsManager.Heading.SettingsVersionName} (not used)");

			w.AddLineBreaksTb(1);
			w.AddDescTextLineTb("<lawngreen>Admin.Info.Header</lawngreen>");
			w.AddLineBreaksTb(1);

			ShowSetting("AssemblyVersion" , SuiteSettings.Info.Header.AssemblyVersion);
			ShowSetting("AssemblyName"    , SuiteSettings.Info.Header.AssemblyName);
			ShowSetting("SettingsVersion" , SuiteSettings.Info.Header.SettingsVersion);

			w.AddDescTextTb("<indent/>");

			w.AddLineBreaksTb(1);
		}

		public void ShowSuiteSettings()
		{
			ShowSettingFilePreface("Suite Settings",SuiteSettings.Path.settingFilePath, SuiteSettings.Path.Exists, false, true, 0, 4);

			ShowSetting("SiteRootPath", SuiteSettings.Data.SiteRootPath);

			w.AddLineBreaksTb(1);

			w.AddDescTextLineTb("<lawngreen>Suite Settings (Data & Info) Settings</lawngreen>");
			w.AddLineBreaksTb(1);

			ShowSetting("Info / Header / SavedBy", SuiteSettings.Info.Header.SavedBy);
			ShowSetting("Info / Header / SavedDateTime", SuiteSettings.Info.Header.SaveDateTime);
			ShowSetting("Info / IsSuite?", SuiteSettings.Info.IsSuite.ToString());
			ShowSetting("Info / DataClassVersion", SuiteSettings.Info.DataClassVersion);
			ShowSetting("Info / Description", SuiteSettings.Info.Description);
			ShowSetting("Info / Notes", SuiteSettings.Info.Notes);
			
			w.AddLineBreaksTb(1);

			w.AddDescTextTb("<indent/>");

			// duplicate information - different way to access
			// ShowSetting("Info / Header / Notes", SuiteSettings.Info.Header.Notes);
			// ShowSetting("Description"     , SuiteSettings.Info.Header.Description);
			// ShowSetting("DataClassVersion", SuiteSettings.Info.Header.DataClassVersion);
			// w.AddLineBreaksTb(1);
			// template information - used when a "from scratch" suite file is created
			// ShowSetting("Info / Data / FileDescription", SuiteSettings.Info.Data.DataFileDescription);
			// ShowSetting("Info / Data / FileNotes", SuiteSettings.Info.Data.DataFileNotes);
			// ShowSetting("Info / Data / FileVersion", SuiteSettings.Info.Data.DataFileVersion);
			// ShowSetting("Data / FileDescription", SuiteSettings.Data.DataFileDescription);
			// ShowSetting("Data / FileNotes"      , SuiteSettings.Data.DataFileNotes);
			// ShowSetting("Data / FileVersion"    , SuiteSettings.Data.DataFileVersion);
		}

		public void ShowMachSettings()
		{
			ShowSettingFilePreface("Machine Settings",MachSettings.Path.settingFilePath, MachSettings.Path.Exists, false, true, 0, 4);

			ShowSetting("LastClassificationFileId", MachSettings.Data.LastClassificationFileId);

			w.AddLineBreaksTb(1);

			w.AddDescTextLineTb("<lawngreen>Machine Settings (Data & Info) Settings</lawngreen>");
			w.AddLineBreaksTb(1);

			ShowSetting("Info / Header / SavedBy", MachSettings.Info.Header.SavedBy);
			ShowSetting("Info / Header / SavedDateTime", MachSettings.Info.Header.SaveDateTime);
			ShowSetting("Info / IsSuite?", MachSettings.Info.IsSuite.ToString());
			ShowSetting("Info / DataClassVersion", MachSettings.Info.DataClassVersion);
			ShowSetting("Info / Description", MachSettings.Info.Description);
			ShowSetting("Info / Notes", MachSettings.Info.Notes);
			
			w.AddLineBreaksTb(1);

			w.AddDescTextTb("<indent/>");
		}
		
		public void ShowSiteSettings()
		{
			//             0   1   2   3   4   5   6   7
			int[] pos1 = [ 24, 10, 10, 10, 10, 10, 10, 10];
			string[][] fmt = [ 
				["<lawngreen>", "</lawngreen>"], // 0
				["<cyan>", "</cyan>"],           // 1
				["<magenta>", "</magenta>"],     // 2
				["<magenta>", "</magenta>"],     // 3
				["<magenta>", "</magenta>"],     // 4
				["<magenta>", "</magenta>"],     // 5
				["<magenta>", "</magenta>"],     // 6
				["<dimgray>", "</dimgray>"],     // 7
			];

			List<object> vals;

			ShowSettingFilePreface("Site Settings",SiteSettings.Path.settingFilePath, SiteSettings.Path.Exists, false, true, 0, 4);

			int count = 0;


			w.AddDescTextLineTb("List of Admin Users");
			w.AddLineBreaksTb(1);

			if (SiteSettings.Data.AdminUsers != null && SiteSettings.Data.AdminUsers.Count > 0) 
			{
				foreach (string adminUser in SiteSettings.Data.AdminUsers)
				{
					ShowSetting($"Admin User {count++}", adminUser);
				}
			}
			else
			{
				ShowSetting("Admin Users", "<red>No Admin Users Found</red>");
			}

			count = 0;

			w.AddLineBreaksTb(1);

			w.AddDescTextLineTb("List of Installed Seed Files");
			w.AddLineBreaksTb(1);

			if (SiteSettings.Data.InstalledSeedFiles != null && SiteSettings.Data.InstalledSeedFiles.Count > 0)
			{
				
				w.ClearAltRowFormat();
				w.StartTb("<foreground color='gray'/>");

				w.AddDescTextLineTb(w.ReportRow(pos1, ["Key", "Status", "Fld Id", "FileName", "UserName", "Selected", "Local", "keep"]));
				w.AddDescTextLineTb(w.ReportRow(pos1, [8], null, null, null, null, true, ["-"], ["-+-"]));

				w.AssignAltRowFormat(preFmt);

				foreach (ConfigSeedFile seedFileData in SiteSettings.Data.InstalledSeedFiles)
				{
					vals =
					[
						seedFileData.Key,
						seedFileData.Status,
						seedFileData.FileId,
						seedFileData.FileName,
						seedFileData.UserName,
						seedFileData.SelectedSeed,
						seedFileData.Local,
						seedFileData.Keep,
					];
					w.AddDescTextLineTb(w.ReportRow(pos1, vals.ToArray(), fmt));

					w.AddLineBreaksTb(1);

					ShowSetting("FilePathLocal",  seedFileData.FilePathLocal.FullFilePath);
					ShowSetting("SampleFilePath", seedFileData.SampleFilePath);
					ShowSetting("SampleFile",     seedFileData.SampleFile.FullFilePath);

					w.AddLineBreaksTb(1);
				}
			}
			else
			{
				ShowSetting("Seed Files", "<red>No Seed Files Found</red>");

				w.AddLineBreaksTb(1);
			}
			
			w.AddDescTextLineTb("<lawngreen>Site Settings (Data & Info) Settings</lawngreen>");
			w.AddLineBreaksTb(1);

			ShowSetting("Info / Header / SavedBy", SiteSettings.Info.Header.SavedBy);
			ShowSetting("Info / Header / SavedDateTime", SiteSettings.Info.Header.SaveDateTime);
			ShowSetting("Info / IsSuite?", SiteSettings.Info.IsSuite.ToString());
			ShowSetting("Info / DataClassVersion", SiteSettings.Info.DataClassVersion);
			ShowSetting("Info / Description", SiteSettings.Info.Description);
			ShowSetting("Info / Notes", SiteSettings.Info.Notes);
			
			w.AddLineBreaksTb(1);

			w.AddDescTextTb("<indent/>");
		}




		// support routines

		public static void ShowSetting(string title, string value, string pf = "darkgray", string sf = "cyan")
		{
			w.AddDescTextLineTb($"<{pf}>{title} <repeat text='.' tocolumn='{showSetgColumn.ToString()}'/></{pf}> <{sf}>{value}</{sf}>");
		}

		public static void ShowSettingFilePreface(string title, string path, bool exists, bool indent1=true, bool indent2=true, int ident1=4, int ident2=8)
		{
			w.AddLineBreaksTb(1);

			if (indent1) w.StartTb($"<indent spaces='{ident1}'/>");

			w.AddDescTextLineTb("<repeat text='-' quantity='85'/>");
			w.AddDescTextLineTb($"<lawngreen>{title}</lawngreen>");

			w.AddLineBreaksTb(1);

			if (indent2) w.StartTb($"<indent spaces='{ident2}'/>");

			w.AddDescTextLineTb($"<darkgray>Path   | </darkgray><dimgray>{path}</dimgray>");
			w.AddDescTextLineTb($"<darkgray>Exists | </darkgray><dimgray>{exists}</dimgray>");

			w.AddLineBreaksTb(1);
		}

		public override string ToString()
		{
			return $"this is {nameof(GeneralSettingsMgr)}";
		}
	}
}
