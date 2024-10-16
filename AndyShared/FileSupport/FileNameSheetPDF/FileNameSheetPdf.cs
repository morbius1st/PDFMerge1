#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Documents;
using AndyShared.FileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using JetBrains.Annotations;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;

#endregion

// username: jeffs
// created:  5/27/2020 10:53:23 PM

// namespace AndyShared.FileSupport.FileNameSheetPDF
namespace UtilityLibrary
{
	public class FileNameSheetPdf : FileNameSimple,
		/*AFileName,*/ INotifyPropertyChanged
	{

	#region private fields

		private static int idx;

		private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();

		private ShtNumber shtNumberObj;

		private FileNameSheetParser parser = FileNameSheetParser.Instance;

		internal FileTypeSheetPdf fileType;
		// internal ShtCompTypes shtCompType;
		internal ShtIdType2 shtCompType;

		// flags
		private bool? parsed = false;
		private bool isSelected = false;

		// fields being accessed by parse routines
		// internal List<string> SheetComps;

		internal bool? isPhaseBldg;
		internal bool? hasIdentifier;

		internal string sheetID;
		internal string sheetTitle;

		internal string originalSheetTitle;

		private FileNameParseStatusCodes statusCode;

	#endregion

	#region public fields

	#endregion

	#region ctor

	#endregion

	#region public properties

		// operational properties
		public FileNameParseStatusCodes StatusCode
		{
			get => statusCode;
			set
			{
				statusCode = value;
				OnPropertyChange();
			}
		}
		public int Index { get; set; }            = idx++;

		// do not convert

		public override string FileNameNoExt
		{
			get => fileNameNoExt;
			set
			{
				fileNameNoExt = value;
				OnPropertyChange();

				parsed = config();
				// parsed = parse();
			}
		}

		public override string ExtensionNoSep
		{
			get => extensionNoSep;
			set
			{
				extensionNoSep = value;
				OnPropertyChange();

				// parsed = parse();
			}
		}


		public string SheetNumber                 => shtNumberObj.ParsedSheetNumber;
		public string SheetID                     => shtNumberObj.ParsedSheetId;
		
		public string SheetName                   => SheetNumber + " - " + SheetTitle;
		public string SheetTitle                  => sheetTitle;


		public string SheetNumFromComps           => shtNumberObj.SheetNumberFromComps;
		public string SheetIdFromComps            => shtNumberObj.SheetIdFromComps;


		public ShtNumber ShtNumberObj           => shtNumberObj;

		// orig
		public string OrigShtNumber               => shtNumberObj.OrigSheetNumber;

		// settings

		public FileTypeSheetPdf FileType          => fileType;

		// public ShtCompTypes SheetIdType => shtCompType;
		public ShtIdType2 SheetIdType             => shtNumberObj.SheetIdType;


		// status

		public bool IsParseGood                  => shtNumberObj.IsParseGood;

		public bool IsValid                      => !FileNameNoExt.IsVoid() && !ExtensionNoSep.IsVoid();

		public bool? IsPhaseBldg                  => shtNumberObj.IsPhaseBldg;

		// name components

		public List<string> SheetComps            => shtNumberObj.ShtNumComps;

		public string PhaseBldg                   => shtNumberObj.PhaseBldg    ;	
		// public string PhaseBldgSep             => shtNumberObj.PhaseBldgSep ;
		public string Discipline                  => shtNumberObj.Discipline   ;
		public string Seperator0                  => shtNumberObj.Seperator0   ;
		public string Category                    => shtNumberObj.Category     ;
		public string Seperator1                  => shtNumberObj.Seperator1   ;
		public string Subcategory                 => shtNumberObj.Subcategory  ;
		public string Seperator2                  => shtNumberObj.Seperator2   ;
		public string Modifier                    => shtNumberObj.Modifier     ;
		public string Seperator3                  => shtNumberObj.Seperator3   ;
		public string Submodifier                 => shtNumberObj.Submodifier  ;
		// identifier                ;
		public string Seperator4                  => shtNumberObj.Seperator4   ;
		public string Identifier                  => shtNumberObj.Identifier   ;
		public string Seperator5                  => shtNumberObj.Seperator5   ;
		public string Subidentifier               => shtNumberObj.Subidentifier;

		// to convert

		// public string SheetNumber  => SheetPb + sheetID;		

		// public string SheetPb      =>  PhaseBldg + PhaseBldgSep;

		// public string SheetID      => sheetID;

		//
		// // phase bldg parse
		// public string PhaseBldg      => SheetComps[VI_PHBLDG]        ;
		// public string PhaseBldgSep   => SheetComps[VI_PBSEP]         ;
		// // sheet Id parse                                                  ;
		// public string Discipline     => SheetComps[VI_DISCIPLINE]    ;
		// public string Seperator0     => SheetComps[VI_SEP0]          ;
		// public string Category       => SheetComps[VI_CATEGORY]      ;
		// public string Seperator1     => SheetComps[VI_SEP1]          ;
		// public string Subcategory    => SheetComps[VI_SUBCATEGORY]   ;
		// public string Seperator2     => SheetComps[VI_SEP2]          ;
		// public string Modifier       => SheetComps[VI_MODIFIER]      ;
		// public string Seperator3     => SheetComps[VI_SEP3]          ;
		// public string Submodifier    => SheetComps[VI_SUBMODIFIER]   ;
		// // identifier                                                      ;
		// public string Seperator4     => SheetComps[VI_SEP4]          ;
		// public string Identifier     => SheetComps[VI_IDENTIFIER]    ;
		// public string Seperator5     => SheetComps[VI_SEP5]          ;
		// public string Subidentifier  => SheetComps[VI_SUBIDENTIFIER] ;



	#endregion

	#region private properties

	#endregion

	#region public methods

		/// <summary>
		/// indexer - provide the components of the sheet number<br/>
		/// adjusting for the "SheetIdType" without separators<br/>
		/// for missing components, provide a null - but note that<br/>
		/// identifier and subidentifier may exist beyond a null return
		/// [0] is always the phase / building code
		/// </summary>
		/// <param name="index"></param>
		public string this[int index]
		{
			get
			{
				int idx = index * 2;

				if (index < VI_PHBLDG || idx > VI_SUBIDENTIFIER) throw new IndexOutOfRangeException();

				return SheetComps[idx];
			}
		}

	#endregion

	#region private methods

		/*
		private bool? parse()
		{
			if (parsed == true || fileNameNoExt.IsVoid() || extensionNoSep.IsVoid()) return false;

			bool? success = parse(fileNameNoExt, extensionNoSep);

			if (success == true)
			{
				NotifyChange();
			}

			return success;
		}

		
		private bool parse(string filename, string fileextension)
		{
			if (filename.IsVoid() || fileextension.IsVoid())
			{
				// status is invalid
				return false;
			}

			bool result = true;

			if (fxc.IsCorrectFileType(fileextension))
			{
				ConfigFileNameComps();

				if (FileNameSheetParser.Instance.ParsePhBldg(this, filename))
				{
					fileType = FileTypeSheetPdf.SHEET_PDF;

					result = FileNameSheetParser.Instance.ParseSheetId(this, sheetID);
				}
				else
				{
					fileType = FileTypeSheetPdf.NON_SHEET_PDF;
				}
			}
			else
			{
				fileType = FileTypeSheetPdf.OTHER;
			}

			return result;
		}
		*/

		/*
		private bool parse2(string filename, string fileextension)
		{
			if (filename.IsVoid() || fileextension.IsVoid())
			{
				// status is invalid
				return false;
			}

			bool result = true;

			if (fxc.IsCorrectFileType(fileextension))
			{
				ConfigFileNameComps();

				if (FileNameSheetParser.Instance.ParsePhBldg(this, filename))
				{
					fileType = FileTypeSheetPdf.SHEET_PDF;

					result = FileNameSheetParser.Instance.ParseSheetId(this, sheetID);
				}
				else
				{
					fileType = FileTypeSheetPdf.NON_SHEET_PDF;
				}
			}
			else
			{
				fileType = FileTypeSheetPdf.OTHER;
			}

			return result;
		}
		*/

		// private void ConfigFileNameComps()
		// {
		// 	// PbComps = new List<string>();
		// 	//
		// 	// for (int i = 0; i < (int) PbCompsIdx.CI_PBCOUNT; i++)
		// 	// {
		// 	// 	PbComps.Add(null);
		// 	// }
		//
		// 	SheetComps = new List<string>();
		//
		// 	for (int i = 0; i < VI_COUNT; i++)
		// 	{
		// 		SheetComps.Add(null);
		// 	}
		//
		// 	// IdentComps = new List<string>();
		// 	//
		// 	// for (int i = 0; i < (int) IdentCompsIdx.CI_IDENTCOUNT; i++)
		// 	// {
		// 	// 	IdentComps.Add(null);
		// 	// }
		// }

		private void NotifyChange()
		{
			OnPropertyChange("FileNameNoExt");
			OnPropertyChange("ExtensionNoSep");

			OnPropertyChange("IsValid");
			OnPropertyChange("IsPhaseBldg");
			OnPropertyChange("HasIdentifier");

			OnPropertyChange("FileType");

			OnPropertyChange("SheetNumber");
			OnPropertyChange("SheetId");
			OnPropertyChange("sheetTitle");
			OnPropertyChange("OriginalSheetTitle");

			// PB
			OnPropertyChange("PhaseBldg");
			OnPropertyChange("PhaseBldgSep");

			// sheet Id
			OnPropertyChange("Discipline");
			OnPropertyChange("Seperator0");
			OnPropertyChange("Category");
			OnPropertyChange("Seperator1");
			OnPropertyChange("Subcategory");
			OnPropertyChange("Seperator2");
			OnPropertyChange("Modifier");
			OnPropertyChange("Seperator3");
			OnPropertyChange("Submodifier");
			OnPropertyChange("Seperator4");
			OnPropertyChange("Identifier");
			OnPropertyChange("Seperator5");
			OnPropertyChange("subidentifier");
			OnPropertyChange("Seperator6");
		}

		private bool config()
		{
			string origShtNum;

			if (fxc.IsCorrectFileType(extensionNoSep) && !FileName.IsVoid())
			{
				if (parser.SplitFileName(this, out origShtNum, out sheetTitle))
				{
					if (!origShtNum.IsVoid())
					{
						shtNumberObj = new ShtNumber(origShtNum, true);

						StatusCode = shtNumberObj.StatusCode;

						parsed = StatusCode == FileNameParseStatusCodes.SC_NONE ? true : false;

						if (parsed==true) fileType = FileTypeSheetPdf.SHEET_PDF;

						return parsed == true;
					}

					StatusCode = FileNameParseStatusCodes.SC_INVALID_FILENAME;
				}
				else
				{
					fileType = FileTypeSheetPdf.NON_SHEET_PDF;
					StatusCode = FileNameParseStatusCodes.SC_INVALID_FILENAME;
				}
			}
			else
			{
				fileType = FileTypeSheetPdf.OTHER;
				StatusCode = FileNameParseStatusCodes.SC_INVALID_FILENAME;
			}

			shtNumberObj = new ShtNumber(null);
			return false;
		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FileNameAsSheetFileName";
		}

	#endregion


		/*
public string ShtCompTypeName => ShtIds.ShtCompList2[(int) shtCompType].Title;

public bool IsSelected
{
	get => isSelected;
	set
	{
		isSelected = value;
		OnPropertyChanged();
	}
}

public bool? HasIdentifier => hasIdentifier;

public string OriginalSheetTitle => originalSheetTitle;
*/

		/*
		public string SheetId()
		{
			StringBuilder sb = new StringBuilder("");

			List<SheetCompInfo2> list =	ShtIds.ShtCompList2[(int) shtCompType].ShtCompInfo;

			foreach (SheetCompInfo2 ci in list)
			{
				if (ci.SeqCtrlUse == SeqCtrlUse2.NOT_USED) break;
				if (ci.SeqCtrlUse == SeqCtrlUse2.SKIP) continue;

				string text = SheetComps[ci.ValueIndex];

				if (ci.SeqCtrlUse == SeqCtrlUse2.REQUIRED) 
				{
					if ((int) ci.GetProceedReqd() <= 0) break;

					if (text.IsVoid())
					{
						text = "{x}";
					}
				}
				else if (ci.SeqCtrlUse == SeqCtrlUse2.OPTIONAL)
				{
					if ((int) ci.GetProceedOpt() <= 0) break;
				}

				sb.Append(text);

				if (ci.IsEnd) break;
			}

			return sb.ToString();
		}
		*/

		// public string Seperator6     => SheetComps[VI_SEP6]          ;

		// // phase bldg parse ----                                   --- [index]
		// public string PhaseBldg      => SheetComps[ShtIds.CompValueIdx2(ShtCompTypes.N_PHBLDG, CI_PHBLDG)]       ;
		// public string PhaseBldgSep   => SheetComps[ShtIds.CompValueIdx2(ShtCompTypes.N_PHBLDG, CI_PBSEP)]        ;
		// // sheet Id parse																							 ;
		// public string Discipline     => SheetComps[ShtIds.CompValueIdx2(shtCompType, DISCIPLINE_COMP_IDX)]           ;
		// public string Seperator0     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP0_COMP_IDX)]                 ;
		// public string Category       => SheetComps[ShtIds.CompValueIdx2(shtCompType, CATEGORY_COMP_IDX)]             ;
		// public string Seperator1     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP1_COMP_IDX)]                 ;
		// public string Subcategory    => SheetComps[ShtIds.CompValueIdx2(shtCompType, SUBCATEGORY_COMP_IDX)]          ;
		// public string Seperator2     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP2_COMP_IDX)]                 ;
		// public string Modifier       => SheetComps[ShtIds.CompValueIdx2(shtCompType, MODIFIER_COMP_IDX)]             ;
		// public string Seperator3     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP3_COMP_IDX)]                 ;
		// public string Submodifier    => SheetComps[ShtIds.CompValueIdx2(shtCompType, SUBMODIFIER_COMP_IDX)]          ;
		// // identifier																								 ;
		// public string Seperator4     => SheetComps[ShtIds.CompValueIdx2(ShtCompTypes.IDENT, SEP4_COMP_IDX)]          ;
		// public string Identifier     => SheetComps[ShtIds.CompValueIdx2(ShtCompTypes.IDENT, IDENTIFIER_COMP_IDX)]    ;
		// public string Seperator5     => SheetComps[ShtIds.CompValueIdx2(ShtCompTypes.IDENT, SEP5_COMP_IDX)]          ;
		// public string Subidentifier  => SheetComps[ShtIds.CompValueIdx2(ShtCompTypes.IDENT, SUBIDENTIFIER_COMP_IDX)] ;
		// public string Seperator6     => SheetComps[ShtIds.CompValueIdx2(ShtCompTypes.IDENT, SEP6_COMP_IDX)]          ;

	}
}