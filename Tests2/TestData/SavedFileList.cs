#region using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Data;
using SettingsManager;
using Tests2.DataStore;
using UtilityLibrary;

#endregion

// in code, after creating the data file for the first time, set the
// header values for 
// {dataset}.Info.Description
// {dataset}.Info.DataClassVersion
// {dataset}.Info.Notes
// the meaning of all three are up to you, however, the dataclass version
// is used to determine if the dataset has been revised and needs an upgrade

namespace Tests2.TestData
{

#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	
	public class SavedFileList : HeaderData, INotifyPropertyChanged
	{
	[IgnoreDataMember]
	public string DataFileDescription { get; set; } = "Sample saved file list";

	[IgnoreDataMember]
	public string DataFileNotes { get; set; } = "testing saved file list";
		
	[IgnoreDataMember]
	public string DataFileVersion { get; set; } = "0.1";
		private int index;

		private ObservableDictionary<string, SavedFile> savedClassfFiles =
			new ObservableDictionary<string, SavedFile>();

		private CollectionView view;

		public SavedFileList()
		{
			OnDeserialized();
		}

		[OnDeserialized()]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			OnDeserialized();
		}

		private void OnDeserialized()
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
		public CollectionView View
		{
			get => view;
			set
			{
				view = value;
				OnPropertyChanged();
			}
		}

		[DataMember(Order = 10)]
		public ObservableDictionary<string, SavedFile> SavedClassfFiles => savedClassfFiles;

		public void Add(string projectNumber, string name, FilePath<FileNameSimple> filePath)
		{
			savedClassfFiles.Add(projectNumber + "-" + (Index++).ToString("D5"), new SavedFile(name, filePath));
		}

		private void setView()
		{
			view = (CollectionView) CollectionViewSource.GetDefaultView(savedClassfFiles);
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

#endregion
}
