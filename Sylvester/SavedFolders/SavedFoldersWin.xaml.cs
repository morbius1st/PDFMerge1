using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Sylvester.Settings;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

using static Sylvester.SavedFolders.SavedFolderType;



/*
purpose
1. select favorite folders that have been saved
2. create a new favorite
3. select a historical folder (max of 10?)
4. save a historical filder
*/

// functions needed
// add project
// add revision pair
// remove revision pair
// remove project
// show / edit / select current folder
// show / edit / select revision folder

namespace Sylvester.SavedFolders
{
	/// <summary>
	/// Interaction logic for SavedFoldersWin.xaml
	/// </summary>
	public partial class SavedFoldersWin : Window, INotifyPropertyChanged
	{
		// upper listbox
		public ObservableCollection<SavedFolderProject> savedFolders;
		// the selected value
		private SavedFolderProject selectedSavedFolderProject;

		// lower listbox
		public ObservableCollection<SavedFolderPair> folderPairs;
		// the selected value
		private SavedFolderPair selectedFolderPair;

		private string currentPath;
		private string revisionPath;
		private string volume;
		private string rootFolder;

		private SavedFolderType index;

		public SavedFoldersWin(SavedFolderType index, string title)
		{
			InitializeComponent();

			Title = title;
			this.index = index;

		}

	#region public properties

		public string Title { get; private set; }


		public ObservableCollection<SavedFolderProject> SavedFolders
		{
			get => SetgMgr.Instance.SavedFolders[index.Value()];
			set
			{
				savedFolders = value;
				OnPropertyChange();
			}
		}

		public ObservableCollection<SavedFolderPair> FolderPairs
		{
			get => folderPairs;
			set
			{
				folderPairs = value;
				OnPropertyChange();
			}
		}

		// the selected value;
		public SavedFolderProject SelectedSavedFolderProject
		{
			private get { return selectedSavedFolderProject; }
			set
			{
				Append(nl);
				AppendLineFmt("selected", value.Name);

				selectedSavedFolderProject = value;

				OnPropertyChange();

				FolderPairs = value.SavedFolderPairs;

				Volume = value.Identifier.Volume;
				RootFolder = value.Identifier.ProjectFolder;
			}
		}

		// the selected value;
		public SavedFolderPair SelectedFolderPair
		{
			get => selectedFolderPair;
			set
			{
				selectedFolderPair = value;
				OnPropertyChange();

				CurrentPath = selectedFolderPair.Current.GetFullPath;
				RevisionPath = selectedFolderPair.Revision.GetFullPath;
			}
		}

	#region saved project public properties


		public string Volume
		{
			get => volume;
			set
			{
				volume = value;
				OnPropertyChange();
			}
		}

		public string RootFolder
		{
			get => rootFolder;
			set
			{
				rootFolder = value;
				OnPropertyChange();
			}
		}

	#endregion


	#region saved folder pair properties

		public string CurrentPath
		{
			get => currentPath;

			set
			{
				currentPath = value;
				OnPropertyChange();
			}
		}

		public string RevisionPath
		{
			get => revisionPath;

			set
			{
				revisionPath = value;
				OnPropertyChange();
			}
		}

	#endregion

	#endregion

	#region public methods

		public void CollectionUpdated()
		{
			lvProjects.Items.Refresh();

			OnPropertyChange("SavedFolders");
		}

	#endregion

	#region private methods

		private void Test01()
		{
			// add complete project test
			tbxMain.Clear();

			RaiseAddFavoriteEvent();

//			sfds.Test_02(HISTORY);
//
//			CollectionUpdated();
		}

	#endregion


	#region window events

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@savedfolderWin| debug");
		}

		private void BtnAddFav_OnClick(object sender, RoutedEventArgs e)
		{
//			Debug.WriteLine("@savedfolderWin| test");

			Test01();
		}


		private void SavedFolderWin_Loaded(object sender, RoutedEventArgs e)
		{
			SavedFolders = SetgMgr.Instance.SavedFolders[index.Value()];
		}
		

	#endregion

	#region events

		public delegate void AddFavoriteEventHandler(object sender, EventArgs e);

		public event AddFavoriteEventHandler AddFavorite;

		protected virtual void RaiseAddFavoriteEvent()
		{
			AddFavorite?.Invoke(this, new EventArgs());
		}

	#endregion
	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region debug routines

	#if DEBUG
		public void Append(string msg)
		{
			tbxMain.AppendText(msg);
		}

		public void AppendLine(string msg)
		{
			Append(msg + nl);
		}

		public void AppendLineFmt(string msg1, string msg2 = "")
		{
			AppendLine(logMsgDbS(msg1, msg2));
		}
#endif

		#endregion


	}
}
