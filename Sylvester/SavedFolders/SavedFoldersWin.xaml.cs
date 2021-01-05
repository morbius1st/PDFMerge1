using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using SettingsManager;
using Sylvester.SavedFolders.SubFolder;
using Sylvester.Windows;
using UtilityLibrary;

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
	public enum FolderProjectOp
	{
		NONE = -1,
		ADD_PROJECT = 0,
		DELETE_PROJECT = 1
	}

	/// <summary>
	/// Interaction logic for SavedFoldersWin.xaml
	/// </summary>
	public partial class SavedFoldersWin : Window, INotifyPropertyChanged
	{

	#region private fields

		private string winTitle;

		private SavedFolderOperation savedFolderOperation = SavedFolderOperation.MANAGEMENT;

		private SubFolderManager currentFolder = null;
		private SubFolderManager revisionFolder = null;

		// the selected project value
		private SavedFolderProject selectedFolderProject;

		// the selected pair value
		private SavedFolderPair selectedFolderPair;

		// lower listbox
		private ObservableCollection<SavedFolderPair> folderPairs;

		// mouse is within the scroll viewer - 
		// switch for lost focus of scroll viewer
		private bool withinScrollViewer = false;

		// the name of a newly created project and/or folder pair entry
		private string NewProjectName;
		private string NewFolderPairName;

		private FolderProjectOp folderProjectOp = FolderProjectOp.NONE;

		private int selectedFolderProjectIdx;

		private DispatcherTimer dispatcherTimer;
		private string message = null;

		private ICollectionView projects; 

	#endregion

		public SavedFoldersWin(SavedFolderType savedFolderType, string winTitle)
		{
			this.winTitle = winTitle;
			SavedFolderType = savedFolderType;

			SetgMgr.GetSavedFolderLayout.Min_Height = MIN_HEIGHT;
			SetgMgr.GetSavedFolderLayout.Min_Height = MIN_WIDTH;

			InitializeComponent();

			currentFolder = new SubFolderManager(FolderRouteCurrent);
			revisionFolder = new SubFolderManager(FolderRouteRevision);

			currentFolder.PropertyChanged += CurrentFolderOnPropertyChanged;
			revisionFolder.PropertyChanged += RevisionFolderOnPropertyChanged;

			dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += DispatcherTimerOnTick;
			dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 4);
		}

	#region public properties

		public static double MIN_WIDTH { get; } = 1000;
		public static double MIN_HEIGHT { get; }  = 600;


//		public bool DebugMode
//		{
//			get => SetgMgr.DebugMode;
//			set
//			{
//				SetgMgr.DebugMode = value;
//				OnPropertyChange();
//			}
//		}

		public string WinTitle
		{
			get
			{
				string title = "";

				return winTitle + " " +
					AppSettingData.SavedFolderOperationDesc[(int) savedFolderOperation, 0];
			}
		}

		public SavedFolderOperation SavedFolderOperation
		{
			get => savedFolderOperation;
			set
			{
				savedFolderOperation = value;

				OnPropertyChange();
				OnPropertyChange("WinTitle");

				Message = AppSettingData.SavedFolderOperationDesc[(int) savedFolderOperation, 1];
			}
		}

		// the project folder collection
//		public ObservableCollection<SavedFolderProject> SavedFolders
		public ICollectionView SavedFolders
		{
			get
			{
				projects = CollectionViewSource.GetDefaultView(SetgMgr.Instance.SavedFolders[(int) SavedFolderType]);

				if (SavedFolderType == SavedFolderType.HISTORY)
				{
					projects.SortDescriptions.Add(new SortDescription("DateTime", ListSortDirection.Ascending));
				}
				else
				{
					projects.SortDescriptions.Add(new SortDescription("UseCount", ListSortDirection.Ascending));
					projects.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

				}


				return projects;
//				SetgMgr.Instance.SavedFolders[(int) SavedFolderType];
			}
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
		public SavedFolderProject SelectedFolderProject
		{
			get { return selectedFolderProject; }
			set
			{
				if (selectedFolderProject != null && value != null &&
					value.Equals(selectedFolderProject)) return;

				if (folderProjectOp == FolderProjectOp.ADD_PROJECT)
				{
					return;
				}

				selectedFolderProject = value;

				if (value != null)
				{
					// assign the folder pair collection
					FolderPairs = value.SavedFolderPairs;
				}
				else
				{
					FolderPairs = null;
				}

				SelectedFolderPair = null;

				OnPropertyChange();
				OnPropertyChange("ProjectFolderIconIndex");
			}
		}

		// the selected folder pair value;
		public SavedFolderPair SelectedFolderPair
		{
			get => selectedFolderPair;
			set
			{
				selectedFolderPair = value;

				if (currentFolder != null)
				{
					currentFolder.Folder = selectedFolderPair?.Current ?? FilePath<FileNameSimple>.Invalid;
				}

				if (revisionFolder != null)
				{
					revisionFolder.Folder = selectedFolderPair?.Revision ?? FilePath<FileNameSimple>.Invalid;
				}

				OnPropertyChange();
				OnPropertyChange("FolderPairIndex");
			}
		}


		// the project folder array index
		public string ProjectFolderIconIndex
		{
			get
			{
				if (selectedFolderProject == null)
				{
					FolderProject.Visibility = Visibility.Hidden;
					return null;
				}

				FolderProject.Visibility = Visibility.Visible;

				if (selectedFolderProject.Icon.IsVoid()) return App.Icon_FolderProjects[0];

				return selectedFolderProject.Icon;
			}
			set
			{
				selectedFolderProject.Icon = value;
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

		public SavedFolderType SavedFolderType {  get; }

		public string Message
		{
			get => message;

			set
			{
				message = value;
				OnPropertyChange();

				if (message != null)
				{
					TblkMessage.Tag = "fadein";
					if (SavedFolderOperation == SavedFolderOperation.MANAGEMENT)
					{
						dispatcherTimer.Start();
					}
				}
				else
				{
					TblkMessage.Tag = "solid";
				}
			}
		}

		public FolderProjectOp FolderProjectOp
		{
			get => folderProjectOp;
			private set
			{
				folderProjectOp = value;
				OnPropertyChange();
			}
		}

		public bool CanSave
		{
			get
			{
				if (FolderProjectOp != FolderProjectOp.ADD_PROJECT) return false;

				return selectedFolderProject.IsConfigured && selectedFolderPair.IsConfigured;
			}
		}

	#endregion

	#region public methods

	#endregion

	#region private methods

		// delete the currently selected project folder
		private void deleteFolderProject()
		{
			if (selectedFolderProject != null)
			{
				FolderPairs.Clear();
				SelectedFolderPair = null;

				SetgMgr.Instance.DeleteSavedProjectFolder(selectedFolderProject, SavedFolderType);

				SelectedFolderProject = null;

				SetgMgr.WriteUsr();
			}
		}

	#endregion

	#region control events

		// operation button events

		private void BtnDebugx_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@savedfolderWin| debug");
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			SetgMgr sm = SetgMgr.Instance;

			ListView lv = lvProjects;

			lv.SelectedIndex = 0;

			Debug.WriteLine("@savedfolderWin| debug");
		}

		private void BtnClose_OnClick(object sender, RoutedEventArgs e)
		{
			SetgMgr.SaveWindowLayout(WindowId.DIALOG_SAVED_FOLDERS, this);
			SetgMgr.WriteUsr();

			this.DialogResult = false;
			this.Close();
		}

		private void BtnSelect_OnClick(object sender, RoutedEventArgs e)
		{
			SetgMgr.SaveWindowLayout(WindowId.DIALOG_SAVED_FOLDERS, this);

			selectedFolderProject.UseCount += 1;

			selectedFolderPair.ParentKey = selectedFolderProject.Key;

			SetgMgr.WriteUsr();

			this.DialogResult = true;
			this.Close();
		}


		// folder project
		private void BtnAddFolderProject_OnClick(object sender, RoutedEventArgs e)
		{
			selectedFolderProjectIdx = lvProjects.Items.Count;
			selectedFolderProject = null;

			SelectedFolderProject =
				SetgMgr.Instance.CreateFolderProject(SavedFolderType);

//			FolderPairs = SelectedFolderProject.SavedFolderPairs;

			SelectedFolderPair =
				selectedFolderProject.SavedFolderPairs[0];

			FolderProjectOp = FolderProjectOp.ADD_PROJECT;
		}

		private void BtnAddToFavorites_OnClick(object sender, RoutedEventArgs e)
		{
			Message = "Adding to Favorites";

			RaiseAddFavoriteEvent();
		}

		private void BtnDeleteFolderProject_OnClick(object sender, RoutedEventArgs e)
		{
			FolderProjectOp = FolderProjectOp.DELETE_PROJECT;

			deleteFolderProject();

			FolderProjectOp = FolderProjectOp.NONE;
		}

		private void BtnBtnSaveNewProject_OnClick(object sender, RoutedEventArgs e)
		{
			SetgMgr.WriteUsr();

			FolderProjectOp = FolderProjectOp.NONE;
		}

		private void BtnCancelAddeNewProject_OnClick(object sender, RoutedEventArgs e)
		{
			deleteFolderProject();

			FolderProjectOp = FolderProjectOp.NONE;

			Message = "";
		}


		// folder pair
		private void BtnAddFolderPair_OnClick(object sender, RoutedEventArgs e)
		{
			if (selectedFolderProject == null) return;

			SetgMgr.Instance.AddFolderPair(selectedFolderProject,
				null, null);

			SetgMgr.WriteUsr();
		}

		private void BtnCopyFolderPair_OnClick(object sender, RoutedEventArgs e)
		{
			if (selectedFolderProject == null ||
				selectedFolderPair == null ) return;

			SavedFolderPair pair =
				SetgMgr.Instance.CopyFolderPair(selectedFolderProject, selectedFolderPair);

			folderPairs.Add(pair);

			SetgMgr.WriteUsr();
		}

		private void BtnDeleteFolderPair_OnClick(object sender, RoutedEventArgs e)
		{
			if (selectedFolderPair == null) return;

			SetgMgr.Instance.DeleteFolderPair(selectedFolderProject, selectedFolderPair);

			SelectedFolderPair = null;

			SetgMgr.WriteUsr();
		}


		// controls button's events
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

		private void FolderPair_OnClick(object sender, RoutedEventArgs e)
		{
			FolderPairScrollViewer.Visibility = Visibility.Visible;
			FolderPairScrollViewer.Focus();
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


		// control events
		private void ScrollViewer_OnMouseEnter(object sender, MouseEventArgs e)
		{
			withinScrollViewer = true;
		}

		private void ScrollViewer_OnMouseLeave(object sender, MouseEventArgs e)
		{
			withinScrollViewer = false;
		}

		private void FolderPairScrollViewer_OnLostFocus(object sender, RoutedEventArgs e)
		{
			if (!withinScrollViewer)
				FolderPairScrollViewer.Visibility = Visibility.Hidden;

			e.Handled = true;
		}

		private void TbxProjInfoName_OnKeyUp(object sender, KeyEventArgs e)
		{
			TextBox t = sender as TextBox;

			if (Validation.GetHasError(t))
			{
				Validation.ClearInvalid(t.GetBindingExpression(TextBox.TextProperty));
			}


			if (e.Key == Key.Enter || e.Key == Key.Return)
			{
				TbxProjInfoName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
			}
			else if (e.Key == Key.Escape)
			{
				TbxProjInfoName.Text = selectedFolderProject.Name;
			}
		}

		private void TxbFolderPairName_OnKeyUp(object sender, KeyEventArgs e)
		{
			TextBox t = sender as TextBox;

			if (Validation.GetHasError(t))
			{
				Validation.ClearInvalid(t.GetBindingExpression(TextBox.TextProperty));
			}

			if (e.Key == Key.Enter || e.Key == Key.Return)
			{
				BindingExpression b = TxbFolderPairName.GetBindingExpression(TextBox.TextProperty);

				TxbFolderPairName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
			}
			else if (e.Key == Key.Escape)
			{
				TxbFolderPairName.Text = selectedFolderPair.Name;
			}
		}

		private void LvProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			if (folderProjectOp == FolderProjectOp.ADD_PROJECT &&
				lvProjects.SelectedIndex != selectedFolderProjectIdx
				)
			{
				Message = " You must save or cancel the new project before you can close this window";
				lvProjects.SelectedIndex = selectedFolderProjectIdx;
			}
		}

	#endregion

	#region window events

		// window events
		private void SavedFolderWin_Initialized(object sender, EventArgs e)
		{
			SetgMgr.RestoreWindowLayout(WindowId.DIALOG_SAVED_FOLDERS, this);
		}

		private void SavedFolderWin_Loaded(object sender, RoutedEventArgs e)
		{
//			SavedFolders = SetgMgr.Instance.SavedFolders[index.Value()];
		}

		private void SavedFolderWin_Closing(object sender, CancelEventArgs e)
		{
			if (folderProjectOp == FolderProjectOp.ADD_PROJECT)
			{
				e.Cancel = true;
				Message = " You must save or cancel the new project before you can close this window";
			}
		}

	#endregion

	#region events

		private void CurrentFolderOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("Folder"))
			{
				SelectedFolderPair.Current = currentFolder.Folder;

				OnPropertyChange("CanSave");
			}
		}

		private void RevisionFolderOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("Folder"))
			{
				SelectedFolderPair.Revision = revisionFolder.Folder;

				OnPropertyChange("CanSave");
			}
		}

		private void DispatcherTimerOnTick(object sender, EventArgs e)
		{
			//Things which happen after 1 timer interval
			message = null;
			TblkMessage.Tag = "solid";

			//Disable the timer
			dispatcherTimer.IsEnabled = false;
		}

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

		private void TbxProjInfoName_TextChanged(object sender, TextChangedEventArgs e)
		{

		}
	}


	public class SavedFolderInformation : DependencyObject
	{
		public static readonly DependencyProperty SavedFolderTypeProperty = DependencyProperty.Register(
			"SavedFolderType", typeof(SavedFolderType), typeof(SavedFolderInformation),
			new PropertyMetadata(default(SavedFolderType)));

		public SavedFolderType SavedFolderType
		{
			get { return (SavedFolderType) GetValue(SavedFolderTypeProperty); }
			set { SetValue(SavedFolderTypeProperty, value); }
		}

		public static readonly DependencyProperty SavedFolderCategoryProperty = DependencyProperty.Register(
			"SavedFolderCategory", typeof(SavedFolderCategory), typeof(SavedFolderInformation),
			new PropertyMetadata(default(SavedFolderCategory)));

		public SavedFolderCategory SavedFolderCategory
		{
			get { return (SavedFolderCategory) GetValue(SavedFolderCategoryProperty); }
			set { SetValue(SavedFolderCategoryProperty, value); }
		}

		public static readonly DependencyProperty SavedFolderProjectProperty = DependencyProperty.Register(
			"SavedFolderProject", typeof(SavedFolderProject), typeof(SavedFolderInformation),
			new PropertyMetadata(default(SavedFolderProject)));

		public SavedFolderProject SavedFolderProject
		{
			get { return (SavedFolderProject) GetValue(SavedFolderProjectProperty); }
			set { SetValue(SavedFolderProjectProperty, value); }
		}

		public static readonly DependencyProperty OriginalNameProperty = DependencyProperty.Register(
			"OriginalName", typeof(string), typeof(SavedFolderInformation), new PropertyMetadata(default(string)));

		public string OriginalName
		{
			get { return (string) GetValue(OriginalNameProperty); }
			set { SetValue(OriginalNameProperty, value); }
		}
	}

	public class DuplicateNameRule : ValidationRule
	{
		public SavedFolderInformation SavedFolderInformation { get; set; }


		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			bool result;
			string name = (string) value;

			if (name.Equals(SavedFolderInformation.OriginalName))
			{
				result = false;
			}
			else if	(SavedFolderInformation.SavedFolderCategory == SavedFolderCategory.FOLDER_PROJECT)
			{
				result = SetgMgr.Instance.ContainsName(name, SavedFolderInformation.SavedFolderType);
			}
			else
			{
				result = SetgMgr.Instance.ContainsName(SavedFolderInformation.SavedFolderProject, name);
			}

			if (result)
				return new ValidationResult(false,
					"Duplicate Name Entered.  Please Provide a Unique Name.");

			return ValidationResult.ValidResult;
		}
	}

	public class BindingProxy : System.Windows.Freezable
	{
		protected override Freezable CreateInstanceCore()
		{
			return new BindingProxy();
		}

		public object Data
		{
			get => (object) GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy),
				new PropertyMetadata(null));
	}

	[ValueConversion(null, typeof(Double))]
	public class InnerWidthConverter : IValueConverter
	{
		// value is the basic width (usually the actualwidth)
		// parameter is the the left and right margin amount
		// returned number is basic width - (2 * margin amount)
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (
				value == null || value.GetType() != typeof(Double) ||
				parameter == null || parameter.GetType() != typeof(string)
				) return null;

			Double width = (Double) (value);

			bool result = double.TryParse((string) parameter, out double margin  );

			if (!result) { margin = 0; }

			return width - (margin * 2);
		}

		public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
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

	[ValueConversion(typeof(int), typeof(bool))]
	public class IntGreaterThanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || value.GetType() != typeof(int)) return false;
			if (parameter == null || parameter.GetType() != typeof(string)) return false;

			int operand = (int) (value);
			int test;

			if (!Int32.TryParse((string) parameter, out test)) return false;

			bool result = operand > test;

			return result;
		}

		public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
		{
			throw  new NotImplementedException();
		}
	}

	[ValueConversion(typeof(string), typeof(bool))]
	public class StringEqualToBoolConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values[0] == null || values[1] == null ||
				values[0].GetType() != typeof(string) || values[1].GetType() != typeof(string)) return false;

			return ((string) values[0]).Equals((string) values[1]);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
	
//	[ValueConversion(typeof(string), typeof(bool))]
//	public class MultiObjectEqualOrToBoolConverter : IMultiValueConverter
//	{
//		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
//		{
//			if (parameter == null) return false;
//
//			bool result = false;
//
//			foreach (object value in values)
//			{
//				if (value == null) return false;
//
//				result |= value.Equals(parameter);
//			}
//
//			return result;
//		}
//
//		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
//		{
////			throw  new NotImplementedException();
//			return null;
//		}
//	}


//	[ValueConversion(null, typeof(Double))]
//	public class DoubleLessThenConverter : IMultiValueConverter
//	{
//		// value[0] is the primary number
//		// value[1] is the test number
//		// returned number is the smaller of the two
//		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
//		{
//			if (value == null) return null;
//
//			if (
//				value[0].GetType() != typeof(Double) &&
//				value[1].GetType() != typeof(Double)
//				) return null;
//
//			if (value[0].GetType() != typeof(Double))
//			{
//				return (Double) (value[1]);
//			}
//			else if (value[1].GetType() != typeof(Double))
//			{
//				return (Double) (value[0]);
//			}
//
//			Double primary = (Double) (value[0]);
//			Double test = (Double) (value[1]);
//
//			Double result = primary < test ? primary : test;
//
//			return result;
//		}
//
//		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
//		{
//			throw  new NotImplementedException();
//		}
//	}




}