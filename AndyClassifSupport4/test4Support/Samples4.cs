#region + Using Directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndyShared.FileSupport.FileNameSheetPDF;
using UtilityLibrary;
using AndyShared.FileSupport.FileNameSheetPDF4;
using AndyShared.SampleFileSupport;
using Test4.Support;
// using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
using static AndyShared.FileSupport.FileNameSheetPDF4.SheetIdentifiers4;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers.ShtIdType2;

#endregion

// user name: jeffs
// created:   9/8/2024 5:57:35 PM

namespace Test4.Support
{

		public class SheetPdfSample
	{
		public FilePath<FileNameSheetPdf4> SheetPdf4 { get; set; }

		public ShtNumber4 ShtNumber { get; set; }

		internal List<string> SheetComps { get; set; }

		public string FileName { get; set; }

		public string SampleSheet { get; set; }
		public string SheetNumber  { get; set; }
		public string SheetTitle   { get; set; }

		public FileNameSheetIdentifiers.ShtIdType2 SheetType { get; set; }

		public SheetPdfSample(
			string sampleFile,
			string sheetNumber,
			string sheetTitle,
			FileNameSheetIdentifiers.ShtIdType2 type,
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

			SheetComps[SI_PHBLDG       ]    = phaseBldg      ;
			SheetComps[SI_PBSEP        ]    = phaseBldgSep   ;
			SheetComps[SI_DISCIPLINE   ]    = discipline     ;
			SheetComps[SI_SEP0         ]    = seperator0     ;
			SheetComps[SI_CATEGORY     ]    = category       ;
			SheetComps[SI_SEP1         ]    = seperator1     ;
			SheetComps[SI_SUBCATEGORY  ]    = subcategory    ;
			SheetComps[SI_SEP2         ]    = seperator2     ;
			SheetComps[SI_MODIFIER     ]    = modifier       ;
			SheetComps[SI_SEP3         ]    = seperator3     ;
			SheetComps[SI_SUBMODIFIER  ]    = submodifier    ;
			SheetComps[SI_SEP4         ]    = seperator4     ;
			SheetComps[SI_IDENTIFIER   ]    = identifier     ;
			SheetComps[SI_SEP5         ]    = seperator5     ;
			SheetComps[SI_SUBIDENTIFIER]    = subidentifier  ;
			// ShtNumComps[VI_SEP6]  =seperator6      ;
		}

		private void initSheetComps()
		{
			SheetComps = new List<string>();

			for (int i = 0; i < SI_MAX; i++)
			{
				SheetComps.Add("");
			}
		}
	}



	public class Samples4
	{

		static Samples4() { }

		public static List<SheetPdfSample> Sheets { get; set; }

		public static SheetFileList4 fileList { get; set; }

		public static void MakeSamples()
		{
			Sheets = new List<SheetPdfSample>();

			Sheets.Add(new SheetPdfSample("CS - sheet title (0).pdf", "CS", "sheet title (0)", ST_TYPE01, "", " ", "CS",
				"", "", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T1.0-0 - sheet title (1).pdf", "T1.0-0", "sheet title (1)", ST_TYPE03, "",
				" ", "T", "", "1", ".", "0", "-", "0", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T2.0 - sheet title (2).pdf", "T2.0", "sheet title (2)", ST_TYPE02, "", " ",
				"T", "", "2", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T2.1 - sheet title (3).pdf", "T2.1", "sheet title (3)", ST_TYPE02, "", " ",
				"T", "", "2", ".", "1", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T3.0 - sheet title (4).pdf", "T3.0", "sheet title (4)", ST_TYPE02, "", " ",
				"T", "", "3", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T3.1 - sheet title (5).pdf", "T3.1", "sheet title (5)", ST_TYPE02, "", " ",
				"T", "", "3", ".", "1", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T3.2 - sheet title (6).pdf", "T3.2", "sheet title (6)", ST_TYPE02, "", " ",
				"T", "", "3", ".", "2", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T3.3 - sheet title (7).pdf", "T3.3", "sheet title (7)", ST_TYPE02, "", " ",
				"T", "", "3", ".", "3", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T3.4 - sheet title (8).pdf", "T3.4", "sheet title (8)", ST_TYPE02, "", " ",
				"T", "", "3", ".", "4", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T4.0 - sheet title (9).pdf", "T4.0", "sheet title (9)", ST_TYPE02, "", " ",
				"T", "", "4", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T4.1 - sheet title (10).pdf", "T4.1", "sheet title (10)", ST_TYPE02, "", " ",
				"T", "", "4", ".", "1", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T4.2 - sheet title (11).pdf", "T4.2", "sheet title (11)", ST_TYPE02, "", " ",
				"T", "", "4", ".", "2", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T4.3 - sheet title (12).pdf", "T4.3", "sheet title (12)", ST_TYPE02, "", " ",
				"T", "", "4", ".", "3", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T4.4 - sheet title (13).pdf", "T4.4", "sheet title (13)", ST_TYPE02, "", " ",
				"T", "", "4", ".", "4", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T4.5 - sheet title (14).pdf", "T4.5", "sheet title (14)", ST_TYPE02, "", " ",
				"T", "", "4", ".", "5", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T4.6 - sheet title (15).pdf", "T4.6", "sheet title (15)", ST_TYPE02, "", " ",
				"T", "", "4", ".", "6", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T5.0 - sheet title (16).pdf", "T5.0", "sheet title (16)", ST_TYPE02, "", " ",
				"T", "", "5", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T5.1 - sheet title (17).pdf", "T5.1", "sheet title (17)", ST_TYPE02, "", " ",
				"T", "", "5", ".", "1", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T5.2 - sheet title (18).pdf", "T5.2", "sheet title (18)", ST_TYPE02, "", " ",
				"T", "", "5", ".", "2", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T5.3-1 - sheet title (19).pdf", "T5.3-1", "sheet title (19)", ST_TYPE03, "",
				" ", "T", "", "5", ".", "3", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("T5.3-2 - sheet title (20).pdf", "T5.3-2", "sheet title (20)", ST_TYPE03, "",
				" ", "T", "", "5", ".", "3", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C0.0-1 - sheet title (21).pdf", "C0.0-1", "sheet title (21)", ST_TYPE03, "",
				" ", "C", "", "0", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C1.0-1 - sheet title (22).pdf", "C1.0-1", "sheet title (22)", ST_TYPE03, "",
				" ", "C", "", "1", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C2.0-1 - sheet title (23).pdf", "C2.0-1", "sheet title (23)", ST_TYPE03, "",
				" ", "C", "", "2", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C2.1-1 - sheet title (24).pdf", "C2.1-1", "sheet title (24)", ST_TYPE03, "",
				" ", "C", "", "2", ".", "1", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C2.2-1 - sheet title (25).pdf", "C2.2-1", "sheet title (25)", ST_TYPE03, "",
				" ", "C", "", "2", ".", "2", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C2.2-2 - sheet title (26).pdf", "C2.2-2", "sheet title (26)", ST_TYPE03, "",
				" ", "C", "", "2", ".", "2", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C3.0-1 - sheet title (27).pdf", "C3.0-1", "sheet title (27)", ST_TYPE03, "",
				" ", "C", "", "3", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C3.1-1 - sheet title (28).pdf", "C3.1-1", "sheet title (28)", ST_TYPE03, "",
				" ", "C", "", "3", ".", "1", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C4.0-1 - sheet title (29).pdf", "C4.0-1", "sheet title (29)", ST_TYPE03, "",
				" ", "C", "", "4", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C4.0-1 - sheet title (30).pdf", "C4.0-1", "sheet title (30)", ST_TYPE03, "",
				" ", "C", "", "4", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("C4.1-1 - sheet title (31).pdf", "C4.1-1", "sheet title (31)", ST_TYPE03, "",
				" ", "C", "", "4", ".", "1", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L0.0 - sheet title (32).pdf", "L0.0", "sheet title (32)", ST_TYPE02, "", " ",
				"L", "", "0", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L1.10 - sheet title (33).pdf", "L1.10", "sheet title (33)", ST_TYPE02, "",
				" ", "L", "", "1", ".", "10", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L1.11 - sheet title (34).pdf", "L1.11", "sheet title (34)", ST_TYPE02, "",
				" ", "L", "", "1", ".", "11", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L1.12 - sheet title (35).pdf", "L1.12", "sheet title (35)", ST_TYPE02, "",
				" ", "L", "", "1", ".", "12", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L1.20 - sheet title (36).pdf", "L1.20", "sheet title (36)", ST_TYPE02, "",
				" ", "L", "", "1", ".", "20", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L1.21 - sheet title (37).pdf", "L1.21", "sheet title (37)", ST_TYPE02, "",
				" ", "L", "", "1", ".", "21", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L1.22 - sheet title (38).pdf", "L1.22", "sheet title (38)", ST_TYPE02, "",
				" ", "L", "", "1", ".", "22", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L1.50 - sheet title (39).pdf", "L1.50", "sheet title (39)", ST_TYPE02, "",
				" ", "L", "", "1", ".", "50", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L1.51 - sheet title (40).pdf", "L1.51", "sheet title (40)", ST_TYPE02, "",
				" ", "L", "", "1", ".", "51", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L1.52 - sheet title (41).pdf", "L1.52", "sheet title (41)", ST_TYPE02, "",
				" ", "L", "", "1", ".", "52", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L3.00 - sheet title (42).pdf", "L3.00", "sheet title (42)", ST_TYPE02, "",
				" ", "L", "", "3", ".", "00", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L3.01 - sheet title (43).pdf", "L3.01", "sheet title (43)", ST_TYPE02, "",
				" ", "L", "", "3", ".", "01", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L5.00 - sheet title (44).pdf", "L5.00", "sheet title (44)", ST_TYPE02, "",
				" ", "L", "", "5", ".", "00", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L8.00 - sheet title (45).pdf", "L8.00", "sheet title (45)", ST_TYPE02, "",
				" ", "L", "", "8", ".", "00", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L8.11 - sheet title (46).pdf", "L8.11", "sheet title (46)", ST_TYPE02, "",
				" ", "L", "", "8", ".", "11", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L8.12 - sheet title (47).pdf", "L8.12", "sheet title (47)", ST_TYPE02, "",
				" ", "L", "", "8", ".", "12", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L8.20 - sheet title (48).pdf", "L8.20", "sheet title (48)", ST_TYPE02, "",
				" ", "L", "", "8", ".", "20", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L8.21 - sheet title (49).pdf", "L8.21", "sheet title (49)", ST_TYPE02, "",
				" ", "L", "", "8", ".", "21", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L8.22 - sheet title (50).pdf", "L8.22", "sheet title (50)", ST_TYPE02, "",
				" ", "L", "", "8", ".", "22", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L8.50 - sheet title (51).pdf", "L8.50", "sheet title (51)", ST_TYPE02, "",
				" ", "L", "", "8", ".", "50", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L8.51 - sheet title (52).pdf", "L8.51", "sheet title (52)", ST_TYPE02, "",
				" ", "L", "", "8", ".", "51", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L8.52 - sheet title (53).pdf", "L8.52", "sheet title (53)", ST_TYPE02, "",
				" ", "L", "", "8", ".", "52", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("L9.00 - sheet title (54).pdf", "L9.00", "sheet title (54)", ST_TYPE02, "",
				" ", "L", "", "9", ".", "00", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-G - sheet title (55).pdf", "LS2.2-G", "sheet title (55)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "G", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-M - sheet title (56).pdf", "LS2.2-M", "sheet title (56)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "M", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-P1 - sheet title (57).pdf", "LS2.2-P1", "sheet title (57)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "P1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-P2 - sheet title (58).pdf", "LS2.2-P2", "sheet title (58)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "P2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-P3 - sheet title (59).pdf", "LS2.2-P3", "sheet title (59)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "P3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-P4 - sheet title (60).pdf", "LS2.2-P4", "sheet title (60)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "P4", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-P5 - sheet title (61).pdf", "LS2.2-P5", "sheet title (61)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "P5", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-R1 - sheet title (62).pdf", "LS2.2-R1", "sheet title (62)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "R1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-R2 - sheet title (63).pdf", "LS2.2-R2", "sheet title (63)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "R2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-R3 - sheet title (64).pdf", "LS2.2-R3", "sheet title (64)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "R3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-R4 - sheet title (65).pdf", "LS2.2-R4", "sheet title (65)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "R4", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-R5 - sheet title (66).pdf", "LS2.2-R5", "sheet title (66)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "R5", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS2.2-R6 - sheet title (67).pdf", "LS2.2-R6", "sheet title (67)", ST_TYPE03,
				"", " ", "LS", "", "2", ".", "2", "-", "R6", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("LS3.0-1 - sheet title (68).pdf", "LS3.0-1", "sheet title (68)", ST_TYPE03,
				"", " ", "LS", "", "3", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A0.1-1 - sheet title (69).pdf", "A0.1-1", "sheet title (69)", ST_TYPE03, "",
				" ", "A", "", "0", ".", "1", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A0.2-1 - sheet title (70).pdf", "A0.2-1", "sheet title (70)", ST_TYPE03, "",
				" ", "A", "", "0", ".", "2", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A0.3-1 - sheet title (71).pdf", "A0.3-1", "sheet title (71)", ST_TYPE03, "",
				" ", "A", "", "0", ".", "3", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.0-0 - sheet title (72).pdf", "A1.0-0", "sheet title (72)", ST_TYPE03, "",
				" ", "A", "", "1", ".", "0", "-", "0", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.1-0 - sheet title (73).pdf", "A1.1-0", "sheet title (73)", ST_TYPE03, "",
				" ", "A", "", "1", ".", "1", "-", "0", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.1-1 - sheet title (74).pdf", "A1.1-1", "sheet title (74)", ST_TYPE03, "",
				" ", "A", "", "1", ".", "1", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A1.1-2 - sheet title (75).pdf", "A1.1-2", "sheet title (75)", ST_TYPE03, "",
				" ", "A", "", "1", ".", "1", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-1 - sheet title (76).pdf", "A2.2-1", "sheet title (76)", ST_TYPE03, "",
				" ", "A", "", "2", ".", "2", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-1S - sheet title (77).pdf", "A2.2-1S", "sheet title (77)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "1S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-1N - sheet title (78).pdf", "A2.2-1N", "sheet title (78)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "1N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-M - sheet title (79).pdf", "A2.2-M", "sheet title (79)", ST_TYPE03, "",
				" ", "A", "", "2", ".", "2", "-", "M", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-MS - sheet title (80).pdf", "A2.2-MS", "sheet title (80)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "MS", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-MN - sheet title (81).pdf", "A2.2-MN", "sheet title (81)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "MN", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P1 - sheet title (82).pdf", "A2.2-P1", "sheet title (82)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P1S - sheet title (83).pdf", "A2.2-P1S", "sheet title (83)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P1S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P1N - sheet title (84).pdf", "A2.2-P1N", "sheet title (84)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P1N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P2 - sheet title (85).pdf", "A2.2-P2", "sheet title (85)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P2S - sheet title (86).pdf", "A2.2-P2S", "sheet title (86)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P2S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P2N - sheet title (87).pdf", "A2.2-P2N", "sheet title (87)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P2N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P3 - sheet title (88).pdf", "A2.2-P3", "sheet title (88)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P3S - sheet title (89).pdf", "A2.2-P3S", "sheet title (89)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P3S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P3N - sheet title (90).pdf", "A2.2-P3N", "sheet title (90)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P3N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P4 - sheet title (91).pdf", "A2.2-P4", "sheet title (91)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P4", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P4S - sheet title (92).pdf", "A2.2-P4S", "sheet title (92)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P4S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P4N - sheet title (93).pdf", "A2.2-P4N", "sheet title (93)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P4N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P5 - sheet title (94).pdf", "A2.2-P5", "sheet title (94)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P5", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P5S - sheet title (95).pdf", "A2.2-P5S", "sheet title (95)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P5S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-P5N - sheet title (96).pdf", "A2.2-P5N", "sheet title (96)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "P5N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-A1 - sheet title (97).pdf", "A2.2-A1", "sheet title (97)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "A1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-A1S - sheet title (98).pdf", "A2.2-A1S", "sheet title (98)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "A1S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-A1N - sheet title (99).pdf", "A2.2-A1N", "sheet title (99)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "A1N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-A2 - sheet title (100).pdf", "A2.2-A2", "sheet title (100)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "A2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-A2S - sheet title (101).pdf", "A2.2-A2S", "sheet title (101)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "A2S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-A2N - sheet title (102).pdf", "A2.2-A2N", "sheet title (102)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "A2N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R1 - sheet title (103).pdf", "A2.2-R1", "sheet title (103)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "R1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R1S - sheet title (104).pdf", "A2.2-R1S", "sheet title (104)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R1S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R1N - sheet title (105).pdf", "A2.2-R1N", "sheet title (105)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R1N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R2 - sheet title (106).pdf", "A2.2-R2", "sheet title (106)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "R2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R2S - sheet title (107).pdf", "A2.2-R2S", "sheet title (107)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R2S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R2N - sheet title (108).pdf", "A2.2-R2N", "sheet title (108)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R2N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R3 - sheet title (109).pdf", "A2.2-R3", "sheet title (109)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "R3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R3S - sheet title (110).pdf", "A2.2-R3S", "sheet title (110)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R3S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R3N - sheet title (111).pdf", "A2.2-R3N", "sheet title (111)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R3N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R4 - sheet title (112).pdf", "A2.2-R4", "sheet title (112)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "R4", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R4S - sheet title (113).pdf", "A2.2-R4S", "sheet title (113)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R4S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R4N - sheet title (114).pdf", "A2.2-R4N", "sheet title (114)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R4N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R5 - sheet title (115).pdf", "A2.2-R5", "sheet title (115)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "R5", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R5S - sheet title (116).pdf", "A2.2-R5S", "sheet title (116)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R5S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R5N - sheet title (117).pdf", "A2.2-R5N", "sheet title (117)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R5N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R6 - sheet title (118).pdf", "A2.2-R6", "sheet title (118)", ST_TYPE03,
				"", " ", "A", "", "2", ".", "2", "-", "R6", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R6S - sheet title (119).pdf", "A2.2-R6S", "sheet title (119)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R6S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A2.2-R6N - sheet title (120).pdf", "A2.2-R6N", "sheet title (120)",
				ST_TYPE03, "", " ", "A", "", "2", ".", "2", "-", "R6N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A3.0-1 - sheet title (121).pdf", "A3.0-1", "sheet title (121)", ST_TYPE03,
				"", " ", "A", "", "3", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A3.0-2 - sheet title (122).pdf", "A3.0-2", "sheet title (122)", ST_TYPE03,
				"", " ", "A", "", "3", ".", "0", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A3.0-3 - sheet title (123).pdf", "A3.0-3", "sheet title (123)", ST_TYPE03,
				"", " ", "A", "", "3", ".", "0", "-", "3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A3.0-4 - sheet title (124).pdf", "A3.0-4", "sheet title (124)", ST_TYPE03,
				"", " ", "A", "", "3", ".", "0", "-", "4", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A3.2-1 - sheet title (125).pdf", "A3.2-1", "sheet title (125)", ST_TYPE03,
				"", " ", "A", "", "3", ".", "2", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A3.2-2 - sheet title (126).pdf", "A3.2-2", "sheet title (126)", ST_TYPE03,
				"", " ", "A", "", "3", ".", "2", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A3.2-3 - sheet title (127).pdf", "A3.2-3", "sheet title (127)", ST_TYPE03,
				"", " ", "A", "", "3", ".", "2", "-", "3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.0-0 - sheet title (128).pdf", "A5.0-0", "sheet title (128)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "0", "-", "0", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.0-1 - sheet title (129).pdf", "A5.0-1", "sheet title (129)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.0-2 - sheet title (130).pdf", "A5.0-2", "sheet title (130)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "0", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.0-3 - sheet title (131).pdf", "A5.0-3", "sheet title (131)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "0", "-", "3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.0-4 - sheet title (132).pdf", "A5.0-4", "sheet title (132)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "0", "-", "4", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.0-5 - sheet title (133).pdf", "A5.0-5", "sheet title (133)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "0", "-", "5", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.0-6 - sheet title (134).pdf", "A5.0-6", "sheet title (134)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "0", "-", "6", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.0-7 - sheet title (135).pdf", "A5.0-7", "sheet title (135)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "0", "-", "7", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.0-8 - sheet title (136).pdf", "A5.0-8", "sheet title (136)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "0", "-", "8", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.3-0 - sheet title (137).pdf", "A5.3-0", "sheet title (137)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "3", "-", "0", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.7-1 - sheet title (138).pdf", "A5.7-1", "sheet title (138)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "7", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.8-1 - sheet title (139).pdf", "A5.8-1", "sheet title (139)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "8", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.8-2 - sheet title (140).pdf", "A5.8-2", "sheet title (140)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "8", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.8-3 - sheet title (141).pdf", "A5.8-3", "sheet title (141)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "8", "-", "3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.8-4 - sheet title (142).pdf", "A5.8-4", "sheet title (142)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "8", "-", "4", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A5.8-5 - sheet title (143).pdf", "A5.8-5", "sheet title (143)", ST_TYPE03,
				"", " ", "A", "", "5", ".", "8", "-", "5", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A6.1-1 - sheet title (144).pdf", "A6.1-1", "sheet title (144)", ST_TYPE03,
				"", " ", "A", "", "6", ".", "1", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A6.1-2 - sheet title (145).pdf", "A6.1-2", "sheet title (145)", ST_TYPE03,
				"", " ", "A", "", "6", ".", "1", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A6.3-1 - sheet title (146).pdf", "A6.3-1", "sheet title (146)", ST_TYPE03,
				"", " ", "A", "", "6", ".", "3", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A6.3-2 - sheet title (147).pdf", "A6.3-2", "sheet title (147)", ST_TYPE03,
				"", " ", "A", "", "6", ".", "3", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A7.1-1 - sheet title (148).pdf", "A7.1-1", "sheet title (148)", ST_TYPE03,
				"", " ", "A", "", "7", ".", "1", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A7.3-1 - sheet title (149).pdf", "A7.3-1", "sheet title (149)", ST_TYPE03,
				"", " ", "A", "", "7", ".", "3", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A7.3-2 - sheet title (150).pdf", "A7.3-2", "sheet title (150)", ST_TYPE03,
				"", " ", "A", "", "7", ".", "3", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A8.0-1 - sheet title (151).pdf", "A8.0-1", "sheet title (151)", ST_TYPE03,
				"", " ", "A", "", "8", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("A8.0-2 - sheet title (152).pdf", "A8.0-2", "sheet title (152)", ST_TYPE03,
				"", " ", "A", "", "8", ".", "0", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH-1 - sheet title (153).pdf", "SH-1", "sheet title (153)", ST_TYPE01, "",
				" ", "SH", "-", "1", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH-1A - sheet title (154).pdf", "SH-1A", "sheet title (154)", ST_TYPE01, "",
				" ", "SH", "-", "1A", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH-1B - sheet title (155).pdf", "SH-1B", "sheet title (155)", ST_TYPE01, "",
				" ", "SH", "-", "1B", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH-2 - sheet title (156).pdf", "SH-2", "sheet title (156)", ST_TYPE01, "",
				" ", "SH", "-", "2", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH-2.1 - sheet title (157).pdf", "SH-2.1", "sheet title (157)", ST_TYPE02,
				"", " ", "SH", "-", "2", ".", "1", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH-3 - sheet title (158).pdf", "SH-3", "sheet title (158)", ST_TYPE01, "",
				" ", "SH", "-", "3", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH-4 - sheet title (159).pdf", "SH-4", "sheet title (159)", ST_TYPE01, "",
				" ", "SH", "-", "4", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH-5 - sheet title (160).pdf", "SH-5", "sheet title (160)", ST_TYPE01, "",
				" ", "SH", "-", "5", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH-6 - sheet title (161).pdf", "SH-6", "sheet title (161)", ST_TYPE01, "",
				" ", "SH", "-", "6", "", "", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("SH-11 - sheet title (162).pdf", "SH-11", "sheet title (162)", ST_TYPE01, "",
				" ", "SH", "-", "11", "", "", "", "", "", "", "", "", "", ""));

			Sheets.Add(new SheetPdfSample("S0.0-1 - sheet title (163).pdf", "S0.0-1", "sheet title (163)", ST_TYPE03,
				"", " ", "S", "", "0", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S0.0-2 - sheet title (164).pdf", "S0.0-2", "sheet title (164)", ST_TYPE03,
				"", " ", "S", "", "0", ".", "0", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S0.1-1 - sheet title (165).pdf", "S0.1-1", "sheet title (165)", ST_TYPE03,
				"", " ", "S", "", "0", ".", "1", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S0.1-2 - sheet title (166).pdf", "S0.1-2", "sheet title (166)", ST_TYPE03,
				"", " ", "S", "", "0", ".", "1", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S0.2-1 - sheet title (167).pdf", "S0.2-1", "sheet title (167)", ST_TYPE03,
				"", " ", "S", "", "0", ".", "2", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S0.3-1 - sheet title (168).pdf", "S0.3-1", "sheet title (168)", ST_TYPE03,
				"", " ", "S", "", "0", ".", "3", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S0.3-2 - sheet title (169).pdf", "S0.3-2", "sheet title (169)", ST_TYPE03,
				"", " ", "S", "", "0", ".", "3", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S0.4-1 - sheet title (170).pdf", "S0.4-1", "sheet title (170)", ST_TYPE03,
				"", " ", "S", "", "0", ".", "4", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S0.5-1 - sheet title (171).pdf", "S0.5-1", "sheet title (171)", ST_TYPE03,
				"", " ", "S", "", "0", ".", "5", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S0.5-2 - sheet title (172).pdf", "S0.5-2", "sheet title (172)", ST_TYPE03,
				"", " ", "S", "", "0", ".", "5", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.1-G - sheet title (173).pdf", "S2.1-G", "sheet title (173)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "1", "-", "G", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.1-M - sheet title (174).pdf", "S2.1-M", "sheet title (174)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "1", "-", "M", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.1-P1 - sheet title (175).pdf", "S2.1-P1", "sheet title (175)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "1", "-", "P1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.1-P2 - sheet title (176).pdf", "S2.1-P2", "sheet title (176)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "1", "-", "P2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.1-P3 - sheet title (177).pdf", "S2.1-P3", "sheet title (177)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "1", "-", "P3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.1-P4 - sheet title (178).pdf", "S2.1-P4", "sheet title (178)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "1", "-", "P4", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.1-P5 - sheet title (179).pdf", "S2.1-P5", "sheet title (179)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "1", "-", "P5", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.1-R - sheet title (180).pdf", "S2.1-R", "sheet title (180)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "1", "-", "R", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-1S - sheet title (181).pdf", "S2.2-1S", "sheet title (181)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "2", "-", "1S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-1SA - sheet title (182).pdf", "S2.2-1SA", "sheet title (182)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "1SA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-1SB - sheet title (183).pdf", "S2.2-1SB", "sheet title (183)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "1SB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-1SC - sheet title (184).pdf", "S2.2-1SC", "sheet title (184)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "1SC", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-1N - sheet title (185).pdf", "S2.2-1N", "sheet title (185)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "2", "-", "1N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-1NA - sheet title (186).pdf", "S2.2-1NA", "sheet title (186)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "1NA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-1NB - sheet title (187).pdf", "S2.2-1NB", "sheet title (187)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "1NB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-1NC - sheet title (188).pdf", "S2.2-1NC", "sheet title (188)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "1NC", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-MN - sheet title (189).pdf", "S2.2-MN", "sheet title (189)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "2", "-", "MN", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-MS - sheet title (190).pdf", "S2.2-MS", "sheet title (190)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "2", "-", "MS", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P1S - sheet title (191).pdf", "S2.2-P1S", "sheet title (191)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P1S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P1SA - sheet title (192).pdf", "S2.2-P1SA", "sheet title (192)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P1SA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P1SB - sheet title (193).pdf", "S2.2-P1SB", "sheet title (193)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P1SB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P1N - sheet title (194).pdf", "S2.2-P1N", "sheet title (194)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P1N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P1NA - sheet title (195).pdf", "S2.2-P1NA", "sheet title (195)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P1NA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P1NB - sheet title (196).pdf", "S2.2-P1NB", "sheet title (196)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P1NB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P2S - sheet title (197).pdf", "S2.2-P2S", "sheet title (197)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P2S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P2SA - sheet title (198).pdf", "S2.2-P2SA", "sheet title (198)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P2SA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P2SB - sheet title (199).pdf", "S2.2-P2SB", "sheet title (199)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P2SB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P2N - sheet title (200).pdf", "S2.2-P2N", "sheet title (200)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P2N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P2NA - sheet title (201).pdf", "S2.2-P2NA", "sheet title (201)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P2NA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P2NB - sheet title (202).pdf", "S2.2-P2NB", "sheet title (202)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P2NB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P3S - sheet title (203).pdf", "S2.2-P3S", "sheet title (203)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P3S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P3SA - sheet title (204).pdf", "S2.2-P3SA", "sheet title (204)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P3SA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P3SB - sheet title (205).pdf", "S2.2-P3SB", "sheet title (205)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P3SB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P3N - sheet title (206).pdf", "S2.2-P3N", "sheet title (206)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P3N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P3NA - sheet title (207).pdf", "S2.2-P3NA", "sheet title (207)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P3NA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P3NB - sheet title (208).pdf", "S2.2-P3NB", "sheet title (208)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P3NB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P4S - sheet title (209).pdf", "S2.2-P4S", "sheet title (209)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P4S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P4SA - sheet title (210).pdf", "S2.2-P4SA", "sheet title (210)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P4SA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P4SB - sheet title (211).pdf", "S2.2-P4SB", "sheet title (211)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P4SB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P4N - sheet title (212).pdf", "S2.2-P4N", "sheet title (212)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P4N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P4NA - sheet title (213).pdf", "S2.2-P4NA", "sheet title (213)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P4NA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P4NB - sheet title (214).pdf", "S2.2-P4NB", "sheet title (214)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P4NB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P5S - sheet title (215).pdf", "S2.2-P5S", "sheet title (215)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P5S", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-P5N - sheet title (216).pdf", "S2.2-P5N", "sheet title (216)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "P5N", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.3-RS - sheet title (217).pdf", "S2.3-RS", "sheet title (217)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "3", "-", "RS", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-RSA - sheet title (218).pdf", "S2.2-RSA", "sheet title (218)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "RSA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-RSB - sheet title (219).pdf", "S2.2-RSB", "sheet title (219)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "RSB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-RSC - sheet title (220).pdf", "S2.2-RSC", "sheet title (220)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "RSC", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.3-RN - sheet title (221).pdf", "S2.3-RN", "sheet title (221)", ST_TYPE03,
				"", " ", "S", "", "2", ".", "3", "-", "RN", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-RNA - sheet title (222).pdf", "S2.2-RNA", "sheet title (222)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "RNA", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-RNB - sheet title (223).pdf", "S2.2-RNB", "sheet title (223)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "RNB", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S2.2-RNC - sheet title (224).pdf", "S2.2-RNC", "sheet title (224)",
				ST_TYPE03, "", " ", "S", "", "2", ".", "2", "-", "RNC", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S3.0-1 - sheet title (225).pdf", "S3.0-1", "sheet title (225)", ST_TYPE03,
				"", " ", "S", "", "3", ".", "0", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S3.0-2 - sheet title (226).pdf", "S3.0-2", "sheet title (226)", ST_TYPE03,
				"", " ", "S", "", "3", ".", "0", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S3.1-1 - sheet title (227).pdf", "S3.1-1", "sheet title (227)", ST_TYPE03,
				"", " ", "S", "", "3", ".", "1", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S3.1-2 - sheet title (228).pdf", "S3.1-2", "sheet title (228)", ST_TYPE03,
				"", " ", "S", "", "3", ".", "1", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S3.1-3 - sheet title (229).pdf", "S3.1-3", "sheet title (229)", ST_TYPE03,
				"", " ", "S", "", "3", ".", "1", "-", "3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S3.2-1 - sheet title (230).pdf", "S3.2-1", "sheet title (230)", ST_TYPE03,
				"", " ", "S", "", "3", ".", "2", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S3.3-1 - sheet title (231).pdf", "S3.3-1", "sheet title (231)", ST_TYPE03,
				"", " ", "S", "", "3", ".", "3", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S4.1-1 - sheet title (232).pdf", "S4.1-1", "sheet title (232)", ST_TYPE03,
				"", " ", "S", "", "4", ".", "1", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S4.1-2 - sheet title (233).pdf", "S4.1-2", "sheet title (233)", ST_TYPE03,
				"", " ", "S", "", "4", ".", "1", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S4.1-3 - sheet title (234).pdf", "S4.1-3", "sheet title (234)", ST_TYPE03,
				"", " ", "S", "", "4", ".", "1", "-", "3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S4.2-1 - sheet title (235).pdf", "S4.2-1", "sheet title (235)", ST_TYPE03,
				"", " ", "S", "", "4", ".", "2", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S4.2-2 - sheet title (236).pdf", "S4.2-2", "sheet title (236)", ST_TYPE03,
				"", " ", "S", "", "4", ".", "2", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S4.2-3 - sheet title (237).pdf", "S4.2-3", "sheet title (237)", ST_TYPE03,
				"", " ", "S", "", "4", ".", "2", "-", "3", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S4.3-1 - sheet title (238).pdf", "S4.3-1", "sheet title (238)", ST_TYPE03,
				"", " ", "S", "", "4", ".", "3", "-", "1", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("S4.3-2 - sheet title (239).pdf", "S4.3-2", "sheet title (239)", ST_TYPE03,
				"", " ", "S", "", "4", ".", "3", "-", "2", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("EBM0.0 - sheet title (240).pdf", "EBM0.0", "sheet title (240)", ST_TYPE02,
				"", " ", "EBM", "", "0", ".", "0", "", "", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("EBM2.2-RF - sheet title (241).pdf", "EBM2.2-RF", "sheet title (241)",
				ST_TYPE03, "", " ", "EBM", "", "2", ".", "2", "-", "RF", "", "", "", "", "", ""));
			Sheets.Add(new SheetPdfSample("EBM9.0 - sheet title (242).pdf", "EBM9.0", "sheet title (242)", ST_TYPE02,
				"", " ", "EBM", "", "9", ".", "0", "", "", "", "", "", "", "", ""));
		}
	}
}