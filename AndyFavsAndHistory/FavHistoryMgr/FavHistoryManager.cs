#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  1/20/2021 6:00:00 AM

namespace AndyFavsAndHistory.FavHistoryMgr
{
	public class FavHistoryManager : INotifyPropertyChanged

	{
	#region private fields

		public bool isInitialized;

		private FilePath<FileNameSimple> filePath;

		private DataManager<FileListData> data;

		private ICollectionView view;

		private SavedFileListSupport support;

	#endregion

	#region ctor

		public FavHistoryManager() { }

	#endregion

	#region public properties

		public ICollectionView View
		{
			get => view;
			private set
			{
				view = value;

				OnPropertyChange();
			}
		}

		public FilePath<FileNameSimple> FilePath
		{
			get => filePath;
			set
			{
				filePath = value;
				OnPropertyChange();
			}
		}

		// public BaseDataFile<T> Data
		// {
		// 	get => data;
		// 	set
		// 	{
		// 		data = value;
		//
		// 		OnPropertyChange();
		// 	}
		// }

		public bool IsInitialized
		{
			get => isInitialized;
			set => isInitialized = value;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure(FilePath<FileNameSimple> filePath)
		{
			string x = UserSettings.Admin.Path.ClassVersionFromFile;

			if (isInitialized) data.Admin.Write();

			data = new DataManager<FileListData>();
			data.Open(filePath);

			isInitialized = true;

			setView();
		}

		public void Add(string key, FileListItem value)
		{
			if (!isInitialized) return;

			data.Data.FileList.Add(key, value);
		}

		public void Remove(string key)
		{
			if (!isInitialized) return;

			data.Data.FileList.Remove(key);
		}

		public FileListItem Find(string key)
		{
			if (!isInitialized) return null;

			support.Select(key, data);

			return data.Data.FileList[key];
		}

	#endregion

	#region private methods

		private void setView()
		{
			if (!isInitialized) return;

			View = CollectionViewSource.GetDefaultView(data);
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FaveHistoryManager";
		}

	#endregion
	}
}