﻿#region + Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using DebugCode;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using ScanPDFBoxes.Process;
using ShItextCode;
using SheetEditor;
using SheetEditor.SheetData;
using SheetEditor.SheetData;
using SheetEditor.SheetEditor;
using SheetEditor.SheetEditor2;
using SheetEditor.Support;
using ShTempCode.DebugCode;
using UtilityLibrary;
using ShowWhere = ShTempCode.DebugCode.ShowWhere;

#endregion

// user name: jeffs
// created:   5/25/2024 7:05:05 AM

namespace SheetEditor.Support
{
	public class ShowSheetRectInfo
	{
		private static ShowWhere showWhere;

		private const int TITLE_WIDTH = 24;
		private const int TYPE_WIDTH = -20;
		private const int START_MSG_WIDTH = 30;

		private const int LABLE_WIDTH = 28;
		private const string TAB_S = "    ";

		private static readonly  string SEPARATOR = "*".Repeat(START_MSG_WIDTH);

		// reports - always to console

		public static void ShowRemoveReport(ProcessManager pm, int beginCount)
		{
			int finalCount = SheetDataManager.SheetsCount;
			Console.WriteLine($"Initial count {beginCount} | final count {finalCount} | removed {beginCount - finalCount}");

			showErrors(pm);
		}

		public static void showScanReport(ProcessManager pm)
		{
			bool result = true;
			string temp;

			Console.WriteLine($"\nscan sheet boxes report");

			foreach (KeyValuePair<string, SheetRects> kvp in SheetDataManager.Data.SheetRectangles)
			{
				Console.WriteLine($"\nfor | {kvp.Value.Name}");

				if (kvp.Value.AllShtRectsFound)
				{
					Console.WriteLine($"\tall boxes found");
				}
				else
				{
					Console.WriteLine($"\tthese boxes are missing");

					result = false;

					foreach (KeyValuePair<string, SheetRectConfigData<SheetRectId>> kvp2 in SheetRectConfigDataSupport.ShtRectIdXref)
					{
						if (kvp2.Value.Id == SheetRectId.SM_NA) continue;

						if (!kvp.Value.ShtRects.ContainsKey(kvp2.Value.Id))
						{
							string name = SheetRectConfigDataSupport.GetShtRectName(kvp2.Value.Id) ?? "no name";

							Console.WriteLine($"\t{name}");
						}
					}

					Console.Write("\n");
				}

				if (kvp.Value.OptRects.Count == 0)
				{
					Console.WriteLine($"\tno optional boxes found");
				}
				else
				{
					if (kvp.Value.OptRects.Count == 1)
					{
						Console.WriteLine($"\t{kvp.Value.OptRects.Count} optional box found");
					}
					else
					{
						Console.WriteLine($"\t{kvp.Value.OptRects.Count} optional boxes found");
					}
					
				}
			}

			showDuplicates(pm);

			showExtras(pm);

			showErrors(pm);
		}


		// listings - always to console

		private static void showDuplicates(ProcessManager pm)
		{
			Console.Write("\n");

			if (pm.duplicateRects.Count == 0)
			{
				Console.WriteLine($"no duplicate boxes found | {pm.duplicateRects.Count}");
				return;
			}

			Console.WriteLine($"duplicate boxes found | {pm.duplicateRects.Count}");

			foreach (Tuple<string, string, Rectangle> dups in pm.duplicateRects)
			{
				Console.WriteLine($"\tfile {dups.Item1,-20} | name {dups.Item2,-20} | location {dups.Item3.GetX():F2}, {dups.Item3.GetY():F2}");
			}

			// Console.WriteLine("\nplease eliminate the duplicate boxes and try again\n");
		}

		private static void showExtras(ProcessManager pm)
		{
			Console.Write("\n");

			if (pm.extraRects.Count == 0)
			{
				Console.WriteLine($"no extra boxes found | {pm.extraRects.Count}");
				return;
			}

			Console.WriteLine($"extra boxes found | {pm.extraRects.Count}");

			foreach (Tuple<string, string, Rectangle> xtra in pm.extraRects)
			{
				Console.WriteLine($"\tfile {xtra.Item1,-20} | name {xtra.Item2,-20} | location {xtra.Item3.GetX():F2}, {xtra.Item3.GetY():F2}");
			}

			// Console.WriteLine("\nplease eliminate the extra boxes and try again\n");
		}

		private static void showErrors(ProcessManager pm)
		{
			Console.Write("\n");

			if (pm.errors.Count == 0)
			{
				Console.WriteLine("All good - no error encountered");
				return;
			}

			if ( pm.errors.Count == 1)
			{
				Console.WriteLine("An error was encountered|");
			}
			else
			{
				Console.WriteLine("Some errors were encountered|");
			}

			foreach (Tuple<string, string, ErrorLevel> fail in pm.errors)
			{
				Console.WriteLine($"file| {fail.Item1} | error level {fail.Item3} | issue| {fail.Item2}");
			}
		}

		
		// special listings - to console or to debug

		public static void showStatus(ShowWhere where)
		{
			showWhere = where;

			

			string count = SheetDataManager.SheetsCount <= 0 ? "no sheets" : SheetDataManager.SheetsCount.ToString();

			showMsgLine($"Currently there are {count} saved");

			string s1 = SheetDataManager.Path?.RootFolderPath ?? "** null **";
			string s2 = SheetDataManager.Path?.FileName ?? "** null **";

			showMsgLine($"data file folder DM| {s1}");
			showMsgLine($"data file name   DM| {s2}");
			
			showMsg("\n");

			count = SheetDataManager2.SheetsCount <= 0 ? "no sheets" : SheetDataManager2.SheetsCount.ToString();

			showMsgLine($"Currently there are {count} saved");

			// string s3 = SheetDataManager2.DataPath?.FolderPath ?? "** null **";
			// string s4 =  SheetDataManager2.DataPath?.FileName ?? "** null **";

			s1 = SheetDataManager2.Path?.RootFolderPath ?? "** null **";
			s2 = SheetDataManager2.Path?.FileName ?? "** null **";

			string s5 = SheetDataManager2.Initialized.ToString();

			// showMsgLine($"data file folder FM| {s3}");
			// showMsgLine($"data file name   FM| {s4}");

			showMsgLine($"DM init          DM| {s5}");
			showMsgLine($"data file folder DM| {s1}");
			showMsgLine($"data file name   DM| {s2}");

			showMsg("\n");

			showMsgLine($"Got Sheet Folder   | {Program.sm2.GotSheetFolder}");
			showMsgLine($"Got Sheet File List| {Program.sm2.GotSheetFileList}");

			s1 = Program.sm2.SheetFileFolder ?? "** null **";

			showMsgLine($"Sheet File folder  | {s1}");

			s1 = SheetDataManager2.SheetsCount.ToString();
			showMsgLine($"DM sheet count     | {s1}");

		}


	#region show info for "program" - orig


		public static void StartMsg(string msg1,  ShowWhere where, string msg2=null)
		{
			showWhere = where;

			showMsg("\n\n");
			showMsgLine(SEPARATOR);
			showMsgLine($"*** {msg1,-1*(START_MSG_WIDTH-8)} ***");
			if (msg2!=null) showMsgLine($"*** {msg2,-1*(START_MSG_WIDTH-8)} ***");
			showMsgLine(SEPARATOR);
			showMsg("\n");
		}

		// sheet names from "program"
		public static void ShowSheetNames(ShowWhere where)
		{
			string found;

			showWhere = where;

			if (SheetDataManager.Data.SheetRectangles == null || SheetDataManager.Data.SheetRectangles.Count == 0)
			{
				showMsgLine("There are no sheets saved");
				return;
			}

			foreach (KeyValuePair<string, SheetRects> kvp in SheetDataManager.Data.SheetRectangles)
			{
				showMsg($"Sheet Name| {kvp.Key} | ");

				showMsg($"Rectangles found {kvp.Value.ShtRects.Count,3} | ");

				if (kvp.Value.AllShtRectsFound)
				{
					found = "Yep";
				}
				else
				{
					found = "Nope";
				}

				showMsgLine($"All Rects Found? | {found}");
			}
		}

		// basic rects from "program"
		public static void showShtRects(ShowWhere where)
		{
			showWhere = where;

			if (SheetDataManager.Data.SheetRectangles == null || SheetDataManager.Data.SheetRectangles.Count == 0)
			{
				return;
			}

			foreach (KeyValuePair<string, SheetRects> kvp in SheetDataManager.Data.SheetRectangles)
			{
				int missing;

				showMsgLine($"\n\nfor {kvp.Key}");

				showMsg($"{"sheet rectangles",TITLE_WIDTH}| found {kvp.Value.ShtRects.Count}");

				missing = SheetRectConfigDataSupport.ShtRectsQty - kvp.Value.ShtRects.Count;

				if (missing > 0)
				{
					showMsgLine($" | missing {missing}");
				}
				else
				{
					showMsg("\n");
				}

				showMsgLine($"{"optional rectangles",TITLE_WIDTH}| found {kvp.Value.OptRects.Count}");

				foreach (KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp2 in kvp.Value.ShtRects)
				{
					showMsgLine(formatSingleRect(kvp2));
				}

				showMsg("\n");
				
				foreach (KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp2 in kvp.Value.OptRects)
				{
					showMsgLine(formatSingleRect(kvp2));
				}

				showMsg("\n");
			}
		}

		public static string formatSingleRect(KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp2, Rectangle pageSize = null)
		{
			string name = SheetRectConfigDataSupport.GetShtRectName(kvp2.Key)!;

			if (name.IsVoid())
			{
				name = SheetRectConfigDataSupport.GetOptRectName(kvp2.Key);
			}

			return formatSingleRect2(name, kvp2.Value.TextBoxRotation, kvp2.Value.Type, kvp2.Value.Rect, pageSize);


			// string rotation = $"{kvp2.Value.TextBoxRotation,8:F2}";
			//
			// string type = kvp2.Value.Type.ToString();
			//
			// Rectangle r = kvp2.Value.Rect;
			//
			// float oaWidth = r.GetX()+r.GetWidth();
			// float oaHeight = r.GetY()+r.GetHeight();
			//
			// string oa = null;
			//
			// if (pageSize != null)
			// {
			// 	float wDiff = pageSize.GetWidth() - oaWidth;
			// 	float hDiff = pageSize.GetHeight() - oaHeight;
			//
			// 	oa = $" | oaW| {oaWidth,8:F2}  wDif {$"({wDiff:F2})",10} | oaH {oaHeight,8:F2}  hDif {$"({hDiff:F2})",10}";
			// }
			//
			// return $"{name,TITLE_WIDTH}| {type,TYPE_WIDTH}| {FormatItextData.FormatRectangle(kvp2.Value.Rect)} ({rotation}°)  {oa}";
		}


		// rect values from "program"
		public static void ShowValues(ShowWhere where)
		{
			showWhere = where;

			foreach (KeyValuePair<string, SheetRects> kvp in SheetDataManager.Data.SheetRectangles)
			{
				showMsgLine($"sheet name| {kvp.Key}");

				foreach (KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp2 in kvp.Value.ShtRects)
				{
					showMsgLine($"\n{kvp2.Key}");

					showBoxValues(kvp2.Value);
				}

				foreach (KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp3 in kvp.Value.OptRects)
				{
					showMsgLine($"\n {kvp3.Key}");

					showBoxValues(kvp3.Value);
				}
			}
		}

		private static void showBoxValues(SheetRectData<SheetRectId> box)
		{
			showMsgLine($"{$"{TAB_S}box data",-LABLE_WIDTH}| ");
			showMsgLine($"{$"{TAB_S}{TAB_S}box id",-LABLE_WIDTH}| {box.Id}");
			showMsgLine($"{$"{TAB_S}{TAB_S}box type",-LABLE_WIDTH}| {box.Type}");
			showMsgLine($"{$"{TAB_S}{TAB_S}rectangle",-LABLE_WIDTH}| {FormatItextData.FormatRectangle(box.Rect, false)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}rotation",-LABLE_WIDTH}| {box.TextBoxRotation:F2}");

			if (box.Id == SheetRectId.SM_XREF)
			{
				showBoundingBoxValues(box);
				return;
			}

			if (box.Type == SheetRectType.SRT_NA ||
				box.Type == SheetRectType.SRT_LOCATION
				) return;

			showMsg($"{$"{TAB_S}bounding box info",-LABLE_WIDTH}| ");

			if (box.HasType(SheetRectType.SRT_BOX))
			{
				showMsg("\n");
				showBoundingBoxValues(box);
			}
			else
			{
				showMsgLine("n/a");
			}

			showMsg($"{$"{TAB_S}link info",-LABLE_WIDTH}| ");

			if (box.HasType(SheetRectType.SRT_LINK))
			{
				showMsg("\n");
				showMsgLine($"{$"{TAB_S}{TAB_S}UrlLink",-LABLE_WIDTH}| {box.UrlLink}");

			}
			else
			{
				showMsgLine("n/a");
			}

			showMsg($"{$"{TAB_S}text info",-LABLE_WIDTH}| ");

			if (box.HasType(SheetRectType.SRT_TEXT))
			{
				showMsg("\n");
				showTextValues(box);
			}
			else
			{
				showMsgLine("n/a");
			}
		}

		private static void showBoundingBoxValues(SheetRectData<SheetRectId> box)
		{
			showMsgLine($"{$"{TAB_S}{TAB_S}FillColor",-LABLE_WIDTH}| {FormatItextData.FormatColor(box.FillColor, false)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}FillOpacity",-LABLE_WIDTH}| {box.FillOpacity}");
			showMsgLine($"{$"{TAB_S}{TAB_S}BdrWidth",-LABLE_WIDTH}| {box.BdrWidth}");
			showMsgLine($"{$"{TAB_S}{TAB_S}BdrColor",-LABLE_WIDTH}| {FormatItextData.FormatColor(box.BdrColor, false)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}BdrOpacity",-LABLE_WIDTH}| {box.BdrOpacity}");
			showMsgLine($"{$"{TAB_S}{TAB_S}BdrDashPattern",-LABLE_WIDTH}| {FormatItextData.FormatDashArray(box.BdrDashPattern)}");

		}

		private static void showTextValues(SheetRectData<SheetRectId> box)
		{
			showMsgLine($"{$"{TAB_S}{TAB_S}InfoText",-LABLE_WIDTH}| {box.InfoText }");
			showMsgLine($"{$"{TAB_S}{TAB_S}FontFamily",-LABLE_WIDTH}| {box.FontFamily }");
			showMsgLine($"{$"{TAB_S}{TAB_S}FontStyle",-LABLE_WIDTH}| {FormatItextData.FormatFontStyle(box.FontStyle)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextSize",-LABLE_WIDTH}| {box.TextSize }");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextHorizAlignment",-LABLE_WIDTH}| {box.TextHorizAlignment }");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextVertAlignment",-LABLE_WIDTH}| {box.TextVertAlignment }");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextWeight",-LABLE_WIDTH}| {box.TextWeight}");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextDecoration",-LABLE_WIDTH}| {TextDecorations.FormatTextDeco(box.TextDecoration)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextColor",-LABLE_WIDTH}| {FormatItextData.FormatColor(box.TextColor, false)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextOpacity",-LABLE_WIDTH}| {box.TextOpacity }");

		}

	#endregion

		private static string formatSingleRect2(string name, float rotn, SheetRectType t, Rectangle r, Rectangle pageSize = null)
		{
			string rotation = $"{rotn,8:F2}";

			string type = t.ToString();

			float oaWidth = r.GetX()+r.GetWidth();
			float oaHeight = r.GetY()+r.GetHeight();

			string oa = null;

			if (pageSize != null)
			{
				float wDiff = pageSize.GetWidth() - oaWidth;
				float hDiff = pageSize.GetHeight() - oaHeight;

				oa = $" | oaW| {oaWidth,8:F2}  wDif {$"({wDiff:F2})",10} | oaH {oaHeight,8:F2}  hDif {$"({hDiff:F2})",10}";
			}

			return $"{name,TITLE_WIDTH}| {type,TYPE_WIDTH}| {FormatItextData.FormatRectangle(r)} ({rotation}°)  {oa}";
		}

		private static void showBoxValues2(SheetRectData<SheetRectId> box)
		{
			showMsgLine($"{$"{TAB_S}box data",-LABLE_WIDTH}| ");
			showMsgLine($"{$"{TAB_S}{TAB_S}box id",-LABLE_WIDTH}| {box.Id}");
			showMsgLine($"{$"{TAB_S}{TAB_S}box type",-LABLE_WIDTH}| {box.Type}");
			showMsgLine($"{$"{TAB_S}{TAB_S}rectangle",-LABLE_WIDTH}| {FormatItextData.FormatRectangle(box.BoxSettings.Rect, false)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}rotation",-LABLE_WIDTH}| {box.BoxSettings.TextBoxRotation:F2}");

			if (box.Id == SheetRectId.SM_XREF)
			{
				showBoundingBoxValues2(box.BoxSettings);
				return;
			}

			if (box.Type == SheetRectType.SRT_NA ||
				box.Type == SheetRectType.SRT_LOCATION
				) return;

			showMsg($"{$"{TAB_S}bounding box info",-LABLE_WIDTH}| ");

			if (box.HasType(SheetRectType.SRT_BOX))
			{
				showMsg("\n");
				showBoundingBoxValues2(box.BoxSettings);
			}
			else
			{
				showMsgLine("n/a");
			}

			showMsg($"{$"{TAB_S}link info",-LABLE_WIDTH}| ");

			if (box.HasType(SheetRectType.SRT_LINK))
			{
				showMsg("\n");
				showMsgLine($"{$"{TAB_S}{TAB_S}UrlLink",-LABLE_WIDTH}| {box.TextSettings.UrlLink}");

			}
			else
			{
				showMsgLine("n/a");
			}

			showMsg($"{$"{TAB_S}text info",-LABLE_WIDTH}| ");

			if (box.HasType(SheetRectType.SRT_TEXT))
			{
				showMsg("\n");
				showTextValues2(box.TextSettings);
			}
			else
			{
				showMsgLine("n/a");
			}
		}

		private static void showBoundingBoxValues2(BoxSettings bxs)
		{
			showMsgLine($"{$"{TAB_S}{TAB_S}FillColor",-LABLE_WIDTH}| {FormatItextData.FormatColor(bxs.FillColor, false)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}FillOpacity",-LABLE_WIDTH}| {bxs.FillOpacity}");
			showMsgLine($"{$"{TAB_S}{TAB_S}BdrWidth",-LABLE_WIDTH}| {bxs.BdrWidth}");
			showMsgLine($"{$"{TAB_S}{TAB_S}BdrColor",-LABLE_WIDTH}| {FormatItextData.FormatColor(bxs.BdrColor, false)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}BdrOpacity",-LABLE_WIDTH}| {bxs.BdrOpacity}");
			showMsgLine($"{$"{TAB_S}{TAB_S}BdrDashPattern",-LABLE_WIDTH}| {FormatItextData.FormatDashArray(bxs.BdrDashPattern)}");

		}

		private static void showTextValues2(TextSettings txs)
		{
			showMsgLine($"{$"{TAB_S}{TAB_S}InfoText",-LABLE_WIDTH}| {txs.InfoText }");
			showMsgLine($"{$"{TAB_S}{TAB_S}FontFamily",-LABLE_WIDTH}| {txs.FontFamily }");
			showMsgLine($"{$"{TAB_S}{TAB_S}FontStyle",-LABLE_WIDTH}| {FormatItextData.FormatFontStyle(txs.FontStyle)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextSize",-LABLE_WIDTH}| {txs.TextSize }");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextHorizAlignment",-LABLE_WIDTH}| {txs.TextHorizAlignment }");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextVertAlignment",-LABLE_WIDTH}| {txs.TextVertAlignment }");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextWeight",-LABLE_WIDTH}| {txs.TextWeight}");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextDecoration",-LABLE_WIDTH}| {TextDecorations.FormatTextDeco(txs.TextDecoration)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextColor",-LABLE_WIDTH}| {FormatItextData.FormatColor(txs.TextColor, false)}");
			showMsgLine($"{$"{TAB_S}{TAB_S}TextOpacity",-LABLE_WIDTH}| {txs.TextOpacity }");

		}

	#region show info for "program" - new

		// sheet names from "program"
		public static void ShowSheetNames2(ShowWhere where)
		{
			string found;

			showWhere = where;

			if (SheetDataManager2.Data.SheetDataList == null || SheetDataManager2.Data.SheetDataList.Count == 0)
			{
				showMsgLine("There are no sheets saved");
				return;
			}

			foreach (KeyValuePair<string, SheetEditor.SheetData.SheetData> kvp in SheetDataManager2.Data.SheetDataList)
			{
				showMsg($"Sheet Name| {kvp.Key} | ");

				showMsg($"Rectangles found {kvp.Value.ShtRects.Count,3} | ");

				if (kvp.Value.AllShtRectsFound)
				{
					found = "Yep";
				}
				else
				{
					found = "Nope";
				}

				showMsgLine($"All Rects Found? | {found}");
			}
		}

		// basic rects from "program"
		public static void showShtRects2(ShowWhere where)
		{
			showWhere = where;

			if (SheetDataManager2.Data.SheetDataList == null || SheetDataManager2.Data.SheetDataList.Count == 0)
			{
				return;
			}

			foreach (KeyValuePair<string, SheetEditor.SheetData.SheetData> kvp in SheetDataManager2.Data.SheetDataList)
			{
				int missing;

				showMsgLine($"\n\nfor {kvp.Key}");

				showMsg($"{"sheet rectangles",TITLE_WIDTH}| found {kvp.Value.ShtRects.Count}");

				missing = SheetRectConfigDataSupport.ShtRectsQty - kvp.Value.ShtRects.Count;

				if (missing > 0)
				{
					showMsgLine($" | missing {missing}");
				}
				else
				{
					showMsg("\n");
				}

				showMsgLine($"{"optional rectangles",TITLE_WIDTH}| found {kvp.Value.OptRects.Count}");

				foreach (KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp2 in kvp.Value.ShtRects)
				{
					showMsgLine(formatSingleRect2(kvp2));
				}

				showMsg("\n");
				
				foreach (KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp2 in kvp.Value.OptRects)
				{
					showMsgLine(formatSingleRect2(kvp2));
				}

				showMsg("\n");
			}
		}

		public static string formatSingleRect2(KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp2, Rectangle pageSize = null)
		{
			string name = SheetRectConfigDataSupport.GetShtRectName(kvp2.Key)!;

			if (name.IsVoid())
			{
				name = SheetRectConfigDataSupport.GetOptRectName(kvp2.Key);
			}

			return formatSingleRect2(name, kvp2.Value.BoxSettings.TextBoxRotation, kvp2.Value.Type, kvp2.Value.BoxSettings.Rect, pageSize);
		}

		// rect values from "program"
		public static void ShowValues2(ShowWhere where)
		{
			showWhere = where;

			foreach (KeyValuePair<string, SheetEditor.SheetData.SheetData> kvp in SheetDataManager2.Data.SheetDataList)
			{
				showMsgLine($"sheet name| {kvp.Key}");

				foreach (KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp2 in kvp.Value.ShtRects)
				{
					showMsgLine($"\n{kvp2.Key}");

					showBoxValues2(kvp2.Value);
				}

				foreach (KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp3 in kvp.Value.OptRects)
				{
					showMsgLine($"\n {kvp3.Key}");

					showBoxValues2(kvp3.Value);
				}
			}
		}

	#endregion

		// always to debug

		// utility routines

		public static void showMsgLine(string msg)
		{

			showMsg(msg+"\n");
		}

		public static void showMsg(string msg)
		{
			if (showWhere == ShowWhere.DEBUG || showWhere == ShowWhere.DBG_CONS)
			{
				Debug.Write(msg);
			}
			
			if (showWhere == ShowWhere.CONSOLE || showWhere == ShowWhere.DBG_CONS)
			{
				Console.Write(msg);
			}
		}

	}
}