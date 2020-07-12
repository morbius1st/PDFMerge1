#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AndyShared.ConfigMgrShared;
using AndyShared.FilesSupport;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  7/8/2020 1:09:18 PM

namespace AndyShared.ConfigSupport
{
	[DataContract(Namespace = "")]
	public class ConfigFileClassificationUser : ConfigFile<FileNameUserAndId>
	{
	#region private fields

		private FilePath<FileNameSimple> assocSampleFile;

		private bool selectedFile;

	#endregion

	#region ctor

		public ConfigFileClassificationUser(string fileId,
			string username,
			string folder,
			string fileName) : base(fileId, username, folder, fileName) { }

		public ConfigFileClassificationUser(string filePath, bool selectedFile = false)
		{
			FilePath = new FilePath<FileNameUserAndId>(filePath);

			if (!IsValid)
			{
				FilePath = FilePath<FileNameUserAndId>.Invalid;
				this.FileId = null;
				this.UserName = null;
				this.FileName = null;
				this.Folder = null;
				return;

				
			}

			this.FileId = FilePath.GetFileNameObject.FileId;
			this.UserName = FilePath.GetFileNameObject.UserName;
			this.Folder = FilePath.GetPath;
			this.FileName = FilePath.GetFileNameObject.FileName;
			this.SelectedFile = selectedFile;

			GetAssociatedSamplePathAndFile(Folder, FileNameNoExt);

			UpdateProperties();
		}

	#endregion

	#region public properties

		public bool IsValid => FilePath?.GetFileNameObject?.IsValid ?? false;

		public bool HasSampleFile => assocSampleFile.IsValid;

		public string FileNameNoExt => FilePath.GetFileNameObject.FileNameNoExt;

		public string GetFullFilePath => FilePath.GetFullFilePath;

		public string AssocSampleFilePath => assocSampleFile.GetFullFilePath;

		public bool SelectedFile
		{
			get => selectedFile;
			set
			{
				selectedFile = value;

				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("IsValid");
			OnPropertyChange("HasSampleFile");
			OnPropertyChange("FileName");
			OnPropertyChange("FileNameNoExt");
			OnPropertyChange("GetFullFilePath");
			OnPropertyChange("AssocSampleFilePath");
		}

		private void GetAssociatedSamplePathAndFile(string folder, string fileNameNoExt)
		{
			assocSampleFile = new FilePath<FileNameSimple>(
				ConfigSeedFileSupport.GetSampleFile(folder, fileNameNoExt, true));
		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ConfigFileClassificationUser";
		}

	#endregion
	}
}