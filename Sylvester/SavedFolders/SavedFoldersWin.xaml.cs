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
using System.Windows.Input;
using Sylvester.Process;
using Sylvester.SavedFolders.SubFolder;
using Sylvester.Settings;
using Sylvester.UserControls;
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

		public static double MIN_WIDTH { get; } = 700;
		public static double MIN_HEIGHT { get; }  = 450;

		public double WidthMin { get; } = MIN_WIDTH;
		public double HeightMin { get; } = MIN_HEIGHT;

		private SubFolderManager currentFolder = null;
		private SubFolderManager revisionFolder = null;

		public SavedFoldersWin(SavedFolderType index, string winTitle)
		{
			WinTitle = winTitle;
			this.index = index;

			InitializeComponent();

			SetgMgr.GetSavedFolderLayout.Min_Height = MIN_HEIGHT;
			SetgMgr.GetSavedFolderLayout.Min_Height = MIN_WIDTH;

			currentFolder = new SubFolderManager(FolderRouteCurrent);
			revisionFolder = new SubFolderManager(FolderRouteRevision);
		}

//		// upper listbox
//		public ObservableCollection<SavedFolderProject> savedFolders;

		// the selected value
		private SavedFolderProject selectedSavedFolderProject;

		// lower listbox
		public ObservableCollection<SavedFolderPair> folderPairs;

		// the selected value
		private SavedFolderPair selectedFolderPair;

		private SavedFolderType index;

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

		public bool Test => false;

		public string WinTitle { get; private set; }

		public SavedFolderType Index => index;

		// the project folder collection
		public ObservableCollection<SavedFolderProject> SavedFolders
		{
			get => SetgMgr.Instance.SavedFolders[index.Value()];
//			set
//			{
//				savedFolders = value;
//				OnPropertyChange();
//			}
		}

		// the folder pair collection - this is assigned after
		// a project folder is selected
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

				// assign the folder pair collection
				FolderPairs = value.SavedFolderPairs;

				OnPropertyChange("ProjectFolderIconIndex");

				SelectedFolderPair = null;
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

				currentFolder.Folder = selectedFolderPair?.Current ?? FilePath<FileNameSimple>.Invalid;
				revisionFolder.Folder = selectedFolderPair?.Revision ?? FilePath<FileNameSimple>.Invalid;

				OnPropertyChange("FolderPairIndex");
			}
		}

		// the project folder array index
		public string ProjectFolderIconIndex
		{
			get
			{
				if (selectedSavedFolderProject == null)
				{
					FolderProject.Visibility = Visibility.Hidden;
					return null;
				}

				FolderProject.Visibility = Visibility.Visible;

				if (selectedSavedFolderProject.Icon.IsVoid()) return App.Icon_FolderProjects[0];

				return selectedSavedFolderProject.Icon;
			}
			set
			{
				selectedSavedFolderProject.Icon = value;
				OnPropertyChange();

				UserSettings.Admin.Write();
			}
		}

		// the folder pair array index
		public string FolderPairIndex
		{
			get
			{
				if (selectedFolderPair == null)
				{
					FolderPair.Visibility = Visibility.Hidden;
					return null;
				}

				FolderPair.Visibility = Visibility.Visible;

				if (selectedFolderPair.Icon.IsVoid()) return App.Icon_FolderPairs[0];

				return selectedFolderPair.Icon;
			}
			set
			{
				selectedFolderPair.Icon = value;
				OnPropertyChange();

				UserSettings.Admin.Write();
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
			SetgMgr.WriteUsr();

			this.DialogResult = true;
			this.Close();
		}

		private int projectFolderIdx = 0;
		private int pairFolderIdx = 0;

		private void BtnDebugx_OnClick(object sender, RoutedEventArgs e)
		{
//			projectFolderIdx = projectFolderIdx > 3 ? 0 : projectFolderIdx;
//			ProjectFolderIconIndex = App.Icon_FolderProjects[projectFolderIdx++];
//
//			pairFolderIdx = pairFolderIdx > 4  ? 0 : pairFolderIdx;
//			FolderPairIndex = App.Icon_FolderPairs[pairFolderIdx++];

			SetgMgr sm = SetgMgr.Instance;


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

		private void ProjectFolderIconButton_OnClick(object sender, RoutedEventArgs e)
		{
			ProjectFolderIconIndex = ((CheckBox) sender).Tag.ToString();

			ProjectScrollViewer.Visibility = Visibility.Hidden;
		}

		private void FolderPairIconButton_OnClick(object sender, RoutedEventArgs e)
		{
			FolderPairIndex = ((CheckBox) sender).Tag.ToString();

			FolderPairScrollViewer.Visibility = Visibility.Hidden;
		}

		private void SavedFolderWin_Initialized(object sender, EventArgs e)
		{
			SetgMgr.RestoreWindowLayout(WindowId.DIALOG_SAVED_FOLDERS, this);
		}


		private void SavedFolderWin_Loaded(object sender, RoutedEventArgs e)
		{
//			SavedFolders = SetgMgr.Instance.SavedFolders[index.Value()];
		}

		private void FolderProject_OnClick(object sender, RoutedEventArgs e)
		{
			ProjectScrollViewer.Visibility = Visibility.Visible;
			ProjectScrollViewer.Focus();
		}

		private void ProjectScrollViewer_LostFocus(object sender, RoutedEventArgs e)
		{
			if (!withinScrollViewer)
				ProjectScrollViewer.Visibility = Visibility.Hidden;

			e.Handled = true;
		}

		private bool withinScrollViewer = false;

		private void ScrollViewer_OnMouseEnter(object sender, MouseEventArgs e)
		{
			withinScrollViewer = true;
		}

		private void ScrollViewer_OnMouseLeave(object sender, MouseEventArgs e)
		{
			withinScrollViewer = false;
		}

		private void FolderPair_OnClick(object sender, RoutedEventArgs e)
		{
			FolderPairScrollViewer.Visibility = Visibility.Visible;
			FolderPairScrollViewer.Focus();
		}

		private void FolderPairScrollViewer_OnLostFocus(object sender, RoutedEventArgs e)
		{
			if (!withinScrollViewer)
				FolderPairScrollViewer.Visibility = Visibility.Hidden;

			e.Handled = true;
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

	#region event handeling

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

	[ValueConversion(null, typeof(Viewbox))]
	public class IconConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values[0] == null || parameter == null ||
				values[0].GetType() != typeof(ListDictionary) || parameter.GetType() != typeof(string)) return null;

//			ListDictionary icons = (ListDictionary) values[0];
			string index = (string) (values[1] ?? parameter);

//			Viewbox c = (Viewbox) icons[index];
//
			Viewbox cx = new Viewbox();
			cx.Child =  (UIElement) Application.Current.Resources[index];

			return cx;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw  new NotImplementedException();
		}
	}

	[ValueConversion(null, typeof(Viewbox))]
	public class IconNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null || parameter.GetType() != typeof(string)) return null;

			string index = (string) (value ?? parameter);

			Viewbox cx = new Viewbox();
			cx.Child =  (UIElement) Application.Current.Resources[index];

			return cx;
		}

		public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
		{
			throw  new NotImplementedException();
		}
	}

	[ValueConversion(typeof(string), typeof(bool))]
	public class StringToBoolConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values[0] == null || values[1] == null ||
				values[0].GetType() != typeof(string) || values[1].GetType() != typeof(string)) return false;

			return ((string) values[0]).Equals((string) values[1]);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
//			throw  new NotImplementedException();
			return null;
		}
	}
}