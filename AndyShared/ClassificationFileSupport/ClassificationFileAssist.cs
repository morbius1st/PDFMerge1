#region using

using System;
using System.IO;
using AndyShared.ConfigMgrShared;
using AndyShared.FileSupport;
using AndyShared.SampleFileSupport;
using AndyShared.Settings;
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

	#endregion

	#region private fields

	#endregion

	#region ctor

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static ClassificationFile GetUserClassfFile(string fileId)
		{
			return new ClassificationFile(ClassificationFileAssist.AssembleClassfFilePath(fileId,
				SettingsSupport.UserClassifFolderPath.FullFilePath).FullFilePath);
		}

		public static FilePath<FileNameUserAndId> AssembleClassfFilePath(string newFileId, params string[] folders)
		{
			return new FilePath<FileNameUserAndId>(
				FilePathUtil.AssembleFilePath(AssembleFileNameNoExt(Environment.UserName, newFileId),
					FilePathConstants.CLASSF_FILE_EXT_NO_SEP, folders));
		}

		public static bool Duplicate(FilePath<FileNameUserAndId> source, string newFileId)
		{
			FilePath<FileNameUserAndId> dest = 
				ClassificationFileAssist.AssembleClassfFilePath(newFileId, source.FolderPath);

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
				string i = e.InnerException?.Message;
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


		public static FilePath<FileNameSimple> GetSampleFilePathFromFile(string classfFilePath)
		{
			string sampleFileNameNoExt = SampleFileNameFromFile(classfFilePath);

			if (sampleFileNameNoExt.IsVoid()) return null;

			FilePath<FileNameSimple> sampleFilePath = DeriveSampleFolderPath(classfFilePath);

			sampleFilePath.ChangeFileName(sampleFileNameNoExt, FilePathConstants.SAMPLE_FILE_EXT);

			return sampleFilePath;
		}

		public static FilePath<FileNameSimple> DeriveSampleFolderPath(string classfFilePath)
		{
			FilePath<FileNameSimple> sampleFolderPath = new FilePath<FileNameSimple>(classfFilePath);

			sampleFolderPath.Down((FilePathConstants.SAMPLE_FOLDER));

			return sampleFolderPath;
		}

		public static string SampleFileNameFromFile(string classifFilePath)
		{
			if (classifFilePath == null) return null;

			string fileName = null;

			try
			{
				fileName =
					CsXmlUtilities.ScanXmlForElementValue(classifFilePath, "SampleFile", 0);
			}
			catch (Exception e)
			{
				string m = e.Message;
				string im = e.InnerException?.Message ?? null;
			}

			return fileName;
		}


	#endregion

	#region private methods

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