// Solution:     PDFMerge1
// Project:       Test3
// File:             SheetsUtility.cs
// Created:      -- ()

using System.Collections.Generic;
using Test3.SheetMgr;
using static Test3.MainWindow;

namespace Test3
{
	public class SheetUtility
	{
		public const string SHEET_PATTERN = @"^\s*(\d*|)([A-Z]*|) ([^ ]*) (.*)$";

		public static void Config()
		{
			SheetSystemManager ShtSysMgr = new SheetSystemManager();

			SheetSystemManager.MaxSheetParts = 4;
			SheetSystemManager.MinSheetParts = 2;

			shtSysConfigNoBldgPhase(ShtSysMgr);	// no phase / no building
			shtSysConfigBldgNoPhase(ShtSysMgr);	// no phase / building
			// phase / no building
			// phase / building

//			SheetPartData[] sheetParts = new []
//			{
//				new SheetPartData(PartIdType.SHEETNUMBER, 1,
//					"Sheet Id", @"^(.*?) ", 1, 2, 20, "{0}"),
//				new SheetPartData(PartIdType.SHEETNAME, 1,
//					"Sheet Name", @"^.*? (.*(?=\.[Pp][Dd][Ff])|.*)", 1, 2, 100, "{0}")
//			};
//
//			ShtSysMgr.StartSheetType("Non-building-phase Sheets");
//
//			ShtSysMgr.AddSheetPartData(sheetParts);
//
//			ShtSysMgr.CompleteSheetType(@"^(([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)", 2,
//				false);

//
//			SheetPartData[] sheetParts = new []
//			{
//				new SheetPartData(PartIdType.SHEETNUMBER, 1, "Building Id",
//					@"^\d*([A-Z]*)", 0, 1, 10, "{0}"),
//				new SheetPartData(PartIdType.SHEETNUMBER, 2, "Sheet Id", @"^.*? (.*?) ",
//					1, 2, 20, " {0}"),
//				new SheetPartData(PartIdType.SHEETNAME, 1, "Sheet Name", @"^.*? .*? (.*(?=\.[Pp][Dd][Ff])|.*)",
//					0, 2, 100, "{0}")
//			};
//
//			ShtSysMgr.StartSheetType("Bldg, no-phase sheets");
//
//			ShtSysMgr.AddSheetPartData(sheetParts);
//
//			ShtSysMgr.CompleteSheetType(
//				@"^(([A-Z]+) ([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*(?=\.[Pp][Dd][Ff])|.*)",
//				3, false);

		}

		private static void shtSysConfigBldgNoPhase(SheetSystemManager ShtSysMgr)
		{
			SheetPartData[] sheetParts = new []
			{
				new SheetPartData(PartIdType.SHEETNUMBER, 1, "Building Id",
					@"^\d*([A-Z]*)", 0, 1, 10, "{0}"),
				new SheetPartData(PartIdType.SHEETNUMBER, 2, "Sheet Id", @"^.*? (.*?) ",
					1, 2, 20, " {0}"),
				new SheetPartData(PartIdType.SHEETNAME, 1, "Sheet Name",
					@"^.*? .*? (.*(?=\.[Pp][Dd][Ff])|.*)",
					0, 2, 100, "{0}")
			};

			ShtSysMgr.StartSheetType("Bldg, no-phase sheets");

			ShtSysMgr.AddSheetPartData(sheetParts);

			ShtSysMgr.CompleteSheetType(
				@"^(([A-Z]+) ([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*(?=\.[Pp][Dd][Ff])|.*)",
				3, false);
		}

		private static void shtSysConfigNoBldgPhase(SheetSystemManager ShtSysMgr)
		{
			SheetPartData[] sheetParts = new []
			{
				new SheetPartData(PartIdType.SHEETNUMBER, 1,
					"Sheet Id", @"^(.*?) ", 1, 2, 20, "{0}"),
				new SheetPartData(PartIdType.SHEETNAME, 1,
					"Sheet Name", @"^.*? (.*(?=\.[Pp][Dd][Ff])|.*)", 1, 2, 100, "{0}")
			};

			ShtSysMgr.StartSheetType("Non-building-phase Sheets");

			ShtSysMgr.AddSheetPartData(sheetParts);

			ShtSysMgr.CompleteSheetType(@"^(([A-Z]+?)\d+(\.\d+\-\w+|\.\d+| ))(.*)", 2,
				false);
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


		public static void test()
		{
			SheetManager sheetManager = new SheetManager();

			Config();

			foreach (string sheet in Samples.normal)
			{
				sheetManager.Add(sheet);
			}

			listSheets(sheetManager);
		}


		public static void listSheets(SheetManager sheetManager)
		{
			List<SheetTypeInfo> sheetTypeInfo =
				SheetSystemManager.SheetTypeManager.SheetTypes;

			List<SheetPartData> sheetPartList =
				SheetSystemManager.SheetPartsManager.SheetPartList;

			SheetPartsDescriptor sheetDescriptor;

			SheetPartData sheetPartData;


			TbxClear();

			foreach (Sheet sheet in sheetManager.SheetList)
			{
				sheetDescriptor =
					sheetTypeInfo[sheet.SheetTypeIdx].SheetPartsDescriptor;

				AppendLine("\nwhole sheet | " + sheet.WholeId);
				AppendLine("is reference| " + sheet.IsForRefernce);
				AppendLine("sheet type  | "
					+ sheetTypeInfo[sheet.SheetTypeIdx].Description);

				AppendLine("sheet name  | " + sheet.SheetName );
				AppendLine("sheet num   | " + sheet.SheetNumber);


				foreach (Sheet.SheetIdPart part in sheet.SheetIdPartsSheetNumber)
				{
					string description =
						SheetSystemManager.SheetPartsManager.SheetPartList[part.SheetPartIndex]
						.PartDescription;

					string value = part.SheetidPartData;

					AppendLine("part name   | "
						+ description.PadRight(15)
						+ "   part info| " + value);
				}
			}
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