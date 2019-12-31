// Solution:     PDFMerge1
// Project:       Test3
// File:             SheetTypeInfo.cs
// Created:      -- ()

using System.Text.RegularExpressions;

namespace Test3 {

	public class SheetTypeInfo
	{

//		public SheetTypeInfo(string description)
//		{
//			Description = description;
//		}

		public SheetTypeInfo(
			string libraryId,
			string description,
			string sheetTypePattern,
			int disciplineDesignatorGroup,
			bool isForRefernce
			)
		{
			LibraryId = libraryId;
			Description = description;
			SheetTypePattern = new Regex(sheetTypePattern,
				RegexOptions.Singleline | RegexOptions.Compiled);
			DisciplineDesignatorGroup = disciplineDesignatorGroup;
			IsForRefernce = isForRefernce;
		}

		public string LibraryId                          { get; private set; }
		public string Description                        { get; private set; }
		public Regex SheetTypePattern                    { get; private set; }
		public int DisciplineDesignatorGroup             { get; private set; }
		public bool IsForRefernce                        { get; private set; }

		public SheetPartsDescriptor SheetPartsDescriptor { get; set; }

	}
}