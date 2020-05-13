#region using directives

using System.Collections.ObjectModel;
using System.Runtime.Serialization;

#endregion

// projname: $projectname$
// itemname: TestDataData2
// username: jeffs
// created:  4/29/2020 10:35:13 PM

namespace StoreAndRead.TestData.Data2
{
#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "TestDataData2", Namespace = "")]
	public class TestDataData2
	{
		[DataMember(Order = 1)]
		public string Description { get; private set; } = "This is TestDataData2";

		[DataMember(Order = 2)]
		public ObservableCollection<TestItem> TestDataRoot { get; set; }
	}

#endregion
}