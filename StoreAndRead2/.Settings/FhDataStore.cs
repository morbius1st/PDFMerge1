#region using directives
using System.Runtime.Serialization;
using SettingsManager;
using StoreAndRead2.FavHistoryAdmin;
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
	public class FhDataStore
	{
		public FhDataStore()
		{
			Favorites = new FhAdministrator<FhClassfValue>();
			FavoritePairs = new FhAdministrator<FhPairValue>();
			History = new FhAdministrator<FhClassfValue>();
			HistoryePairs = new FhAdministrator<FhPairValue>();
		}

		[DataMember(Order = 1)]
		public string FavAndHistoryDescription { get; set; } = "this is a description for the Fav and History data file";

		[DataMember(Order = 2)]
		public FhAdministrator<FhClassfValue> Favorites { get; set; }

		[DataMember(Order = 4)]
		public FhAdministrator<FhPairValue> FavoritePairs { get; set; }

		[DataMember(Order = 6)]
		public FhAdministrator<FhClassfValue> History { get; set; }

		[DataMember(Order = 8)]
		public FhAdministrator<FhPairValue> HistoryePairs { get; set; }


	}

#endregion
}
