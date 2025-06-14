using System;
using System.Collections.Generic;
using System.Text;
using AndyScan.Samples;
using AndyScan.SbSystem;
using AndyScan.Support;
using UtilityLibrary;
using static AndyScan.SbLocal.SbMainMnuItemExt;
using static AndyScan.SbSystem.SbmnuItems;

// username: jeffs
// created:  12/24/2024 8:16:50 AM

namespace AndyScan.SbLocal
{
	public class SbMainMnuItemExt
	{
		public static SbMnuItemId MMI_INIT_1 = new SbMnuItemId(101);
		public static SbMnuItemId MMI_INIT_2 = new SbMnuItemId(102);

		public static SbMnuItemId MMI_RESET_0 = new SbMnuItemId(200);
		public static SbMnuItemId MMI_RESET_2 = new SbMnuItemId(202);
		public static SbMnuItemId MMI_RESET_4 = new SbMnuItemId(204);
		public static SbMnuItemId MMI_RESET_6 = new SbMnuItemId(205);
		public static SbMnuItemId MMI_RESET_8 = new SbMnuItemId(208);

		public static SbMnuItemId MMI_ADD_2   = new SbMnuItemId(302);
		public static SbMnuItemId MMI_ADD_M   = new SbMnuItemId(321);

		public static SbMnuItemId MMI_DATA_SEL1   = new SbMnuItemId(401);
		public static SbMnuItemId MMI_DATA_READ1  = new SbMnuItemId(421);

		public static SbMnuItemId MMI_SHOW_RPT1  = new SbMnuItemId(501);

		public static SbMnuItemId MMI_OP_SCAN1    = new SbMnuItemId(801);

		public static SbMnuItemId MMI_OP_2_SCDATA    = new SbMnuItemId(1000);
	}

	public class SbMain : IMenu2
	{
		private const int MENU_WIDTH = 30;
		private const int MENU_OPTION_WIDTH = 6;

	#region private fields

		private SbHandler sbh;


		private WinInput wIput;
		private string keyUp;
		private int processKeyIdx;

		private IFdFmt iw;
		private IWinMain iwm;

		private int menuIdx;

	#endregion

	#region ctor

		public SbMain(IFdFmt iw, IWinMain iwm)
		{
			DM.Start0();
			this.iw = iw;
			this.iwm = iwm;

			configMenu();

			sbh = new SbHandler(iw, this);

			DM.End0();
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		//                                  +- menu idx
		//                                  v   +- selected option
		//                                      v       +- option title
		//                                              v
		public int ProcessMenuChoice(Tuple<int, string, string> menuKey)
		{
			DM.Start0();

			int nextMnu = (GetNextMenu(menuKey.Item1, menuKey.Item2));

			if (nextMnu == -100)
			{
				DM.End0("end 1");
				return iwm.ProcessMsg(-100);
			}

			bool? result = processMenuChoice(menuKey.Item1, menuKey.Item2);

			DM.End0();

			return 0;
		}

		public bool? ValidateOption(int menuIdx, string choice, out int subMenuIdx)
		{
			DM.Start0();

			int len = choice.Length;

			subMenuIdx = -1;

			foreach (KeyValuePair<string, Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int> >>
						kvp in Menus
						[menuIdx])
			{
				if (kvp.Key.Length < len) continue;

				if (kvp.Key.Substring(0, len).Equals(choice))
				{
					if (kvp.Key.Length == len)
					{
						subMenuIdx = kvp.Value.Item6[0];

						DM.End0("end 1");
						return true;
					}

					DM.End0("end 2");
					return null;
				}
			}

			DM.End0();

			return false;
		}

		public int GetNextMenu(int menuIdx, string menuKey)
		{
			// iwm.ProcessMsg();
			return Menus[menuIdx][menuKey].Item6[0];
		}

		public int GetTitleLength(int menuIdx, string menuKey)
		{
			return Menus[menuIdx][menuKey].Item5;
		}

		public string getTitlePlain(int menuIdx, string key)
		{
			string answer = "";

			foreach (string s in Menus[menuIdx][key].Item3)
			{
				answer += s;
			}

			return answer;
		}

		public string getKeyFormatted(int menuIdx, string menuKey)
		{
			List<string> codes = Menus[menuIdx][menuKey].Item2;

			if (codes == null || codes.Count != 2) return menuKey;

			return $"{codes[0]}{menuKey}{codes[1]}";
		}

		public string getTitleFormatted(int menuIdx, string menuKey)
		{
			int i;

			StringBuilder sb = new StringBuilder();
			List<string> titles = Menus[menuIdx][menuKey].Item3;
			List<string> codes = Menus[menuIdx][menuKey].Item4;

			if (codes == null || titles.Count + 1 != codes.Count) return titles[0];

			for (i = 0; i < titles.Count; i++)
			{
				sb.Append(codes[i]).Append(titles[i]);
			}

			sb.Append(codes[i]);

			return sb.ToString();
		}

		public string getHeaderFormatted(int menuIdx, string menuKey)
		{
			if (menuIdx == 0)
			{
				return getHeaderFormattedDef(menuIdx, menuKey);
			}
			else
			{
				return getHeaderFormattedDef(menuIdx, menuKey);
			}
		}

		public string getBlankLineFormatted(int menuIdx, string menuKey)
		{
			if (menuIdx == 0)
			{
				return getBlankLineFormattedDef(menuIdx, menuKey);
			}
			else
			{
				return getBlankLineFormattedDef(menuIdx, menuKey);
			}
		}

		public string getMenuItemFormatted(int menuIdx, string menuKey)
		{
			if (menuIdx == 0)
			{
				return getMenuItemFormattedDef(menuIdx, menuKey);
			}
			else
			{
				return getMenuItemFormattedDef(menuIdx, menuKey);
			}
		}

	#endregion

	#region private methods / menu processing

		// private void processMenu()
		// {
		// 	Menus = new  Dictionary<string, Tuple<SbMainMnuItem, List<string>, List<string>, List<string>, int, List<int>>>[Menus2.Length];
		//
		// 	Tuple<List<string>, List<string>, List<string>, int, List<int> > t;
		// 	int count;
		//
		// 	for (int i = 0; i < Menus2.Length; i++)
		// 	{
		// 		Menus[i] = new Dictionary<string, Tuple<SbMainMnuItem, List<string>, List<string>, List<string>, int, List<int>>>();
		//
		// 		foreach (KeyValuePair<string, Tuple<List<string>,List<string>, List<string>, int, List<int> >> kvp in Menus2[i])
		// 		{
		// 			t = kvp.Value;
		// 			count = 0;
		//
		// 			foreach (string s in t.Item2)
		// 			{
		// 				count += s?.Length ?? 0;
		// 			}
		//
		// 			Menus[i].Add(kvp.Key, new Tuple<List<string>, List<string>, List<string>, int, List<int> >(t.Item1, t.Item2, t.Item3, count, t.Item5));
		// 		}
		// 	}
		// }

	#endregion

	#region private methods

		// used for menu[0] and is the default
		private string getHeaderFormattedDef(int menuIdx, string menuKey)
		{
			StringBuilder sb = new StringBuilder();

			Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int> > mi = Menus[menuIdx][menuKey];

			string prefixA = $"╭{"─".Repeat(MENU_OPTION_WIDTH - 1)}";
			string prefixB = "• ";
			string prefixC = " •";
			string title;

			// int sfxWidth = MENU_WIDTH - mi.Item4 - prefixB.Length - prefixC.Length;
			int sfxWidth = rightMenuWidth - mi.Item5 - prefixC.Length;

			string suffix = "─".Repeat(sfxWidth) + "╮";

			title = formatTitle(mi);

			return $"<red>{prefixA}</red>{prefixB}{title}{prefixC}<red>{suffix}</red>";
		}

		// used for menu[0] and is the default
		public string getBlankLineFormattedDef(int menuIdx, string menuKey)
		{
			string result = "";

			SbMnuItemId which = Menus[menuIdx][menuKey].Item1;

			if (which.Value == SBMI_BLANK.Value)
			{
				// result = "<linebreak/>";
				result = "";
			}
			else
			if (which.Value == SBMI_BLANK.Value)
			{
				string prefixA = "<red>│</red>";
				string prefixB = " ".Repeat(MENU_OPTION_WIDTH - 1);
				string prefixC = "<red>│</red>";

				result = $"{prefixA}{prefixB}{prefixC}";
			}

			return result;
		}

		// used for menu[0] and is the default
		public string getMenuItemFormattedDef(int menuIdx, string menuKey)
		{
			StringBuilder sb = new StringBuilder();

			Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int> > mi = Menus[menuIdx][menuKey];

			string prefixA = "│ ";
			string prefixB = $"{menuKey,-2}";
			string prefixC = "│";
			string title;

			int spaceWidth = MENU_OPTION_WIDTH - prefixA.Length - prefixB.Length;

			title = formatTitle(mi);

			if (mi.Item6[0] > 0) title += " ❯ ❯❯";

			return
				$"<red>{prefixA}</red>{mi.Item2[0]}{prefixB}{mi.Item2[1]}{" ".Repeat(spaceWidth)}<red>{prefixC}</red> {title}";
		}

		private string formatTitle(Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int> > mi)
		{
			int i;

			StringBuilder sb = new StringBuilder();

			if (mi.Item4.Count != mi.Item3.Count * 2) return "invalid";

			for (i = 0; i < mi.Item3.Count; i++)
			{
				sb.Append(mi.Item4[i * 2]).Append(mi.Item3[i]).Append(mi.Item4[i * 2 + 1]);
			}

			return sb.ToString();
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(SbMain)}";
		}

	#endregion

	#region menu processing
		
		private bool? processMenuChoice(int menuItem, string menuKey)
		{
			DM.Start0();

			return sbh.ProcessMenuChoice(menuItem, menuKey, out menuIdx);
		}

	#endregion

	#region data

		private readonly int leftMenuWidth = (" ".Repeat(MENU_OPTION_WIDTH) + "| ").Length;
		private int rightMenuWidth = 0;

		private int rowIdx = 1;

		private readonly List<string> EXIT_TITLE_TEXT      = new () { "▶❯ ", "Exit", " ❮◀" };

		private readonly List<string> EMPTY_FORMAT         = new () { null, null };
		private readonly List<string> HEADER_TITLE_FORMAT  = new () { "<bold>", "</bold>" };
		private readonly List<string> EXIT_OPTION_FORMAT   = new () { "<magenta>", "</magenta>" };

		private readonly List<string> EXIT_TITLE_FORMAT    =
			new () { null, null, "<magenta>", "</magenta>", null, null };

		private readonly List<string> ITEM_OPTION_FORMAT   = new () { "<bold><green>", "</green></bold>" };
		private readonly string[]     ITEM_HELP_FORMAT_01 = new [] { "<gray>", "</gray>" };

		private readonly List<string> ITEM_TITLE_FORMAT_INACTIVE = new () { null, null, "<slategray>", "</slategray>" };
		private readonly List<string> ITEM_TITLE_FORMAT_ACTIVE_1 = new () { null, null, "<ltgray>", "</ltgray>" };

		private readonly Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int> > BLANK_LINE =
			new (SBMI_BLANK, new () { null, null }, new () { null }, new () { null, null } , 0 , new () { -1 });

		private readonly Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int> > SPACER_LINE =
			new (SBMI_SPACER, new () { null, null }, new () { null }, new () { null, null } , 0 , new () { -1 });

		//formatter: off

		//                              item1           item2        item3         item4        item5  item6
		//                                              key          title         title        title  next
		//               key            function idx    format       text          format       len    menu
		public Dictionary<string, Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int>>>[]
			Menus { get; set; }

		private void configMenu()
		{
			Menus =
				new Dictionary<string, Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int>>>
					[3];

			menu0();
			menu1();
			menu2();
		}

		// formatter: off
		private void menu0()
		{
			menuIdx = 0;

			Menus[menuIdx] =
				new Dictionary<string,
					Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int>>>();

			addheaderItem("Primary Menu");

			addBlankItem();

			addheaderItem("Initialize");
			addMenuItem("I2", MMI_INIT_1, ITEM_OPTION_FORMAT, new () { "▪ ", "Initialize" }                     , ITEM_TITLE_FORMAT_INACTIVE, 0, new () { -2 }, " [using default data]", ITEM_HELP_FORMAT_01);
			addMenuItem("IS", MMI_INIT_2, ITEM_OPTION_FORMAT, new () { "▪ ", "Initialize" }                     , ITEM_TITLE_FORMAT_INACTIVE, 0, new () { -2 }, " [select data]"       , ITEM_HELP_FORMAT_01);

			addSpacerItem();

			addheaderItem("Reset");
			addMenuItem("R0", MMI_RESET_0, ITEM_OPTION_FORMAT, new () { "▪ ", "Reset full" }                     , ITEM_TITLE_FORMAT_INACTIVE, 0, new () { -2 });
			addMenuItem("R2", MMI_RESET_2, ITEM_OPTION_FORMAT, new () { "▪ ", "Reset the data manager" }         , ITEM_TITLE_FORMAT_INACTIVE, 0, new () { -2 });
			addMenuItem("R4", MMI_RESET_4, ITEM_OPTION_FORMAT, new () { "▪ ", "Reset the data file" }            , ITEM_TITLE_FORMAT_INACTIVE, 0, new () { -2 });
			addMenuItem("R6", MMI_RESET_6, ITEM_OPTION_FORMAT, new () { "▪ ", "Reset the sheet file manager" }   , ITEM_TITLE_FORMAT_INACTIVE, 0, new () { -2 });
			addMenuItem("R8", MMI_RESET_8, ITEM_OPTION_FORMAT, new () { "▪ ", "Reset for new sheet" }            , ITEM_TITLE_FORMAT_INACTIVE, 0, new () { -2 });

			addSpacerItem();

			addheaderItem("Add and Remove");
			addMenuItem("A2", MMI_ADD_2, ITEM_OPTION_FORMAT, new () { "▪ ", "Add" }                              , ITEM_TITLE_FORMAT_INACTIVE, 0, new () { -2 }, " [only to empty sheet]"   , ITEM_HELP_FORMAT_01);
			addMenuItem("RM", MMI_ADD_M, ITEM_OPTION_FORMAT, new () { "▪ ", "Remove a sheet" }                   , ITEM_TITLE_FORMAT_INACTIVE, 0, new () { -2 });

			addSpacerItem();

			addheaderItem("Data to Scan");
			addMenuItem("SD", MMI_DATA_SEL1, ITEM_OPTION_FORMAT, new () { "▪ ", "Select data" }                  , ITEM_TITLE_FORMAT_ACTIVE_1, 0, new () { 2 }, " [select a data set]"  , ITEM_HELP_FORMAT_01);
			addMenuItem("RD", MMI_DATA_READ1, ITEM_OPTION_FORMAT, new () { "▪ ", "Read Files" }                  , ITEM_TITLE_FORMAT_INACTIVE, 0, new () { -2 }, " [read the data file]"  , ITEM_HELP_FORMAT_01);

			addSpacerItem();

			addheaderItem("Reports");
			addMenuItem("S1", MMI_SHOW_RPT1, ITEM_OPTION_FORMAT, new () { "▪ ", "Show Report 1" }                  , ITEM_TITLE_FORMAT_ACTIVE_1, 0, new () { -2 });


			addSpacerItem();

			addheaderItem("Operations");
			addMenuItem("SS", MMI_OP_SCAN1, ITEM_OPTION_FORMAT, new () { "▪ ", "Scan Menu" }                    , ITEM_TITLE_FORMAT_ACTIVE_1, 0, new () { 1 });

			addSpacerItem();

			addheaderItem("Completion Options");

			addExitItem("X", new () { -100 });
		}

		private void menu1()
		{
			menuIdx = 1;

			Menus[menuIdx] =
				new Dictionary<string,
					Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int>>>();

			addheaderItem(">01", "Process Data");

			// addMenuItem(">01", EMPTY_FORMAT,new (){"Process Data"}         , HEADER_TITLE_FORMAT, 0, -1);

			addMenuItem("SC", MMI_OP_SCAN1, ITEM_OPTION_FORMAT, new () { "▪ Scan" }         , EMPTY_FORMAT, 0, new () { -2 });

			addExitItem("X", new () { -101 });
		}

		// selection of data to scan
		private void menu2()
		{
			menuIdx = 2;

			int itemIdx = 1;

			Menus[menuIdx] =
				new Dictionary<string,
					Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int>>>();

			addheaderItem(">01", "Select Scan Data Set");

			foreach (KeyValuePair<int, Sample> kvp in SampleData.SampleScanData)
			{
				// Sample s = kvp.Value;

				SbMnuItemId mid = MMI_OP_2_SCDATA;
				mid.SubValue =kvp.Key;

				addMenuItem(kvp.Key.ToString(), mid, ITEM_OPTION_FORMAT, new () { "▪ ", $"{kvp.Value.Description}"}         , ITEM_TITLE_FORMAT_ACTIVE_1, 0, new () { -2 });
			}

			addExitItem("X", new () { -101 });
		}
		


		// formatter: on

		private void addheaderItem(params string[] data)
		{
			string menuKey;
			string title = "missing";

			if (data.Length == 1)
			{
				menuKey = $">{rowIdx++:D2}";
				title = data[0];
			} 
			else
			{
				menuKey = data[0];
				title = data[1];
			}

			addMenuItem(menuKey, SBMI_TITLE, EMPTY_FORMAT, new List<string>([title]), HEADER_TITLE_FORMAT, 0,
				new () { -1 });
		}

		private void addSpacerItem()
		{
			string menuKey = $"<{rowIdx++:D2}";

			Menus[menuIdx].Add(menuKey, SPACER_LINE);
		}

		private void addBlankItem()
		{
			string menuKey = $"<{rowIdx++:D2}";

			// Menus[menuIdx].Add(menuKey, BLANK_LINE);
			Menus[menuIdx].Add(menuKey, BLANK_LINE);
		}

		private void addExitItem(string menuKey, List<int> nextMenu)
		{
			addMenuItem(menuKey, SBMI_EXIT, EXIT_OPTION_FORMAT, EXIT_TITLE_TEXT , EXIT_TITLE_FORMAT, 0, nextMenu);
		}

		private void addMenuItem(
			string key,
			SbMnuItemId menuItemId,
			List<string> keyFmt,
			List<string> ttlText,
			List<string> ttlFmt,
			int ttlLen,
			List<int> nextMenu,
			string helpText = null,
			string[] helpFmt = null)
		{
			int len = 0;
			int i;

			List<string> titleText = new List<string>(ttlText);
			List<string> titleFmt = new List<string>(ttlFmt);

			foreach (string s in ttlText)
			{
				len += s.Length;
			}

			if (helpText != null && helpFmt.Length == 2)
			{
				len += helpText.Length;

				titleText.Add(helpText);
				titleFmt.Add(helpFmt[0]);
				titleFmt.Add(helpFmt[1]);
			}

			if (len > rightMenuWidth) rightMenuWidth = len;

			Menus[menuIdx].Add(key,
				new Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int> >(menuItemId, keyFmt,
					titleText, titleFmt, len,
					nextMenu));
		}

		//formatter: on

	#endregion

	}
}