#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettingsManager;
using UtilityLibrary;

using static SettingsManager.FileLocationSupport;

#endregion

// user name: jeffs
// created:   11/28/2024 1:16:34 PM

namespace SuiteInfoEditor.Support
{
	public class Operations1
	{
		private const int SUBJECT_WIDTH = -50;

		private ITblkFmt w;

		public Operations1(ITblkFmt w)
		{
			this.w = w;
		}

		//
		// public void ShowLocations()
		// {
		// 	w.DebugMsgLine("Showing Locations");
		//
		// 	w.DebugMsgLine("\n*** consts (1) ***\n");
		//
		// 	w.DebugMsgLine($"\t{"data file extension (1)"                               , SUBJECT_WIDTH} | {DATA_FILE_EXT}");
		// 	w.DebugMsgLine($"\t{"data file pattern (2)"                                 , SUBJECT_WIDTH} | {FILE_PATTERN}");
		// 	w.DebugMsgLine($"\t{"sort name prop (3)"                                    , SUBJECT_WIDTH} | {SORT_NAME_PROP}");
		// 	w.DebugMsgLine($"\t{"default folder name (4)"                               , SUBJECT_WIDTH} | {DEFAULT_FOLDER_NAME}");
		// 	
		// 	w.DebugMsgLine("\n*** system (101) ***\n");
		// 	
		// 	w.DebugMsgLine($"\t{"user name (101)"                                       , SUBJECT_WIDTH} | {Environment.UserName}");
		//
		//
		// 	w.DebugMsgLine("\n*** locations - general (A) ***");
		//
		// 	w.DebugMsgLine($"\t{"FileRootLocationUser (A1)"                             , SUBJECT_WIDTH} | {FileRootLocationUser}");
		// 	w.DebugMsgLine($"\t{"FileRootLocationDefault (A2)"                          , SUBJECT_WIDTH} | {FileRootLocationDefault}");
		//
		//
		//
		// 	w.DebugMsgLine("\n\n*** locations - sheet metrics (B1) ***");
		//
		// 	w.DebugMsgLine($"\t{"sheet metrics folder name (B1)"                        , SUBJECT_WIDTH} | {SHEET_STORAGE_FOLDER_NAME}");
		//
		// 	w.DebugMsgLine("\n*** locations - sheet metrics - user (B11) ***");
		//
		// 	w.DebugMsgLine($"\t{"general user's location         (B11=A1+B1)"           , SUBJECT_WIDTH} | {ShtMetricsFileLocation}");
		// 	w.DebugMsgLine($"\t{"user's location                 (B12=B11+101)"         , SUBJECT_WIDTH} | {ShtMetricsFileLocationUser}");
		// 	w.DebugMsgLine($"\t{"user's file name                (B101=B12++)"          , SUBJECT_WIDTH} | {ShtMetricsFilePathUser("USER FILE ID")}");
		//
		//
		// 	w.DebugMsgLine("\n*** locations - sheet metrics - default (machine) (B51) ***");
		//
		// 	w.DebugMsgLine($"\t{"general default location       (A21)"                  , SUBJECT_WIDTH} | {FileRootLocationDefault}");
		// 	w.DebugMsgLine($"\t{"default location               (B51=B11+4)"            , SUBJECT_WIDTH} | {ShtMetricsFileLocationDefault}");
		// 	w.DebugMsgLine($"\t{"default's file name            (B102=B51++)"           , SUBJECT_WIDTH} | {ShtMetricsFilePathDefault("DEFAULT FILE ID")}");
		//
		//
		//
		// 	w.DebugMsgLine("\n\n*** locations - classification file (C1) ***");
		//
		// 	w.DebugMsgLine($"\t{"SAMPLE_FOLDER (C1)"                                    , SUBJECT_WIDTH} | {SAMPLE_FOLDER}");
		// 	w.DebugMsgLine($"\t{"SAMPLE_FILE_EXT (C2)"                                  , SUBJECT_WIDTH} | {SAMPLE_FILE_EXT}");
		// 	w.DebugMsgLine($"\t{"CLASSIF_STORAGE_FOLDER_NAME (C3)"                      , SUBJECT_WIDTH} | {CLASSIF_STORAGE_FOLDER_NAME}");
		// 	
		//
		// 	w.DebugMsgLine("\n*** locations - classification file - user (C11) ***");
		// 	
		// 	// user location
		// 	w.DebugMsgLine($"\t{"general user's location        (C11=A1+C3)"            , SUBJECT_WIDTH} | {ClassifFileLocation}");
		// 	w.DebugMsgLine($"\t{"general user's location (path) (C12->C11)"             , SUBJECT_WIDTH} | {ClassifFileLocationPath.FolderPath}");
		//
		// 	w.DebugMsgLine($"\t{"user's location                (C13=C11+101)"          , SUBJECT_WIDTH} | {ClassifFileLocationUser}");
		// 	w.DebugMsgLine($"\t{"user's location (path)         (C14->C13)"             , SUBJECT_WIDTH} | {ClassifFilePathUser.FolderPath}");
		// 	w.DebugMsgLine($"\t{"user's file name               (C101=C13++)"           , SUBJECT_WIDTH} | {ClassifFileUserPath("USER FILE ID")}");
		// 	
		// 	w.DebugMsgLine($"\t{"user's sample location         (C15=C11+4))"           , SUBJECT_WIDTH} | {ClassifSampleFileLocationUser}");
		// 	w.DebugMsgLine($"\t{"user's sample file name        (C101=C13++)"           , SUBJECT_WIDTH} | {ClassifSampleFilePathUser("USER FILE ID")}");
		//
		// 	w.DebugMsgLine("\n*** locations - classification file - default (C51) ***");
		// 	
		// 	w.DebugMsgLine($"\t{"general default location       (A2)"                   , SUBJECT_WIDTH} | {FileRootLocationDefault}");
		//
		// 	w.DebugMsgLine($"\t{"default location               (C51=A2+4))"            , SUBJECT_WIDTH} | {ClassifFileLocationDefault}");
		// 	w.DebugMsgLine($"\t{"default file name              (C102=C51++)"           , SUBJECT_WIDTH} | {ClassifFileDefaultPath("DEFAULT FILE ID")}");
		//
		// 	w.DebugMsgLine($"\t{"default sample location        (C53=A21+C1))"          , SUBJECT_WIDTH} | {ClassifSampleFileLocationDefault}");
		// 	w.DebugMsgLine($"\t{"default sample file name       (C103=C53++)"           , SUBJECT_WIDTH} | {ClassifSampleFileDefaultPath("DEFAULT FILE ID")}");
		//
		//
		//
		// }

		public override string ToString()
		{
			return $"this is {nameof(Operations1)}";
		}
	}
}
