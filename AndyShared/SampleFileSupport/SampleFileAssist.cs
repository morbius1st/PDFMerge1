#region using

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using CSLibraryIo.CommonFileFolderDialog;
using Microsoft.WindowsAPICodePack.Dialogs;
using UtilityLibrary;
using AndyShared.FileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.Support;
// using ClassifierEditor.Windows;
// using WpfShared.Windows;

#endregion

// username: jeffs
// created:  8/28/2020 10:00:21 PM

namespace AndyShared.SampleFileSupport
{
	public static class SampleFileAssist
	{
		public const string SAMPLE_FILE_PATTERN = @"*." + FileLocationSupport.SAMPLE_FILE_EXT;

	#region private fields

	#endregion

	#region ctor

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static FilePath<FileNameSimple> InstallSampleFile(string startingFolderPath, string sampleFolderPath)
		{
			FilePath<FileNameSimple> existFilePath =  SelectSampleFile(startingFolderPath);
			FilePath<FileNameSimple> newFilePath = 
				new FilePath<FileNameSimple>(sampleFolderPath + FilePathUtil.PATH_SEPARATOR + existFilePath.FileName);

			if (!newFilePath.IsValid || newFilePath.IsFound) return null;

			bool result = FileUtilities.CopyFile(existFilePath.FullFilePath, newFilePath.FullFilePath);

			if (!result) return null;

			return newFilePath;
		}

		public static string DescriptionFromFile(string sampleFilePath)
		{
			if (sampleFilePath == null)  return null;

			string description = null;

			try
			{
				description = CsXmlUtilities.ScanXmlForElementValue(sampleFilePath, "Description", 0);
			}
			catch (Exception e)
			{
				string m = e.Message;
				string im = e.InnerException?.Message ?? null;
			}

			return description;
		}
		
		public static string BuildingFromFile(string sampleFilePath)
		{
			if (sampleFilePath == null)  return null;

			string building = null;

			try
			{
				building = CsXmlUtilities.ScanXmlForElementValue(sampleFilePath, "Building", 0);
			}
			catch (Exception e)
			{
				string m = e.Message;
				string im = e.InnerException?.Message ?? null;
			}

			return building;
		}

		/// <summary>
		/// get a sample file's filepath based on the folderpath<br/>
		/// of the provided file
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static string GetSampleFile(FilePath<FileNameSimpleSelectable> file)
		{
			return GetSampleFile(file.FolderPath, file.FileNameObject.FileNameNoExt);
		}

		/// <summary>
		/// assemble a sample file's filepath<br/>
		/// to only get filepath to existing sample files, set target to true
		/// </summary>
		/// <param name="path"></param>
		/// <param name="fileNameNoExt"></param>
		/// <param name="isTarget"></param>
		/// <returns></returns>
		public static string GetSampleFile(string path, string fileNameNoExt, bool isTarget = false)
		{
			string sampleFile = path + FilePathUtil.PATH_SEPARATOR + fileNameNoExt + FileLocationSupport.SAMPLE_FILE_EXT;

			if (!isTarget && !File.Exists(sampleFile))
			{
				sampleFile = null;
			}

			return sampleFile;
		}

		public static string GetSampleFolderPath(string userClassfFolderPath)
		{
			return userClassfFolderPath + FilePathUtil.PATH_SEPARATOR
				+ FileLocationSupport.SAMPLE_FOLDER;
		}


	#endregion

	#region private methods

		private static FilePath<FileNameSimple> SelectSampleFile(string startingFolder)
		{
			bool status = true;

			FileAndFolderDialog fd;
			FilePath<FileNameSimple> filePath = null;

			while (status)
			{
				fd = new FileAndFolderDialog();
				fd.Title = "Select A Sample File to Install";
				fd.Filters.Add(new CommonFileDialogFilter("Sample Files", SAMPLE_FILE_PATTERN));

				string file = fd.GetFile();

				if (fd.Result == CommonFileDialogResult.Cancel) return null;

				if (file == null) continue;

				filePath = new FilePath<FileNameSimple>(file);

				if (!filePath.FileExtensionNoSep.Equals(FileLocationSupport.SAMPLE_FILE_EXT))
				{
					DialogInvalidSampleFile(filePath.FileNameNoExt);

					continue;
				}

				status = false;
			}

			return filePath;
		}

		private static void DialogInvalidSampleFile(string fileName, IntPtr handle =  default)
		{
			CommonTaskDialogs.CommonErrorDialog(
				"Invalid Sample File",
				"Invalid Sample File",
				"The Sample File selected \"" + (fileName ?? "(unknown)") + "\" " +
				"Cannot be used as it is the wrong file type.  Please select a file of type \""
				+ FileLocationSupport.SAMPLE_FILE_EXT, handle);

			// TaskDialog td = new TaskDialog();
			// td.Caption = "Invalid Sample File";
			// td.InstructionText = "Invalid Sample File";
			// td.Icon = TaskDialogStandardIcon.Error;
			// td.Text = "The Sample File selected \"" + (fileName ?? "(unknown)") + "\" " +
			// 	"Cannot be used as it is the wrong file type.  Please select a file of type \""
			// 	+ FilePathConstants.SAMPLE_FILE_EXT;
			// td.Cancelable = false;
			// td.OwnerWindowHandle = ScreenParameters.GetWindowHandle(Common.GetCurrentWindow());
			// td.StartupLocation = TaskDialogStartupLocation.CenterOwner;
			// td.Opened += Common.TaskDialog_Opened;
			// td.Show();

		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public new static string ToString()
		{
			return "this is SampleFileAssist";
		}

	#endregion
	}
}