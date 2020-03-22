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
	public class SavedFolderProject : IComparable<SavedFolderProject>, 
		IEquatable<SavedFolderProject>, INotifyPropertyChanged
	{
		private const string PROJECT_PREFIX = "Project ";

		private string icon;
		private string name;
		private string key;
		private int useCount;
		private ObservableCollection<SavedFolderPair> savedFolderPairs = new ObservableCollection<SavedFolderPair>();

	#region ctor

		public SavedFolderProject() { }

		public SavedFolderProject(FilePath<FileNameSimple> folder, SavedFolderType folderType, string name = "")
		{
			UseCount = 0;

			Name = name.IsVoid() ? MakeName(folder, folderType) : name;

			Key = MakeKey(folderType);

			Icon = App.Icon_FolderProject00;
		}

	#endregion

	#region public properties

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
		public int UseCount
		{
			get => useCount;
			set
			{
				useCount = value;
				OnPropertyChange("IsConfigured");
			}
		}


		[DataMember]
		public ObservableCollection<SavedFolderPair> SavedFolderPairs
		{
			get => savedFolderPairs;
			set
			{
				savedFolderPairs = value;
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
				result &= isFolderPairsConfigured();

				return result;
			}
		}

	#endregion

	#region public methods

		public static string MakeName(FilePath<FileNameSimple> folder, SavedFolderType folderType)
		{
			return folder?.AssemblePath(1) ?? TempFolderProjectKey(folderType);
		}

	#endregion

	#region private methods

		private string MakeKey(SavedFolderType folderType)
		{
			string tempKey;
			do
			{
				tempKey = CsUtilities.RandomString(12);

			}
			while (SetgMgr.Instance.ContainsKey(tempKey, folderType));

			return tempKey;
		}

		private bool isFolderPairsConfigured()
		{
			if (savedFolderPairs == null) return false;

			foreach (SavedFolderPair sfp in savedFolderPairs)
			{
				if (!sfp.IsConfigured) return false;
			}

			return true;
		}

	#endregion

	#region static methods

		private static string TempFolderProjectKey(SavedFolderType folderType)
		{
			int idx = 1;
			string tempKey = PROJECT_PREFIX + "1";

			while (SetgMgr.Instance.ContainsName(tempKey, folderType))
			{
				tempKey = $"{PROJECT_PREFIX}{++idx:D}";
			}

			return tempKey;
		}
	#endregion

	#region system override methods

		public int CompareTo(SavedFolderProject other)
		{
			return Name.ToUpper().CompareTo(other.Name.ToUpper());
		}

		public bool Equals(SavedFolderProject other)
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