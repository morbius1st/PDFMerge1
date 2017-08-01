using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static PDFMerge1.Utility;
using static PDFMerge1.FileList.FileItemType;


namespace PDFMerge1
{
	public class FileList : IEnumerable<FileList.FileItem>
	{
		public enum FileItemType { NULL, MISSING, FILE, DIRECTORY }

		public class FileItem
		{
			// static
			private static string rootPath;
			internal static int notFound { get; private set; }
			internal static int RootPathLen { get; private set; }

			// instance
			internal string path { get; set; } = null;
			internal bool isFullPath { get; set; } = false;
			internal FileItemType ItemType { get; set; }

			public FileItem()
			{
				ItemType = NULL;
			}

			public FileItem(string path)
			{
				ItemType = DetermineItemType(path);

				if (ItemType != MISSING)
				{
					this.path = setPath(Path.GetFullPath(path));
				}
				else
				{
					// missing only - just save the path for later
					// review 
					this.path = path;
				}
			}

			private string setPath(string testPath)
			{
				if (testPath.Length > RootPathLen &&
					testPath.Substring(0, RootPathLen).Equals(rootPath))
				{
					isFullPath = false;
					return testPath.Substring(RootPathLen);
				}
				isFullPath = true;
				return testPath;

			}

			private static FileItemType DetermineItemType(string testPath)
			{
				if (testPath != null)
				{
					if (File.Exists(testPath))
					{
						// found file
						return FILE;
					}

					if (Directory.Exists(testPath))
					{
						return DIRECTORY;
					}
				}

				notFound++;
				return MISSING;
			}

//			private string adjustForRootedPath(string testPath)
//			{
//				if (!Path.IsPathRooted(testPath))
//				{
//					return testPath;
//				}
//
//				return testPath.Substring(Path.GetPathRoot(testPath).Length - 1);
//			}

			internal int Depth
			{
				get
				{
					if (ItemType == MISSING) { return -1; }
					return path.CountSubstring("\\") - 1;
				}
			}

			internal string getHeading()
			{
				if (ItemType == MISSING) { return ""; }

				string[] directories = Path.GetDirectoryName(path).Split('\\');

				return directories[directories.Length - 1];
			}

			internal string getName()
			{
				if (ItemType == MISSING) { return ""; }

				return Path.GetFileNameWithoutExtension(path);
			}

//			internal string getDirectoryNameToDepth(int requestedDepth)
//			{
//				if (ItemType == FileItemType.INVALID) { return ""; }
//
//				if (requestedDepth == 0) { return "\\"; }
//
//				string[] splitDir = getDirectory().Split('\\');
//
//				if (requestedDepth > Depth) { requestedDepth = Depth; }
//
//				if (requestedDepth > splitDir.Length)
//				{
//					requestedDepth = splitDir.Length;
//				}
//
//				string finalPath = "";
//
//				for (int i = 1; i < requestedDepth + 1; i++)
//				{
//					finalPath += "\\" + splitDir[i];
//				}
//
//				return finalPath;
//
//			}
//
//			internal string getDirectoryNameAtDepth(int depth)
//			{
//				if (ItemType == FileItemType.INVALID) { return ""; }
//
//				string[] splitDir = path.Split('\\');
//
//				return depth + 1 >= splitDir.Length ? "" : splitDir[depth + 1];
//
//			}

			internal string getDirectory()
			{
				if (ItemType == MISSING) { return ""; }

				return Path.GetDirectoryName(path) ?? "\\";
			}

			internal string getFullPath
			{
				get
				{
					if (ItemType == MISSING) { return ""; }

					if (isFullPath) { return path; }

					return Path.GetFullPath(RootPath + "\\" + path);
				}
			}

			public static string RootPath
			{
				get { return rootPath; }
				set
				{
					if (value == null || value.Length <= 3)
					{
						rootPath = null;
					}

					rootPath = Path.GetFullPath(value);
					RootPathLen = RootPath.Length;
				}
			}
		}

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

		internal int Add(string dirEntry)
		{
			if (dirEntry == null) { return -1; }

			FileItems.Add(new FileItem(dirEntry));

			return FileItems.Count - 1;
		}

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

		internal bool Add()
		{
			return Add("*.pdf", SearchOption.AllDirectories);
		}

		// add files
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


		//		// add files and directories
		//		internal bool Add(string pattern, SearchOption searchOption)
		//		{
		//			if (pattern == null) { pattern = "*";  }
		//
		//			List<string> directoryItem = new List<string>(5);
		//
		//			foreach (string file in Directory.EnumerateFiles(RootPath, pattern, SearchOption.TopDirectoryOnly))
		//			{
		//				directoryItem.Add(file);
		//			}
		//
		//			foreach (string directory in Directory.EnumerateDirectories(RootPath, "*", searchOption))
		//			{
		//				directoryItem.Add(directory);
		//
		//				foreach (string file in Directory.EnumerateFiles(directory, pattern))
		//				{
		//					directoryItem.Add(file);
		//				}
		//			}
		//			return Add(directoryItem.ToArray());
		//		}

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

//		private bool canMove(int from, int to)
//		{
//			return FileItems[from].getDirectory().Equals(FileItems[to].getDirectory());
//		}


		internal string listFiles()
		{
			if (FileItems.Count == 0) return "empty" + nl;

			StringBuilder sb = new StringBuilder();

			sb.Append(nl);
			sb.Append(fmtMsg("root path| ", RootPath)).Append(nl);
			sb.Append(fmtMsg("gross count| ", GrossCount)).Append(nl);
			sb.Append(fmtMsg("net count| ", NetCount)).Append(nl);
			sb.Append(nl);

			for (int i = 0; i < GrossCount; i++)
			{
				if (FileItems[i].ItemType != FILE)
				{
					logMsgln("idx| ", i + " type| " 
						+ FileItems[i].ItemType 
						+ "  item| " + FileItems[i].path);
					continue;
				}

				sb.Append(FileItems[i].path).Append(nl);

			}

			return sb.ToString();
		}

		public override string ToString()
		{
			if (FileItems.Count == 0) return "empty" + nl;

			StringBuilder sb = new StringBuilder();

			int idx = 0;

			sb.Append(nl);
			sb.Append(fmtMsg("root path| ", RootPath)).Append(nl);
			sb.Append(fmtMsg("gross count| ", GrossCount)).Append(nl);
			sb.Append(fmtMsg("net count| ", NetCount)).Append(nl);
			sb.Append(nl);


			foreach (FileItem fi in FileItems)
			{
				sb.Append(fmtMsg("index| ", $"{idx++ , 3}"));
				sb.Append("  path type| ").Append(fi.ItemType);
				sb.Append("  path| ");

				if (fi.ItemType == FILE)
				{
					sb.Append(fi.getDirectory());
					sb.Append("  name| ").Append(fi.getName());
					sb.Append(nl);
					sb.Append(fmtMsg("requestedDepth| ", $"{fi.Depth , 3}"));

				}
				else
				{
					sb.Append(fi.path);
				}

				sb.Append(nl);
				sb.Append(fmtMsg("expansion| ", ""));

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
