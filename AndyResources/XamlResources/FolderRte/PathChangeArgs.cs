// Solution:     PDFMerge1
// Project:       Sylvester
// File:             PathChangeArgs.cs
// Created:      -- ()

using System;

using UtilityLibrary;

namespace AndyResources.XamlResources.FolderRte
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

