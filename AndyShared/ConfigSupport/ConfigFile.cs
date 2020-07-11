#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
	[KnownType(typeof(ConfigSeedFile))]
	[KnownType(typeof(ConfigFileClassificationUser))]
	public class ConfigFile<T> : INotifyPropertyChanged, IObservCollMember  where T : AFileName,  new()  
	{
		private FilePath<T> filePath;
		private string fileName;
		private string folder;

		public ConfigFile() { }

		public ConfigFile(string fileId,
			string username,
			string folder,
			string fileName)
		{
			FileId = fileId;
			UserName = username;
			Folder = folder;
			FileName = fileName;
		}

		[IgnoreDataMember]
		public string Key => MakeKey(UserName, FileId);

		// identification name
		[DataMember(Order = 1)]
		public string FileId { get; set; }

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

				UpdateFilePath();
			}
		}

		/// <summary>
		/// The file's name + extension
		/// </summary>
		[DataMember(Order = 4)]
		public string FileName
		{
			get => fileName;
			set
			{
				fileName = value;
				OnPropertyChange();

				UpdateFilePath();
			}
		}

		[IgnoreDataMember]
		public FilePath<T> FilePath
		{
			get => filePath;
			set
			{
				filePath = value;
				OnPropertyChange();
			}
		}

		public void UpdateFilePath()
		{
			string fp = "";

			if (fileName.IsVoid() || folder.IsVoid())
			{
				return;
				// fp = folder + FilePathUtil.PATH_SEPARATOR + fileName;
			} 
			// else if (folder != null)
			// {
			// 	fp = folder;
			// } 
			// else if (fileName != null)
			// {
			// 	fp = fileName;
			// }

			fp = folder + FilePathUtil.PATH_SEPARATOR + fileName;

			filePath = new FilePath<T>(fp);
		}

		public static string MakeKey(string userName, string id)
		{
			return userName + " :: " + id;

		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	// note that the value is also an index in
	// a string array
	public enum SeedFileStatus
	{
		IGNORE = -1,
		OK_AS_IS = 0,
		COPY = 1,
		REMOVE = 2
	}

	[DataContract(Namespace = "")]
	public class ConfigSeedFile : ConfigFile<FileNameSimple>, ICloneable
	{
		private string assocSamplePathAndFile;
		private FilePath<FileNameSimple> assocSampleFile;
		private bool local;
		private bool selectedSeedSeed;
		private bool keep;
		private SeedFileStatus status;


		public ConfigSeedFile() { }

		public ConfigSeedFile(
			string fileId,
			string username,
			string folder,
			string fileName,
			string samplePathAndFile,
			bool   local,
			bool   selectedSeedSeed,
			bool   keep,
			SeedFileStatus status) : base(fileId, username, folder, fileName)
		{
			AssociatedSamplePathAndFile = samplePathAndFile;
			this.local = local;
			this.selectedSeedSeed = selectedSeedSeed;
			this.keep = keep;
			this.status = status;
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

		/// <summary>
		/// flag to copy this item in to the folder<br/>
		/// this is a re-purpose of selected
		/// </summary>
		[IgnoreDataMember]
		public bool Copy
		{
			get => selectedSeedSeed;
			set
			{
				selectedSeedSeed = value;
				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public SeedFileStatus Status
		{
			get => status;
			set
			{
				status = value;
				OnPropertyChange();
			}
		}

		/// <summary>
		/// flag that this is has been selected<br/>
		/// this flag and copy use the same underlying
		/// data item
		/// </summary>
		[DataMember(Order = 12)]
		public bool SelectedSeed
		{
			get => selectedSeedSeed;
			set
			{
				selectedSeedSeed = value;
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
			return new ConfigSeedFile(
				FileId,
				UserName,
				Folder,
				FileName,
				AssociatedSamplePathAndFile, 
				Local, SelectedSeed, Keep, 
				SeedFileStatus.IGNORE);
		}

	#region public static methods

	#endregion

	}

	// public class ConfigSeedFileComparer : IEqualityComparer<ConfigSeedFile>
	// {
	// 	public bool Equals(ConfigSeedFile x, string y)
	// 	{
	// 		return x.MakeKey().Equals(y);
	// 	}
	//
	// 	public bool Equals(ConfigSeedFile x, ConfigSeedFile y)
	// 	{
	// 		return false;
	// 	}
	//
	// 	public int GetHashCode(ConfigSeedFile obj)
	// 	{
	// 		return 0;
	// 	}
	// }
}