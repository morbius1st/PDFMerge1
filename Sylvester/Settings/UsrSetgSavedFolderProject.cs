// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SavedFolderProject.cs
// Created:      -- ()

using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using UtilityLibrary;

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
			public string ProjectFolder { get; set; }
		}

		[DataMember]
		public FolderRoot Identifier { get; set; } = new FolderRoot() {Volume = null, ProjectFolder = null};

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

		public SavedFolderProject(FilePath<FileNameSimple> folder, string name = "")
		{
			Identifier = new FolderRoot()
			{
				Volume = folder.GetDrivePath, 
				ProjectFolder = folder.AssemblePath(1)
			};
			UseCount = 0;

			Name = string.IsNullOrWhiteSpace(name) ? Identifier.ProjectFolder : name;

			Key = MakeSavedFolderKey(Name);

			Icon = null;
		}

		public static string MakeSavedFolderKey(string name)
		{
			return name.ToUpper();
		}
	}
}