#region + Using Directives

using System;
using System.Runtime.Serialization;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   9/26/2020 7:29:51 AM

namespace AndyShared.SampleFileSupport
{
	[DataContract(Name = "SampleFile", Namespace = "")]
	public class SampleFileListData
	{
		[DataMember(Order = 1)]
		public Hdr Header { get; set; } = new Hdr();

		[DataMember(Order = 2)]
		public SheetFileData Data { get; set; } = new SheetFileData();


		[DataContract(Namespace = "")]
		public class Hdr
		{
			[DataMember]
			public string Description { get; set; }
		}

		// [DataContract]
		// public class Shts
		// {
		// 	[DataMember]
		// 	public List<string> Sheets { get; set; } = new List<string>();
		// }

		[DataContract(Namespace = "")]
		public class SheetFileData
		{
			[DataMember]
			public string Building { get; set; }

			[DataMember]
			public string Sheets { get; set; }

			[IgnoreDataMember]
			public string[] SheetList { get; set; }

			public bool Parse()
			{
			#if DML1
				DM.Start0();
			#endif

				DM.Stat0("SheetList loaded here");
				SheetList = Sheets?.Split(new [] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);

			#if DML1
				DM.End0();
			#endif

				return (SheetList?.Length ?? 0) > 0;
			}
		}
	}
}
