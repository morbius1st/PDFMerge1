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

	#region private fields

		private SheetTypeInfo sheetTypeInfo;
		private SheetPartsDescriptor sheetPartsDescriptor;

	#endregion

	#region public properties

		public static int MaxSheetParts { get; set; }
		public static int MinSheetParts { get; set; }

		public static SheetTypeManager SheetTypeManager { get; private set; } =
			new SheetTypeManager();

		public static SheetPartsManager SheetPartsManager { get; private set; } =
			new SheetPartsManager();

	#endregion

	#region public methods

		public void ConfigSheetType(
			string sheetTypeLibraryId,
			string sheetTypeDescription,
			string sheetTypePattern,
			int disciplineDesignatorGroup,
			bool isForRefernce,
			string[] libKeys
			)
		{
			startSheetType(
			sheetTypeLibraryId,
			sheetTypeDescription, 
			sheetTypePattern, 
			disciplineDesignatorGroup, 
			isForRefernce);

			AddSheetPartLibKeys(libKeys);
			CompleteSheetType();
		}

		public void addSheetPartData(SheetPartData sheetPartData)
		{
			SheetPartsManager.Add(sheetPartData);
		}

		private void AddSheetPartLibKeys(string[] libKeys)
		{

			foreach (string libKey in libKeys)
			{
				SheetPartData spd = SheetPartsManager.SheetPartDataList[libKey];

				SheetPartsDescriptor.PartIndex partIndex = 
					new SheetPartsDescriptor.PartIndex(spd.PartIdOrder, libKey);

				sheetPartsDescriptor.AddPartIndex(partIndex, spd.PartIdType);
			}
		}

	#endregion

	#region private methods

		private void startSheetType(
			string sheetTypeLibraryId,
			string sheetTypeDescription,
			string sheetTypePattern,
			int disciplineDesignatorGroup,
			bool isForRefernce
			)
		{
			sheetTypeInfo = new SheetTypeInfo(			
				sheetTypeLibraryId,
				sheetTypeDescription,
				sheetTypePattern,
				disciplineDesignatorGroup,
				isForRefernce);

			sheetPartsDescriptor = new SheetPartsDescriptor(sheetTypeDescription);
		}

		public void AddSheetPartData(SheetPartData[] sheetParts)
		{
			foreach (SheetPartData sheetPartData in sheetParts)
			{
				addSheetPartData(sheetPartData);
			}
		}

		private void CompleteSheetType(
//			string sheetTypePattern,
//			int disciplineDesignatorGroup,
//			bool isForRefernce
			)
		{
			sheetPartsDescriptor.SortIndicies();

//			sheetTypeInfo.SheetTypePattern = new Regex(sheetTypePattern,
//				RegexOptions.Singleline | RegexOptions.Compiled);
//
//			sheetTypeInfo.DisciplineDesignatorGroup = disciplineDesignatorGroup;
//
//			sheetTypeInfo.IsForRefernce = isForRefernce;

			sheetTypeInfo.SheetPartsDescriptor = sheetPartsDescriptor;

			SheetTypeManager.SheetTypes.Add(sheetTypeInfo);
		}

	#endregion

	}
}

// system breakdown
/*
                                SheetPart
     +-> SheetId			 |> yes
    ----------------		 |
     +-> SheetNumber		 |> no
	--------				 |
	+-> PhaseId				 |> yes
    |+-> BuildingId			 |> yes
	||  +-> SheetId			 |> yes
	||  |      +-> SheetName |> no
	-- ------ -------
	1A A1.0-1 ShtName 
						-- length --				
			desc		min		max					pattern
	1		Phase		1		1					^(\d*)
	A		Building	1		1					^\d*([A-Z]*)
	ShtName	Sheet Name	2		100					[A A1.0...]: ^.*? .*? (.*)
													[A1...]: ^.*? (.*)
	A1.0-1	Sheet Id	2		20  (LT10.10-10.10)	[A A1.0...]: ^.*? (.*?) 
													[A1...]: ^(.*?) 

	NON - phase / building / phase-building sheet number pattern:
	^(([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)
	full match: the whole line worked
	group 0: sheet number
	group 1: discipline designator
	group 2: n/a - does not matter
	group 3: sheet name - does not matter

	phase / building / phase-building sheet number pattern:
	^(([A-Z1-9]+) ([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)
	full match: the whole line worked
	group 0: full sheet number (incl phase &/or building)
	group 1: phase &/or building
	group 2: discipline designator
	group 3: n/a - does not matter
	group 4: sheet name - does not matter



 */