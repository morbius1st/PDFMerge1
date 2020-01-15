using System;
using Sylvester.Process;
using Sylvester.Settings;
using static Sylvester.Support.Support;



// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SheetIdBase.cs
// Created:      -- ()



namespace Sylvester.FileSupport {
	public class TestFile : SheetNameInfo, ICloneable
	{
		
		public object Clone()
		{
			return this.Clone<BaseFile>();
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