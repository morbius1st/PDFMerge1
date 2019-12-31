#region + Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;
using static Test3.UtilityLocal;



#endregion


// projname: Test3
// itemname: Samples
// username: jeffs
// created:  12/28/2019 6:51:01 AM


namespace Test3
{
	public class Samples
	{

		internal const string BASE_FOLDER             = @"C:\2099-999 Sample Project\Publish\Bulletins";

		internal const string NORMAL_FOLDER           = @"\2017-07-01 normal";
		internal const string PDF_IN_INDIV_PDF_FOLDER = @"\2017-07-01 not in indiv PDFs folder and an empty sub-folders";
		internal const string NO_PDFS                 = @"\2017-07-01 no PDFs";
		internal const string EMPTY_SUB_FOLDER        = @"\2017-07-01 with an empty sub-folders";
		internal const string CORRUPT_PDF             = @"\2017-07-01 with corrupted PDF";
		internal const string NON_PDF                 = @"\2017-07-01 with Non-PDF's";
		internal const string ROOT_PDFS               = @"\2017-07-01 with PDFs in root";
		internal const string NO_SUCH_FOLDER          = @"\2017-07-01";

		internal const string PDF_FOLDER              = @"\Individual PDFs";

//		private const string BASE_PATH                = @"\";
//
//		private const string BASE_COVER               = BASE_PATH + "00 Cover A\";
//		private const string BASE_GENERAL             = BASE_PATH + "00 General A\";
//		private const string BASE_CIVIL               = BASE_PATH + "01 Civil A\";
//		private const string BASE_ARCH                = BASE_PATH + "07 Architectural A\";
//
//		private const string COVER_SUB1               = BASE_COVER + @"00.1 Test Sub Dir 1.0\";
//		private const string COVER_SUB2               = BASE_COVER + @"00.2 Test Sub Dir 2.0\";
//
//		private const string COVER_SUB11              = COVER_SUB1 + @"00.1.1 Test Sub-Sub Dir 1.1\";
//		private const string COVER_SUB21              = COVER_SUB2 + @"00.2.1 Test Sub-Sub Dir 2.1\";
//
//		private const string ARCH_SUB2                = BASE_ARCH + @"07.2 Test Sub Dir 2.0\";
//		private const string ARCH_SUB3                = BASE_ARCH + @"07.3 Test Sub Dir 3.0\";

		private const string BASE_PATH                = "";
		private const string BASE_COVER               = "";
		private const string BASE_GENERAL             = "";
		private const string BASE_CIVIL               = "";
		private const string BASE_ARCH                = "";
		private const string COVER_SUB1               = "";
		private const string COVER_SUB2               = "";
		private const string COVER_SUB11              = "";
		private const string COVER_SUB21              = "";
		private const string ARCH_SUB2                = "";
		private const string ARCH_SUB3                = "";


		private const string FILE_X00                 = BASE_PATH + @"X X0.0 This is a Test X00.pdf";
		private const string FILE_X10                 = BASE_PATH + @"X X1.0 This is a Test X10.pdf";
		private const string FILE_X20                 = BASE_PATH + @"X X2.0 This is a Test X20.pdf";
		private const string FILE_XX30                = BASE_PATH + @"X X3.0 Corrupted X30.pdf";

		private const string FILE_CS10                = BASE_COVER + @"A CS1.0-0 This is a Test CS10.pdf";
		private const string FILE_CS11                = COVER_SUB1 + @"A CS1.1-0 This is a Test CS11.pdf";
		private const string FILE_CS111               = COVER_SUB11 + @"A CS1.1-1 This is a Test CS11.1.pdf";
		private const string FILE_CS21                = COVER_SUB2 + @"A CS2.1-0 This is a Test CS21.pdf";
		private const string FILE_CS211               = COVER_SUB21 + @"A CS2.1-1 This is a Test CS21.1.pdf";

		private const string FILE_T10                 = BASE_GENERAL + @"A T1.0-0 This is a Test T10.pdf";
		private const string FILE_T20                 = BASE_GENERAL + @"A T2.0-0 This is a Test T20.pdf";

		private const string FILE_C10                 = BASE_CIVIL + @"C1.0-0 This is a Test C10.pdf";
		private const string FILE_C20                 = BASE_CIVIL + @"C2.0-0 This is a Test C20.pdf";

		private const string FILE_A10                 = BASE_ARCH + @"A A1.0-0 This is a Test A10.pdf";

		private const string FILE_A20                 = ARCH_SUB2 + @"A A2.0-0 This is a Test A20.pdf";
		private const string FILE_A30                 = ARCH_SUB3 + @"A A3.0-0 This is a Test A30.pdf";
		private const string FILE_A31                 = ARCH_SUB3 + @"A A3.1-0 This is a Test A31.pdf";

		internal const string ROOT_FOLDER = BASE_FOLDER + ROOT_PDFS + PDF_FOLDER;

		internal static List<string> normal = new List<string>()
		{
			FILE_X00,
			FILE_X10,
			FILE_X20,
			FILE_CS10,
			FILE_CS11,
			FILE_CS111,
			FILE_CS21,
			FILE_CS211,
			FILE_T10,
			FILE_T20,
			FILE_C10,
			FILE_C20,
			FILE_A10,
			FILE_A20,
			FILE_A30,
			FILE_A31
		};

//
//		// "normal" listing - but with (1) corrupted file
//		internal const string OUTPUT_ORIG = BASE_FOLDER + @"\output-orig.pdf";
//
//		internal static List<string> orig = new List<string>()
//		{
//			FILE_X00,
//			FILE_X10,
//			FILE_X20,
//			FILE_XX30,
//			FILE_CS10,
//			FILE_CS11,
//			FILE_CS111,
//			FILE_CS21,
//			FILE_CS211,
//			FILE_T10,
//			FILE_T20,
//			FILE_C10,
//			FILE_C20,
//			FILE_A10,
//			FILE_A20,
//			FILE_A30,
//			FILE_A31
//		};
//
//		// "normal" listing with root level pdf's moved
//		internal const string OUTPUT_ALT1 = BASE_FOLDER + @"\output-alt1.pdf";
//
//		internal static List<string> alt1 = new List<string>()
//		{
//			FILE_X00,
//			FILE_CS10,
//			FILE_CS11,
//			FILE_CS111,
//			FILE_CS21,
//			FILE_CS211,
//			FILE_X10,
//			FILE_T10,
//			FILE_T20,
//			FILE_X20,
//			FILE_C10,
//			FILE_C20,
//			FILE_A10,
//			FILE_A20,
//			FILE_A30,
//			FILE_A31
//		};
//
//
//
//		// "normal" with (1) deep level PDF moved
//		internal const string OUTPUT_ALT2 = BASE_FOLDER + @"\output-alt2.pdf";
//
//		internal static List<string> alt2 = new List<string>()
//		{
//			FILE_X00,
//			FILE_X10,
//			FILE_X20,
//			FILE_CS10,
//			FILE_CS11,
//			FILE_CS21,
//			FILE_CS211,
//			FILE_CS111,
//			FILE_T10,
//			FILE_T20,
//			FILE_C10,
//			FILE_C20,
//			FILE_A10,
//			FILE_A20,
//			FILE_A30,
//			FILE_A31
//		};
//
//		public static string ConstructRootFolder(string pdfFolder)
//		{
//			return BASE_FOLDER + pdfFolder + PDF_FOLDER;
//		}

		static internal void listSample(List<string> sample, string output_file)
		{

			logMsgFmtln("root folder", ROOT_FOLDER);
			logMsgFmtln("output file", output_file);
			UtilityLocal.logMsg(UtilityLocal.nl);

			foreach (string file in sample)
			{
				string fileName = ROOT_FOLDER + file;

				logMsgFmt("found", $"{File.Exists(fileName).ToString(), -6}");
				logMsgln("  file", file);
			}
		}
	}
}
