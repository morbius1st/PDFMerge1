// Solution:     PDFMerge1
// Project:       Test3
// File:             SheetPdfSample.cs
// Created:      2024-09-29 (11:58 PM)

using System.Collections.Generic;
using System.IO;
using AndyShared.FileSupport.FileNameSheetPDF;

using UtilityLibrary;

namespace Test3.SheetMgr
{
	public class SheetPdfSample
	{
		public FilePath<FileNameSheetPdf>  SheetPdf  { get; set; }
		// public FilePath<FileNameSheetPdf2> SheetPdf2 { get; set; }
		// public FilePath<FileNameSheetPdf3> SheetPdf3 { get; set; }

		public ShtNumber ShtNumber { get; set; }

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

			SheetComps[FileNameSheetIdentifiers.VI_PHBLDG]        = phaseBldg      ;
			SheetComps[FileNameSheetIdentifiers.VI_PBSEP]         = phaseBldgSep   ;
			SheetComps[FileNameSheetIdentifiers.VI_DISCIPLINE]    = discipline     ;
			SheetComps[FileNameSheetIdentifiers.VI_SEP0]          = seperator0     ;
			SheetComps[FileNameSheetIdentifiers.VI_CATEGORY]      = category       ;
			SheetComps[FileNameSheetIdentifiers.VI_SEP1]          = seperator1     ;
			SheetComps[FileNameSheetIdentifiers.VI_SUBCATEGORY]   = subcategory    ;
			SheetComps[FileNameSheetIdentifiers.VI_SEP2]          = seperator2     ;
			SheetComps[FileNameSheetIdentifiers.VI_MODIFIER]      = modifier       ;
			SheetComps[FileNameSheetIdentifiers.VI_SEP3]          = seperator3     ;
			SheetComps[FileNameSheetIdentifiers.VI_SUBMODIFIER]   = submodifier    ;
			SheetComps[FileNameSheetIdentifiers.VI_SEP4]          = seperator4     ;
			SheetComps[FileNameSheetIdentifiers.VI_IDENTIFIER]    = identifier     ;
			SheetComps[FileNameSheetIdentifiers.VI_SEP5]          = seperator5     ;
			SheetComps[FileNameSheetIdentifiers.VI_SUBIDENTIFIER] = subidentifier  ;
			// ShtNumComps[VI_SEP6]          =seperator6     ;
		}

		private void initSheetComps()
		{
			SheetComps = new List<string>();

			for (int i = 0; i < FileNameSheetIdentifiers.VI_COUNT; i++)
			{
				SheetComps.Add("");
			}
		}
	}
}