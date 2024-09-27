#region using

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UtilityLibrary;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;

#endregion

// username: jeffs
// created:  5/27/2020 10:55:08 PM

namespace AndyShared.FileSupport.FileNameSheetPDF
{
	public class FileNameSheetParser
	{
	#region private fields

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
			@"^(?<shtnum>((?<PhBldgid>([0-9]*|[A-Z]*)([A-Z]*|[0-9]*))(?= ))?(?<pbsep> *)(?<shtid>[^ ]*))([ -]+)(?<shtname>.*)";

		private static Regex patternShtNumAndName =
			new Regex(PATTERN_SHTNUM_AND_NAME, RegexOptions.Compiled | RegexOptions.Singleline);


		/// <summary>
		/// Parse out the Building / Phase, separator, Sheet ID, sheet name
		/// </summary>
		/// <param name="sheetPdf"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public bool Parse2(FileNameSheetPdf sheetPdf, string filename)
		{
			sheetPdf.isPhaseBldg = null;
			sheetPdf.shtCompType = ShtCompTypes.UNASSIGNED;

			Match match = patternShtNumAndName.Match(filename);

			if (!match.Success)
			{
				sheetPdf.fileType = FileTypeSheetPdf.INVALID;
				return false;
			}

			GroupCollection g = match.Groups;

			string test;

			// phase-bldg
			test = g[ShtIds.CompGrpName2(ShtCompTypes.PHBLDG, PHBLDG_COMP_IDX)].Value;

			if (!test.IsVoid())
			{
				sheetPdf.SheetComps[PHBLDG_VALUE_IDX] = test;
				sheetPdf.SheetComps[PBSEP_VALUE_IDX] =
					g[ShtIds.CompGrpName2(ShtCompTypes.PHBLDG, PBSEP_COMP_IDX)].Value;
				sheetPdf.sheetID = g[ShtIds.CompGrpName2(ShtCompTypes.PHBLDG, SHTID_COMP_IDX)].Value;

				sheetPdf.isPhaseBldg = true;
			}
			else
			{
				sheetPdf.sheetID = g[ShtIds.CompGrpName2(ShtCompTypes.PHBLDG, SHTID_COMP_IDX)].Value;
				sheetPdf.isPhaseBldg = false;
			}

			sheetPdf.originalSheetTitle = g[ShtIds.CompGrpName2(ShtCompTypes.PHBLDG, SHTNAME_COMP_IDX)].Value;

			sheetPdf.sheetTitle = sheetPdf.originalSheetTitle;

			return true;
		}

		private const string SHT_NUM_PATTERN =
			@"(((?<Discipline10>[A-Z][A-Z]?(?=[\.\-0-9]))(?<Category10>[0-9]{0,2})(?<sep11>\.)(?<SubCategory10>[0-9]{0,3}[A-Za-z]?)(?<sep12>\-)(?<Modifier10>[0-9A-Za-z]{0,4})((?<sep13>\.)(?<SubModifier10>[0-9]{0,2}))?|(?<Discipline20>GRN)(?<sep21>|\.)(?<Category20>[0-9]{1,3})|(?<Discipline30>[A-Z][A-Z]?)(?<Category30>[0-9]{1,3}(?=\.))(?<sep31>\.(?=[0-9]))?(?<SubCategory30>(?<=\.)[0-9]{1,3}[A-Za-z]{0,2})?){1}|((?<Discipline40>(?<![A-Z0-9])[A-Z]{1,2})(?<sep41>\-?)(?<Category40>[0-9]{1,3}[A-Za-z]{0,2})))($|(?<sep4>\()(?=[0-9A-Za-z])(?<Identifier>(?<=\()[0-9A-Za-z]*(?=│|\)))(?<sep5>│{0,1})(?<SubIdentifier>[0-9A-Za-z]*(?=\)))(?<sep6>\){0,1})$)";

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
				|| shtIdComps.fileType != FileTypeSheetPdf.SHEET_PDF)
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

					if (status)
					{
						shtIdComps.hasIdentifier = false;

						// if (!ParseIdent(g, shtIdComps)) status = false;
						if (!ParseType(ShtCompTypes.IDENT, g, shtIdComps))
						{
							status = false;
						} else
						{
							if (!shtIdComps.SheetComps[SEP4_VALUE_IDX].IsVoid())
							{
								shtIdComps.hasIdentifier = true;
							}
						}
					}
				}
				else
				{
					status = false;
				}
			}

			if (!status)
			{
				shtIdComps.fileType = FileTypeSheetPdf.INVALID;
				return false;
			}

			return true;
		}

		internal bool ParseType(ShtCompTypes type, GroupCollection g, FileNameSheetPdf shtIdComps)
		{
			string test;

			bool? use = null; // false is optional & true is required
			bool hasPrior = false;
			bool inOptSeq = false;

			foreach (SheetCompInfo2 ci in ShtIds.ShtCompList2[(int) type].ShtCompInfo)
			{
				if (!ci.IsUseOK) return false;

				if (ci.SeqCtrlUse == SeqCtrlUse2.SKIP ||
					ci.SeqCtrlUse == SeqCtrlUse2.NOT_USED) continue;

				if (ci.SeqCtrlUse == SeqCtrlUse2.OPTIONAL) use = false;
				else if (ci.SeqCtrlUse == SeqCtrlUse2.REQUIRED) use = true;

				if (inOptSeq)
				{
					if (ci.GetNextOpt() == SeqCtrlNextOpt.SEQ_END)
					{
						inOptSeq = false;
					}

					continue;
				}

				test = g[ci.GrpName].Value;

				if (use == true)
				{
					// required
					if (!ci.IsReqdProceedOK || !ci.IsReqdNextOK) return false;

					if (test.IsVoid()) return false;
				}
				else
				{
					if (!ci.IsOptProceedOK || !ci.IsOptNextOK) return false;

					// optional
					if (test.IsVoid())
					{
						// no test value
						if (ci.GetNextOpt() == SeqCtrlNextOpt.SEQ_END_SEQ_REQ) return false;
						if (hasPrior && ci.GetNextOpt() == SeqCtrlNextOpt.REQ_IF_PRIOR) return false;

						if (ci.GetNextOpt() == SeqCtrlNextOpt.SEQ_START)
						{
							inOptSeq = true;
						}

						hasPrior = false;
						continue;
					}

					// not void - has a test value

					if (ci.GetNextOpt() == SeqCtrlNextOpt.SEQ_END_SEQ_REQ)
					{
						inOptSeq = false;
					}
				}

				if (ci.ValueIndex < 15)
				{
					shtIdComps.SheetComps[ci.ValueIndex] = test;
				}
				else
				{
					Debug.WriteLine($"index| {ci.ValueIndex}");
					Debug.WriteLine($"title| {ci.Title}");
					Debug.WriteLine($"name | {ci.GrpName}");
				}

				

				hasPrior = true;

				if (ci.GetProceedOpt() == SeqCtrlProceedOpt.END ||
					ci.GetProceedReqd() == SeqCtrlProceedReqd.END) break;
			}

			return true;
		}

		internal ShtCompTypes GetShtCompTypeFromGrpCollection(GroupCollection g)
		{
			if (!g[ShtIds.CompGrpName2(ShtCompTypes.TYPE10, 0)].Value.IsVoid())
			{
				return ShtCompTypes.TYPE10;
			}
			else if (!g[ShtIds.CompGrpName2(ShtCompTypes.TYPE20, 0)].Value.IsVoid())
			{
				return ShtCompTypes.TYPE20;
			}
			else if (!g[ShtIds.CompGrpName2(ShtCompTypes.TYPE30, 0)].Value.IsVoid())
			{
				return ShtCompTypes.TYPE30;
			}
			else if (!g[ShtIds.CompGrpName2(ShtCompTypes.TYPE40, 0)].Value.IsVoid())
			{
				return ShtCompTypes.TYPE40;
			}

			return ShtCompTypes.UNASSIGNED;
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