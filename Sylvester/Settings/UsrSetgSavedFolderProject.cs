// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SavedFolderProject.cs
// Created:      -- ()

using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using UtilityLibrary;

namespace Sylvester.SavedFolders
{

	[DataContract]
	public class SavedFolderProject : IComparable<SavedFolderProject>, IEquatable<SavedFolderProject>
	{
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
			UseCount = 0;

			Name = name.IsVoid() ? MakeSavedFolderKey(folder) : name;

			Icon = null;
		}

		public static string MakeSavedFolderKey(FilePath<FileNameSimple> folder)
		{
			return folder.AssemblePath(1);
		}

		public int CompareTo(SavedFolderProject other)
		{
			return Name.ToUpper().CompareTo(other.Name.ToUpper());
		}

		public bool Equals(SavedFolderProject other)
		{
			return Name.ToUpper().Equals(other.Name.ToUpper());
		}
	}
}