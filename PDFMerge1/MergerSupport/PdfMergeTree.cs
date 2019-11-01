using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.XMP.Impl;

//using static PDFMerge1.UtilityLocal;
using static PDFMerge1.FileItemType;
using static PDFMerge1.FileList;
using static PDFMerge1.bookmarkType;

using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

namespace PDFMerge1
{
	internal enum bookmarkType
	{
		INVALID,
		BRANCH,
		LEAF
	}

	class PdfMergeTree
	{
		private List<MergeItem> mergeItems = new List<MergeItem>(5);

		private string rootPath { get; set; } = "";

		internal PdfMergeTree(string rootPath)
		{
			if (rootPath == null 
				|| rootPath.Length < 3 
				|| !Directory.Exists(rootPath))
			{
				throw new DirectoryNotFoundException();
			}

			this.rootPath = rootPath;

		}

		internal List<MergeItem> GetMergeItems => mergeItems;

		internal MergeItem this[int index] => mergeItems[index];

		internal int Count => mergeItems.Count;

		internal int count => cnt(mergeItems, 0);

		internal int cnt(List<MergeItem> items, int currentCount)
		{
			foreach (MergeItem mergeItem in items)
			{
				if (mergeItem.hasChildren)
				{
					currentCount = cnt(mergeItem.mergeItems, currentCount);
				}
				else
				{
					currentCount++;
				}
			}

			return currentCount;
		}

		internal void Add(FileList fileList)
		{
			Add(fileList, 0, 0, "\\", 
				mergeItems);
		}

		private int Add(FileList fileList, int index, 
			int currDepth, string priorFileItem,
			List<MergeItem> mergeItems)
		{
			if (index >= fileList.GrossCount) { return index++; }

			int i = index;

			for (; i < fileList.GrossCount; i++)
			{
				if (fileList[i].ItemType == MISSING
					|| fileList[i].ItemType == DIRECTORY)
				{
					continue;
				}

				if (!fileList[i].getOutlineDirectory().GetSubDirectory(currDepth - 1).
					Equals(priorFileItem.GetSubDirectory(currDepth - 1)))
				{
					return i;
				}

				if (fileList[i].Depth > currDepth)
				{
					List<MergeItem> childBookmarks = new List<MergeItem>(1);

					mergeItems.Add(new MergeItem(fileList[i].getOutlineDirectory().GetSubDirectoryName(currDepth), 
						bookmarkType.BRANCH, childBookmarks, -1, currDepth, new FileItem()));

					i = Add(fileList, i, currDepth + 1, fileList[i].getOutlineDirectory(), childBookmarks) - 1;

					continue;
				}

				mergeItems.Add(new MergeItem(fileList[i].getName(), LEAF, null, -1, currDepth, fileList[i]));

			}
			return i;
		}

		private int priorDepth;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			int depth = 0;

			foreach (MergeItem mi in mergeItems)
			{
				sb.Append(listMergeItem(mi, depth));

				if (mi.mergeItems != null)
				{
					sb.Append(listMergeItems(mi.mergeItems, depth + 1));
				}
			}

			return sb.ToString();
		}

		private StringBuilder listMergeItems(List<MergeItem> mergeItems, int depth)
		{
			StringBuilder sb = new StringBuilder();

			foreach (MergeItem mi in mergeItems)
			{
				sb.Append(listMergeItem(mi, depth));

				if (mi.mergeItems != null)
				{
					sb.Append(listMergeItems(mi.mergeItems, depth + 1));
				}
			}

			return sb;
		}

		private StringBuilder listMergeItem(MergeItem mi, int depth)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(fmtMsg("calc'd depth| ", $"{depth, 4}"));
			sb.Append(" vs ").Append(fmt(mi.depth));
			sb.Append(" bookmarkType| ").Append($"{mi.bookmarkType.ToString(),-8}");
			sb.Append(" fileitemType| ").Append($"{mi.fileItem.ItemType.ToString(),-8}");
			sb.Append(" page#| ").Append(fmt(mi.pageNumber));
			sb.Append(" vs ").Append(fmt(findPageNumber(mi)));
			sb.Append(" bookmark title| ").Append(" ".Repeat(depth * 3)).Append(mi.bookmarkTitle);
			sb.Append(nl);

			if (mi.fileItem.ItemType == FILE)
			{
				sb.Append(fmtMsg("file name| ", mi.fileItem.getName())).Append(nl);
				sb.Append(fmtMsg("path| ", mi.fileItem.getFullPath)).Append(nl);
				sb.Append(fmtMsg("outline Path| ", mi.fileItem.outlinePath));
				sb.Append(nl).Append(nl);
			}

			return sb;
		}

		int findPageNumber(MergeItem mergeItem)
		{
			if (mergeItem.fileItem.isMissing) return -2;

			if (mergeItem.pageNumber >= 0) return mergeItem.pageNumber;

			if (mergeItem.mergeItems != null && mergeItem.mergeItems.Count > 0)
			{
				return findPageNumber(mergeItem.mergeItems[0]);
			}

			return -1;
		}
	}
}
