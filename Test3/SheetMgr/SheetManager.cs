#region + Using Directives

using System;
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
	// holds a list of sheets
	public class SheetManager
	{

	#region ctor

		public SheetManager()
		{
			SheetList  = new List<Sheet>();
		}

	#endregion

	#region public properties

		public List<Sheet> SheetList { get; private set; }

	#endregion

	#region public methods

		public bool Add(string sheetId)
		{
			return addToList(getSheet(sheetId));
		}

	#endregion

	#region private methods

		private Sheet getSheet(string sheetId)
		{
			string si = sheetId.Trim();

			int sheetTypeIndex =
				SheetSystemManager.SheetTypeManager.SheetTypeIndex(sheetId);

			if (sheetTypeIndex < 0 ) return null;

			return parseSheetId(sheetId, sheetTypeIndex);
		}

		private bool addToList(Sheet sheet)
		{
			if (sheet == null) return false;

			SheetList.Add(sheet);

			return true;
		}

		private Sheet parseSheetId(string sheetId, int typeIdx)
		{
			bool reference = SheetSystemManager.SheetTypeManager.SheetTypes[typeIdx]
			.IsForRefernce;

			Sheet sheet = new Sheet(sheetId, typeIdx, reference);

			SheetPartsDescriptor sd = SheetSystemManager.SheetTypeManager
			.SheetTypes[typeIdx].SheetPartsDescriptor;

			parseSheetIdForSheetNumber(sheetId, typeIdx, sheet, sd);
			parseSheetIdForSheetName(sheetId, typeIdx, sheet, sd);

			return sheet;
		}

		private void parseSheetIdForSheetNumber(string sheetId, int typeIdx, Sheet sheet,
			SheetPartsDescriptor sd)
		{
			List<SheetPartsDescriptor.PartIndex> partIndexList =
				sd.SheetNumberPartIndicies;

			sheet.AddSheetNumberParts(parseSheetIdForSheetParts(sheetId, partIndexList));
		}

		private void parseSheetIdForSheetName (string sheetId, int typeIdx, Sheet sheet,
			SheetPartsDescriptor sd)
		{
			List<SheetPartsDescriptor.PartIndex> partIndexList = sd.SheetNamePartIndicies;

			sheet.AddSheetNameParts(parseSheetIdForSheetParts(sheetId, partIndexList));
		}

		private List<Sheet.SheetIdPart> parseSheetIdForSheetParts(string sheetId,
			List<SheetPartsDescriptor.PartIndex> partIndexList)
		{
			List<Sheet.SheetIdPart> SheetParts = new List<Sheet.SheetIdPart>();

			foreach (SheetPartsDescriptor.PartIndex partIndex in partIndexList)
			{
				string sheetPartLibId = partIndex.LibraryId;

				SheetPartData spd =
					SheetSystemManager.SheetPartsManager.SheetPartDataList[sheetPartLibId];

				if (!spd.IsValid) continue;

				int groupIdx = spd.PartPatternGroup;

				Match match = Regex.Match(sheetId, spd.PartPattern,
					RegexOptions.Singleline);

				if (match.Groups.Count < groupIdx) continue;

				string result = match.Groups[groupIdx].Value.Trim();

				int len = result.Length;

				if (!validateSheetPart(result, spd)) continue;

				Sheet.SheetIdPart idPart =
					new  Sheet.SheetIdPart(result, sheetPartLibId);

				SheetParts.Add(idPart);
			}

			return SheetParts;
		}

		private bool validateSheetPart(string sheetPart, SheetPartData spd)
		{
			int len = sheetPart.Length;

			if (len < spd.PartMinLen) return false;

			if (spd.PartMaxLen > 0) // 0 == unlimited
			{
				if (len > spd.PartMaxLen) return false;
			}

			return true;
		}

	#endregion
	}
}

