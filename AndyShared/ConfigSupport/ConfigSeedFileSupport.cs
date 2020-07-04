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
		public const string SEED_FOLDER = @"\" + SEED_FOLDER_NAME;

		public static bool GetFiles(ObservableCollection<ConfigSeedFile> collection,
			string folder, string pattern, SearchOption options)
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
				false,
				false,
				file.GetFileNameObject.Selected,
				file.GetPath,
				file.GetFileName,
				sampleFile);
		}

		public static string GetSampleFile(FilePath<FileNameSimpleSelectable> file)
		{
			string sampleFile =
				file.GetPath + @"\" + file.GetFileNameObject.Name + @".dat";

			if (!File.Exists(sampleFile))
			{
				sampleFile = null;
			}

			return sampleFile;
		}

	}
}

