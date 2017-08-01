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
using static PDFMerge1.BookmarkTree.bookmarkType;

namespace PDFMerge1
{
	class BookmarkTree
	{
		internal enum bookmarkType { INVALID, BRANCH, LEAF }

		internal class Bookmark
		{
			internal string title;
			internal bookmarkType bookmarkType;
			internal int pageNumber;
			internal int depth;
			internal FileItem fileItem;
			internal List<Bookmark> bookmarks;

			internal Bookmark(string title, int depth, int pageNumber, 
				bookmarkType bookmarkType, FileItem fileItem, List<Bookmark> bookmarks)
			{
				this.title = title;
				this.pageNumber = pageNumber;
				this.depth = depth;
				this.bookmarkType = bookmarkType;
				this.fileItem = fileItem;
				this.bookmarks = bookmarks;
			}
		}

		private List<Bookmark> bookmarks = new List<Bookmark>(5);

		private string rootPath { get; set; } = "";

		internal BookmarkTree(string rootPath)
		{
			if (rootPath == null 
				|| rootPath.Length < 3 
				|| !Directory.Exists(rootPath))
			{
				throw new DirectoryNotFoundException();
			}

			this.rootPath = rootPath;

		}
		internal List<Bookmark> getBookmarks => bookmarks;

		internal Bookmark this[int index] => bookmarks[index];

		internal int Count => bookmarks.Count;

		internal void Add(FileList fileList)
		{
			logMsgFmtln("gross count| ", fileList.GrossCount.ToString());
			Add(fileList, 0, 0, "\\", 
				bookmarks);
		}

		private int Add(FileList fileList, int index, int currDepth,
			string priorFileItem,
			List<Bookmark> bookmarks)
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
					List<Bookmark> childBookmarks = new List<Bookmark>(1);

					bookmarks.Add(new Bookmark(fileList[i].getDirectory().GetSubDirectoryName(currDepth), 
						currDepth, -1, bookmarkType.BRANCH, new FileItem(), childBookmarks));

					i = Add(fileList, i, currDepth + 1, fileList[i].getDirectory(),childBookmarks) - 1;

					continue;
				}

				bookmarks.Add(new Bookmark(fileList[i].getName(), 
					currDepth, -1, LEAF, fileList[i], null));

			}
			return i;
		}

		private int priorDepth;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			int depth = 0;

			foreach (Bookmark bm in bookmarks)
			{
				sb.Append(listBookmark(bm, depth));


				if (bm.bookmarks != null)
				{
					sb.Append(listBookmarks(bm.bookmarks, depth + 1));
				}
			}

			return sb.ToString();
		}

		private StringBuilder listBookmarks(List<Bookmark> bookmarks, int depth)
		{
			StringBuilder sb = new StringBuilder();

			foreach (Bookmark bm in bookmarks)
			{
				sb.Append(listBookmark(bm, depth));

				if (bm.bookmarks != null)
				{
					sb.Append(listBookmarks(bm.bookmarks, depth + 1));
				}
			}

			return sb;
		}

		private StringBuilder listBookmark(Bookmark bm, int depth)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(fmtMsg("calc'd depth| ", $"{depth, 4}"));
			sb.Append(" vs ").Append(fmt(bm.depth));
			sb.Append(" bookmarkType| ").Append($"{bm.bookmarkType.ToString(),-8}");
			sb.Append(" fileitemType| ").Append($"{bm.fileItem.ItemType.ToString(),-8}");
			sb.Append("  bookmark| ").Append(" ".Repeat(depth * 3)).Append(bm.title);
			sb.Append(nl);

			if (bm.fileItem.ItemType == FILE)
			{
				sb.Append(fmtMsg("   path| ", bm.fileItem.getFullPath));
				sb.Append(nl).Append(nl);
			}

			return sb;
		}
	}
}
