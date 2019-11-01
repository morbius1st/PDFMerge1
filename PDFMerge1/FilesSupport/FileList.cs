using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using static PDFMerge1.UtilityLocal;
using static PDFMerge1.FileItemType;

using static UtilityLibrary.MessageUtilities;


namespace PDFMerge1
{
	public enum FileItemType
	{
		NULL,
		MISSING,
		FILE,
		DIRECTORY
	}


	public class FileList : IEnumerable<FileItem>
	{
		private List<FileItem> FileItems = new List<FileItem>();

		public FileList(string rootPath)
		{
			if (!Directory.Exists(rootPath))
			{
				throw new DirectoryNotFoundException();
			}

			FileItem.RootPath = rootPath;
		}

		internal FileItem this[int index] => FileItems[index];

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<FileItem> GetEnumerator()
		{
			return FileItems.GetEnumerator();
		}

		internal int GrossCount => FileItems.Count;
		internal int NetCount => FileItems.Count - FileItem.notFound;
		internal string RootPath => FileItem.RootPath;

		// add a single file
		internal int Add(string dirEntry)
		{
			if (dirEntry == null) { return -1; }

			FileItems.Add(new FileItem(dirEntry));

			return FileItems.Count - 1;
		}

		// add an array of files
		internal bool Add(string[] files)
		{
			bool result = true;

			if (files == null || files.Length == 0) { return false; }

			foreach (string file in files)
			{
				result = (Add(file) != -1) && result;
			}
			return result;
		}

		// add files based on a pattern
		internal bool Add(string pattern, SearchOption searchOption)
		{
			if (pattern == null) { pattern = "*.pdf";  }

			List<string> directoryItem = new List<string>(5);

			foreach (string file in Directory.EnumerateFiles(RootPath, pattern, searchOption))
			{
				directoryItem.Add(file);
			}

			return Add(directoryItem.ToArray());
		}

		// move only allowed within the same directory requestedDepth
		internal bool Move(int from, int to)
		{
			if (from < 0 || to < 0
				|| from == to) { return false; }

			int remove = from;

			if (to < from) { remove++; }

			FileItems.Insert(to, FileItems[from]);
			FileItems.RemoveAt(remove);

			return true;
		}

		public override string ToString()
		{
			if (FileItems.Count == 0) return "empty" + nl;

			StringBuilder sb = new StringBuilder();

			int idx = 0;

			sb.Append(nl);
			sb.Append(fmtMsg("root path", RootPath)).Append(nl);
			sb.Append(fmtMsg("gross count", GrossCount)).Append(nl);
			sb.Append(fmtMsg("net count", NetCount)).Append(nl);
			sb.Append(nl);


			foreach (FileItem fi in FileItems)
			{
				sb.Append(fmtMsg("index", $"{idx++ , 3}"));
				sb.Append("  path type").Append(fi.ItemType);
				sb.Append("  path");

				if (fi.ItemType == FILE)
				{
					sb.Append(fi.getDirectory());
					sb.Append("  name").Append(fi.getName());
					sb.Append(nl);
					sb.Append(fmtMsg("requestedDepth", $"{fi.Depth , 3}"));

				}
				else
				{
					sb.Append(fi.path);
				}

				sb.Append(nl);
				sb.Append(fmtMsg("expansion", ""));

				for (int i = 0; i < fi.Depth + 1; i++)
				{
					sb.Append("(").Append(i).Append(") >").Append(fi.getDirectory().GetSubDirectory(i)).Append("<");
					if (i != fi.Depth)
					{
						sb.Append(" :: ");
					}
				}
				sb.Append(nl);
				sb.Append(nl);

			}

			return sb.ToString();
		}
	}
}
