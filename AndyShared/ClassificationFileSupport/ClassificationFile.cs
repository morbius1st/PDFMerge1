#region using directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AndyShared.FileSupport;
using AndyShared.SampleFileSupport;
using AndyShared.Settings;
using SettingsManager;
using UtilityLibrary;
using static AndyShared.FileSupport.FileNameUserAndId;

#endregion

// username: jeffs
// created:  8/2/2020 10:24:57 PM

namespace AndyShared.ClassificationFileSupport
{
	[DataContract(Namespace = "")]
	public class ClassificationFile : INotifyPropertyChanged
	{
		public const string CLASSF_FILE_EXT_NO_SEP = "xml";

	#region private fields

		private FilePath<FileNameUserAndId> filePathLocal;

		// private string sampleFilePath;

		private SampleFile sampleFile;

		private BaseDataFile<ClassificationFileData> dataFile;

		private bool isSelected;

		private bool dataFileRead = false;
		private bool isDefaultClassfFile = true;
		private bool isUserClassfFile = false;

	#endregion

	#region ctor

		// public ClassificationFile()
		// {
		// 	FilePathLocal = FilePath<FileNameUserAndId>.Invalid;
		// }
		//

		public ClassificationFile(string filePath, bool fileSelected = false)
		{
			FilePathLocal = new FilePath<FileNameUserAndId>(filePath);

			if (!filePathLocal.IsValid) return;

			if (!SettingsSupport.ValidateXmlFile(filePath)
				|| !ValidateAgainstUsername(filePathLocal)
				)
			{
				FilePathLocal = FilePath<FileNameUserAndId>.Invalid;
				return;
			}

			InitailizeSample(FilePathLocal.FullFilePath);
		}

	#endregion

	#region public properties

		// tied to xml data file
		public string DataDescription
		{
			get => dataFile.Info.Description;
			set
			{
				if (value.Equals(dataFile.Info.Description)) return;

				dataFile.Info.Description = value;

				dataFile.Admin.Write();

				OnPropertyChange();
			}
		}

		public string DateNote
		{
			get => dataFile.Info.Notes;
			set
			{
				dataFile.Info.Notes = value;

				OnPropertyChange();

				dataFile.Admin.Write();
			}
		}

		public FilePath<FileNameUserAndId> FilePathLocal
		{
			get => filePathLocal;

			set
			{
				filePathLocal = value;


				OnPropertyChange();
			}
		}

		/// <summary>
		/// The whole file name - name + extension
		/// </summary>
		public string FileName => filePathLocal.FileName;

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
				if (value.Equals(FileId)) return;

				string newFileNameNoExt = AssembleFileNameNoExt(UserName, value);

				filePathLocal.ChangeFileName(newFileNameNoExt);

				OnPropertyChange();

				OnSelectedPropertyChange();

				OnPropertyChange("SampleFilePath");
				OnPropertyChange("FileName");
				OnPropertyChange("GetFullFilePath");
			}
		}

		// management 
		public string FullFilePath => FilePathLocal.FullFilePath;

		/// <summary>
		/// The folder path to the user's specific configuration files
		/// </summary>
		public string FolderPath => FilePathLocal.FolderPath;

		public string FileNameNoExt => FilePathLocal.FileNameObject.FileNameNoExt;

		public string SampleFilePath
		{
			get => sampleFile?.FullFilePath;
			set
			{
				// sampleFile = new SampleFile(value);
				sampleFile = new SampleFile();
				sampleFile.InitializeFromClassfFilePath(value);

				UpdateSampleFileProperties();
			}
		}

		public string SampleFileName => sampleFile?.SampleFilePath?.FileNameNoExt ?? null;

		// public bool IsValid => FilePathLocal?.FileNameObject?.IsValid ?? false;
		public bool IsValid => FilePathLocal?.IsValid ?? false;

		public bool EditingEnabled => DataFileRead && IsUserClassfFile;

		public bool IsSelected
		{
			get => isSelected;
			set
			{
				isSelected = value;

				OnPropertyChange();
			}
		}

		public string DescriptionFromFile => CsUtilities.ScanXmlForElementValue(FullFilePath, "Description", 0);

	#endregion

	#region private properties

		private bool DataFileRead
		{
			get => dataFileRead;
			set
			{
				dataFileRead = value;

				OnPropertyChange();
				OnPropertyChange("EditingEnabled");
			}
		}

		private bool IsDefaultClassfFile
		{
			get => isDefaultClassfFile;
			set
			{
				isDefaultClassfFile = value;

				OnPropertyChange();
				OnPropertyChange("EditingEnabled");
			}
		} // this will pre-configure to disallow editing

		private bool IsUserClassfFile
		{
			get => isUserClassfFile;
			set
			{
				isUserClassfFile = value;

				OnPropertyChange();
				OnPropertyChange("EditingEnabled");
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

		public void Read()
		{
			dataFile = new BaseDataFile<ClassificationFileData>();
			dataFile.Configure(FolderPath, FileName);
			dataFile.Admin.Read();

			DataFileRead = true;

			UpdateProperties();
		}

		public void UpdateProperties()
		{
			OnPropertyChange("IsValid");
			OnPropertyChange("HasSampleFile");
			OnPropertyChange("FileName");
			OnPropertyChange("FileNameNoExt");
			OnPropertyChange("GetFullFilePath");
			OnPropertyChange("GetPath");
			OnPropertyChange("SampleFilePath");
			OnPropertyChange("DataDescription");
		}

	#endregion

	#region private methods

		private void InitailizeSample(string classfFilePath)
		{
			// sampleFile = new SampleFile(classfFilePath);
			sampleFile = new SampleFile();

			sampleFile.InitializeFromClassfFilePath(classfFilePath);

			// bool v = sampleFile.IsValid;
			UpdateSampleFileProperties();
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
			OnPropertyChange("SampleFilePath");
			OnPropertyChange("SampleFileName");
		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public event PropertyChangedEventHandler FileIdChanged;

		private void OnSelectedPropertyChange([CallerMemberName] string memberName = "")
		{
			FileIdChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ClassificationFile";
		}

	#endregion
	}
}