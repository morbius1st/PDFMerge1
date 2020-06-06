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

/*
	path

	FilePath<FileNameSheetPdf>
	+-> FileNameSheetPdf
		+-> FileNameSheetComponents (fnc)
			+-> FileNameSheetParser(FileNameSheetComponents)

*/

namespace ClassifierEditor.FilesSupport
{
	public class FileNameSheetPdf : AFileName, INotifyPropertyChanged
	{
	#region private fields

		private FileNameSheetComponents fnc;

		// flags
		private bool parsed = false;
		private bool selected = false;

	#endregion

	#region public fields

		public static string[] SheetNumberComponentTitles { get; } =
		new []
		{
			"Phase/Bldg", "Discipline", "Category", "Sub-Category", "Modifier", "sub-modifier"
		};

	#endregion

	#region ctor

		public FileNameSheetPdf() { }

	#endregion

	#region public properties

		public override string Name
		{
			get => filename;

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

		// sheet number and name parse
		public FileTypeSheetPdf FileType => fnc?.fileType ?? FileTypeSheetPdf.OTHER;

		public string SheetName          => SheetNumber + " :: " + SheetTitle;
		public string SheetNumber        => fnc.phaseBldg + fnc.phaseBldgSep + (fnc.sheetID ?? "n/a");
		public string SheetTitle         => fnc.sheetTitle;
		public string PhaseBldg          => fnc.phaseBldg;
		public string PhaseBldgSep       => fnc.phaseBldgSep;
		public string SheetId            => fnc.sheetID;
		public string Separator          => fnc.separator;
		public string OriginalSheetTitle => fnc.originalSheetTitle;

		public bool Selected
		{
			get => selected;
			set
			{
				selected = value;
				OnPropertyChange();
			}
		}


		public bool IsValid => !Name.IsVoid() && !Extension.IsVoid();

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
			if (parsed || filename.IsVoid() || fileextension.IsVoid()) return false;

			fnc = new FileNameSheetComponents(filename, fileextension);

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

		public override string ToString()
		{
			return "this is FileNameAsSheetFileName";
		}

	#endregion
	}
}