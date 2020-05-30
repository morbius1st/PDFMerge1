#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  5/27/2020 10:53:23 PM

namespace ClassifierEditor.FilesSupport
{
//	public enum FileTypeSheetPdf
//	{
//		SHEET_PDF,
//		NON_SHEET_PDF,
//		OTHER
//	}

	public class FileNameAsSheetPdf : AFileName, INotifyPropertyChanged
	{
	#region private fields

		private bool selected = false;
		private FileNameComponentsPDF fnc;

		// flags
		private bool parsed = false;

	#endregion

	#region ctor

		public FileNameAsSheetPdf() { }

	#endregion

	#region public properties

		public override string Name
		{
			get => SheetNumber + " :: " + fnc.sheetTitle;

			set
			{
				filename = value;
				OnPropertyChange();

				parsed = parse();
			}
		}

		public override string Extension
		{
			get => fileextension;
			set
			{
				fileextension = value;
				OnPropertyChange();

				parsed = parse();

			}
		}

		public FileTypeSheetPdf FileType=> fnc.fileType;

		public string SheetNumber => fnc.phaseBldg + fnc.phaseBldgSep + (fnc.sheetID ?? "n/a");

		public string SheetTitle => fnc.sheetTitle;

		public string PhaseBldg=> fnc.phaseBldg;

		public string PhaseBldgSep=> fnc.phaseBldgSep;

		public string Separator=> fnc.separator;

		public string OriginalSheetTitle=> fnc.originalSheetTitle;

		public bool Selected
		{
			get => selected;
			set
			{
				selected = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private bool parse()
		{
			if (parsed || filename.IsVoid() || fileextension.IsVoid()) return false;

			fnc = new FileNameComponentsPDF(filename, fileextension, "none");

			bool result = FileNameParseSheet.Instance.Parse(fnc, filename);

			if (result)
			{
				NotifyChange();
			}

			return result;
		}

		private void NotifyChange()
		{
			OnPropertyChange("FileType");
			OnPropertyChange("SheetNumber");
			OnPropertyChange("OriginalSheetTitle");

			OnPropertyChange("PhaseBldg");
			OnPropertyChange("PhaseBldgSep");
			OnPropertyChange("Separator");
			OnPropertyChange("SheetId");
			OnPropertyChange("SheetTitle");
			OnPropertyChange("Comment");
			OnPropertyChange("");
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