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

//		private int projectFolderIdx = 0;
//		private int pairFolderIdx = 0;

	#endregion

		public SavedFoldersWin(SavedFolderType savedFolderType, string winTitle)
		{
			this.winTitle = winTitle;
			SavedFolderType = savedFolderType;

			InitializeComponent();

			SetgMgr.GetSavedFolderLayout.Min_Height = MIN_HEIGHT;
			SetgMgr.GetSavedFolderLayout.Min_Height = MIN_WIDTH;

			currentFolder = new SubFolderManager(FolderRouteCurrent);
			revisionFolder = new SubFolderManager(FolderRouteRevision);

			currentFolder.PropertyChanged += CurrentFolderOnPropertyChanged;
			revisionFolder.PropertyChanged += RevisionFolderOnPropertyChanged;
		}


	#region public properties

		public static double MIN_WIDTH { get; } = 850;
		public static double MIN_HEIGHT { get; }  = 530;

		public bool DebugMode
		{
			get => SetgMgr.DebugMode;
			set
			{
				SetgMgr.DebugMode = value;
				OnPropertyChange();
			}
		}

		public string WinTitle
		{
			get => winTitle + " (" +
				SavedFolderOperation.ToString() +
				")";
		}

		public SavedFolderOperation SavedFolderOperation
		{
			get => savedFolderOperation;
			set
			{
				savedFolderOperation = value;

				OnPropertyChange();
				OnPropertyChange("WinTitle");
				OnPropertyChange("Message");
			}
		}

		// the project folder collection
		public ObservableCollection<SavedFolderProject> SavedFolders
		{
			get => SetgMgr.Instance.SavedFolders[(int) SavedFolderType];
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
			private get { return selectedFolderProject; }
			set
			{
//				Append(nl);
//				AppendLineFmt("selected", value.Name);

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
				OnPropertyChange("SelectedFolderPair");
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

				currentFolder.Folder = selectedFolderPair?.Current ?? FilePath<FileNameSimple>.Invalid;
				revisionFolder.Folder = selectedFolderPair?.Revision ?? FilePath<FileNameSimple>.Invalid;

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
			get
			{
				if (savedFolderOperation == SavedFolderOperation.GET_CURRENT)
				{
					return "Selecting a Current Folder";
				}
				else if (savedFolderOperation == SavedFolderOperation.GET_REVISION)
				{
					return "Selecting a Revision Folder";
				}

				return null;
			}
		}

	#endregion

	#region public methods

	#endregion

	#region test methods

	#endregion

	#region private methods

	#endregion

	#region window events

		// main button events


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
			SetgMgr.WriteUsr();

			this.DialogResult = true;
			this.Close();
		}

		private void BtnDebugx_OnClick(object sender, RoutedEventArgs e)
		{
			SetgMgr sm = SetgMgr.Instance;

			Debug.WriteLine("@savedfolderWin| debug");
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
//			DebugMode = !DebugMode;

			if (SelectedFolderPair == null)
			{
				Debug.WriteLine("@savedfolderWin| SelectedFolderPair is null");
				return;
			}

//			SelectedFolderPair.Current = new FilePath<FileNameSimple>("C:\\Temp");
//			SelectedFolderPair.Revision = new FilePath<FileNameSimple>("C:\\Temp");

			Debug.WriteLine("@savedfolderWin| debug");
		}

		// folder project
		private void BtnAddFolderProject_OnClick(object sender, RoutedEventArgs e)
		{
			SelectedFolderProject =
				SetgMgr.Instance.CreateSavedProject(SavedFolderType);

			SelectedFolderPair =
				selectedFolderProject.SavedFolderPairs[0];
		}

		private void BtnDeleteFolderProject_OnClick(object sender, RoutedEventArgs e)
		{
			if (selectedFolderProject == null) return;

			SetgMgr.Instance.DeleteSavedProjectFolder(selectedFolderProject, SavedFolderType);

			SelectedFolderPair = null;
			SelectedFolderProject = null;

			SetgMgr.WriteUsr();
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

		// window events
		private void SavedFolderWin_Initialized(object sender, EventArgs e)
		{
			SetgMgr.RestoreWindowLayout(WindowId.DIALOG_SAVED_FOLDERS, this);
		}


		private void SavedFolderWin_Loaded(object sender, RoutedEventArgs e)
		{
//			SavedFolders = SetgMgr.Instance.SavedFolders[index.Value()];
		}

	#endregion

	#region events

		private void CurrentFolderOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("Folder"))
			{
				SelectedFolderPair.Current = currentFolder.Folder;
			}
		}

		private void RevisionFolderOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("Folder"))
			{
				SelectedFolderPair.Revision = revisionFolder.Folder;
			}
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

	#region debug routines

	#if DEBUG
		public void Append(string msg)
		{
//			tbxMain.AppendText(msg);
		}

		public void AppendLine(string msg)
		{
//			Append(msg + nl);
		}

		public void AppendLineFmt(string msg1, string msg2 = "")
		{
			AppendLine(logMsgDbS(msg1, msg2));
		}

	#endif

	#endregion

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
				result = SetgMgr.Instance.ContainsSavedFolder(name, SavedFolderInformation.SavedFolderType);
			}
			else
			{
				result = SetgMgr.Instance.ContainsFolderPair(SavedFolderInformation.SavedFolderProject, name);
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
	public class DoubleLessThenConverter : IMultiValueConverter
	{
		// value[0] is the primary number
		// value[1] is the test number
		// returned number is the smaller of the two
		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return null;

			if (
				value[0].GetType() != typeof(Double) &&
				value[1].GetType() != typeof(Double)
				) return null;

			if (value[0].GetType() != typeof(Double))
			{
				return (Double) (value[1]);
			}
			else if (value[1].GetType() != typeof(Double))
			{
				return (Double) (value[0]);
			}

			Double primary = (Double) (value[0]);
			Double test = (Double) (value[1]);

			Double result = primary < test ? primary : test;

			return result;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw  new NotImplementedException();
		}
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