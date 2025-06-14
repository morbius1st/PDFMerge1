#region + Using Directives

using System;
using SettingsManager;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   9/2/2020 6:01:46 AM

namespace SettingsManager
{
	public static class FileLocationSupport
	{
		// shortcut: "." + FileLocationSupport.DATA_FILE_EXT
		public const string DATA_FILE_EXT = "xml";

		public static readonly string FILE_PATTERN = "*."  +  DATA_FILE_EXT;

		public const string SORT_NAME_PROP = "SortName";

		public const string DEFAULT_FOLDER_NAME = "Default";

		// root locations
		public static string FileRootLocationUser => $"{UserSettings.Path.SuiteFolderPath}";

		public static string FileRootLocationDefault => $"{SuiteSettings.Path.FilePath.FolderPath}";

		public static void ShowLocations(IWin iw)
		{
			iw.DebugMsgLine($"sht metric file location    | {ShtMetricsFileLocation}");
			iw.DebugMsgLine($"classification file location| {ClassifFileLocation}");
		}

	#region sheet metrics

		// sheet metrics = data about sheets and box settings

		public const string SHEET_STORAGE_FOLDER_NAME = ".Sheet Metrics Files";

		// shortcut: FileLocationSupport.ClassifFileLocation + "\\"
		/// <summary>
		/// the general location for all of the user sheet metrics files
		/// </summary>
		public static string ShtMetricsFileLocation => $"{FileRootLocationUser}\\{SHEET_STORAGE_FOLDER_NAME}";

		// shortcut: FileLocationSupport.ClassifFileLocationUser + "\\" 
		/// <summary>
		/// the location for the user's sheet metrics files
		/// </summary>
		public static string  ShtMetricsFileLocationUser => $"{ShtMetricsFileLocation}\\{Environment.UserName}";

		/// <summary>
		/// the file path to a user's sheet metrics file
		/// </summary>
		public static string ShtMetricsFilePathUser(string fileId)
		{
			return ShtMetricsFileLocationUser + $"\\({Environment.UserName}) {fileId}.{DATA_FILE_EXT}";
		}


		/// <summary>
		/// the location for all of the default classification files, if any (management provided)
		/// will also contain some examples
		/// </summary>
		public static string ShtMetricsFileLocationDefault => $"{FileRootLocationDefault}\\{SHEET_STORAGE_FOLDER_NAME}";


		/// <summary>
		/// the file path to a default sheet metrics file
		/// </summary>
		public static string ShtMetricsFilePathDefault(string fileId)
		{
			return ShtMetricsFileLocationDefault + $"\\({DEFAULT_FOLDER_NAME}) {fileId}.{DATA_FILE_EXT}";
		}

	#endregion

	#region classification file

		// shortcut: FileLocationSupport.SAMPLE_FOLDER
		public const string SAMPLE_FOLDER = "Sample Files";

		// shortcut: "." + FileLocationSupport.SAMPLE_FILE_EXT
		public const string SAMPLE_FILE_EXT = "Sample";

		public const string CLASSIF_STORAGE_FOLDER_NAME = ".User Classification Files";

		// shortcut: FileLocationSupport.ClassifFileLocation + "\\"
		/// <summary>
		/// the location for all of the user classification files
		/// </summary>
		public static string ClassifFileLocation => $"{FileRootLocationUser}\\{CLASSIF_STORAGE_FOLDER_NAME}";

		/// <summary>
		/// the path for the root folder of all of the user classification files<br/>
		/// equals: "C:\ProgramData\{app name}\Andy\User Classification Files  (SM7.5)<br/>
		/// or equals: "C:\ProgramData\{app name}\Andy\{app name}\User Classification Files  (pre SM7.5)<br/>
		/// </summary>
		public static FilePath<FileNameSimple> ClassifFileLocationPath =>
			new FilePath<FileNameSimple>(ClassifFileLocation);


		// shortcut: FileLocationSupport.ClassifFileLocationUser + "\\" 
		/// <summary>
		/// the location for the user's classification files
		/// </summary>
		public static string ClassifFileLocationUser => $"{ClassifFileLocation}\\{Environment.UserName}";

		/// <summary>
		/// the path for the user's user classification files<br/>
		/// equals: "C:\ProgramData\{app name}\Andy\User Classification Files\{user name}  (SM7.5)<br/>
		/// or equals: "C:\ProgramData\{app name}\Andy\{app name}\User Classification Files\{user name}  (pre SM7.5)<br/>
		/// </summary>
		public static FilePath<FileNameSimple> ClassifFilePathUser =>
			new FilePath<FileNameSimple>(ClassifFileLocationUser);

		/// <summary>
		/// the file path to a user's classification file
		/// </summary>
		public static string ClassifFileUserPath(string fileId)
		{
			return ClassifFileLocationUser + $"\\({Environment.UserName}) {fileId}.{DATA_FILE_EXT}";
		}

		// shortcut: FileLocationSupport.ClassifSampleFileLocationUser
		/// <summary>
		/// the location for the user's classification file's sample folder
		/// </summary>
		public static string ClassifSampleFileLocationUser => $"{ClassifFileLocationUser}\\{SAMPLE_FOLDER}";

		/// <summary>
		/// the file path to a user's sample file
		/// </summary>
		public static string ClassifSampleFilePathUser(string fileId)
		{
			return ClassifSampleFileLocationUser + $"\\({Environment.UserName}) {fileId}.{SAMPLE_FILE_EXT}";
		}


		/// <summary>
		/// the location for all of the default classification files, if any (management provided)
		/// will also contain some examples
		/// </summary>
		public static string ClassifFileLocationDefault => $"{FileRootLocationDefault}\\{CLASSIF_STORAGE_FOLDER_NAME}";


		/// <summary>
		/// the file path to a default sheet metrics file
		/// </summary>
		public static string ClassifFileDefaultPath(string fileId)
		{
			return ClassifFileLocationDefault + $"\\({DEFAULT_FOLDER_NAME}) {fileId}.{DATA_FILE_EXT}";
		}

		/// <summary>
		/// the location for all of the default classification files, if any (management provided)
		/// will also contain some examples
		/// </summary>
		public static string ClassifSampleFileLocationDefault => $"{ClassifFileLocationDefault}\\{SAMPLE_FOLDER}";


		/// <summary>
		/// the file path to a default sheet metrics file
		/// </summary>
		public static string ClassifSampleFileDefaultPath(string fileId)
		{
			return ClassifSampleFileLocationDefault + $"\\({DEFAULT_FOLDER_NAME}) {fileId}.{SAMPLE_FILE_EXT}";
		}

	#endregion

		
	}
}