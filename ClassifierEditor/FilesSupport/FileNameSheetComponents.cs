using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilityLibrary;

// Solution:     PDFMerge1
// Project:       ClassifierEditor
// File:             FileNameSheetComponents.cs


//***					***
// FileNameSheetComponents
//***					***

namespace ClassifierEditor.FilesSupport 
{

	public class FileNameSheetComponents
	{
	#region private fields

		private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();

	#endregion

	#region public fields

		// sheet number and name parse
		public string phaseBldg;
		public string phaseBldgSep;
		public string sheetID;

		public string separator;
		public string sheetTitle;

//		public string comment;
		public string originalSheetTitle;
		public FileTypeSheetPdf fileType;

		// sheet Id parse
		public string discipline;
		public string category;
		public string seperator1;
		public string subcategory;
		public string seperator2;
		public string modifier;
		public string seperator3;
		public string submodifier;

		// status
		public bool success;

	#endregion

	#region ctor

		public FileNameSheetComponents(string filename, string fileextension)
		{
			parse(filename, fileextension);

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

			if (fxc.IsCorrectFileType(fileextension))
			{
				if (FileNameSheetParser.Instance.Parse(this, filename))
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

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FileNameSheetComponents";
		}

	#endregion

	}
}