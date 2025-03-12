// Solution:     PDFMerge1
// Project:       AndyScan
// File:             IProcessOption.cs
// Created:      2024-12-25 (10:31 PM)

namespace AndyScan.Support;

public interface IProcessOption
{
	public int SelectedOption();

	public int SelectedExit();
}