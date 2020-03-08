// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SavedFolderPair.cs
// Created:      -- ()

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Sylvester.FileSupport;
using UtilityLibrary;

namespace Sylvester.SavedFolders 
{
	[DataContract]
	public class SavedFolderPair : IComparable<SavedFolderPair>, IEquatable<SavedFolderPair>, INotifyPropertyChanged
	{
		private string icon;

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Icon
		{
			get => icon;
			set
			{
				icon = value;
				OnPropertyChange();
			}
		}

		[DataMember]
		public FilePath<FileNameSimple> Current { get; set; }

		[DataMember]
		public FilePath<FileNameSimple> Revision { get; set; }

		public SavedFolderPair() { }

		public SavedFolderPair(FilePath<FileNameSimple> current,
			FilePath<FileNameSimple> revision, string name = "")
		{
			Icon = null;
			Current = current;
			Revision = revision;


			Name = name.IsVoid()
				? MakeCurrRevFolderPairkey(current[current.GetFolderCount],
					revision[revision.GetFolderCount])
				: name;
		}

		public static string MakeCurrRevFolderPairkey(string currentRootFolder, string revisionRootFolder)
		{
			return currentRootFolder + " / " + revisionRootFolder;
		}

		public int CompareTo(SavedFolderPair other)
		{
			return Name.ToUpper().CompareTo(other.Name.ToUpper());
		}

		public bool Equals(SavedFolderPair other)
		{
			return Name.ToUpper().Equals(other.Name.ToUpper());
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}