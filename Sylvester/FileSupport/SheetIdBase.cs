// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SheetIdBase.cs
// Created:      -- ()

namespace Sylvester.FileSupport {
	public class SheetIdBase : SheetId
	{
		public override string SheetID
		{
			get => sheetID;
			set
			{
				sheetID = value;
				OnPropertyChange();

				SheetNumber = value;

				AdjustedSheetID = AdjustSheetNumber(sheetID);
			}
		}
	}
}