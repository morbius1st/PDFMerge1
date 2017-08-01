using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.XMP.Impl;
using static PDFMerge1.Utility;
using static PDFMerge1.FileList.FileItemType;
using static PDFMerge1.FileList;
using static PDFMerge1.PdfMergeTree.bookmarkType;

namespace PDFMerge1
{
	class PdfMergeTree
	{
		internal enum bookmarkType { INVALID, BRANCH, LEAF }

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
				int pageNumber, int depth, FileItem fileItem)
			{
				this.bookmarkTitle = bookmarkTitle;
				this.bookmarkType = bookmarkType;
				this.mergeItems = mergeItems;
				this.pageNumber = pageNumber;
				this.depth = depth;
				this.fileItem = fileItem;
			}
		}

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

		internal void Add(FileList fileList)
		{
			logMsgFmtln("gross count| ", fileList.GrossCount.ToString());
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

				if (!fileList[i].getDirectory().GetSubDirectory(currDepth - 1).
					Equals(priorFileItem.GetSubDirectory(currDepth - 1)))
				{
					return i;
				}

				if (fileList[i].Depth > currDepth)
				{
					List<MergeItem> childBookmarks = new List<MergeItem>(1);

					mergeItems.Add(new MergeItem(fileList[i].getDirectory().GetSubDirectoryName(currDepth), bookmarkType.BRANCH, childBookmarks, -1, currDepth, new FileItem()));

					i = Add(fileList, i, currDepth + 1, fileList[i].getDirectory(),childBookmarks) - 1;

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
			sb.Append(" bookmark| ").Append(" ".Repeat(depth * 3)).Append(mi.bookmarkTitle);
			sb.Append(nl);

			if (mi.fileItem.ItemType == FILE)
			{
				sb.Append(fmtMsg("   path| ", mi.fileItem.getFullPath));
				sb.Append(nl).Append(nl);
			}

			return sb;
		}
	}
}
