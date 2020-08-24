#region using

using System;
using System.IO;
using AndyShared.ConfigMgrShared;
using AndyShared.FilesSupport;
using AndyShared.SampleFileSupport;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  8/2/2020 5:33:56 PM

namespace AndyShared.ClassificationFileSupport
{
	public class ClassificationFileAssist
	{
	#region public fields

		public const string USER_STORAGE_FOLDER_NAME = @"User Classification Files";
		public const string USER_STORAGE_PATTERN = @"*.xml";
		public const string USER_STORAGE_FOLDER = FilePathUtil.PATH_SEPARATOR + USER_STORAGE_FOLDER_NAME;

	#endregion


	#region private fields

	#endregion

	#region ctor

		public ClassificationFileAssist() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static string MakeSampleFileName(string classifFileNameNoExt)
		{
			return classifFileNameNoExt + SampleFile.SAMPLE_FILE_EXT;
		}

		public static string MakeSampleFileName(string username, string id)
		{
			return $"({username}) {id}" + SampleFile.SAMPLE_FILE_EXT;
		}

		public static string MakeClassificationFileNameNoExt(string username, string id)
		{
			return $"({username}) {id}";
		}

		public static string MakeClassificationFileName(string username, string id)
		{
			return MakeClassificationFileNameNoExt(username, id) + ConfigSeedFileSupport.INFO_FILE_EXT;
		}

		public static string GetSampleFile(FilePath<FileNameSimpleSelectable> file)
		{
			return GetSampleFile(file.FolderPath, file.FileNameObject.FileNameNoExt);
		}

		public static string GetSampleFile(string path, string fileNameNoExt, bool isTarget = false)
		{
			string sampleFile = path + @"\" + fileNameNoExt + SampleFile.SAMPLE_FILE_EXT;

			if (!isTarget && !File.Exists(sampleFile))
			{
				sampleFile = null;
			}

			return sampleFile;
		}


		// take an existing file and adjust to be a sample file
		// proposed sample file must have an extension of ".dat"
		/// <summary>
		/// take an existing file and adjust to be a sample file<br/>
		/// proposed sample file must have an extension of ".sample"
		/// </summary>
		// /// <param name="existFilePath"></param>
		// /// <param name="userClassfFileFolderPath"></param>
		// /// <returns></returns>
		// public static string IncorporateSampleFile(string existFilePath, string classfFilePath)
		// {
		// 	FilePath<FileNameSimple> eFilePath = new FilePath<FileNameSimple>(existFilePath);
		//
		// 	FilePath<FileNameSimple> cFilePath = new FilePath<FileNameSimple>(classfFilePath);
		//
		// 	FilePath<FileNameSimple> nFilePath =
		// 		new FilePath<FileNameSimple>(
		// 			cFilePath.GetPath + FilePathUtil.EXT_SEPARATOR +
		// 			MakeSampleFileName(cFilePath.GetFileNameWithoutExtension));
		//
		// 	// step 1 - validate - existing file path cannot equal new file path
		// 	if (eFilePath.Equals(nFilePath)) return null;
		//
		// 	// step 2 - validate - new file cannot exist
		// 	if (File.Exists(nFilePath.GetFullFilePath)) return null;
		//
		// 	// step 3 - copy file to new name
		// 	File.Copy(eFilePath.GetFullFilePath, nFilePath.GetFullFilePath);
		//
		// 	return nFilePath.GetFullFilePath;
		// }


		public static string IncorporateSampleFile(string existFilePath, 
			string classfFolderPath, string classfFileNameNoExt)
		{
			string newFilePath = classfFolderPath + FilePathUtil.EXT_SEPARATOR +
						MakeSampleFileName(classfFileNameNoExt);

			bool result = CopyFile(existFilePath, newFilePath);

			if (result) return newFilePath;

			return null;
		}



		public static bool CopyFile(string eFilePath, string nFilePath)
		{
			// step 1 - validate - existing file path cannot equal new file path
			if (eFilePath.Equals(nFilePath)) return false;

			// step 2 - validate - new file cannot exist
			if (File.Exists(nFilePath)) return false;

			// step 3 - copy file to new name
			File.Copy(eFilePath, nFilePath);

			return true;
		}

	#endregion

	#region private methods

		private static string CopyFileToTemp(string existFile, string newFolderPath)
		{
			string tempFilePath = GetTempFileName(newFolderPath);

			if (tempFilePath == null) return null;

			try
			{
				File.Copy(existFile, tempFilePath);
			}
			catch
			{
				return null;
			}


			return tempFilePath;
		}

		private static string GetTempFileName(string newFolderPath)
		{
			string tempFileName;
			string tempFilePath;

			for (int i = 0; i < 99; i++)
			{
				tempFileName = $"AndyTempFile{i:D3}.tmp";
				tempFilePath = newFolderPath + FilePathUtil.PATH_SEPARATOR + tempFileName;

				if (!File.Exists(tempFilePath))
				{
					return tempFilePath;
				}
			}

			return null;
		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ClassificationFileAssist";
		}

	#endregion
	}
}