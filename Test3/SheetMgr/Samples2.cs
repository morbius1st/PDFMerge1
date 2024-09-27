#region + Using Directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;
using AndyShared.FileSupport.FileNameSheetPDF;
using Test3.FileNameSheetPDF;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
using static Test3.FileNameSheetPDF.SheetIdentifiers3;
using static Test3.FileNameSheetPDF.SheetIdentifiers3.ShtIdType2;

#endregion

// user name: jeffs
// created:   9/8/2024 5:57:35 PM

namespace Test3.SheetMgr
{
	public class SheetPdfSample
	{
		public FilePath<FileNameSheetPdf2> SheetPdf { get; set; }
		public FilePath<FileNameSheetPdf3> SheetPdf3 { get; set; }

		public ShtNumber ShtNumber { get; set; }

		internal List<string> SheetComps { get; set; }

		public string FileName { get; set; }

		public string SampleSheet { get; set; }
		public string SheetNumber  { get; set; }
		public string SheetTitle   { get; set; }

		public ShtIdType2 SheetType { get; set; }

		public SheetPdfSample(
			string sampleFile,
			string sheetNumber,
			string sheetTitle,
			ShtIdType2 type,
			string phaseBldg,
			string phaseBldgSep,
			string discipline,
			string seperator0,
			string category,
			string seperator1,
			string subcategory,
			string seperator2,
			string modifier,
			string seperator3,
			string submodifier,
			string seperator4,
			string identifier,
			string seperator5,
			string subidentifier
			)
		{
			initSheetComps();

			FileName = sampleFile;

			SampleSheet = Path.GetFileNameWithoutExtension( sampleFile );

			SheetNumber = sheetNumber ;
			SheetTitle  = sheetTitle  ;

			SheetType = type ;

			SheetComps[PHBLDG_VALUE_IDX]        = phaseBldg      ;
			SheetComps[PBSEP_VALUE_IDX]         = phaseBldgSep   ;
			SheetComps[DISCIPLINE_VALUE_IDX]    = discipline     ;
			SheetComps[SEP0_VALUE_IDX]          = seperator0     ;
			SheetComps[CATEGORY_VALUE_IDX]      = category       ;
			SheetComps[SEP1_VALUE_IDX]          = seperator1     ;
			SheetComps[SUBCATEGORY_VALUE_IDX]   = subcategory    ;
			SheetComps[SEP2_VALUE_IDX]          = seperator2     ;
			SheetComps[MODIFIER_VALUE_IDX]      = modifier       ;
			SheetComps[SEP3_VALUE_IDX]          = seperator3     ;
			SheetComps[SUBMODIFIER_VALUE_IDX]   = submodifier    ;
			SheetComps[SEP4_VALUE_IDX]          = seperator4     ;
			SheetComps[IDENTIFIER_VALUE_IDX]    = identifier     ;
			SheetComps[SEP5_VALUE_IDX]          = seperator5     ;
			SheetComps[SUBIDENTIFIER_VALUE_IDX] = subidentifier  ;
			// ShtNumComps[SEP6_VALUE_IDX]          =seperator6     ;
		}

		private void initSheetComps()
		{
			SheetComps = new List<string>();

			for (int i = 0; i < VALUE_IDX_COUNT; i++)
			{
				SheetComps.Add("");
			}
		}
	}

	public class Samples2
	{
		static Samples2() { }

		public static List<SheetPdfSample> Sheets { get; set; }

		public static void MakeSamples()
		{
			Sheets = new List<SheetPdfSample>();

			/*
			Sheets.Add(new SheetPdfSample(
				"This is a Test.pdf", "", "This is a Test.pdf",
				"", " ", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
			*/
			Sheets.Add(new SheetPdfSample("S1 - sheet title (0).pdf", "S1", "sheet title (0)", ST_TYPE01, "", " ", "S",
				"", "1", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S1.0 - sheet title (1).pdf", "S1.0", "sheet title (1)", ST_TYPE02, "", " ",
				"S", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH1 - sheet title (2).pdf", "SH1", "sheet title (2)", ST_TYPE01, "", " ",
				"SH", "", "1", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH1.0 - sheet title (3).pdf", "SH1.0", "sheet title (3)", ST_TYPE02, "", " ",
				"SH", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("GRN1 - sheet title (4).pdf", "GRN1", "sheet title (4)", ST_TYPE01, "", " ",
				"GRN", "", "1", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("GRN1.0 - sheet title (5).pdf", "GRN1.0", "sheet title (5)", ST_TYPE02, "",
				" ", "GRN", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("GRN-1 - sheet title (6).pdf", "GRN-1", "sheet title (6)", ST_TYPE01, "", " ",
				"GRN", "-", "1", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("GRN 1.0 - sheet title (7).pdf", "GRN 1.0", "sheet title (7)", ST_TYPE02, "",
				" ", "GRN", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T24-1 - sheet title (8).pdf", "T24-1", "sheet title (8)", ST_TYPE01, "", " ",
				"T24", "-", "1", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T24 1.0 - sheet title (9).pdf", "T24 1.0", "sheet title (9)", ST_TYPE02, "",
				" ", "T24", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("X30 1.0 - sheet title (10).pdf", "X30 1.0", "sheet title (10)", ST_TYPE02,
				"", " ", "X30", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("Z31 1.0 - sheet title (11).pdf", "Z31 1.0", "sheet title (11)", ST_TYPE02,
				"", " ", "Z31", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A S1.0 - sheet title (12).pdf", "A S1.0", "sheet title (12)", ST_TYPE12, "A",
				" ", "S", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A S1-0 - sheet title (13).pdf", "A S1-0", "sheet title (13)", ST_TYPE12, "A",
				" ", "S", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A S 1.0 - sheet title (14).pdf", "A S 1.0", "sheet title (14)", ST_TYPE12,
				"A", " ", "S", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A SH1.0 - sheet title (15).pdf", "A SH1.0", "sheet title (15)", ST_TYPE12,
				"A", " ", "SH", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A SH1-0 - sheet title (16).pdf", "A SH1-0", "sheet title (16)", ST_TYPE12,
				"A", " ", "SH", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A SH 1.0 - sheet title (17).pdf", "A SH 1.0", "sheet title (17)", ST_TYPE12,
				"A", " ", "SH", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A GRN1.0 - sheet title (18).pdf", "A GRN1.0", "sheet title (18)", ST_TYPE12,
				"A", " ", "GRN", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A GRN1-0 - sheet title (19).pdf", "A GRN1-0", "sheet title (19)", ST_TYPE12,
				"A", " ", "GRN", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A GRN 1.0 - sheet title (20).pdf", "A GRN 1.0", "sheet title (20)",
				ST_TYPE12, "A", " ", "GRN", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A T24 1.0 - sheet title (21).pdf", "A T24 1.0", "sheet title (21)",
				ST_TYPE12, "A", " ", "T24", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A T24 1-0 - sheet title (22).pdf", "A T24 1-0", "sheet title (22)",
				ST_TYPE12, "A", " ", "T24", " ", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A T24 1.0 - sheet title (23).pdf", "A T24 1.0", "sheet title (23)",
				ST_TYPE12, "A", " ", "T24", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A X30 1.0 - sheet title (24).pdf", "A X30 1.0", "sheet title (24)",
				ST_TYPE12, "A", " ", "X30", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A X30 1-0 - sheet title (25).pdf", "A X30 1-0", "sheet title (25)",
				ST_TYPE12, "A", " ", "X30", " ", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A X30 1.0 - sheet title (26).pdf", "A X30 1.0", "sheet title (26)",
				ST_TYPE12, "A", " ", "X30", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A Z31 1.0 - sheet title (27).pdf", "A Z31 1.0", "sheet title (27)",
				ST_TYPE12, "A", " ", "Z31", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A Z31 1-0 - sheet title (28).pdf", "A Z31 1-0", "sheet title (28)",
				ST_TYPE12, "A", " ", "Z31", " ", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A Z31 1.0 - sheet title (29).pdf", "A Z31 1.0", "sheet title (29)",
				ST_TYPE12, "A", " ", "Z31", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1 - sheet title (30).pdf", "A1", "sheet title (30)", ST_TYPE01, "", " ",
				"A", "", "1", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1a - sheet title (31).pdf", "A1a", "sheet title (31)", ST_TYPE01, "", " ",
				"A", "", "1a", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0 - sheet title (32).pdf", "A1.0", "sheet title (32)", ST_TYPE02, "", " ",
				"A", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0a - sheet title (33).pdf", "A1.0a", "sheet title (33)", ST_TYPE02, "",
				" ", "A", "", "1", ".", "0a", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1a - sheet title (34).pdf", "A1.0-1a", "sheet title (34)", ST_TYPE03,
				"", " ", "A", "", "1", ".", "0", "-", "1a", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1aN - sheet title (35).pdf", "A1.0-1aN", "sheet title (35)", ST_TYPE03,
				"", " ", "A", "", "1", ".", "0", "-", "1aN", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.a - sheet title (36).pdf", "A1.0-1.a", "sheet title (36)", ST_TYPE04,
				"", " ", "A", "", "1", ".", "0", "-", "1", ".", "a", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.aN - sheet title (37).pdf", "A1.0-1.aN", "sheet title (37)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "aN", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.a.b - sheet title (38).pdf", "A1.0-1.a.b", "sheet title (38)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "a", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.aN.b - sheet title (39).pdf", "A1.0-1.aN.b", "sheet title (39)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "aN", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.a.bN - sheet title (40).pdf", "A1.0-1.a.bN", "sheet title (40)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "a", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.aN.bN - sheet title (41).pdf", "A1.0-1.aN.bN", "sheet title (41)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "aN", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.a.b.c - sheet title (42).pdf", "A1.0-1.a.b.c", "sheet title (42)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "a", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1.aN.b.c - sheet title (43).pdf", "A1.0-1.aN.b.c", "sheet title (43)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "aN", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1.a.bN.c - sheet title (44).pdf", "A1.0-1.a.bN.c", "sheet title (44)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "a", ".", "bN", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1.aN.bN.c - sheet title (45).pdf", "A1.0-1.aN.bN.c", "sheet title (45)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "aN", ".", "bN", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1.A - sheet title (46).pdf", "A1.0-1.A", "sheet title (46)", ST_TYPE04,
				"", " ", "A", "", "1", ".", "0", "-", "1", ".", "A", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.AN - sheet title (47).pdf", "A1.0-1.AN", "sheet title (47)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "AN", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.A.b - sheet title (48).pdf", "A1.0-1.A.b", "sheet title (48)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "A", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.AN.b - sheet title (49).pdf", "A1.0-1.AN.b", "sheet title (49)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "AN", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.A.bN - sheet title (50).pdf", "A1.0-1.A.bN", "sheet title (50)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "A", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.AN.bN - sheet title (51).pdf", "A1.0-1.AN.bN", "sheet title (51)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "AN", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.A.b.c - sheet title (52).pdf", "A1.0-1.A.b.c", "sheet title (52)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "A", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1.AN.b.c - sheet title (53).pdf", "A1.0-1.AN.b.c", "sheet title (53)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "AN", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1.A.bN.c - sheet title (54).pdf", "A1.0-1.A.bN.c", "sheet title (54)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "A", ".", "bN", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1.AN.bN.c - sheet title (55).pdf", "A1.0-1.AN.bN.c", "sheet title (55)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "AN", ".", "bN", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1a.A - sheet title (56).pdf", "A1.0-1a.A", "sheet title (56)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", "-", "1a", ".", "A", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1a.AN - sheet title (57).pdf", "A1.0-1a.AN", "sheet title (57)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", "-", "1a", ".", "AN", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1a.A.b - sheet title (58).pdf", "A1.0-1a.A.b", "sheet title (58)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1a", ".", "A", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1a.AN.b - sheet title (59).pdf", "A1.0-1a.AN.b", "sheet title (59)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1a", ".", "AN", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1a.A.bN - sheet title (60).pdf", "A1.0-1a.A.bN", "sheet title (60)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1a", ".", "A", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1a.AN.bN - sheet title (61).pdf", "A1.0-1a.AN.bN", "sheet title (61)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1a", ".", "AN", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1a.A.b.c - sheet title (62).pdf", "A1.0-1a.A.b.c", "sheet title (62)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1a", ".", "A", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1a.AN.b.c - sheet title (63).pdf", "A1.0-1a.AN.b.c", "sheet title (63)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1a", ".", "AN", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1a.A.bN.c - sheet title (64).pdf", "A1.0-1a.A.bN.c", "sheet title (64)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1a", ".", "A", ".", "bN", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-1a.AN.bN.c - sheet title (65).pdf", "A1.0-1a.AN.bN.c",
				"sheet title (65)", ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1a", ".", "AN", ".", "bN", ".",
				"c"));
			Sheets.Add(new SheetPdfSample("A1.0-12.A - sheet title (66).pdf", "A1.0-12.A", "sheet title (66)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "A", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-12.AN - sheet title (67).pdf", "A1.0-12.AN", "sheet title (67)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "AN", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-12.A.b - sheet title (68).pdf", "A1.0-12.A.b", "sheet title (68)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "A", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-12.AN.b - sheet title (69).pdf", "A1.0-12.AN.b", "sheet title (69)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "AN", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-12.A.bN - sheet title (70).pdf", "A1.0-12.A.bN", "sheet title (70)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "A", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-12.AN.bN - sheet title (71).pdf", "A1.0-12.AN.bN", "sheet title (71)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "AN", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-12.A.b.c - sheet title (72).pdf", "A1.0-12.A.b.c", "sheet title (72)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "A", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-12.AN.b.c - sheet title (73).pdf", "A1.0-12.AN.b.c", "sheet title (73)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "AN", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-12.A.bN.c - sheet title (74).pdf", "A1.0-12.A.bN.c", "sheet title (74)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "A", ".", "bN", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-12.AN.bN.c - sheet title (75).pdf", "A1.0-12.AN.bN.c",
				"sheet title (75)", ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "AN", ".", "bN", ".",
				"c"));
			Sheets.Add(new SheetPdfSample("A1.0-A1.A - sheet title (76).pdf", "A1.0-A1.A", "sheet title (76)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", "-", "A1", ".", "A", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-A1.AN - sheet title (77).pdf", "A1.0-A1.AN", "sheet title (77)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", "-", "A1", ".", "AN", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-A1.A.b - sheet title (78).pdf", "A1.0-A1.A.b", "sheet title (78)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "A1", ".", "A", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-A1.AN.b - sheet title (79).pdf", "A1.0-A1.AN.b", "sheet title (79)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "A1", ".", "AN", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-A1.A.bN - sheet title (80).pdf", "A1.0-A1.A.bN", "sheet title (80)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "A1", ".", "A", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-A1.AN.bN - sheet title (81).pdf", "A1.0-A1.AN.bN", "sheet title (81)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "A1", ".", "AN", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-A1.A.b.c - sheet title (82).pdf", "A1.0-A1.A.b.c", "sheet title (82)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "A1", ".", "A", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-A1.AN.b.c - sheet title (83).pdf", "A1.0-A1.AN.b.c", "sheet title (83)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "A1", ".", "AN", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-A1.A.bN.c - sheet title (84).pdf", "A1.0-A1.A.bN.c", "sheet title (84)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "A1", ".", "A", ".", "bN", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-A1.AN.bN.c - sheet title (85).pdf", "A1.0-A1.AN.bN.c",
				"sheet title (85)", ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "A1", ".", "AN", ".", "bN", ".",
				"c"));
			Sheets.Add(new SheetPdfSample("A11.0-12.A - sheet title (86).pdf", "A11.0-12.A", "sheet title (86)",
				ST_TYPE04, "", " ", "A", "", "11", ".", "0", "-", "12", ".", "A", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A111.0-12.AN - sheet title (87).pdf", "A111.0-12.AN", "sheet title (87)",
				ST_TYPE04, "", " ", "A", "", "111", ".", "0", "-", "12", ".", "AN", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1a.0-12.A.b - sheet title (88).pdf", "A1a.0-12.A.b", "sheet title (88)",
				ST_TYPE05, "", " ", "A", "", "1a", ".", "0", "-", "12", ".", "A", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1a.0-12.AN.b - sheet title (89).pdf", "A1a.0-12.AN.b", "sheet title (89)",
				ST_TYPE05, "", " ", "A", "", "1a", ".", "0", "-", "12", ".", "AN", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1aa.0-12.A.bN - sheet title (90).pdf", "A1aa.0-12.A.bN", "sheet title (90)",
				ST_TYPE05, "", " ", "A", "", "1aa", ".", "0", "-", "12", ".", "A", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.000-12.AN.bN - sheet title (91).pdf", "A1.000-12.AN.bN",
				"sheet title (91)", ST_TYPE05, "", " ", "A", "", "1", ".", "000", "-", "12", ".", "AN", ".", "bN", "",
				""));
			Sheets.Add(new SheetPdfSample("A1.0aa-12.A.b.c - sheet title (92).pdf", "A1.0aa-12.A.b.c",
				"sheet title (92)", ST_TYPE06, "", " ", "A", "", "1", ".", "0aa", "-", "12", ".", "A", ".", "b", ".",
				"c"));
			Sheets.Add(new SheetPdfSample("A1.0-12.AN.b.c - sheet title (93).pdf", "A1.0-12.AN.b.c", "sheet title (93)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "AN", ".", "b", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-12.A.bN.c - sheet title (94).pdf", "A1.0-12.A.bN.c", "sheet title (94)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "A", ".", "bN", ".", "c"));
			Sheets.Add(new SheetPdfSample("A1.0-12.AN.bN.c - sheet title (95).pdf", "A1.0-12.AN.bN.c",
				"sheet title (95)", ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "12", ".", "AN", ".", "bN", ".",
				"c"));
			Sheets.Add(new SheetPdfSample("A1-0 - sheet title (96).pdf", "A1-0", "sheet title (96)", ST_TYPE02, "", " ",
				"A", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0a - sheet title (97).pdf", "A1-0a", "sheet title (97)", ST_TYPE02, "",
				" ", "A", "", "1", "-", "0a", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1a - sheet title (98).pdf", "A1-0-1a", "sheet title (98)", ST_TYPE03,
				"", " ", "A", "", "1", "-", "0", "-", "1a", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1aN - sheet title (99).pdf", "A1-0-1aN", "sheet title (99)", ST_TYPE03,
				"", " ", "A", "", "1", "-", "0", "-", "1aN", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1.a - sheet title (100).pdf", "A1-0-1.a", "sheet title (100)",
				ST_TYPE04, "", " ", "A", "", "1", "-", "0", "-", "1", ".", "a", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1.aN - sheet title (101).pdf", "A1-0-1.aN", "sheet title (101)",
				ST_TYPE04, "", " ", "A", "", "1", "-", "0", "-", "1", ".", "aN", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1.a.b - sheet title (102).pdf", "A1-0-1.a.b", "sheet title (102)",
				ST_TYPE05, "", " ", "A", "", "1", "-", "0", "-", "1", ".", "a", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1.aN.b - sheet title (103).pdf", "A1-0-1.aN.b", "sheet title (103)",
				ST_TYPE05, "", " ", "A", "", "1", "-", "0", "-", "1", ".", "aN", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1.a.bN - sheet title (104).pdf", "A1-0-1.a.bN", "sheet title (104)",
				ST_TYPE05, "", " ", "A", "", "1", "-", "0", "-", "1", ".", "a", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1.aN.bN - sheet title (105).pdf", "A1-0-1.aN.bN", "sheet title (105)",
				ST_TYPE05, "", " ", "A", "", "1", "-", "0", "-", "1", ".", "aN", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0.1a - sheet title (106).pdf", "A1.0.1a", "sheet title (106)", ST_TYPE03,
				"", " ", "A", "", "1", ".", "0", ".", "1a", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0.1aN - sheet title (107).pdf", "A1.0.1aN", "sheet title (107)",
				ST_TYPE03, "", " ", "A", "", "1", ".", "0", ".", "1aN", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0.1.a - sheet title (108).pdf", "A1.0.1.a", "sheet title (108)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", ".", "1", ".", "a", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0.1.aN - sheet title (109).pdf", "A1.0.1.aN", "sheet title (109)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", ".", "1", ".", "aN", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0.1.a.b - sheet title (110).pdf", "A1.0.1.a.b", "sheet title (110)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", ".", "1", ".", "a", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0.1.aN.b - sheet title (111).pdf", "A1.0.1.aN.b", "sheet title (111)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", ".", "1", ".", "aN", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0.1.a.bN - sheet title (112).pdf", "A1.0.1.a.bN", "sheet title (112)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", ".", "1", ".", "a", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0.1.aN.bN - sheet title (113).pdf", "A1.0.1.aN.bN", "sheet title (113)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", ".", "1", ".", "aN", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1-a - sheet title (114).pdf", "A1-0-1-a", "sheet title (114)",
				ST_TYPE04, "", " ", "A", "", "1", "-", "0", "-", "1", "-", "a", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1-aN - sheet title (115).pdf", "A1-0-1-aN", "sheet title (115)",
				ST_TYPE04, "", " ", "A", "", "1", "-", "0", "-", "1", "-", "aN", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1-a-b - sheet title (116).pdf", "A1-0-1-a-b", "sheet title (116)",
				ST_TYPE05, "", " ", "A", "", "1", "-", "0", "-", "1", "-", "a", "-", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1-aN-b - sheet title (117).pdf", "A1-0-1-aN-b", "sheet title (117)",
				ST_TYPE05, "", " ", "A", "", "1", "-", "0", "-", "1", "-", "aN", "-", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1-a-bN - sheet title (118).pdf", "A1-0-1-a-bN", "sheet title (118)",
				ST_TYPE05, "", " ", "A", "", "1", "-", "0", "-", "1", "-", "a", "-", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1-aN-bN - sheet title (119).pdf", "A1-0-1-aN-bN", "sheet title (119)",
				ST_TYPE05, "", " ", "A", "", "1", "-", "0", "-", "1", "-", "aN", "-", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A1-0-1-a-b-c - sheet title (120).pdf", "A1-0-1-a-b-c", "sheet title (120)",
				ST_TYPE06, "", " ", "A", "", "1", "-", "0", "-", "1", "-", "a", "-", "b", "-", "c"));
			Sheets.Add(new SheetPdfSample("A1-0-1-aN-b-c - sheet title (121).pdf", "A1-0-1-aN-b-c", "sheet title (121)",
				ST_TYPE06, "", " ", "A", "", "1", "-", "0", "-", "1", "-", "aN", "-", "b", "-", "c"));
			Sheets.Add(new SheetPdfSample("A1-0-1-a-bN-c - sheet title (122).pdf", "A1-0-1-a-bN-c", "sheet title (122)",
				ST_TYPE06, "", " ", "A", "", "1", "-", "0", "-", "1", "-", "a", "-", "bN", "-", "c"));
			Sheets.Add(new SheetPdfSample("A1-0-1-aN-bN-c - sheet title (123).pdf", "A1-0-1-aN-bN-c",
				"sheet title (123)", ST_TYPE06, "", " ", "A", "", "1", "-", "0", "-", "1", "-", "aN", "-", "bN", "-",
				"c"));
			Sheets.Add(new SheetPdfSample("A 1 - sheet title (124).pdf", "A 1", "sheet title (124)", ST_TYPE01, "", " ",
				"A", " ", "1", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A 1a - sheet title (125).pdf", "A 1a", "sheet title (125)", ST_TYPE01, "",
				" ", "A", " ", "1a", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A 1.0a - sheet title (126).pdf", "A 1.0a", "sheet title (126)", ST_TYPE02,
				"", " ", "A", " ", "1", ".", "0a", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A 1.0-1a - sheet title (127).pdf", "A 1.0-1a", "sheet title (127)",
				ST_TYPE03, "", " ", "A", " ", "1", ".", "0", "-", "1a", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A 1.0-1aN - sheet title (128).pdf", "A 1.0-1aN", "sheet title (128)",
				ST_TYPE03, "", " ", "A", " ", "1", ".", "0", "-", "1aN", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A 1.0-1.a - sheet title (129).pdf", "A 1.0-1.a", "sheet title (129)",
				ST_TYPE04, "", " ", "A", " ", "1", ".", "0", "-", "1", ".", "a", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A 1.0-1.aN - sheet title (130).pdf", "A 1.0-1.aN", "sheet title (130)",
				ST_TYPE04, "", " ", "A", " ", "1", ".", "0", "-", "1", ".", "aN", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A 1.0-1.a.b - sheet title (131).pdf", "A 1.0-1.a.b", "sheet title (131)",
				ST_TYPE05, "", " ", "A", " ", "1", ".", "0", "-", "1", ".", "a", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A 1.0-1.aN.b - sheet title (132).pdf", "A 1.0-1.aN.b", "sheet title (132)",
				ST_TYPE05, "", " ", "A", " ", "1", ".", "0", "-", "1", ".", "aN", ".", "b", "", ""));
			Sheets.Add(new SheetPdfSample("A 1.0-1.a.bN - sheet title (133).pdf", "A 1.0-1.a.bN", "sheet title (133)",
				ST_TYPE05, "", " ", "A", " ", "1", ".", "0", "-", "1", ".", "a", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A 1.0-1.aN.bN - sheet title (134).pdf", "A 1.0-1.aN.bN", "sheet title (134)",
				ST_TYPE05, "", " ", "A", " ", "1", ".", "0", "-", "1", ".", "aN", ".", "bN", "", ""));
			Sheets.Add(new SheetPdfSample("A100 - sheet title (135).pdf", "A100", "sheet title (135)", ST_TYPE01, "",
				" ", "A", "", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A100a - sheet title (136).pdf", "A100a", "sheet title (136)", ST_TYPE01, "",
				" ", "A", "", "100a", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A.100 - sheet title (137).pdf", "A.100", "sheet title (137)", ST_TYPE01, "",
				" ", "A", ".", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A.100a - sheet title (138).pdf", "A.100a", "sheet title (138)", ST_TYPE01,
				"", " ", "A", ".", "100a", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A-100 - sheet title (139).pdf", "A-100", "sheet title (139)", ST_TYPE01, "",
				" ", "A", "-", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A-100a - sheet title (140).pdf", "A-100a", "sheet title (140)", ST_TYPE01,
				"", " ", "A", "-", "100a", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A A1.0 - sheet title (141).pdf", "A A1.0", "sheet title (141)", ST_TYPE12,
				"A", " ", "A", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A A1-0 - sheet title (142).pdf", "A A1-0", "sheet title (142)", ST_TYPE12,
				"A", " ", "A", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A A 1.0 - sheet title (143).pdf", "A A 1.0", "sheet title (143)", ST_TYPE12,
				"A", " ", "A", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A A100 - sheet title (144).pdf", "A A100", "sheet title (144)", ST_TYPE11,
				"A", " ", "A", "", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A A.100 - sheet title (145).pdf", "A A.100", "sheet title (145)", ST_TYPE11,
				"A", " ", "A", ".", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A A-100 - sheet title (146).pdf", "A A-100", "sheet title (146)", ST_TYPE11,
				"A", " ", "A", "-", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("AA A1.0 - sheet title (147).pdf", "AA A1.0", "sheet title (147)", ST_TYPE12,
				"AA", " ", "A", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("AA A1-0 - sheet title (148).pdf", "AA A1-0", "sheet title (148)", ST_TYPE12,
				"AA", " ", "A", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("AA A 1.0 - sheet title (149).pdf", "AA A 1.0", "sheet title (149)",
				ST_TYPE12, "AA", " ", "A", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("AA A100 - sheet title (150).pdf", "AA A100", "sheet title (150)", ST_TYPE11,
				"AA", " ", "A", "", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("AA A.100 - sheet title (151).pdf", "AA A.100", "sheet title (151)",
				ST_TYPE11, "AA", " ", "A", ".", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("AA A-100 - sheet title (152).pdf", "AA A-100", "sheet title (152)",
				ST_TYPE11, "AA", " ", "A", "-", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1 A1.0 - sheet title (153).pdf", "1 A1.0", "sheet title (153)", ST_TYPE12,
				"1", " ", "A", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1 A1-0 - sheet title (154).pdf", "1 A1-0", "sheet title (154)", ST_TYPE12,
				"1", " ", "A", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1 A 1.0 - sheet title (155).pdf", "1 A 1.0", "sheet title (155)", ST_TYPE12,
				"1", " ", "A", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1 A100 - sheet title (156).pdf", "1 A100", "sheet title (156)", ST_TYPE11,
				"1", " ", "A", "", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1 A.100 - sheet title (157).pdf", "1 A.100", "sheet title (157)", ST_TYPE11,
				"1", " ", "A", ".", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1 A-100 - sheet title (158).pdf", "1 A-100", "sheet title (158)", ST_TYPE11,
				"1", " ", "A", "-", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11 A1.0 - sheet title (159).pdf", "11 A1.0", "sheet title (159)", ST_TYPE12,
				"11", " ", "A", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11 A1-0 - sheet title (160).pdf", "11 A1-0", "sheet title (160)", ST_TYPE12,
				"11", " ", "A", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11 A 1.0 - sheet title (161).pdf", "11 A 1.0", "sheet title (161)",
				ST_TYPE12, "11", " ", "A", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11 A100 - sheet title (162).pdf", "11 A100", "sheet title (162)", ST_TYPE11,
				"11", " ", "A", "", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11 A.100 - sheet title (163).pdf", "11 A.100", "sheet title (163)",
				ST_TYPE11, "11", " ", "A", ".", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11 A-100 - sheet title (164).pdf", "11 A-100", "sheet title (164)",
				ST_TYPE11, "11", " ", "A", "-", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1A A1.0 - sheet title (165).pdf", "1A A1.0", "sheet title (165)", ST_TYPE12,
				"1A", " ", "A", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1A A1-0 - sheet title (166).pdf", "1A A1-0", "sheet title (166)", ST_TYPE12,
				"1A", " ", "A", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1A A 1.0 - sheet title (167).pdf", "1A A 1.0", "sheet title (167)",
				ST_TYPE12, "1A", " ", "A", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1A A100 - sheet title (168).pdf", "1A A100", "sheet title (168)", ST_TYPE11,
				"1A", " ", "A", "", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1A A.100 - sheet title (169).pdf", "1A A.100", "sheet title (169)",
				ST_TYPE11, "1A", " ", "A", ".", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1A A-100 - sheet title (170).pdf", "1A A-100", "sheet title (170)",
				ST_TYPE11, "1A", " ", "A", "-", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1AA A1.0 - sheet title (171).pdf", "1AA A1.0", "sheet title (171)",
				ST_TYPE12, "1AA", " ", "A", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1AA A1-0 - sheet title (172).pdf", "1AA A1-0", "sheet title (172)",
				ST_TYPE12, "1AA", " ", "A", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1AA A 1.0 - sheet title (173).pdf", "1AA A 1.0", "sheet title (173)",
				ST_TYPE12, "1AA", " ", "A", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1AA A100 - sheet title (174).pdf", "1AA A100", "sheet title (174)",
				ST_TYPE11, "1AA", " ", "A", "", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1AA A.100 - sheet title (175).pdf", "1AA A.100", "sheet title (175)",
				ST_TYPE11, "1AA", " ", "A", ".", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("1AA A-100 - sheet title (176).pdf", "1AA A-100", "sheet title (176)",
				ST_TYPE11, "1AA", " ", "A", "-", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11A A1.0 - sheet title (177).pdf", "11A A1.0", "sheet title (177)",
				ST_TYPE12, "11A", " ", "A", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11A A1-0 - sheet title (178).pdf", "11A A1-0", "sheet title (178)",
				ST_TYPE12, "11A", " ", "A", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11A A 1.0 - sheet title (179).pdf", "11A A 1.0", "sheet title (179)",
				ST_TYPE12, "11A", " ", "A", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11A A100 - sheet title (180).pdf", "11A A100", "sheet title (180)",
				ST_TYPE11, "11A", " ", "A", "", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11A A.100 - sheet title (181).pdf", "11A A.100", "sheet title (181)",
				ST_TYPE11, "11A", " ", "A", ".", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11A A-100 - sheet title (182).pdf", "11A A-100", "sheet title (182)",
				ST_TYPE11, "11A", " ", "A", "-", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11AA A1.0 - sheet title (183).pdf", "11AA A1.0", "sheet title (183)",
				ST_TYPE12, "11AA", " ", "A", "", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11AA A1-0 - sheet title (184).pdf", "11AA A1-0", "sheet title (184)",
				ST_TYPE12, "11AA", " ", "A", "", "1", "-", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11AA A 1.0 - sheet title (185).pdf", "11AA A 1.0", "sheet title (185)",
				ST_TYPE12, "11AA", " ", "A", " ", "1", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11AA A100 - sheet title (186).pdf", "11AA A100", "sheet title (186)",
				ST_TYPE11, "11AA", " ", "A", "", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11AA A.100 - sheet title (187).pdf", "11AA A.100", "sheet title (187)",
				ST_TYPE11, "11AA", " ", "A", ".", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("11AA A-100 - sheet title (188).pdf", "11AA A-100", "sheet title (188)",
				ST_TYPE11, "11AA", " ", "A", "-", "100", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1 1- sheet title (189).pdf", "A1 1", "sheet title (189)", ST_TYPE01, "",
				" ", "A", "", "1 1", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1a+1- sheet title (190).pdf", "A1a+1", "sheet title (190)", ST_TYPE01, "",
				" ", "A", "", "1a+1", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0#1- sheet title (191).pdf", "A1.0#1", "sheet title (191)", ST_TYPE02, "",
				" ", "A", "", "1", ".", "0#1", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0a 1- sheet title (192).pdf", "A1.0a 1", "sheet title (192)", ST_TYPE02,
				"", " ", "A", "", "1", ".", "0a 1", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1a+1 -sheet title (193).pdf", "A1.0-1a+1", "sheet title (193)",
				ST_TYPE03, "", " ", "A", "", "1", ".", "0", "-", "1a+1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1aN#1 -sheet title (194).pdf", "A1.0-1aN#1", "sheet title (194)",
				ST_TYPE03, "", " ", "A", "", "1", ".", "0", "-", "1aN#", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.a 1 -sheet title (195).pdf", "A1.0-1.a 1", "sheet title (195)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "a 1", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.aN#1 -sheet title (196).pdf", "A1.0-1.aN#1", "sheet title (196)",
				ST_TYPE04, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "aN#1", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.a.b+1sheet title (197).pdf", "A1.0-1.a.b+1", "sheet title (197)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "a", ".", "b+1", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.aN.b asheet title (198).pdf", "A1.0-1.aN.b a", "sheet title (198)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "aN", ".", "b a", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.a.bN#asheet title (199).pdf", "A1.0-1.a.bN#a", "sheet title (199)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "a", ".", "bN#a", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.aN.bN+asheet title (200).pdf", "A1.0-1.aN.bN+a", "sheet title (200)",
				ST_TYPE05, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "aN", ".", "bN+a", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-1.a.b.c A sheet title (201).pdf", "A1.0-1.a.b.c A", "sheet title (201)",
				ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "a", ".", "b", ".", "c A"));
			Sheets.Add(new SheetPdfSample("A1.0-1.aN.b.c#A sheet title (202).pdf", "A1.0-1.aN.b.c#A",
				"sheet title (202)", ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "aN", ".", "b", ".",
				"c#A"));
			Sheets.Add(new SheetPdfSample("A1.0-1.a.bN.c+A sheet title (203).pdf", "A1.0-1.a.bN.c+A",
				"sheet title (203)", ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "a", ".", "bN", ".",
				"c+A"));
			Sheets.Add(new SheetPdfSample("A1.0-1.aN.bN.c#A sheet title (204).pdf", "A1.0-1.aN.bN.c#A",
				"sheet title (204)", ST_TYPE06, "", " ", "A", "", "1", ".", "0", "-", "1", ".", "aN", ".", "bN", ".",
				"c#A"));
		}
	}
}