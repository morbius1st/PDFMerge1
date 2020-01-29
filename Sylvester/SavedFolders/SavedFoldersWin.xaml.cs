using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sylvester.FileSupport;
using Sylvester.Settings;
using Sylvester.Support;
using UtilityLibrary;
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

		public ObservableCollection<SavedProject> savedFolders;
		private SavedProject selectedSavedProject;
		private ObservableCollection<SavedFolderPair> folderPairs;

		private SavedFolderPair selectedFolderPair;

		private SavedFoldersDebugSupport sfds = SavedFoldersDebugSupport.Instance;

		private string currentPath;
		private string revisionPath;

		public SavedFolderManager sfMgr { get; private set; }

		public SavedFoldersWin(SavedFolderManager sfMgr)
		{
			InitializeComponent();

			this.sfMgr = sfMgr;

			SavedFolders = sfMgr.SavedFolders;

		}

	#region public properties

//		public Dictionary<string, SavedProject> SavedFolders => SetgMgr.Instance.SavedProjectFolders;
		public ObservableCollection<SavedProject> SavedFolders
		{
			get => savedFolders;
			set
			{
				savedFolders = value;
				OnPropertyChange();
			}
		}

		public SavedProject SelectedSavedProject
		{
			private get { return selectedSavedProject; }
			set
			{
				Append(nl);
				AppendLineFmt("selected", value.Name);

				selectedSavedProject = value;

				OnPropertyChange();

				FolderPairs = value.SavedFolderPairs;

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

				CurrentPath = selectedFolderPair.Current.FullPath;
				RevisionPath = selectedFolderPair.Revision.FullPath;
			}
		}


	#region saved project public properties

		public string Volume { get; set; }






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

			sfds.Test_02(SAVED);

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
