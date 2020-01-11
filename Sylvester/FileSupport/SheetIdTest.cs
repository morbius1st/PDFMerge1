#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;

#endregion


// projname: Sylvester.FileSupport
// itemname: SheetIdTest
// username: jeffs
// created:  1/1/2020 8:03:25 AM


namespace Sylvester.FileSupport
{
	public class SheetIdTest : SheetId
	{
		private const string SPACER = " ";

		public int paddingShtNum = 0;
		public int paddingSep = 0;
		public int paddingShtName = 0;

		private string matchedSheetNumber;
		private string matchedSeparator;
		private string matchedSheetName;

		protected override bool PreSelect { get; set; } = true;

		public bool HasDiferences => SheetNumberMatches || SeparationMatches || SheetNameMatches;
		public bool MakeChanges { get; set; } = true;

		public bool IsMissing => string.IsNullOrWhiteSpace(MatchedSheetNumber) &&
			string.IsNullOrWhiteSpace(MatchedSeparator) &&
			string.IsNullOrWhiteSpace(MatchedSheetName);

		public string MatchedSheetNumber
		{
			get => SPACER.Repeat(paddingShtNum) + matchedSheetNumber;
			set
			{
				matchedSheetNumber = value;

				paddingShtNum =
					GetPadding(matchedSheetNumber, SheetNumber);
			}
		}

		public string MatchedSeparator
		{
			get => matchedSeparator  + SPACER.Repeat(paddingSep);
			set
			{
				matchedSeparator = value;

				paddingSep =
					GetPadding(matchedSeparator, separator);
			}
		}

		public string MatchedSheetName
		{
			get => matchedSheetName + SPACER.Repeat(paddingShtName);
			set
			{
				matchedSheetName = value;

				paddingShtName =
					GetPadding(matchedSheetName, sheetName);
			}
		}

		public bool SheetNumberMatches => SheetNumber.Equals(MatchedSheetNumber);

		public bool SeparationMatches => separator.Equals(MatchedSeparator);

		public bool SheetNameMatches => sheetName.Equals(MatchedSheetName);

		private int GetPadding(string basePart, string testPart)
		{
			if (basePart.Length == 0) return  testPart.Length;

			if (basePart.Length > testPart.Length) return 0;

			return testPart.Length - basePart.Length + 1;
		}

	}
}