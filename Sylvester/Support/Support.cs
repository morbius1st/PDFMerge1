#region + Using Directives

using System.Text;
using System.Text.RegularExpressions;
using Sylvester.FileSupport;

#endregion


// projname: Sylvester.FileSupport
// itemname: Support
// username: jeffs
// created:  1/4/2020 11:31:35 PM


namespace Sylvester.Support
{
	public class Support
	{
		public static string ToCapEachWord(string phrase)
		{
			if (string.IsNullOrEmpty(phrase)) return "";
			int pos = 0;
			StringBuilder sb = new StringBuilder(phrase.ToLower());

			do
			{
				if (sb[pos] >= 'a' && sb[pos] <= 'z')
				{
					sb[pos] = (char) (sb[pos] + 65 - 97);
				}

				pos = phrase.IndexOf(' ', pos) + 1;
			}
			while (pos > 0 && pos < sb.Length);

			return sb.ToString();
		}

		private const string PATTERN2 =
			@"^(?<shtnum>(?<bldgid>[0-9]*[A-Z]*)(?=[ -]+[A-Z])(?<pbsep>[ -]*)(?<shtid1>[A-Z0-9\.-]*[a-z]*)|(?<shtid2>[A-Z]+ *[0-9\.-]+[a-z]*){1})(?<sep>[- ]+)(?>(?<comment1>\(.*\))|(?<shtname2>.*) (?<comment2>\(.*\))(?<ext2>\.[Pp][dD][Ff])|(?<shtname3>.*)(?<ext3>\.[Pp][dD][Ff])|(?<shtname4>.*))";

		private static Regex pattern2 =  new Regex(PATTERN2, RegexOptions.Compiled | RegexOptions.Singleline);

		// capture group names
		private const string PHASE_BLDG_STR     = "bldgid";
		private const string PHASE_BLDG_SEP_STR = "pbsep";
		private const string SHEET_ID_1_STR     = "shtid1";
		private const string SHEET_ID_2_STR     = "shtid2";
		private const string SEPARATOR_STR      = "sep";
		private const string SHEET_NAME_2_STR   = "shtname2";
		private const string SHEET_NAME_3_STR   = "shtname3";
		private const string SHEET_NAME_4_STR   = "shtname4";
		private const string COMMENT_1_STR      = "comment1";
		private const string COMMENT_2_STR      = "comment2";
		private const string EXTENSION_1_STR    = "ext2";
		private const string EXTENSION_2_STR    = "ext3";

		public static bool ParseFileName(SheetNameInfo sheet)
		{
			Match match = pattern2.Match(sheet.FileName);

			if (!match.Success) return false;

			GroupCollection g = match.Groups;

			string test;

			sheet.Separator = g[SEPARATOR_STR].Value;

			// phase-bldg
			test = g[PHASE_BLDG_STR].Value;
			if (!string.IsNullOrEmpty(test))
			{
				sheet.PhaseBldg = test;
				sheet.PhaseBldgSep = g[PHASE_BLDG_SEP_STR].Value;
				sheet.SheetID = g[SHEET_ID_1_STR].Value;
			}
			else
			{
				sheet.SheetID = g[SHEET_ID_2_STR].Value;
			}

			test = g[COMMENT_1_STR].Value;
			if (string.IsNullOrEmpty(test))
			{
				test = g[COMMENT_2_STR].Value;
				if (!string.IsNullOrEmpty(test))
				{
					sheet.Comment = test;
					sheet.OriginalSheetTitle = g[SHEET_NAME_2_STR].Value;
				} 
				else if (!string.IsNullOrEmpty(g[SHEET_NAME_3_STR].Value))
				{
					sheet.OriginalSheetTitle = g[SHEET_NAME_3_STR].Value;
				} 
				else if (!string.IsNullOrEmpty(g[SHEET_NAME_4_STR].Value))
				{
					sheet.OriginalSheetTitle = g[SHEET_NAME_4_STR].Value;
				}

			}
			else
			{ 
				sheet.OriginalSheetTitle = "";
				sheet.Comment = test;
			}

			return true;
		}
	}
}
