#region using

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using AndyShared.FileSupport.FileNameSheetPDF4;
using Test4.SheetMgr;
using Test4.Support;

using UtilityLibrary;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

// using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
using static Test4.Support.SheetIdentifiers3;

#endregion

// username: jeffs
// created:  5/27/2020 10:55:08 PM

namespace AndyShared.FileSupport.FileNameSheetPDF
{
	public class FileNameSheetParser4
	{
	#region private fields

		private List<int> compLengths;

		private Regex shtFileNamePattern;

		private static readonly Lazy<FileNameSheetParser4> instance =
			new Lazy<FileNameSheetParser4>(() => new FileNameSheetParser4());

	#endregion

	#region ctor

		private FileNameSheetParser4() { }

	#endregion

	#region public properties

		public static FileNameSheetParser4 Instance => instance.Value;

		public bool IsConfigd => !PdfFileNamePattern.IsVoid();

		public List<int> CompLengths
		{
			get => compLengths;
			set => compLengths = value;
		}
		

	#endregion

	#region private properties

	#endregion

	#region public methods

		public string PdfFileNamePattern { get; private set; }

		public string SpecialDisciplines { get; set; }

		// private const string SHT_FILE_NAME_PATTERN =
		// @"^(?<PhBldg>(?:[0-9]{0,2}[A-Z]{0,2}(?= [^0-9])))*? ?(?<Discipline>[A-Z](?=[\-\. 0-9]))(?<sep0>[\-\. ]?)(?<Category>\w*)(?<sep1>[\-\.]?)(?<SubCategory>\w*)(?<sep2>[\-\.]?)(?<Modifier>\w*?(?=[\-\.]))*(?<sep3>[\-\.]?)(?<SubModifier>\w*?(?=[\-\.]))*(?<sep4>[\-\.]?)(?<Identifier>\w*?(?=[\-\.]))*(?<sep5>[\-\.]?)(?<SubIdentifier>\w*)";

		// this should extract the sheet title as well as parsing the sheet number	
		// maybe not
		//@"^(?<PhBldgid>(?:[0-9]{0,2}[A-Z]{0,2}(?= [^0-9])))*? ?(?<Discipline>[A-Z](?=[\-\. 0-9]))(?<sep0>[\-\. ]?)(?<Category>\w*)(?<sep1>[\-\.]?)(?<SubCategory>\w*)(?<sep2>[\-\.]?)(?<Modifier>\w*?(?=[\-\. ]))*(?<sep3>[\-\.]?)(?<SubModifier>\w*?(?=[\-\. ]))*(?<sep4>[\-\.]?)(?<Identifier>\w*?(?=[\-\. ]))*(?<sep5>[\-\.]?)(?<SubIdentifier>\w*)\s-\s(?<SheetName>.*)";
		// revised - v3
		// @"^(?<PhBldgid>(?:[0-9]{0,2}[A-Z]{0,2}(?= [^0-9])))*? ?(?<Discipline>[A-Z](?=[\-\. 0-9]))(?<sep0>[\-\. ]?)(?<Category>\w*)(?<sep1>[\-\.]?)(?<SubCategory>\w*)(?<sep2>[\-\.]?)(?<Modifier>(?>\w*?(?=[\-\. ]))*)(?<sep3>[\-\.]?)(?<SubModifier>(?>\w*?(?=[\-\. ]))*)(?<sep4>[\-\.]?)(?<Identifier>(?>\w*?(?=[\-\. ]))*)(?<sep5>[\-\.]?)(?<SubIdentifier>\w*)\s-\s(?<Title>.*)";
		// revised - v4
		//@"^(?<PhBldgid>(?>(?:[0-9]{0,2}[A-Z]{0,2}(?= [^0-9])))?) ?(?<Discipline>[A-Z](?=[\-\. 0-9]))(?<sep0>[\-\. ]?)(?<Category>\w*)(?<sep1>[\-\.]?)(?<SubCategory>\w*)(?<sep2>[\-\.]?)(?<Modifier>(?>\w*?(?=[\-\. ]))*)(?<sep3>[\-\.]?)(?<SubModifier>(?>\w*?(?=[\-\. ]))*)(?<sep4>[\-\.]?)(?<Identifier>(?>\w*?(?=[\-\. ]))*)(?<sep5>[\-\.]?)(?<SubIdentifier>\w*)\s-\s(?<Title>.*)";
		//@"^(?<SheetId>(?<PhBldgid>(?>(?:[0-9]{0,2}[A-Z]{0,2}(?= [^0-9])))?) ?(?<Discipline>[A-Z](?=[\-\. 0-9]))(?<sep0>[\-\. ]?)(?<Category>\w*)(?<sep1>[\-\.]?)(?<SubCategory>\w*)(?<sep2>[\-\.]?)(?<Modifier>(?>\w*?(?=[\-\. ]))*)(?<sep3>[\-\.]?)(?<SubModifier>(?>\w*?(?=[\-\. ]))*)(?<sep4>[\-\.]?)(?<Identifier>(?>\w*?(?=[\-\. ]))*)(?<sep5>[\-\.]?)(?<SubIdentifier>\w*))\s-\s(?<SheetTitle>.*)";
		//@"^(?<SheetNum>(?<PhaseBuilding>(?>(?:[0-9]{0,2}[A-Z]{0,2}(?= [^0-9])))?) ?(?<SheetId>(?<Discipline>(?>T24|[A-Z]{0,3}(?=[\-\. 0-9])))(?<sep0>[\-\. ]?)(?<Category>\w*)(?<sep1>[\-\.]?)(?<SubCategory>\w*)(?<sep2>[\-\.]?)(?<Modifier>(?>\w*?(?=[\-\. ]))*)(?<sep3>[\-\.]?)(?<SubModifier>(?>\w*?(?=[\-\. ]))*)(?<sep4>[\-\.]?)(?<Identifier>(?>\w*?(?=[\-\. ]))*)(?<sep5>[\-\.]?)(?<SubIdentifier>\w*)))\s-\s(?<SheetTitle>.*)";

		public void Config()
		{
			initCompLengths(SheetIdentifiers3.SheetNumComponents.Count);
		}

		public void CreateFileNamePattern()
		{
			string patternPrefix =
				@"^(?<SheetNum>(?<PhBldgid>(?>(?:[0-9]{0,3}[A-Z]{0,3}(?= [^0-9])))?) ?(?<SheetId>(?<Discipline>(?>";
			string patternSuffix =
				@"[A-Z]{0,3}(?=[\-\. 0-9]|$)))(?<sep0>[\-\. ]?)(?<Category>\w*)(?<sep1>[\-\.]?)(?<SubCategory>\w*)(?<sep2>[\-\.]?)(?<Modifier>(?>\w*?(?=[\-\.]|$))*)(?<sep3>[\-\.]?)(?<SubModifier>(?>\w*?(?=[\-\.]|$))*)(?<sep4>[\-\.]?)(?<Identifier>(?>\w*?(?=[\-\.]|$))*)(?<sep5>[\-\.]?)(?<SubIdentifier>\w*)))(?>\s-\s(?<SheetTitle>.*))*";

			PdfFileNamePattern = $"{patternPrefix}{SpecialDisciplines}{patternSuffix}";

			shtFileNamePattern =
				new Regex(PdfFileNamePattern, RegexOptions.Compiled | RegexOptions.Singleline);
		}

		public void CreateSpecialDisciplines(List<string> specialDisciplines)
		{
			SpecialDisciplines = "";

			if (specialDisciplines == null || specialDisciplines.Count == 0) return;

			StringBuilder sb = new StringBuilder("");

			foreach (string s in specialDisciplines)
			{
				sb.Append(s).Append("|");
			}

			SpecialDisciplines = sb.ToString();
		}

		// version 3 - use this
		public Match MatchShtNumber(string  shtNum)
		{
			return shtFileNamePattern.Match(shtNum);
		}

		public bool extractShtNumComps3(ShtNumber4 shtNumber, GroupCollection g)
		{
			Group grp;
			string grpName;
			int idx;

			try
			{
				foreach (KeyValuePair<int, ShtNumComps2> kvp in SheetNumComponents)
				{
					if (kvp.Value.ItemClass == ShtIdClass2.SC_IGNORE) continue;

					idx = kvp.Key;
					grpName = kvp.Value.GroupId;

					grp = g[grpName];

					if (idx > SI_SEP0 && grp.Value.IsVoid() || idx == SI_SUBIDENTIFIER)
					{
						shtNumber.SetShtIdType((idx - 2) / 2);

						if (idx != SI_SUBIDENTIFIER) break;
					}

					shtNumber.ShtNumComps[idx] = grp.Value;

					updateCompLength(idx, grp.Value);
				}
			}
			catch
			{
				return false;
			}

			return true;
		}


		/*  voided - do not use

		public bool ParseSheetName2(FileNameSheetPdf2 shtPdf2, string sheetName)
		{
			bool result = true;

			if (sheetName.IsVoid())
			{
				result = false;
			}
			else
			{
				Match match = shtFileNamePattern.Match(sheetName);

				if (match.Success)
				{
					// shtPdf2.FileType = FileTypeSheetPdf.SHEET_PDF;

					GroupCollection g = match.Groups;

					extractSheetInfo(shtPdf2, g);

					shtPdf2.SheetName = sheetName;
				}
				else
				{
					result = false;
				}
			}

			return result;
		}

		// private int typeModifier;

		internal void extractSheetInfo(FileNameSheetPdf2 shtPdf2, GroupCollection g)
		{
			shtPdf2.SheetNumber = g[CN_SHTNUM]?.Value ?? "";
			shtPdf2.SheetID = g[CN_SHTID]?.Value ?? "";
			shtPdf2.SheetTitle = g[CN_SHTTITLE]?.Value ?? "";

			extractSheetComps(shtPdf2, g);
		}

		private void extractSheetComps(FileNameSheetPdf2 shtPdf2, GroupCollection g)
		{
			Group grp;
			string grpName;
			int idx;

			foreach (KeyValuePair<int, ShtNumComps2> kvp in SheetNumComponents)
			{
				if (kvp.Value.ItemClass == ShtIdClass2.SC_IGNORE) continue;

				idx = kvp.Key;
				grpName = kvp.Value.GroupId;

				grp= g[grpName];

				if (idx > SI_SEP0 && grp.Value.IsVoid() || idx==SI_SUBIDENTIFIER)
				{
					shtPdf2.SetShtIdType((idx-2)/2);

					if (idx != SI_SUBIDENTIFIER) break;
				}

				shtPdf2.SheetComps[idx] = grp.Value;

			}
		}
		*/


		// public bool ParseSheetNumber1(ShtNumber4 shtNumberObj, string shtNum)
		// {
		// 	bool result = true;
		//
		// 	if (shtNum.IsVoid())
		// 	{
		// 		result = false;
		// 	}
		// 	else
		// 	{
		// 		Match match = shtFileNamePattern.Match(shtNum);
		//
		// 		if (match.Success)
		// 		{
		// 			GroupCollection g = match.Groups;
		//
		// 			extractShtNumComps3(shtNumberObj, g);
		//
		// 			shtNumberObj.OrigSheetNumber = g[CN_SHTNUM]?.Value ?? "";
		// 			shtNumberObj.OrigSheetID = g[CN_SHTID]?.Value ?? "";
		// 		}
		// 		else
		// 		{
		// 			result = false;
		// 		}
		// 	}
		//
		// 	return result;
		// }

	#endregion

	#region private methods

		private void updateCompLength(int idx, string compStr)
		{
			if (compStr.Length > CompLengths[idx]) CompLengths[idx]=compStr.Length;
		}

		private void initCompLengths(int count)
		{
			CompLengths = new List<int>(count);

			for (int i = 0; i < count; i++)
			{
				CompLengths.Add(-1);
			}
		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FileNameSheetParser4";
		}

	#endregion
	}


	// public class FileExtensionPdfClassifier : FileExtensionClassifier
	// {
	// 	public override string[] fileTypes { get; set; } = {"pdf"};
	// }
}