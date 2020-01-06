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

	public enum MatchStatus
	{
		DOES_NOT_MATCH,
		DOES_MATCH,
		NEW_FILE
	}

	public class TestFile : SheetId
	{
		private const string SPACER = " ";

		public int paddingShtNum = 0;
		public int paddingSep = 0;
		public int paddingShtName = 0;

		private bool apply = true;

		private string matchedSheetNumber = "";
		private string matchedSeparator = "";
		private string matchedSheetName = "";
		private SheetIdBase baseFile;

		public TestFile(Route fullFileRoute)
		{
			FullFileRoute = fullFileRoute;

			SetPadding();
		}

		public bool Apply
		{
			get
			{
				if (FileNew || FileMatches) return false;

				return apply;
			}
			set
			{
				apply = value;
				OnPropertyChange();
			}
		}

		public bool FileNew => SheetNameMatches == MatchStatus.NEW_FILE &&
			SeparationMatches == MatchStatus.NEW_FILE && SheetNameMatches == MatchStatus.NEW_FILE;

		public bool FileMatches => SheetNumberMatches == MatchStatus.DOES_MATCH &&
			SeparationMatches == MatchStatus.DOES_MATCH && SheetNameMatches == MatchStatus.DOES_MATCH;

		public SheetIdBase BaseFile
		{
			get => baseFile;
			set
			{
				baseFile = value;
				OnPropertyChange();

				matchedSheetNumber = baseFile.SheetNumber;
				matchedSeparator = baseFile.Separator;
				matchedSheetName = baseFile.SheetName;

				SetPadding();
			}
		}

		public string MatchedSheetNumber => SPACER.Repeat(paddingShtNum) + matchedSheetNumber;

		public string MatchedSeparator => matchedSeparator  + SPACER.Repeat(paddingSep);

		public string MatchedSheetName => matchedSheetName + SPACER.Repeat(paddingShtName);

		public MatchStatus SheetNumberMatches
		{
			get
			{
				if (matchedSheetNumber.Length == 0) return MatchStatus.NEW_FILE;

				if (SheetNumber.Equals(baseFile?.SheetNumber ?? SheetNumber))
				{
					return MatchStatus.DOES_MATCH;
				}

				return MatchStatus.DOES_NOT_MATCH;
			}
		}

		public MatchStatus SeparationMatches {

		get {
			if (matchedSeparator.Length == 0) return MatchStatus.NEW_FILE;

			if (separator.Equals(baseFile?.Separator ?? separator))
			{
				return MatchStatus.DOES_MATCH;
			}

			return MatchStatus.DOES_NOT_MATCH;
		}
	}

		public MatchStatus SheetNameMatches {

			get
			{
				if (matchedSheetName.Length == 0) return MatchStatus.NEW_FILE;

				if (sheetName.Equals(baseFile?.SheetName ?? sheetName))
				{
					return MatchStatus.DOES_MATCH;
				}

				return MatchStatus.DOES_NOT_MATCH;
			}
	}

		private int GetPadding(string basePart, string testPart)
		{
			if (basePart.Length == 0) return  testPart.Length;

			if (basePart.Length > testPart.Length) return 0;

			return testPart.Length - basePart.Length + 1;
		}

		private void SetPadding()
		{
			paddingShtNum =
				GetPadding(matchedSheetNumber, SheetNumber);

			paddingSep =
				GetPadding(matchedSeparator, separator);

			paddingShtName =
				GetPadding(matchedSheetName, sheetName);
		}
	}

}