// Solution:     PDFMerge1
// Project:       PDFMerge1
// File:             MergeItem.cs
// Created:      -- ()

using System.Collections.Generic;
using Felix.FileListManager;

namespace Felix.PDFMerge.PDFMergeTree
{
	internal enum TreeNodeType
	{
		INVALID,
		BRANCH,
		LEAF
	}

	internal class PDFMergeItem 
	{
		internal string bookmarkTitle;
		internal TreeNodeType TreeNodeType;
		internal int pageNumber;
		internal int depth;
		internal FileItem fileItem;
		internal List<PDFMergeItem> mergeItems;

		internal PDFMergeItem(string bookmarkTitle,
			TreeNodeType treeNodeType, List<PDFMergeItem> mergeItems,
			int pageNumber, int depth, FileItem fileItem
			)
		{
			this.bookmarkTitle = bookmarkTitle;
			this.TreeNodeType = treeNodeType;
			this.mergeItems = mergeItems ?? new List<PDFMergeItem>();
			this.pageNumber = pageNumber;
			this.depth = depth;
			this.fileItem = fileItem;
		}

		internal bool hasChildren => mergeItems.Count > 0;

		internal int count => mergeItems.Count;


	}
}