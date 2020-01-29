// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SavedProject.cs
// Created:      -- ()

using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Sylvester.FileSupport;

namespace Sylvester.SavedFolders
{

	[DataContract]
	public class SavedProject
	{
		// this will be the root folder name
		public struct FolderRoot
		{
			[DataMember]
			public string Volume { get; set; }

			[DataMember]
			public string RootFolder { get; set; }
		}

		[DataMember]
		public FolderRoot Identifier { get; set; }

		[DataMember]
		public string Key { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Icon { get; set; }

		[DataMember]
		public int UseCount { get; set; }

		[DataMember]
		public ObservableCollection<SavedFolderPair> SavedFolderPairs { get; set; }
			= new ObservableCollection<SavedFolderPair>();

		public SavedProject() { }

		public SavedProject(string volume, string rootFolder, string name = "")
		{
			Identifier = new FolderRoot()
			{
				Volume = volume,
				RootFolder = rootFolder
			};
			UseCount = 0;

			Name = string.IsNullOrWhiteSpace(name) ? Identifier.RootFolder : name;

			Key = MakeSavedFolderKey(UseCount, Name);

			Icon = null;
		}

		public static string MakeSavedFolderKey(int useCount, string name)
		{
			return $"{useCount:D5} / " + name;
		}
	}


	[DataContract]
	public class SavedFolderPair
	{
		[DataMember]
		public string Key { get; set; }

		[DataMember]
		public string Icon { get; set; }

		[DataMember]
		public Route Current { get; set; }

		[DataMember]
		public Route Revision { get; set; }

		public SavedFolderPair() { }

		public SavedFolderPair(Route current,
			Route revision, string name = "")
		{
			Icon = null;
			Current = current;
			Revision = revision;

			Key = MakeCurrRevFolderPairkey(current.FolderName(-1), revision.FolderName(-1), name);
		}

		public static string MakeCurrRevFolderPairkey(string currentRootFolder, string revisionRootFolder, string name = "")
		{
			if (!string.IsNullOrWhiteSpace(name)) return name;

			return currentRootFolder + " / " + revisionRootFolder;
		}
	}

}