using System.Collections.Generic;
using Test3.SheetMgr; 
 
// Solution:     PDFMerge1
// Project:      Test3
// File:         Sheet.cs
// Created:      -- ()

namespace Test3
{
	public class Sheet
	{

	#region preface

		public class SheetIdPart
		{
			public string SheetidPartData { get; private set; }
			public string SheetPartLibId { get; private set; }

			public SheetIdPart(string sheetidPartData, string sheetPartLibId)
			{
				SheetidPartData = sheetidPartData;
				SheetPartLibId = sheetPartLibId;
			}

			private SheetIdPart() { }
		}

	#endregion

	#region ctor

		public Sheet(string sheet, int sheetTypeIdx,
			bool isForReference)
		{
			SheetId = sheet;
			SheetTypeIdx = sheetTypeIdx;
			IsForRefernce = isForReference;

			SheetIdPartsSheetNumber = new List<SheetIdPart>();
			SheetIdPartsSheetName = new List<SheetIdPart>();
		}


	#endregion

	#region public properties

		public string SheetId { get; private set; }
		public int SheetTypeIdx { get; private set; }
		public List<SheetIdPart> SheetIdPartsSheetNumber { get; private set; }
		public List<SheetIdPart> SheetIdPartsSheetName { get; private set; }
		public bool IsForRefernce { get; set; }
		public string SheetName => sheetName();
		public string SheetNumber => sheetNumber();

	#endregion

	#region public methods

		public string SheetPartDescription(string libKey)
		{
			return SheetSystemManager.SheetPartsManager
			.SheetPartDataList[libKey].PartDescription;
		}

		public void AddSheetNumberParts(List<SheetIdPart> sheetIdPartsSheetNumber)
		{
			SheetIdPartsSheetNumber = sheetIdPartsSheetNumber;
		}

		public void AddSheetNameParts(List<SheetIdPart> sheetIdPartsSheetName)
		{
			SheetIdPartsSheetName = sheetIdPartsSheetName;
		}

	#endregion

	#region private methods

		private string sheetNumber()
		{
			string sheetNumber = "";

			foreach (SheetIdPart sheetIdPart in SheetIdPartsSheetNumber)
			{
				string format =
					SheetSystemManager.SheetPartsManager.FormatString(
						sheetIdPart.SheetPartLibId);

				sheetNumber += string.Format(format, sheetIdPart.SheetidPartData);
			}

			return sheetNumber;
		}

		private string sheetName()
		{
			string sheetName = "";

			foreach (SheetIdPart sheetIdPart in SheetIdPartsSheetName)
			{
				string format =
					SheetSystemManager.SheetPartsManager.FormatString(
						sheetIdPart.SheetPartLibId);

				sheetName += string.Format(format, sheetIdPart.SheetidPartData);
			}

			return sheetName;
		}

	#endregion

	#region system overrieds

		public override string ToString()
		{
			return sheetNumber() + SheetSystemManager.SheetNumberNameSeperator + sheetName();
		}

	#endregion
	}
}