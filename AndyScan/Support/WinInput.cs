// Solution:     PDFMerge1
// Project:       AndyScan
// File:             WinInput.cs
// Created:      2024-12-25 (10:30 PM)

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using AndyScan.SbSystem;

namespace AndyScan.Support;

public class WinInput
{
	private IInput iPut;
	private bool inputOk;
	private UIElement input;

	public WinInput(IInputWin iwi, IInput iPut)
	{
		this.iPut = iPut;
		input = iwi.GetInputWindow();
		// input.KeyUp += InputOnKeyUp;
		input.TextInput += InputOnTextInput;
	}

	private void InputOnTextInput(object sender, TextCompositionEventArgs e)
	{
		if (!inputOk) return;

		e.Handled = true;

		iPut.OnKeyUp = e.Text.ToUpper();
	}

	public void EnableInput()
	{
		input.IsEnabled = true;
		inputOk = true;
	}

	public void DisableInput()
	{
		input.IsEnabled = false;
		inputOk = false;
	}

	public void EnabldDisableInput(bool which)
	{
		input.IsEnabled = which;
		inputOk = which;
	}

	// private async void InputOnKeyUp(object sender, KeyEventArgs e)
	// {
	// 	if (!inputOk) return;
	//
	// 	e.Handled = true;
	//
	// 	iPut.OnKeyUp = e.Key.ToString();
	//
	// }

}