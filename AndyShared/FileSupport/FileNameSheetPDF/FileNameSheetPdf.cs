#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Documents;
using AndyShared.FileSupport.FileNameSheetPDF;
using UtilityLibrary;
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

		private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();

		// flags
		private bool? parsed = false;
		private bool isSelected = false;

		// fields being accessed by parse routines
		internal List<string> SheetComps;

		internal bool? isPhaseBldg;
		internal bool? hasIdentifier;

		internal FileTypeSheetPdf fileType;
		internal ShtCompTypes shtCompType;

		internal string sheetID;
		internal string originalSheetTitle;
		internal string sheetTitle;

	#endregion

	#region public fields

	#endregion

	#region ctor

	#endregion

	#region public properties

		public override string FileNameNoExt
		{
			get => fileNameNoExt;
			set
			{
				fileNameNoExt = value;
				OnPropertyChange();

				parsed = parse();
			}
		}

		public override string ExtensionNoSep
		{
			get => extensionNoSep;
			set
			{
				extensionNoSep = value;
				OnPropertyChange();

				parsed = parse();
			}
		}

		// sheet number and name parse
		public FileTypeSheetPdf FileType => fileType;

		public ShtCompTypes SheetComponentType => shtCompType;

		public string ShtCompTypeName => ShtIds.ShtCompList2[(int) shtCompType].Title;

		public bool IsSelected
		{
			get => isSelected;
			set
			{
				isSelected = value;
				OnPropertyChange();
			}
		}

		public new bool IsValid => !FileNameNoExt.IsVoid() && !ExtensionNoSep.IsVoid();

		public bool? IsPhaseBldg => isPhaseBldg;

		public bool? HasIdentifier => hasIdentifier;

		public string OriginalSheetTitle => originalSheetTitle;

		public string SheetName    => SheetNumber + " :: " + SheetTitle;

		public string SheetNumber  => SheetPb + sheetID;

		public string SheetID      => sheetID;

		public string SheetTitle   => sheetTitle;

		public string SheetPb      =>  PhaseBldg + PhaseBldgSep;

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

		// phase bldg parse
		public string PhaseBldg      => SheetComps[PHBLDG_VALUE_IDX]        ;
		public string PhaseBldgSep   => SheetComps[PBSEP_VALUE_IDX]         ;
		// sheet Id parse                                                  ;
		public string Discipline     => SheetComps[DISCIPLINE_VALUE_IDX]    ;
		public string Seperator0     => SheetComps[SEP0_VALUE_IDX]          ;
		public string Category       => SheetComps[CATEGORY_VALUE_IDX]      ;
		public string Seperator1     => SheetComps[SEP1_VALUE_IDX]          ;
		public string Subcategory    => SheetComps[SUBCATEGORY_VALUE_IDX]   ;
		public string Seperator2     => SheetComps[SEP2_VALUE_IDX]          ;
		public string Modifier       => SheetComps[MODIFIER_VALUE_IDX]      ;
		public string Seperator3     => SheetComps[SEP3_VALUE_IDX]          ;
		public string Submodifier    => SheetComps[SUBMODIFIER_VALUE_IDX]   ;
		// identifier                                                      ;
		public string Seperator4     => SheetComps[SEP4_VALUE_IDX]          ;
		public string Identifier     => SheetComps[IDENTIFIER_VALUE_IDX]    ;
		public string Seperator5     => SheetComps[SEP5_VALUE_IDX]          ;
		public string Subidentifier  => SheetComps[SUBIDENTIFIER_VALUE_IDX] ;
		public string Seperator6     => SheetComps[SEP6_VALUE_IDX]          ;

		// // phase bldg parse ----                                   --- [index]
		// public string PhaseBldg      => SheetComps[ShtIds.CompValueIdx2(ShtCompTypes.PHBLDG, PHBLDG_COMP_IDX)]       ;
		// public string PhaseBldgSep   => SheetComps[ShtIds.CompValueIdx2(ShtCompTypes.PHBLDG, PBSEP_COMP_IDX)]        ;
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



	#endregion

	#region private properties

	#endregion

	#region public methods

		/// <summary>
		/// indexer - provide the components of the sheet number<br/>
		/// adjusting for the "SheetComponentType" without separators<br/>
		/// for missing components, provide a null - but note that<br/>
		/// identifier and subidentifier may exist beyond a null return
		/// [0] is always the phase / building code
		/// </summary>
		/// <param name="index"></param>
		public string this[int index]
		{
			get
			{
				if (index < ShtIds.IDX_XLATE_MIN || index > ShtIds.IDX_XLATE_MAX ) throw new IndexOutOfRangeException();

				return SheetComps[IDX_XLATE[index]];
			}
		}

	#endregion

	#region private methods

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

				if (FileNameSheetParser.Instance.Parse2(this, filename))
				{
					fileType = FileTypeSheetPdf.SHEET_PDF;

					result = FileNameSheetParser.Instance.ParseSheetId2(this, sheetID);
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

		private void ConfigFileNameComps()
		{
			// PbComps = new List<string>();
			//
			// for (int i = 0; i < (int) PbCompsIdx.CI_PBCOUNT; i++)
			// {
			// 	PbComps.Add(null);
			// }

			SheetComps = new List<string>();

			for (int i = 0; i < VALUE_IDX_COUNT; i++)
			{
				SheetComps.Add(null);
			}

			// IdentComps = new List<string>();
			//
			// for (int i = 0; i < (int) IdentCompsIdx.CI_IDENTCOUNT; i++)
			// {
			// 	IdentComps.Add(null);
			// }
		}

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

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

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
	}
}