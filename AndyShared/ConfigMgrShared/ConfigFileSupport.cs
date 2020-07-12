using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using AndyShared.ConfigSupport;
using AndyShared.FilesSupport;
using SettingsManager;
using UtilityLibrary;

namespace AndyShared.ConfigMgrShared
{
	public static class ConfigFileSupport
	{
		public const string USER_STORAGE_FOLDER_NAME = @"User Classification Files";
		public const string USER_STORAGE_PATTERN = @"*.xml";
		public const string USER_STORAGE_FOLDER = FilePathUtil.PATH_SEPARATOR + USER_STORAGE_FOLDER_NAME;

		public static string UserClassificationFolderPath => 
			SuiteSettings.Path.SettingFolderPath + USER_STORAGE_FOLDER;


	#region public methods

		public static string MakeClassificationFileName(string id, string username)
		{
			return $"{id} ({username})" + ConfigSeedFileSupport.INFO_FILE_EXT;
		}

	#endregion
	}


	public static class ConfigSeedFileSupport
	{
		public const string INFO_FILE_EXT = ".xml";
		public const string ASSOC_FILE_EXT = ".dat";

		public const string SEED_PATTERN = @"*.seed" + INFO_FILE_EXT;
		public const string SEED_FOLDER_NAME = @"Seed Files";
		public const string SEED_FOLDER_SITE = @"\Site " + SEED_FOLDER_NAME;
		public const string SEED_FOLDER_SUITE = @"\Suite " + SEED_FOLDER_NAME;
		public const string SEED_FOLDER_INSTALLED = @"\Installed " + SEED_FOLDER_NAME;

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

				seed.SelectedSeed = select;
				seed.Keep = keep;
				seed.Local = local;
				seed.Status = status;

				collection.Add(seed);
			}
			return collection.Count - count > 0;
		}


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
			return GetSampleFile(file.GetPath, file.GetFileNameObject.FileNameNoExt);
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

