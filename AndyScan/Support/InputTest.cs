// Solution:     PDFMerge1
// Project:       AndyScan
// File:             InputTest.cs
// Created:      2024-12-24 (8:10 AM)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

using AndyScan.SbSystem;

using UtilityLibrary;

namespace AndyScan.Support;

public class InputTest
{
	public Window win;
	private IInputWin iwi;
	public IFdFmt iw;

	private UIElement input;

	public InputTest(IFdFmt iw, IInputWin iwi)
	{
		this.iw = iw;
		this.iwi = iwi;

		input = iwi.GetInputWindow();
		input.IsEnabled = false;
		input.KeyUp += InputOnKeyUp;

		// win = iwi.GetWindow();
		// win.KeyUp += WinOnKeyUp;

	}

	private bool isValid;
	private string option;
	private int maxOptions = 2;
	private string twoCharOptions = "S";

	private List<Tuple<string, string>> optionList = new List<Tuple<string, string>>()
	{
		new Tuple<string, string>("A", "Option A"),
		new Tuple<string, string>("1", "Option 1"),
		new Tuple<string, string>("2", "Option 2"),
		new Tuple<string, string>("3", "Option 3"),
		new Tuple<string, string>("S1", "Option S1"),
		new Tuple<string, string>("S2", "Option S2"),
	};

	public string Option
	{
		get => option;
		set
		{
			option = value;
		}
	}

	public bool IsValid => isValid;

	public void SelectOption()
	{
		option = null;

		showOptions();

		iw.TblkFmtdLine($"select an option> ");

		input.IsEnabled = true;
		input.Focus();
	}

	// private

	private void showOptions()
	{
		int len;

		foreach (Tuple<string, string> opt in optionList)
		{
			len = opt.Item1.Length;

			iw.TblkFmtdLine($" <White><black> {opt.Item1,-3}</black></White><margin spaces={{5}}/><bold>{opt.Item2}</bold>");
		}
	}

	private void getOption(string c)
	{
		// Debug.WriteLine($"got option| >{c}<");

		if (option.IsVoid())
		{
			option = c;

			if (twoCharOptions.IndexOf(c)<0)
			{
				input.IsEnabled = false;
				Option = c;
				processOption();
			}
		} 
		else
		{
			option += c;

			if (c.Length == maxOptions)
			{
				input.IsEnabled = false;
				Option = c;
				processOption();
			}
		}

	}

	private void InputOnKeyUp(object sender, KeyEventArgs e)
	{
		e.Handled = true;

		getOption(e.Key.ToString());
	}

	private void validateOption()
	{
		foreach (Tuple<string, string> t in optionList)
		{
			if (t.Item1.Equals(option))
			{
				isValid = true;
				return;
			}
		}

		isValid = false;
	}
		
	private void processOption()
	{
		validateOption();

		iw.TblkFmtdLine($"the option selected is| {option} ({(isValid ? "valid" : "invalid")})");

		// iwi.SelectedOption();
	}

}