#region using directives

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SettingsManager;
using Tests2.Support;
using UtilityLibrary;

#endregion

// in code, after creating the data file for the first time, set the
// header values for 
// {dataset}.Info.Description
// {dataset}.Info.DataClassVersion
// {dataset}.Info.Notes
// the meaning of all three are up to you, however, the dataclass version
// is used to determine if the dataset has been revised and needs an upgrade

namespace Tests2.TestData
{
#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	// public class DataSet // : HeaderData
	public class DataSet : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileDescription { get; set; } = "Sample Dataset";

		[IgnoreDataMember]
		public string DataFileNotes { get; set; } = "testing observable dictionary";
		
		[IgnoreDataMember]
		public string DataFileVersion { get; set; } = "1.0";

		[DataMember]
		public ObservableDictionary<string, subDataClass> Od = new ObservableDictionary<string, subDataClass>();

		[DataMember]
		public Dictionary<string, subDataStruct> D = new Dictionary<string, subDataStruct>();


	}

	[DataContract(Namespace = "")]
	public class subDataClass
	{
		[DataMember]
		public string S1 { get; set; }
		[DataMember]
		public double D1 { get; set; }
		[DataMember]
		public int I1 { get; set; }

		public subDataClass(string s, double d, int i)
		{
			S1 = s;
			D1 = d;
			I1 = i;
		}
	}

	[DataContract(Namespace = "")]
	public struct subDataStruct
	{
		[DataMember]
		public string S1 { get; set; }
		[DataMember]
		public double D1 { get; set; }
		[DataMember]
		public int I1 { get; set; }

		public subDataStruct(string s, double d, int i)
		{
			S1 = s;
			D1 = d;
			I1 = i;
		}
	}

#endregion
}