#region + Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;
using AndyShared;

#endregion


// projname: AODeliverable.FileSelection
// itemname: FileItem
// username: jeffs
// created:  11/3/2019 8:30:24 AM


namespace AODeliverable.FileSelection
{
	public enum FileItemType
	{
		NULL,
		MISSING,
		FILE,
		DIRECTORY
	}

	public class FileItem : IComparable<FileItem>
	{ 

		// static
		private static string rootPath;
		internal static int notFound { get; private set; }
		internal static int RootPathLen { get; private set; }

		// instance
		// path is the actual sub-folder path 
		// starting from the rootpath down to
		// the actual folder
		internal string path { get; set; } = null;

		// this is the path of names that will define the 
		// bookmark names.  for a merge based on
		// using a folder structure, this will match
		// the path value.  
		// for using a created outline structure,
		// this will need to be manufactured
		internal string outlinePath { get; set; } = null;

		internal bool isFullPath { get; set; } = false;
		internal FileItemType ItemType { get; set; }



		public FileItem()
		{
			ItemType = FileItemType.NULL;
		}

		public FileItem(string path)
		{
			ItemType = DetermineItemType(path);

			if (ItemType != FileItemType.MISSING)
			{
				this.path = setPath(Path.GetFullPath(path));

				// initial, make the outlinepath match the 
				// file path
				this.outlinePath = this.path;
			}
			else
			{
				// missing only - just save the path for later
				// review 
				this.path = path;
				this.outlinePath = path;
			}
		}


		internal bool isMissing
		{
			get { return ItemType == FileItemType.MISSING; }
		}

		internal int Depth
		{
			get
			{
				if (isMissing) { return -1; }

				return outlinePath.CountSubstring("\\") - 1;
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

		public int CompareTo(FileItem other)
		{
			return outlinePath.CompareTo(other.outlinePath);
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
					return FileItemType.FILE;
				}

				if (Directory.Exists(testPath))
				{
					return FileItemType.DIRECTORY;
				}
			}

			notFound++;
			return FileItemType.MISSING;
		}


		internal string getName()
		{
			if (isMissing) { return ""; }

			return Path.GetFileNameWithoutExtension(path);
		}

		internal string getDirectory()
		{
			if (isMissing) { return ""; }

			return Path.GetDirectoryName(path) ?? "\\";
		}

		internal string getOutlineDirectory()
		{
			if (isMissing) { return ""; }

			return Path.GetDirectoryName(outlinePath) ?? "\\";
		}

		internal string getFullPath
		{
			get
			{
				if (isMissing) { return ""; }

				if (isFullPath) { return path; }

				return Path.GetFullPath(RootPath + "\\" + path);
			}
		}

	}
}
