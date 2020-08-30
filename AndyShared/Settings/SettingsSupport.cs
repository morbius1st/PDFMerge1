using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using AndyShared.FileSupport;
using SettingsManager;
using UtilityLibrary;

namespace AndyShared.Settings
{
	class SettingsSupport
	{
		public static bool ValidateXmlFile(string filePath)
		{
			try
			{
				XDocument xml = new XDocument();

				xml = XDocument.Load(filePath);
			}
			catch
			{
				return false;
			}

			return true;
		}


		public static List<FilePath<FileNameSimpleSelectable>>
			GetSiteFiles( FilePath<FileNameSimpleSelectable> folder,
			string pattern)
		{
			List<FilePath<FileNameSimpleSelectable>> files = new List<FilePath<FileNameSimpleSelectable>>();

			foreach (string file in
				Directory.EnumerateFiles(folder.FullFilePath, pattern, SearchOption.AllDirectories))
			{
				files.Add(new FilePath<FileNameSimpleSelectable>(file));
			}

			return files;
		}
	}
}