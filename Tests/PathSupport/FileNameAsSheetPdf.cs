#region using directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
#pragma warning disable CS0246 // The type or namespace name 'UtilityLibrary' could not be found (are you missing a using directive or an assembly reference?)
using UtilityLibrary;
#pragma warning restore CS0246 // The type or namespace name 'UtilityLibrary' could not be found (are you missing a using directive or an assembly reference?)

#endregion

// username: jeffs
// created:  5/27/2020 10:53:23 PM

namespace Tests.PathSupport
{
//	public enum FileTypeSheetPdf
//	{
//		SHEET_PDF,
//		NON_SHEET_PDF,
//		OTHER
//	}

#pragma warning disable CS0246 // The type or namespace name 'AFileName' could not be found (are you missing a using directive or an assembly reference?)
	public class FileNameAsSheetPdf : AFileName, INotifyPropertyChanged
#pragma warning restore CS0246 // The type or namespace name 'AFileName' could not be found (are you missing a using directive or an assembly reference?)
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

#pragma warning disable CS0115 // 'FileNameAsSheetPdf.FileNameNoExt': no suitable method found to override
		public override string FileNameNoExt
#pragma warning restore CS0115 // 'FileNameAsSheetPdf.FileNameNoExt': no suitable method found to override
		{
			get => fileNameNoExt;

			set
			{
				fileNameNoExt = value;
				OnPropertyChange();

				parsed = parse();
			}
		}

#pragma warning disable CS0115 // 'FileNameAsSheetPdf.Extension': no suitable method found to override
		public override string ExtensionNoSep
#pragma warning restore CS0115 // 'FileNameAsSheetPdf.Extension': no suitable method found to override
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
		public FileTypeSheetPdf FileType => fnc?.fileType ?? FileTypeSheetPdf.OTHER;

		public string SheetName => SheetNumber + " :: " + SheetTitle;
		public string SheetNumber => fnc.phaseBldg + fnc.phaseBldgSep + (fnc.sheetID ?? "n/a");
		public string SheetTitle => fnc.sheetTitle;
		public string PhaseBldg=> fnc.phaseBldg;
		public string PhaseBldgSep=> fnc.phaseBldgSep;
		public string SheetId => fnc.sheetID;
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

		public new bool IsValid => !FileNameNoExt.IsVoid() && !ExtensionNoSep.IsVoid();

		public bool SheetIdIdsMatch => SheetId.Equals(SheetIdByComponent);

		// sheet Id parse

		public string Discipline  => fnc.discipline;
		public string Category    => fnc.category;
		public string Seperator1  => fnc.seperator1;
		public string Subcategory => fnc.subcategory;
		public string Seperator2  => fnc.seperator2;
		public string Modifier    => fnc.modifier;
		public string Seperator3  => fnc.seperator3;
		public string Submodifier => fnc.submodifier;

		public string SheetIdByComponent
		{
			get
			{
				string shtId = fnc.discipline + fnc.category;

				if (fnc.seperator1 != null)
				{
					shtId += fnc.seperator1 + fnc.subcategory;

					if (fnc.seperator2 != null)
					{
						shtId += fnc.seperator2 + fnc.modifier;

						if (fnc.seperator3 != null)
						{
							shtId += fnc.seperator3 + fnc.submodifier;
						}
					}
				}

				return shtId;
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
			if (parsed || fileNameNoExt.IsVoid() || extensionNoSep.IsVoid()) return false;

			fnc = new FileNameComponentsPDF(fileNameNoExt, extensionNoSep);

//			bool result = FileNameParseSheet.Instance.Parse(fnc, filename);

			if (fnc.success)
			{
				NotifyChange();
			}

			return fnc.success;
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
//			OnPropertyChange("Comment");
			OnPropertyChange("Discipline");
			OnPropertyChange("Category");
			OnPropertyChange("Seperator1");
			OnPropertyChange("Subcategory");
			OnPropertyChange("Seperator2");
			OnPropertyChange("Modifier");
			OnPropertyChange("Seperator3");
			OnPropertyChange("Submodifier");

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

#pragma warning disable CS0115 // 'FileNameAsSheetPdf.ToString()': no suitable method found to override
		public override string ToString()
#pragma warning restore CS0115 // 'FileNameAsSheetPdf.ToString()': no suitable method found to override
		{
			return "this is FileNameAsSheetFileName";
		}

	#endregion
	}
}