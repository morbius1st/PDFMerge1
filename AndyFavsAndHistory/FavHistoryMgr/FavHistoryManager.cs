#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
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

		private DataManager<FileList> data;

		private ICollectionView view;

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

		public FilePath<FileNameSimple> FilePath => 
			new FilePath<FileNameSimple>(data.Path.SettingFilePath);

		public bool IsInitialized
		{
			get => isInitialized;
			set => isInitialized = value;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure( FilePath<FileNameSimple> filePath, string fileName, string[] folders)
		{

			if (isInitialized) data.Admin.Write();

			data = new DataManager<FileList>(filePath);
			data.Open(filePath, fileName, folders);

			isInitialized = true;

			setView();
		}

		public void Add(FileListKey key, FileListItem value)
		{
			if (!isInitialized) return;

			keyCheck(key);
			valueCheck(value);

			if (!isInitialized) return;

			data.Data.FileListItems.Add(key, value);
		}

		public void Remove(FileListKey key)
		{
			if (!isInitialized) return;

			keyCheck(key);

			data.Data.FileListItems.Remove(key);
		}

		public bool Select(FileListKey key, out FileListItem item)
		{
			keyCheck(key);

			FileListItem match;
			item = null;

			bool result = data.Data.FileListItems.TryGetValue(key, out match);

			if (result) item = match;

			return result;
		}

		
		// public void FilterByProject(string projectNumber, BaseDataFile<FileList> fld)
		//
		// {
		// 	projNumCheck(projectNumber);
		//
		// 	view = (CollectionView) CollectionViewSource.GetDefaultView(fld.Data.FileListItems);
		//
		// 	int len = projectNumber.Length;
		//
		// 	view.Filter = o =>  ((KeyValuePair<string, FileListItem>) o).Key.Substring(0, len).Equals(projectNumber);
		//
		// 	OnPropertyChange("View");
		// }


		public void RemoveByProject(string projectNumber)

		{
			projNumCheck(projectNumber);

			KeyValuePair<FileListKey, FileListItem>[] selected =
				data.Data.FileListItems.Where(kvp => kvp.Key.ProjectNumber.Equals(projectNumber)).ToArray();

			if (selected.Length <= 0) return;

			foreach (KeyValuePair<FileListKey, FileListItem> kvp in selected)
			{
				data.Data.FileListItems.Remove(kvp.Key);
			}
		}


	#endregion

	#region private methods

		private void setView()
		{
			if (!isInitialized) return;

			View = CollectionViewSource.GetDefaultView(data);
		}

		
		private static void keyCheck(FileListKey key)
		{
			if (key == null) throw new InvalidEnumArgumentException("FavAndHistory: Key");
		}
				
		private static void valueCheck(FileListItem item)
		{
			if (item == null) throw new InvalidEnumArgumentException("FavAndHistory: Value");
		}

		private static void projNumCheck(string projNumber)
		{
			if (projNumber.IsVoid() || projNumber.Length != 7) throw new InvalidEnumArgumentException("ProjectNumber");
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