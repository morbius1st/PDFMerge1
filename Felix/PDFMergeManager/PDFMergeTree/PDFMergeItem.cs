// Solution:     PDFMerge1
// Project:       PDFMerge1
// File:             MergeItem.cs
// Created:      -- ()

using System.Collections.Generic;
using Felix.FileListManager;

namespace Felix.PDFMergeManager.PDFMergeTree
{
	internal enum TreeNodeType
	{
		INVALID,
		BRANCH,
		LEAF
	}

	public class PDFMergeItem 
	{
		internal string bookmarkTitle;
		internal TreeNodeType TreeNodeType;
		internal int pageNumber;
		internal int depth;
		internal FileItem fileItem;
		internal Dictionary<string, PDFMergeItem> mergeItems;

		internal PDFMergeItem(
			string bookmarkTitle,
			TreeNodeType treeNodeType, 
			int pageNumber, 
			int depth, 
			FileItem fileItem
			)
		{
			this.bookmarkTitle = bookmarkTitle;
			this.TreeNodeType = treeNodeType;
			this.pageNumber = pageNumber;
			this.depth = depth;
			this.fileItem = fileItem;
			this.mergeItems = null;
		}

		internal bool hasChildren => mergeItems.Count > 0;

		internal int count => mergeItems.Count;

		public void AddMergeItems()
		{
			mergeItems = new Dictionary<string, PDFMergeItem>();
		}


	}
}