#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AndyShared.FileSupport.FileNameSheetPDF;
using UtilityLibrary;

using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;

#endregion


// projname: $projectname$
// itemname: Identifiers
// username: jeffs
// created:  11/29/2024 5:17:51 PM

namespace SuiteInfoEditor.Support
{
	public class Identifiers
	{
	#region private fields

		private const int SUBJECT_WIDTH = -20;
		private const int VALUE_WIDTH_1 = -20;
		private const int VALUE_WIDTH_2 = -16;
		private const int VALUE_WIDTH_3 = -10;

		private const string SPACER = CsFlowDocManager.HORIZ_LINE;
		private const string DIVIDER = CsFlowDocManager.VERT_LINE;
		private const string INTERSECTION = $"{SPACER}{CsFlowDocManager.INTERRSECTION}{SPACER}";

		// private IFdFmt w;


		private static readonly Lazy<Identifiers> instance =
			new Lazy<Identifiers>(() => new Identifiers());

		private CsFlowDocManager w = CsFlowDocManager.Instance;

		#endregion

		#region ctor

		private Identifiers() { }

		// public void Init(IFdFmt w)
		// {
		// 	this.w = w;
		// }

	#endregion

	#region public properties

		public static Identifiers Instance => instance.Value;

	#endregion

	#region private properties

	#endregion


	#region public methods

		private int[] cols = [ 17, 15, 15];
		private string[] preFmt = ["<textformat background='40, 40, 40'/>", null];
		string[][] fldFmt = [ ["<lawngreen>", "</lawngreen>"],["<cyan>", "</cyan>"], ["<slategray>", "</slategray>"] ];
		private string hdrFmt = "<foreground color='lightgray'/>";
		private string baseFmt = "<foreground color='gray'/>";
		
		public void showShtNumComponentConstants1()
		{
			// CsFlowDocUtilities cu = new CsFlowDocUtilities();
			// cu.reportRowTest();
			//
			// return;

			w.ClearAltRowFormat();

			w.StartTb(hdrFmt);

			w.AddTextLineTb("Showing sheet number component names (in ShtNumComps2, CompNameInfo)");
			w.AddTextLineTb($"file| {typeof(FileNameSheetIdentifiers).FullName}\n" );

			w.StartTb(baseFmt);

			w.AddDescTextLineTb(w.ReportRow(cols, ["Const", "Value"]));
			w.AddDescTextLineTb(w.ReportRow(cols, [2], null, null, null, null, true, [SPACER], [INTERSECTION]));

			w.AssignAltRowFormat(preFmt);

			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_PHBLD ), N_PHBLD ], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_DISCP ), N_DISCP ], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_CAT   ), N_CAT   ], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SUBCAT), N_SUBCAT], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_MOD   ), N_MOD   ], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SUBMOD), N_SUBMOD], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_ID    ), N_ID    ], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SUBID ), N_SUBID ], fldFmt));

			w.AddLineBreaksTb(1);

			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_PBSEP), N_PBSEP], fldFmt));
			
			w.AddLineBreaksTb(1);

			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SEP0), N_SEP0], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SEP1), N_SEP1], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SEP2), N_SEP2], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SEP3), N_SEP3], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SEP4), N_SEP4], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SEP5), N_SEP5], fldFmt));
			
			w.AddLineBreaksTb(1);
			
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SHTNUM  ), N_SHTNUM  ], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SHTID   ), N_SHTID   ], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(N_SHTTITLE), N_SHTTITLE], fldFmt));

		}

		public void showShtNumComponentConstants2()
		{
			w.ClearAltRowFormat();

			w.StartTb(hdrFmt);

			w.AddTextLineTb("Showing sheet number array const's (array: SheetNumComponentData)");
			w.AddTextLineTb($"file| {typeof(FileNameSheetIdentifiers).FullName}\n" );

			w.StartTb(baseFmt);

			w.AddDescTextLineTb(w.ReportRow(cols, ["Const", "Value"]));
			w.AddDescTextLineTb(w.ReportRow(cols, ["", ""], null, null, null, null, true, [SPACER], [INTERSECTION]));

			w.AssignAltRowFormat(preFmt);

			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_PHBLDG       ), VI_PHBLDG       .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_PBSEP        ), VI_PBSEP        .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_DISCIPLINE   ), VI_DISCIPLINE   .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SEP0         ), VI_SEP0         .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_CATEGORY     ), VI_CATEGORY     .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SEP1         ), VI_SEP1         .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SUBCATEGORY  ), VI_SUBCATEGORY  .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SEP2         ), VI_SEP2         .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_MODIFIER     ), VI_MODIFIER     .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SEP3         ), VI_SEP3         .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SUBMODIFIER  ), VI_SUBMODIFIER  .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SEP4         ), VI_SEP4         .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_IDENTIFIER   ), VI_IDENTIFIER   .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SEP5         ), VI_SEP5         .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SUBIDENTIFIER), VI_SUBIDENTIFIER.ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SORT_SUFFIX  ), VI_SORT_SUFFIX  .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SHTNUM       ), VI_SHTNUM       .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SHTID        ), VI_SHTID        .ToString()], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_SHTTITLE     ), VI_SHTTITLE     .ToString()], fldFmt));

			w.AddLineBreaksTb(1);

			cols[2] = 70;

			w.ClearAltRowFormat();

			w.AddDescTextLineTb(w.ReportRow(cols, ["Const", "Value", "Description"]));
			w.AddDescTextLineTb(w.ReportRow(cols, [3], null, null, null, null, true, [SPACER], [INTERSECTION]));

			w.AssignAltRowFormat(preFmt);

			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_COMP_COUNT),  VI_COMP_COUNT, "(number of sheet components [up to and incl. VI_SUBIDENTIFIER] )"], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_COUNT)     ,  VI_COUNT     , "(maximum component value in this list + 1 [equal to VI_SHTTITLE + 1] ))"], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(cols, [nameof(VI_MIN)       ,  VI_MIN       , "(minimum component value in this list [equal to VI_PHBLDG] )"], fldFmt));

		}

		public void ShowCompNameInfo1()
		{
			int[] pos = [ cols[0], cols[1], 20, 20, 20, 20 ];
			string[][] fldFmt = [ ["<lawngreen>", "</lawngreen>"],["<cyan>", "</cyan>"], ["<magenta>", "</magenta>"], ["<magenta>", "</magenta>"], ["<white>", "</white>"] ];

			w.ClearAltRowFormat();

			w.StartTb(hdrFmt);

			w.AddTextLineTb("Showing component name info (individual data elements)(CompNameInfo)");
			w.AddTextLineTb($"file| {typeof(FileNameSheetIdentifiers).FullName}\n" );


			w.StartTb(baseFmt);
			w.AddDescTextLineTb(w.ReportRow(pos, ["Const", "Index", "Value", "ShtIdType", "Value"]));
			w.AddDescTextLineTb(w.ReportRow(pos, [5], null, null, null, null, true, [SPACER], [INTERSECTION]));

			w.AssignAltRowFormat(preFmt);

			//                                    col 0                      col 1                    col 2              col 3                    col 4
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SHTID   ),  $" {CNI_SHTID   .Index, 4}", CNI_SHTID   .Name, CNI_SHTID   .Type, (int) CNI_SHTID   .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SHTTITLE),  $" {CNI_SHTTITLE.Index, 4}", CNI_SHTTITLE.Name, CNI_SHTTITLE.Type, (int) CNI_SHTTITLE.Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_PHBLD   ),  $" {CNI_PHBLD   .Index, 4}", CNI_PHBLD   .Name, CNI_PHBLD   .Type, (int) CNI_PHBLD   .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_DISCP   ),  $" {CNI_DISCP   .Index, 4}", CNI_DISCP   .Name, CNI_DISCP   .Type, (int) CNI_DISCP   .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_CAT     ),  $" {CNI_CAT     .Index, 4}", CNI_CAT     .Name, CNI_CAT     .Type, (int) CNI_CAT     .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SUBCAT  ),  $" {CNI_SUBCAT  .Index, 4}", CNI_SUBCAT  .Name, CNI_SUBCAT  .Type, (int) CNI_SUBCAT  .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_MOD     ),  $" {CNI_MOD     .Index, 4}", CNI_MOD     .Name, CNI_MOD     .Type, (int) CNI_MOD     .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SUBMOD  ),  $" {CNI_SUBMOD  .Index, 4}", CNI_SUBMOD  .Name, CNI_SUBMOD  .Type, (int) CNI_SUBMOD  .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_ID      ),  $" {CNI_ID      .Index, 4}", CNI_ID      .Name, CNI_ID      .Type, (int) CNI_ID      .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SUBID   ),  $" {CNI_SUBID   .Index, 4}", CNI_SUBID   .Name, CNI_SUBID   .Type, (int) CNI_SUBID   .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SEP0    ),  $" {CNI_SEP0    .Index, 4}", CNI_SEP0    .Name, CNI_SEP0    .Type, (int) CNI_SEP0    .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SEP1    ),  $" {CNI_SEP1    .Index, 4}", CNI_SEP1    .Name, CNI_SEP1    .Type, (int) CNI_SEP1    .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SEP2    ),  $" {CNI_SEP2    .Index, 4}", CNI_SEP2    .Name, CNI_SEP2    .Type, (int) CNI_SEP2    .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SEP3    ),  $" {CNI_SEP3    .Index, 4}", CNI_SEP3    .Name, CNI_SEP3    .Type, (int) CNI_SEP3    .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SEP4    ),  $" {CNI_SEP4    .Index, 4}", CNI_SEP4    .Name, CNI_SEP4    .Type, (int) CNI_SEP4    .Type], fldFmt));
			w.AddDescTextLineTb(w.ReportRow(pos, [nameof(CNI_SEP5    ),  $" {CNI_SEP5    .Index, 4}", CNI_SEP5    .Name, CNI_SEP5    .Type, (int) CNI_SEP5    .Type], fldFmt));

			w.AddLineBreaksTb(1);

			w.ClearAltRowFormat();

			w.StartTb(hdrFmt);
			w.AddTextLineTb("Showing same info in the Dictionary: 'CompNames'");

			w.AddLineBreaksTb(1);

			int idx = 0;

			w.StartTb(baseFmt);
			w.AddDescTextLineTb(w.ReportRow(pos, ["Idx", "Key", "Indx", "Name", "Type"]));
			w.AddDescTextLineTb(w.ReportRow(pos, [2], null, null, null, null, true, [SPACER], [INTERSECTION]));

			w.AssignAltRowFormat(preFmt);

			foreach (KeyValuePair<int, CompNameInfo> cni in CompNames)
			{
				w.AddDescTextLineTb(w.ReportRow(pos, [$"{idx++,-3}", $"{cni.Key, 4}", $"{cni.Value.Index, 4}", cni.Value.Name, cni.Value.Type], fldFmt));
			}

		}

		public void ShowSheetNumComponentData()
		{
			const int COL5 = 22;
			const int COL6 = 18;
			const int COL8 = 14;
			const int COL9 = 10;
			//             0  1  2  3   4   5     6      7  8
			int[] pos1 = [ 7, 7, 7, 18, 12, COL5, COL6];
			int[] pos2 = [ 7, 7, 7, 18, 12, COL5, COL6, 20, COL8, COL9, COL9];
			int[] pos = pos1;

			string[][] fmt = [ 
				["<lawngreen>", "</lawngreen>"],
				["<cyan>", "</cyan>"], 
				["<magenta>", "</magenta>"], 
				["<magenta>", "</magenta>"], 
				["<magenta>", "</magenta>"], 
				["<magenta>", "</magenta>"], 
				["<magenta>", "</magenta>"], 
				["<white>", "</white>"],
				["<dimgray>", "</dimgray>"],
				["<red>", "</red>"],
				["<dimgray>", "</dimgray>"],
			];

			List<object> vals;

			object[] values = null;

			int idx = 0;

			w.ClearAltRowFormat();

			w.StartTb(hdrFmt);

			w.AddTextLineTb("Showing sheet number component data (in classifier editor)");
			w.AddTextLineTb($"file| {typeof(FileNameSheetIdentifiers).FullName}\n" );

			w.StartTb(baseFmt);
			// col                                 0      1      2        3               4        5                6                   7                 8                  9                  10
			w.AddDescTextLineTb(w.ReportRow(pos2, [""   , ""   , "◂---", "◂---", "Value ---▸", $"{"---▸", COL5}", $"{"---▸", COL6}", ""]));
			w.AddDescTextLineTb(w.ReportRow(pos2, [""   , ""   , "",      "",            "Abbv",   "Item",          "Group" ,           "Neumonic ---▸", $"{"---▸", COL8}", $"{"---▸", COL9}", $"{"---▸|", COL9}"]));
			// col                                 0      1      2        3               4         5               6                   7                 8                  9                  10
			w.AddDescTextLineTb(w.ReportRow(pos2, ["Idx", "Key", "Index", "Name",        "Name",    "Class",        "Id",               "Value",          "Preface",         "Body",            "Suffix"]));
			w.AddDescTextLineTb(w.ReportRow(pos2, [11], null, null, null, null, true, [SPACER], [INTERSECTION]));

			w.AssignAltRowFormat(preFmt);



			foreach (KeyValuePair<int, ShtNumComps2> snc in SheetNumComponentData)
			{
				pos = pos1;
				vals = [idx++, snc.Key, snc.Value.Index, snc.Value.Name, snc.Value.AbbrevName, snc.Value.ItemClass, snc.Value.GroupId];

				if (!snc.Value.Neumonic.ToString().IsVoid())
				{
					pos = pos2;
					vals.Add(snc.Value.Neumonic);
					vals.Add(snc.Value.Neumonic.Preface);
					vals.Add(snc.Value.Neumonic.Body);
					vals.Add(snc.Value.Neumonic.Suffix);
				}

				w.AddDescTextLineTb(w.ReportRow(pos, vals.ToArray(), fmt));
			}
		}


	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(Identifiers)}";
		}

	#endregion
	}
}