#region using directives
using System.Collections.Generic;
using System.Runtime.Serialization;
using SettingsManager;
using SheetEditor.SheetData;

#endregion

// in code, after creating the data file for the first time, set the
// header values for 
// {dataset}.Info.Description
// {dataset}.Info.DataClassVersion
// {dataset}.Info.Notes
// the meaning of all three are up to you, however, the dataclass version
// is used to determine if the dataset has been revised and needs an upgrade



namespace Settings
{
#region data class

	[DataContract(Namespace = "")]
	public class SheetDataSet : IDataFile
	{
		[IgnoreDataMember]
		public static string DataFileName { get; } = "SheetData.xml";

		[IgnoreDataMember]
		public string DataFileDescription { get; set; } = "Sheet Data Information";

		[IgnoreDataMember]
		public string DataFileNotes { get; set; } = "Sheet Data is user specific / created";

		[IgnoreDataMember]
		public string DataFileVersion { get; set; } = "v1.2x";

		[DataMember(Order = 10)]
		public Dictionary<string, SheetData> SheetDataList { get; set; }= new Dictionary<string, SheetData>();

	}
#endregion
}
