#region using

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using AndyShared.ClassificationDataSupport.TreeSupport;
using SettingsManager;
using UtilityLibrary;
using AndyShared.FileSupport;
using AndyShared.Settings;
using AndyShared.Support;
using static AndyShared.FileSupport.FileNameUserAndId;
using DebugCode;

#endregion

// username: jeffs
// created: 8/2/2020 5:33:56 PM

namespace AndyShared.ClassificationFileSupport
{
	/* moved to ClassificationFile

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

		/// <summary>
		/// gets an existing ClassificationFile for the specific user based on
		/// the fileId.  ClassificationFile object is configured but no data is
		/// read.  use ".Initialize()" to read and process the data in the file
		/// </summary>
		public static ClassificationFile GetUserClassfFile(string fileId)
		{
		#if DML1
			DM.InOut0();
		#endif

			if (fileId.IsVoid()) return null;

			return new ClassificationFile(ClassificationFileAssist.AssembleClassfFilePath(fileId,
				SettingsSupport.UserClassifFolderPath.FullFilePath).FullFilePath);
		}

		public static FilePath<FileNameUserAndId> AssembleClassfFilePath(string newFileId, params string[] folders)
		{
		#if DML1
			DM.InOut0();
		#endif

			return new FilePath<FileNameUserAndId>(
				FilePathUtil.AssembleFilePathS(AssembleFileNameNoExt(Environment.UserName, newFileId),
					FilePathConstants.CLASSF_FILE_EXT_NO_SEP, folders));
		}
		
		public static bool Exists(string fileId)
		{
			return ClassificationFileAssist
			.AssembleClassfFilePath(fileId, SettingsSupport.UserClassifFolderPath.FullFilePath).Exists;
		}

		public static bool Duplicate(FilePath<FileNameUserAndId> source, string newFileId)
		{
			FilePath<FileNameUserAndId> dest =
				ClassificationFileAssist.AssembleClassfFilePath(newFileId, source.FolderPath);

			if (!ValidateProposedClassfFile( dest,
					false, "Duplicate a Classification File", "already exists")) return false;

			if (!FileUtilities.CopyFile(source.FullFilePath, dest.FullFilePath)) return false;

			DataManager<ClassificationFileData> df =
				new DataManager<ClassificationFileData>(null);

			// df.Configure(dest.FolderPath, dest.FileName);
			df.Configure(dest.FolderPath, dest.FileNameNoExt, null, dest.FileExtensionNoSep);
			df.Admin.Read();

			// string x = UserSettings.Admin.ToString();

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

		/// <summary>
		/// creates a ClassificationFile in the folder provided using the<br/>
		/// default name of {Pdf Classfications xxx.xml} where xxx is the next available file number
		/// </summary>
		public static ClassificationFile Create(string fileId = null, string classfRootFolderPath = null)
		{
			string path = classfRootFolderPath.IsVoid()
				? SettingsSupport.AllClassifFolderPath.FullFilePath
				: classfRootFolderPath;

			FilePath<FileNameUserAndId> dest;

			if (fileId.IsVoid())
			{
				dest =
					FileUtilities.UniqueFileName(AssembleFileNameNoExt(Environment.UserName, "Pdf Classfications {0:D3}"),
						FilePathConstants.CLASSF_FILE_EXT_NO_SEP, path + FilePathUtil.PATH_SEPARATOR + Environment.UserName);
			}
			else
			{
				dest = AssembleClassfFilePath(fileId, path);
			}

			if (dest == null) return null;

		#if SM74
			// quick fix - may need to provide a path rather than null

			DataManager<ClassificationFileData> df =
				new DataManager<ClassificationFileData>(null, dest.FolderPath, dest.FileName);
			#else
			BaseDataFile<ClassificationFileData> df =
				new BaseDataFile<ClassificationFileData>();
		#endif

			df.Configure(dest.FolderPath, dest.FileName, null, dest.FileExtensionNoSep);
			df.Admin.Read();
			df.Info.Description = "This file holds the PDF sheet classification information";
			df.Info.Notes = Environment.UserName + " created this file on " + DateTime.Now;

			df.Data.BaseOfTree.Initalize();

			TreeNode tn = new TreeNode(df.Data.BaseOfTree, new SheetCategory("Initial Item", "Initial Item"), false);

			df.Data.BaseOfTree.AddNode(tn);
			
			// df.Data.BaseOfTree.Item = new SheetCategory("Base of Tree", "Base of Tree");

			df.Admin.Write();

			string d = dest.FullFilePath;

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
		/// false if the proposed classification file DOES exist
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

			CommonTaskDialogs.CommonErrorDialog(title,
				"The classification file already exists",
				"The classification File Id provided: \"");

			// TaskDialog td = new TaskDialog();
			// td.Caption = title;
			// td.Text = "The classification File Id provided: \"" 
			// 	//+ fileId +
			// 	+ fp.FileNameObject.FileId +
			// 	"\" "
			// 	+ msg
			// 	+ ". Please provide a different File Id";
			// td.InstructionText = "The classification file already exists";
			// td.Icon = TaskDialogStandardIcon.Error;
			// td.Cancelable = false;
			// td.OwnerWindowHandle = ScreenParameters.GetWindowHandle(Common.GetCurrentWindow());
			// td.StartupLocation = TaskDialogStartupLocation.CenterOwner;
			// td.Opened += Common.TaskDialog_Opened;
			// td.Show();

			return false;
		}


		public static FilePath<FileNameSimple> GetSampleFilePathFromFile(string classfFilePath)
		{
		#if DML1
			DM.Start0();
		#endif

			string sampleFileNameNoExt = SampleFileNameFromFile(classfFilePath);

			if (sampleFileNameNoExt.IsVoid()) return null;

			FilePath<FileNameSimple> sampleFilePath = DeriveSampleFolderPath(classfFilePath);

			sampleFilePath.ChangeFileName(sampleFileNameNoExt, FilePathConstants.SAMPLE_FILE_EXT);

		#if DML1
			DM.End0();
		#endif

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

	*/
}