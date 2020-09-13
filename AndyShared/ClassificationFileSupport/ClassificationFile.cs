#region using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FileSupport;
using AndyShared.SampleFileSupport;
using AndyShared.Settings;
using AndyShared.Support;
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
	#region private fields

		private FilePath<FileNameUserAndId> filePathLocal;

		private SampleFile sampleFile;

		private BaseDataFile<ClassificationFileData> dataFile;

		private bool isSelected;

		private bool isModified;

		private bool isInitialized = false;
		private bool isDefaultClassfFile = true;
		private bool isUserClassfFile = false;

		private Orator.ConfRoom.Announcer OnTnInitAnnouncer;

	#endregion

	#region ctor

		public ClassificationFile(string filePath, bool fileSelected = false)
		{
			FilePathLocal = new FilePath<FileNameUserAndId>(filePath);

			if (!filePathLocal.IsValid) return;

			// setup inter-class communication
			// tell parent, I have been modified announcer

			// listen to children - they have been modified
			Orator.Listen(OratorRooms.TN_MODIFIED, OnAnnounceSubChildModified);

			// listen to parent, changes have been saved
			Orator.Listen(OratorRooms.SAVED, OnAnnounceSaved);

			// announce to treenode (treebase) / and components initialized and to initialize
			OnTnInitAnnouncer = Orator.GetAnnouncer(this, OratorRooms.TN_INIT, "Initialize treenode & components");

			bool a = !SettingsSupport.ValidateXmlFile(filePath);
			bool b = !ValidateAgainstUsername(filePathLocal);


			if ((FilePathLocal.IsFound && !SettingsSupport.ValidateXmlFile(filePath))
				|| !ValidateAgainstUsername(filePathLocal)
				)
			{
				FilePathLocal = FilePath<FileNameUserAndId>.Invalid;
				return;
			}

			PreInitialize();

			InitailizeSample(FilePathLocal.FullFilePath);
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

				OnPropertyChange();

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

				OnPropertyChange();

				IsModified = true;
			}
		}

		public string FullFilePath => FilePathLocal.FullFilePath;

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

		public string FileNameNoExt => FilePathLocal.FileNameObject.FileNameNoExt;

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

				UpdateSampleFileProperties();
			}
		}

		public string SampleFileName => sampleFile?.SampleFilePath?.FileName ?? null;

		public string SampleFileNameNoExt => sampleFile?.SampleFilePath?.FileNameNoExt ?? null;

		// status
		public bool CanEdit => IsInitialized && IsUserClassfFile;

		public bool IsInitialized
		{
			get => isInitialized;
			private set
			{
				isInitialized = value;

				OnPropertyChange();
				OnPropertyChange("CanEdit");
			}
		}

		public bool IsModified
		{
			get => isModified; 
			set
			{
				if (value == isModified) return;

				isModified = value;
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

		public bool IsValid => FilePathLocal?.IsValid ?? false;

	#endregion

	#region private properties

		private bool IsDefaultClassfFile
		{
			get => isDefaultClassfFile;
			set
			{
				isDefaultClassfFile = value;

				OnPropertyChange();
				OnPropertyChange("CanEdit");
			}
		} // this will pre-configure to disallow editing

		private bool IsUserClassfFile
		{
			get => isUserClassfFile;
			set
			{
				isUserClassfFile = value;

				OnPropertyChange();
				OnPropertyChange("CanEdit");
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
			dataFile = new BaseDataFile<ClassificationFileData>();
			dataFile.Configure(FolderPath, FileName);
		}

		public void Initialize()
		{
			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ classF|@ initialize| start-init");
			PreInitialize();

			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ classF|@ initialize| read");
			dataFile.Admin.Read();

			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ classF|@ initialize| baseoftree-init");
			dataFile.Data.BaseOfTree.Initalize();

			IsInitialized = true;

			UpdateProperties();

		}

		public void Write()
		{
			dataFile.Admin.Write();
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
			OnPropertyChange("TreeBase");
		}

	#endregion

	#region private methods

		private void InitailizeSample(string classfFilePath)
		{
			sampleFile = new SampleFile();

			sampleFile.InitializeFromClassfFilePath(classfFilePath);

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

	#region event consuming

		// one of the sub-children have been modified
		private void OnAnnounceSubChildModified(object sender, object value)
		{
			IsModified = true;

			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ classfFile|@ onmodified| who| " + sender.ToString() + "| value| " + value);
		}

		private void OnAnnounceSaved(object sender, object value)
		{
			IsModified = false;
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
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