using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using DisciplineEditor.Support;
using UtilityLibrary;

// User settings (per user) 
//  - user's settings for a specific app
//	- located in the user's app data folder / app name


// ReSharper disable once CheckNamespace


namespace SettingsManager
{
#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class UserSettingDataFile : IDataFile
	{
		public const string  DATAFILE_EXTN = "xml";

		// private FilePath<FileNameSimple> disciplineDataFilePath;
		private string disciplineDataFileFolder;
		private string disciplineDataFileName;

		[IgnoreDataMember]
		public string DataFileVersion => "user 7.4u";

		[IgnoreDataMember]
		public string DataFileDescription => "user setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "user / any notes go here";


		// [DataMember(Order = 1)]
		// public int UserSettingsValue { get; set; } = 7;

		// required by other classes
		[DataMember(Order = 1)]
		public string LastClassificationFileId { get; set; } = "PdfSample 1";

		/// <summary>
		/// the discipline data file's filename - no extension
		/// </summary>
		[DataMember(Order = 2)]
		public string DisciplineDataFileName
		{
			get => disciplineDataFileName;
			set
			{
				disciplineDataFileName = value;

				// if (!disciplineDataFileFolder.IsVoid())
				// {
				// 	makeDisciplineFilePath();
				// }
			}
		} // = @"Disciplines-01.xml";

		/// <summary>
		/// the folder path to the discipline data file
		/// </summary>
		[DataMember(Order = 3)]
		public string DisciplineDataFileFolder
		{
			get => disciplineDataFileFolder;
			set
			{
				disciplineDataFileFolder = value;

				// if (!disciplineDataFileName.IsVoid())
				// {
				// 	makeDisciplineFilePath();
				// }
			}
		} // = @"C:\Users\jeffs\Documents\Programming\VisualStudioProjects\PDF SOLUTIONS\PDFMerge1\.configfilestemp\";

		[DataMember(Order = 4)]
		public ObservableCollection<RecentItem> RecentList { get; set; } = new ObservableCollection<RecentItem>();

	}

#endregion

	[DataContract]
	public struct RecentItem
	{
		[DataMember(Order = 0)]
		public string FileNameNoExt { get; set; }

		[DataMember(Order = 1)]
		public string FolderPath { get; set; }

		public RecentItem(string folderPath, string fileNameNoExt)
		{
			FolderPath = folderPath;
			FileNameNoExt = fileNameNoExt;
			Index = 0;
		}

		[IgnoreDataMember]
		public string FileNameEllipsised =>
			CsStringUtil.EllipsisifyString(
				FileNameNoExt ?? "null", CsStringUtil.JustifyHoriz.CENTER, 25);

		[IgnoreDataMember]
		public string ToolTip => FolderPath;

		[IgnoreDataMember]
		public int Index { 
			get; 
			set; }

		[IgnoreDataMember]
		public string filePathNoExt => $"{FolderPath}\\{FileNameNoExt}";

	}




//
// #region info class
//
// 	[DataContract(Name = "UserSettings", Namespace = "")]
// 	public class UserSettingInfo<T> : UserSettingInfoBase<T>
// 		where T : new()
// 	{
// 		public UserSettingInfo()
// 		{
// 			// these are specific to this data file
// 			DataClassVersion = "user 7.2u";
// 			Description = "user setting file for SettingsManager v7.2";
// 			Notes = "any notes go here";
// 		}
//
// 		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
// 	}
//
// #endregion
//
// #region user data class
//
// 	// this is the actual data set saved to the user's configuration file
// 	// this is unique for each program
// 	[DataContract(Namespace = "")]
// 	public class UserSettingData //: IDataFile
// 	{
// 		public const string  DATAFILE_EXTN = "xml";
//
// 		// private FilePath<FileNameSimple> disciplineDataFilePath;
// 		private string disciplineDataFileFolder;
// 		private string disciplineDataFileName;
//
// 		// [DataMember(Order = 1)]
// 		// public int UserSettingsValue { get; set; } = 7;
//
// 		// required by other classes
// 		[DataMember(Order = 1)]
// 		public string LastClassificationFileId { get; set; } = "PdfSample 1";
//
// 		/// <summary>
// 		/// the discipline data file's filename - no extension
// 		/// </summary>
// 		[DataMember(Order = 2)]
// 		public string DisciplineDataFileName
// 		{
// 			get => disciplineDataFileName;
// 			set
// 			{
// 				disciplineDataFileName = value;
//
// 				// if (!disciplineDataFileFolder.IsVoid())
// 				// {
// 				// 	makeDisciplineFilePath();
// 				// }
// 			}
// 		} // = @"Disciplines-01.xml";
//
// 		/// <summary>
// 		/// the folder path to the discipline data file
// 		/// </summary>
// 		[DataMember(Order = 3)]
// 		public string DisciplineDataFileFolder
// 		{
// 			get => disciplineDataFileFolder;
// 			set
// 			{
// 				disciplineDataFileFolder = value;
//
// 				// if (!disciplineDataFileName.IsVoid())
// 				// {
// 				// 	makeDisciplineFilePath();
// 				// }
// 			}
// 		} // = @"C:\Users\jeffs\Documents\Programming\VisualStudioProjects\PDF SOLUTIONS\PDFMerge1\.configfilestemp\";
//
// 		[DataMember(Order = 4)]
// 		public ObservableCollection<RecentItem> RecentList { get; set; } = new ObservableCollection<RecentItem>();
//

/*
		[IgnoreDataMember]
		public FilePath<FileNameSimple> DisciplineDataFilePath
		{
			get => disciplineDataFilePath;
			set
			{
				if (value == null) return;
				disciplineDataFilePath = value;

				disciplineDataFileFolder = disciplineDataFilePath.FolderPath;
				disciplineDataFileName = disciplineDataFilePath.FileNameNoExt;
			}
		}

		[IgnoreDataMember]
		public bool DisciplineFileExists => disciplineDataFilePath?.Exists ?? false;

		private void makeDisciplineFilePath()
		{
			disciplineDataFilePath = new FilePath<FileNameSimple>(
				$"{disciplineDataFileFolder}\\{disciplineDataFileName}.{DATAFILE_EXTN}");
		}
*/
// 	}
//
// #endregion


}

// USER_SETTINGS, APP_SETTINGS, SUITE_SETTINGS, MACH_SETTINGS, SITE_SETTINGS