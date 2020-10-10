#region using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using UtilityLibrary;

// using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers;

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

//
// namespace AndyShared.FileSupport.SheetPDF
// {
// 	public class FileNameSheetPdf : AFileName, INotifyPropertyChanged
// 	{
// 	#region private fields
// 		
// 		private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();
//
// 		// true = good, false = invalid, null = unknown / unassigned
// 		private bool? status = null;
//
// 		// flags
// 		private bool? parsed = false;
// 		private bool selected = false;
//
// 		// fields being accessed by parse routines
// 		internal List<string> PbComps;
// 		internal List<string> ShtIdComps;
// 		internal List<string> IdentComps;
//
// 		internal bool? isPhaseBldg;
// 		internal bool? hasIdentifier;
//
// 		internal FileTypeSheetPdf fileType;
// 		internal ShtCompTypes shtCompType;
//
// 		internal string sheetID;
// 		internal string originalSheetTitle;
// 		internal string sheetTitle;
//
// 	#endregion
//
// 	#region public fields
//
// 	#endregion
//
// 	#region ctor
//
// 	#endregion
//
// 	#region public properties
//
// 		public static string[] SheetNumberComponentTitles { get; } =
// 			new []
// 			{
// 				ShtIds.ShtCompList[(int) ShtCompTypes.PHBLDG].ShtCompNames[PHBLDGIDX].Title,
// 				ShtIds.ShtCompList[(int) ShtCompTypes.TYPE10].ShtCompNames[DISCIPLINEIDX].Title,
// 				ShtIds.ShtCompList[(int) ShtCompTypes.TYPE10].ShtCompNames[CATEGORYIDX].Title,
// 				ShtIds.ShtCompList[(int) ShtCompTypes.TYPE10].ShtCompNames[SUBCATEGORYIDX].Title,
// 				ShtIds.ShtCompList[(int) ShtCompTypes.TYPE10].ShtCompNames[MODIFIERIDX].Title,
// 				ShtIds.ShtCompList[(int) ShtCompTypes.TYPE10].ShtCompNames[SUBMODIFIERIDX].Title,
// 				ShtIds.ShtCompList[(int) ShtCompTypes.IDENT].ShtCompNames[IDENTIFIERIDX].Title,
// 				ShtIds.ShtCompList[(int) ShtCompTypes.IDENT].ShtCompNames[SUBIDENTIFIERIDX].Title,
// 				
// 				// "Discipline", "Category", "Sub-Category", "Modifier", "sub-modifier", "Identifier",
// 				// "Sub-Identifier"
// 			};
//
// 		public override string FileNameNoExt
// 		{
// 			get => fileNameNoExt;
// 			set
// 			{
// 				fileNameNoExt = value;
// 				OnPropertyChange();
//
// 				parsed = parse();
// 			}
// 		}
//
// 		public override string ExtensionNoSep
// 		{
// 			get => extensionNoSep;
// 			set
// 			{
// 				extensionNoSep = value;
// 				OnPropertyChange();
//
// 				parsed = parse();
// 			}
// 		}
//
// 		// sheet number and name parse
// 		public FileTypeSheetPdf FileType => fileType;
//
// 		public ShtCompTypes SheetComponentType => shtCompType;
//
// 		// public string ShtCompTypeName => ShtIds.ShtCompTypesNames[(int) SheetComponentType];
// 		public string ShtCompTypeName => ShtIds[SheetComponentType.ToString()].Title;
//
// 		public bool Selected
// 		{
// 			get => selected;
// 			set
// 			{
// 				selected = value;
// 				OnPropertyChange();
// 			}
// 		}
//
// 		public new bool IsValid => !FileNameNoExt.IsVoid() && !ExtensionNoSep.IsVoid();
//
// 		public bool? IsPhaseBldg => isPhaseBldg;
//
// 		public bool? HasIdentifier => hasIdentifier;
//
// 		public string OriginalSheetTitle => originalSheetTitle;
//
// 		public string SheetName    => SheetNumber + " :: " + SheetTitle;
//
// 		// public string SheetNumber2 => (phaseBldg + phaseBldgSep + sheetID) ?? "sht number";
// 		public string SheetNumber  => (PbComps[PHBLDGIDX] + PbComps[PBSEPIDX] + sheetID);
// 		public string SheetID      => sheetID;
// 		public string SheetTitle   => sheetTitle;
//
// 		public string SheetPb      => PbComps[PHBLDGIDX] + PbComps[PBSEPIDX];
// 		public string SheetId
// 		{
// 			get
// 			{
// 				StringBuilder sb = new StringBuilder();
//
// 				switch (shtCompType)
// 				{
// 					case ShtCompTypes.TYPE10:
// 						{
// 							sb.Append(ShtIdComps[DISCIPLINEIDX]);
// 							sb.Append(ShtIdComps[CATEGORYIDX]);
// 							sb.Append(ShtIdComps[SEP1IDX]);
// 							sb.Append(ShtIdComps[SUBCATEGORYIDX]);
// 							sb.Append(ShtIdComps[SEP2IDX]);
// 							sb.Append(ShtIdComps[MODIFIERIDX]);
// 							sb.Append(ShtIdComps[SEP3IDX]);
// 							sb.Append(ShtIdComps[SUBMODIFIERIDX]);
//
// 							break;
// 						}
// 					case ShtCompTypes.TYPE20:
// 						{
// 							sb.Append(ShtIdComps[DISCIPLINEIDX]);
// 							sb.Append(ShtIdComps[SEP0IDX]);
// 							sb.Append(ShtIdComps[CATEGORYIDX]);
// 							break;
// 						}
// 					case ShtCompTypes.TYPE30:
// 						{
// 							sb.Append(ShtIdComps[DISCIPLINEIDX]);
// 							sb.Append(ShtIdComps[CATEGORYIDX]);
// 							sb.Append(ShtIdComps[SEP1IDX]);
// 							sb.Append(ShtIdComps[SUBCATEGORYIDX]);
//
// 							break;
// 						}
// 					case ShtCompTypes.TYPE40:
// 						{
// 							sb.Append(ShtIdComps[DISCIPLINEIDX]);
// 							sb.Append(ShtIdComps[SEP0IDX]);
// 							sb.Append(ShtIdComps[CATEGORYIDX]);
// 							break;
// 						}
// 				}
//
// 				return sb.ToString();
//
// 			}
// 		}
//
// 		public string SheetIdent
// 		{
// 			get
// 			{
// 				StringBuilder sb = new StringBuilder();
//
// 				sb.Append(ShtIdComps[SEP4IDX]);
// 				sb.Append(ShtIdComps[IDENTIFIERIDX]);
// 				sb.Append(ShtIdComps[SEP5IDX]);
// 				sb.Append(ShtIdComps[SUBIDENTIFIERIDX]);
// 				sb.Append(ShtIdComps[SEP6IDX]);
//
// 				return sb.ToString();
// 			}
// 		}
//
// 		// phase bldg parse ----                                   --- [index]
// 		public string PhaseBldg    => PbComps[PHBLDGIDX]              ; // [0]
// 		public string PhaseBldgSep => PbComps[PBSEPIDX]               ;
//
// 		// sheet Id parse
// 		public string Discipline     => ShtIdComps[DISCIPLINEIDX]     ; // [1] (0) = [1] - 1 * 2 = 0      | (0) / 2 + 1 = 1
// 		public string Seperator0     => ShtIdComps[SEP0IDX]           ; //     (1)
// 		public string Category       => ShtIdComps[CATEGORYIDX]       ; // [2] (2) = [2] - 1 * 2 = 2      | (2) / 2 + 1 = 2
// 		public string Seperator1     => ShtIdComps[SEP1IDX]           ; //     (3)
// 		public string Subcategory    => ShtIdComps[SUBCATEGORYIDX]    ; // [3] (4) = [3] - 1 * 2 = 4      | (4) / 2 + 1 = 3
// 		public string Seperator2     => ShtIdComps[SEP2IDX]           ; //     (5)
// 		public string Modifier       => ShtIdComps[MODIFIERIDX]       ; // [4] (6) = [4] - 1 * 2 = 6      | (6) / 2 + 1 = 4
// 		public string Seperator3     => ShtIdComps[SEP3IDX]           ; //     (7)
// 		public string Submodifier    => ShtIdComps[SUBMODIFIERIDX]    ; // [5] (8) = [5] - 1 * 2 = 8      | (8) / 2 + 1 = 5
// 		public string Seperator4     => IdentComps[SEP4IDX]           ; //     (0)
// 		public string Identifier     => IdentComps[IDENTIFIERIDX]     ; // [6] (1) = [6] - 5 * 2 - 1 = 1  | (1) + 1 / 2 + 5 = 6
// 		public string Seperator5     => IdentComps[SEP5IDX]           ; //     (2)
// 		public string Subidentifier  => IdentComps[SUBIDENTIFIERIDX]  ; // [7] (3) = [7] - 5 * 2 - 1 = 3  | (3) + 1 / 2 + 5 = 7
// 		public string Seperator6     => IdentComps[SEP6IDX]           ; //     (4)
// 																		// [8] (5) = [8] - 5 * 2 - 1 = 5  | (5) + 1 / 2 + 5 = 8
//
// 	#endregion
//
// 	#region private properties
//
// 	#endregion
//
// 	#region public methods
//
//
//
//
// 		/// <summary>
// 		/// indexer - provide the not separator components of the<br/>
// 		/// sheet number adjusting for the "SheetComponentType"<br/>
// 		/// for missing components, provide a null - but note that<br/>
// 		/// identifier and subidentifier may exist beyond a null return
// 		/// [0] is always the phase / building code
// 		/// </summary>
// 		/// <param name="index"></param>
// 		public string this[int index]
// 		{
// 			get
// 			{
// 				int idx = translateIndexToArrayIndex(index);
//
// 				if (idx >= IndexOffset[0] && idx <= IndexOffset[1])
// 				{
// 					return PhaseBldg;
// 				}
//
// 				if (idx >= IndexOffset[1] && idx <= IndexOffset[2])
// 				{
// 					return ShtIdComps[idx - IndexOffset[1]];
// 				}
//
// 				if (idx >= IndexOffset[2] && idx <= IndexOffset[3])
// 				{
// 					return IdentComps[idx - IndexOffset[2]];
// 				}
//
// 				throw new IndexOutOfRangeException();
// 			}
// 		}
//
//
//
//
// 	#endregion
//
// 	#region private methods
//
// 		private int[] IndexOffset = new [] {0, 10, 20, 30};
//
//
// 		private int translateIndexToArrayIndex(int index)
// 		{
// 			if (index == 0) return 0 + IndexOffset[0];
//
// 			int idx = (index - 1) * 2;
//
// 			// 1 through 5
// 			if (idx >= DISCIPLINEIDX && idx <= SUBMODIFIERIDX) return idx + IndexOffset[1];
//
// 			idx = (index - 5) * 2 - 1;
//
// 			if (idx >= IDENTIFIERIDX && idx <= SUBIDENTIFIERIDX)  return idx  + IndexOffset[2];
//
// 			throw new IndexOutOfRangeException();
// 		}
//
//
//
//
// 		private bool? parse()
// 		{
// 			if (parsed == true || fileNameNoExt.IsVoid() || extensionNoSep.IsVoid()) return false;
//
// 			bool? success = parse(fileNameNoExt, extensionNoSep);
//
// 			if (success == true)
// 			{
// 				NotifyChange();
// 			}
//
// 			return success;
// 		}
//
// 		private bool parse(string filename, string fileextension)
// 		{
// 			// unknown status
// 			status = null;
//
// 			if (filename.IsVoid() || fileextension.IsVoid())
// 			{
// 				// status is invalid
// 				status = false;
// 				return false;
// 			}
//
// 			bool result = true;
//
// 			if (fxc.IsCorrectFileType(fileextension))
// 			{
// 				ConfigFileNameComps();
//
// 				if (FileNameSheetParser.Instance.Parse2(this, filename))
// 				{
// 					fileType = FileTypeSheetPdf.SHEET_PDF;
//
// 					result = FileNameSheetParser.Instance.ParseSheetId2(this, sheetID);
// 				}
// 				else
// 				{
// 					fileType = FileTypeSheetPdf.NON_SHEET_PDF;
// 				}
// 			}
// 			else
// 			{
// 				fileType = FileTypeSheetPdf.OTHER;
// 			}
//
// 			return result;
// 		}
//
// 		private void ConfigFileNameComps()
// 		{
// 			PbComps = new List<string>();
//
// 			for (int i = 0; i < (int) PbCompsIdx.CI_PBCOUNT; i++)
// 			{
// 				PbComps.Add(null);
// 			}
//
// 			ShtIdComps = new List<string>();
//
// 			for (int i = 0; i < (int) ShtIdCompsIdx.CI_TYPECOUNT; i++)
// 			{
// 				ShtIdComps.Add(null);
// 			}
//
// 			IdentComps = new List<string>();
//
// 			for (int i = 0; i < (int) IdentCompsIdx.CI_IDENTCOUNT; i++)
// 			{
// 				IdentComps.Add(null);
// 			}
// 		}
//
// 		private void NotifyChange()
// 		{
// 			OnPropertyChange("FileNameNoExt");
// 			OnPropertyChange("ExtensionNoSep");
//
// 			OnPropertyChange("IsValid");
// 			OnPropertyChange("IsPhaseBldg");
// 			OnPropertyChange("HasIdentifier");
//
// 			OnPropertyChange("FileType");
// 			
// 			OnPropertyChange("SheetNumber");
// 			OnPropertyChange("SheetId");
// 			OnPropertyChange("sheetTitle");
// 			OnPropertyChange("OriginalSheetTitle");
//
// 			// PB
// 			OnPropertyChange("PhaseBldg");
// 			OnPropertyChange("PhaseBldgSep");
//
// 			// sheet Id
// 			OnPropertyChange("Discipline");
// 			OnPropertyChange("Seperator0");
// 			OnPropertyChange("Category");
// 			OnPropertyChange("Seperator1");
// 			OnPropertyChange("Subcategory");
// 			OnPropertyChange("Seperator2");
// 			OnPropertyChange("Modifier");
// 			OnPropertyChange("Seperator3");
// 			OnPropertyChange("Submodifier");
// 			OnPropertyChange("Seperator4");
// 			OnPropertyChange("Identifier");
// 			OnPropertyChange("Seperator5");
// 			OnPropertyChange("subidentifier");
// 			OnPropertyChange("Seperator6");
//
// 		}
//
// 	#endregion
//
// 	#region event processing
//
// 		public event PropertyChangedEventHandler PropertyChanged;
//
// 		private void OnPropertyChange([CallerMemberName] string memberName = "")
// 		{
// 			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
// 		}
//
// 	#endregion
//
// 	#region event handeling
//
// 	#endregion
//
// 	#region system overrides
//
// 		public override string ToString()
// 		{
// 			return "this is FileNameAsSheetFileName";
// 		}
//
// 	#endregion
// 	}
// }