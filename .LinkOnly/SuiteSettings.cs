using System.Runtime.Serialization;


// projname: SettingsManagerV40
// itemname: AppSettingInfo70
// username: jeffs
// Created:      -- ()

// USER_SETTINGS; APP_SETTINGS; SUITE_SETTINGS; MACH_SETTINGS; SITE_SETTINGS

namespace SettingsManager
{
	#region suite data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SuiteSettingDataFile : IDataFile
	{
		// start - this is default data used when the file is first created and saved
		[IgnoreDataMember]
		public string DataFileVersion => "0.1";
		[IgnoreDataMember]
		public string DataFileDescription => "Suite Setting Information for Andy";
		[IgnoreDataMember]
		public string DataFileNotes => "Work in progress";
		// end
		
		// don't know how this is used yet.
		[DataMember(Order = 1)]
		public string SiteRootPath { get; set; }
		//			= @"D:\Users\Jeff\OneDrive\Prior Folders\Office Stuff\CAD\Copy Y Drive & Office Standards\AppData" ;

		// this is the folder that all discipline data files are stored
		// for both users and company
		[DataMember(Order = 2)]
		public string DisciplineDataFileFolder { get; set; }
			= @"C:\Users\jeffs\Documents\Programming\VisualStudioProjects\PDF SOLUTIONS\PDFMerge1\.configfilestemp";
	}

	#endregion
}