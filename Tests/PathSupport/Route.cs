#region + Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using SysPath = System.IO.Path;

#endregion

// itemname: Route
// username: jeffs
// created:  11/2/2019 5:18:09 PM

/*
1.  Change name to PathEx?
2.  Make properties use Get...
3.  Add a const for path separator.
4.  Follow Path class for properties
5.  Validate path characters
6.  Add GetCurrentDirectory
7.  getpath = all of the parts as an array
8.  make volume be the Unc volume and change to UncVloume, add Uncpath be the unc volume + its path
9.  change root to RootVolume
10. add switch to provide as unc when possible?


 */


namespace UtilityLibrary
{
//	public enum RouteType
//	{
//		UNDEFINED,
//		OTHER,
//		FILE,
//		DIRECTORY
//	}

	[DataContract]
	public class Route : IEquatable<Route>, IComparable<Route>
	{
	#region private fields

		private PathWay pathway;

		private bool useUnc = false;

	#endregion

	#region static properties

		public static Route Invalid => new Route();

		public static Route CurrentDirectory => new Route(Environment.CurrentDirectory);

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
			PathWay.getUncNameMap();

			pathway = new PathWay();

			IsValid = pathway.parse(initialPath);
		}

	#endregion

	#region  public status properties

		public bool IsValid { get; private set; }
		public bool HasQualifiedPath => pathway.HasQualifiedPath;
		public bool HasUnc => pathway.HasUnc;
		public bool HasDrive => pathway.HasDrive;
		public bool HasFileName => !pathway.FileName.IsVoid();
		public bool IsFolderPath => pathway.IsFolderPath;
		public bool IsFilePath => pathway.IsFilePath;
		public bool IsFound => pathway.IsFound;

	#endregion

	#region public properties

		public PathWay PathWay => pathway;

		[DataMember]
		public string FullPath
		{
			get => pathway.PathWayString;
			set => pathway.PathWayString = value;
		}

		/// <summary>
		/// the drive volume without a slash suffix
		/// </summary>
		public string DriveVolume => pathway.DriveVolume;

		/// <summary>
		/// the drive volume with a slash suffix
		/// </summary>
		public string DrivePath => pathway.DriveVolume + @"\";

		public string UncVolume => pathway.UncVolume;

		public string UncPath => pathway.UncPath;

		public string UncShare => pathway.UncVolume + pathway.UncPath;

		public string FileNameAndExtension
		{
			get
			{
				if (pathway.FileName.IsVoid() &&
					pathway.FileExt.IsVoid()) return null;

				if (pathway.FileExt.IsVoid()) return pathway.FileName;

				return pathway.FileName + "." + pathway.FileExt;
			}
		}

		public string FileName => pathway.FileName;

		public string FileExtension => pathway.FileExt;

		/// <summary>
		/// the path without the filename and extension
		/// </summary>
		public string Path
		{
			get
			{
				if (!IsValid) return null;

//				int file = 0;
//
//				if (FileNameAndExtension != null)
//				{
//					file = FileNameAndExtension.Length + 1;
//				}
//
//				int len = FullPath.Length - file;
//
//				return FullPath.Substring(0, len);

				return AssemblePath(Depth);

			}
		}

		public string Folders
		{
			get
			{
				if (!IsValid) return null;

				return AssembleFolders();

//				int root = (DrivePath ?? "").Length;
//				int file;
//
//				if (FileNameAndExtension != null)
//				{
//					file = FileNameAndExtension.Length + 1;
//				}
//				else
//				{
//					file = 0;
//				}
//
////				if (!IsUnc)
////				{
////					root -= 1;
////				}
//
//				if (root < 0) root = 0;
//
//				int len = FullPath.Length - root - file;
//
//				if (len <= 0) return "";
//
//				return FullPath.Substring(root, len);
			}
		}

//		[IgnoreDataMember]
//		public string[] FullPathNames
//		{
//			get
//			{
//				string[] folderNames = FolderNames;
//
//				string[] fullPathNames = new string[folderNames.Length + 1];
//
//				fullPathNames[0] = UncVolume;
//
//				for (int i = 0; i < folderNames.Length; i++)
//				{
//					fullPathNames[i + 1] = folderNames[i];
//				}
//
//				return fullPathNames;
//			}
//		}

//		[IgnoreDataMember]
//		public string[] FolderNames => FolderNameList(Folders);

		[IgnoreDataMember]
		public string[] FolderNames => pathway.Folders.ToArray();

		public int FolderCount => pathway.Folders.Count;

//		public int FolderCount
//		{
//			get
//			{
//				string[] names = FolderNames;
//				if (names == null) return -1;
//				return names.Length;
//			}
//		}

		/// <summary>
		/// the number of folders deep counting the volume as a depth
		/// </summary>
		public int Depth
		{
			get
			{
				int d = HasDrive ? 1 : 0;

				d += pathway.Folders.Count;

				return d;
			}
		}

		public int Length => FullPath.Length;

//		public RouteType RouteType
//		{
//			get
//			{
//				if (!IsValid) return RouteType.UNDEFINED;
//
//				if (File.Exists(FullPath)) return RouteType.FILE;
//
//				if (Directory.Exists(FullPath)) return RouteType.DIRECTORY;
//
//				return RouteType.OTHER;
//			}
//		}

	#endregion

	#region public methods

		/// <summary>
		/// return the folder name (or root path) based on the
		/// index allowing negative numbers to index from
		/// the end rather than from the front
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string GetFolder(int index)
		{
			if (!IsValid) return null;

			string answer = this[index];

			if (index == 0) return answer;

			return answer == null ? null : @"\" + answer;
		}


//		public string GetFolder(string path)
//		{
//			if (string.IsNullOrWhiteSpace(path)) return null;
//
//			string answer = path.Trim();
//
//			if (answer.StartsWith(@"\\"))
//			{
//				return answer.Substring(2);
//			}
//
//			if (answer.StartsWith(@"\"))
//			{
//				return answer.Substring(1);
//			}
//
//			return path;
//		}

		public string AssemblePath(int index)
		{
//			if (index >= Depth ||
//				index < (-1 * Depth)
//				) return null;
//
//			int idx = index >= 0 ? index : Depth + index;

			if (!IsValid) return null;

			int idx = CalcIndex(index);

			if (idx < 0) return null;

			if (idx == 0) return DrivePath;

			StringBuilder sb = new StringBuilder(DriveVolume);

			for (int i = 0; i < idx - 1; i++)
			{
				sb.Append(@"\").Append(pathway.Folders[i]);
			}

			return sb.ToString();
		}

		public string AssembleFolders(int index = 0)
		{
//			if (index >= Depth ||
//				index < (-1 * Depth) ||
//				pathway.Folders.Count == 0
//				) return null;
//
//			int idx = index >= 0 ? index : Depth + index;

			if (!IsValid) return null;

			int idx = CalcIndex(index);

			if (idx < 0) return null;

			// do not return just the root
			if (idx == 0) idx = Depth;

			StringBuilder sb = new StringBuilder();

			for (int i = 1; i < idx - 1; i++)
			{
				sb.Append(@"\").Append(pathway.Folders[i-1]);
			}

			return sb.ToString();
		}



//		public string AssemblePath2(int index)
//		{
//			string[] folders = DividePath(Folders);
//
//			if (folders == null || index == 0 ||
//				Math.Abs(index) >= folders.Length) return null;
//
//			string answer = null;
//
//			if (index > 0)
//			{
//				for (int i = 0; i < index; i++)
//				{
//					answer = answer + folders[i];
//				}
//			}
//			else
//			{
//				int end = folders.Length + index + 1;
//				for (int i = 0; i < end; i++)
//				{
//					answer = answer + folders[i];
//				}
//			}
//
//			return DriveVolume + answer;
//		}

//		public string[] DividePath(string path)
//		{
//			if (string.IsNullOrWhiteSpace(path)) return null;
//
//			string[] answer = FolderNameList(path);
//
//			for (int i = 0; i < answer.Length; i++)
//			{
//				answer[i] = @"\" + answer[i];
//			}
//
//			return answer;
//		}

		public Route Clone()
		{
			return new Route(FullPath);
		}

//		// provide a Route that is below the root path
//		public Route SubPath(Route rootPath)
//		{
//			if (!IsValid || !rootPath.IsValid ||
//				!HasQualifiedPath || !rootPath.HasQualifiedPath) return Route.Invalid;
//
//			int subtractorLength = rootPath.Length;
//
//			if (Length < subtractorLength) return Route.Invalid;
//
//			bool compare =
//				FullPath.Substring(0, subtractorLength).ToUpper()
//				.Equals(rootPath.FullPath.ToUpper());
//
//			if (!compare) return Route.Invalid;
//
//			return new Route(FullPath.Substring(subtractorLength));
//		}

	#endregion


	#region indexer

		public string this[int index]
		{
			get
			{
				if (!IsValid) return null;

				int idx = CalcIndex(index);

				if (idx < 0) return null;

				if (idx == 0) return DrivePath;

				return pathway.Folders[idx - 1];
			}
		}

	#endregion

	#region private methods

//		private string[] FolderNameList(string path)
//		{
//			if (string.IsNullOrWhiteSpace(path)) return null;
//
//			return path.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);
//		}

		private int CalcIndex(int index)
		{
			if (index < 0) index = Depth + index;

			if (index > Depth || index < 0) return -1;

			return index;
		}
//
//		private string GetSubFolder(int index)
//		{
//			if (!IsValid) return null;
//
//			index -= 1;
//
//			if (index < 0) return null;
//
//
//		}
//
//
//
//		private string getSubFolder(int index)
//		{
//			if (!IsValid) return null;
//
//			index -= 1;
//
//			if (index < 0) return null;
//
//			// start with folders
//			string[] f = DividePath(Folders);
//
//			if (f == null || index >= f.Length ) return null;
//
//			return f[index];
//		}
//
//		private string getSubFolderInverse(int index)
//		{
//			if (!IsValid) return null;
//
//			if (index >= 0) return null;
//
//			index = (index * -1) - 1;
//
//			// start with folders
//			string[] f = DividePath(Folders);
//
//			if (f == null || f.Length < index) return null;
//
//			string result;
//
//			if (f.Length == index)
//			{
//				result = DrivePath;
//			}
//			else
//			{
//				result = f[f.Length - 1 - index];
//			}
//
//			return  result;
//		}

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

//
//
//		public enum PathStatus
//		{
//			FALSE = -1,
//			UNDETERMINED = 0,
//			TRUE = 1
//		}
//
//		public static string xUncVolume; // '//' + computer name
//
//		public static string drvVolume; // drive letter + colon
//
//		public static string uncPath; // all except for the uncVolume
//
//		public static List<string> folders; // all of the folders along the path
//
//		public static string fileName;
//		public static string fileExt;
//
//		public static string remain;
//
//		public static PathStatus HasQualifiedPath = PathStatus.FALSE;
//		public static PathStatus IsFolderPath = PathStatus.FALSE;
//		public static PathStatus IsFilePath = PathStatus.FALSE;
//		public static PathStatus IsFound = PathStatus.FALSE;
//		public static PathStatus HasDrive = PathStatus.FALSE;
//		public static PathStatus HasUnc = PathStatus.FALSE;
//
//		private static void reset()
//		{
//			xUncVolume = null;
//			drvVolume = null;
//			uncPath = null;
//			fileName = null;
//			fileExt = null;
//			remain = null;
//		}
//
//
//		public static bool parse(string way)
//		{
//			if (way.IsVoid()) return false;
//
//			reset();
//			folders = new List<string>();
//
//			string path = CleanPath(way);
//
//			remain = searateDriveVolume(path);
//
//			remain = separateFileAndExtension(remain);
//
//			folders =
//				separateFolders(remain);
//
//			setStatus();
//
//			return true;
//		}
//
//		private static List<string> separateFolders(string folders)
//		{
//			if (folders.IsVoid()) return new List<string>();
//
//			return new List<string>(folders.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries));
//		}
//
//		private static string separateFileAndExtension(string foldersAndFile)
//		{
//			if (foldersAndFile.IsVoid()) return null;
//
//			int posPeriod = foldersAndFile.LastIndexOf('.');
//			int posEndSeparator = foldersAndFile.LastIndexOf('\\');
//			int result = posPeriod - posEndSeparator;
//
//			if (posPeriod == 0 || result < 0)
//			{
//				// outcome C
//				return foldersAndFile;
//			}
//
//			int len = foldersAndFile.Length;
//
//
//			// posible outcomes
//			// A "normal", posPeriod - posEndSeparator > 1
//			//     1234567890123
//			//     0123456789
//			//  ie path\file.ext  == 9 - 4 = 5 (9 - 4 - 1 = 4) / (13 - 9 - 1 = 3)
//			// B "normal2", posPeriod - posEndSeparator = 1
//			//     1234567890
//			//     0123456789
//			//  ie path\.file  == 5 - 4 = 1 (10 - 5 = 5)
//			// C "normal3", posPeriod == 0
//			//     0123456789
//			//  ie path\path  == if posPeriod == 0 -> 0
//			// D "abby normal", posPeriod - posEndSeparator < 0
//			//     0123456789
//			//  ie path.\path  == 4 - 5 = -1
//			//  ie pa.th\path  == 2 - 5 = -3
//
//
//			if (result == 1)
//			{
//				// outcome B
//				fileName = foldersAndFile.Substring(posEndSeparator + 1, len - posPeriod);
//			}
//			else
//			{
//				// outcome A
//				fileName = foldersAndFile.Substring(posEndSeparator + 1, posPeriod - posEndSeparator - 1);
//				fileExt = foldersAndFile.Substring(posPeriod + 1, len - posPeriod - 1);
//			}
//
//			return foldersAndFile.Substring(0, posEndSeparator);
//		}
//
//		private static string searateDriveVolume(string path)
//		{
//			if (path.IsVoid()) return null;
//
//			uncPath = PathWay.UncVolumeFromPath(path);
//			drvVolume = PathWay.DriveVolumeFromPath(path);
//
//			// 5 possible outcomes
//			// A  got drive and unc
//			// B  got drive and no unc
//			// C  got no drive and no unc
//			//  		and path starts with '\\'
//			// D  got no drive and no unc
//			//  		and path starts with '\'
//			// E  got no drive and no unc
//			//  		and path starts with anything else
//
//			if (!drvVolume.IsVoid())
//			{
//				return separateRemainderFromVolume(path);
//			}
//
//			string result = null;
//
//			if (path.StartsWith(@"\\"))
//			{
//				return extractUncVolume(path);
//			}
//
//			return separateRemainder(path);
//		}
//
//		private static string separateRemainder(string path)
//		{
//			if (path[0] == '\\' &&
//				path.Length == 1) return null;
//
//			return path;
//		}
//
//		private static string separateRemainderFromVolume(string path)
//		{
//			// maybe A or B
//			int pos = 2;
//
//			separateUncVolume();
//
//			if (path.StartsWith(@"\\") && !uncPath.IsVoid())
//			{
//				pos = xUncVolume.Length + uncPath.Length;
//			}
//
//			if (path.Length == pos) return null;
//
//			return path.Substring(pos);
//		}
//
//		private static void separateUncVolume()
//		{
//			if (uncPath.IsVoid()) return;
//
//			int pos = uncPath.IndexOf('\\', 2);
//
//			if (pos == -1)
//			{
//				xUncVolume = uncPath;
//				uncPath = null;
//				return;
//			}
//
//			if (pos > 2)
//			{
//				xUncVolume = uncPath.Substring(0, pos);
//				uncPath = uncPath.Substring(pos);
//				return;
//			}
//
//			xUncVolume = null;
//			uncPath = null;
//		}
//
//		private static string extractUncVolume(string path)
//		{
//			if (path.Length == 2) return null;
//
//			int pos = path.IndexOf('\\', 2);
//
//			if (pos == -1)
//			{
//				xUncVolume = path;
//				return null;
//			}
//			else if (pos > 2)
//			{
//				xUncVolume = path.Substring(0, pos);
//				return path.Substring(pos);
//			}
//
//			return null;
//		}
//
//		private static void setStatus()
//		{
//			HasQualifiedPath = PathStatus.FALSE;
//			IsFolderPath = PathStatus.FALSE;
//			IsFilePath = PathStatus.FALSE;
//			IsFound = PathStatus.FALSE;
//			HasDrive = PathStatus.FALSE;
//			HasUnc = PathStatus.FALSE;
//
//			if (!drvVolume.IsVoid()) HasDrive = PathStatus.TRUE;
//			if (!xUncVolume.IsVoid()) HasUnc = PathStatus.TRUE;
//			if ((folders?.Count ?? 0) > 0) IsFolderPath = PathStatus.TRUE;
//			if (!fileName.IsVoid()) IsFilePath = PathStatus.TRUE;
//
//			if (HasDrive == PathStatus.TRUE && IsFolderPath == PathStatus.TRUE || IsFilePath == PathStatus.TRUE)
//				HasQualifiedPath = PathStatus.TRUE;
//		}
//
//
//		protected class xPathWay
//		{
//			public static Dictionary<string, string> UncNameMap { get; private set; }  =
//				new Dictionary<string, string>(10);
//
//			private string uncVolume; // '//' + computer name
//
//			private string drvVolume; // drive letter + colon
//
//			private string uncPath; // all except for the uncVolume
//
//			private List<string> folders; // all of the folders along the path
//
//			private string fileName;
//			private string extension;
//
//			public xPathWay(string way)
//			{
//				parse(way);
//			}
//
//		#region public properties
//
//			public string UncVolume
//			{
//				get => uncVolume;
//				set => uncVolume = value;
//			}
//
//			public string DriveVolume
//			{
//				get => drvVolume;
//				set => drvVolume = value;
//			}
//
//			public string UncPath
//			{
//				get => uncPath;
//				set => uncPath = value;
//			}
//
//			public List<string> Folders
//			{
//				get => folders;
//				set => folders = value;
//			}
//
//			public string FileName1
//			{
//				get => fileName;
//				set => fileName = value;
//			}
//
//			public string Extension
//			{
//				get => extension;
//				set => extension = value;
//			}
//
//		#endregion
//
//		#region public methods
//
//			public string CleanPath(string path)
//			{
//				if (string.IsNullOrWhiteSpace(path))
//				{
//					return null;
//				}
//
//				string result;
//
//				try
//				{
//					result = path.Replace('/', '\\').Trim();
//
//					if (result[1] == ':')
//					{
//						result = result.Substring(0, 1).ToUpper() + result.Substring(1);
//					}
//				}
//				catch
//				{
//					return null;
//				}
//
//				return result;
//			}
//
//		#endregion
//
//		#region public static methods
//
//		#endregion
//
//		#region private methods
//
//			private void parse(string way)
//			{
//				string path = CleanPath(way);
//			}
//
//			private string searateDriveVolume(string path)
//			{
//				UncPath = PathWay.UncVolumeFromPath(path);
//				DriveVolume = PathWay.DriveVolumeFromPath(path);
//
//				string result;
//
//				if (path.StartsWith(@"\\")) { }
//				else if (path[0] >= 'A' && path[0] <= 'Z' && path[1] == ':')
//				{
//					result = PathWay.findDriveFromUncPath(path);
//				}
//				else { }
//
//				return null;
//			}
//
//		#endregion
//		}
	}


	public class PathWay
	{
	#region public fields

		public static Dictionary<string, string> UncNameMap { get; private set; }  =
			new Dictionary<string, string>(10);

	#endregion

	#region private fields

		private string pathway; // original - un-modified;

		private string uncVolume; // '//' + computer name
		private string uncPath;   // all except for the uncVolume
		// also uncShare = uncVolume +  uncPath

		private string driveVolume; // drive letter + colon

		private List<string> folders; // all of the folders along the path

		private string fileName;
		private string fileExt;

	#endregion

	#region public properties

		public string PathWayString
		{
			get => pathway;
			set => parse(value);
		}

		public string UncVolume
		{
			get => uncVolume;
			private set => uncVolume = value;
		}

		public string UncPath
		{
			get => uncPath;
			private set => uncPath = value;
		}

		public string DriveVolume
		{
			get => driveVolume;
			private set => driveVolume = value;
		}

		public List<string> Folders
		{
			get => folders;
			private set => folders = value;
		}

		/// <summary>
		/// the name of the file - no extension
		/// </summary>
		public string FileName
		{
			get => fileName;
			private set => fileName = value;
		}

		/// <summary>
		/// the file's extension
		/// </summary>
		public string FileExt
		{
			get => fileExt;
			private set => fileExt = value;
		}

	#endregion

	#region public status properties

		public bool HasQualifiedPath => HasDrive && (IsFolderPath || IsFilePath);

//		public bool IsFolderPath => (folders?.Count ?? 0) > 0;
//
//		public bool IsFilePath => !fileName.IsVoid();

		public bool IsFolderPath { get; private set; }

		public bool IsFilePath { get; private set; }

		public bool IsFound { get; private set; }

		public bool HasDrive => !driveVolume.IsVoid();

		public bool HasUnc => !uncVolume.IsVoid();

	#endregion

	#region sepatate path

		public bool parse(string pathWay)
		{
			this.pathway = pathWay;

			if (pathWay.IsVoid()) return false;

			string remain;

			separateReset();

			string path = CleanPath(pathWay);

			IsFound = separateIsFound(pathWay);

			remain = searateVolume(path);

			remain = separateFileAndExtension(remain);

			folders =
				separateFolders(remain);

			return true;
		}

		private void separateReset()
		{
			folders = new List<string>();
			uncVolume = null;
			driveVolume = null;
			uncPath = null;
			fileName = null;
			fileExt = null;
		}

		private bool separateIsFound(string pathWay)
		{
			bool isFolderPath;
			bool isFilePath;

			bool isFound = Exists(pathWay, out isFolderPath, out isFilePath);

			IsFolderPath = isFolderPath;
			IsFilePath = isFilePath;

			return isFound;
		}


		private List<string> separateFolders(string folders)
		{
			if (folders.IsVoid()) return new List<string>();

			return new List<string>(folders.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries));
		}

		/// <summary>
		/// extracts the filename and file extension from a partial path string
		/// IsFilePath & IsFolderPath must be determined before calling
		/// </summary>
		/// <param name="foldersAndFile"></param>
		/// <returns>the original string with the fileneme and file extension removed</returns>
		private string separateFileAndExtension(string foldersAndFile)
		{
			if (foldersAndFile.IsVoid()) return null;

			// note, do not do a full check - don't test
			// IsFilePath as the path may refer to a currently
			// non-existent file.  However, if it is
			// for sure a folder path, return
			// this will allow folder paths that looks like 
			// a filename and file extension to not be parsed wrong
			if (IsFolderPath) return foldersAndFile;

			// possible cases
			// A
			//    0         1
			//    012345678901234
			// ...\path\file.ext  ('.'=10, '\'=5 -> 10-5 = 5)
			// B
			//    0         1
			//    012345678901234
			// ...\path\file      ('.'=0, '\'=5 -> 0-5 = -5)
			// C
			//    0         1
			//    012345678901234
			// ...\path\.ext      ('.'=6, '\'=5 -> 6-5 = 1)
			// D
			//    0         1
			//    012345678901234
			// ...\path.x\file    ('.'=5, '\'=7 -> 5-7 = -2)

			int posPeriod = foldersAndFile.LastIndexOf('.');
			int posEndSeparator = foldersAndFile.LastIndexOf('\\');
			int result = posPeriod - posEndSeparator;


			if (result > 1)
			{
				// case A
				fileName = foldersAndFile.Substring(posEndSeparator + 1, posPeriod - posEndSeparator - 1);
				fileExt = foldersAndFile.Substring(posPeriod + 1);
			} 
			else if (result == 1)
			{
				// case C
				fileName = "";
				fileExt = foldersAndFile.Substring(posEndSeparator + 1);
			}
			else
			{
				// case B
				// case D
				fileName = foldersAndFile.Substring(posEndSeparator + 1);
				fileExt = "";
			}

			return foldersAndFile.Substring(0, posEndSeparator);
		}

		private string searateVolume(string path)
		{
			if (path.IsVoid()) return null;

			uncPath = UncVolumeFromPath(path);
			driveVolume = DriveVolumeFromPath(path);

			if (!driveVolume.IsVoid())
			{
				return separateRemainderFromVolume(path);
			}

			string result = null;

			if (path.StartsWith(@"\\"))
			{
				return extractUncVolume(path);
			}

			return separateRemainder(path);
		}

		private string separateRemainder(string path)
		{
			if (path[0] == '\\' &&
				path.Length == 1) return null;

			return path;
		}

		private string separateRemainderFromVolume(string path)
		{
			// maybe A or B
			int pos = 2;

			separateUncVolume();

			if (path.StartsWith(@"\\") && !uncPath.IsVoid())
			{
				pos = uncVolume.Length + uncPath.Length;
			}

			if (path.Length == pos) return null;

			return path.Substring(pos);
		}

		private void separateUncVolume()
		{
			if (uncPath.IsVoid()) return;

			int pos = uncPath.IndexOf('\\', 2);

			if (pos == -1)
			{
				uncVolume = uncPath;
				uncPath = null;
				return;
			}

			if (pos > 2)
			{
				uncVolume = uncPath.Substring(0, pos);
				uncPath = uncPath.Substring(pos);
				return;
			}

			uncVolume = null;
			uncPath = null;
		}

		private string extractUncVolume(string path)
		{
			if (path.Length == 2) return null;

			int pos = path.IndexOf('\\', 2);

			if (pos == -1)
			{
				uncVolume = path;
				return null;
			}
			else if (pos > 2)
			{
				uncVolume = path.Substring(0, pos);
				return path.Substring(pos);
			}

			return null;
		}

	#endregion

	#region public static methods

		/// <summary>
		/// determine if the pathway points of an
		/// actual file or folder
		/// </summary>
		/// <param name="pathWay"></param>
		/// <returns>true if pathway points to an actual file or folder</returns>
		public bool Exists(string pathWay, out bool isFolderPath, out bool isFilePath)
		{
			isFolderPath = false;
			isFilePath = false;
			try
			{
				isFolderPath = Directory.Exists(pathWay);
			}
			catch
			{
				/* ignored */
			}

			if (!isFolderPath)
			{
				try
				{
					isFilePath = File.Exists(pathWay);
				}
				catch
				{
					/* ignored */
				}
			}

			return isFolderPath || isFilePath;
		}


		public static string CleanPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}

			string result;

			try
			{
				result = path.Replace('/', '\\').Trim();

				if (result[1] == ':')
				{
					result = result.Substring(0, 1).ToUpper() + result.Substring(1);
				}
			}
			catch
			{
				return null;
			}

			return result;
		}

		public static void getUncNameMap()
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

		public static string DriveVolumeFromPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return null;

			string drive = findDriveFromUncPath(path);

			if (!string.IsNullOrWhiteSpace(drive)) return drive;


			if (!path.StartsWith(@"\\"))
			{
				// does not start with "\\" if character 2 is ':' 
				// assume provided with a drive and return that portion
				if (path.Substring(1, 1).Equals(":")) return path.Substring(0, 2);
			}

			return null;
		}


		public static string findDriveFromUncPath(string path)
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

		public static string findUncFromUncPath(string path)
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
// UncVolume     ^------^   ||      | |  | | |
// DrivePath       ^----------^|      | |  | | | 
// Folders        |           ^------^ |  | | |
// Path           ^------------------^ |  | | |
// FileName                ^--^ | |
// FileExtension                       |    ^-^
// FileNameAndExtension                            ^------^
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
// UncVolume     ^^|          | |  | | |
// DrivePath       ^-^          | |  | | | 
// Folders        | ^----------^ |  | | |
// Path           ^------------^ |  | | |
// FileName          ^--^ | |
// FileExtension                 |    ^-^
// FileNameAndExtension                      ^------^
// FullPath       ^---------------------^

// example
// levels 4
//   full route| P:\2099-999 Sample Project\Publish\9999 Current\A  A2.1-0  - DO NOT REMOVE.pdf
//               []                               GetFolder([])
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

GetFolder(r[i])
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