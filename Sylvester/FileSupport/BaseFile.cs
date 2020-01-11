// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SheetIdBase.cs
// Created:      -- ()

namespace Sylvester.FileSupport {
	public class BaseFile : SheetId
	{
		protected override bool PreSelect { get; set; } = false;

		public BaseFile() { }
	}
}