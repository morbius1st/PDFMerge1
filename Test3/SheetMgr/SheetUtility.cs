using System.Collections.Generic;
using Test3.SheetMgr;


// Solution:     PDFMerge1
// Project:      Test3
// File:         SheetsUtility.cs
// Created:      -- ()

namespace Test3
{
	public class SheetUtility
	{
		public static void test()
		{
			SheetManager sheetManager = new SheetManager();

			Config();

			bool result = false;

			foreach (string sheet in Samples.fullRange)
			{
				result = sheetManager.Add(sheet);

				if (!result) break;
			}

			if (!result)
				AppendLine("*** parse sheets failed ***");
			else
				listSheets(sheetManager);
		}


		public static void listSheets(SheetManager sheetManager)
		{
			List<SheetTypeInfo> sheetTypeInfo =
				SheetSystemManager.SheetTypeManager.SheetTypes;

			SheetPartsDescriptor sheetDescriptor;

			SheetPartData sheetPartData;


			TbxClear();

			foreach (Sheet sheet in sheetManager.SheetList)
			{
				sheetDescriptor =
					sheetTypeInfo[sheet.SheetTypeIdx].SheetPartsDescriptor;

				AppendLine("\nSheetId     | " + sheet.SheetId);
				AppendLine("is reference| " + sheet.IsForRefernce);
				AppendLine("sheet type  | "
					+ sheetTypeInfo[sheet.SheetTypeIdx].Description);

				AppendLine("sheet num   | " + sheet.SheetNumber);
				AppendLine("sheet name  | " + sheet.SheetName );

				ListParts(sheet, sheet.SheetIdPartsSheetNumber);
				ListParts(sheet, sheet.SheetIdPartsSheetName);

			}
		}

		private static void ListParts(Sheet sheet, List<Sheet.SheetIdPart> parts)
		{
			foreach (Sheet.SheetIdPart part in parts)
			{
				string description = sheet.SheetPartDescription(part.SheetPartLibId);

				string value = part.SheetidPartData;

				AppendLine("part name   | "
					+ description.PadRight(15)
					+ "   part info| " + value);
			}
		}

/*
	adjusted to discount the .PDF extension
			regex for building, no phase:
				@"^(([A-Z]+) ([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)"
			regex for phase, no building:
				@"^(([0-9]+) ([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)"
			regex for phase and building:
				@"^(([0-9]+[A-Z]+) ([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)"
			regex for non phase-building:
				@"^(([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)"

		discipline specific - must occur before above general sheet types
			regex for civil sheets
				@"^((C)\d+(\.\d+\-\w+|\.\d+| ))(.*)"
			regex for landscape sheets
				@"^((L)\d+(\.\d+\-\w+|\.\d+| ))(.*)"
*/


		public static void Config()
		{
			SheetSystemManager ShtSysMgr = new SheetSystemManager();

			SheetSystemManager.MaxSheetParts = 4;
			SheetSystemManager.MinSheetParts = 2;

			shtConfigSheetParts(ShtSysMgr);
			shtSysConfigPhaseBldg(ShtSysMgr); // phase / building
			shtSysConfigPhaseNoBldg(ShtSysMgr); // phase / no building
			shtSysConfigNoPhaseBldg(ShtSysMgr); // no phase / building
			shtSysConfigNoBldgPhase(ShtSysMgr); // no phase / no building
		}

		private const string SP_BLDG_PHASE_PHASE_ID       = "sp-phase-bldg-PhaseId";
		private const string SP_BLDG_PHASE_BLDG_ID        = "sp-phase-bldg-BuildingId";
		private const string SP_BLDG_PHASE_SHEET_ID       = "sp-phase-bldg-ShtId";
		private const string SP_BLDG_PHASE_SHEET_NAME     = "sp-phase-bldg-ShtName";

		private const string SP_NON_BLDG_PHASE_SHEET_ID   = "sp-non-phase-non-bldg-ShtId";
		private const string SP_NON_BLDG_PHASE_SHEET_NAME = "sp-non-phase-non-bldg-ShtName";

		private const string ST_NON_PHASE_NON_BLDG        = "st-non-phase-non-bldg";
		private const string ST_PHASE_NON_BLDG            = "st-phase-non-bldg";
		private const string ST_NON_PHASE_BLDG            = "st-non-phase-bldg";
		private const string ST_PHASE_BLDG                = "st-phase-bldg";

		private static void shtConfigSheetParts(SheetSystemManager ShtSysMgr)
		{
			SheetPartData[] sheetParts = new []
			{
				new SheetPartData(SP_BLDG_PHASE_PHASE_ID, PartIdType.SHEETNUMBER, 1,
					"Phase Id",
					@"^(\d*)", 0, 1, 10, "{0}"),
				new SheetPartData(SP_BLDG_PHASE_BLDG_ID, PartIdType.SHEETNUMBER, 2,
					"Building Id",
					@"^\d*([A-Z]*)", 1, 1, 10, "{0}"),
				new SheetPartData(SP_BLDG_PHASE_SHEET_ID, PartIdType.SHEETNUMBER, 3,
					"Sheet Id",
					@"^.*? (.*?) ", 1, 2, 20, " {0}"),
				new SheetPartData(SP_BLDG_PHASE_SHEET_NAME, PartIdType.SHEETNAME, 1,
					"Sheet Name",
					@"^.*? .*? (.*(?=\.[Pp][Dd][Ff])|.*)",
					1, 2, 0, "{0}"),
				new SheetPartData(SP_NON_BLDG_PHASE_SHEET_ID, PartIdType.SHEETNUMBER, 1,
					"Sheet Id", @"^(.*?) ", 1, 2, 20, "{0}"),
				new SheetPartData(SP_NON_BLDG_PHASE_SHEET_NAME, PartIdType.SHEETNAME, 1,
					"Sheet Name", @"^.*? (.*(?=\.[Pp][Dd][Ff])|.*)", 1, 2, 0, "{0}")
			};

			ShtSysMgr.AddSheetPartData(sheetParts);
		}


		private static void shtSysConfigNoBldgPhase(SheetSystemManager ShtSysMgr)
		{
			ShtSysMgr.ConfigSheetType(
				ST_NON_PHASE_NON_BLDG,
				"Non-phase-building Sheets",
				@"^(([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)", 2, false,
				new []
				{
					SP_NON_BLDG_PHASE_SHEET_ID,
					SP_NON_BLDG_PHASE_SHEET_NAME
				});
		}

		private static void shtSysConfigPhaseBldg(SheetSystemManager ShtSysMgr)
		{
			ShtSysMgr.ConfigSheetType(
				ST_PHASE_BLDG,
				"Phase and bldg sheets",
				@"^(([0-9]+[A-Z]+) ([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)",
				3, false,
				new []
				{
					SP_BLDG_PHASE_PHASE_ID,
					SP_BLDG_PHASE_BLDG_ID,
					SP_BLDG_PHASE_SHEET_ID,
					SP_BLDG_PHASE_SHEET_NAME
				});
		}

		private static void shtSysConfigNoPhaseBldg(SheetSystemManager ShtSysMgr)
		{
			ShtSysMgr.ConfigSheetType(
				ST_NON_PHASE_BLDG,
				"Bldg, no-phase sheets",
				@"^(([A-Z]+) ([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)",
				3, false,
				new []
				{
					SP_BLDG_PHASE_BLDG_ID,
					SP_BLDG_PHASE_SHEET_ID,
					SP_BLDG_PHASE_SHEET_NAME
				});
		}

		private static void shtSysConfigPhaseNoBldg(SheetSystemManager ShtSysMgr)
		{
			ShtSysMgr.ConfigSheetType(
				ST_PHASE_NON_BLDG,
				"Phase, no-bldg sheets",
				@"^(([0-9]+) ([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)",
				3, false,
				new []
				{
					SP_BLDG_PHASE_PHASE_ID,
					SP_BLDG_PHASE_SHEET_ID,
					SP_BLDG_PHASE_SHEET_NAME
				});
		}

		private static void TbxClear()
		{
			MainWindow.Instance.tbk1.Clear();
		}

		private static void Append(string msg)
		{
			MainWindow.Instance.tbk1.AppendText(msg);
		}

		private static void AppendLine(string msg)
		{
			Append(msg + MainWindow.nl);
		}
	}
}