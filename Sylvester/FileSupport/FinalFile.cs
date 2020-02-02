#region + Using Directives

#endregion


// projname: Sylvester.FileSupport
// itemname: FinalFile
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

	// got status
	// got match status


	public class FinalFile : SheetNameInfo
	{
		public static string StatusMsgMatches { get; } = "OK As Is";
		public static string StatusMsgNotMatches { get; } = "Rename";
		public static string StatusMsgMissing { get; } = "Ignore";

		private string matchedSheetNumber = "";
		private string matchedSeparator = "";
		private string matchedSheetTitle = "";
		private BaseFile baseFile;

		public FinalFile() { }

		public FinalFile(Route fullFileRoute)
		{
			FullFileRoute = fullFileRoute;
		}

		public override string SheetTitle
		{
			get => sheetTitle;
			set => sheetTitle = value;
		}

		public BaseFile BaseFile
		{
			get => baseFile;
			set
			{
				baseFile = value;
				OnPropertyChange();

				matchedSheetNumber = baseFile.SheetNumber;
				matchedSeparator = baseFile.Separator;
				matchedSheetTitle = baseFile.SheetTitle;

				UpdateSelectStatus();
			}
		}

		public bool FileNew => SheetTitleMatches == MatchStatus.NEW_FILE &&
			SeparationMatches == MatchStatus.NEW_FILE && SheetTitleMatches == MatchStatus.NEW_FILE;

		public bool FileMatches => SheetNumberMatches == MatchStatus.DOES_MATCH &&
			SeparationMatches == MatchStatus.DOES_MATCH && SheetTitleMatches == MatchStatus.DOES_MATCH;

		public string StatusMessage
		{
			get
			{
				if (FileNew) return StatusMsgMissing;
				if (FileMatches) return StatusMsgMatches;
				return StatusMsgNotMatches;
			}
		}

		public string NewFileName
		{
			get => matchedSheetNumber + matchedSeparator
				+ MatchedSheetTitle + SheetNameInfo.FILE_TYPE_EXT;
		}

		public string MatchedSheetNumber => matchedSheetNumber;

		public string MatchedSeparator => matchedSeparator;

		public string MatchedSheetTitle
		{
			get => baseFile?.SheetTitle ?? "no sheet title";

			set
			{
				matchedSheetTitle = value;
				OnPropertyChange();
			}
		}

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

		public MatchStatus SeparationMatches
		{
			get
			{
				if (matchedSeparator.Length == 0) return MatchStatus.NEW_FILE;

				if (separator.Equals(baseFile?.Separator ?? separator))
				{
					return MatchStatus.DOES_MATCH;
				}

				return MatchStatus.DOES_NOT_MATCH;
			}
		}

		public MatchStatus SheetTitleMatches
		{
			get
			{
				if (matchedSheetTitle.Length == 0) return MatchStatus.NEW_FILE;

				if (sheetTitle.Equals(baseFile?.SheetTitle ?? sheetTitle))
				{
					return MatchStatus.DOES_MATCH;
				}

				return MatchStatus.DOES_NOT_MATCH;
			}
		}

		public override void UpdateSelectStatus()
		{
			Selected = StatusMessage.Equals(StatusMsgNotMatches);
		}
	}
}