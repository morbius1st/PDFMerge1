#region + Using Directives

using System.Collections;
using System.Collections.Generic;
using System.IO;

#endregion


// projname: AODeliverable.FolderSelection
// itemname: SelectFolderMgr
// username: jeffs
// created:  11/2/2019 3:17:22 PM


namespace AODeliverable.FileSelection
{
	public class SelectFilesMgr : IEnumerable<FileItem>
	{
		private static SelectFilesMgr instance = null;

		private Route baseFolder = null;

		private List<FileItem> fileItems = new List<FileItem>();

		public static SelectFilesMgr Instance
		{
			get
			{
				if (instance == null) instance = new SelectFilesMgr();

				return instance;
			}
		}

		private SelectFilesMgr() 
		{ }

		public FileItem this[int index] => fileItems[index];

		public int ItemCount => fileItems.Count;

		public bool Initialized { get; private set; } = false;

		public Route BaseFolder
		{
			get { return baseFolder; }
			set
			{
				if (value != null)
				{
					if (!Directory.Exists(value.path))
					{
						throw new DirectoryNotFoundException();
					}

					baseFolder = value;
					FileItem.RootPath = baseFolder.FullRoute;
				}
			}
		}

		public void Clear()
		{
			fileItems.Clear();
		}

		public void Sort()
		{
			fileItems.Sort();
		}

		public bool GetFiles()
		{
			foreach (string file in 
				Directory.EnumerateFiles(baseFolder.path, "*.pdf", 
					SearchOption.AllDirectories))
			{
				fileItems.Add(new FileItem(file));
			}

			if (fileItems.Count <= 0) return false;

			Initialized = true;

			return true;
		}

		public IEnumerator<FileItem> GetEnumerator()
		{
			return fileItems.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}


	}
}
