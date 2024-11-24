
using System;

// using Sylvester.FileSupport;
using UtilityLibrary;
// Solution:     PDFMerge1
// Project:       Sylvester
// File:             PathChangeArgs.cs
// Created:      -- ()

namespace Sylvester.UserControls 
{
	public class PathChangeArgs : EventArgs
	{
		public int Index;
		public string SelectedFolder;
		public FilePath<FileNameSimple> SelectedPath;

		public PathChangeArgs(int index, string selectedFolder, FilePath<FileNameSimple> selectedPath)
		{
			this.Index = index;
			this.SelectedFolder = selectedFolder;
			this.SelectedPath = selectedPath;
		}
	}
}

