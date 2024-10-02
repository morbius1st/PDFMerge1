#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Documents;
using AndyShared.FileSupport.FileNameSheetPDF;
using UtilityLibrary;
// using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
using static Test3.FileNameSheetPDF.SheetIdentifiers3;

#endregion

// username: jeffs
// created:  5/27/2020 10:53:23 PM

// namespace AndyShared.FileSupport.FileNameSheetPDF
namespace UtilityLibrary
{
	// void - do not use
	
	public class FileNameSheetPdf2 : FileNameSimple,
		/*AFileName,*/ INotifyPropertyChanged
	{

	#region private fields

		private static int index;
		
		private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();

		// flags
		private bool? parsed = false;
		private bool isSelected = false;

		// fields being accessed by parse routines

		private bool hasIdentifier;

		private FileTypeSheetPdf fileType;
		private FileNameSheetIdentifiers.ShtIdType2 shtIdType;

		private string sheetID;
		private string originalSheetTitle;
		private string sheetTitle;

		private string sheetName;
		private string sheetNumber;

	#endregion

	#region public fields

	#endregion

	#region ctor

		public FileNameSheetPdf2()
		{
			Index = index++;
		}

	#endregion

	#region public properties

		public List<string> SheetComps { get; set; }

		public override string FileNameNoExt
		{
			get => fileNameNoExt;
			set
			{
				fileNameNoExt = value;
				OnPropertyChange();

				// parsed = parse();
				parsed = parse2();
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
				parsed = parse2();
			}
		}

		public FileNameSheetIdentifiers.ShtIdType2 SheetIdType 
		{
			get => shtIdType;
			private set => shtIdType = value;
		}

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

		public bool IsPhaseBldg => !PhaseBldg.IsVoid();

		public string SheetName
		{
			get => sheetName;
			set => sheetName = value;
		}

		public string SheetNumber
		{
			get => sheetNumber;
			set => sheetNumber = value;
		}

		public string SheetID      {
			get => sheetID;
			set => sheetID = value;
		}

		public string SheetTitle   {
			get=> sheetTitle;
			set => sheetTitle = value;
		}

		public int Index { get; set; }

		public string PhaseBldg      => SheetComps[SI_PHBLDG]        ;
		// public string PhaseBldgSep   => SheetComps[VI_PBSEP]         ;
		public string Discipline     => SheetComps[SI_DISCIPLINE]    ;
		public string Seperator0     => SheetComps[SI_SEP0]          ;
		public string Category       => SheetComps[SI_CATEGORY]      ;
		public string Seperator1     => SheetComps[SI_SEP1]          ;
		public string Subcategory    => SheetComps[SI_SUBCATEGORY]   ;
		public string Seperator2     => SheetComps[SI_SEP2]          ;
		public string Modifier       => SheetComps[SI_MODIFIER]      ;
		public string Seperator3     => SheetComps[SI_SEP3]          ;
		public string Submodifier    => SheetComps[SI_SUBMODIFIER]   ;
		public string Seperator4     => SheetComps[SI_SEP4]          ;
		public string Identifier     => SheetComps[SI_IDENTIFIER]    ;
		public string Seperator5     => SheetComps[SI_SEP5]          ;
		public string Subidentifier  => SheetComps[SI_SUBIDENTIFIER] ;

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
				if (index < SI_MIN || index > SI_MAX ) throw new IndexOutOfRangeException();

				return SheetComps[index];
			}
		}

		public void SetShtIdType(int value)
		{
			if (IsPhaseBldg) value += (int) FileNameSheetIdentifiers.ShtIdType2.ST_IS_PHBLD;

			SheetIdType = (FileNameSheetIdentifiers.ShtIdType2) value;
		}


	#endregion

	#region private methods

		private bool? parse2()
		{
			if (parsed == true || fileNameNoExt.IsVoid() || extensionNoSep.IsVoid()) return false;

			bool? success = parse2(fileNameNoExt, extensionNoSep);

			if (success == true)
			{
				NotifyChange();
			}

			return success;
		}

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
				fileType = FileTypeSheetPdf.SHEET_PDF;

				ConfigFileNameComps();

				// voided - no longer available
				// result = FileNameSheetParser3.Instance.ParseSheetName2(this, filename);

			}
			else
			{
				fileType = FileTypeSheetPdf.NON_SHEET_PDF;
			}


			return result;
		}

		private void ConfigFileNameComps()
		{
			SheetComps = new List<string>();

			for (int i = SI_MIN; i < SI_MAX; i++)
			{
				SheetComps.Add(null);
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