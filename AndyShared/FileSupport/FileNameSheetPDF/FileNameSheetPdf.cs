#region using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Documents;
using UtilityLibrary;

using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;


#endregion

// username: jeffs
// created:  5/27/2020 10:53:23 PM

/*
	path

	FilePath<FileNameSheetPdf>
	+-> FileNameSheetPdf
		+-> FileNameSheetComponents (fnc)
			+-> FileNameSheetParser(FileNameSheetComponents)

*/


namespace AndyShared.FileSupport.FileNameSheetPDF
{
	public class FileNameSheetPdf : AFileName, INotifyPropertyChanged
	{
	#region private fields
		
		private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();

		// true = good, false = invalid, null = unknown / unassigned
		// private bool? status = null;

		// flags
		private bool? parsed = false;
		private bool isSelected = false;

		// fields being accessed by parse routines
		internal List<string> SheetComps;

		// internal List<string> PbComps;
		// internal List<string> ShtIdComps;
		// internal List<string> IdentComps;

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

		// public static string[] SheetNumberComponentTitles { get; } =
		// 	new []
		// 	{
		// 		ShtIds.ShtCompList[(int) ShtCompTypes.PHBLDG].ShtCompInfo[PHBLDG_COMP_IDX].Title,
		// 		ShtIds.ShtCompList[(int) ShtCompTypes.TYPE10].ShtCompInfo[DISCIPLINE_COMP_IDX].Title,
		// 		ShtIds.ShtCompList[(int) ShtCompTypes.TYPE10].ShtCompInfo[CATEGORY_COMP_IDX].Title,
		// 		ShtIds.ShtCompList[(int) ShtCompTypes.TYPE10].ShtCompInfo[SUBCATEGORY_COMP_IDX].Title,
		// 		ShtIds.ShtCompList[(int) ShtCompTypes.TYPE10].ShtCompInfo[MODIFIER_COMP_IDX].Title,
		// 		ShtIds.ShtCompList[(int) ShtCompTypes.TYPE10].ShtCompInfo[SUBMODIFIER_COMP_IDX].Title,
		// 		ShtIds.ShtCompList[(int) ShtCompTypes.IDENT].ShtCompInfo[IDENTIFIER_COMP_IDX].Title,
		// 		ShtIds.ShtCompList[(int) ShtCompTypes.IDENT].ShtCompInfo[SUBIDENTIFIER_COMP_IDX].Title,
		// 		
		// 		// "Discipline", "Category", "Sub-Category", "Modifier", "sub-modifier", "Identifier",
		// 		// "Sub-Identifier"
		// 	};

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

		// public string ShtCompTypeName => ShtIds.SheetNumTypeTitle(shtCompType);
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

		// public string SheetNumber2 => (phaseBldg + phaseBldgSep + sheetID) ?? "sht number";
		public string SheetNumber  => SheetPb + sheetID;
		public string SheetID      => sheetID;
		public string SheetTitle   => sheetTitle;

		public string SheetPb      =>  PhaseBldg + PhaseBldgSep;

		public string SheetId
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				
				// List<SheetCompInfo> compInfoList = ShtIds.ShtCompList[(int) shtCompType].ShtCompInfo;
				//
				// foreach (SheetCompInfo ci in compInfoList)
				// {
				// 	string value = null;
				//
				// 	if (ci.IsUsed == false || ci.SeqCtrlUse == SeqCtrl.SKIP_CONTINUE) continue;
				//
				// 	if (ci.SeqCtrlUse == SeqCtrl.REQUIRED_CONTINUE)
				// 	{
				// 		value = SheetComps[ci.ValueIndex];
				//
				// 		if (value.IsVoid()) value = "** error **";
				//
				// 		sb.Append(value);
				// 	} else if (ci.SeqCtrlUse == op)
				//
				//
				//
				//
				//
				// 	sb.Append(value);
				//
				// 	if (ci.SeqCtrlUse == SeqCtrl.REQUIRED_END ||
				// 		ci.SeqCtrlUse == SeqCtrl.REQUIRED_END_AND_OPT_SEQ_END ||
				// 		ci.SeqCtrlUse == SeqCtrl.OPTIONAL_END)
				// 	{
				// 		break;
				// 	}
				// }



				switch (shtCompType)
				{
					case ShtCompTypes.TYPE10:
						{
							
							sb.Append(SheetComps[DISCIPLINE_COMP_IDX]);
							sb.Append(SheetComps[CATEGORY_COMP_IDX]);
							sb.Append(SheetComps[SEP1_COMP_IDX]);
							sb.Append(SheetComps[SUBCATEGORY_COMP_IDX]);
							sb.Append(SheetComps[SEP2_COMP_IDX]);
							sb.Append(SheetComps[MODIFIER_COMP_IDX]);
							sb.Append(SheetComps[SEP3_COMP_IDX]);
							sb.Append(SheetComps[SUBMODIFIER_COMP_IDX]);

							break;
						}
					case ShtCompTypes.TYPE20:
						{
							sb.Append(SheetComps[DISCIPLINE_COMP_IDX]);
							sb.Append(SheetComps[SEP0_COMP_IDX]);
							sb.Append(SheetComps[CATEGORY_COMP_IDX]);
							break;
						}
					case ShtCompTypes.TYPE30:
						{
							sb.Append(SheetComps[DISCIPLINE_COMP_IDX]);
							sb.Append(SheetComps[CATEGORY_COMP_IDX]);
							sb.Append(SheetComps[SEP1_COMP_IDX]);
							sb.Append(SheetComps[SUBCATEGORY_COMP_IDX]);

							break;
						}
					case ShtCompTypes.TYPE40:
						{
							sb.Append(SheetComps[DISCIPLINE_COMP_IDX]);
							sb.Append(SheetComps[SEP0_COMP_IDX]);
							sb.Append(SheetComps[CATEGORY_COMP_IDX]);
							break;
						}
				}

				return sb.ToString();

			}
		}

		public string SheetIdentifier
		{
			get
			{
				StringBuilder sb = new StringBuilder();

				sb.Append(SheetComps[SEP4_COMP_IDX]);
				sb.Append(SheetComps[IDENTIFIER_COMP_IDX]);
				sb.Append(SheetComps[SEP5_COMP_IDX]);
				sb.Append(SheetComps[SUBIDENTIFIER_COMP_IDX]);
				sb.Append(SheetComps[SEP6_COMP_IDX]);

				return sb.ToString();
			}
		}

		// phase bldg parse ----                                   --- [index]
		public string PhaseBldg      => SheetComps[ShtIds.CompValueIdx2(shtCompType, PHBLDG_COMP_IDX)]         ; // [0]
		public string PhaseBldgSep   => SheetComps[ShtIds.CompValueIdx2(shtCompType, PBSEP_COMP_IDX)]          ;

		// sheet Id parse
		public string Discipline     => SheetComps[ShtIds.CompValueIdx2(shtCompType, DISCIPLINE_COMP_IDX)]     ; // [1] (0) = [1] - 1 * 2 = 0      | (0) / 2 + 1 = 1
		public string Seperator0     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP0_COMP_IDX)]           ; //     (1)
		public string Category       => SheetComps[ShtIds.CompValueIdx2(shtCompType, CATEGORY_COMP_IDX)]       ; // [2] (2) = [2] - 1 * 2 = 2      | (2) / 2 + 1 = 2
		public string Seperator1     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP1_COMP_IDX)]           ; //     (3)
		public string Subcategory    => SheetComps[ShtIds.CompValueIdx2(shtCompType, SUBCATEGORY_COMP_IDX)]    ; // [3] (4) = [3] - 1 * 2 = 4      | (4) / 2 + 1 = 3
		public string Seperator2     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP2_COMP_IDX)]           ; //     (5)
		public string Modifier       => SheetComps[ShtIds.CompValueIdx2(shtCompType, MODIFIER_COMP_IDX)]       ; // [4] (6) = [4] - 1 * 2 = 6      | (6) / 2 + 1 = 4
		public string Seperator3     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP3_COMP_IDX)]           ; //     (7)
		public string Submodifier    => SheetComps[ShtIds.CompValueIdx2(shtCompType, SUBMODIFIER_COMP_IDX)]    ; // [5] (8) = [5] - 1 * 2 = 8      | (8) / 2 + 1 = 5

		// identifier
		public string Seperator4     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP4_COMP_IDX)]           ; //     (0)
		public string Identifier     => SheetComps[ShtIds.CompValueIdx2(shtCompType, IDENTIFIER_COMP_IDX)]     ; // [6] (1) = [6] - 5 * 2 - 1 = 1  | (1) + 1 / 2 + 5 = 6
		public string Seperator5     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP5_COMP_IDX)]           ; //     (2)
		public string Subidentifier  => SheetComps[ShtIds.CompValueIdx2(shtCompType, SUBIDENTIFIER_COMP_IDX)]  ; // [7] (3) = [7] - 5 * 2 - 1 = 3  | (3) + 1 / 2 + 5 = 7
		public string Seperator6     => SheetComps[ShtIds.CompValueIdx2(shtCompType, SEP6_COMP_IDX)]           ; //     (4)
																		// [8] (5) = [8] - 5 * 2 - 1 = 5  | (5) + 1 / 2 + 5 = 8

	#endregion

	#region private properties

	#endregion

	#region public methods

		/// <summary>
		/// indexer - provide the not separator components of the<br/>
		/// sheet number adjusting for the "SheetComponentType"<br/>
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