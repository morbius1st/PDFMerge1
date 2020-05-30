#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  5/27/2020 10:55:08 PM

namespace ClassifierEditor.FilesSupport
{
	public enum FileTypeSheetPdf
	{
		SHEET_PDF,
		NON_SHEET_PDF,
		OTHER
	}

	public class FileNameParseSheet
	{
	#region private fields

		private static readonly Lazy<FileNameParseSheet> instance =
			new Lazy<FileNameParseSheet>(() => new FileNameParseSheet());

		private const string PATTERN2 =
			@"^(?<shtnum>(?<bldgid>[0-9]*[A-Z]*)(?=[ -]+[A-Z])(?<pbsep>[ -]*)(?<shtid1>[A-Z0-9\.-]*[a-z]*)|(?<shtid2>[A-Z]+ *[0-9\.-]+[a-z]*){1})(?<sep>[- ]+)(?>(?<comment1>\(.*\))|(?<shtname2>.*) (?<comment2>\(.*\))(?<ext2>\.[Pp][dD][Ff])|(?<shtname3>.*)(?<ext3>\.[Pp][dD][Ff])|(?<shtname4>.*))";

		private static Regex pattern2 =  new Regex(PATTERN2, RegexOptions.Compiled | RegexOptions.Singleline);

		// capture group names
		private const string PHASE_BLDG_STR     = "bldgid";
		private const string PHASE_BLDG_SEP_STR = "pbsep";
		private const string SHEET_ID_1_STR     = "shtid1";
		private const string SHEET_ID_2_STR     = "shtid2";
		private const string SEPARATOR_STR      = "sep";
		private const string SHEET_NAME_2_STR   = "shtname2";
		private const string SHEET_NAME_3_STR   = "shtname3";
		private const string SHEET_NAME_4_STR   = "shtname4";
		private const string COMMENT_1_STR      = "comment1";
		private const string COMMENT_2_STR      = "comment2";
		private const string EXTENSION_1_STR    = "ext2";
		private const string EXTENSION_2_STR    = "ext3";

	#endregion

	#region ctor

		private FileNameParseSheet() { }

	#endregion

	#region public properties

		public static FileNameParseSheet Instance => instance.Value;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Parse(FileNameComponentsPDF components, string filename)
		{
			Match match = pattern2.Match(filename);

			if (!match.Success) return false;

			GroupCollection g = match.Groups;

			string test;

			components.separator = g[SEPARATOR_STR].Value;

			// phase-bldg
			test = g[PHASE_BLDG_STR].Value;
			if (!string.IsNullOrEmpty(test))
			{
				components.phaseBldg = test;
				components.phaseBldgSep = g[PHASE_BLDG_SEP_STR].Value;
				components.sheetID = g[SHEET_ID_1_STR].Value;
			}
			else
			{
				components.sheetID = g[SHEET_ID_2_STR].Value;
			}

			test = g[COMMENT_1_STR].Value;
			if (string.IsNullOrEmpty(test))
			{
				test = g[COMMENT_2_STR].Value;
				if (!string.IsNullOrEmpty(test))
				{
//					components.comment = test;
					components.originalSheetTitle = g[SHEET_NAME_2_STR].Value;
				}
				else if (!string.IsNullOrEmpty(g[SHEET_NAME_3_STR].Value))
				{
					components.originalSheetTitle = g[SHEET_NAME_3_STR].Value;
				}
				else if (!string.IsNullOrEmpty(g[SHEET_NAME_4_STR].Value))
				{
					components.originalSheetTitle = g[SHEET_NAME_4_STR].Value;
				}
			}
			else
			{
				components.originalSheetTitle = "";
//				components.comment = test;
			}

			components.sheetTitle = components.originalSheetTitle;

			return true;
		}

	#endregion

	#region private methods

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FileNameParse";
		}

	#endregion
	}



	public abstract class FileExtensionClassifier
	{
		public abstract string[] fileTypes { get; set; }

		public bool IsCorrectFileType(string extNoDot)
		{
			string searchText = extNoDot.ToLower();

			foreach (string fileType in fileTypes)
			{
				if (fileType.Equals(searchText)) return true;
			}

			return false;
		}

	}

	public class FileExtensionPdfClassifier : FileExtensionClassifier
	{
		public override string[] fileTypes { get; set; } = {"pdf"};
	}


	//***					***
	// FileNameComponentsPDF
	//***					***

	public class FileNameComponentsPDF
	{
	#region private fields

		private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();

	#endregion

	#region public fields

//		public string filename;
//		public string fileextension;
		public string phaseBldg;
		public string phaseBldgSep;
		public string separator;
		public string sheetID;
		public string sheetTitle;
		public string comment;
		public string originalSheetTitle;
//		public bool selected = false;
		public FileTypeSheetPdf fileType;

	#endregion


	#region ctor

		public FileNameComponentsPDF(string filename, string fileextension, string comment)
		{

			this.comment = comment;

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


			if (fxc.IsCorrectFileType(fileextension))
			{
				if (FileNameParseSheet.Instance.Parse(this, filename))
				{
					fileType = FileTypeSheetPdf.SHEET_PDF;
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

			return true;
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
			return "this is FileNameComponentsPDF";
		}

	#endregion

		//
//		public string Name
//		{
//			get => SheetNumber + " :: " + sheetTitle;
//
//			set
//			{
//				filename = value;
//				OnPropertyChange();
//
//				parsed = parse();
//			}
//		}
//
//		public string Extension
//		{
//			get => fileextension;
//			set
//			{
//				fileextension = value;
//				OnPropertyChange();
//
//				parsed = parse();
//
//			}
//		}
//
//		public FileTypeSheetPdf FileType
//		{
//			get => fileType;
//			set
//			{
//				fileType = value;
//				OnPropertyChange();
//
//				if (fileType == FileTypeSheetPdf.SHEET_PDF)
//				{
//					Selected = true;
//				}
//			}
//		}
//
//		public string SheetNumber
//		{
//			get { return phaseBldg + phaseBldgSep + (sheetID ?? "n/a"); }
//			set
//			{
//				// value is not needed - just ignore
//				OnPropertyChange();
//			}
//		}
//
//		public string SheetTitle { get; set; }
//
//
//		public string PhaseBldg
//		{
//			get => phaseBldg;
//			set
//			{
//				phaseBldg = value;
//				SheetNumber = value;
//			}
//		}
//
//		public string PhaseBldgSep
//		{
//			get => phaseBldgSep;
//			set
//			{
//				phaseBldgSep = value;
//				SheetNumber = value;
//			}
//		}
//
//		public string Separator
//		{
//			get => separator;
//			set => separator = value;
//		}
//
//		public string OriginalSheetTitle
//		{
//			get => originalSheetTitle;
//			set
//			{
//				originalSheetTitle = value;
//				SheetTitle = value;
//			}
//		}
//
//		public bool Selected
//		{
//			get => selected;
//			set
//			{
//				selected = value;
//				OnPropertyChange();
//			}
//		}

	}


}