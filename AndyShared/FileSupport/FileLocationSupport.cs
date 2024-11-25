#region + Using Directives

using System;
using SettingsManager;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   9/2/2020 6:01:46 AM

namespace AndyShared.FileSupport
{
	public static class FileLocationSupport
	{
		// shortcut: "." + FileLocationSupport.DATA_FILE_EXT
		public const string DATA_FILE_EXT = "xml";

		public static readonly string USER_STORAGE_PATTERN = "*."+DATA_FILE_EXT;

		public const string SORT_NAME_PROP = "SortName";


		public const string DEFAULT_FOLDER_NAME = "Default";

		public static string UserFileRootLocation => $"{UserSettings.Path.SuiteFolderPath}";

	#region sheet metrics

		public const string SHEET_STORAGE_FOLDER_NAME = ".Sheet Metrics Files";

		// shortcut: FileLocationSupport.ClassifFileLocation + "\\"
		/// <summary>
		/// the location for all of the sheet metrics files
		/// </summary>
		public static string ShtMetricsFileLocation => $"{UserFileRootLocation}\\{SHEET_STORAGE_FOLDER_NAME}";

		// shortcut: FileLocationSupport.ClassifFileLocationUser + "\\" 
		/// <summary>
		/// the location for the user's sheet metrics files
		/// </summary>
		public static string  ShtMetricsLocationUser => $"{ShtMetricsFileLocation}\\{Environment.UserName}";

		/// <summary>
		/// the location for all of the default classification files, if any (management provided)
		/// will also contain some examples
		/// </summary>
		public static string ShtMetricsLocationDefault => $"{ClassifFileLocation}\\{DEFAULT_FOLDER_NAME}";


		/// <summary>
		/// the file path to a user's sheet metrics file
		/// </summary>
		public static string SheetMetricsFilePathUser(string fileId)
		{
			return ShtMetricsLocationUser + $"\\({Environment.UserName}) {fileId}.{DATA_FILE_EXT}";
		}

		/// <summary>
		/// the file path to a default sheet metrics file
		/// </summary>
		public static string SheetMetricsFilePathDefault(string fileId)
		{
			return ShtMetricsLocationDefault + $"\\({DEFAULT_FOLDER_NAME}) {fileId}.{DATA_FILE_EXT}";
		}

	#endregion

	#region classification file

		// shortcut: FileLocationSupport.SAMPLE_FOLDER
		public const string SAMPLE_FOLDER = "Sample Files";
		// shortcut: "." + FileLocationSupport.SAMPLE_FILE_EXT
		public const string SAMPLE_FILE_EXT = "sample";

		public const string CLASSIF_STORAGE_FOLDER_NAME = ".User Classification Files";

		// shortcut: FileLocationSupport.ClassifFileLocation + "\\"
		/// <summary>
		/// the location for all of the user classification files
		/// </summary>
		public static string ClassifFileLocation => $"{UserFileRootLocation}\\{CLASSIF_STORAGE_FOLDER_NAME}";

		/// <summary>
		/// the path for the root folder of all of the user classification files<br/>
		/// equals: "C:\ProgramData\{app name}\Andy\User Classification Files  (SM7.5)<br/>
		/// or equals: "C:\ProgramData\{app name}\Andy\{app name}\User Classification Files  (pre SM7.5)<br/>
		/// </summary>
		public static FilePath<FileNameSimple> AllClassifFolderPath => new FilePath<FileNameSimple>(ClassifFileLocation);
		
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
		public static FilePath<FileNameSimple> UserClassifFolderPath => new FilePath<FileNameSimple>(ClassifFileLocationUser);
		
		// shortcut: FileLocationSupport.ClassifSampleFileLocationUser
		/// <summary>
		/// the location for the user's classification file's sample folder
		/// </summary>
		public static string ClassifSampleFileLocationUser => $"{ClassifFileLocationUser}\\{SAMPLE_FOLDER}";

		/// <summary>
		/// the location for all of the default classification files, if any (management provided)
		/// will also contain some examples
		/// </summary>
		public static string ClassifFileLocationDefault => $"{ClassifFileLocation}\\{DEFAULT_FOLDER_NAME}";
		
		/// <summary>
		/// the file path to a user's classification file
		/// </summary>
		public static string ClassifFilePathUser(string fileId)
		{
			return ClassifFileLocationUser + $"\\({Environment.UserName}) {fileId}.{DATA_FILE_EXT}";
		}

		/// <summary>
		/// the file path to a default sheet metrics file
		/// </summary>
		public static string ClassifFilePathDefault(string fileId)
		{
			return ClassifFileLocationDefault + $"\\({DEFAULT_FOLDER_NAME}) {fileId}.{DATA_FILE_EXT}";
		}

	#endregion
	}
}
