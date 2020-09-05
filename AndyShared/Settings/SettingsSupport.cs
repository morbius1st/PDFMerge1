using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using AndyShared.FileSupport;
using SettingsManager;
using UtilityLibrary;

namespace AndyShared.Settings
{
	public static class SettingsSupport
	{

		public static FilePath<FileNameSimple> AllClassifFolderPath = 
			new FilePath<FileNameSimple>(MachSettings.Path.SettingFolderPath + FilePathConstants.USER_STORAGE_FOLDER);

		public static FilePath<FileNameSimple> UserClassifFolderPath = 
			new FilePath<FileNameSimple>(FilePathUtil.AssembleFolderPath(false, 
				AllClassifFolderPath.FolderPath, Environment.UserName));




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