using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using AndyShared.ConfigSupport;
using UtilityLibrary;
using static SettingsManager.FileLocationSupport;
using UtilityLibrary;

// user name: jeffs
// created:   3/23/2025 11:51:06 PM

namespace SettingsManager
{
	public class FileLocationShowInfo
	{
		private static string WHT1 = "<white>";
		private static string WHT2 = "</white>";
		private static string COLORA1 =  "<limegreen>";
		private static string COLORA2 = "</limegreen>";
		private static string DGRY1 =  "<dimgray>";
		private static string DGRY2 = "</dimgray>";


		private const int SUBJECT_WIDTH = -50;

		private static CsFlowDocManager w = CsFlowDocManager.Instance;

		private static void addToTblk(string text1)
		{
			w.AddDescTextLineTb($"{WHT1}{text1}{WHT2}");
		}

		private static void addToTblk(string text1, string text2)
		{
			w.AddDescTextLineTb($"   {DGRY1}{text1,SUBJECT_WIDTH}{DGRY2}{WHT1}|{WHT2} {COLORA1}{text2}{COLORA2}");
		}

				// @formatter:off
		public static void ShowLocations()
		{
			w.StartTb("<foreground color='gray'/>");

			addToTblk("Showing Locations");

			addToTblk($"from", $"{typeof(SettingsManager.FileLocationSupport).FullName}");
			
			w.AddLineBreaks(1);
			
			addToTblk("*** consts (1) ***");
			w.AddLineBreaks(1);

			addToTblk("data file extension (1)"                       , $"{DATA_FILE_EXT}");
			addToTblk("data file pattern (2)"                         , $"{FILE_PATTERN}");
			addToTblk("sort name prop (3)"                            , $"{SORT_NAME_PROP}");
			addToTblk("default folder name (4)"                       , $"{DEFAULT_FOLDER_NAME}");

			w.AddLineBreaks(1);
			addToTblk("*** system (101) ***");
			w.AddLineBreaks(1);
			
			addToTblk("user name (101)"                               , $"{Environment.UserName}");

			w.AddLineBreaks(1);
			addToTblk("*** locations - general (A) ***");

			addToTblk("FileRootLocationUser (A1)"                     , $"{FileRootLocationUser}");
			addToTblk("FileRootLocationDefault (A2)"                  , $"{FileRootLocationDefault}");

			w.AddLineBreaks(2);
			addToTblk("<background color='70, 70, 70'>*** locations - sheet metrics (B1) ***<repeat text=' ' tocolumn='54'/></background>");

			addToTblk("sheet metrics folder name (B1)"                , $"{SHEET_STORAGE_FOLDER_NAME}");

			w.AddLineBreaks(1);
			addToTblk("*** locations - sheet metrics - user (B11) ***");

			addToTblk("general user's location         (B11=A1+B1)"  , $"{ShtMetricsFileLocation}");
			addToTblk("user's location                 (B12=B11+101)", $"{ShtMetricsFileLocationUser}");
			addToTblk("user's file name                (B101=B12++)" , $"{ShtMetricsFilePathUser("{{USER FILE ID}}")}");

			w.AddLineBreaks(1);
			addToTblk("*** locations - sheet metrics - default (machine) (B51) ***");

			addToTblk("general default location       (A21)"        , $"{FileRootLocationDefault}");
			addToTblk("default location               (B51=B11+4)"  , $"{ShtMetricsFileLocationDefault}");
			addToTblk("default's file name            (B102=B51++)" , $"{ShtMetricsFilePathDefault("{{DEFAULT FILE ID}}")}");

			w.AddLineBreaks(2);
			addToTblk("<background color='70, 70, 70'>*** locations - classification file (C1) ***<repeat text=' ' tocolumn='54'/></background>");

			addToTblk("SAMPLE_FOLDER (C1)"                          , $"{SAMPLE_FOLDER}");
			addToTblk("SAMPLE_FILE_EXT (C2)"                        , $"{SAMPLE_FILE_EXT}");
			addToTblk("CLASSIF_STORAGE_FOLDER_NAME (C3)"            , $"{CLASSIF_STORAGE_FOLDER_NAME}");
			
			w.AddLineBreaks(1);
			addToTblk("*** locations - classification file - user (C11) ***");
			
			// user location
			addToTblk("general user's location        (C11=A1+C3)"  , $"{ClassifFileLocation}");
			addToTblk("general user's location (path) (C12->C11)"   , $"{ClassifFileLocationPath.FolderPath}");
			addToTblk("user's location                (C13=C11+101)", $"{ClassifFileLocationUser}");
			addToTblk("user's location (path)         (C14->C13)"   , $"{ClassifFilePathUser.FolderPath}");
			addToTblk("user's file name               (C101=C13++)" , $"{ClassifFileUserPath("{{USER FILE ID}}")}");
			addToTblk("user's sample location         (C15=C11+4))" , $"{ClassifSampleFileLocationUser}");
			addToTblk("user's sample file name        (C101=C13++)" , $"{ClassifSampleFilePathUser("{{USER FILE ID}}")}");

			w.AddLineBreaks(1);
			addToTblk("*** locations - classification file - default (C51) ***");
			
			addToTblk("general default location       (A2)"         , $"{FileRootLocationDefault}");
			addToTblk("default location               (C51=A2+4))"  , $"{ClassifFileLocationDefault}");
			addToTblk("default file name              (C102=C51++)" , $"{ClassifFileDefaultPath("{{DEFAULT FILE ID}}")}");
			addToTblk("default sample location        (C53=A21+C1))", $"{ClassifSampleFileLocationDefault}");
			addToTblk("default sample file name       (C103=C53++)" , $"{ClassifSampleFileDefaultPath("{{DEFAULT FILE ID}}")}");


			// @formatter:on
		}

		public static void ShowLocations2()
		{
			int[] pos = [ 20, 20, 10, 10, 10, 10, 10, 10 ];
			string[][] fldFmt = [ ["<lawngreen>", "</lawngreen>"],["<cyan>", "</cyan>"], ["<magenta>", "</magenta>"]];

			int[] pos2 = [ 20, 98 ];
			string[][] fldFmt2 = [ ["<lawngreen>", "</lawngreen>"],["<cyan>", "</cyan>"] ];

			string baseFmt = "<foreground color='gray'/>";

			string[] preFmt = ["<textformat background='40, 40, 40'/>", null];


			const string SPACER = CsFlowDocManager.HORIZ_LINE;
			const string DIVIDER = CsFlowDocManager.VERT_LINE;
			const string INTERSECTION = $"{SPACER}{CsFlowDocManager.INTERRSECTION}{SPACER}";

			w.StartTb("<foreground color='gray'/>");


			addToTblk("Global");

			addToTblk("Suite Name", $"{Heading.SuiteName}");

			w.AddLineBreaks(1);


			addToTblk("Site Setting File - suite specific settings per site (company)");

			addToTblk("File Path", $"{SiteSettings.Path.SettingFilePath}");
			addToTblk("File Name", $"{SiteSettings.Path.FileName}");
			addToTblk("Exists?"  , $"{SiteSettings.Path.Exists}");
			
			w.AddLineBreaks(1);
			
			addToTblk("Admin Users Count", $"{SiteSettings.Data.AdminUsers.Count}");

			int idx = 0;

			foreach (string admin in SiteSettings.Data.AdminUsers)
			{
				addToTblk($"Admin User {idx}", $"{admin}");
			}

			w.AddLineBreaks(1);

			addToTblk("Installed Seed Files Count", $"{SiteSettings.Data.InstalledSeedFiles.Count}");

			w.AddLineBreaks(1);

			w.ClearAltRowFormat();
			w.StartTb($"{baseFmt}<indent spaces='4'/>");

			w.AddDescTextLineTb(w.ReportRow(pos, ["Id", "Key", "UserName", "!Status", "!Local", "!Selected", "!Keep", "!Copy", ], null, null, null, null, null));
			
			w.AddDescTextLineTb(w.ReportRow(pos, [8], null, null, null, null, true, [SPACER], [INTERSECTION] ));

			w.AssignAltRowFormat(preFmt);

			foreach (ConfigSeedFile sd in SiteSettings.Data.InstalledSeedFiles)
			{
				w.AddDescTextLineTb(w.ReportRow(pos, [sd.FileId, sd.Key, sd.UserName, sd.Status, sd.Local, sd.SelectedSeed, sd.Keep, sd.Copy], fldFmt));

				w.AddDescTextLineTb(w.ReportRow(pos2, ["'FileName'", sd.FileName], fldFmt2));
				
				w.AddDescTextLineTb(w.ReportRow(pos2, ["'Folder'", sd.Folder], fldFmt2));
				
				w.AddDescTextLineTb(w.ReportRow(pos2, ["'SampleFilePath'", sd.SampleFilePath]));
				
				w.AddDescTextLineTb(w.ReportRow(pos2, ["'SampleFile'", sd.SampleFile.FullFilePath]));
				
				w.AddDescTextLineTb(w.ReportRow(pos2, ["'FilePathLocal'", sd.FilePathLocal.FullFilePath]));

				w.AddLineBreaks(1);
			}

			w.AddDescTextTb("<indent/>");

			w.AddLineBreaks(1);

			addToTblk("Machine Setting File - suite specific settings per machine");

			addToTblk("File Path", $"{MachSettings.Path.SettingFilePath}");
			addToTblk("File Name", $"{MachSettings.Path.FileName}");
			addToTblk("Exists?"  , $"{MachSettings.Path.Exists}");
			addToTblk("Data", "no data");

			w.AddLineBreaks(1);

			addToTblk("Suite Setting File - suite specific settings per user");

			addToTblk("Root Path?", $"{SuiteSettings.SiteRootPath}");
			addToTblk("File Path", $"{SuiteSettings.Path.SettingFilePath}");
			addToTblk("File Name", $"{SuiteSettings.Path.FileName}");
			addToTblk("Exists?"  , $"{SuiteSettings.Path.Exists}");
			addToTblk("Data", $"{SuiteSettings.Data.SiteRootPath}");
			
			w.AddLineBreaks(1);

			addToTblk("App Setting File - app specific settings per user");

			addToTblk("File Path", $"{AppSettings.Path.SettingFilePath}");
			addToTblk("File Name", $"{AppSettings.Path.FileName}");
			addToTblk("Exists?"  , $"{AppSettings.Path.Exists}");
			addToTblk("Data", "no data");

			w.AddLineBreaks(1);

			addToTblk("User Setting File - personal settings per app");

			addToTblk("File Path", $"{UserSettings.Path.SettingFilePath}");
			addToTblk("File Name", $"{UserSettings.Path.FileName}");
			addToTblk("Exists?"  , $"{UserSettings.Path.Exists}");
			addToTblk("Data", "no data");


		}

		// public static void ShowLocations2()
		// {
		// 	// @formatter:off
		//
		// 	w.StartTb("<foreground color='gray'/>");
		//
		// 	w.AddTextLineTb("Showing Locations");
		//
		// 	w.AddTextLineTb($"\t{"from"                                                  , SUBJECT_WIDTH} | {typeof(SettingsManager.FileLocationSupport).FullName}");
		// 	w.AddTextLineTb("\n");
		// 	
		// 	w.AddTextLineTb("\n*** consts (1) ***\n");
		//
		// 	w.AddTextLineTb($"\t{"data file extension (1)"                               , SUBJECT_WIDTH} | {DATA_FILE_EXT}");
		// 	w.AddTextLineTb($"\t{"data file pattern (2)"                                 , SUBJECT_WIDTH} | {FILE_PATTERN}");
		// 	w.AddTextLineTb($"\t{"sort name prop (3)"                                    , SUBJECT_WIDTH} | {SORT_NAME_PROP}");
		// 	w.AddTextLineTb($"\t{"default folder name (4)"                               , SUBJECT_WIDTH} | {DEFAULT_FOLDER_NAME}");
		// 	
		// 	w.AddTextLineTb("\n*** system (101) ***\n");
		// 	
		// 	w.AddTextLineTb($"\t{"user name (101)"                                       , SUBJECT_WIDTH} | {Environment.UserName}");
		//
		//
		// 	w.AddTextLineTb("\n*** locations - general (A) ***");
		//
		// 	w.AddTextLineTb($"\t{"FileRootLocationUser (A1)"                             , SUBJECT_WIDTH} | {FileRootLocationUser}");
		// 	w.AddTextLineTb($"\t{"FileRootLocationDefault (A2)"                          , SUBJECT_WIDTH} | {FileRootLocationDefault}");
		//
		//
		//
		// 	w.AddTextLineTb("\n\n*** locations - sheet metrics (B1) ***");
		//
		// 	w.AddTextLineTb($"\t{"sheet metrics folder name (B1)"                        , SUBJECT_WIDTH} | {SHEET_STORAGE_FOLDER_NAME}");
		//
		// 	w.AddTextLineTb("\n*** locations - sheet metrics - user (B11) ***");
		//
		// 	w.AddTextLineTb($"\t{"general user's location         (B11=A1+B1)"           , SUBJECT_WIDTH} | {ShtMetricsFileLocation}");
		// 	w.AddTextLineTb($"\t{"user's location                 (B12=B11+101)"         , SUBJECT_WIDTH} | {ShtMetricsFileLocationUser}");
		// 	w.AddTextLineTb($"\t{"user's file name                (B101=B12++)"          , SUBJECT_WIDTH} | {ShtMetricsFilePathUser("USER FILE ID")}");
		//
		//
		// 	w.AddTextLineTb("\n*** locations - sheet metrics - default (machine) (B51) ***");
		//
		// 	w.AddTextLineTb($"\t{"general default location       (A21)"                  , SUBJECT_WIDTH} | {FileRootLocationDefault}");
		// 	w.AddTextLineTb($"\t{"default location               (B51=B11+4)"            , SUBJECT_WIDTH} | {ShtMetricsFileLocationDefault}");
		// 	w.AddTextLineTb($"\t{"default's file name            (B102=B51++)"           , SUBJECT_WIDTH} | {ShtMetricsFilePathDefault("DEFAULT FILE ID")}");
		//
		//
		//
		// 	w.AddTextLineTb("\n\n*** locations - classification file (C1) ***");
		//
		// 	w.AddTextLineTb($"\t{"SAMPLE_FOLDER (C1)"                                    , SUBJECT_WIDTH} | {SAMPLE_FOLDER}");
		// 	w.AddTextLineTb($"\t{"SAMPLE_FILE_EXT (C2)"                                  , SUBJECT_WIDTH} | {SAMPLE_FILE_EXT}");
		// 	w.AddTextLineTb($"\t{"CLASSIF_STORAGE_FOLDER_NAME (C3)"                      , SUBJECT_WIDTH} | {CLASSIF_STORAGE_FOLDER_NAME}");
		// 	
		//
		// 	w.AddTextLineTb("\n*** locations - classification file - user (C11) ***");
		// 	
		// 	// user location
		// 	w.AddTextLineTb($"\t{"general user's location        (C11=A1+C3)"            , SUBJECT_WIDTH} | {ClassifFileLocation}");
		// 	w.AddTextLineTb($"\t{"general user's location (path) (C12->C11)"             , SUBJECT_WIDTH} | {ClassifFileLocationPath.FolderPath}");
		//
		// 	w.AddTextLineTb($"\t{"user's location                (C13=C11+101)"          , SUBJECT_WIDTH} | {ClassifFileLocationUser}");
		// 	w.AddTextLineTb($"\t{"user's location (path)         (C14->C13)"             , SUBJECT_WIDTH} | {ClassifFilePathUser.FolderPath}");
		// 	w.AddTextLineTb($"\t{"user's file name               (C101=C13++)"           , SUBJECT_WIDTH} | {ClassifFileUserPath("USER FILE ID")}");
		// 	
		// 	w.AddTextLineTb($"\t{"user's sample location         (C15=C11+4))"           , SUBJECT_WIDTH} | {ClassifSampleFileLocationUser}");
		// 	w.AddTextLineTb($"\t{"user's sample file name        (C101=C13++)"           , SUBJECT_WIDTH} | {ClassifSampleFilePathUser("USER FILE ID")}");
		//
		// 	w.AddTextLineTb("\n*** locations - classification file - default (C51) ***");
		// 	
		// 	w.AddTextLineTb($"\t{"general default location       (A2)"                   , SUBJECT_WIDTH} | {FileRootLocationDefault}");
		//
		// 	w.AddTextLineTb($"\t{"default location               (C51=A2+4))"            , SUBJECT_WIDTH} | {ClassifFileLocationDefault}");
		// 	w.AddTextLineTb($"\t{"default file name              (C102=C51++)"           , SUBJECT_WIDTH} | {ClassifFileDefaultPath("DEFAULT FILE ID")}");
		//
		// 	w.AddTextLineTb($"\t{"default sample location        (C53=A21+C1))"          , SUBJECT_WIDTH} | {ClassifSampleFileLocationDefault}");
		// 	w.AddTextLineTb($"\t{"default sample file name       (C103=C53++)"           , SUBJECT_WIDTH} | {ClassifSampleFileDefaultPath("DEFAULT FILE ID")}");
		//
		//
		// 	// @formatter:on
		// }

		public override string ToString()
		{
			return $"this is {nameof(FileLocationShowInfo)}";
		}
	}
}