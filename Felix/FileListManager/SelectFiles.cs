#region + Using Directives

using System.Collections.Generic;
using System.IO;

#endregion


// projname: Tests2.FileListManager
// itemname: SelectFiles
// username: jeffs
// created:  11/16/2019 8:45:41 AM


namespace Felix.FileListManager
{
	/// <summary>
	/// the folder path has been selected
	/// select the files in the folder hierarchy
	/// depending on program settings
	/// this creates the FileList
	/// </summary>
	public class SelectFiles
	{
		private FileItems filLst;

		public SelectFiles()
		{
			filLst = FileItems.Instance;

			filLst.Initialize();
		}

		public bool GetFileList()
		{
			if (!MakeFileList()) return false;

			return UpdateBookmarks();
		}

		private bool UpdateBookmarks()
		{
			return true;
		}

		public bool MakeFileList()
		{
			List<FileItem> files = GetFiles();
			
			if (files.Count <= 0) return false;

			filLst.Add(files);

			return true;
		}

		private List<FileItem> GetFiles()
		{
			List<FileItem> files = new List<FileItem>();

			string pattern = "*.pdf";
			
			foreach (string file in 
				Directory.EnumerateFiles(FileListMgr.BaseFolder.FullPath, pattern, 
					SearchOption.AllDirectories))
			{
				files.Add(new FileItem(file));
			}

			return files;
		}

	}
}
