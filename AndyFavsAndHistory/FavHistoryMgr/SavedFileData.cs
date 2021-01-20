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

	public interface ISavedFile
	{
		ObservableDictionary<string, SavedFile> SavedFileList { get; }
		int Index { get; }
		ICollectionView View { get; }
	}



#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	
	public class SavedFileData : INotifyPropertyChanged, ISavedFile
	{
		private int index;

		private ObservableDictionary<string, SavedFile> savedFileList =
			new ObservableDictionary<string, SavedFile>();

		private CollectionView view;

		public SavedFileData()
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
		public ICollectionView View => view;

		[DataMember(Order = 10)]
		public ObservableDictionary<string, SavedFile> SavedFileList => savedFileList;

		public void Add(string projectNumber, string name, FilePath<FileNameSimple> filePath)
		{
			savedFileList.Add(projectNumber + (Index++).ToString("D5"), new SavedFile(name, filePath));
		}

		public void ModifyName(string key, string name)
		{
			SavedFile sf;

			bool result = savedFileList.TryGetValue(key, out sf);

			if (result) sf.Name = name;
		}

		public void RemoveProject(string projectNumber)
		{
			CollectionView view = (CollectionView) CollectionViewSource.GetDefaultView(savedFileList);

			int len = projectNumber.Length;

			view.Filter = o => ((KeyValuePair<string, SavedFile>) o).Key.Substring(0, len).Equals(projectNumber);
		}

		private void setView()
		{
			view = (CollectionView) CollectionViewSource.GetDefaultView(savedFileList);
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

#endregion
}
