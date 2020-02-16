using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using static UtilityLibrary.MessageUtilities;

using static Sylvester.SavedFolders.SavedFolderType;

namespace Sylvester.SavedFolders
{
	/// <summary>
	/// Interaction logic for SavedFoldersWin.xaml
	/// </summary>
	public partial class SavedFoldersWin : Window, INotifyPropertyChanged
	{
		// functions needed
		// add project
		// add revision pair
		// remove revision pair
		// remove project
		// show / edit / select current folder
		// show / edit / select revision folder

		public ObservableCollection<SavedFolderProject> savedFolders;
		private SavedFolderProject selectedSavedFolderProject;
		private ObservableCollection<SavedFolderPair> folderPairs;

		private SavedFolderPair selectedFolderPair;

		private SavedFoldersDebugSupport sfds = SavedFoldersDebugSupport.Instance;


		private string currentPath;
		private string revisionPath;
		private string volume;
		private string rootFolder;

		public SavedFolderManager sfMgr { get; private set; }

		public SavedFoldersWin(SavedFolderManager sfMgr)
		{
			InitializeComponent();

			this.sfMgr = sfMgr;

			SavedFolders = sfMgr.SavedFolders;

		}

	#region public properties

		public ObservableCollection<SavedFolderProject> SavedFolders
		{
			get => savedFolders;
			set
			{
				savedFolders = value;
				OnPropertyChange();
			}
		}

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
				RootFolder = value.Identifier.RootFolder;
			}
		}

		public ObservableCollection<SavedFolderPair> FolderPairs
		{
			get { return folderPairs; }
			set
			{
				folderPairs = value;
				OnPropertyChange();
			}
		}

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

		public void CollectionUpdated()
		{
			lvProjects.Items.Refresh();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@savedfolderWin| debug");
		}

		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@savedfolderWin| test");

			Test01();
		}

		private void Test01()
		{
			// add complete project test
			tbxMain.Clear();

			sfds.Test_02(HISTORY);

			CollectionUpdated();
		}


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
