﻿// Solution:     PDFMerge1
// Project:       Sylvester
// File:             PathChangeArgs.cs
// Created:      -- ()

using System;
using Sylvester.FileSupport;

namespace Sylvester.UserControls 
{
	public class PathChangeArgs : EventArgs
	{
		public int Index;
		public string SelectedFolder;
		public Route SelectedPath;

		public PathChangeArgs(int index, string selectedFolder, string selectedPath)
		{
			Index = index;
			SelectedFolder = selectedFolder;
			SelectedPath = new Route(selectedPath);
		}
	}
}