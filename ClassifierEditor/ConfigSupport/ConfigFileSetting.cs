#region + Using Directives
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using UtilityLibrary;



#endregion

// user name: jeffs
// created:   6/14/2020 7:35:44 AM


namespace ClassifierEditor.ConfigSupport
{
	[DataContract(Namespace = "")]
	[KnownType(typeof(ConfigSeedFileSetting))]
	public class ConfigFileSetting : INotifyPropertyChanged
	{
		private FilePath<FileNameSimple> filePath;
		private string fileName;
		private string folder;

		public ConfigFileSetting() {}

		public ConfigFileSetting(
			string name,
			string username,
			string folder,
			string fileName 
			)
		{
			Name = name;
			UserName = username;
			this.fileName = fileName;
			this.folder = folder;
		}

		[DataMember(Order = 1)]
		public string Name { get; set; }

		[DataMember(Order = 2)]
		public string UserName { get; set; }

		[DataMember(Order = 3)]
		public string Folder
		{
			get => folder;
			set
			{
				folder = value;
				OnPropertyChange();

				AssignFilePath();
			}
		}

		[DataMember(Order = 4)]
		public string FileName
		{
			get => fileName;
			set
			{
				fileName = value;
				OnPropertyChange();

				AssignFilePath();
			}
		}

		[IgnoreDataMember]
		public FilePath<FileNameSimple> FilePath
		{
			get => filePath;
			set
			{
				filePath = value;
				OnPropertyChange();
			}
		}


		private void AssignFilePath()
		{
			if (fileName != null && folder != null)
			{
				FilePath = new FilePath<FileNameSimple>(folder + @"\" + fileName);
			}
			else
			{
				FilePath =FilePath<FileNameSimple>.Invalid;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	[DataContract(Namespace = "")]
	public class ConfigSeedFileSetting : ConfigFileSetting
	{
		private string assocSamplePathAndFile;
		private FilePath<FileNameSimple> assocSampleFile;

		public ConfigSeedFileSetting(
			string name,
			string username,
			string folder,
			string fileName,
			string samplePathAndFile
			) : base(name, username, folder, fileName)
		{
			assocSamplePathAndFile = samplePathAndFile;
		}

		[DataMember(Order = 10)]
		public string AssociatedSamplePathAndFile
		{
			get => assocSamplePathAndFile;
			set
			{
				assocSamplePathAndFile = value;
				OnPropertyChange();

				if (!value.IsVoid())
				{
					AssocSampleFile = new FilePath<FileNameSimple>(value);
				}
				else
				{
					AssocSampleFile = FilePath<FileNameSimple>.Invalid;
				}
			}
		}

		public FilePath<FileNameSimple> AssocSampleFile
		{
			get => assocSampleFile;
			set
			{
				assocSampleFile = value;
				OnPropertyChange();
			}
		}

	}



}
