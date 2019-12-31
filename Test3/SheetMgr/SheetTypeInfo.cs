// Solution:     PDFMerge1
// Project:       Test3
// File:             SheetTypeInfo.cs
// Created:      -- ()

using System.Text.RegularExpressions;

namespace Test3 {

	public class SheetTypeInfo
	{
		public string Description                        { get; set; }
		public Regex SheetTypePattern                    { get; set; }
		public int DisciplineDesignatorGroup             { get; set; }
		public bool IsForRefernce                        { get; set; }
		public SheetPartsDescriptor SheetPartsDescriptor { get; set; }

		public SheetTypeInfo(string description)
		{
			Description = description;
		}

		public SheetTypeInfo(string description,
			string sheetTypePattern,
			int disciplineDesignatorGroup,
			bool isForRefernce,
			SheetPartsDescriptor sheetPartsDescriptor
			)
		{
			Description = description;
			SheetTypePattern = new Regex(sheetTypePattern,
				RegexOptions.Singleline | RegexOptions.Compiled);
			DisciplineDesignatorGroup = disciplineDesignatorGroup;
			SheetPartsDescriptor = sheetPartsDescriptor;
			IsForRefernce = isForRefernce;

		}
	}
}