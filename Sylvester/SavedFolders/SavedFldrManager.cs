#region + Using Directives

#endregion


// projname: Sylvester.FolderSupport
// itemname: SavedFolderManager
// username: jeffs
// created:  1/20/2020 8:55:27 PM
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Sylvester.FileSupport;
using Sylvester.Settings;
using UtilityLibrary;
using static Sylvester.SavedFolders.SavedFolderType;

namespace Sylvester.SavedFolders
{
	public enum SavedFolderType
	{
		HISTORY = 0,
		FAVORITES = 1,
		COUNT = 2
	}

	public class SavedFolderManager : INotifyPropertyChanged
	{
		// will be two versions 
		// one for history
		// one for favorites

		private static SavedFoldersWin savedWinInstance;

//		private ObservableCollection<SavedProject> savedFolders;

//		private Dictionary<string, SavedProject> savedFolders;
//		private ICollectionView savedFolderVue;

		private SetgMgr sm;

		public SavedFolderManager() { }

		public SavedFolderManager(SavedFolderType index
//			,
//			ObservableCollection<SavedProject> savedFolders
			)
		{
			// before make savedfolderwin
			sm = SetgMgr.Instance;

			savedWinInstance = new SavedFoldersWin(this);

			Index = index;

//			this.savedFolders = savedFolders;
		}

//		public void Initialize()
//		{
//			Vue = CollectionViewSource.GetDefaultView(savedFolders);
//		}
//

//		public ICollectionView Vue
//		{
//			get => savedFolderVue;
//			set
//			{
//				savedFolderVue = value;
//				OnPropertyChange();
//			}
//		}

//		public ObservableCollection<SavedProject> SavedFolders => savedFolders;
		public ObservableCollection<SavedProject> SavedFolders => sm.SavedFolders[Index.Value()];

		public SavedFolderType Index { get; set; }

		public bool HasSavedFolders => sm.HasSavedFolders(Index);

		public static SavedFoldersWin SavedWinInstance => savedWinInstance;

		public void test()
		{
			bool? result = savedWinInstance.ShowDialog();
		}

		public bool AddProject(
			Route current,
			Route revision
			)
		{

			UserSettings.Data.priorPath = current;

			SavedProject sf = sm.FindSavedFolder(current.FolderNames[0], Index);
			SavedFolderPair cfp = new SavedFolderPair(current, revision);

			if (sf == null)
			{
				sf = new SavedProject(current.VolumeName, current.FolderNames[0]);
				sm.AddSavedFolder(sf, Index);
			}
			else
			{
				if (sm.FindCurrentRevisionFolderPair(sf, cfp.Key) != null) return false;
			}

			sf.SavedFolderPairs.Add(cfp);

			UserSettings.Admin.Write();

			OnPropertyChange("SavedFolders");

			savedWinInstance.CollectionUpdated();

//			savedWinInstance

			return true;
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}