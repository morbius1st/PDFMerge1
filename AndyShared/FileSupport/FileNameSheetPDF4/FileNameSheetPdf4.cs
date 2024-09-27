#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AndyShared.FileSupport.FileNameSheetPDF;
using UtilityLibrary;
using SettingsManager;

#endregion

// username: jeffs
// created:  9/15/2024 9:43:31 AM

namespace AndyShared.FileSupport.FileNameSheetPDF4
{
	public class FileNameSheetPdf4 : FileNameSimple
	{
		public enum StatusCodes
		{
			SC_NONE = 0,
			SC_VOID_SHTNUM = -1,
			SC_INVALID_FILENAME = -2,

			SC_PARSER_NOT_CONFIGD=-102,
			SC_PARSER_MATCH_FAILED = -103,
			SC_PARSER_COMP_EXTRACT_FAILED=-104,
		}

	#region private fields

		private static int index;

		private string origShtNumber;
		private string shtTitle;

		// flags

		// false == fail / not parsed / null = divided only / true = parse complete & worked
		private bool? parsed;  
		private bool isSelected = false;

		// objects
		private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();

		private ShtNumber4 shtNumber;
		private StatusCodes statusCode = StatusCodes.SC_NONE;

	#endregion

	#region ctor

		public FileNameSheetPdf4() { }

	#endregion

	#region public properties

		public override string FileNameNoExt
		{
			get => fileNameNoExt;
			set
			{
				fileNameNoExt = value;
				OnPropertyChange();

				config();
			}
		}

		public override string ExtensionNoSep
		{
			get => extensionNoSep;
			set
			{
				extensionNoSep = value;
				OnPropertyChange();
			}
		}

		public ShtNumber4 ShtNumber => shtNumber;

		public string SheetNumber => shtNumber.ParsedSheetNumber;
		public string SheetId => shtNumber.ParsedSheetId;


		public StatusCodes StatusCode
		{
			get => statusCode;
			private set
			{
				statusCode = value;
				OnPropertyChange();
			}
		}

		public string OrigShtNumber
		{
			get => origShtNumber;
			set
			{
				origShtNumber = value;
				OnPropertyChange();
			}
		}

		public string SheetTitle
		{
			get => shtTitle;
			set
			{
				shtTitle = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private bool splitFileName()
		{
			string[] parts = fileNameNoExt.Split(new [] {" - "}, StringSplitOptions.None);

			if (parts.Length != 2 )
			{
				parsed = false;
				StatusCode = StatusCodes.SC_INVALID_FILENAME;
				return false;
			}

			parsed = null;
			OrigShtNumber = parts[0];
			SheetTitle = parts[1];

			return true;
		}

		private void config()
		{
			if (!fxc.IsCorrectFileType(extensionNoSep) || !splitFileName())
			{
				shtNumber = new ShtNumber4(null);

				return;
			}

			shtNumber = new ShtNumber4(OrigShtNumber, true);

			StatusCode = shtNumber.StatusCode;

			parsed = StatusCode == StatusCodes.SC_NONE ? true : false;

		}


	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public void NotifyChanges()
		{
			OnPropertyChange(nameof(ShtNumber));
			OnPropertyChange(nameof(SheetNumber));
			OnPropertyChange(nameof(SheetId));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(FileNameSheetPdf4)}";
		}

	#endregion
	}
}