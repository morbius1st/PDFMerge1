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

		private const string PATTERN =
			@"^(([0-9]*[A-Z]*)(?=[ -]+[A-Z])([ -]*)([A-Z0-9\.-]*[a-z]*)|([A-Z]+ *[0-9\.-]+[a-z]*){1})([- ]+)((.*) (\(.*\))(\.[Pp][dD][Ff])|(.*)(\.[Pp][dD][Ff])|(.*))";

		private static Regex pattern =  new Regex(PATTERN, RegexOptions.Compiled | RegexOptions.Singleline);

		private const int PHASE_BLDG            = 2;
		private const int PHASE_BLDG_SEP        = 3;
		private const int SHEET_ID_GOT          = 4;
		private const int SHEET_ID_NOT          = 5;
		private const int SEPARATOR             = 6;
		private const int SHEET_NAME_COMMENT    = 8;
		private const int SHEET_NAME_NO_COMMENT = 11;
		private const int SHEET_NAME            = 7;
		private const int COMMENT               = 9;
		private const int EXTENSION_COMMENT     = 10;
		private const int EXTENSION_NO_COMMENT  = 12;

		public static bool ParseFile(SheetId sheet)
		{
			Match match = pattern.Match(sheet.FileName);

			if (!match.Success) return false;

			GroupCollection g = match.Groups;

			string test;

			// phase-bldg
			test = g[PHASE_BLDG].Value;
			if (!string.IsNullOrEmpty(test))
			{
				sheet.PhaseBldg = test;
				sheet.PhaseBldgSep = g[PHASE_BLDG_SEP].Value;
			}

			// separator
			sheet.Separator = g[SEPARATOR].Value;

			// comment
			test = g[COMMENT].Value;
			if (!string.IsNullOrEmpty(test)) sheet.Comment = test;

			// sheet ID
			test = g[SHEET_ID_GOT].Value;
			sheet.SheetID = !string.IsNullOrEmpty(test) ? test : g[SHEET_ID_NOT].Value;

			// sheet name
			test = g[EXTENSION_COMMENT].Value;
			if (!string.IsNullOrEmpty(test))
			{
				sheet.SheetName = g[SHEET_NAME_COMMENT].Value;
			}
			else if (!string.IsNullOrEmpty(g[EXTENSION_NO_COMMENT].Value))
			{
				sheet.SheetName = g[SHEET_NAME_NO_COMMENT].Value;
			}
			else
			{
				sheet.SheetName = g[SHEET_NAME].Value;
			}

			return true;
		}




//		public static T ParseFileName<T>(Route r) where T : SheetId, new()
//		{
//			T sheet = new T {FullFileRoute = r};
//
//			Match match = pattern.Match(r.FileName);
//
//			GroupCollection g = match.Groups;
//
//			string test;
//
//			// phase-bldg
//			test = g[PHASE_BLDG].Value;
//			if (!string.IsNullOrEmpty(test))
//			{
//				sheet.PhaseBldg = test;
//				sheet.PhaseBldgSep = g[PHASE_BLDG_SEP].Value;
//			}
//
//			// separator
//			sheet.Separator = g[SEPARATOR].Value;
//
//			// comment
//			test = g[COMMENT].Value;
//			if (!string.IsNullOrEmpty(test)) sheet.Comment = test;
//
//			// sheet ID
//			test = g[SHEET_ID_GOT].Value;
//			sheet.SheetID = !string.IsNullOrEmpty(test) ? test : g[SHEET_ID_NOT].Value;
//
//			// sheet name
//			test = g[EXTENSION_COMMENT].Value;
//			if (!string.IsNullOrEmpty(test))
//			{
//				sheet.SheetName = g[SHEET_NAME_COMMENT].Value;
//			}
//			else if (!string.IsNullOrEmpty(g[EXTENSION_NO_COMMENT].Value))
//			{
//				sheet.SheetName = g[SHEET_NAME_NO_COMMENT].Value;
//			}
//			else
//			{
//				sheet.SheetName = g[SHEET_NAME].Value;
//			}
//
//			return sheet;
//		}
	}
}
