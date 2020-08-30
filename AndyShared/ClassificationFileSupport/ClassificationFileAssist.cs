#region using

using System;
using System.IO;
using AndyShared.ConfigMgrShared;
using AndyShared.FileSupport;
using AndyShared.SampleFileSupport;
using AndyShared.Support;
using Microsoft.WindowsAPICodePack.Dialogs;
using SettingsManager;
using UtilityLibrary;

using static AndyShared.FileSupport.FileNameUserAndId;

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


		public static FilePath<FileNameSimple> GetSampleFilePathFromFile(string classfFilePath)
		{
			string sampleFileNameNoExt = SampleFileNameFromFile(classfFilePath);

			if (sampleFileNameNoExt.IsVoid()) return null;

			FilePath<FileNameSimple> sampleFilePath = DeriveSampleFolderPath(classfFilePath);

			sampleFilePath.ChangeFileName(sampleFileNameNoExt, SampleFile.SAMPLE_FILE_EXT);

			return sampleFilePath;
		}

		public static FilePath<FileNameSimple> DeriveSampleFolderPath(string classfFilePath)
		{
			FilePath<FileNameSimple> sampleFolderPath = new FilePath<FileNameSimple>(classfFilePath);

			sampleFolderPath.Down((SampleFile.SAMPLE_FOLDER));

			return sampleFolderPath;
		}

		public static string SampleFileNameFromFile(string classifFilePath)
		{
			if (classifFilePath == null) return null;

			string fileName = null;

			try
			{
				fileName =
					CsUtilities.ScanXmlForElementValue(classifFilePath, "SampleFile", 0);
			}
			catch (Exception e)
			{
				string m = e.Message;
				string im = e.InnerException?.Message ?? null;
			}

			return fileName;
		}

		public static FilePath<FileNameUserAndId> AssembleClassfFilePath(string newFileId, params string[] folders)
		{
			return new FilePath<FileNameUserAndId>(
				FilePathUtil.AssembleFilePath(AssembleFileNameNoExt(Environment.UserName, newFileId),
					ClassificationFile.CLASSF_FILE_EXT_NO_SEP, folders));
		}

		public static bool Duplicate(FilePath<FileNameUserAndId> source, string newFileId)
		{
			FilePath<FileNameUserAndId> dest = 
				ClassificationFileAssist.AssembleClassfFilePath(newFileId, source.FolderPath);
			//
			// new FilePath<FileNameUserAndId>(
			// FilePathUtil.AssemblePath(formatFileName(Environment.UserName, newFileId),
			// 	CLASSF_FILE_EXT_NO_SEP, source.FolderPath));

			if (!ValidateProposedClassfFile(  dest,
				false, "Duplicate a Classification File", "already exists")) return false;

			if (!FileUtilities.CopyFile(source.FullFilePath, dest.FullFilePath)) return false;

			BaseDataFile<ClassificationFileData>  df =
				new BaseDataFile<ClassificationFileData>();
			df.Configure(dest.FolderPath, dest.FileName);
			df.Admin.Read();

			if (!df.Info.Description.IsVoid())
			{
				df.Info.Description = "COPY OF " + df.Info.Description;
			}
			else
			{
				df.Info.Description = "This file holds the PDF sheet classification information";
			}

			if (!df.Info.Notes.IsVoid())
			{
				df.Info.Notes = "COPY OF " + df.Info.Notes;
			}
			else
			{
				df.Info.Notes = dest.FileNameObject.UserName + " created this file on " + DateTime.Now;
			}

			df.Admin.Write();

			df = null;

			return true;
		}

		public static ClassificationFile Create(string classfRootFolderPath)
		{
			FilePath<FileNameSimple> dest = 
				FileUtilities.UniqueFileName(AssembleFileNameNoExt(Environment.UserName, "Pdf Classfications {0:D3}"),
					"xml", classfRootFolderPath + FilePathUtil.PATH_SEPARATOR + Environment.UserName);

			if (dest == null) return null;

			BaseDataFile<ClassificationFileData> df =
				new BaseDataFile<ClassificationFileData>();

			df.Configure(dest.FolderPath, dest.FileName);
			df.Admin.Read();
			df.Info.Description = "This file holds the PDF sheet classification information";
			df.Info.Notes = Environment.UserName + " created this file on " + DateTime.Now;

			df.Admin.Write();

			return new ClassificationFile(dest.FullFilePath);
		}

		public static string Rename(FilePath<FileNameUserAndId> source, string newFileId)
		{
			FilePath<FileNameUserAndId> dest =
				AssembleClassfFilePath(newFileId, source.FolderPath);

			if (!ValidateProposedClassfFile( dest, false,
				"Rename a Classification File", "already exists")) return null;

			try
			{
				File.Move(source.FullFilePath, dest.FullFilePath);
			}
			catch (Exception e)
			{
				string m = e.Message;
				string i = e.InnerException.Message;
				return null;
			}

			return dest.FileNameNoExt;
		}

		public static bool Delete(string sourceFilePath)
		{
			if (!File.Exists(sourceFilePath)) return false;

			try
			{
				File.Delete(sourceFilePath);
			}
			catch (Exception e)
			{
				string m = e.Message;
				string i = e.InnerException?.Message;

				return false;

			}
			return true;
		}

		/// <summary>
		/// Check if the proposed classification file exists and 
		/// if so, provide a dialog to tell the user
		/// </summary>
		/// <returns>
		/// true if the proposed classification file DOES NOT exist<br/>
		/// false if  the proposed classification file DOES exist
		/// </returns>
		/// <param name="fp">The FilePath for the proposed classification file</param>
		/// <param name="test"></param>
		/// <param name="title">The error dialog box's title</param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public static bool ValidateProposedClassfFile(FilePath<FileNameUserAndId> fp, 
			bool test, string title, string msg)
		{
			if (fp.IsFound == test) return true;

			TaskDialog td = new TaskDialog();
			td.Caption = title;
			td.Text = "The classification File Id provided: \"" 
				//+ fileId +
				+ fp.FileNameObject.FileId +
				"\" "
				+ msg
				+ ".  Please provide a different File Id";
			td.InstructionText = "The classification file already exists";
			td.Icon = TaskDialogStandardIcon.Error;
			td.Cancelable = false;
			td.OwnerWindowHandle = ScreenParameters.GetWindowHandle(Common.GetCurrentWindow());
			td.StartupLocation = TaskDialogStartupLocation.CenterOwner;
			td.Opened += Common.TaskDialog_Opened;
			td.Show();

			return false;
		}

		// take an existing file and adjust to be a sample file
		// proposed sample file must have an extension of ".dat"
		// /// <summary>
		// /// take an existing file and adjust to be a sample file<br/>
		// /// proposed sample file must have an extension of ".sample"
		// /// </summary>
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


		// public static string IncorporateSampleFile(string existFilePath, 
		// 	string classfFolderPath, string classfFileNameNoExt)
		// {
		// 	string newFilePath = classfFolderPath + FilePathUtil.EXT_SEPARATOR +
		// 				MakeSampleFileName(classfFileNameNoExt);
		//
		// 	bool result = CopyFile(existFilePath, newFilePath);
		//
		// 	if (result) return newFilePath;
		//
		// 	return null;
		// }


	#endregion

	#region private methods

		// private static string CopyFileToTemp(string existFile, string newFolderPath)
		// {
		// 	string tempFilePath = GetTempFileName(newFolderPath);
		//
		// 	if (tempFilePath == null) return null;
		//
		// 	try
		// 	{
		// 		File.Copy(existFile, tempFilePath);
		// 	}
		// 	catch
		// 	{
		// 		return null;
		// 	}
		//
		//
		// 	return tempFilePath;
		// }

		// private static string GetTempFileName(string newFolderPath)
		// {
		// 	string tempFileName;
		// 	string tempFilePath;
		//
		// 	for (int i = 0; i < 99; i++)
		// 	{
		// 		tempFileName = $"AndyTempFile{i:D3}.tmp";
		// 		tempFilePath = newFolderPath + FilePathUtil.PATH_SEPARATOR + tempFileName;
		//
		// 		if (!File.Exists(tempFilePath))
		// 		{
		// 			return tempFilePath;
		// 		}
		// 	}
		//
		// 	return null;
		// }

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