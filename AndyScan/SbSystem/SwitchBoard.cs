// Solution:     PDFMerge1
// Project:       AndyScan
// File:             SwitchBoard.cs
// Created:      2024-12-24 (8:11 AM)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

using AndyScan.Support;

using UtilityLibrary;

namespace AndyScan.SbSystem
{
	public struct SbMnuItemId(int value)
	{
		public int Value { get; } = value;
		public int SubValue { get; set; } = -1;
	}

	public class SbmnuItems
	{
		public static SbMnuItemId SBMI_PRIOR   = new (-2);
		public static SbMnuItemId SBMI_EXIT    = new (-1);
		public static SbMnuItemId SBMI_INVALID = new (0);
		public static SbMnuItemId SBMI_BLANK   = new (1);
		public static SbMnuItemId SBMI_SPACER  = new (2);
		public static SbMnuItemId SBMI_HEADER  = new (11);
		public static SbMnuItemId SBMI_TITLE   = new (21);
	}

	public class SwitchBoard : IInput
	{
		private IInputWin iwi;
		private ITblkFmt iw;
		private IProcessOption ipo;
		private IMenu menu;

		private UIElement input;
		private bool inputOk;
		private string choice;
		private int processKeyIdx;

		private WinInput wIput;

		private List<Tuple<int,string>> selectedOption;
		private bool isValid;

		private int menuIdx;
		private int subMenuIdx;

		private Dictionary<string, Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int>>>[]
			menus;
		// private string[] multiCharOptions;

		public SwitchBoard(ITblkFmt iw, IInputWin iwi, IProcessOption ipo)
		{
			this.iw = iw;
			this.iwi = iwi;
			this.ipo = ipo;

			input = iwi.GetInputWindow();

			// enableDisableInput(false);
			//
			// input.KeyUp += InputOnKeyUp;

			wIput = new WinInput(iwi, this);
			wIput.DisableInput();

			ProcessKeyUp += OnProcessKeyUp;
		}

		public void SetMenu(IMenu menu)
		{
			this.menu = menu;
		}

		public List<Tuple<int,string>> SelectedOption
		{
			get => selectedOption;
			private set => selectedOption = value;
		}

		public bool IsValid => isValid;

		public Dictionary<string, Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int>>>[] Menu
		{
			get => menus;
			set => menus = value;
		}


		// because this is drawn in a window (textblock), the normal process
		// of waiting for a keypress will not work.
		// these routines only draw the switchboard.  the window then
		// waits for the user to enter a key.  the key is then processed

		// show the switchboard

		public void ShowSb(IMenu menu, int startMenu)
		{
			DM.Start0();

			this.menu = menu;
			menus = menu.Menus;
			menuIdx = startMenu;

			selectedOption = new List<Tuple<int,string>>();

			showSb();

			DM.End0();
		}

		private void requestOptionMsg()
		{
			enableDisableInput(true);

			iw.TblkFmtd("<linebreak/><green>Select an Option | </green> ");

			input.Focus();
		}

		private void showSb()
		{
			DM.Start0();

			choice = null;

			processKeyIdx = 0;

			if (menuIdx == 0) iw.TblkMsgClear();

			drawSb();

			requestOptionMsg();

			DM.End0();
		}

		private void drawSb()
		{
			DM.Start0();

			int menuWidth = 50;

			string text;

			foreach (KeyValuePair<string, Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int>>>
						kvp in menus[menuIdx])
				if (kvp.Key.StartsWith('>'))
					// text = menu.getHeaderFormatted(menuIdx, kvp.Key);
					iw.TblkFmtdLine(menu.getHeaderFormatted(menuIdx, kvp.Key));
				else 
				if (kvp.Key.StartsWith('<'))
					// text = menu.getBlankLineFormatted(menuIdx);
					iw.TblkFmtdLine(menu.getBlankLineFormatted(menuIdx, kvp.Key));
				else
					// text = menu.getMenuItemFormatted(menuIdx, kvp.Key);
					iw.TblkFmtdLine(menu.getMenuItemFormatted(menuIdx, kvp.Key));

			DM.End0();
		}


		// keystroke processing

		public string OnKeyUp
		{
			set
			{
				DM.Start0($"got | {value}");

				RaiseProcessKeyUpEvent(value);

				// if (processKeyIdx == 0)
				// {
				// 	if (!inputOk)
				// 	{
				// 		DM.End0("end 1");
				// 		return;
				// 	}
				//
				// 	getOption(value);
				// }
				// else
				// if (processKeyIdx == 1) 
				// {
				// 	
				// }
				//
				// processKeyIdx = 0;

				DM.End0();
			}
		}

		private void OnProcessKeyUp(object sender, string k)
		{
			DM.Start0();

			switch (processKeyIdx)
			{
			case 0:
				{
					if (!inputOk)
					{
						DM.End0("end 1 - input not good");
						return;
					}

					if (getOption(k) == true)
						handleOption();

					break;
				}
			case 1:
				{
					completeProcessEnd(k);
					processKeyIdx = 0;
					break;
				}
			default:
				{
					wIput.DisableInput();
					processKeyIdx = 0;
					break;
				}
			}

			DM.End0();
		}

		private void handleOption()
		{
			DM.Start0();

			enableDisableInput(false);
			processOption();

			DM.End0();
		}

		private bool? getOption(string c)
		{
			DM.Start0();

			iw.TblkFmtd(c);

			choice += c;

			// bool? result = validateOption();

			// validate the user's choice incrementally - verify each group of characters as entered
			// return
			// false = choice is not valid - characters, as entered, do not match
			// true = choice has matched, return the associated menu index
			// null = matching in progress, characters matched thus far but is not a complete match
			bool? result = menu.ValidateOption(menuIdx, choice, out subMenuIdx);

			if (result == false)
			{
				// entered is no good
				choice = null;
				isValid = false;

				requestOptionMsg();
				DM.Stat0("end 1 - invalid choice");
			}
			else 
			if (result == true)
			{
				// option is one of the valid choices

				DM.Stat0("valid choice");

				iw.TblkMsg("\n"); // no more entries on the enter choice line
				isValid = true;

				// enableDisableInput(false);
				// processOption();
			}

			DM.End0();

			return result;
		}

		// what to do next
		// 0+ = display menu 0+ 
		// -1 = n/a - ignore / nothing or
		// -2 = process choice, then display menu 0
		// -100 = exit SB

		private void processOption()
		{
			DM.Start0();

			iw.TblkMsg("\n");

			selectedOption.Add(new (menuIdx, choice));

			if (subMenuIdx >= 0)
				menuIdx = subMenuIdx;
			else if (subMenuIdx == -2)
			{
				menuIdx = ipo.SelectedOption();
				subMenuIdx = 0;
			}
			else if (subMenuIdx == -100)
				menuIdx = ipo.SelectedExit();
			else if (subMenuIdx == -101)
				menuIdx = 0;

			if (menuIdx >= 0)
				if (menuIdx == 0 && subMenuIdx >= 0)
					verifyProcessEnd();
				else
					showSb();

			DM.End0();
		}

		private void enableDisableInput(bool which)
		{
			DM.Start0();
			input.Focus();
			// input.IsEnabled = which;
			wIput.EnabldDisableInput(which);
			inputOk = which;
			DM.End0();
		}

		// event publish

		public delegate void ProcessKeyUpEventHandler(object sender, string k);

		public event SwitchBoardManager.ProcessKeyUpEventHandler ProcessKeyUp;

		protected virtual void RaiseProcessKeyUpEvent(string k)
		{
			ProcessKeyUp?.Invoke(this, k);
		}

		// process completion verification

		private void verifyProcessEnd()
		{
			DM.InOut0();
			wIput.EnableInput();
			processKeyIdx = 1;
			iw.TblkMsg("\nProcess complete\nPress a key to continue? |");
		}

		private void completeProcessEnd(string k)
		{
			wIput.DisableInput();

			iw.TblkMsgLine($"{k}\n");

			showSb();
		}
	}
}