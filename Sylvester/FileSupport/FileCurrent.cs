using System;
using SettingsManager;
using Sylvester.Process;
// using Sylvester.Settings;
using UtilityLibrary;
using static Sylvester.Support.Support;



// Solution:     PDFMerge1
// Project:       Sylvester
// File:             FileCurrent.cs
// Created:      -- ()


	
namespace Sylvester.FileSupport {
	public class FileCurrent : SheetNameInfo, ICloneable
	{
		public new static FolderType FolderType => FolderType.CURRENT;

		public object Clone()
		{
			return this.Clone<FileCurrent>();
		}

		public override int FolderTypeValue => FolderType.Value();

		public override string SheetTitle
		{
			get => sheetTitle ?? "n/a";

			set
			{
				switch (SetgMgr.SheetTitleCase)
				{
					case SheetTitleCase.TO_CAP_EA_WORD:
						sheetTitle = ToCapEachWord(originalSheetTitle);
						break;
					case SheetTitleCase.TO_UPPER_CASE:
						sheetTitle = originalSheetTitle.ToUpper();
						break;
					default:
						sheetTitle = originalSheetTitle;
						break;
				}

				OnPropertyChange();
			}
		}

		public void UpdateSheetTitleCase()
		{
			SheetTitle = sheetTitle;

			OnPropertyChange("SheetName");
		}

		public override void UpdateSelectStatus() { }
	}
}