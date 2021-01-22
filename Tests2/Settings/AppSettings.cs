using System.Collections.Generic;
using System.Runtime.Serialization;
using Tests2.OutlineManager;

// App settings (per user)
//	- applies to a specific app in the suite
//	- holds information specific to the app
//	- located in the user's app data folder / app name / AppSettings


// ReSharper disable once CheckNamespace

namespace SettingsManager
{
#region app data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class AppSettingData : HeaderData
	{
		[IgnoreDataMember]
		public string DataFileVersion => "app 7.4a";

		[IgnoreDataMember]
		public string DataFileDescription => "app setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "app / any notes go here";


		[DataMember(Order = 1)]
		public bool AllowPropertyEditing { get; set; } = false;

		[DataMember]
		public List<OutlineItem> DefaultOutlineItems = new List<OutlineItem>()
		{
			new OutlineItem("00.0", "CS", "Cover Sheet", "Set Cover Sheet" ),
			new OutlineItem("00.1", "T", "Title Sheets", "Title Sheets" ),
			new OutlineItem("00.7", "LS", "Life / Safety", "Life Safety Sheets" ),
			new OutlineItem("07.0", "A", "Architectural", "Architectural Sheets" ),
			new OutlineItem("11.0", "S", "Structural", "Structural Sheets" ),
			new OutlineItem("13.0", "M", "Mechanical", "Mechanical Sheets" ),
			new OutlineItem("15.0", "P", "Plumbing", "Plumbing Sheets" ),
			new OutlineItem("17.0", "E", "Electrical", "Electrical Sheets" ),
		};


	}

#endregion
}