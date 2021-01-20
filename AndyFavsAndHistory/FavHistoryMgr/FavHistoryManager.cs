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
	public class FavHistoryManager<T> : INotifyPropertyChanged
		where T : class, ISavedFile, new()
		
	{
	#region private fields

		public bool isInitialized;

		private FilePath<FileNameSimple> filePath;

		private BaseDataFile<T> data;

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
			if (isInitialized) data.Admin.Write();

			data = new BaseDataFile<T>();
			data.Open(filePath);

			isInitialized = true;

			setView();
		}

		public void Add(string key, SavedFile value)
		{
			if (!isInitialized) return;

			data.Data.SavedFileList.Add(key, value);
		}

		public void Remove(string key)
		{
			if (!isInitialized) return;

			data.Data.SavedFileList.Remove(key);
		}

		public SavedFile Find(string key)
		{
			if (!isInitialized) return null;

			support.Select<T>(key, data);

			return data.Data.SavedFileList[key];
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