#region + Using Directives

using UtilityLibrary;

#endregion

// user name: jeffs
// created:   9/2/2020 6:01:46 AM

namespace AndyShared.FileSupport
{
	public static class FilePathConstants
	{
		public const string USER_STORAGE_FOLDER_NAME = @"User Classification Files";

		public const string USER_STORAGE_PATTERN = @"*.xml";
		public const string USER_STORAGE_FOLDER = FilePathUtil.PATH_SEPARATOR + USER_STORAGE_FOLDER_NAME;

		public const string CLASSF_FILE_EXT_NO_SEP = "xml";

		public const string SAMPLE_FILE_EXT = "sample";
		public const string SAMPLE_FOLDER = "Sample Files";
		public const string SORT_NAME_PROP = "SortName";
	}
}
