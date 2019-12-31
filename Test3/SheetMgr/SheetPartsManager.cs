#region + Using Directives

using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading;
using static Test3.SheetManager;

#endregion


// projname: Test3.SheetMgr
// itemname: SheetPartsManager
// username: jeffs
// created:  12/29/2019 6:27:15 PM


namespace Test3.SheetMgr
{
	public class SheetPartsManager
	{

	#region ctor

		public SheetPartsManager()
		{
			SheetPartDataList = new Dictionary<string, SheetPartData>();
		}

	#endregion

	#region public properties

		public Dictionary<string, SheetPartData> SheetPartDataList { get; private set; }
		public int Count => SheetPartDataList.Count;
		public bool Initialized => (SheetPartDataList?.Count ?? 0) > 0;

	#endregion

	#region public properties

		public void Add(SheetPartData sd)
		{
			SheetPartDataList.Add(sd.LibraryId, sd);
		}

		public string FormatString(string libId)
		{
			return SheetPartDataList[libId].FormatString;
		}

	#endregion

	}
}