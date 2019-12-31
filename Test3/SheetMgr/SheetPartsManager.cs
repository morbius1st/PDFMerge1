#region + Using Directives

using System.Collections.Generic;
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
		// note - never sort, insert, or anything else that changes
		// the order of the elements
		public List<SheetPartData> SheetPartList { get; private set; }

		public SheetPartsManager()
		{
			SheetPartList = new List<SheetPartData>(SheetSystemManager.MaxSheetParts);

			for (int i = 0; i < SheetSystemManager.MinSheetParts; i++)
			{
				SheetPartList.Add(new SheetPartData());
			}
		}

		public int Add(SheetPartData sd)
		{
			SheetPartList.Add(sd);

			return Count - 1;
		}

		public int Count => SheetPartList.Count;

		public bool Initialized => (SheetPartList?.Count ?? 0) > 0;

		public string FormatString(int idx)
		{
			return SheetPartList[idx].FormatString;
		}

		public bool HasPart(int idx)
		{
			return SheetPartList[idx] != null;
		}

	}
}