#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  5/27/2020 10:55:08 PM

namespace AndyShared.FilesSupport
{
	public enum FileTypeSheetPdf
	{
		SHEET_PDF,
		NON_SHEET_PDF,
		OTHER
	}

	public class FileNameSheetParser
	{
	#region private fields

		private static readonly Lazy<FileNameSheetParser> instance =
			new Lazy<FileNameSheetParser>(() => new FileNameSheetParser());

		private const string PATTERN_SHTNUM_AND_NAME =
			@"^(?<shtnum>(?<bldgid>[0-9]*[A-Z]*)(?=[ -]+[A-Z])(?<pbsep>[ -]*)(?<shtid1>[A-Z0-9\.-]*[a-z]*)|(?<shtid2>[A-Z]+ *[0-9\.-]+[a-z]*){1})(?<sep>[- ]+)(?>(?<comment1>\(.*\))|(?<shtname2>.*) (?<comment2>\(.*\))(?<ext2>\.[Pp][dD][Ff])|(?<shtname3>.*)(?<ext3>\.[Pp][dD][Ff])|(?<shtname4>.*))";

		private static Regex patternShtNumAndName =  
			new Regex(PATTERN_SHTNUM_AND_NAME, RegexOptions.Compiled | RegexOptions.Singleline);

		// capture group names
		private const string PHASE_BLDG_STR     = "bldgid";
		private const string PHASE_BLDG_SEP_STR = "pbsep";
		private const string SHEET_ID_1_STR     = "shtid1";
		private const string SHEET_ID_2_STR     = "shtid2";
		private const string SEPARATOR_STR      = "sep";
		private const string SHEET_NAME_2_STR   = "shtname2";
		private const string SHEET_NAME_3_STR   = "shtname3";
		private const string SHEET_NAME_4_STR   = "shtname4";
//		private const string COMMENT_1_STR      = "comment1";
//		private const string COMMENT_2_STR      = "comment2";
		private const string EXTENSION_1_STR    = "ext2";
		private const string EXTENSION_2_STR    = "ext3";

	#endregion

	#region ctor

		private FileNameSheetParser() { }

	#endregion

	#region public properties

		public static FileNameSheetParser Instance => instance.Value;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Parse(FileNameSheetComponents components, string filename)
		{
			Match match = patternShtNumAndName.Match(filename);

			if (!match.Success) return false;

			GroupCollection g = match.Groups;

			string test;

			components.separator = g[SEPARATOR_STR].Value;

			// phase-bldg
			test = g[PHASE_BLDG_STR].Value;
			if (!string.IsNullOrEmpty(test))
			{
				components.phaseBldg = test;
				components.phaseBldgSep = g[PHASE_BLDG_SEP_STR].Value;
				components.sheetID = g[SHEET_ID_1_STR].Value;
			}
			else
			{
				components.sheetID = g[SHEET_ID_2_STR].Value;
			}

//			test = g[COMMENT_1_STR].Value;
//			if (string.IsNullOrEmpty(test))
//			{
//				test = g[COMMENT_2_STR].Value;
//				if (!string.IsNullOrEmpty(test))
//				{
////					components.comment = test;
//					components.originalSheetTitle = g[SHEET_NAME_2_STR].Value;
//				}
//				else if (!string.IsNullOrEmpty(g[SHEET_NAME_3_STR].Value))
//				{
//					components.originalSheetTitle = g[SHEET_NAME_3_STR].Value;
//				}
//				else if (!string.IsNullOrEmpty(g[SHEET_NAME_4_STR].Value))
//				{
//					components.originalSheetTitle = g[SHEET_NAME_4_STR].Value;
//				}
//			}
//			else
//			{
				components.originalSheetTitle = "";
//				components.comment = test;
//			}

			components.sheetTitle = components.originalSheetTitle;

			return true;
		}


		private const string PATTERN_SHT_NUM =
			@"^(?<discipline>GRN|(?>[A-Z][A-Z]?)(?=[\-,0-9]))(?<cat>[0-9]{0,2})((?<sep1>\-|\.)(?<subcat>[0-9]{1,3}[A-Za-z]?)((?<sep2>\-)(?<modifier>[0-9,A-Z,a-z]{1,4}))?((?<sep3>\.)(?<submodifier>[0-9]{1,2}))?)?";

		private static Regex patternShtNum =
			new Regex(PATTERN_SHT_NUM, RegexOptions.Compiled | RegexOptions.Singleline);

		// capture group names
		private const string DISCIPLINE  = "discipline";
		private const string CATEGORY    = "cat";
		private const string SEP1        = "sep1";
		private const string SUBCATEGORY = "subcat";
		private const string SEP2        = "sep2";
		private const string MODIFIER    = "modifier";
		private const string SEP3        = "sep3";
		private const string SUBMODIFIER = "submodifier";

		public bool ParseSheetId(FileNameSheetComponents components, string shtId)
		{
			if (shtId.IsVoid()) return false;

			Match match = patternShtNum.Match(shtId);

			if (!match.Success) return false;

			GroupCollection g = match.Groups;

			string test;

			test = g[DISCIPLINE].Value;

			if (!test.IsVoid())
			{
				components.discipline = test;
				components.category = g[CATEGORY].Value;
				test = g[SEP1].Value;

				if (!test.IsVoid())
				{
					components.seperator1 = test;
					components.subcategory = g[SUBCATEGORY].Value;

					test = g[SEP2].Value;

					if (!test.IsVoid())
					{
						components.seperator2 = test;
						components.modifier = g[MODIFIER].Value;

						test = g[SEP3].Value;

						if (!test.IsVoid())
						{
							components.seperator3 = test;
							components.submodifier = g[SUBMODIFIER].Value;
						}
					}
				}
			}
			else
			{
				// should never happen
				return false;
			}

			return true;
		}

	#endregion

	#region private methods

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FileNameSheetParser";
		}

	#endregion
	}



	public class FileExtensionPdfClassifier : FileExtensionClassifier
	{
		public override string[] fileTypes { get; set; } = {"pdf"};
	}



}