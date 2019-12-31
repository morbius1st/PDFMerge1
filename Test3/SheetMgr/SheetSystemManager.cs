#region + Using Directives

using System.Text.RegularExpressions;

#endregion


// projname: Test3.SheetMgr
// itemname: SheetSystemManager
// username: jeffs
// created:  12/30/2019 4:11:36 PM


namespace Test3.SheetMgr
{
	public enum PartIdType
	{
		SHEETNUMBER,
		SHEETNAME
	}

	public class SheetSystemManager
	{
		public static int MaxSheetParts { get; set; }
		public static int MinSheetParts { get; set; }

		public static SheetTypeManager SheetTypeManager { get; private set; } =
			new SheetTypeManager();

		public static SheetPartsManager SheetPartsManager { get; private set; } =
			new SheetPartsManager();

		private SheetTypeInfo sheetTypeInfo;
		private SheetPartsDescriptor sheetPartsDescriptor;

		public void StartSheetType(string sheetTypeDescription)
		{
			sheetTypeInfo = new SheetTypeInfo(sheetTypeDescription);

			sheetPartsDescriptor = new SheetPartsDescriptor(sheetTypeDescription);
		}

		public void AddSheetPartData(SheetPartData[] sheetParts)
		{
			foreach (SheetPartData sheetPartData in sheetParts)
			{
				addSheetPartData(sheetPartData);
			}
		}

		private void addSheetPartData(SheetPartData sheetPartData)
		{
			int index = SheetPartsManager.Add(sheetPartData);

			SheetPartsDescriptor.PartIndex partIndex;

			partIndex =
				new SheetPartsDescriptor.PartIndex(sheetPartData.PartIdOrder, index);

			sheetPartsDescriptor.AddPartIndex(partIndex, sheetPartData.PartIdType);
		}

		public void CompleteSheetType(
			string sheetTypePattern,
			int disciplineDesignatorGroup,
			bool isForRefernce
			)
		{
			sheetPartsDescriptor.SortIndicies();

			sheetTypeInfo.SheetTypePattern = new Regex(sheetTypePattern,
				RegexOptions.Singleline | RegexOptions.Compiled);

			sheetTypeInfo.DisciplineDesignatorGroup = disciplineDesignatorGroup;

			sheetTypeInfo.IsForRefernce = isForRefernce;

			sheetTypeInfo.SheetPartsDescriptor = sheetPartsDescriptor;

			SheetTypeManager.SheetTypes.Add(sheetTypeInfo);
		}


	}
}