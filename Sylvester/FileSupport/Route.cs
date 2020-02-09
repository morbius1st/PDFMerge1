#region + Using Directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using SysPath = System.IO.Path;

#endregion

// itemname: Route
// username: jeffs
// created:  11/2/2019 5:18:09 PM

namespace UtilityLibrary
{
	public enum RouteType
	{
		UNDEFINED,
		OTHER,
		FILE,
		DIRECTORY
	}

	[DataContract]
	public class Route : IEquatable<Route>, IComparable<Route>
	{
	#region private fields

		private string fullPath;

		private static Dictionary<string, string> UncNameMap = new Dictionary<string, string>(10);

	#endregion

	#region static properties

		public static Route Invalid => new Route();

	#endregion

	#region ctor

		public Route(string initialPath)
		{
			ConfigureRoute(initialPath);
		}

		public Route()
		{
			ConfigureRoute(null);
		}

		public Route(string[] path)
		{
			StringBuilder sb = new StringBuilder(path[0]);

			for (int i = 1; i < path.Length; i++)
			{
				sb.Append(@"\").Append(path[i]);
			}

			ConfigureRoute(sb.ToString());
		}

		private void ConfigureRoute(string initialPath)
		{
			getUncNameMap();

			IsValid = false;

			fullPath = Validate(initialPath);

			if (fullPath != null)
			{
				IsValid = true;
				fullPath = initialPath;
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

		public bool IsRooted => !(RootPath?.Equals(@"\") ?? false);
		public bool HasFileName => !string.IsNullOrWhiteSpace(FileName);

	#endregion

	#region public properties

		[DataMember]
		public string FullPath
		{
			get => fullPath;
			set => ConfigureRoute(value);
		}

		public string RootPath
		{
			get
			{
				string result = null;

				if (IsValid)
				{
//					return SysPath.GetPathRoot(FullPath);

					result = findUncFromUncPath(fullPath);

					if (string.IsNullOrWhiteSpace(result))
					{
						result =  DriveVolumeFromPath(fullPath);
					}

					if (!string.IsNullOrWhiteSpace(result))
					{
						result += @"\";
					}
				}

				return result;
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
						return SysPath.GetExtension(FullPath).ToLower();
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

				if (root < 0) root = 0;

				int len = FullPath.Length - root - file;

				if (len <= 0) return "";

				return FullPath.Substring(root, len);
			}
		}

		[IgnoreDataMember]
		public string[] FullPathNames
		{
			get
			{
				string[] folderNames = FolderNames;

				string[] fullPathNames = new string[folderNames.Length + 1];

				fullPathNames[0] = VolumeName;

				for (int i = 0; i < folderNames.Length; i++)
				{
					fullPathNames[i + 1] = folderNames[i];
				}

				return fullPathNames;
			}
		}

		[IgnoreDataMember]
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

		public bool Prepend(string prependPath)
		{
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

	#region static methods

		// place holder - needs to be integrated
		public static string UncVolumeFromPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path) ||
				path.Length < 2
				) return null;

			if (!path.StartsWith(@"\\"))
			{
				StringBuilder sb = new StringBuilder(1024);
				int size = sb.Capacity;

				// still may fail but has a better chance;
				int error = WNetGetConnection(path.Substring(0, 2), sb, ref size);

				if (error != 0) return null;

				return sb.ToString();
			}

			return findUncFromUncPath(path);
		}

		// place holder - needs to be integrated
		public static string DriveVolumeFromPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return null;

			string drive = findDriveFromUnc(path);

			if (!string.IsNullOrWhiteSpace(drive)) return drive;


			if (!path.StartsWith(@"\\"))
			{
				// does not start with "\\" if character 2 is ':' 
				// assume provided with a drive and return that portion
				if (path.Substring(1, 1).Equals(":")) return path.Substring(0, 2);
			}

			return null;
		}

	#endregion

	#region indexer

		public string this[int index]
		{
			get
			{
				if (!IsValid) return null;

				if (index < 0) return getSubFolderInverse(index);

				if (index == 0) return RootPath;

				return getSubFolder(index);
			}
		}

	#endregion

	#region private methods

		private string[] FolderNameList(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return null;

			return path.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);
		}

		private string getSubFolder(int index)
		{
			if (!IsValid) return null;

			index -= 1;

			if (index < 0) return null;

			// start with folders
			string[] f = DividePath(Folders);

			if (f == null || index >= f.Length ) return null;

			return f[index];
		}

		private string getSubFolderInverse(int index)
		{
			if (!IsValid) return null;

			if (index >= 0) return null;

			index = (index * -1) - 1;

			// start with folders
			string[] f = DividePath(Folders);

			if (f == null || f.Length < index) return null;

			string result;

			if (f.Length == index)
			{
				result = RootPath;
			}
			else
			{
				result = f[f.Length - 1 - index];
			}

			return  result;
		}

		private static void getUncNameMap()
		{
			DriveInfo[] drives = DriveInfo.GetDrives();

			foreach (DriveInfo di in drives)
			{
				if (di.IsReady)
				{
					string driveletter = di.Name.Substring(0, 2);

					string unc = UncVolumeFromPath(driveletter);

					if (!string.IsNullOrWhiteSpace(unc))
					{
						if (!UncNameMap.ContainsKey(driveletter))
						{
							UncNameMap.Add(driveletter, unc);
						}
					}
				}
			}
		}

		private static string findDriveFromUnc(string path)
		{
			if (string.IsNullOrWhiteSpace(path) ||
				!path.StartsWith(@"\\") ||
				path.Length < 3) return null;

			if (UncNameMap == null || UncNameMap.Count == 0) getUncNameMap();

			foreach (KeyValuePair<string, string> kvp in UncNameMap)
			{
				int len = kvp.Value.Length;

				if (path.Length < len) continue;

				if (kvp.Value.ToLower().Equals(path.Substring(0, len).ToLower())) return kvp.Key;
			}

			return null;
		}

		private static string findUncFromUncPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path)
				|| !path.StartsWith(@"\\")
				) return null;

			if (UncNameMap == null || UncNameMap.Count == 0) getUncNameMap();

			foreach (KeyValuePair<string, string> kvp in UncNameMap)
			{
				int len = kvp.Value.Length;

				if (path.Length < len) continue;

				if (kvp.Value.ToLower().Equals(path.Substring(0, len).ToLower())) return kvp.Value;
			}

			return null;
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

	#region system Dll calls

		[DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int WNetGetConnection(
			[MarshalAs(UnmanagedType.LPTStr)] string localName,
			[MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName,
			ref int length);

	#endregion
	}
}

//
// Depth          3
//
// Paths(2)       v------------------v
// Paths(1)       v--------------v   |
// Paths(0)       v----------v   |   |
// [0]            v----------v   |   |
// [1]            |          |v--v   |
// [2]            |          ||  |v--v
// FullPath       \\cs-004\dir\dir\dir\file.ext
// [-1]           |      |   ||  |^--^ |  | | |
// [-2]           |      |   |^--^   | |  | | |
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
// Levels??         4
// Depth??
//
// Paths(3)       v------------v
// Paths(2)       v---------v  |
// Paths(1)       v----v   |   |
// Paths(0)       vv   |   |   |
// [0]            vv   |   |   |
// [1]            ||v--v   |   |
// [2]            |||  |v--v   |
// [3]            |||  ||  |v--v
// FullPath       c:\dir\dir\dir\file.ext
// [-1]           |||  ||  |^--^ |  | | |
// [-2]           |||  |^--^   | |  | | |
// [-3]           ||^--^       | |  | | |
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
//   full route| P:\2099-999 Sample Project\Publish\9999 Current\A  A2.1-0  - DO NOT REMOVE.pdf
//               []                               GetFolderName([])
//   [+0] [-5] | P:\                              P:\
//   [+1] [-4] | \2099-999 Sample Project         2099-999 Sample Project
//   [+2] [-3] | \Publish                         Publish
//   [+3] [-2] | \9999 Current                    9999 Current
//   [+4] [-1] 

// assemble path(+2)   P:\2099-999 Sample Project\Publish
// assemble path(-1)   P:\2099-999 Sample Project\Publish\9999 Current

/*
foldernames() as []
[0]       | 2015-491 Centercal - Long Beach
[1]       | CD
[2]       | 00 Primary
[3]       | New folder

GetFolderName(r[i])
( 0)      | P:\
( 1)      | 2015-491 Centercal - Long Beach
( 2)      | CD
( 3)      | 00 Primary
( 4)      | New folder
(-1)      | New folder
(-2)      | 00 Primary
(-3)      | CD
(-4)      | 2015-491 Centercal - Long Beach
(-5)      | P:\

 */

