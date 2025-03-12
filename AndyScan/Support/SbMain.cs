#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;

using UtilityLibrary;

#endregion

// username: jeffs
// created:  12/24/2024 8:16:50 AM

namespace AndyScan.Support
{
	public class SbMain : IMenu
	{

		private const int MENU_WIDTH = 30;
		private const int MENU_OPTION_WIDTH = 6;

	#region private fields

		private WinInput wIput;
		private string keyUp;
		private int processKeyIdx;

		private ITblkFmt iw;
		private IWinMain iwm;

		private int menuIdx;

	#endregion

	#region ctor

		public SbMain(ITblkFmt iw, IWinMain iwm)
		{
			this.iw = iw;
			this. iwm = iwm;

			configMenu();
			// processMenu();
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

			foreach (KeyValuePair<string, Tuple<List<string>, List<string>, List<string>, int, List<int> >> kvp in Menus[menuIdx])
			{
				if (kvp.Key.Length<len) continue;

				if (kvp.Key.Substring(0, len).Equals(choice))
				{
					if (kvp.Key.Length == len)
					{
						subMenuIdx = kvp.Value.Item5[0];

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
			return Menus[menuIdx][menuKey].Item5[0];
		}

		public int GetTitleLength(int menuIdx, string menuKey)
		{
			return Menus[menuIdx][menuKey].Item4;
		}
		
		public string getTitlePlain(int menuIdx, string key)
		{
			string answer = "";

			foreach (string s in Menus[menuIdx][key].Item2)
			{
				answer += s;
			}

			return answer;
		}

		public string getKeyFormatted(int menuIdx, string menuKey)
		{
			List<string> codes = Menus[menuIdx][menuKey].Item1;

			if (codes == null || codes.Count != 2) return menuKey;

			return $"{codes[0]}{menuKey}{codes[1]}";
		}

		public string getTitleFormatted(int menuIdx, string menuKey)
		{
			int i;

			StringBuilder sb = new StringBuilder();
			List<string> titles = Menus[menuIdx][menuKey].Item2;
			List<string> codes = Menus[menuIdx][menuKey].Item3;

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

		public string getBlankLineFormatted(int menuIdx)
		{
			if (menuIdx == 0)
			{
				return getBlankLineFormattedDef(menuIdx);
			} 
			else
			{
				return getBlankLineFormattedDef(menuIdx);
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


		private bool? processMenuChoice(int menu, string menuChoice)
		{
			DM.Start0();

			// Debug.WriteLine($"process menu choice| menu = {menu} | item = {menuChoice}");
			bool? result = true;

			switch (menu)
			{
			case 0:
				{
					result = processMenu0(menuChoice);
					break;
				}
			case 1:
				{
					result = processMenu1(menuChoice);
					break;
				}
			}

			DM.End0();

			return result;
		}

		private bool? processMenu0(string menuChoice)
		{
			DM.Start0();

			bool? result = true;

			switch (menuChoice)
			{
			case "RD":
				{
					iw.TblkMsgLine("got choice 'RD'");
					DM.Stat0("got choice 'RD'");
					// Debug.WriteLine("got choice 'RD'");
					result = false;

					if (result == false) menuIdx = 0;

					break;
				}
			case "SC":
				{
					iw.TblkMsgLine("got choice 'SC-1'");
					DM.Stat0("got choice 'SC-1'");
					// Debug.WriteLine("got choice 'SC-1'");
					break;
				}
			case "X":
				{
					iw.TblkMsgLine("got choice 'X-1'");
					DM.Stat0("got choice 'X-1'");
					// Debug.WriteLine("got choice 'X'");
					result = null;
					menuIdx = -2;
					break;
				}
			}

			DM.End0();
			return result;
		}

		private bool? processMenu1(string menuChoice)
		{
			DM.Start0();

			bool? result = true;

			switch (menuChoice)
			{
			case "SC":
				{
					
					iw.TblkMsgLine("got choice 'SC-2'");
					DM.Stat0("got choice 'SC-2'");
					// Debug.WriteLine("got choice 'SC-2'");
					break;
				}
			case "X":
				{
					iw.TblkMsgLine("got choice 'X-2'");
					DM.Stat0("got choice 'X-2'");
					// Debug.WriteLine("got choice 'X'");
					result = null;
					menuIdx = 0;
					break;
				}
			}
			DM.End0();

			return result;
		}

		// private void processMenu()
		// {
		// 	Menus = new  Dictionary<string, Tuple<List<string>, List<string>, List<string>, int, List<int> >>[Menus2.Length];
		//
		// 	Tuple<List<string>, List<string>, List<string>, int, List<int> > t;
		// 	int count;
		//
		// 	for (int i = 0; i < Menus2.Length; i++)
		// 	{
		// 		Menus[i] = new Dictionary<string, Tuple<List<string>, List<string>, List<string>, int, List<int> >>();
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

			Tuple<List<string>, List<string>, List<string>, int, List<int> > mi = Menus[menuIdx][menuKey];

			string prefixA =$"╭{"─".Repeat(MENU_OPTION_WIDTH - 1)}";
			string prefixB = "• ";
			string prefixC = " •";
			string title;

			// int sfxWidth = MENU_WIDTH - mi.Item4 - prefixB.Length - prefixC.Length;
			int sfxWidth = rightMenuWidth - mi.Item4 - prefixC.Length;

			string suffix = "─".Repeat(sfxWidth) + "╮";

			title = formatTitle(mi);

			return $"<red>{prefixA}</red>{prefixB}{title}{prefixC}<red>{suffix}</red>";
		}

		// used for menu[0] and is the default
		public string getBlankLineFormattedDef(int menuIdx)
		{
			string prefixA = "<red>│</red>";
			string prefixB = " ".Repeat(MENU_OPTION_WIDTH - 1);
			string prefixC= "<red>│</red>";

			return $"{prefixA}{prefixB}{prefixC}";
		}

		// used for menu[0] and is the default
		public string getMenuItemFormattedDef(int menuIdx, string menuKey)
		{
			StringBuilder sb = new StringBuilder();

			Tuple<List<string>, List<string>, List<string>, int, List<int> > mi = Menus[menuIdx][menuKey];

			string prefixA = "│ ";
			string prefixB = $"{menuKey,-2}";
			string prefixC = "│";
			string title;

			int spaceWidth = MENU_OPTION_WIDTH - prefixA.Length - prefixB.Length;

			title = formatTitle(mi);

			if (mi.Item5[0] > 0) title += " ❯ ❯❯";

			return $"<red>{prefixA}</red>{mi.Item1[0]}{prefixB}{mi.Item1[1]}{" ".Repeat(spaceWidth)}<red>{prefixC}</red> {title}";
		}

		private string formatTitle(Tuple<List<string>, List<string>, List<string>, int, List<int> > mi)
		{
			int i;

			StringBuilder sb = new StringBuilder();

			if (mi.Item3.Count != mi.Item2.Count * 2) return "invalid";

			for (i = 0; i < mi.Item2.Count; i++)
			{
				sb.Append(mi.Item3[i * 2]).Append(mi.Item2[i]).Append(mi.Item3[i * 2 + 1]);
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

	#region data

		private readonly int leftMenuWidth = (" ".Repeat(MENU_OPTION_WIDTH)+"| ").Length;
		private int rightMenuWidth = 0;

		private int rowIdx = 1;

		private readonly List<string> EXIT_TITLE_TEXT      =new () {"▶❯ ", "Exit", " ❮◀"};

		private readonly List<string> EMPTY_FORMAT         =new () { null, null };
		private readonly List<string> HEADER_TITLE_FORMAT  =new () {"<bold>", "</bold>"};
		private readonly List<string> EXIT_OPTION_FORMAT   =new () {"<magenta>", "</magenta>"};
		private readonly List<string> EXIT_TITLE_FORMAT    =new () {null, null, "<magenta>", "</magenta>", null, null};

		private readonly List<string> ITEM_OPTION_FORMAT   =new () {"<bold><green>", "</green></bold>"};
		private readonly List<string> ITEM_TITLE_FORMAT_02 =new () { null, null, "<gray>", "</gray>" };
		private readonly string[]      ITEM_HELP_FORMAT_01 =new [] {"<gray>", "</gray>" };

		private readonly Tuple<List<string>, List<string>, List<string>, int, List<int> > BLANK_LINE = new (new () { null, null },
			new () { null }, new () { null, null } , 0 , new () {-1 });

		//formatter: off
		public Dictionary<string, Tuple<List<string>, List<string>, List<string>, int, List<int> >>[] Menus { get; set; }

		private void configMenu()
		{
			Menus = new Dictionary<string, Tuple<List<string>, List<string>, List<string>, int, List<int> >>[2];

			menu0();
			menu1();


		}

		private void menu0()
		{
			menuIdx = 0;

			Menus[menuIdx] = new Dictionary<string, Tuple<List<string>, List<string>, List<string>, int, List<int> >>();

			addheaderItem("Primary Menu");

			addheaderItem("Initialize");
			addMenuItem("I2" , ITEM_OPTION_FORMAT,new (){"▪ Initialize"}, EMPTY_FORMAT, 0, new () {-2 }, " [using default data]", ITEM_HELP_FORMAT_01);
			addMenuItem("IS" , ITEM_OPTION_FORMAT,new (){"▪ Initialize"}, EMPTY_FORMAT, 0, new () {-2 }, " [select data]"       , ITEM_HELP_FORMAT_01);

			addBlankItem();

			addheaderItem("Reset");
			addMenuItem("R0" , ITEM_OPTION_FORMAT,new (){"▪ Reset full"}, EMPTY_FORMAT, 0, new () {-2 });
			addMenuItem("R2" , ITEM_OPTION_FORMAT,new (){"▪ Reset the data manager"}, EMPTY_FORMAT, 0, new () {-2 });
			addMenuItem("R4" , ITEM_OPTION_FORMAT,new (){"▪ Reset the data file"}, EMPTY_FORMAT, 0, new () {-2 });
			addMenuItem("R6" , ITEM_OPTION_FORMAT,new (){"▪ Reset the sheet file manager"}, EMPTY_FORMAT, 0, new () {-2 });
			addMenuItem("R8" , ITEM_OPTION_FORMAT,new (){"▪ Reset for new sheet"}, EMPTY_FORMAT, 0, new () {-2 });

			addBlankItem();

			addheaderItem("Add and Remove");
			addMenuItem("A2" , ITEM_OPTION_FORMAT,new (){"▪ Add"}, EMPTY_FORMAT, 0, new () {-2 }, " [only to empty sheet]"       , ITEM_HELP_FORMAT_01);
			addMenuItem("RM" , ITEM_OPTION_FORMAT,new (){"▪ Remove a sheet"}, EMPTY_FORMAT, 0, new () {-2 });

			addBlankItem();

			addheaderItem("Data to Scan");
			addMenuItem("SS" , ITEM_OPTION_FORMAT,new (){"▪ Select data"}, EMPTY_FORMAT, 0, new () {1 });
			addMenuItem("RD" , ITEM_OPTION_FORMAT,new (){"▪ Read Files"}   , EMPTY_FORMAT, 0, new () {-2 });
			addMenuItem("SC" , ITEM_OPTION_FORMAT,new (){"▪ Scan"}         , EMPTY_FORMAT, 0, new () {-2 }, " [preforms a quick scan]", new []{"<gray>", "</gray>"});

			addBlankItem();

			addheaderItem("Completion Options");

			addExitItem("X", new () {-100 });

		}

		private void menu1()
		{
			menuIdx = 1;

			Menus[menuIdx] = new Dictionary<string, Tuple<List<string>, List<string>, List<string>, int, List<int> >>();

			addheaderItem(">01", "Process Data");

			// addMenuItem(">01", EMPTY_FORMAT,new (){"Process Data"}         , HEADER_TITLE_FORMAT, 0, -1);

			addMenuItem("SC" , ITEM_OPTION_FORMAT,new (){"▪ Scan"}         , EMPTY_FORMAT, 0, new () {-2 });

			addExitItem("X", new () {-101 });

			// addMenuItem("X" , EXIT_OPTION_FORMAT, EXIT_TITLE_TEXT           , EXIT_TITLE_FORMAT, 0, -101);
		}

		private void addheaderItem(params string[] title)
		{
			string menuKey = $">{rowIdx++:D2}";

			addMenuItem(menuKey, EMPTY_FORMAT, new List<string>(title), HEADER_TITLE_FORMAT, 0, new () {-1});
		}

		private void addBlankItem()
		{
			string menuKey = $"<{rowIdx++:D2}";

			Menus[menuIdx].Add(menuKey, BLANK_LINE);
		}

		private void addExitItem(string menuKey, List<int> nextMenu)
		{
			addMenuItem(menuKey, EXIT_OPTION_FORMAT, EXIT_TITLE_TEXT , EXIT_TITLE_FORMAT, 0, nextMenu);
		}

		private void addMenuItem(string key, List<string> keyFmt, List<string> ttlText, List<string> ttlFmt, int ttlLen, List<int> nextMenu, string helpText = null, string[] helpFmt=null)
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
				new Tuple<List<string>, List<string>, List<string>, int, List<int> >(keyFmt, titleText, titleFmt, len, nextMenu));
		}

		//formatter: on

	#endregion
	}
}