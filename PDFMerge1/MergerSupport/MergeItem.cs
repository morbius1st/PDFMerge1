// Solution:     PDFMerge1
// Project:       PDFMerge1
// File:             MergeItem.cs
// Created:      -- ()

using System.Collections.Generic;

namespace PDFMerge1
{
	internal class MergeItem
	{
		internal string bookmarkTitle;
		internal bookmarkType bookmarkType;
		internal int pageNumber;
		internal int depth;
		internal FileItem fileItem;
		internal List<MergeItem> mergeItems;

		internal MergeItem(string bookmarkTitle,
			bookmarkType bookmarkType, List<MergeItem> mergeItems,
			int pageNumber, int depth, FileItem fileItem
			)
		{
			this.bookmarkTitle = bookmarkTitle;
			this.bookmarkType = bookmarkType;
			this.mergeItems = mergeItems ?? new List<MergeItem>();
			this.pageNumber = pageNumber;
			this.depth = depth;
			this.fileItem = fileItem;
		}

		internal bool hasChildren => mergeItems.Count > 0;

		internal int count => mergeItems.Count;
	}
}