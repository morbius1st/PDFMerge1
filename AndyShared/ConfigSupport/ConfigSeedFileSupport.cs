using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using AndyShared.ConfigSupport;
using AndyShared.FilesSupport;
using SettingsManager;
using UtilityLibrary;

namespace AndyShared.ConfigSupport
{
	public static class ConfigSeedFileSupport
	{
		public const string SEED_PATTERN = @"*.seed.xml";
		public const string SEED_FOLDER_NAME = @"Seed Files";
		public const string SEED_FOLDER_SITE = @"\Site " + SEED_FOLDER_NAME;
		public const string SEED_FOLDER_SUITE = @"\Suite " + SEED_FOLDER_NAME;
		public const string SEED_FOLDER_INSTALLED = @"\Installed " + SEED_FOLDER_NAME;
		public const string ASSOC_FILE_EXT = ".dat";

		public static bool GetFiles(ObservableCollection<ConfigSeedFile> collection,
			string folder, string pattern = SEED_PATTERN, 
			SearchOption options = SearchOption.AllDirectories, 
			bool select = false, bool local = false, bool keep = false, 
			SeedFileStatus status = SeedFileStatus.IGNORE)
		{
			int count = collection.Count;

			foreach (string file in
				Directory.EnumerateFiles(folder, pattern, options))
			{
				FilePath < FileNameSimpleSelectable > seedFile =
					new FilePath<FileNameSimpleSelectable>(file);

				string sampleFile = GetSampleFile(seedFile);

				ConfigSeedFile seed =
					MakeConfigSeedFileItem(seedFile, Heading.SuiteName,
						sampleFile);

				seed.Selected = select;
				seed.Keep = keep;
				seed.Local = local;
				seed.Status = status;

				collection.Add(seed);
			}
			return collection.Count - count > 0;
		}

		// public static bool GetFiles(ObservableCollection<ConfigSeedFile> collection,
		// 	string folder)
		// {
		// 	return GetFiles(
		// 		collection, folder, SEED_PATTERN, SearchOption.AllDirectories);
		// }

		public static ConfigSeedFile MakeConfigSeedFileItem(
			FilePath<FileNameSimpleSelectable> file, string suiteName,
			string sampleFile)
		{
			return new ConfigSeedFile(
				file.GetFileNameWithoutExtension,
				suiteName,
				file.GetPath,
				file.GetFileName,
				sampleFile, false, false, file.GetFileNameObject.Selected, SeedFileStatus.IGNORE);
		}

		public static string GetSampleFile(FilePath<FileNameSimpleSelectable> file)
		{
			return GetSampleFile(file.GetPath, file.GetFileNameObject.Name);
		}

		public static string GetSampleFile(string path, string fileNameNoExt, bool isTarget = false)
		{
			string sampleFile = path + @"\" + fileNameNoExt + ASSOC_FILE_EXT;

			if (!isTarget && !File.Exists(sampleFile))
			{
				sampleFile = null;
			}

			return sampleFile;
		}

	}
}

