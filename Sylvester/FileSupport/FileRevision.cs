using System;
using Sylvester.Process;
using UtilityLibrary;


// Solution:     PDFMerge1
// Project:       Sylvester
// File:             FileRevision.cs
// Created:      -- ()



namespace Sylvester.FileSupport {
	public class FileRevision : SheetNameInfo, ICloneable
	{
		public new static FolderType FolderType => FolderType.REVISION;
		
		public object Clone()
		{
			return this.Clone<FileCurrent>();
		}

		public override string SheetTitle
		{
			get => sheetTitle;
			set => sheetTitle = value;
		}

		public override int FolderTypeValue => FolderType.Value();

		public void UpdateSheetTitleCase()
		{
			SheetTitle = sheetTitle;
		}

		public override void UpdateSelectStatus() { }
	}
}