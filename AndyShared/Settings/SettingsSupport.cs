using System;
using System.IO;
using System.Xml.Linq;
using SettingsManager;
using UtilityLibrary;
using AndyShared.FileSupport;

namespace AndyShared.Settings
{
	public static class SettingsSupport
	{





		// TODO: use the utility library version
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


		// public static List<FilePath<FileNameSimpleSelectable>>
		// 	GetSiteFiles( FilePath<FileNameSimpleSelectable> folder,
		// 	string pattern)
		// {
		// 	List<FilePath<FileNameSimpleSelectable>> files = new List<FilePath<FileNameSimpleSelectable>>();
		//
		// 	foreach (string file in
		// 		Directory.EnumerateFiles(folder.FullFilePath, pattern, SearchOption.AllDirectories))
		// 	{
		// 		files.Add(new FilePath<FileNameSimpleSelectable>(file));
		// 	}
		//
		// 	return files;
		// }
	}
}