// Solution:     PDFMerge1
// Project:       AndyScan
// File:             SwitchBoardManager.cs
// Created:      2024-12-24 (8:09 AM)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UtilityLibrary;

namespace AndyScan.Support;

public class SwitchBoardManager : IProcessOption, IInput
{
	private SwitchBoard Sb;

	private WinInput wIput;
	private int processKeyIdx;

	private IMenu menu;
	private int menuIdx = 0;

	private ITblkFmt iw;
	private IInputWin iwi;
	private IWinMain iwm;

	public SwitchBoardManager(ITblkFmt iw, IInputWin iwi, IWinMain iwm)
	{
		this.iw = iw;
		this.iwi = iwi;
		this.iwm =iwm;

		Sb = new SwitchBoard(iw, iwi, this);

		wIput = new WinInput(iwi, this);
		wIput.DisableInput();

		this.ProcessKeyUp += OnProcessKeyUp;
	}

	public void ProcessSb(IMenu menu, int menuIdx = 0)
	{
		DM.Start0();
		
		this.menu = menu;
		this.menuIdx = menuIdx;
		
		startSb();
		
		DM.End0();
	}

	public int SelectedOption()
	{
		DM.Start0();

		int result = menu.ProcessMenuChoice(getChoice(Sb.SelectedOption));

		DM.End0();

		return result;
	}

	public int SelectedExit()
	{
		DM.Start0();

		ProcessExitRequest();

		DM.End0();
	
		return -1;
	}

	private void startSb()
	{
		Sb.SelectSbOption(menu, menuIdx);
	}

	private Tuple<int, string, string> getChoice(List<string> selectedOptions)
	{
		return divideMenuChoice(selectedOptions[^1]);
	}

	private Tuple<int, string, string> divideMenuChoice(string choice)
	{
		int menuIdx = -1;
		string title = "";
		string[] option = choice.Split('_');

		if (option.Length != 2 ) return new Tuple<int, string, string>(menuIdx, "", "");

		if (int.TryParse(option[0], out menuIdx))
		{
			title = getMenuTitle(menuIdx, option[1]);
		}

		return new Tuple<int, string, string>(menuIdx, option[1], title);
	}

	private string getMenuTitle(int menuIdx, string menuKey)
	{
		return this.menu.getTitlePlain(menuIdx, menuKey);
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

				startSb();
					
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

	private void showSelected()
	{
		Tuple<int, string, string> choice;

		iw.TblkFmtdLine($"selected| is ({(Sb.IsValid ? "valid" : "invalid")})");

		for (var i = 0; i < Sb.SelectedOption.Count; i++)
		{
			choice = divideMenuChoice(Sb.SelectedOption[i]);

			iw.TblkFmtdLine($"\t| ({i}) | menu ({choice.Item1}) | item ({choice.Item2}) | title [{choice.Item3}]");
		}
	}

}