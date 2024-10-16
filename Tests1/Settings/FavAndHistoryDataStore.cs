#region using directives
using System.Runtime.Serialization;
using SettingsManager;
using Tests1.FaveHistoryMgr;
using UtilityLibrary;

#endregion

// in code, after creating the data file for the first time, set the
// header values for 
// {dataset}.Info.Description
// {dataset}.Info.DataClassVersion
// {dataset}.Info.Notes
// the meaning of all three are up to you, however, the dataclass version
// is used to determine if the dataset has been revised and needs an upgrade

namespace SettingsManager
{

#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class FavAndHistoryDataStore : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "data 7.4d";

		[IgnoreDataMember]
		public string DataFileDescription => "data setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "data / any notes go here";

		[DataMember(Order = 1)]
		public string FavAndHistoryDescription { get; set; } = "this is a description for the Fav and History data file";

		[DataMember(Order = 2)]
		public ObservableDictionary<FileListKey, FilePath<FileNameSimple>> ClassfFileList { get; set; }

		[DataMember(Order = 3)]
		public ObservableDictionary<FileListKey, FilePath<FileNameSimple>> SampleFileList { get; set; }



		[DataMember(Order = 4)]
		public ObservableDictionary<UserListKey, UserFavClassfListValue> UserFavClassfList { get; set; }

		[DataMember(Order = 5)]
		public ObservableDictionary<UserListKey, UserFavPairListValue> UserFavPairList { get; set; }



		[DataMember(Order = 6)]
		public ObservableDictionary<UserListKey, UserHistClassfListValue> UserHistClassfList { get; set; }

		[DataMember(Order = 7)]
		public ObservableDictionary<UserListKey, UserHistPairListValue> UserHistPairList { get; set; }

	}

#endregion
}
