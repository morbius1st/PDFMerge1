using UtilityLibrary;


// Solution:     PDFMerge1
// Project:       ClassifierEditor
// File:             FileNameSheetComponents.cs


//***					***
// FileNameSheetComponents
//***					***




namespace AndyShared.FileSupport.SheetPDF
{

	public class FileNameSheetComponents
	{
	#region private fields

		// private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();

	#endregion

	#region public fields

		// sheet number and name parse
		internal string sheetID;

		internal string separator;
		internal string sheetTitle;

		internal string originalSheetTitle;
		internal FileNameSheetIdentifiers.FileTypeSheetPdf fileType;

		// sheet Id parse
		internal string phaseBldg;
		internal string phaseBldgSep;

		internal string discipline;
		internal string category;
		internal string seperator1;
		internal string subcategory;
		internal string seperator2;
		internal string modifier;
		internal string seperator3;
		internal string submodifier;
		internal string seperator4;
		internal string identifier;
		internal string seperator5;
		internal string subidentifier;
		internal string seperator6;

		// status
		public bool success;

	#endregion

	#region ctor

		public FileNameSheetComponents(string filename, string fileextension)
		{
			success = parse(filename, fileextension);
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private bool parse(string filename, string fileextension)
		{
			if (filename.IsVoid() || fileextension.IsVoid()) return false;

			bool result = true;

			return result;
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FileNameSheetComponents";
		}

	#endregion
	}
}