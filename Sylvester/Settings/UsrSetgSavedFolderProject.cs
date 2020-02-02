// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SavedFolderProject.cs
// Created:      -- ()

using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Sylvester.SavedFolders
{

	[DataContract]
	public class SavedFolderProject
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

		public SavedFolderProject() { }

		public SavedFolderProject(string volume, string rootFolder, string name = "")
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
}