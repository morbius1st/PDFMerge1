using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using AndyShared.ClassificationFileSupport;
using AndyShared.ConfigSupport;
using AndyShared.FilesSupport;
using SettingsManager;
using UtilityLibrary;

namespace AndyShared.ConfigMgrShared
{
	public static class ConfigFileSupport
	{

	#region public methods

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

	#endregion
	}


	public static class ConfigSeedFileSupport
	{
		public const string INFO_FILE_EXT = ".xml";
		public const string SAMPLE_FILE_EXT = ".sample";

		public const string SEED_PATTERN = @"*.seed" + INFO_FILE_EXT;
		public const string SEED_FOLDER_NAME = @"Seed Files";
		public const string SEED_FOLDER_SITE = @"\Site " + SEED_FOLDER_NAME;
		public const string SEED_FOLDER_LOCAL = @"\Local " + SEED_FOLDER_NAME;
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

				string sampleFile = ClassificationFileAssist.GetSampleFile(seedFile);

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
	}
}

