#region + Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

#endregion


// projname: Test3
// itemname: Samples
// username: jeffs
// created:  12/28/2019 6:51:01 AM


namespace Test3
{
	public class Samples
	{

		internal const string BASE_FOLDER              = @"C:\2099-999 Sample Project\Publish\Bulletins";

		internal const string NORMAL_FOLDER            = @"\2017-07-01 normal";
		internal const string PDF_IN_INDIV_PDF_FOLDER  = @"\2017-07-01 not in indiv PDFs folder and an empty sub-folders";
		internal const string NO_PDFS                  = @"\2017-07-01 no PDFs";
		internal const string EMPTY_SUB_FOLDER         = @"\2017-07-01 with an empty sub-folders";
		internal const string CORRUPT_PDF              = @"\2017-07-01 with corrupted PDF";
		internal const string NON_PDF                  = @"\2017-07-01 with Non-PDF's";
		internal const string ROOT_PDFS                = @"\2017-07-01 with PDFs in root";
		internal const string NO_SUCH_FOLDER           = @"\2017-07-01";

		internal const string PDF_FOLDER               = @"\Individual PDFs";

//		private const string BASE_PATH                 = @"\";
//
//		private const string BASE_COVER                = BASE_PATH + "00 Cover A\";
//		private const string BASE_GENERAL              = BASE_PATH + "00 General A\";
//		private const string BASE_CIVIL                = BASE_PATH + "01 Civil A\";
//		private const string BASE_ARCH                 = BASE_PATH + "07 Architectural A\";
//
//		private const string COVER_SUB1                = BASE_COVER + @"00.1 Test Sub Dir 1.0\";
//		private const string COVER_SUB2                = BASE_COVER + @"00.2 Test Sub Dir 2.0\";
//
//		private const string COVER_SUB11               = COVER_SUB1 + @"00.1.1 Test Sub-Sub Dir 1.1\";
//		private const string COVER_SUB21               = COVER_SUB2 + @"00.2.1 Test Sub-Sub Dir 2.1\";
//
//		private const string ARCH_SUB2                 = BASE_ARCH + @"07.2 Test Sub Dir 2.0\";
//		private const string ARCH_SUB3                 = BASE_ARCH + @"07.3 Test Sub Dir 3.0\";

		private const string BASE_PATH                 = "";
		private const string BASE_COVER                = "";
		private const string BASE_GENERAL              = "";
		private const string BASE_CIVIL                = "";
		private const string BASE_ARCH                 = "";
		private const string COVER_SUB1                = "";
		private const string COVER_SUB2                = "";
		private const string COVER_SUB11               = "";
		private const string COVER_SUB21               = "";
		private const string ARCH_SUB2                 = "";
		private const string ARCH_SUB3                 = "";


		private const string FILE_X00                  = BASE_PATH + @"X X0.0 This is a Test X00.pdf";
		private const string FILE_X10                  = BASE_PATH + @"X X1.0 This is a Test X10.pdf";
		private const string FILE_X20                  = BASE_PATH + @"X X2.0 This is a Test X20.pdf";
		private const string FILE_XX30                 = BASE_PATH + @"X X3.0 Corrupted X30.pdf";

		private const string FILE_CS10                 = BASE_COVER + @"A CS1.0-0 This is a Test CS10.pdf";
		private const string FILE_CS11                 = COVER_SUB1 + @"A CS1.1-0 This is a Test CS11.pdf";
		private const string FILE_CS111                = COVER_SUB11 + @"A CS1.1-1 This is a Test CS11.1.pdf";
		private const string FILE_CS21                 = COVER_SUB2 + @"A CS2.1-0 This is a Test CS21.pdf";
		private const string FILE_CS211                = COVER_SUB21 + @"A CS2.1-1 This is a Test CS21.1.pdf";

		private const string FILE_T10                  = BASE_GENERAL + @"A T1.0-0 This is a Test T10.pdf";
		private const string FILE_T20                  = BASE_GENERAL + @"A T2.0-0 This is a Test T20.pdf";

		private const string FILE_C10                  = BASE_CIVIL + @"C1.0-0 This is a Test C10.pdf";
		private const string FILE_C20                  = BASE_CIVIL + @"C2.0-0 This is a Test C20.pdf";

		private const string FILE_A10                  = BASE_ARCH + @"A A1.0-0 This is a Test A10.pdf";

		private const string FILE_A20                  = ARCH_SUB2 + @"A A2.0-0 This is a Test A20.pdf";
		private const string FILE_A30                  = ARCH_SUB3 + @"A A3.0-0 This is a Test A30.pdf";
		private const string FILE_A31                  = ARCH_SUB3 + @"A A3.1-0 This is a Test A31.pdf";

		private const string FILE_120                  = ARCH_SUB2 + @"1 A2.0-0 This is a Test 120.pdf";
		private const string FILE_130                  = ARCH_SUB3 + @"1 A3.0-0 This is a Test 130.pdf";
		private const string FILE_131                  = ARCH_SUB3 + @"1 A3.1-0 This is a Test 131.pdf";

		private const string FILE_1A20                 = ARCH_SUB2 + @"1A A2.0-0 This is a Test 1A20.pdf";
		private const string FILE_1A30                 = ARCH_SUB3 + @"1A A3.0-0 This is a Test 1A30.pdf";
		private const string FILE_1A31                 = ARCH_SUB3 + @"1A A3.1-0 This is a Test 1A31.pdf";


		private const string FILE_xA10_01a		= BASE_ARCH + @"A A1.0-0 This is a Test A10.pdf";
		private const string FILE_xA10_01b		= BASE_ARCH + @"A A1-0-0 This is a Test A10.pdf";

		private const string FILE_xA10_02a		= BASE_ARCH + @"A1.0-0 This is a Test A10.pdf";
		private const string FILE_xA10_02b		= BASE_ARCH + @"A1-0-0 This is a Test A10.pdf";


		private const string FILE_xA10_03a		= BASE_ARCH + @"A A1.0-0-This is a Test A10.pdf";
		private const string FILE_xA10_03b		= BASE_ARCH + @"A A1-0-0-This is a Test A10.pdf";

		private const string FILE_xA10_04a		= BASE_ARCH + @"A1.0-0-This is a Test A10.pdf";
		private const string FILE_xA10_04b		= BASE_ARCH + @"A1-0-0-This is a Test A10.pdf";


		private const string FILE_xA10_05a		= BASE_ARCH + @"A A1.0-0 - This is a Test A10.pdf";
		private const string FILE_xA10_05b		= BASE_ARCH + @"A A1-0-0 - This is a Test A10.pdf";

		private const string FILE_xA10_06a		= BASE_ARCH + @"A1.0-0 - This is a Test A10.pdf";
		private const string FILE_xA10_06b		= BASE_ARCH + @"A1-0-0 - This is a Test A10.pdf";


		internal const string ROOT_FOLDER = BASE_FOLDER + ROOT_PDFS + PDF_FOLDER;

		internal static List<string> fullRange = new List<string>()
		{
			FILE_1A20,
			FILE_X00,
			FILE_X10,
			FILE_X20,
			FILE_CS10,
			FILE_C10,
			FILE_C20,
			FILE_A20,
			FILE_A30,
			FILE_A31,
			FILE_120,
			FILE_130,
			FILE_131,
			FILE_1A30,
			FILE_1A31
		};

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
			logMsg(nl);

			foreach (string file in sample)
			{
				string fileName = ROOT_FOLDER + file;

				logMsgFmt("found", $"{File.Exists(fileName).ToString(), -6}");
				logMsgln("  file", file);
			}
		}
	}
}

/*
  more sheet id examples

A A1.0-1 this is a sheet name (this is a comment).PDF
A1.0a this is a sheet name.PDF
A1a this is a sheet name.PDF
A LT1.0-1  - - this is a - sheet name.PDF
A A1.0 this is a 10 sheet name
A A1 this is a c10 sheet name.PDF
AA A1.0-1a this is a sheet name

A1.0-1a this is a sheet name
A1.0-1a this is a sheet name.PDF
A1 A1.0-1a this is a sheet name.pDf

1A A1.0-1a this is a sheet name
1A A1.0-1a this is a sheet name.PDF
11A A1.0-1a this is a sheet name
1AA A1.0-1a this is a sheet name.PDF

1 A1.0-1 this is a sheet name
11 A1.0-1a this is a sheet name.PDF
111 A1.0-1a this is a sheet name

A1.0 this is a sheet name.PDF
A1 this is a sheet name.PDF
A1.0-1 this is a sheet name.PDF
A1.0-1a this is a sheet name.PDF

CS1.0 this is a 10 sheet name

LT1.0 this is a sheet name
LT1.0 this is a sheet name.pdF

C1 this is a sheet name.PDF
C1.0 this is a sheet name
C1.0-0 This is a Test C10.Pdf
C11 this is a sheet name.PdF
C11.0 this is a sheet name.PDF

L1 this is a sheet name.PDF
L1.0 this is a sheet name
L1.0-0 This is a 10 Test C10 .Pdf
L11 this is a sheet name.PdF
L11.0 this is a sheet name.PDF
A A1.0-0 This is a Test A10 .pdf
A A1-0-0 This is a Test A10 .pdf

A1.0-0 This is a Test A10.pdf
A1-0-0 This is a Test A10.pdf

A A1.0-0-This is a Test A10.pdf
A A1-0-0-This is a Test A10.pdf

A1.0-0-This is a Test A10.pdf
A1-0-0-This is a Test A10.pdf

A A1.0-0 - This is a Test A10.pdf
A A1-0-0 - This is a Test A10.pdf

A1.0-0 - This is a Test A10.pdf
A1-0-0 - This is a Test A10.pdf

A1-0-0a - This is a Test A10.pdf
A1-0-0a This is a Test A10.pdf

try 1:
gets the sheet number separate from the sheet name
^(.*\d\s+\-\s+|.*\d\s+|.*\d\-|.*\d\-)(.*)


try 2:
subdivides - 
group 1 is the sheet number
group 2 is the whole file name
group 4 is the comment

^([A-Za-z0-9 \.-]+\d+(?! \()[ -]+)((.*(\(.*\)).*)|(.*))

try 3:
works
group 1 is the sheet number
group 2 is the whole file name
when no comment, there is a group 7

group 5 is the comment
when there is a group 5:
group 4 is the file name without the comment
group 6 is the file extension
^([A-Za-z0-9 \.-]+\d+(?! \()[ -]+)(((.*)(\(.*\))(.*))|(.*))

try 4:
works better - removed extra characters after the sheet number
group 1 is the sheet number
group 2 is the whole file name
when no comment, there is a group 7

group 5 is the comment
when there is a group 5:
group 4 is the file name without the comment
group 6 is the file extension
^([A-Za-z0-9 \.-]+\d+)(?! \()[ -]+(((.*)(\(.*\))(.*))|(.*))


try 5:
works same - except made extension separate group
group 1 is the sheet number
group 2 is the file name (no extension)
group 8 is the file extension

when there is a group 5:
group 5 is the comment
group 4 is the file name without the comment
^([A-Za-z0-9 \.-]+\d+)(?! \()[ -]+(((.*)(\(.*\))(.*))|(.*))(\.[pP][dD][fF])

try 6 -  no good
works same - except made extension separate group
group 1 is the sheet number
group 4 is the file name (no extension)
group 3 is the separation between the sheet number and the sheet name
group 9 is the file name (no extension)
group 10 is the file extension

when there is a group 7:
group 6 is the file name without the comment
group 7 is the comment
^([A-Za-z0-9 \.-]+(\d|\d[A-Za-z]))(?! \()([ -]+)(((.*)(\(.*\))(.*))|(.*))(\.[pP][dD][fF])

try 7 - no good
works same - except made extension works when no file extension
group 1 is the sheet number
group 3 is the separation between the sheet number and the sheet name
group 4 is the file name (with extension)
group 5 is the file name (no extension)
group 10 is the file name (no extension)
group 11 is the file extension

when there is a group 8:
group 7 is the file name without the comment
group 8 is the comment

when there is no group 11
group 4 is the file name (no extension)

^([A-Za-z0-9 \.-]+(\d|\d[A-Za-z]))(?! \()([ -]+)((((.*)(\(.*\))(.*))|(.*))(\.[pP][dD][fF])|.*)


try 8 - no good
simplify
group 1 is the sheet number
group 3 is the separator
group 4 is the sheet name

except, if there is a group 6:
group 5 is the sheet name
group 6 is the comment

^([A-Za-z0-9 \.-]+(\d|\d[A-Za-z]))(?! \()([ -]+)((.*)(\(.*\))|.*\.|.*)

try 9:
works!?!
group 1: sheet number
group 2: when exists sheet number
group 3: separator
group 4: sheet name

when there is a group 6:
group 5: the sheet name
group 6: the comment

^(([A-Z0-9 ]+[A-Z]+[a-z\.0-9-]+)|[A-Z]+[a-z0-9\.-]+)([ -]+)((.*)(\(.*\))|.*\.|.*)

try 10: 
same as 9 except that this allows for
(2) additional wrong formats

^(([A-Z0-9 ]+[A-Z]+[a-z\.0-9-]+)|[A-Z -]+[a-z0-9\.-]+)([ -]+)((.*)(\(.*\))|.*\.|.*)


try 11:
^(([0-9]*[A-Z]*)(?= |-)[ -]*([A-Z0-9\.-]*[a-z]*)|[A-Z]+[0-9\.-]+[a-z]*){1}([- ]+)(.*(\(.*\))(\.[Pp][dD][Ff])|.*)

group 1: full sheet number
group 2: phase &/or building
group 3: sheet id
group 4: separator
group 5: sheet name

if there is a group 6:
group 6: sheet name
group 7: comment


try 12:
^(([0-9]*[A-Z]*)(?= |-)[ -]*([A-Z0-9\.-]*[a-z]*)|[A-Z]+[0-9\.-]+[a-z]*){1}([- ]+)((.*)(\(.*\))(\.[Pp][dD][Ff])|(.*)(\.[Pp][dD][Ff])|(.*))

--- group -----
got	got	got	got                                                     bldg   sheet        sheet      
 3   5   7   9      match 1										    phase   id     sep  name   comment  ext                       
 x   -   x   -      phase/building + comment has extension		      .      .      .     .       .      . 	                      
                    group 1: full sheet number					  1   .      .      .     .       .      . 	                      
                    group 2: phase &/or building				      2      .      .     .       .      . 	                      
                    group 3: sheet id							      .      3      .     .       .      . 	                      
                    group 4: separator							      .      .      4     .       .      . 	                      
					group 5: sheet name w/ extension              5   .      .      .     .       .      . 	                      
                    group 6: sheet name							      .      .      .     6       .      . 	                      
                    group 7: comment							      .      .      .     .       7      . 	                      
                    group 8: extension							      .      .      .     .       .      8                        
got	got	got	got                    								      .      .      .     .       .      . 	                      
 3   5   7   9      match 7										      .      .      .     .       .      . 	                      
 -   x   x   -      no phase or building + comment has extension      .      .      .     .       .      . 	                      
                    group 1: sheet id     						      .      1      .     .       .      . 	                      
					group 2                                     none										                      
					group 3                                     none										                      
                    group 4: separator							      .      .      4     .       .      . 	                      
					group 5: sheet name w/ extension              5   .      .      .     .       .      . 	                      
                    group 6: sheet name							      .      .      .     6       .      . 	                      
                    group 7: comment							      .      .      .     .       7      . 	                      
                    group 8: extension							      .      .      .     .       .      8 	                      
																      .      .      .     .       .      . 	                      
got	got	got	got													      .      .      .     .       .      . 	                      
 3   5   7   9      match 13									      .      .      .     .       .      . 	                      
 -   -   -   x      no phase or building & no comment			      .      .      .     .       .      . 	                      
                    group 1: sheet id							      .      1      .     .       .      . 	                      
					group 2                                     none										                      
					group 3                                     none										                      
                    group 4: separator							      .      .      4     .       .      . 	                      
                    group 9: sheet name							      .      .      .     9       .      . 	                      
                    group 10: extension							      .      .      .     .       .      10                       
                    											      .      .      .     .       .      . 	                      
got	got	got	got													      .      .      .     .       .      . 	                      
 3   5   7   9      match 15										      .      .      .     .       .      . 	                      
 x   x   -   x      phase/building & no comment has extension	      .      .      .     .       .      . 	                      
                    group 1: full sheet number					  1   .      .      .     .       .      . 	                      
                    group 2: phase &/or building				      2      .      .     .       .      . 	                      
                    group 3: sheet id							      .      3      .     .       .      . 	                      
                    group 4: separator							      .      .      4     .       .      . 	                      
                    group 5: sheet name w/ extension			      .      .      .     .       .      . 	                      
                    group 9: sheet name							      .      .      .     9       .      . 	                      
                    group 10: extension							      .      .      .     .       .      10                       
																      .      .      .     .       .      . 	                      
got	got	got	got													      .      .      .     .       .      . 	                      
 3   5   7   9      match 18									      .      .      .     .       .      . 	                      
 x   x   -   -      phase/building & no comment, no extension	      .      .      .     .       .      . 	                      
                    group 1: full sheet number					   1  .      .      .     .       .      . 	                      
                    group 2: phase &/or building				      2      .      .     .       .      . 	                      
                    group 3: sheet id							      .      3      .     .       .      . 	                      
                    group 4: separator							      .      .      4     .       .      . 	                      
                    group 5: sheet name							      .      .      .     5       .      . 	                      
                    group 11: sheet name						      .      .      .     .       .      . 	                      
                    											      .      .      .     .       .      . 	                      
got	got	got	got													      .      .      .     .       .      . 	                      
 3   5   7   9      match 21									      .      .      .     .       .      . 	                      
 x   x   -   x      phase/building - no comment, has extension	      .      .      .     .       .      . 	                      
                    group 1: full sheet number					   1  .      .      .     .       .      . 	                      
                    group 2: phase &/or building				      2      .      .     .       .      . 	                      
                    group 3: sheet id							      .      3      .     .       .      . 	                      
                    group 4: separator							      .      .      4     .       .      . 	                      
                    group 5: sheet name with extension			      .      .      .     .       .      . 	                      
                    group 9: sheet name							      .      .      .     .       9      . 	                      
                    group 10: extension							      .      .      .     .       .      10                       
																                                    
group 1: full sheet number
group 1: sheet id (no 2 (and no 3))
group 2: phase &/or building
group 3: phase-bldg-sep
group 4: sheet id	(unless blank then is 1)
group 5: separator	(always)				
group 6: sheet name with extension (when 11 or 9)
group 6: sheet name (no 11 / no 9)
group 7: sheet name (when 8)
group 8: comment    (always)
group 9: extension  (always when exists)
group 10: sheet name (when 11)			
group 11: extension	(when exists)


// phase-bldg
if (got 2) is phase-bldg

// phase-bldg-sep
if got 3 is phase-bldg-sEP

// sheet id
if (got 4) is sheet id, else == 1

// separator
== 5 is separator

// sheet name
if (when 8) == 7
else (when 11) == 10
else == 6

// comment
== (when 8) is comment

try 13
^(([0-9]*[A-Z]*)(?=[ -]+[A-Z])([ -]*)([A-Z0-9\.-]*[a-z]*)|([A-Z]+ *[0-9\.-]+[a-z]*){1})([- ]+)((.*)(\(.*\))(\.[Pp][dD][Ff])|(.*)(\.[Pp][dD][Ff])|(.*))


--- group -----                                                           phase
got	got	got	got                                                     phase bldg  sheet        sheet      
 2                  match 1										    bldg   sep   id     sep  name   comment  ext                       
 x                  phase/building + comment has extension		      .     .     .      .     .        .     .
                    group 1: full sheet number					  1   .     .     .      .     .        .     .
                    group 2: phase &/or building				      2     .     .      .     .        .     .
					group 3: phase bldg sep                           .     3     .      .     .        .     .
                    group 4: sheet id							      .     .     4      .     .        .     .
                    group 6: separator							      .     .     .      6     .        .     .
					group 7: sheet name w/ extension              5   .     .     .      .     .        .     .
                    group 8: sheet name							      .     .     .      .     8        .     .
                    group 9: comment							      .     .     .      .     .        9     .
                    group 10: extension							      .     .     .      .     .        .     10
got	got	got	got                    								      .     .     .      .     .        .     .
                    match 7										      .     .     .      .     .        .     .
                    no phase or building + comment has extension      .     .     .      .     .        .     .
                    group 1: sheet id     						      1     .     .      .     .        .     .
					group 2                                     none  .     .     .      .     .        .     .
					group 3                                     none  .     .     .      .     .        .     .
                    group 5: sheet id     						      .     .     5      .     .        .     .
                    group 6: separator							      .     .     .      6     .        .     .
					group 7: sheet name w/ extension              5   .     .     .      .     .        .     .
                    group 8: sheet name							      .     .     .      .     8        .     .
                    group 9: comment							      .     .     .      .     .        9     .
                    group 10: extension							      .     .     .      .     .        .     10
																      .     .     .      .     .        .     .
got	got	got	got													      .     .     .      .     .        .     .
 3   5   7   9      match 13									      .     .     .      .     .        .     .
 -   -   -   x      no phase or building & no comment has extension   .     .     .      .     .        .     .
                    group 1: sheet id							      1     .     .      .     .        .     .
					group 2                                     none  .     .     .      .     .        .     .
					group 3                                     none  .     .     .      .     .        .     .
                    group 5: sheet id     						      .     .     5      .     .        .     .
                    group 6: separator							      .     .     .      .     .        .     .
                    group 11: sheet name                              .     .     .      .     .        .     .
                    group 12: extension							      .     .     .      .     .        .     .
                    											      .     .     .      .     .        .     .
got	got	got	got													      .     .     .      .     .        .     .
 3   5   7   9      match 15										  .     .     .      .     .        .     .
 x   x   -   x      phase/building & no comment has extension	      .     .     .      .     .        .     .
                    group 1: full sheet number					  1   .     .     .      .     .        .     .
                    group 2: phase &/or building				      2     .     .      .     .        .     .
					group 3: phase bldg sep                           .     3     .      .     .        .     .
                    group 4: sheet id							      .     .     .      .     .        .     .
                    group 6: separator							      .     .     .      .     .        .     .
                    group 7: sheet name w/ extension			      .     .     .      .     .        .     .
                    group 11: sheet name                              .     .     .      .     .        .     .
                    group 12: extension							      .     .     .      .     .        .     .
																      .     .     .      .     .        .     .
got	got	got	got													      .     .     .      .     .        .     .
 3   5   7   9      match 18									      .     .     .      .     .        .     .
 x   x   -   -      phase/building & no comment, no extension	      .     .     .      .     .        .     .
                    group 1: full sheet number					   1  .     .     .      .     .        .     .
                    group 2: phase &/or building				      .     .     .      .     .        .     .
					group 3: phase bldg sep                           .     3     .      .     .        .     .
                    group 4: sheet id							      .     .     .      .     .        .     .
                    group 6: separator							      .     .     .      .     .        .     .
                    group 7: sheet name							      .     .     .      .     .        .     .
                    group 13: sheet name						      .     .     .      .     .        .     .


group 1: full sheet number         (in general)
group 1: sheet id                  (no 4)
group 2: phase &/or building       (when exists)
group 3: phase-bldg-sep            (when 2 exists)
group 4: sheet id	               (when exists else 1)
group 6: separator	               (always)				
group 7: sheet name with extension (when 10 or 12)
group : sheet name                 ()
group 8: sheet name                (when 10)
group 9: comment                   (when exists)
group 10: extension                (when exists)

group 11: sheet name               (when 12)			
group 12: extension	               (when exists)


// phase-bldg
if (got 2) is phase-bldg

// phase-bldg-sep
if (got 2) 3 is phase-bldg-sep

// sheet id
if (got 4) is sheet id
if (got 5) is sheet id

// separator
6 is separator

// sheet name
if (when 10) == 8
else (when 12) == 11
else is 7

// comment
(got 9) is comment




A  A1.0-0  -
A  A1.0-0  -
A 1.5-0 -   
A A2-0-0 -  
A A3.0-0-   
A A4-0-0 -  
A5.0-0 -    
A6-0-0-     
A7-0-0 -    
A-A8-0-0-   
A-A30.0-0 - 

1A A1.0-1   
A1.0a       
A1a         
A-LT1.0-1  -
A A1.0 -    
A A1        
AA A1.0-1a  

A1.0-1a     
A1.0-1a     
1A A1.0-1a  

1A A1.0-1a  
1A A1.0-1a  
11A A1.0-1a 
1AA A1.0-1a 

1 A1.0-1    
11 A1.0-1a  
111 A1.0-1a 

A1.0        
A1          
A1.0-1      
A1.0-1a     

CS1.0       

LT1.0       
LT1.0       

C1          
C1.0        
C1.0-0      
C11         
C11.0       

L1          
L1.0        
L1.0-0      
L11         
L11.0       
A A1.0-0    
A A1-0-0    

A1.0-0      
A1-0-0      

A A1.0-0-   
A A1-0-0-   

A1.0-0-     
A1-0-0-     

A A1.0-0 -  
A A1-0-0 -  

A1.0-0 -    
A1-0-0 -    

A1-0-0a -   
A1-0-0a     













 */
