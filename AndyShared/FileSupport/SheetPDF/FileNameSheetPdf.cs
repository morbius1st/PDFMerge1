#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using UtilityLibrary;
using AndyShared.FileSupport.SheetPDF;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.FileTypeSheetPdf;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.pbCompsIdx;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.ShtIdCompsIdx;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.identCompsIdx;
using shtIds = AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers;

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


namespace AndyShared.FileSupport.SheetPDF
{
	public class FileNameSheetPdf : AFileName, INotifyPropertyChanged
	{
	#region private fields
		
		private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();

		// true = good, false = invalid, null = unknown / unassigned
		private bool? status = null;

		// flags
		private bool? parsed = false;
		private bool selected = false;

		// fields being accessed by parse routines
		internal List<string> PbComps;
		internal List<string> ShtIdComps;
		internal List<string> IdentComps;

		internal bool? isPhaseBldg;
		internal bool? hasIdentifier;

		internal shtIds.FileTypeSheetPdf fileType;
		internal shtIds.ShtCompTypes shtCompType;

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
		public shtIds.FileTypeSheetPdf FileType => fileType;

		public ShtCompTypes SheetComponentType => shtCompType;

		public bool Selected
		{
			get => selected;
			set
			{
				selected = value;
				OnPropertyChange();
			}
		}

		public new bool IsValid => !FileNameNoExt.IsVoid() && !ExtensionNoSep.IsVoid();

		public bool? IsPhaseBldg => isPhaseBldg;

		public bool? HasIdentifier => hasIdentifier;

		public string OriginalSheetTitle => originalSheetTitle;

		public string SheetName    => SheetNumber + " :: " + SheetTitle;

		// public string SheetNumber2 => (phaseBldg + phaseBldgSep + sheetID) ?? "sht number";
		public string SheetNumber  => (PbComps[PHBLDGIDX] + PbComps[PBSEPIDX] + sheetID);
		public string SheetId      => sheetID;
		public string SheetTitle   => sheetTitle;

		// phase bldg parse
		public string PhaseBldg    => PbComps[PHBLDGIDX];
		public string PhaseBldgSep => PbComps[PBSEPIDX];

		// sheet Id parse
		public string Discipline     => ShtIdComps[DISCIPLINEIDX]     ;
		public string Seperator0     => ShtIdComps[SEP0IDX]           ;
		public string Category       => ShtIdComps[CATEGORYIDX]       ;
		public string Seperator1     => ShtIdComps[SEP1IDX]           ;
		public string Subcategory    => ShtIdComps[SUBCATEGORYIDX]    ;
		public string Seperator2     => ShtIdComps[SEP2IDX]           ;
		public string Modifier       => ShtIdComps[MODIFIERIDX]       ;
		public string Seperator3     => ShtIdComps[SEP3IDX]           ;
		public string Submodifier    => ShtIdComps[SUBMODIFIERIDX]    ;
		public string Seperator4     => ShtIdComps[SEP4IDX]           ;
		public string Identifier     => ShtIdComps[IDENTIFIERIDX]     ;
		public string Seperator5     => ShtIdComps[SEP5IDX]           ;
		public string subidentifier  => ShtIdComps[SUBIDENTIFIERIDX]  ;
		public string Seperator6     => ShtIdComps[SEP6IDX]           ;

	#endregion

	#region private properties

	#endregion

	#region public methods

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
			// unknown status
			status = null;

			if (filename.IsVoid() || fileextension.IsVoid())
			{
				// status is invalid
				status = false;
				return false;
			}

			bool result = true;

			if (fxc.IsCorrectFileType(fileextension))
			{
				ConfigFileNameComps();

				if (FileNameSheetParser.Instance.Parse2(this, filename))
				{
					fileType = SHEET_PDF;

					result = FileNameSheetParser.Instance.ParseSheetId2(this, sheetID);
				}
				else
				{
					fileType = NON_SHEET_PDF;
				}
			}
			else
			{
				fileType = OTHER;
			}

			return result;
		}

		private void ConfigFileNameComps()
		{
			PbComps = new List<string>();

			for (int i = 0; i < (int) CI_PBCOUNT; i++)
			{
				PbComps.Add(null);
			}

			ShtIdComps = new List<string>();

			for (int i = 0; i < (int) CI_TYPECOUNT; i++)
			{
				ShtIdComps.Add(null);
			}

			IdentComps = new List<string>();

			for (int i = 0; i < (int) CI_IDENTCOUNT; i++)
			{
				IdentComps.Add(null);
			}
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