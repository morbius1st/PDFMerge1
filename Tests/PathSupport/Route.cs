#region + Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using SysPath = System.IO.Path;

#endregion

// itemname: Route
// username: jeffs
// created:  11/2/2019 5:18:09 PM

/*
1.  Change name to PathEx?
2.  done - Make properties use Get...
3.  done - Add a const for path separator.
4.  Follow Path class for properties
5.  Validate path characters
6.  done fdAdd GetCurrentDirectory
7.  done getpath = all of the parts as an array
8.  done make volume be the Unc volume and change to UncVloume, add Uncpath be the unc volume + its path
9.  done change root to RootVolume
10. done add switch to provide as unc when possible?


 */


namespace UtilityLibrary
{
	[DataContract]
	public class Route<T> : IEquatable<Route<T>>, IComparable<Route<T>>
		where T : AFileName, new()
	{
		internal const string PATH_SEPARATOR = @"\";
		internal const char PATH_SEPARATOR_C = '\\';
		internal const string DRV_SUFFIX = ":";
		internal const char DRV_SUFFIX_C = ':';
		internal const string UNC_PREFACE = @"\\";
		internal const char EXT_SEPARATOR_C = '.';


	#region private fields

		private PathWay<T> pathway;

	#endregion

	#region static properties

		/// <summary>
		/// a Route that is not valid (IsValid == false)
		/// </summary>
		public static Route<T>Invalid => new Route<T>();

		/// <summary>
		/// the current directory
		/// </summary>
		public static Route<T> CurrentDirectory => new Route<T>(Environment.CurrentDirectory);

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
				sb.Append(PATH_SEPARATOR).Append(path[i]);
			}

			ConfigureRoute(sb.ToString());
		}

		private void ConfigureRoute(string initialPath)
		{
			PathWay<T>.getUncNameMap();

			pathway = new PathWay<T>();

			IsValid = pathway.parse(initialPath);
		}

	#endregion

	#region  public status properties

		public bool IsValid { get; private set; }
		public bool HasQualifiedPath => pathway.HasQualifiedPath;
		public bool HasUnc => pathway.HasUnc;
		public bool HasDrive => pathway.HasDrive;
		public bool HasFileName => !pathway.Name.IsVoid();
		public bool IsFolderPath => pathway.IsFolderPath;
		public bool IsFilePath => pathway.IsFilePath;
		public bool IsFound => pathway.IsFound;

	#endregion

	#region public properties

		public PathWay<T> PathWay => pathway;

		[DataMember]
		public string GetFullPath
		{
			get => pathway.Path;
			set => pathway.Path = value;
		}

		/// <summary>
		/// the drive volume without a slash suffix
		/// </summary>
		public string GetDriveVolume => pathway.DriveVolume;

		/// <summary>
		/// the drive volume with a slash suffix
		/// </summary>
		public string GetDrivePath => pathway.DriveVolume + DRV_SUFFIX;

		public string GetDriveRoot => GetDrivePath + PATH_SEPARATOR;

		public string GetUncVolume => pathway.UncVolume;

		public string GetUncPath => pathway.UncVolume + pathway.UncShare;

		public string GetUncShare => pathway.UncShare;

		public T GetFileNameObject => pathway.FileName;

		public string GetFileName
		{
			get
			{
				if (pathway.Name.IsVoid() &&
					pathway.Extension.IsVoid()) return null;

				if (pathway.Extension.IsVoid()) return pathway.Name;

				return pathway.Name + "." + pathway.Extension;
			}
		}

		public string GetFileNameWithoutExtension => pathway.Name;

		public string GetFileExtension => pathway.Extension;

		/// <summary>
		/// the path without the filename and extension
		/// </summary>
		public string GetPath
		{
			get
			{
				if (!IsValid) return null;

				return AssemblePath(Depth);
			}
		}
		
		/// <summary>
		/// the path without the filename and extension and using unc if exists
		/// </summary>
		public string GetPathUnc
		{
			get
			{
				if (!IsValid) return null;

				return AssemblePath(Depth, true);
			}
		}

		public string[] GetPathNames => PathNames(false);

		public string[] GetPathNamesAlt => PathNames(true);

		public string GetFolders
		{
			get
			{
				if (!IsValid) return null;

				return AssembleFolders();
			}
		}

		public int GetFolderCount => pathway.Folders.Count;

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

		public int Length => GetFullPath.Length;

		/// <summary>
		/// cause information returned to provide the UncShare
		/// rather than the GetDrivePath
		/// </summary>
		public bool UseUnc { get; set; } = false;

	#endregion

	#region public methods

		public void ChangeFileName(string name)
		{
			pathway.Name = name;
		}

		public void ChangeExtension(string ext)
		{
			pathway.Extension = ext;
		}

		public string[] PathNames(bool withBackSlash = false, bool useUnc = false)
		{
			if (!IsValid) return null;

			List<string> path = new List<string>();

			path.Add(GetRootPath(useUnc));

			for (int i = 0; i < Depth - 1; i++)
			{
				path.Add(
					(withBackSlash ? PATH_SEPARATOR : "") +
					pathway.Folders[i]);
			}

			return path.ToArray();
		}

		public string AssemblePath(int index, bool useUnc = false)
		{
			if (!IsValid) return null;

			int idx = CalcIndex(index);

			if (idx < 0) return null;

			if (idx == 0)
			{
				return GetRootPath(useUnc);
			}

			StringBuilder sb = new StringBuilder(GetRootPath(useUnc));

			for (int i = 0; i < idx - 1; i++)
			{
				sb.Append(PATH_SEPARATOR).Append(pathway.Folders[i]);
			}

			return sb.ToString();
		}

		public string AssembleFolders(int index = 0)
		{
			if (!IsValid) return null;

			int idx = CalcIndex(index);

			if (idx < 0) return null;

			// do not return just the root
			if (idx == 0) idx = Depth;

			StringBuilder sb = new StringBuilder();

			for (int i = 1; i < idx - 1; i++)
			{
				sb.Append(PATH_SEPARATOR).Append(pathway.Folders[i - 1]);
			}

			return sb.ToString();
		}

		public Route<T>Clone()
		{
			return new Route<T>(GetFullPath);
		}

	#endregion

	#region indexer

		public string this[double index]
		{
			get
			{
				if (!IsValid) return null;

				bool makePath = !(index % 1).Equals(0);

				int idx = CalcIndex((int) index);

				if (idx < 0) return null;

				if (idx == 0 ||
					pathway.Folders.Count == 0) return GetRootPath();

				return 
					(makePath ? PATH_SEPARATOR : "") +
					pathway.Folders[idx - 1];
			}
		}

	#endregion

	#region private methods

		private string GetRootPath(bool useUnc = false)
		{
			if ((useUnc || UseUnc) && HasUnc)
			{
				return GetUncPath;
			}

			return GetDrivePath;
		}

		private int CalcIndex(int index)
		{
			if (index < 0) index = Depth + index;

			if (index > Depth || index < 0) return -1;

			return index;
		}

	#endregion

	#region system methods

		public bool Equals(Route<T>other)
		{
			return this.GetFullPath.ToUpper().Equals(other.GetFullPath.ToUpper());
		}

		public int CompareTo(Route<T>other)
		{
			return GetFullPath.CompareTo(other.GetFullPath);
		}

		public override string ToString()
		{
			return GetFullPath;
		}

	#endregion
	}

	public static class DllImports
	{
		[DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int WNetGetConnection(
			[MarshalAs(UnmanagedType.LPTStr)] string localName,
			[MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName,
			ref int length);
	}

	public class PathWay<T> where T : AFileName, new()
	{

	#region public fields

		public static Dictionary<string, string> UncNameMap { get; private set; }  =
			new Dictionary<string, string>(10);

	#endregion

	#region private fields

		private string path; // original - un-modified;

		private string uncVolume; // '//' + computer name

		private string uncShare; // all except for the uncVolume

		private string driveVolume; // drive letter + colon

		private List<string> folders; // all of the folders along the path

//		private string fileName;

		private T fileName = new T();

	#endregion

	#region public properties

		public string Path
		{
			get => path;
			set => parse(value);
		}

		public string UncVolume
		{
			get => uncVolume;
			private set => uncVolume = value;
		}

		public string UncShare
		{
			get => uncShare;
			private set => uncShare = value;
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

		public T FileName => fileName;

		/// <summary>
		/// the name of the file - no extension
		/// </summary>
		public string Name
		{
			get => fileName.Name;
			set => fileName.Name = value;
		}

		/// <summary>
		/// the file's extension
		/// </summary>
		public string Extension
		{
			get => fileName.Extension;
			set => fileName.Extension = value;
		}

	#endregion

	#region public status properties

		public bool HasQualifiedPath => HasDrive && (IsFolderPath || IsFilePath);

		public bool IsFolderPath { get; private set; }

		public bool IsFilePath { get; private set; }

		public bool IsFound { get; private set; }

		public bool HasDrive => !driveVolume.IsVoid();

		public bool HasUnc => !uncVolume.IsVoid();

	#endregion

	#region parse

		public bool parse(string pathWay)
		{
			this.path = pathWay;

			if (pathWay.IsVoid()) return false;

			string remain;

			parseReset();

			string path = CleanPath(pathWay);

			IsFound = parseIsFound(pathWay);

			remain = parseVolume(path);

			remain = parseFileAndExtension(remain);

			folders =
				parseFolders(remain);

			return true;
		}

		private void parseReset()
		{
			folders = new List<string>();
			uncVolume = null;
			driveVolume = null;
			uncShare = null;
			fileName = new T();
		}

		private bool parseIsFound(string pathWay)
		{
			bool isFolderPath;
			bool isFilePath;

			bool isFound = Exists(pathWay, out isFolderPath, out isFilePath);

			IsFolderPath = isFolderPath;
			IsFilePath = isFilePath;

			return isFound;
		}

		private List<string> parseFolders(string folders)
		{
			if (folders.IsVoid()) return new List<string>();

			return new List<string>(folders.Split(new char[] {Route<T>.PATH_SEPARATOR_C}, StringSplitOptions.RemoveEmptyEntries));
		}

		/// <summary>
		/// extracts the filename and file extension from a partial path string
		/// IsFilePath & IsFolderPath must be determined before calling
		/// </summary>
		/// <param name="foldersAndFile"></param>
		/// <returns>the original string with the fileneme and file extension removed</returns>
		private string parseFileAndExtension(string foldersAndFile)
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
			// A  (>0)
			//    0         1
			//    012345678901234
			// ...\path\file.ext  ('.'=10, '\'=5 -> 10-5 = 5)
			// B  (<0)
			//    0         1
			//    012345678901234
			// ...\path\file      ('.'=0, '\'=5 -> 0-5 = -5)
			// C  (== 1)
			//    0         1
			//    012345678901234
			// ...\path\.ext      ('.'=6, '\'=5 -> 6-5 = 1)
			// D  (<0)
			//    0         1
			//    012345678901234
			// ...\path.x\file    ('.'=5, '\'=7 -> 5-7 = -2)

			int posPeriod = foldersAndFile.LastIndexOf(Route<T>.EXT_SEPARATOR_C);
			int posEndSeparator = foldersAndFile.LastIndexOf(Route<T>.PATH_SEPARATOR_C);
			int result = posPeriod - posEndSeparator;


			if (result > 1)
			{
				// case A
				fileName.Name = foldersAndFile.Substring(posEndSeparator + 1, posPeriod - posEndSeparator - 1);
				fileName.Extension = foldersAndFile.Substring(posPeriod + 1);
			}
			else if (result == 1)
			{
				// case C
				fileName.Name = "";
				fileName.Extension = foldersAndFile.Substring(posPeriod + 1);
			}
			else
			{
				// case B
				// case D
				fileName.Name = foldersAndFile.Substring(posEndSeparator + 1);
				fileName.Extension = "";
			}

			return foldersAndFile.Substring(0, posEndSeparator);
		}

		private string parseVolume(string path)
		{
			if (path.IsVoid()) return null;

			uncShare = UncVolumeFromPath(path);
			driveVolume = DriveVolumeFromPath(path);

			if (!driveVolume.IsVoid())
			{
				return parseRemainderFromVolume(path);
			}

			string result = null;

			if (path.StartsWith(Route<T>.PATH_SEPARATOR))
			{
				return extractUncVolume(path);
			}

			return parseRemainder(path);
		}

		private string parseRemainder(string path)
		{
			if (path[0] == Route<T>.PATH_SEPARATOR_C &&
				path.Length == 1) return null;

			return path;
		}

		private string parseRemainderFromVolume(string path)
		{
			// maybe A or B
			int pos = 2;

			parseUncVolume();

			if (path.StartsWith(Route<T>.PATH_SEPARATOR) && !uncShare.IsVoid())
			{
				pos = uncVolume.Length + uncShare.Length;
			}

			if (path.Length == pos) return null;

			return path.Substring(pos);
		}

		private void parseUncVolume()
		{
			if (uncShare.IsVoid()) return;

			int pos = uncShare.IndexOf(Route<T>.PATH_SEPARATOR_C, 2);

			if (pos == -1)
			{
				uncVolume = uncShare;
				uncShare = null;
				return;
			}

			if (pos > 2)
			{
				uncVolume = uncShare.Substring(0, pos);
				uncShare = uncShare.Substring(pos);
				return;
			}

			uncVolume = null;
			uncShare = null;
		}

		private string extractUncVolume(string path)
		{
			if (path.Length == 2) return null;

			int pos = path.IndexOf(Route<T>.PATH_SEPARATOR_C, 2);

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
		/// determine if the path points of an
		/// actual file or folder
		/// </summary>
		/// <param name="path">String of the folder or file determine if it exists</param>
		/// <param name="isFolderPath">out bool - true if the path references a folder</param>
		/// <param name="isFilePath">out bool - true if the path references a file</param>
		/// <returns>true if path points to an actual file or folder</returns>
		public static bool Exists(string path, out bool isFolderPath, out bool isFilePath)
		{
			isFolderPath = false;
			isFilePath = false;
			try
			{
				isFolderPath = Directory.Exists(path);
			}
			catch
			{
				/* ignored */
			}

			if (!isFolderPath)
			{
				try
				{
					isFilePath = File.Exists(path);

					

				}
				catch
				{
					/* ignored */
				}
			}

			return isFolderPath || isFilePath;
		}

		/// <summary>
		/// clean the string that represents a path -
		/// replace slashes with back slashes
		/// remove preface and suffix spaces
		/// </summary>
		/// <param name="path">the file path to clean</param>
		/// <returns></returns>
		public static string CleanPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}

			string result;

			try
			{
				result = path.Replace('/', Route<T>.PATH_SEPARATOR_C).Trim();

				if (result[1] == Route<T>.DRV_SUFFIX_C)
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

			if (!path.StartsWith(Route<T>.UNC_PREFACE))
			{
				StringBuilder sb = new StringBuilder(1024);
				int size = sb.Capacity;

				// still may fail but has a better chance;
				int error = DllImports.WNetGetConnection(path.Substring(0, 2), sb, ref size);

				if (error != 0) return null;

				return sb.ToString();
			}

			return findUncFromUncPath(path);
		}

		public static string DriveVolumeFromPath(string path)
		{
			if (path.IsVoid()) return null;

			string drive = findDriveFromUncPath(path);

			if (!drive.IsVoid()) return drive;


			if (!path.StartsWith(Route<T>.UNC_PREFACE))
			{
				// does not start with "\\" if character 2 is ':' 
				// assume provided with a drive and return that portion
				if (path.Substring(1, 1).Equals(Route<T>.DRV_SUFFIX)) return path.Substring(0, 1);
			}

			return null;
		}

		public static string findDriveFromUncPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path) ||
				!path.StartsWith(Route<T>.UNC_PREFACE) ||
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
				|| !path.StartsWith(Route<T>.UNC_PREFACE)
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

	#region system overrites

		public override string ToString()
		{
			return path;
		}

	#endregion
	}


	public abstract class AFileName : IEquatable<AFileName>, IComparable<AFileName>
	{
		protected string filename;
		protected string fileextension;

		public virtual string Name
		{
			get => filename;
			set => filename = value;
		}

		public string Extension
		{
			get => fileextension;
			set => fileextension = value;
		}

		public bool Equals(AFileName other)
		{
			return Name.Equals(other.Name) &&
				Extension.Equals(other.Extension);
		}

		public int CompareTo(AFileName other)
		{
			int result = Name.CompareTo(other.Name);

			if ( result != 0) return result;

			return Extension.CompareTo(other.Extension); 
		}


	}

	public class FileNameSimple : AFileName
	{

//		public FileNameSimple() { }

//		public FileNameSimple(string filename)
//		{
//			this.filename = filename;
//		}
	}

	public class FileNameAsSheet : AFileName
	{
		private string sheetnumber;
		private string sheetname;

//		public FileNameAsSheet() { }
//
//		public FileNameAsSheet(string name)
//		{
//			ParseName(name);
//
//		}

		public string SheetNumber
		{
			get => sheetnumber;
			set => sheetnumber = value;
		}

		public string SheetName
		{
			get => sheetname;
			set => sheetname = value;
		}

		public override string Name
		{
			get => sheetnumber + " :: " + sheetname;
			set
			{
				ParseName(value);
			}
		}

		private void ParseName(string name)
		{
			sheetnumber = name?.Substring(0, 5) ?? null;
			sheetname = name?.Substring(6) ?? null;
		}
	}

}

/*
Depth = 4
GetFolderCount = 3
Length = 63 (characters long)

the below is modified by UseUnc

use indexer
note: use index of [#.x] to prepend a slash -----------------------------v
FolderName 3  [+3] or [-1]                                v----------v   [+3.1] or [-1.1] = \FolderName 3
FolderName 2  [+2] or [-2]                   v----------v |          |   [+2.1] or [-2.1] = \FolderName 2
FolderName 1  [+1] or [-3]      v----------v |          | |          |   [+1.1] or [-3.1] = \FolderName 1
P:            [+0] or [-4]   vv |          | |          | |          |   [+0.1] or [-4.1] = P:\
GetDriveRoot                 v-v|          | |          | |          |
GetDrivePath                 vv||          | |          | |          |
GetDriveVolume               v|||          | |          | |          |
GetFullPath                  P:\FolderName 1\FolderName 2\FolderName 3\New Text Document.txt
GetPath                      ^---------------------------------------^ |               | | |
GetFolders                    |^-------------------------------------^ |               | | |
equivalent unc:               ||                                     | |               | | |
	           \\CS-006\P Drive\FolderName 1\FolderName 2\FolderName 3\New Text Document.txt
GetUncVolume   ^------^|      |                                      | |               | | |
GetUncShare    |       ^------^                                      | |               | | |
GetUncPath     ^--------------^                                      | |               | | |
GetPathUnc     ^-----------------------------------------------------^ |               | | |
GetFileNameWithoutExtension                                          | ^---------------^ | | (does not include '.')
GetExtension                                                         | |                 ^-^ (does not include '.')
GetFileName                                                          | ^-------------------^ (does include '.')

get array: GetPathNames    -> [0] same as indexer[0],   [1] same as indexer [1], etc.
get array: GetPathNamesAlt -> [0] same as indexer[0.1], [1] same as indexer [1.1], etc.

AssemblePath(2)     | P:\FolderName 1
AssemblePath(-1)    | P:\FolderName 1\FolderName 2

*** Settings ***
UseUnc              | False

*** Status ***
IsValid             | True
HasQualifiedPath    | True
HasUnc              | True
HasDrive            | True
hasFilename         | True
IsFolderPath        | False
IsFilePath          | True
IsFound             | True


*/
