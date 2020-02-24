using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Sylvester.Settings;
using Sylvester.Windows;
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
	public static class WinX
	{
		public static int TEST = 900;
	}

	/// <summary>
	/// Interaction logic for SavedFoldersWin.xaml
	/// </summary>
	public partial class SavedFoldersWin : Window, INotifyPropertyChanged
	{
		private Dictionary<string, string> z;

		public double MIN_WIDTH { get; } = 700;
		public double MIN_HEIGHT { get; }  = 450;



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

		public PropertyPath[] pp1 = new PropertyPath[]
		{
			new PropertyPath("[folder1]"),
			new PropertyPath("[folder2]"),

		};

		public PropertyPath ppx => pp1[1];


		public SavedFoldersWin(SavedFolderType index, string title)
		{
			InitializeComponent();

			Title = title;
			this.index = index;

			SetgMgr.GetSavedFolderLayout.Min_Height = MIN_HEIGHT;
			SetgMgr.GetSavedFolderLayout.Min_Height = MIN_WIDTH;
		}

	#region public properties

		public bool DebugMode
		{
			get => SetgMgr.DebugMode;
			set
			{
				SetgMgr.DebugMode = value;
				OnPropertyChange();
			}
		}

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

		// the selected project value;
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
			}
		}

		// the selected folder pair value;
		public SavedFolderPair SelectedFolderPair
		{
			get => selectedFolderPair;
			set
			{
				selectedFolderPair = value;
				OnPropertyChange();
			}
		}

		private string x1 = "folder1";

		public string x
		{
			get => x1;
			set
			{
				x1 = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region public methods

		public void CollectionUpdated()
		{
			lvProjects.Items.Refresh();

			OnPropertyChange("SavedFolders");
		}

	#endregion

	#region test methods

		private void Test01()
		{
			// add complete project test
			tbxMain.Clear();

			RaiseAddFavoriteEvent();
		}

	#endregion

	#region private methods

	#endregion

	#region window events

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			SetgMgr.SaveWindowLayout(WindowId.DIALOG_SAVED_FOLDERS, this);

			this.DialogResult = true;
			this.Close();
		}

		private void BtnDebugx_OnClick(object sender, RoutedEventArgs e)
		{
			x = x.Equals("folder1") ? "folder2" : "folder1";

			Debug.WriteLine("@savedfolderWin| debug");
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			DebugMode = !DebugMode;

			Debug.WriteLine("@savedfolderWin| debug");
		}

		private void BtnAddFav_OnClick(object sender, RoutedEventArgs e)
		{
//			Debug.WriteLine("@savedfolderWin| test");

			Test01();
		}

		private void SavedFolderWin_Initialized(object sender, EventArgs e)
		{
			SetgMgr.RestoreWindowLayout(WindowId.DIALOG_SAVED_FOLDERS, this);
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

	public class IconConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			ListDictionary icons = (ListDictionary) values[0];
			string index = (string) values[1];

			Viewbox c = (Viewbox) icons[index];

			return c;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw  new NotImplementedException();
		}
	}

}