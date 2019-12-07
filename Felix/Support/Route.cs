#region + Using Directives
using System;
using System.IO;
using SysPath = System.IO.Path;
#endregion

// itemname: Route
// username: jeffs
// created:  11/2/2019 5:18:09 PM

namespace Felix.Support
{
	public enum RouteType
	{
		UNDEFINED,
		OTHER,
		FILE,
		DIRECTORY
	}

		public class Route : IEquatable<Route>, IComparable<Route>
	{

	#region static properties

		public static Route Invalid => new Route(null);

	#endregion

	#region ctor

		public Route(string initialRoute)
		{
			IsValid = false;

			FullPath = Validate(initialRoute);

			if (FullPath != null)
			{
				IsValid = true;
				FullPath = initialRoute;
			}
		}

	#endregion

	#region  public status properties

		public bool IsValid { get; private set; }
		public bool IsUnc
		{
			get
			{
				if (IsValid)
				{
					return FullPath.StartsWith(@"\\");
				}

				return false;
			}
		}
		public bool IsRooted => !(RootPath.Equals(@"\"));
		public bool HasFileName => !string.IsNullOrWhiteSpace(FileName);

	#endregion

	#region public properties

		public string FullPath { get; private set; }

		public string RootPath
		{
			get
			{
				if (IsValid)
				{
//					FileInfo fi = new FileInfo(FullPath);
//					DirectoryInfo di = new DirectoryInfo(FullPath);
//					string s1 = SysPath.GetFullPath(FullPath);
//					string s2 = SysPath.GetPathRoot(FullPath);
//					bool b1= SysPath.IsPathRooted(FullPath);
//					
//

					return SysPath.GetPathRoot(FullPath);
				}

				return null;
			}
		}

		public string Root
		{
			get
			{
				if (!IsValid
					|| !IsRooted 
					|| RootPath == null) return null;

				if (!IsUnc && RootPath.EndsWith(@"\"))
				{
					return RootPath.Substring(0, RootPath.Length - 1);
				}

				return RootPath;
			}
		}

		public string VolumeName
		{
			get
			{
				if (IsValid)
				{
					if (IsUnc)
					{
						int pos = FullPath.IndexOf('\\', 2);
						if (pos < 2) return null;
						return FullPath.Substring(0, pos + 1);
					}
					else
					{
						int pos = FullPath.IndexOf(':');
						if (pos != 1) return null;
						return FullPath.Substring(0, 2);
					}
				}

				return null;
			}
		}

		public string FileName
		{
			get
			{
				if (IsValid)
				{
					if (SysPath.HasExtension(FullPath))
						return SysPath.GetFileName(FullPath);
				}

				return null;
			}
		}

		public string FileWithoutExtension
		{
			get
			{
				if (IsValid)
				{
					if (SysPath.HasExtension(FullPath))
						return SysPath.GetFileNameWithoutExtension(FullPath);
				}
				return null;
			}
		}

		public string FileExtension
		{
			get
			{
				if (IsValid)
				{
					if (SysPath.HasExtension(FullPath))
						return SysPath.GetExtension(FullPath);
				}

				return null;
			}
		}

		public string Path
		{
			get
			{
				if (!IsValid) return null;

				int file;

				if (FileName != null)
				{
					file = FileName.Length + 1;
				}
				else
				{
					file = 0;
				}

				int len = FullPath.Length - file;

				return FullPath.Substring(0, len);
			}
		}

		public string Folders
		{
			get
			{
				if (!IsValid) return null;

				int root = (RootPath ?? "").Length;
				int file;

				if (FileName != null)
				{
					file = FileName.Length + 1;
				}
				else
				{
					file = 0;
				}

				if (!IsUnc)
				{
					root -= 1;
				}

				int len = FullPath.Length - root - file;

				if (len <= 0) return "";

				return FullPath.Substring(root, len);
			}
		}

		public string[] FolderNames => FolderNameList(Folders);

		public int FolderCount
		{
			get
			{
				string[] names = FolderNames;
				if (names == null) return -1;
				return names.Length;
			}
		}

		public int Depth
		{
			get
			{
				if (!IsValid || Folders == null) return -1;

				return DividePath(Folders)?.Length + 1 ?? 0;
			}
		}

		public int Length => FullPath.Length;

		public RouteType RouteType
		{
			get
			{
				if (!IsValid) return RouteType.UNDEFINED;

				if (File.Exists(FullPath)) return RouteType.FILE;

				if (Directory.Exists(FullPath)) return RouteType.DIRECTORY;

				return RouteType.OTHER;
			}
		}

	#endregion

	#region public methods

		public bool Prepend(string prependPath){
			if (!IsValid || IsRooted)
			{
				return false;
			}
			   
			FullPath = prependPath + FullPath;
			return true;
		}

		public string Validate(string testRoute)
		{
			if (string.IsNullOrWhiteSpace(testRoute))
			{
				return null;
			}

			string result;

			try
			{
				result = testRoute.Replace('/', '\\').Trim();
			}
			catch
			{
				return null;
			}

			return result;
		}

		public string GetFolderName(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return null;

			string answer = path.Trim();

			if (answer.StartsWith(@"\\"))
			{
				return answer.Substring(2);
			}

			if (answer.StartsWith(@"\"))
			{
				return answer.Substring(1);
			}

			return path;
		}

		public string AssemblePath(int index)
		{
			string[] folders = DividePath(Folders);

			if (folders == null || index == 0 ||
				Math.Abs(index) >= folders.Length) return null;

			string answer = null;

			if (index > 0)
			{
				for (int i = 0; i < index; i++)
				{
					answer = answer + folders[i];
				}
			}
			else
			{
				int end = folders.Length + index + 1;
				for (int i = 0; i < end; i++)
				{
					answer = answer + folders[i];
				}
			}

			return Root + answer;
		}

		public string[] FolderNameList(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return null;

			return path.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);
		}

		public string[] DividePath(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return null;

			string[] answer = FolderNameList(path);

			for (int i = 0; i < answer.Length; i++)
			{
				answer[i] = @"\" + answer[i];
			}

			return answer;

		}

		public Route Clone()
		{
			return new Route(FullPath);
		}

		// provide a Route that is below the root path
		public Route SubPath(Route rootPath)
		{
			if (!IsValid || !rootPath.IsValid ||
				!IsRooted || !rootPath.IsRooted) return Route.Invalid;

			int subtractorLength = rootPath.Length;

			if (Length < subtractorLength) return Route.Invalid;

			bool compare =
				FullPath.Substring(0, subtractorLength).ToUpper()
				.Equals(rootPath.FullPath.ToUpper());

			if (!compare) return Route.Invalid;

			return new Route(FullPath.Substring(subtractorLength));
		}

	#endregion

	#region indexer

		public string this[int index]
		{
			get
			{
				if (!IsValid) return null;

				if (index < 0) return GetSubFolderInverse(index);

				if (index == 0) return RootPath;

				return GetSubFolder(index);
			}
		}

	#endregion

	#region private methods

		private string GetSubFolder(int index)
		{
			if (!IsValid) return null;

			index -= 1;

			if (index < 0) return null;

			// start with folders
			string[] f = DividePath(Folders);

			if (f == null || index >= f.Length ) return null;

			return f[index];
		}

		private string GetSubFolderInverse(int index)
		{
			if (!IsValid) return null;

			if (index >= 0) return null;

			index = (index * -1) - 1;

			// start with folders
			string[] f = DividePath(Folders);

			if (f == null || f.Length-1 < index) return null;

			return f[f.Length-1 - index];
		}

	#endregion

	#region system methods

		public bool Equals(Route other)
		{
			return this.FullPath.ToUpper().Equals(other.FullPath.ToUpper());
		}

		public int CompareTo(Route other)
		{
			return FullPath.CompareTo(other.FullPath);
		}

		public override string ToString()
		{
			return FullPath;
		}

	#endregion

	}
}



//
// Levels         3
//
// Paths(2)       v------------------v
// Paths(1)       v--------------v   |
// Paths(0)       v----------v   |   |
		// [0]    v----------v   |   |
		// [1]    |          |v--v   |
		// [2]    |          ||  |v--v
// FullPath       \\cs-004\dir\dir\dir\file.ext
		// [-1]   |      |   ||  |^--^ |  | | |
		// [-2]   |      |   |^--^   | |  | | |
// VolumeName     ^------^   ||      | |  | | |
// RootPath       ^----------^|      | |  | | | 
// Folders        |           ^------^ |  | | |
// Path           ^------------------^ |  | | |
// FileWithoutExtension                ^--^ | |
// FileExtension                       |    ^-^
// FileName                            ^------^
// FullPath       ^---------------------------^

// example
// levels = 4
//   full route| \\CS-004\Documents\Files\021 - Household\MicroStation\0047116612.PDF
//   [+0]      | \\CS-004\Documents  (CS-004\Documents)
//   [+1]      | \Files  (Files)
//   [+2]      | \021 - Household  (021 - Household)
//   [+3]      | \MicroStation  (MicroStation)
//   [-1]      | \MicroStation  (MicroStation)
//   [-2]      | \021 - Household  (021 - Household)
//   [-3]      | \Files  (Files)
 
// assemble path() [+2] \\CS-004\Documents\Files\021 - Household
// assemble path() [-1] \\CS-004\Documents\Files

// FolderNameList() & FolderNames[]
// [0] = Documents  |  [1] = Files
// [2] = 021 - Household  |  [3] = MicroStation



//
// Levels         4
//
// Paths(3)       v------------v
// Paths(2)       v---------v  |
// Paths(1)       v----v   |   |
// Paths(0)       vv   |   |   |
		// [0]    vv   |   |   |
		// [1]    ||v--v   |   |
		// [2]    |||  |v--v   |
		// [3]    |||  ||  |v--v
// FullPath       c:\dir\dir\dir\file.ext
		// [-1]   |||  ||  |^--^ |  | | |
		// [-2]   |||  |^--^   | |  | | |
		// [-3]   ||^--^       | |  | | |
// VolumeName     ^^|          | |  | | |
// RootPath       ^-^          | |  | | | 
// Folders        | ^----------^ |  | | |
// Path           ^------------^ |  | | |
// FileWithoutExtension          ^--^ | |
// FileExtension                 |    ^-^
// FileName                      ^------^
// FullPath       ^---------------------^

// example
// levels 4
//   full route| C:\Documents\Files\021 - Household\MicroStation\0047116612.PDF
//   [+0]      | C:\  (C:\)
//   [+1]      | \Documents  (Documents)
//   [+2]      | \Files  (Files)
//   [+3]      | \021 - Household  (021 - Household)
//   [+4]      | \MicroStation  (MicroStation)
//   [-1]      | \MicroStation  (MicroStation)
//   [-2]      | \021 - Household  (021 - Household)
//   [-3]      | \Files  (Files)
//   [-4]      | \Documents  (Documents)

// assemble path() [+2] C:\Documents\Files
// assemble path() [-2] C:\Documents\Files\021 - Household

// FolderNameList() & FolderNames
// [0] = Documents  |  [1] = Files
// [2] = 021 - Household  |  [3] = MicroStation
