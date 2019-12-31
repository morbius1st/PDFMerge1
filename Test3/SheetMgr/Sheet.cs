// Solution:     PDFMerge1
// Project:       Test3
// File:             Sheet.cs
// Created:      -- ()

using System;
using System.Collections.Generic;
using Test3.SheetMgr;

namespace Test3
{
	public class Sheet
	{
		public class SheetIdPart
		{
			public string SheetidPartData { get; private set; }
			public int SheetPartIndex { get; private set; }

			public SheetIdPart(string sheetidPartData, int sheetPartIndex)
			{
				SheetidPartData = sheetidPartData;
				SheetPartIndex = sheetPartIndex;
			}

			private SheetIdPart() { }
		}

		public string WholeId { get; private set; }
		public int SheetTypeIdx { get; private set; }
		public List<SheetIdPart> SheetIdPartsSheetNumber { get; private set; }
		public List<SheetIdPart> SheetIdPartsSheetName{ get; private set; }
		public bool IsForRefernce { get; set; }

		public Sheet(string sheet, int sheetTypeIdx,
			bool isForReference)
		{
			WholeId = sheet;
			SheetTypeIdx = sheetTypeIdx;
			IsForRefernce = isForReference;

			SheetIdPartsSheetNumber = new List<SheetIdPart>();
			SheetIdPartsSheetName = new List<SheetIdPart>();
		}

		public string SheetName => sheetName();
		public string SheetNumber => sheetNumber();

		public void AddSheetNumberParts(List<SheetIdPart> sheetIdPartsSheetNumber)
		{
			SheetIdPartsSheetNumber = sheetIdPartsSheetNumber;
		}

		public void AddSheetNameParts(List<SheetIdPart> sheetIdPartsSheetName)
		{
			SheetIdPartsSheetName = sheetIdPartsSheetName;
		}

		private string sheetNumber()
		{
			string sheetNumber = "";

			foreach (SheetIdPart sheetIdPart in SheetIdPartsSheetNumber)
			{
				string format =
					SheetSystemManager.SheetPartsManager.FormatString(
						sheetIdPart.SheetPartIndex);

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
						sheetIdPart.SheetPartIndex);

				sheetName += string.Format(format, sheetIdPart.SheetidPartData);
			}

			return sheetName;
		}
	}
}