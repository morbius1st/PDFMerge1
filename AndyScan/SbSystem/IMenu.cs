// Solution:     PDFMerge1
// Project:       AndyScan
// File:             IMenu.cs
// Created:      2024-12-25 (10:31 PM)

using System;
using System.Collections.Generic;

namespace AndyScan.SbSystem;

public interface IMenu2
{
	Dictionary<string, Tuple<SbMnuItemId, List<string>, List<string>, List<string>, int, List<int>>>[] Menus { get; set; }
	// Dictionary<string, Tuple<string, int>>[] Menus4 { get; set; }

	public int ProcessMenuChoice(Tuple<int, string, string> menuKey);


	public int GetNextMenu(int menuIdx, string menuKey);
	public int GetTitleLength(int menuIdx, string menuKey);
	public string getTitlePlain(int menuIdx, string key);
	public string getKeyFormatted(int menuIdx, string menuKey);
	public string getTitleFormatted(int menuIdx, string menuKey);

	public string getHeaderFormatted(int menuIdx, string menuKey);
	public string getBlankLineFormatted(int menuIdx, string menuKey);
	public string getMenuItemFormatted(int menuIdx, string menuKey);

	public bool? ValidateOption(int menuIdx, string choice, out int subMenuIdx);


}