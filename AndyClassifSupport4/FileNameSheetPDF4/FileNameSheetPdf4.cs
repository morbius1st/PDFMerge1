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
using static AndyShared.FileSupport.FileNameSheetPDF4.SheetIdentifiers4;
#endregion

// username: jeffs
// created:  9/15/2024 9:43:31 AM

namespace AndyShared.FileSupport.FileNameSheetPDF4
{
	public partial class FileNameSheetPdf4 : FileNameSimple
	{
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
		private FileNameParseStatusCodes statusCode = FileNameParseStatusCodes.SC_NONE;

	#endregion

	#region ctor

		public FileNameSheetPdf4()
		{
			Index = index++;
		}

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


		public FileNameParseStatusCodes StatusCode
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

		public int Index { get; }

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
				if (index < ShtIds2.IDX_XLATE_MIN || index > ShtIds2.IDX_XLATE_MAX ) throw new IndexOutOfRangeException();

				return shtNumber.ShtNumComps[IDX_XLATE[index]];
			}
		}

	#endregion

	#region private methods

		private bool splitFileName()
		{
			string[] parts = fileNameNoExt.Split(new [] {" - "}, StringSplitOptions.None);

			if (parts.Length != 2 )
			{
				parsed = false;
				StatusCode = FileNameParseStatusCodes.SC_INVALID_FILENAME;
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

			parsed = StatusCode == FileNameParseStatusCodes.SC_NONE ? true : false;

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