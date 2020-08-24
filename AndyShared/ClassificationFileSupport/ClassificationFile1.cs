#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AndyShared.ClassificationFileSupport;
using AndyShared.ConfigMgrShared;
using AndyShared.FilesSupport;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  7/8/2020 1:09:18 PM


// represents a single classification configuration file
namespace AndyShared.ConfigSupport
{
	[DataContract(Namespace = "")]
	public class ClassificationFile1 : ConfigFile<FileNameUserAndId>
	{
	#region private fields

		private FilePath<FileNameSimple> sampleFile;

		private BaseDataFile<ClassificationFileData> dataFile;

		private bool isSelected;

		private bool editingEnabled;

	#endregion

	#region ctor

		public ClassificationFile1(string fileId,
			string username,
			string folder,
			string fileName) : base(fileId, username, folder, fileName) { }

		public ClassificationFile1(string filePath, bool fileSelected = false)
		{
			FilePathLocal = new FilePath<FileNameUserAndId>(filePath);

			if (!IsValid)
			{
				FilePathLocal = FilePath<FileNameUserAndId>.Invalid;
				FileId = null;
				UserName = null;
				FileName = null;
				Folder = null;
				return;
			}

			FileId = FilePathLocal.FileNameObject.FileId;
			UserName = FilePathLocal.FileNameObject.UserName;
			Folder = FilePathLocal.FolderPath;
			FileName = FilePathLocal.FileNameObject.FileName;
			IsSelected = fileSelected;
			editingEnabled = true;

			UpdateProperties();
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

		public string DataFileId
		{
			get => FileId;

			set
			{
				// value = value.Trim();

				if (value.Equals(FileId)) return;

				string oldSampleFilePath;
				string newSampleFileName;
				string newSampleFilePath;

				string newFileName = ClassificationFileAssist.MakeClassificationFileName(UserName, value);

				string newFilePath = GetFolderPath + FilePathUtil.PATH_SEPARATOR + newFileName;
				string oldFilePath = GetFullFilePath;

				try
				{
					File.Move(oldFilePath, newFilePath);


					if (HasSampleFile)
					{
						oldSampleFilePath = SampleFilePath;

						newSampleFileName = ClassificationFileAssist.MakeSampleFileName(UserName, value);
						newSampleFilePath = GetFolderPath + FilePathUtil.PATH_SEPARATOR + newSampleFileName;

						File.Move(oldSampleFilePath, newSampleFilePath);
					}
				}

				catch { }

				FileId = value;

				OnPropertyChange();
				OnSelectedPropertyChange();

				OnPropertyChange("SampleFilePath");
				OnPropertyChange("FileName");
				OnPropertyChange("GetFullFilePath");

			}
		}


		// management 
		public string GetFullFilePath => FilePathLocal.FullFilePath;

		public string GetFolderPath => FilePathLocal.FolderPath;

		public string FileNameNoExt => FilePathLocal.FileNameObject.FileNameNoExt;

		public string SampleFilePath
		{
			get
			{
				ValidateSampleFile();

				return sampleFile.FullFilePath;
			}
		}

		public bool HasSampleFile
		{
			get
			{
				ValidateSampleFile();

				return sampleFile.IsValid;
			}
		}

		public bool SampleFileValidated { get; private set;  } = false;

		public bool IsValid => FilePathLocal?.FileNameObject?.IsValid ?? false;

		public bool EditingEnabled
		{
			get => editingEnabled;
			set
			{
				editingEnabled = value;
				OnPropertyChange();
			}
		}

		public bool IsSelected
		{
			get => isSelected;
			set
			{
				isSelected = value;

				OnPropertyChange();
			}
		}

		public string DescriptionFromFile => CsUtilities.ScanXmlForElementValue(GetFullFilePath, "Description", 1);

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void ValidateSampleFile()
		{
			if (!SampleFileValidated)
			{
				GetSampleFilePath(Folder, FileNameNoExt);

				SampleFileValidated = true;
			}
		}

		private void UpdateProperties()
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

		private void GetSampleFilePath(string folder, string fileNameNoExt)
		{
			sampleFile = new FilePath<FileNameSimple>(
				ClassificationFileAssist.GetSampleFile(folder, fileNameNoExt, false));
		}

		public void Read()
		{
			dataFile = new BaseDataFile<ClassificationFileData>();
			dataFile.Configure(GetFolderPath, FilePathLocal.FileName);
			dataFile.Admin.Read();

			EditingEnabled = true;

			UpdateProperties();
		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler SelectedPropertyChanged;

		private void OnSelectedPropertyChange([CallerMemberName] string memberName = "")
		{
			SelectedPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
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