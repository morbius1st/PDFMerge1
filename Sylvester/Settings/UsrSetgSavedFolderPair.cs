// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SavedFolderPair.cs
// Created:      -- ()

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml;

using Sylvester.FileSupport;
using Sylvester.Settings;
using UtilityLibrary;

namespace Sylvester.SavedFolders
{
	[DataContract]
	public class SavedFolderPair : IComparable<SavedFolderPair>, 
		IEquatable<SavedFolderPair>, INotifyPropertyChanged, ICloneable
	{
		private const string PAIR_PREFIX = "Pair ";

		private string icon;
		private string name;
		private string key;
		private string parentKey;
		private FilePath<FileNameSimple> current;
		private FilePath<FileNameSimple> revision;

	#region ctor

		public SavedFolderPair() { }

		public SavedFolderPair(SavedFolderProject sf,
			FilePath<FileNameSimple> current,
			FilePath<FileNameSimple> revision)
		{
			Icon = App.Icon_FolderPair00;
			Current = current;
			Revision = revision;
			ParentKey = sf.Key;

			// name can be changed
			Name = MakeName(sf, current, revision);

			// key is a fixed value
			Key = MakeKey(sf);

			OnPropertyChange("IsConfigured");
		}

	#endregion

	#region public properties

		[DataMember]
		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChange();
				OnPropertyChange("IsConfigured");
			}
		}

		[DataMember]
		public string Key
		{
			get => key;
			set
			{
				key = value;
				OnPropertyChange("IsConfigured");
			}
		}

		[DataMember]
		public string ParentKey
		{
			get => parentKey;
			set
			{
				parentKey = value;
				OnPropertyChange("IsConfigured");
			}
		}

		[IgnoreDataMember]
		public bool IsConfigured
		{
			get
			{
				bool result = !name.IsVoid();

				result &= !Key.IsVoid();
				result &= !ParentKey.IsVoid();
				result &= current?.IsValid ?? false;
				result &= revision?.IsValid ?? false;

				return result;
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
		public FilePath<FileNameSimple> Current
		{
			get => current;
			set
			{
				current = value;
				OnPropertyChange();
				OnPropertyChange("IsConfigured");
			}
		}


		[DataMember]
		public FilePath<FileNameSimple> Revision
		{
			get => revision;
			set
			{
				revision = value;
				OnPropertyChange();
				OnPropertyChange("IsConfigured");
			}
		}

	#endregion

	#region static methods


		// create a name for this pair - this is a "temporary" name
		// as the user can change as needed
		public static string MakeName(SavedFolderProject sf,
			FilePath<FileNameSimple> currentRootFolder, 
			FilePath<FileNameSimple> revisionRootFolder)
		{
			string result;

			if (currentRootFolder == null || revisionRootFolder == null || sf == null ||
				currentRootFolder.FullFilePath.IsVoid() || revisionRootFolder.FullFilePath.IsVoid())
			{
				result = CreateName(sf);
			}
			else
			{
				result = CreateName(sf,
					currentRootFolder[-1] + " / " + revisionRootFolder[-1]);
			}

			return result;

		}

		// create a temporary name
		private static string CreateName(SavedFolderProject sf, string prefix = null)
		{
			int idx = 1;
			string preface = prefix ?? PAIR_PREFIX;
			string tempName = preface + "1";

			while (SetgMgr.Instance.ContainsName(sf, tempName))
			{
				tempName = $"{preface}{++idx:D}";
			}

			return tempName;
		}

	#endregion

	#region public methods

		public SavedFolderPair Clone(SavedFolderProject sf)
		{
			SavedFolderPair clone = Clone() as SavedFolderPair;

			clone.Key = MakeKey(sf);

			clone.name = name;
			clone.Icon = Icon;

			return clone;
		}

	#endregion

	#region private methods

		// create a random key - 12 characters long
		private string MakeKey(SavedFolderProject sf)
		{
			string tempKey;
			do
			{
				tempKey = CsUtilities.RandomString(12);

			}
			while (SetgMgr.Instance.ContainsKey(sf, tempKey));

			return tempKey;
		}

	#endregion

	#region system override methods

		public object Clone()
		{
			SavedFolderPair clone = new SavedFolderPair();

			clone.current = current.Clone();
			clone.revision = revision.Clone();
			clone.name = name;

			clone.icon = Icon;
			clone.Key = null;

			return clone;
		}

		// todo: need better - must also compare filepaths
		public int CompareTo(SavedFolderPair other)
		{
			return Name.ToUpper().CompareTo(other.Name.ToUpper());
		}

		// todo: need better - must also compare filepaths
		public bool Equals(SavedFolderPair other)
		{
			return Name.ToUpper().Equals(other.Name.ToUpper());
		}

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	}
}