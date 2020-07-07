using System.Collections.Generic;
using System.IO;
using AndyShared.FilesSupport;
using SettingsManager;
using UtilityLibrary;

namespace AndyShared.Settings
{
	class SettingsSupport
	{

		public static List<FilePath<FileNameSimpleSelectable>>
			GetSiteFiles( FilePath<FileNameSimpleSelectable> folder,
			string pattern)
		{
			List<FilePath<FileNameSimpleSelectable>> files = new List<FilePath<FileNameSimpleSelectable>>();

			foreach (string file in
				Directory.EnumerateFiles(folder.GetFullFilePath, pattern, SearchOption.AllDirectories))
			{
				files.Add(new FilePath<FileNameSimpleSelectable>(file));
			}

			return files;
		}
	}
}