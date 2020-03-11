// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SavedFolderPair.cs
// Created:      -- ()

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml;
using Sylvester.FileSupport;
using Sylvester.Settings;
using UtilityLibrary;

namespace Sylvester.SavedFolders
{
	[DataContract]
	public class SavedFolderPair : IComparable<SavedFolderPair>, IEquatable<SavedFolderPair>, INotifyPropertyChanged
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
		public FilePath<FileNameSimple> Current { get; set; }

		[DataMember]
		public FilePath<FileNameSimple> Revision { get; set; }

		public SavedFolderPair() { }

		public SavedFolderPair(FilePath<FileNameSimple> current,
			FilePath<FileNameSimple> revision, string name)
		{
			Icon = App.Icon_FolderPair00;
			Current = current;   // ?? FilePath<FileNameSimple>.Invalid;
			Revision = revision; // ?? FilePath<FileNameSimple>.Invalid;

			Name = name;
		}

		public static string MakeFolderPairkey(
			SavedFolderProject sf,
			FilePath<FileNameSimple> currentRootFolder, FilePath<FileNameSimple> revisionRootFolder)
		{
			string result;

			if (currentRootFolder.GetFullPath.IsVoid() || revisionRootFolder.GetFullPath.IsVoid())
			{
				result = tempFolderPairKey(sf);
			}
			else
			{
				result = tempFolderPairKey(sf,
					currentRootFolder + " / " + revisionRootFolder);
			}

			return result;

		}

		private const string PAIR_PREFIX = "Pair ";

		private static string tempFolderPairKey(SavedFolderProject sf, string prefix = null)
		{
			int idx = 1;
			string preface = prefix ?? PAIR_PREFIX;
			string tempKey = preface + "1";

			while (SetgMgr.Instance.ContainsFolderPair(sf, tempKey))
			{
				tempKey = $"{preface}{++idx:D}";
			}

			return tempKey;
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