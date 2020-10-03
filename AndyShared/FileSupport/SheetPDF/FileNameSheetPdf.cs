#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using UtilityLibrary;

using AndyShared.FileSupport.SheetPDF;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.FileTypeSheetPdf;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.pbCompsIdx;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.ShtIdCompsIdx;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.identCompsIdx;
using shtIds = AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers;
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


namespace AndyShared.FileSupport.SheetPDF
{
	public class FileNameSheetPdf : AFileName, INotifyPropertyChanged
	{
	#region private fields

		private FileExtensionPdfClassifier fxc = new FileExtensionPdfClassifier();

		public List<string> PbComps;
		public List<string> ShtIdComps;
		public List<string> IdentComps;


		// flags
		public bool? parsed = false;
		public bool selected = false;

		// true = good, false = invalid, null = unknown / unassigned
		public bool? status = null;
		public bool? isPhaseBldg;
		public bool? hasIdentifier;

		public shtIds.FileTypeSheetPdf fileType;
		public shtIds.ShtCompTypes shtCompType;



		public string sheetID;
		public string originalSheetTitle;



		// sheet number and name parse
		public string separator;
		public string sheetTitle;

		// sheet Id parse
		public string phaseBldg;
		public string phaseBldgSep;

		public string discipline;
		public string category;
		public string seperator11;
		public string subcategory;
		public string seperator12;
		public string modifier;
		public string seperator13;
		public string submodifier;
		public string seperator1;
		public string identifier;
		public string seperator2;
		public string subidentifier;
		public string seperator3;

		// status
		// private bool success;



	#endregion

	#region public fields
		
		public static string[] SheetNumberComponentTitles { get; } =
		new []
		{
			"Phase/Bldg", "Discipline", "Category", "Sub-Category", "Modifier", "sub-modifier", "Identifier", "Sub-Identifier"
		};

	#endregion

	#region ctor

	#endregion

	#region public properties

		public override string FileNameNoExt
		{
			get => fileNameNoExt;

			set
			{
				fileNameNoExt = value;
				OnPropertyChange();

				parsed = parse();
			}
		}

		public override string ExtensionNoSep
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
		public shtIds.FileTypeSheetPdf FileType => fileType;

		// todo write indexer
		// public string this[int idx]
		// {
		// 	get
		// 	{
		// 		if (idx >= SheetNumberComponentTitles.Length
		// 			|| idx < 0) throw new IndexOutOfRangeException();
		//
		// 		switch (idx)
		// 		{
		//
		// 		}
		//
		//
		// 	}
		// }

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

		public string OriginalSheetTitle => originalSheetTitle ?? "orig sht ttl";


		public string SheetName          => SheetNumber + " :: " + SheetTitle;
		public string SheetNumber2        => (phaseBldg + phaseBldgSep + sheetID) ?? "sht number";
		public string SheetNumber        => (PbComps[PHBLDGIDX] + PbComps[PBSEPIDX] + sheetID) ?? "sht number";
		public string SheetTitle         => sheetTitle ?? "sht ttl";
		public string PhaseBldg          => phaseBldg ?? "phase-bldg";
		public string PhaseBldgSep       => phaseBldgSep ?? "pb sep";
		public string SheetId            => sheetID ?? "sht id";
		public string Separator          => separator;



		// sheet Id parse

		public string Discipline  => discipline ?? "discipline";
		public string Category    => category ?? "category";
		public string Seperator1  => seperator1;
		public string Subcategory => subcategory ?? "sub-category";
		public string Seperator2  => seperator2;
		public string Modifier    => modifier ?? "modifier";
		public string Seperator3  => seperator3;
		public string Submodifier => submodifier ?? "sub-modifier";

		public string SheetIdByComponent
		{
			get
			{
				string shtId = discipline + category;

				if (seperator1 != null)
				{
					shtId += seperator1 + subcategory;

					if (seperator2 != null)
					{
						shtId += seperator2 + modifier;

						if (seperator3 != null)
						{
							shtId += seperator3 + submodifier;
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

		private bool? parse()
		{
			if (parsed == true || fileNameNoExt.IsVoid() || extensionNoSep.IsVoid()) return false;

			bool? success = parse(fileNameNoExt, extensionNoSep);

			if (success == true)
			{
				NotifyChange();
			}

			return success;
		}

		private bool parse(string filename, string fileextension)
		{
			// unknown status
			status = null;

			if (filename.IsVoid() || fileextension.IsVoid())
			{
				// status is invalid
				status = false;
				return false;
			}

			bool result = true;

			if (fxc.IsCorrectFileType(fileextension))
			{
				ConfigFileNameComps();

				if (FileNameSheetParser.Instance.Parse2(this, filename))
				{
					fileType = SHEET_PDF;

					result = FileNameSheetParser.Instance.ParseSheetId2(this, sheetID);
				}
				else
				{
					fileType = NON_SHEET_PDF;
				}
			}
			else
			{
				fileType = OTHER;
			}

			return result;
		}

		private void ConfigFileNameComps()
		{
			PbComps = new List<string>();

			for (int i = 0; i < (int) CI_PBCOUNT; i++)
			{

				PbComps.Add(null);
			}

			ShtIdComps = new List<string>();

			for (int i = 0; i < (int) CI_TYPECOUNT; i++)
			{
				
				ShtIdComps.Add(null);
			}

			IdentComps = new List<string>();

			for (int i = 0; i < (int) CI_IDENTCOUNT; i++)
			{

				IdentComps.Add(null);
			}

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