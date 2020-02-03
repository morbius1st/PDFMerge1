using System;


// Solution:     PDFMerge1
// Project:       Sylvester
// File:             FileRevision.cs
// Created:      -- ()



namespace Sylvester.FileSupport {
	public class FileRevision : SheetNameInfo, ICloneable
	{
		
		public object Clone()
		{
			return this.Clone<FileCurrent>();
		}

		public override string SheetTitle
		{
			get => sheetTitle;
			set => sheetTitle = value;
		}

		public void UpdateSheetTitleCase()
		{
			SheetTitle = sheetTitle;
		}

		public override void UpdateSelectStatus() { }
	}
}