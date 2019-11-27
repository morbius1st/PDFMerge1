#region + Using Directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Felix.Support;

#endregion


// projname: AODeliverable.FileSelection
// itemname: FileItem
// username: jeffs
// created:  11/3/2019 8:30:24 AM


namespace Felix.FileListManager
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

		public bool IsFile => FilePath.RouteType == RouteType.FILE;

		public string SequenceCode
		{
			get => sequenceCode;
			set
			{
				sequenceCode = value;
				OnPropertyChange();
			}
		}

		public string SortCode => sequenceCode + " :: " + OutlinePath.FileWithoutExtension;

		public int OutlineDepth
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
			return SortCode.CompareTo(other.SortCode);

//			return OutlinePath.CompareTo(other.OutlinePath);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}
}
