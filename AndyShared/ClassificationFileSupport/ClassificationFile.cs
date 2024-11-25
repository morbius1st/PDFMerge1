// #define SHOWTICKS

#define DML1

#region using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AndyShared.ClassificationDataSupport.SheetSupport;
using AndyShared.ClassificationDataSupport.TreeSupport;
using SettingsManager;
using UtilityLibrary;
using AndyShared.FileSupport;
using AndyShared.SampleFileSupport;
using AndyShared.Settings;
using AndyShared.Support;
using DebugCode;
using JetBrains.Annotations;
using static AndyShared.FileSupport.FileNameUserAndId;
using static AndyShared.ClassificationDataSupport.SheetSupport.SheetCategory;
using static AndyShared.ClassificationDataSupport.SheetSupport.ItemClassDef;

#endregion

// username: jeffs
// created:  8/2/2020 10:24:57 PM

namespace AndyShared.ClassificationFileSupport
{
	[DataContract(Namespace = "")]
	public class ClassificationFile : INotifyPropertyChanged
	{
		


		public static int M_IDX = 1000;

	#region private fields

		private FilePath<FileNameUserAndId> filePathLocal;

		private SampleFile sampleFile;

		private DataManager<ClassificationFileData> dataFile;
		// #if SM74
		// #else
		// 	// private BaseDataFile<ClassificationFileData> dataFile;
		// #endif

		private bool isSelected;
		private bool isModified;
		private bool isInitialized = false;
		private bool isDefaultClassfFile = true;
		private bool isUserClassfFile = false;

	#endregion

	#region ctor

		// public ClassificationFile(string filePath, bool fileSelected = false)
		public ClassificationFile(string filePath)
		{
		#if DML1
			DM.Start0();
		#endif

			FilePathLocal = new FilePath<FileNameUserAndId>(filePath);

			if (!filePathLocal.IsValid) return;

			// bool a = !SettingsSupport.ValidateXmlFile(filePath);
			// bool b = !ValidateAgainstUsername(filePathLocal);


			if ((FilePathLocal.IsFound && !SettingsSupport.ValidateXmlFile(filePath))
				|| !ValidateAgainstUsername(filePathLocal)
				)
			{
				FilePathLocal = FilePath<FileNameUserAndId>.Invalid;

			#if DML1
				DM.End0("end 1");
			#endif

				return;
			}

			PreInitialize();

			InitailizeSample(FilePathLocal.FullFilePath);

		#if DML1
			DM.End0();
		#endif
		}

	#endregion

	#region public properties

		// // classification file information
		public BaseOfTree TreeBase => dataFile.Data.BaseOfTree;

		public ClassificationFileData Data => dataFile.Data;

		// tied to xml data file
		public string HeaderDescFromMemory
		{
			get => dataFile.Info.Description;
			set
			{
				if (value?.Equals(dataFile.Info.Description) ?? false) return;

				dataFile.Info.Description = value;

				// dataFile.Admin.Write();

				OnPropertyChanged();

				IsModified = true;
			}
		}

		public string HeaderDescFromFile => CsXmlUtilities.ScanXmlForElementValue(FullFilePath, "Description", 0);

		public string HeaderNote
		{
			get => dataFile.Info.Notes;
			set
			{
				if (value?.Equals(dataFile.Info.Notes) ?? false) return;

				dataFile.Info.Notes = value;

				// dataFile.Admin.Write();

				OnPropertyChanged();

				IsModified = true;
			}
		}


		public string FolderPathLocal
		{
			get
			{
				FilePath<FileNameUserAndId> f = filePathLocal;

				return filePathLocal.FolderPath;
			}
		}

		public string FullFilePath => filePathLocal.FullFilePath;

		public FilePath<FileNameUserAndId> FilePathLocal
		{
			get => filePathLocal;

			set
			{
				filePathLocal = value;

				OnPropertyChanged();
			}
		}

		/// <summary>
		/// The whole file name - name + extension
		/// </summary>
		public string FileName => filePathLocal.FileName;

		public string FileNameNoExt => FilePathLocal.FileNameObject.FileNameNoExt;

		public string FileNameExtNoSep => FilePathLocal.FileNameObject.ExtensionNoSep;

		public string UserName
		{
			get => filePathLocal.FileNameObject.UserName;
		}

		public string FileId
		{
			get => filePathLocal.FileNameObject.FileId;

			// this renames the the associated classification file
			set
			{
				Debug.WriteLine("@fileId| updated");

				return;

				if (value.Equals(FileId)) return;

				string newFileNameNoExt = AssembleFileNameNoExt(UserName, value);

				filePathLocal.ChangeFileName(newFileNameNoExt);

				OnPropertyChanged();
				OnSelectedPropertyChange();
				OnPropertyChanged("SampleFilePath");
				OnPropertyChanged("FileName");
				OnPropertyChanged("GetFullFilePath");
			}
		}

		/// <summary>
		/// The folder path to the user's specific configuration files
		/// </summary>
		public string FolderPath => FilePathLocal.FolderPath;

		// sample
		public string SampleFilePath
		{
			get => sampleFile?.FullFilePath;
			set
			{
				// sampleFile = new SampleFile(value);
				sampleFile = new SampleFile();
				sampleFile.InitializeFromClassfFilePath(value);

			#if DEBUG
				if (sampleFile.SampleFilePath == null)
				{
					sampleFile.SampleFilePath = new FilePath<FileNameSimple>(value);
				}
			#endif

				UpdateSampleFileProperties();
			}
		}

		public string SampleFileName => sampleFile?.SampleFilePath?.FileName ?? null;

		public string SampleFileNameNoExt => sampleFile?.SampleFilePath?.FileNameNoExt ?? "";

		public string SampleFileFolderPath => sampleFile.SampleFilePath?.FolderPath;


		// status

		public bool CanSave => IsModified || TreeBase.TreeNodeModified || TreeBase.TreeNodeChildItemModified;

		public bool CanEdit => IsInitialized && IsUserClassfFile;

		public bool IsInitialized
		{
			get => isInitialized;
			private set
			{
				isInitialized = value;

				OnPropertyChanged();
				OnPropertyChanged("CanEdit");
			}
		}

		/// <summary>
		/// flag noting that this object has been modified
		/// </summary>
		public bool IsModified
		{
			get => isModified;
			set
			{
				if (value == isModified) return;

				isModified = value;
				OnPropertyChanged();
			}
		}

		public bool IsValid => FilePathLocal?.IsValid ?? false;


		// public bool IsSelected
		// {
		// 	get => isSelected;
		// 	set
		// 	{
		// 		isSelected = value;
		//
		// 		OnPropertyChanged();
		// 	}
		// }

	#endregion

	#region private properties

		private bool IsDefaultClassfFile
		{
			get => isDefaultClassfFile;
			set
			{
				isDefaultClassfFile = value;

				OnPropertyChanged();
				OnPropertyChanged("CanEdit");
			}
		} // this will pre-configure to disallow editing

		private bool IsUserClassfFile
		{
			get => isUserClassfFile;
			set
			{
				isUserClassfFile = value;

				OnPropertyChanged();
				OnPropertyChanged("CanEdit");
			}
		}

	#endregion

	#region public methods

		public void UpdateSampleFile(string sampleFile)
		{
			dataFile.Data.SampleFile = sampleFile;
			dataFile.Admin.Write();


			InitailizeSample(FilePathLocal.FullFilePath);
		}

		private void PreInitialize()
		{
		#if DML1
			DM.Start0();
		#endif
			FilePath<FileNameSimple> filePath =
				new FilePath<FileNameSimple>($"{FolderPath}\\{FileNameNoExt}.{FileNameExtNoSep}");

			dataFile = new DataManager<ClassificationFileData>(filePath);

			// #if SM74
			// #else
			// 	// dataFile = new BaseDataFile<ClassificationFileData>();
			// 	// dataFile.Configure(FolderPath, FileNameNoExt, null, FileNameExtNoSep);
			// #endif

		#if DML1
			DM.End0();
		#endif
		}

		public void Initialize2()
		{
		#if DML1
			DM.Start0();
		#endif

			// PreInitialize();

			dataFile.Admin.Read();

			initialize();

		#if DML1
			DM.End0();
		#endif
		}

		public void Initialize()
		{
		#if DML1
			DM.Start0();
		#endif

			PreInitialize();

			dataFile.Admin.Read();

			initialize();

		#if DML1
			DM.End0();
		#endif
		}

		private void initialize()
		{
			dataFile.Data.BaseOfTree.Initalize();

			IsInitialized = true;

			UpdateProperties();

			TreeBase.TreeItemModified += TreeBaseOnTreeItemModified;
			TreeBase.TreeModified     += TreeBaseOnTreeModified;
		}

		public void Write(bool createBackup = true)
		{
			if (createBackup)
			{
				string s1 = dataFile.Path.SettingFilePath;
				string s2 = dataFile.Path.FileNameNoExt;
				string s3 = dataFile.Path.FileName;
				string s4 = dataFile.Path.SettingFolderPath;

				// string filename = dataFile.Path.
				string newFileName = dataFile.Path.FileNameNoExt + FilePathUtil.EXT_SEPARATOR
					+ FilePathUtil.BACKUP_EXTNOSEP;

				string newFilePath = dataFile.Path.SettingFolderPath + FilePathUtil.PATH_SEPARATOR + newFileName;

				File.Copy(dataFile.Path.SettingFilePath, newFilePath, true);
			}

			dataFile.Admin.Write();
		}

		public void UpdateProperties()
		{
			OnPropertyChanged("IsValid");

			OnPropertyChanged("FileName");
			OnPropertyChanged("FileNameNoExt");

			OnPropertyChanged("SampleFilePath");

			OnPropertyChanged("TreeBase");
		}

	#endregion

	#region public static methods

		/// <summary>
		/// creates a ClassificationFile in the folder provided using the<br/>
		/// default name of {Pdf Classfications xxx.xml} where xxx is the next available file number
		/// </summary>
		public static ClassificationFile Create(string fileId = null, string classfRootFolderPath = null)
		{
			string path = classfRootFolderPath.IsVoid()
				? FileLocationSupport.AllClassifFolderPath.FullFilePath
				: classfRootFolderPath;

			FilePath<FileNameUserAndId> dest;

			if (fileId.IsVoid())
			{
				dest =
					FileUtilities.UniqueFileName(
						AssembleFileNameNoExt(Environment.UserName, "Pdf Classfications {0:D3}"),
						FileLocationSupport.DATA_FILE_EXT,
						path + FilePathUtil.PATH_SEPARATOR + Environment.UserName);
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

		public static ClassificationFile Open(string fileid)
		{
			return GetUserClassfFile(fileid);
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

		public static bool Exists(string fileId)
		{
			return ClassificationFile
			.AssembleClassfFilePath(fileId, FileLocationSupport.UserClassifFolderPath.FullFilePath).Exists;
		}

		public static bool Duplicate(FilePath<FileNameUserAndId> source, string newFileId)
		{
			FilePath<FileNameUserAndId> dest =
				ClassificationFile.AssembleClassfFilePath(newFileId, source.FolderPath);

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
		/// gets an existing ClassificationFile for the specific user based on
		/// the fileId.  ClassificationFile object is configured but no data is
		/// read.  use ".Initialize()" to read and process the data in the file
		/// </summary>
		public static ClassificationFile GetUserClassfFile(string fileId)
		{
		#if DML1
			DM.Start0(false, $"geting file id = {fileId ?? "null"}");
		#endif

			if (fileId.IsVoid()) return null;

			ClassificationFile c = new ClassificationFile(ClassificationFile.AssembleClassfFilePath(fileId,
				FileLocationSupport.UserClassifFolderPath.FullFilePath).FullFilePath);

		#if DML1
			DM.End0();
		#endif

			return c;
		}

		public static FilePath<FileNameUserAndId> AssembleClassfFilePath(string newFileId, params string[] folders)
		{
		#if DML1
			DM.InOut0();
		#endif

			return new FilePath<FileNameUserAndId>(
				FilePathUtil.AssembleFilePathS(AssembleFileNameNoExt(Environment.UserName, newFileId),
					FileLocationSupport.DATA_FILE_EXT, folders));
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
			bool test, string title, string msg, IntPtr handle =  default)
		{
			if (fp.IsFound == test) return true;

			CommonTaskDialogs.CommonErrorDialog(title,
				"The classification file already exists",
				"The classification File Id provided: \"",
				handle);

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
			DM.InOut0();
		#endif

			string sampleFileNameNoExt = SampleFileNameFromFile(classfFilePath);

			if (sampleFileNameNoExt.IsVoid()) return null;

			FilePath<FileNameSimple> sampleFilePath = DeriveSampleFolderPath(classfFilePath);

			sampleFilePath.ChangeFileName(sampleFileNameNoExt, FileLocationSupport.SAMPLE_FILE_EXT);

			// #if DML1
			// 	DM.End0();
			// #endif

			return sampleFilePath;
		}

		public static FilePath<FileNameSimple> DeriveSampleFolderPath(string classfFilePath)
		{
			FilePath<FileNameSimple> sampleFolderPath = new FilePath<FileNameSimple>(classfFilePath);

			sampleFolderPath.Down((FileLocationSupport.SAMPLE_FOLDER));

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

		private void InitailizeSample(string classfFilePath)
		{
		#if DML1
			DM.Start0();
		#endif
			sampleFile = new SampleFile();

			sampleFile.InitializeFromClassfFilePath(classfFilePath);

			UpdateSampleFileProperties();

		#if DML1
			DM.End0();
		#endif
		}

		private bool ValidateAgainstUsername(FilePath<FileNameUserAndId> filePath)
		{
			IsDefaultClassfFile =
				filePath.FileNameObject.UserName.Equals("default", StringComparison.OrdinalIgnoreCase);

			IsUserClassfFile =
				filePath.FileNameObject.UserName.Equals(Environment.UserName, StringComparison.OrdinalIgnoreCase);

			return IsUserClassfFile || IsDefaultClassfFile;
		}

		private void UpdateSampleFileProperties()
		{
			OnPropertyChanged("SampleFilePath");
			OnPropertyChanged("SampleFileName");
		}

	#endregion

	#region event consuming

		private void TreeBaseOnTreeModified(object sender)
		{
			OnPropertyChanged(nameof(CanSave));
		}

		private void TreeBaseOnTreeItemModified(object sender)
		{
			OnPropertyChanged(nameof(CanSave));
		}


		// private void OnAnnounceTnInitx(object sender, object value)
		// {
		// 	isInitialized = true;
		// 	IsModified = false;
		// }
		//
		// // one of the sub-children have been modified
		// private void OnAnnounceSubChildModifiedx(object sender, object value)
		// {
		// 	IsModified = true;
		//
		// 	if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ classfFile|@ onmodified| who| " + sender.ToString() + "| value| " + value);
		// }
		//
		// private void OnAnnounceSavedx(object sender, object value)
		// {
		// 	IsModified = false;
		// }

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public event PropertyChangedEventHandler FileIdChanged;

		// used by WpfSelect
		private void OnSelectedPropertyChange([CallerMemberName] string memberName = "")
		{
			FileIdChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ClassificationFile";
		}

	#endregion
	}
}