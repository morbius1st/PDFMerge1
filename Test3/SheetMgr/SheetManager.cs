#region + Using Directives

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Test3.SheetMgr;

#endregion


// projname: Test3
// itemname: Sheets
// username: jeffs
// created:  12/28/2019 7:13:30 AM


// deals with a couple of sheet aspects
// 0. determine the sheet type
// 1. divide a sheet's id into its parts:
//		phase / building / id / name (or other system)
//		use a tree structure
// 2. divide the sheet id into its parts:
//		A. discipline designator
//		B. primary sequence number
//		C. secondary sequence number (if any)
//		(or some other system)
//		use a tree structure
//	3. depending on 2A / 2B / 2C / etc. 
//		A. apply a sequence code


namespace Test3
{

//	// regardless of any other parts,
//	// this indicates the required
//	// indices for these parts
//	public enum SheetPartIndex
//	{
//		SHEETPHASE = 0,
//		SHEETBLDG = 1,
//		SHEETID = 2,
//		SHEETNAME = 3,
//		ANY
//	}
//
//	public enum SheetIdPart
//	{
//		SHEETNUMBER,
//		SHEETNAME
//	}

	// holds a list of sheets
	public class SheetManager
	{


		public List<Sheet> SheetList { get; private set; }

		public SheetManager()
		{
			SheetList  = new List<Sheet>();
		}

		public static List<SheetPartData> SheetPartsList => SheetSystemManager.SheetPartsManager.SheetPartList;

		public void Add(string sheetId)
		{
			AddToList(GetSheet(sheetId));
		}

		private Sheet GetSheet(string sheetId)
		{
			string si = sheetId.Trim();

			int sheetTypeIndex = SheetSystemManager.SheetTypeManager.SheetTypeIndex(sheetId);

			if (sheetTypeIndex < 0 ) return null;

			return analize(sheetId, sheetTypeIndex);
		}

		private void AddToList(Sheet sheet)
		{
			if (sheet == null) throw new InvalidDataException();

			SheetList.Add(sheet);
		}

		private Sheet analize(string sheetId, int typeIdx)
		{
			bool reference = SheetSystemManager.SheetTypeManager.SheetTypes[typeIdx].IsForRefernce;

			Sheet sheet = new Sheet(sheetId, typeIdx, reference);

			AnalizeForSheetNumber(sheetId, typeIdx, sheet);
			AnalizeForSheetName(sheetId, typeIdx, sheet);

			return sheet;
		}

		private void AnalizeForSheetNumber(string sheetId, int typeIdx, Sheet sheet)
		{
			SheetPartsDescriptor sd = SheetSystemManager.SheetTypeManager.SheetTypes[typeIdx].SheetPartsDescriptor;

			List<SheetPartsDescriptor.PartIndex> partIndexList =
				sd.SheetNumberPartIndicies;

			sheet.AddSheetNumberParts(AnalizeForSheetParts(sheetId, partIndexList));
		}


		private void AnalizeForSheetName (string sheetId, int typeIdx, Sheet sheet)
		{
			SheetPartsDescriptor sd = SheetSystemManager.SheetTypeManager.SheetTypes[typeIdx].SheetPartsDescriptor;

			List<SheetPartsDescriptor.PartIndex> partIndexList = sd.SheetNamePartIndicies;

			sheet.AddSheetNameParts(AnalizeForSheetParts(sheetId, partIndexList));
		}


		private List<Sheet.SheetIdPart> AnalizeForSheetParts(string sheet,
			List<SheetPartsDescriptor.PartIndex> partIndexList)
		{
			List<Sheet.SheetIdPart> SheetParts = new List<Sheet.SheetIdPart>();

			foreach (SheetPartsDescriptor.PartIndex partIndex in partIndexList)
			{
				int sheetPartIndex = partIndex.SheetPartDataIndex;

				SheetPartData spd =
					SheetManager.SheetPartsList[sheetPartIndex];

				if (!spd.IsValid) continue;

				int groupIdx = spd.PartPatternGroup;

				Match match = Regex.Match(sheet, spd.PartPattern,
					RegexOptions.Singleline);

				if (match.Groups.Count <= 0) continue;

				string result = match.Groups[groupIdx].Value.Trim();

				int len = result.Length;

				if (len > spd.PartMaxLen ||
					len < spd.PartMinLen) throw new InvalidDataException();

				Sheet.SheetIdPart idPart =
					new  Sheet.SheetIdPart(result, sheetPartIndex);

				SheetParts.Add(idPart);
			}

			return SheetParts;
		}
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