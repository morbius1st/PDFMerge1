#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;

#endregion


// projname: Sylvester.FileSupport
// itemname: TestFile
// username: jeffs
// created:  1/4/2020 10:35:42 PM


namespace Sylvester.FileSupport
{
	public class TestFile : SheetId
	{
		private const string SPACER = " ";

		public int paddingShtNum = 0;
		public int paddingSep = 0;
		public int paddingShtName = 0;

		private string matchedSheetNumber;
		private string matchedSeparator;
		private string matchedSheetName;
		private SheetIdBase baseFile;

		public TestFile(Route fullFileRoute)
		{
			FullFileRoute = fullFileRoute;
		}

		public bool HasDiferences => SheetNumberMatches || SeparationMatches || SheetNameMatches;
		public bool MakeChanges { get; set; } = true;

		public bool IsMissing => string.IsNullOrWhiteSpace(MatchedSheetNumber) &&
			string.IsNullOrWhiteSpace(MatchedSeparator) &&
			string.IsNullOrWhiteSpace(MatchedSheetName);

		public SheetIdBase BaseFile
		{
			get => baseFile;
			set
			{
				baseFile = value;
				OnPropertyChange();

				matchedSheetNumber = baseFile.SheetNumber;

				paddingShtNum =
					GetPadding(matchedSheetNumber, SheetNumber);

				matchedSeparator = baseFile.Separator;

				paddingSep =
					GetPadding(matchedSeparator, separator);

				matchedSheetName = baseFile.FileName;

				paddingShtName =
					GetPadding(matchedSheetName, sheetName);

			}
		}

		public string MatchedSheetNumber=> SPACER.Repeat(paddingShtNum) + matchedSheetNumber;

		public string MatchedSeparator => matchedSeparator  + SPACER.Repeat(paddingSep);

		public string MatchedSheetName => matchedSheetName + SPACER.Repeat(paddingShtName);

		public bool SheetNumberMatches => SheetNumber.Equals(baseFile?.SheetNumber ?? SheetNumber);

		public bool SeparationMatches => separator.Equals(baseFile?.Separator ?? separator);

		public bool SheetNameMatches => sheetName.Equals(baseFile?.FileName ?? sheetName);

		private int GetPadding(string basePart, string testPart)
		{
			if (basePart.Length == 0) return  testPart.Length;

			if (basePart.Length > testPart.Length) return 0;

			return testPart.Length - basePart.Length + 1;
		}
	}
}