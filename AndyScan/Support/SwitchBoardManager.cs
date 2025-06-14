// Solution:     PDFMerge1
// Project:       AndyScan
// File:             SwitchBoardManager.cs
// Created:      2024-12-24 (8:09 AM)

using System;
using System.Collections.Generic;
using System.Diagnostics;

using AndyScan.SbSystem;

using UtilityLibrary;

namespace AndyScan.Support;

public class SwitchBoardManager : IProcessOption, IInput
{
	private SwitchBoard Sb;

	private WinInput wIput;
	private int processKeyIdx;

	private IMenu2 menu;
	private int menuIdx = 0;

	private IFdFmt iw;
	private IInputWin iwi;
	private IWinMain iwm;

	public SwitchBoardManager(IFdFmt iw, IInputWin iwi, IWinMain iwm)
	{
		this.iw = iw;
		this.iwi = iwi;
		this.iwm =iwm;

		Sb = new SwitchBoard(iw, iwi, this);

		wIput = new WinInput(iwi, this);
		wIput.DisableInput();

		this.ProcessKeyUp += OnProcessKeyUp;
	}

	public void ProcessSb(IMenu2 menu, int menuIdx = 0)
	{
		DM.Start0();
		
		this.menu = menu;
		this.menuIdx = menuIdx;
		
		showSb();
		
		DM.End0();
	}

	public int SelectedOption()
	{
		DM.Start0();

		// int result = menu.ProcessMenuChoice(getChoice(Sb.SelectedOption));

		DM.End0();

		// return result;

		return 0;
	}

	public int SelectedExit()
	{
		DM.Start0();

		ProcessExitRequest();

		DM.End0();
	
		return -1;
	}

	private void showSb()
	{
		Sb.ShowSb(menu, menuIdx);
	}

	private Tuple<int, string, string> getChoice(List<Tuple<int,string>> menuChoices)
	{
		int menuIdx = -1;
		string menuChoice;
		string title = "";

		if (menuChoices == null || menuChoices.Count == 0) 
			return new Tuple<int, string, string>(menuIdx, "", "");

		menuIdx = menuChoices[^1].Item1;
		menuChoice = menuChoices[^1].Item2;
		title = getMenuTitle(menuIdx, menuChoice);

		return new Tuple<int, string, string>(menuIdx, menuChoice, title);
	}

	private string getMenuTitle(int menuIdx, string menuKey)
	{
		// return this.menu.getTitlePlain(menuIdx, menuKey);

		return "";
	}

	public string OnKeyUp
	{
		set
		{
			DM.Start0();
			RaiseProcessKeyUpEvent(value);
			DM.End0();
		}
	}

	// process exit request

	private void ProcessExitRequest()
	{
		DM.Start0();

		processKeyIdx = -100;
		verifyExit();
		DM.End0();
	}

	private void verifyExit()
	{
		DM.InOut0();
		wIput.EnableInput();
		iw.TblkMsg("Verify, exit SwitchBoard (Y/N)? |");
	}

	// event consume

	private void OnProcessKeyUp(object sender, string k)
	{
		DM.Start0(true);

		switch (processKeyIdx)
		{
		case -100:
			{
				wIput.DisableInput();

				iw.TblkMsgLine($"{k}\n");

				if (k.Equals("Y"))
				{
					iwm.ProcessMsg(-100);
					return;
				}

				showSb();
					
				break;
			}
		default:
			{
				wIput.DisableInput();
				break;
			}
		}

		DM.End0("processKey", true);
	}

	// event publish

	public delegate void ProcessKeyUpEventHandler(object sender, string k);

	public event SwitchBoardManager.ProcessKeyUpEventHandler ProcessKeyUp;

	protected virtual void RaiseProcessKeyUpEvent(string k)
	{
		ProcessKeyUp?.Invoke(this, k);
	}

	// show info

	// private void showSelected()
	// {
	// 	Tuple<int, string, string> choice;
	//
	// 	iw.TblkFmtdLine($"selected| is ({(Sb.IsValid ? "valid" : "invalid")})");
	//
	// 	for (var i = 0; i < Sb.SelectedOption.Count; i++)
	// 	{
	// 		choice = divideMenuChoice(Sb.SelectedOption[i]);
	//
	// 		iw.TblkFmtdLine($"\t| ({i}) | menu ({choice.Item1}) | item ({choice.Item2}) | title [{choice.Item3}]");
	// 	}
	// }

}