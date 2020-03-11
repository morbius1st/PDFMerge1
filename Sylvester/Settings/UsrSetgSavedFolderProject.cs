// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SavedFolderProject.cs
// Created:      -- ()

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Sylvester.Settings;
using UtilityLibrary;

namespace Sylvester.SavedFolders
{

	[DataContract]
	public class SavedFolderProject : IComparable<SavedFolderProject>, IEquatable<SavedFolderProject>, INotifyPropertyChanged
	{
		private string icon;
		private string name;

		[DataMember]
		public string Name
		{
			get => name;
			set
			{ 
				name = value;
				OnPropertyChange();
			}
		}

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
		public int UseCount { get; set; }

		[DataMember]
		public ObservableCollection<SavedFolderPair> SavedFolderPairs { get; set; }
			= new ObservableCollection<SavedFolderPair>();

		public SavedFolderProject() { }

		public SavedFolderProject(FilePath<FileNameSimple> folder, SavedFolderType folderType, string name = "")
		{
			UseCount = 0;

			Name = name.IsVoid() ? MakeFolderProjectKey(folder, folderType) : name;

			Icon = App.Icon_FolderProject00;
		}

		public static string MakeFolderProjectKey(FilePath<FileNameSimple> folder, SavedFolderType folderType)
		{
			return folder?.AssemblePath(1) ?? TempFolderProjectKey(folderType);
		}

		private const string PROJECT_PREFIX = "Project ";

		private static string TempFolderProjectKey(SavedFolderType folderType)
		{
			int idx = 1;
			string tempKey = PROJECT_PREFIX + "1";

			while (SetgMgr.Instance.ContainsSavedFolder(tempKey, folderType))
			{
				tempKey = $"{PROJECT_PREFIX}{++idx:D}";
			}

			return tempKey;
		}

		public int CompareTo(SavedFolderProject other)
		{
			return Name.ToUpper().CompareTo(other.Name.ToUpper());
		}

		public bool Equals(SavedFolderProject other)
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