#region using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Data;
using SettingsManager;
using UtilityLibrary;

#endregion

// in code, after creating the data file for the first time, set the
// header values for 
// {dataset}.Info.Description
// {dataset}.Info.DataClassVersion
// {dataset}.Info.Notes
// the meaning of all three are up to you, however, the dataclass version
// is used to determine if the dataset has been revised and needs an upgrade

namespace AndyFavsAndHistory.FavHistoryMgr
{


#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class FileList : INotifyPropertyChanged, IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "data 7.4u";

		[IgnoreDataMember]
		public string DataFileDescription => "data setting file for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "data / any notes go here";


		private CollectionView view;
		private ObservableDictionary<FileListKey, FileListItem> fileListItems =
			new ObservableDictionary<FileListKey, FileListItem>();

		public FileList()
		{
			OnDeserialized();
		}

		[OnDeserialized]
		public void OnDeserialized(StreamingContext context)
		{
			OnDeserialized();
		}

		public void OnDeserialized()
		{
			if (fileListItems == null) fileListItems = new ObservableDictionary<FileListKey, FileListItem>();
		}

		[DataMember]
		public int UniqueIndex
		{
			get => FileListKey.uniqueIndex;
			set => FileListKey.uniqueIndex = value;
		}

		[IgnoreDataMember]
		public ICollectionView FileListView => view;

		[DataMember(Order = 10)]
		public ObservableDictionary<FileListKey, FileListItem> FileListItems => fileListItems;

		public void Add(FileListKey key, FileListItem item)
		{

			fileListItems.Add(key, item);
		}


		public void ModifyName(string key, string name)
		{
			FileListItem sf;

			// bool result = fileList.TryGetValue(key, out sf);

			// if (result) sf.Name = name;
		}

		// public void RemoveProject(string projectNumber)
		// {
		// 	CollectionView view = (CollectionView) CollectionViewSource.GetDefaultView(fileListItems);
		//
		// 	int len = projectNumber.Length;
		//
		// 	view.Filter = o => ((KeyValuePair<string, FileListItem>) o).Key.Substring(0, len).Equals(projectNumber);
		// }

		private void setView()
		{
			view = (CollectionView) CollectionViewSource.GetDefaultView(fileListItems);
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

#endregion
}
