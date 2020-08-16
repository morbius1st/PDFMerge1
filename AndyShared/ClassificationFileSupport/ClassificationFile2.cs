#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using AndyShared.FilesSupport;
using AndyShared.Settings;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  8/2/2020 10:24:57 PM

namespace AndyShared.ClassificationFileSupport
{
	[DataContract(Namespace = "")]
	public class ClassificationFile : INotifyPropertyChanged
	{
	#region private fields

		private FilePath<FileNameUserAndId> filePathLocal;

		private string sampleFile;

		private BaseDataFile<ClassificationFileData> dataFile;

		private bool isSelected;

		private bool editingEnabled;

	#endregion

	#region ctor

		public ClassificationFile(string filePath, bool fileSelected = false)
		{
			FilePathLocal = new FilePath<FileNameUserAndId>(filePath);

			if (!SettingsSupport.ValidateXmlFile(filePath))
			{
				FilePathLocal = FilePath<FileNameUserAndId>.Invalid;
				return;
			}

			SampleFilePath = SampleFromFile();

		}

	#endregion

	#region public properties

		// tied to xml data file
		public string DataDescription
		{
			get => dataFile.Data.Description;
			set
			{
				if (value.Equals(dataFile.Data.Description)) return;

				dataFile.Data.Description = value;

				dataFile.Admin.Write();

				OnPropertyChange();
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

		public string FileName => filePathLocal.GetFileName;

		public string UserName
		{
			get => filePathLocal.GetFileNameObject.UserName;
		}

		public string FileId
		{
			get => filePathLocal.GetFileNameObject.FileId;

			set
			{
				// value = value.Trim();

				if (value.Equals(FileId)) return;

				string oldSampleFilePath;
				string newSampleFileName;
				string newSampleFilePath;

				string newFileNameNoExt = ClassificationFileAssist.MakeClassificationFileNameNoExt(UserName, value);

				string newFileName = ClassificationFileAssist.MakeClassificationFileName(UserName, value);

				string newFilePath = GetFolderPath + FilePathUtil.PATH_SEPARATOR + newFileName;
				string oldFilePath = GetFullFilePath;

				try
				{
					bool found = File.Exists(oldFilePath);


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

				filePathLocal.ChangeFileName(newFileNameNoExt);

				OnPropertyChange();
				OnSelectedPropertyChange();

				OnPropertyChange("SampleFilePath");
				OnPropertyChange("FileName");
				OnPropertyChange("GetFullFilePath");
			}
		}


		// management 
		public string GetFullFilePath => FilePathLocal.GetFullFilePath;

		public string GetFolderPath => FilePathLocal.GetPath;

		public string FileNameNoExt => FilePathLocal.GetFileNameObject.FileNameNoExt;

		public string SampleFilePath
		{
			get => sampleFile;
			private set
			{
				sampleFile = value;
				OnPropertyChange();
			}
		}

		public bool HasSampleFile
		{
			get
			{
				return sampleFile != null;
			}
		}

		public bool SampleFileValidated { get; private set;  } = false;

		public bool IsValid => FilePathLocal?.GetFileNameObject?.IsValid ?? false;

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

		public bool InstallSampleFile(string proposedSampleFile)
		{
			string sampleFile = 
				ClassificationFileAssist.IncorporateSampleFile(proposedSampleFile,
				GetFolderPath, FileNameNoExt);

			if (sampleFile == null)
			{
				return false;
			}
			SampleFilePath = sampleFile;

			return true;
		}

	#endregion

	#region private methods

		private string FileDescriptionFromFile(string filePath) => CsUtilities.ScanXmlForElementValue(filePath, "FileDescription", 1);

		private string SampleFromFile() => CsUtilities.ScanXmlForElementValue(GetFullFilePath, "SampleFile", 1);

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
		
		public void Read()
		{
			dataFile = new BaseDataFile<ClassificationFileData>();
			dataFile.Configure(GetFolderPath, FilePathLocal.GetFileName);
			dataFile.Admin.Read();

			EditingEnabled = true;

			UpdateProperties();
		}
	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
		
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
			return "this is ClassificationFile2";
		}

	#endregion
	}
}