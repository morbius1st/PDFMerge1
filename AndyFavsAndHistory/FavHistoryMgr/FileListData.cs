#region using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Data;
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
	public class FileListData : INotifyPropertyChanged
	{
		private int index;


		private CollectionView view;
		private ObservableDictionary<string, FileListItem> fileList =
			new ObservableDictionary<string, FileListItem>();

		public FileListData()
		{
			OnDeserialized();
		}

		[OnDeserialized]
		public void OnDeserialized()
		{
			setView();
		}

		[DataMember]
		public int Index
		{
			get => index;
			private set => index = value;
		}

		[IgnoreDataMember]
		public ICollectionView FileListView => view;

		[DataMember(Order = 10)]
		public ObservableDictionary<string, FileListItem> FileList => fileList;

		public void Add(string projectNumber, string name, FilePath<FileNameSimple> filePath)
		{

			fileList.Add(projectNumber + (Index++).ToString("D5"), new FileListItem(name, filePath));
		}

		public void ModifyName(string key, string name)
		{
			FileListItem sf;

			// bool result = fileList.TryGetValue(key, out sf);

			// if (result) sf.Name = name;
		}

		public void RemoveProject(string projectNumber)
		{
			CollectionView view = (CollectionView) CollectionViewSource.GetDefaultView(fileList);

			int len = projectNumber.Length;

			view.Filter = o => ((KeyValuePair<string, FileListItem>) o).Key.Substring(0, len).Equals(projectNumber);
		}

		private void setView()
		{
			view = (CollectionView) CollectionViewSource.GetDefaultView(fileList);
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

#endregion
}
