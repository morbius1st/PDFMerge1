#region + Using Directives

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Tests2.DebugSupport;
using UtilityLibrary;

#endregion


// projname: AODeliverable.FileSelection
// itemname: FileItem
// username: jeffs
// created:  11/3/2019 8:30:24 AM


namespace Tests2.FileListManager
{
//	public enum FileItemType
//	{
//		UNDEFINED,
//		OTHER,
//		FILE,
//		DIRECTORY
//	}

	public class FileItem : IComparable<FileItem>, INotifyPropertyChanged
	{
		public const int MAX_DEPTH = 99;

		// static

		// the root basis for all of the subpaths

		private static Route rootPath = Route.Invalid;

		public static Route RootPath
		{
			get { return rootPath; }
			set
			{
				if (!value.IsValid)
				{
					rootPath = Route.Invalid;
					return;
				}

				rootPath = value;
			}
		}

		internal static int NonFileCount { get; private set; }

		// instance

		private string sequenceCode;

		// as provided

		public FileItem(Route path)
		{
			Configure(path);
		}

		public FileItem(string path) 
		{
			Configure(new Route(path));
		}

		private void Configure(Route path)
		{
			FilePath = path;

			if (!path.IsValid || path.RouteType == RouteType.FILE) 
			{ 
				// initial, make the outlinepath  
				// be the relative file path

//				DebugHelper.Prime.ListRoute(path);
				
				SubPath = path.SubPath(rootPath);
				OutlinePath = SubPath.Clone();
			}
			else
			{
				// all but FILE - just save the path for later
				// review 
				SubPath = path.Clone();
				OutlinePath = path.Clone();
			}

			if (SubPath.RouteType != RouteType.FILE)
			{
				NonFileCount++;
			}
		}

		public Route FilePath { get; set; } = Route.Invalid;

		// path is the actual sub-folder path 
		// starting from the rootpath down to
		// the actual folder
		public Route SubPath { get; private set; } = Route.Invalid;

		// this is the path of names that will define the 
		// bookmark names.  for a merge based on
		// using a folder structure, this will match
		// the path value.  
		// for using a created outline structure,
		// this will need to be manufactured
		public Route OutlinePath { get; set; } = null;

		public string OutlineFolders
		{
			get
			{
				string p = null;
				string[] names = OutlinePath.FolderNames;

				for (int i = 0; i < names.Length; i++ )
				{
					p += " |  [" + i + "] " + names[i];
				}

				return p;

			}
		}

		public bool IsFile => FilePath.RouteType == RouteType.FILE;

		public string SequenceCode
		{
			get
			{
				if (string.IsNullOrWhiteSpace(sequenceCode))
				{
					return "Ζ";
				}

				return sequenceCode;
			}
			set
			{
				sequenceCode = value;
				OnPropertyChange();
			}
		}

//		public string SortCode => SequenceCode + "Ζ :: " + OutlinePath.FileWithoutExtension;
		public string SortCode {
		get {
			return SequenceCode + "-" + (MAX_DEPTH - OutlinePathDepth) +
				" :: " + OutlinePath.FileWithoutExtension;
		}
	}

		public int OutlinePathDepth
		{
			get
			{
				if (!IsFile) { return -1; }

				return OutlinePath.Depth;
			}
		}

		public string FileNameWithoutExtension()
		{
			if (IsFile) { return ""; }

			return SubPath.FileWithoutExtension;
		}

		public string FullPath
		{
			get
			{
				if (IsFile) { return ""; }

				return rootPath.FullPath + "\\" + SubPath.FullPath;
			}
		}

		public int CompareTo(FileItem other)
		{
//			int b = "".CompareTo("a");
//			int c = "Ζ".CompareTo("A");
//			int d = "Ζ".CompareTo("z");
//			int e = "Ζ".CompareTo("Z");
//			int f = "Ζ".CompareTo("0");
//			int g = "Ζ".CompareTo("9");
//
//
			int a = SortCode.CompareTo(other.SortCode);

			return SortCode.CompareTo(other.SortCode);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return SortCode + " <> " + OutlinePath.ToString();
		}
	}
}
