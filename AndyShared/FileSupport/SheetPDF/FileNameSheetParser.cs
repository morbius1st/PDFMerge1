#region using

using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UtilityLibrary;

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

		private const string PATTERN_SHTNUM_AND_NAME =
			@"^(?<shtnum>((?<bldgid>([0-9]*|[A-Z]*)([A-Z]*|[0-9]*))(?= ))?(?<pbsep> *)(?<shtid>[^ ]*))([ -]+)(?<shtname>.*)";

		private static Regex patternShtNumAndName =
			new Regex(PATTERN_SHTNUM_AND_NAME, RegexOptions.Compiled | RegexOptions.Singleline);


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

			// phase-bldg
			test = g[ids.CompGrpName(compType.PHBLDG, PHBLDGIDX)].Value;

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

			shtIdComps.sheetTitle = shtIdComps.originalSheetTitle;

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