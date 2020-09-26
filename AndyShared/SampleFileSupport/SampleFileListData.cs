#region + Using Directives

using System;
using System.Runtime.Serialization;

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
		public Shts Data { get; set; } = new Shts();


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
		public class Shts
		{
			[DataMember]
			public string Sheets { get; set; }

			[IgnoreDataMember]
			public string[] SheetList { get; set; }

			public bool Parse()
			{
				SheetList = Sheets?.Split(new [] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);

				return (SheetList?.Length ?? 0) > 0;
			}
		}
	}
}
