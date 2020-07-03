#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using AndyShared.FilesSupport;
using AndyShared.Support;
using SettingsManager;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   6/14/2020 7:35:44 AM


namespace AndyShared.ConfigSupport
{
	[DataContract(Namespace = "")]
	[KnownType(typeof(ConfigSeedFileSetting))]
	public class ConfigFileSetting : INotifyPropertyChanged, IObservCollMember
	{
		private FilePath<FileNameSimple> filePath;
		private string fileName;
		private string folder;

		public ConfigFileSetting() { }

		public ConfigFileSetting(string name,
			string username,
			string folder,
			string fileName)
		{
			Name = name;
			UserName = username;
			this.fileName = fileName;
			this.folder = folder;
		}

		[IgnoreDataMember]
		public string Key => MakeKey();

		// identification name
		[DataMember(Order = 1)]
		public string Name { get; set; }

		// who created the entry
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


		public string MakeKey()
		{
			return SiteSettings.Data.MakeKey(UserName, Name);
		}

		private void AssignFilePath()
		{
			if (fileName != null && folder != null)
			{
				FilePath = new FilePath<FileNameSimple>(folder + @"\" + fileName);
			}
			else
			{
				FilePath = FilePath<FileNameSimple>.Invalid;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	[DataContract(Namespace = "")]
	public class ConfigSeedFileSetting : ConfigFileSetting, ICloneable
	{
		private string assocSamplePathAndFile;
		private FilePath<FileNameSimple> assocSampleFile;
		private bool local;
		private bool selected;
		private bool keep;

		public ConfigSeedFileSetting() { }

		public ConfigSeedFileSetting(string name,
			string username,
			bool local,
			bool selected,
			bool remove,
			string folder,
			string fileName,
			string samplePathAndFile) : base(name, username, folder, fileName)
		{
			assocSamplePathAndFile = samplePathAndFile;
			this.local = local;
			this.selected = selected;
		}

		[DataMember(Order = 10)]
		public bool Local
		{
			get => local;
			set
			{
				local = value;
				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public bool Keep
		{
			get => keep;
			set
			{
				keep = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 12)]
		public bool Selected
		{
			get => selected;
			set
			{
				selected = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 15)]
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

		[IgnoreDataMember]
		public FilePath<FileNameSimple> AssocSampleFile
		{
			get => assocSampleFile;
			set
			{
				assocSampleFile = value;
				OnPropertyChange();
			}
		}

		public object Clone()
		{
			return new ConfigSeedFileSetting(
				Name,
				UserName,
				Local,
				Selected,
				Keep,
				Folder,
				FileName,
				AssociatedSamplePathAndFile);
		}

	#region public static methods


		public static string MakeKey(FilePath<FileNameSimpleSelectable> file)
		{
			return SiteSettings.Data.MakeKey(Heading.SuiteName,
				file.GetFileNameWithoutExtension);
		}


		public static ConfigSeedFileSetting MakeSeedItem(
			FilePath<FileNameSimpleSelectable> file, string suiteName,
			string sampleFile)
		{
			string key = MakeKey(file);

			return new ConfigSeedFileSetting(
				file.GetFileNameWithoutExtension,
				suiteName,
				false,
				false,
				file.GetFileNameObject.Selected,
				file.GetPath,
				file.GetFileName,
				sampleFile);
		}

	#endregion

	}

	// public class ConfigSeedFileComparer : IEqualityComparer<ConfigSeedFileSetting>
	// {
	// 	public bool Equals(ConfigSeedFileSetting x, string y)
	// 	{
	// 		return x.MakeKey().Equals(y);
	// 	}
	//
	// 	public bool Equals(ConfigSeedFileSetting x, ConfigSeedFileSetting y)
	// 	{
	// 		return false;
	// 	}
	//
	// 	public int GetHashCode(ConfigSeedFileSetting obj)
	// 	{
	// 		return 0;
	// 	}
	// }
}