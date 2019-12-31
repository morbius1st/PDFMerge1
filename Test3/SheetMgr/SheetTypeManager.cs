// Solution:     PDFMerge1
// Project:       Test3
// File:             SheetAnalyser.cs
// Created:      -- ()

using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Test3
{
	public class SheetTypeManager
	{
		public List<SheetTypeInfo> SheetTypes { get; set; } =
			new List<SheetTypeInfo>();


		public int SheetTypeIndex(string sheet)
		{
			int sheetTypeIdx = -1;

			for (int i = 0; i < SheetTypes.Count; i++)
			{
				Match match = SheetTypes[i].SheetTypePattern.Match(sheet);

				if (match.Success)
				{
					sheetTypeIdx = i;
					break;
				}
			}

			return sheetTypeIdx;
		}


	}
}