#region using

using System.Collections.Generic;
using System.IO;
using AndyShared.FilesSupport;
using AndyShared.Settings;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  6/14/2020 10:57:14 PM

namespace AndyShared.FilesSupport
{
	public class FolderAndFileSupport
	{
	#region private fields

		private FilePath<FileNameSimpleSelectable> folder;
		private string pattern;
		private List<FilePath<FileNameSimpleSelectable>> foundFiles;

	#endregion

	#region ctor

		public FolderAndFileSupport( string folder, string pattern)
		{
			if (folder != null)
			{
				Folder = new FilePath<FileNameSimpleSelectable>(folder);
			}
			else
			{
				Folder = FilePath<FileNameSimpleSelectable>.Invalid;
			}

			this.pattern = pattern;
		}

	#endregion

	#region public properties

		public FilePath<FileNameSimpleSelectable> Folder
		{
			get => folder;
			set => folder = value;
		}

		public string Pattern
		{
			get => pattern;
			set => pattern = value;
		}

		public List<FilePath<FileNameSimpleSelectable>> FoundFiles
		{
			get => foundFiles;
			set
			{
				foundFiles = value;
			}
		}

		public int Count => foundFiles.Count;

		public bool HasFiles => foundFiles.Count > 0;

		public bool FolderExists => Directory.Exists(folder.FullFilePath);

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void GetFiles()
		{
			FoundFiles = SettingsSupport.GetSiteFiles(folder, pattern);
		}

	#endregion

	#region private methods

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FolderAndFileSupport";
		}

	#endregion
	}
}