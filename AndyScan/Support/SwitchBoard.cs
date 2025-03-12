// Solution:     PDFMerge1
// Project:       AndyScan
// File:             SwitchBoard.cs
// Created:      2024-12-24 (8:11 AM)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UtilityLibrary;

namespace AndyScan.Support
{
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

		private List<string> selectedOption;
		private bool isValid;

		private int menuIdx;
		private int subMenuIdx;
		private Dictionary<string, Tuple<List<string>, List<string>, List<string>, int, List<int>>>[]menus;
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

			this.ProcessKeyUp += OnProcessKeyUp;
		}

		public void SetMenu(IMenu menu)
		{
			this.menu = menu;
		}

		public List<string> SelectedOption
		{
			get => selectedOption;
			private set => selectedOption = value;
		}

		public bool IsValid => isValid;

		public Dictionary<string, Tuple<List<string>, List<string>, List<string>, int, List<int>>>[] Menu
		{
			get => menus;
			set => menus = value;
		}

		public void SelectSbOption(IMenu menu, int startMenu)
		{
			DM.Start0();
			
			this.menu = menu;
			menus = menu.Menus;
			menuIdx = startMenu;

			selectedOption = new List<string>();

			showSbStart();
			
			DM.End0();
		}

		private void showSbStart()
		{
			DM.Start0();

			choice = null;

			processKeyIdx = 0;

			if (menuIdx == 0) iw.TblkMsgClear();

			showSb();

			requestOption();
			
			DM.End0();
		}

		private void requestOption()
		{
			enableDisableInput(true);

			iw.TblkFmtd("<linebreak/><green>Select an Option | </green> ");

			input.Focus();
		}

		private void showSb()
		{
			DM.Start0();

			int menuWidth = 50;

			string text;

			foreach (KeyValuePair<string, Tuple<List<string>, List<string>, List<string>, int, List<int>>> kvp in menus[menuIdx])
			{
				if (kvp.Key.StartsWith('>'))
				{
					text = menu.getHeaderFormatted(menuIdx, kvp.Key);
					iw.TblkFmtdLine(menu.getHeaderFormatted(menuIdx, kvp.Key));
				}
				else
				if (kvp.Key.StartsWith('<'))
				{
					text = menu.getBlankLineFormatted(menuIdx);
					iw.TblkFmtdLine(menu.getBlankLineFormatted(menuIdx));
				}
				else
				{
					text = menu.getMenuItemFormatted(menuIdx, kvp.Key);
					iw.TblkFmtdLine(menu.getMenuItemFormatted(menuIdx, kvp.Key));
				}
			}
		}

		public string OnKeyUp
		{
			set
			{
				DM.Start0();

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
						DM.End0("end 1");
						return;
					}

					getOption(k);
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

		private void getOption(string c)
		{
			DM.Start0();

			iw.TblkFmtd(c);

			choice += c;

			// bool? result = validateOption();

			// validate the user's choice incrementally - verify each set characters as entered
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

				requestOption();
				DM.End0("end 1");
				return;
			} 

			if (result == true)
			{
				// option is one of the valid choices

				iw.TblkMsg("\n"); // no more entries on the enter choice line
				isValid = true;

				enableDisableInput(false);
				processOption();
			}

			DM.End0();
		}

		// what to do next
		// 0+ = display menu 0+ 
		// -1 = n/a - ignore / nothing or
		// -2 = process choice, then display menu 0
		// -100 = exit SB

		private void processOption()
		{
			DM.Start0();

			// iw.DebugFmtd("<linebreak/><linebreak/>");
			iw.TblkMsg("\n");

			selectedOption.Add($"{menuIdx}_{choice}");

			if (subMenuIdx >= 0)
			{
				menuIdx = subMenuIdx;
			}
			else
			if (subMenuIdx == -2)
			{
				menuIdx = ipo.SelectedOption();
				subMenuIdx = 0;
			}
			else
			if (subMenuIdx == -100)
			{
				menuIdx = ipo.SelectedExit();
			}
			else
			if (subMenuIdx == -101)
			{
				menuIdx = 0;
			}

			if (menuIdx >= 0)
			{
				if (menuIdx == 0 && subMenuIdx >= 0)
				{
					verifyProcessEnd();
				}
				else
				{
					showSbStart();
				}
			} 

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

			showSbStart();
		}

	}
}
