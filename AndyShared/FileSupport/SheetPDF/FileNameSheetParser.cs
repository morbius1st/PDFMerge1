#region using

using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UtilityLibrary;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.pbCompsIdx;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.ShtIdCompsIdx;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.identCompsIdx;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers;
using shtIds = AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers;
using fileType = AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.FileTypeSheetPdf;
using compType = AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.ShtCompTypes;

#endregion

// username: jeffs
// created:  5/27/2020 10:55:08 PM

namespace AndyShared.FileSupport.SheetPDF
{
	public class FileNameSheetParser
	{
	#region private fields

		private FileNameSheetIdentifiers ids = new FileNameSheetIdentifiers();

		private static readonly Lazy<FileNameSheetParser> instance =
			new Lazy<FileNameSheetParser>(() => new FileNameSheetParser());

		// private const string PATTERN_SHTNUM_AND_NAME =
		// 	@"^(?<shtnum>(?<bldgid>[0-9]*[A-Z]*)(?=[ -]+[A-Z])(?<pbsep>[ -]*)(?<shtid1>[A-Z0-9\.-]*[a-z]*)|(?<shtid2>[A-Z]+ *[0-9\.-]+[a-z]*){1})(?<sep>[- ]+)(?>(?<comment1>\(.*\))|(?<shtname2>.*) (?<comment2>\(.*\))(?<ext2>\.[Pp][dD][Ff])|(?<shtname3>.*)(?<ext3>\.[Pp][dD][Ff])|(?<shtname4>.*))";

		// private const string PATTERN_SHTNUM_AND_NAME =
		// 	@"^(?<shtnum>(?<bldgid>[0-9]*[A-Z]*)(?=[ -]+[A-Z])(?<pbsep>[ -]*)(?<shtid1>[A-Z0-9\.\-]*[a-z]*\(?[A-Za-z]*\/?[A-Za-z]*\)?)|(?<shtid2>[A-Z]+ *[0-9\.-]+[a-z]*){1})(?<sep>[- ]+)(?>(?<comment1>\(.*\))|(?<shtname2>.*) (?<comment2>\(.*\))(?<ext2>\.[Pp][dD][Ff])|(?<shtname3>.*)(?<ext3>\.[Pp][dD][Ff])|(?<shtname4>.*))";

		// private const string PATTERN_SHTNUM_AND_NAME =
		// 	@"^(?<shtnum>(?<bldgid>([0-9]*|[A-Z]*)([A-Z]*|[0-9]*))(?=[ -]+[A-Z])(?<pbsep>[ -]*)(?<shtid1>[A-Z0-9\.\-]*[a-z]*\(?[A-Za-z]*\/?[A-Za-z]*\)?)|(?<shtid2>[A-Z]+ *[0-9\.-]+[a-z]*){1})(?<sep>[- ]+)(?>(?<comment1>\(.*\))|(?<shtname2>.*) (?<comment2>\(.*\))(?<ext2>\.[Pp][dD][Ff])|(?<shtname3>.*)(?<ext3>\.[Pp][dD][Ff])|(?<shtname4>.*))";

		private const string PATTERN_SHTNUM_AND_NAME =
			@"^(?<shtnum>((?<bldgid>([0-9]*|[A-Z]*)([A-Z]*|[0-9]*))(?= ))?(?<pbsep> *)(?<shtid>[^ ]*))([ -]+)(?<shtname>.*)";


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
		private const string COMMENT_1_STR      = "comment1";
		private const string COMMENT_2_STR      = "comment2";
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

		/// <summary>
		/// Parse out the Building / Phase, separator, Sheet ID, sheet name
		/// </summary>
		/// <param name="sheetComps"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public bool Parse(FileNameSheetPdf sheetComps, string filename)
		{
			Match match = patternShtNumAndName.Match(filename);

			if (!match.Success) return false;

			GroupCollection g = match.Groups;

			string test;

			sheetComps.separator = g[SEPARATOR_STR].Value;

			// phase-bldg
			test = g[PHASE_BLDG_STR].Value;
			if (!string.IsNullOrEmpty(test))
			{
				sheetComps.phaseBldg = test;
				sheetComps.phaseBldgSep = g[PHASE_BLDG_SEP_STR].Value;
				sheetComps.sheetID = g[SHEET_ID_1_STR].Value;
			}
			else
			{
				sheetComps.sheetID = g[SHEET_ID_2_STR].Value;
			}

			test = g[COMMENT_1_STR].Value;
			if (string.IsNullOrEmpty(test))
			{
				test = g[COMMENT_2_STR].Value;
				if (!string.IsNullOrEmpty(test))
				{
//					components.comment = test;
					sheetComps.originalSheetTitle = g[SHEET_NAME_2_STR].Value;
				}
				else if (!string.IsNullOrEmpty(g[SHEET_NAME_3_STR].Value))
				{
					sheetComps.originalSheetTitle = g[SHEET_NAME_3_STR].Value;
				}
				else if (!string.IsNullOrEmpty(g[SHEET_NAME_4_STR].Value))
				{
					sheetComps.originalSheetTitle = g[SHEET_NAME_4_STR].Value;
				}
			}
			else
			{
				sheetComps.originalSheetTitle = "";
				// components.comment = test;
			}

			sheetComps.sheetTitle = sheetComps.originalSheetTitle;

			return true;
		}

		/// <summary>
		/// Parse out the Building / Phase, separator, Sheet ID, sheet name
		/// </summary>
		/// <param name="shtIdComps"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public bool Parse2(FileNameSheetPdf shtIdComps, string filename)
		{
			shtIdComps.isPhaseBldg = null;
			shtIdComps.shtCompType = compType.UNASSIGNED;

			Match match = patternShtNumAndName.Match(filename);

			if (!match.Success)
			{
				shtIdComps.fileType = fileType.INVALID;
				return false;
			}

			GroupCollection g = match.Groups;

			string test;

			// shtIdComps.separator = g[SEPARATOR_STR].Value;

			// phase-bldg
			test = g[PHASE_BLDG_STR].Value;
			if (!test.IsVoid())
			{
				shtIdComps.PbComps[PHBLDGIDX] = test;
				shtIdComps.PbComps[PBSEPIDX] =
					g[ids.CompGrpName(compType.PHBLDG, PBSEPIDX)].Value;
				shtIdComps.sheetID = g[ids.CompGrpName(compType.PHBLDG, SHTIDIDX)].Value;

				shtIdComps.isPhaseBldg = true;
			}
			else
			{
				shtIdComps.sheetID = g[ids.CompGrpName(compType.PHBLDG, SHTIDIDX)].Value;
				shtIdComps.isPhaseBldg = false;
			}

			shtIdComps.originalSheetTitle = g[ids.CompGrpName(compType.PHBLDG, SHTNAMEIDX)].Value;


// 			if (!test.IsVoid())
// 			{
// 				test = g[COMMENT_2_STR].Value;
// 				if (!test.IsVoid())
// 				{
// //					components.comment = test;
// 					shtIdComps.originalSheetTitle = g[SHEET_NAME_2_STR].Value;
// 				}
// 				else if (!string.IsNullOrEmpty(g[SHEET_NAME_3_STR].Value))
// 				{
// 					shtIdComps.originalSheetTitle = g[SHEET_NAME_3_STR].Value;
// 				}
// 				else if (!string.IsNullOrEmpty(g[SHEET_NAME_4_STR].Value))
// 				{
// 					shtIdComps.originalSheetTitle = g[SHEET_NAME_4_STR].Value;
// 				}
// 			}
// 			else
// 			{
// 				shtIdComps.originalSheetTitle = "";
// 				// components.comment = test;
// 			}

			shtIdComps.sheetTitle = shtIdComps.originalSheetTitle;

			return true;
		}


		private const string PATTERN_SHT_NUM =
			@"^(?<discipline>GRN|(?>[A-Z][A-Z]?)(?=[\-,0-9]))(?<cat>[0-9]{0,2})((?<sep1>\-|\.)(?<subcat>[0-9]{1,3}[A-Za-z]?)((?<sep2>\-)(?<modifier>[0-9,A-Z,a-z]{1,4}))?((?<sep3>\.)(?<submodifier>[0-9]{1,2}))?)?";

		// ^(?<discipline>GRN|(?>[A-Z][A-Z]?)(?=[\-,0-9]))(?<cat>[0-9]{0,2})((?<sep1>\-|\.)(?<subcat>[0-9]{1,3}[A-Za-z]?)((?<sep2>\-)(?<modifier>[0-9,A-Z,a-z]{1,4}))?((?<sep3>\.)(?<submodifier>[0-9]{1,2}))?((?<sep4>\({1})(?<identifier>[0-9,A-Z,a-z]+)(?<sep5>\/{0,1})(?<subidentifier>[0-9,A-Z,a-z]*)(?<sep6>\){1}))?)?
		private static Regex patternShtNum =
			new Regex(PATTERN_SHT_NUM, RegexOptions.Compiled | RegexOptions.Singleline);

		// capture group names
		private const string DISCIPLINE    = "discipline";
		private const string CATEGORY      = "cat";
		private const string SEP1          = "sep1";
		private const string SUBCATEGORY   = "subcat";
		private const string SEP2          = "sep2";
		private const string MODIFIER      = "modifier";
		private const string SEP3          = "sep3";
		private const string SUBMODIFIER   = "submodifier";
		private const string SEP4          = "sep4";
		private const string IDENTIFIER    = "identifier";
		private const string SEP6          = "sep5";
		private const string SUBIDENTIFIER = "subidentifier";
		private const string SEP5          = "sep6";

		/// <summary>
		/// parse the sheet id into its components
		/// </summary>
		/// <param name="components"></param>
		/// <param name="shtId"></param>
		/// <returns></returns>
		public bool ParseSheetId(FileNameSheetPdf components, string shtId)
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
					components.seperator11 = test;
					components.subcategory = g[SUBCATEGORY].Value;

					test = g[SEP2].Value;

					if (!test.IsVoid())
					{
						components.seperator12 = test;
						components.modifier = g[MODIFIER].Value;

						test = g[SEP3].Value;

						if (!test.IsVoid())
						{
							components.seperator13 = test;
							components.submodifier = g[SUBMODIFIER].Value;

							test = g[SEP4].Value;

							if (!test.IsVoid())
							{
								components.seperator1 = test;
								components.identifier = g[IDENTIFIER].Value;

								test = g[SEP5].Value;

								if (!test.IsVoid())
								{
									components.seperator2 = test;
									components.subidentifier = g[SUBIDENTIFIER].Value;

									test = g[SEP6].Value;

									if (!test.IsVoid())
									{
										components.seperator3 = test;
									}
								}
							}
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


		private const string SHT_NUM_PATTERN =
			@"(((?<Discipline10>[A-Z][A-Z]?(?=[\.\-0-9]))(?<Category10>[0-9]{0,2})(?<sep11>\.)(?<SubCategory10>[0-9]{0,3}[A-Za-z]?)(?<sep12>\-)(?<Modifier10>[0-9A-Za-z]{0,4})((?<sep13>\.)(?<SubModifier10>[0-9]{0,2}))?|(?<Discipline20>GRN)(?<sep21>|\.)(?<Category20>[0-9]{1,3})|(?<Discipline30>[A-Z][A-Z]?)(?<Category30>[0-9]{1,3})(?<sep31>\.(?=[0-9]))?(?<SubCategory30>(?<=\.)[0-9]{1,3}[A-Za-z]{0,2})?){1}|((?<Discipline40>[A-Z][A-Z]?)(?<sep41>\-)(?<Category40>[0-9]{1,3}[A-Za-z]{0,2})))($|(?<sep4>\()(?=[0-9A-Za-z])(?<Identifier>(?<=\()[0-9A-Za-z]*(?=│|\)))(?<sep5>│{0,1})(?<SubIdentifier>[0-9A-Za-z]*(?=\)))(?<sep6>\){0,1})$)";

//																																																																																																																																									 (?=│|\)))(?<sep5>│{0,1})
		private static Regex shtIdPattern =
			new Regex(SHT_NUM_PATTERN, RegexOptions.Compiled | RegexOptions.Singleline);

		/// <summary>
		/// parse out the components of a sheet id
		/// </summary>
		/// <returns></returns>
		public bool ParseSheetId2(FileNameSheetPdf shtIdComps, string shtId)
		{
			bool status = true;

			if (shtId.IsVoid()
				|| shtIdComps.fileType != fileType.SHEET_PDF)
			{
				status = false;
			}
			else
			{
				Match match = shtIdPattern.Match(shtId);

				if (match.Success)
				{
					GroupCollection g = match.Groups;

					shtIdComps.shtCompType = GetShtCompTypeFromGrpCollection(g);

					try
					{
						if (!ParseType(shtIdComps.shtCompType, g, shtIdComps)) status = false;
					}
					catch
					{
						status = false;
					}

					if (!ParseIdent(g, shtIdComps)) status = false;
				}
				else
				{
					status = false;
				}
			}

			if (!status)
			{
				shtIdComps.fileType = fileType.INVALID;
				return false;
			}

			return true;
		}


		internal bool ParseIdent(GroupCollection g, FileNameSheetPdf shtIdComps)
		{
			bool status = false;
			shtIdComps.hasIdentifier = false;

			string test = g[ids.CompGrpName(compType.IDENT, SEP4IDX)].Value;

			if (!test.IsVoid())
			{
				shtIdComps.IdentComps[SEP4IDX] = test;

				test = g[ids.CompGrpName(compType.IDENT, IDENTIFIERIDX)].Value;

				if (!test.IsVoid())
				{
					shtIdComps.IdentComps[IDENTIFIERIDX] = test;

					test = g[ids.CompGrpName(compType.IDENT, SEP5IDX)].Value;

					if (!test.IsVoid())
					{
						shtIdComps.IdentComps[SEP5IDX] = test;

						test = g[ids.CompGrpName(compType.IDENT, SUBIDENTIFIERIDX)].Value;

						if (!test.IsVoid())
						{
							shtIdComps.IdentComps[SUBIDENTIFIERIDX] = test;

							status = true;
							shtIdComps.hasIdentifier = true;
						}
					}

					test = g[ids.CompGrpName(compType.IDENT, SEP6IDX)].Value;

					if (!test.IsVoid())
					{
						shtIdComps.IdentComps[SEP6IDX] = test;

						shtIdComps.hasIdentifier = true;
					} 
					else
					{
						status = false;
						shtIdComps.hasIdentifier = false;
					}
				}
			} 
			else
			{
				shtIdComps.hasIdentifier = false;
			}

			return true;
		}

		internal bool ParseType(compType type, GroupCollection g, FileNameSheetPdf shtIdComps)
		{
			string test;
			bool skipNext = false;

			foreach (SheetCompNames ci in ShtCompList[(int) type].ShtCompNames)
			{
				if (ci.SeqCtrl == SeqCtrl.SKIP_CONTINUE) continue;

				test = g[ids.CompGrpName(type, ci.Index)].Value;

				if (skipNext)
				{
					if (ci.SeqCtrl == SeqCtrl.OPTIONAL_END)
					{
						break;
					}

					skipNext = false;
					continue;
				}

				if (test.IsVoid())
				{
					if (ci.SeqCtrl == SeqCtrl.OPTIONAL_SKIP_NEXT)
					{
						skipNext = true;
					}
					else
					{
						return false;
					}
				}

				shtIdComps.ShtIdComps[ci.Index] = test;

				if (ci.SeqCtrl == SeqCtrl.REQUIRED_END) break;
			}

			return true;
		}

		//
		// internal bool ParseType10(GroupCollection g, FileNameSheetPdf shtIdComps)
		// {
		// 	string test;
		//
		// 	foreach (SheetCompNames ci in ShtCompList[(int) compType.TYPE10].ShtCompNames)
		// 	{
		// 		if (ci.SeqCtrl == SeqCtrl.SKIP_CONTINUE) continue;
		//
		// 		test = g[ids.CompGrpName(compType.TYPE10, ci.Index)].Value;
		//
		// 		if (test.IsVoid())
		// 		{
		// 			shtIdComps.fileType = fileType.INVALID;
		// 			return false;
		// 		}
		//
		// 		shtIdComps.ShtIdComps[ci.Index] = test;
		//
		// 		if (ci.SeqCtrl == SeqCtrl.REQUIRED_END) break;
		// 	}
		//
		// 	return true;
		// }
		//
		// internal bool ParseType20(GroupCollection g, FileNameSheetPdf shtIdComps)
		// {
		// 	return true;
		// }
		//
		// internal bool ParseType30(GroupCollection g, FileNameSheetPdf shtIdComps)
		// {
		// 	return true;
		// }
		//
		//
		// // segregate the regex parsed components into their standard locations
		// internal bool ParseType40(GroupCollection g, FileNameSheetPdf shtIdComps)
		// {
		// 	bool status = false;
		// 	string test;
		//
		// 	// required - cannot be null / empty
		// 	test = g[ids.CompGrpName(compType.TYPE40, DISCIPLINEIDX)].Value;
		//
		// 	if (!test.IsVoid())
		// 	{
		// 		shtIdComps.ShtIdComps[DISCIPLINEIDX] = test;
		//
		// 		// required - cannot be null / empty
		// 		test = g[ids.CompGrpName(compType.TYPE40, SEP0IDX)].Value;
		//
		// 		if(!test.IsVoid())
		// 		{
		// 			shtIdComps.ShtIdComps[SEP0IDX] = test;
		//
		// 			// required - cannot be null / empty
		// 			test = g[ids.CompGrpName(compType.TYPE40, CATEGORYIDX)].Value;
		//
		// 			if (!test.IsVoid())
		// 			{
		// 				shtIdComps.ShtIdComps[CATEGORYIDX] = test;
		//
		// 				status = true;
		// 			}
		// 		}
		// 	}
		//
		// 	if (!status) shtIdComps.fileType = fileType.INVALID;
		//
		// 	return status;
		// }

		// return true == has identifier and is good
		// return false == has identifier and is bad
		// return null == does not have an indetifier
		// internal bool? ParseIndentifier(GroupCollection g, FileNameSheetPdf shtIdComps)
		// {
		// 	bool status = false;
		// 	string test;
		//
		// 	test = g[ids.CompGrpName(compType.IDENT, IDENTIFIERIDX)].Value;
		//
		// 	if (!test.IsVoid())
		// 	{
		// 		// has the start of an identifier - now must have all
		//
		// 	} else
		// 	{
		// 		// no identifier
		// 		shtIdComps.hasIdentifier = null;
		// 		return null;
		// 	}
		//
		//
		// 	return true;
		// }


		internal shtIds.ShtCompTypes GetShtCompTypeFromGrpCollection(GroupCollection g)
		{
			if (!g[ids.CompGrpName(compType.TYPE10, DISCIPLINEIDX)].Value.IsVoid())
			{
				return compType.TYPE10;
			}
			else if (!g[ids.CompGrpName(compType.TYPE20, DISCIPLINEIDX)].Value.IsVoid())
			{
				return compType.TYPE20;
			}
			else if (!g[ids.CompGrpName(compType.TYPE30, DISCIPLINEIDX)].Value.IsVoid())
			{
				return compType.TYPE30;
			}
			else if (!g[ids.CompGrpName(compType.TYPE40, DISCIPLINEIDX)].Value.IsVoid())
			{
				return compType.TYPE40;
			}

			return compType.UNASSIGNED;
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