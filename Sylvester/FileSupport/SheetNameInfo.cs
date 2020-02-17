// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SheetNameInfo.cs
// Created:      -- ()

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Sylvester.Process;
using UtilityLibrary;

namespace Sylvester.FileSupport
{
	public enum FileType
	{
		SHEET_PDF,
		NON_SHEET_PDF,
		OTHER
	}

	public abstract class SheetNameInfo : INotifyPropertyChanged
	{
		public FolderType FolderType => FolderType.UNASSIGNED;

		public const string FILE_TYPE_EXT = ".pdf";

		protected const string SEARCH_PATTERN_A = @"([ \.\-]+)";
		protected const string SUBST_PATTERN_A = @" ";

		protected const string SEARCH_PATTERN_B = @"(?<=[A-Z])([ -]+)(?=[0-9])";
		protected const string SUBST_PATTERN_B = @"";

		// turns single digit sheet numbers into two digit sheet numbers
		protected const string SEARCH_PATTERN_C = @"^([A-Z]+)([0-9](?![0-9]))";
		protected const string SUBST_PATTERN_C = @"${1}0$2";

		protected KeyValuePair<Regex, string>[] ReplacePatterns =
		{
			new KeyValuePair<Regex, string>(
				new Regex(SEARCH_PATTERN_A, RegexOptions.Compiled | RegexOptions.Singleline),
				SUBST_PATTERN_A
				),
			new KeyValuePair<Regex, string>(
				new Regex(SEARCH_PATTERN_C, RegexOptions.Compiled | RegexOptions.Singleline),
				SUBST_PATTERN_C
				)
		};

		protected Regex regex = new Regex(SEARCH_PATTERN_A, RegexOptions.Compiled | RegexOptions.Singleline);

		protected FilePath<FileNameSimple> fullFileRoute;
		protected string phaseBldg;
		protected string phaseBldgSep;
		protected string sheetID;
		protected string separator;
		protected string originalSheetTitle;
		protected string sheetTitle;
		protected string comment;
		protected string adjustedSheetID;
		private bool selected = false;
		private FileType fileType;

		public SheetNameInfo()
		{
			fullFileRoute = UtilityLibrary.FilePath<FileNameSimple>.Invalid;

			phaseBldg = "";
			phaseBldgSep = "";
			sheetID = "";
			separator = "";
			sheetTitle = "";
			comment = "";
			adjustedSheetID = "";
		}


		public abstract int FolderTypeValue { get; }

		public bool PreSelect { get; set; } = false;

		public FilePath<FileNameSimple> FullFileRoute
		{
			get => fullFileRoute;
			set
			{
				fullFileRoute = value;

				if (PreSelect && fullFileRoute.GetFileExtension.ToUpper().Equals(".PDF"))
				{
					Selected = true;
				}

				OnPropertyChange();
			}
		}

		public string SheetID
		{
			get => sheetID;
			set
			{
				sheetID = value;
				OnPropertyChange();

				SheetNumber = value;

				AdjustedSheetId = AdjustSheetNumber(sheetID);
			}
		}

		public string FilePath => fullFileRoute.GetPath;

		public string FileName => fullFileRoute.GetFileName;
		
		public FileType FileType
		{
			get => fileType;
			set
			{
				fileType = value;
				OnPropertyChange();
			}
		}

		public string PhaseBldg
		{
			get => phaseBldg;
			set
			{
				phaseBldg = value;
				SheetNumber = value;
			}
		}

		public string PhaseBldgSep
		{
			get => phaseBldgSep;
			set
			{
				phaseBldgSep = value;
				SheetNumber = value;
			}
		}

		public string SheetNumber
		{
			get { return phaseBldg + phaseBldgSep + (sheetID ?? "n/a"); }
			set
			{
				// value is not needed - just ignore
				OnPropertyChange();
			}
		}

		public string AdjustedSheetId
		{
			get => adjustedSheetID;
			set
			{
				adjustedSheetID = value;
				OnPropertyChange();
			}
		}

		public string Separator
		{
			get => separator;
			set => separator = value;
		}

		public string OriginalSheetTitle
		{
			get => originalSheetTitle;
			set
			{
				originalSheetTitle = value;
				SheetTitle = value;
			}
		}

		public abstract string SheetTitle { get; set; }

		public string SheetName 
		{ 
			get
			{
				string result =  SheetNumber + Separator + SheetTitle;

				if (fileType != FileType.SHEET_PDF)
				{
					result = FileName;
				}

				return result;
			}
	}

		public string Comment
		{
			get => comment;
			set
			{
				comment = value;
				OnPropertyChange();
			}
		}

		public bool Selected
		{
			get => selected;
			set
			{
				selected = value;
				OnPropertyChange();
			}
		}

		private bool ParseFile()
		{
			return Support.Support.ParseFileName(this);
		}

		public void Initalize()
		{
			if (fullFileRoute.GetFileExtension.Equals(FILE_TYPE_EXT))
			{
				if (ParseFile())
				{
					FileType = FileType.SHEET_PDF;
				}
				else
				{
					FileType = FileType.NON_SHEET_PDF;
					AdjustedSheetId = "Ⓩ";
				}
			}
			else
			{
				FileType = FileType.OTHER;
				AdjustedSheetId = "Ⓩ";
			}
		}

		public abstract void UpdateSelectStatus();

		protected string AdjustSheetNumber(string sheetId)
		{
			string result = sheetId;

			for (int i = 0; i < ReplacePatterns.Length; i++)
			{
				result =
					ReplacePatterns[i].Key.Replace(result, ReplacePatterns[i].Value).Trim();
			}

			return result;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return phaseBldg.PadRight(3)
				+ " | " + phaseBldgSep.PadRight(4)
				+ " | " + sheetID.PadRight(10)
				+ (" (" + adjustedSheetID + ")").PadRight(10)
				+ " | " + separator.PadRight(6)
				+ " | " + sheetTitle
				+ " ( " + comment + ")";
		}

		public object Clone<T>() where T : SheetNameInfo, new()
		{
			T t = new T();

			t.fullFileRoute   = this.fullFileRoute;
			t.sheetID         = this.sheetID;
			t.fileType        = this.fileType;
			t.phaseBldg       = this.phaseBldg;
			t.phaseBldgSep    = this.phaseBldgSep;
			t.adjustedSheetID = this.adjustedSheetID;
			t.separator       = this.separator;
			t.sheetTitle       = this.sheetTitle;
			t.comment         = this.comment;

			return t;
		}
	}
}