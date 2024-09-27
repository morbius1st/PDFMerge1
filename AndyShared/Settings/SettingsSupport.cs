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
		/// <summary>
		/// the path for the root folder of all of the user classification files<br/>
		/// equals: "C:\ProgramData\{app name}\Andy\User Classification Files  (SM7.5)<br/>
		/// or equals: "C:\ProgramData\{app name}\Andy\{app name}\User Classification Files  (pre SM7.5)<br/>
		/// </summary>
		public static readonly FilePath<FileNameSimple> AllClassifFolderPath = 
			new FilePath<FileNameSimple>(MachSettings.Path.SettingFolderPath + FilePathConstants.USER_STORAGE_FOLDER);

		/// <summary>
		/// the path for the user's user classification files<br/>
		/// equals: "C:\ProgramData\{app name}\Andy\User Classification Files\{user name}  (SM7.5)<br/>
		/// or equals: "C:\ProgramData\{app name}\Andy\{app name}\User Classification Files\{user name}  (pre SM7.5)<br/>
		/// </summary>
		public static readonly FilePath<FileNameSimple> UserClassifFolderPath = 
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