using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using Sylvester.Process;
using Sylvester.SavedFolders;
using Sylvester.Settings;
using Sylvester.Windows;
using UtilityLibrary;


	/*

	todo
	1.  saved folder
	a.  create a new project - must provide a real name
	b.  *  all fields must use a temp field that is not saved
	 to the settings file unless saved
	 *  saved project / saved pair will have a key that is internal
	 *  key is derived from saved folder pairs
	 *  names cannot be blank
	c.  must add a save button when adding new entry
	d.  adding a new entry is for favorites only
	e.  allow add project from external source.

	2.  setgmgr
	a.  need find project by key
	b.  need find project by both filepaths
	c.  need add a project but must have all fields
	d.  need add a pair but must have all fields
	e.  add project by savedfolderproject object


	3. usr setg folderproject
	a. add key
	b. key internally created - provide public method
	c. fail if key exists
	d. key = current folderpath root foldername
	e. add method to find project from  folder pair
	f. add field - fully configured - which means
	   i.    has a name
	   ii.   has a key
	   iii.  has min 1 folder pair (fully configured)
	g. add method to clone

	4. usr setg folder pair
	a. add key
	b. key internally created - provide public method
	c. key == current folder name + " :: " + revision foldername
	d. add field = project key - part of constructor
	e. add field - fully configured - which means
	   i.    has name
	   ii.   has a key
	   iii   has both folders
	   iv.   has a project name
	f. add method to clone

	 */




namespace Sylvester
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{

		public double MIN_WIDTH { get; } = 1000;
		public double MIN_HEIGHT { get; }  = 850;

		private bool compare = false;
		private bool go = false;

		private SavedFolderManager favorites;
		private SavedFolderManager history;


		public static Window MainWin;

		public ProcessManager pm { get; private set; } // = new ProcessManager();

		public MainWindow()
		{

			InitializeComponent();

			UserSettings.Admin.Read();

			MainWin = this;

			SetgMgr.GetMainWindowLayout.Min_Height = MIN_HEIGHT;
			SetgMgr.GetMainWindowLayout.Min_Height = MIN_WIDTH;

			SetgMgr.RestoreWindowLayout(WindowId.WINDOW_MAIN, this);

			favorites = SavedFolderManager.GetFavoriteManager();
			history = SavedFolderManager.GetHistoryManager();

			SavedFolderManager.Parent = this;
		}

	#region public properties

		public bool SetFocus
		{
			set { SetFocusComparison(); }
		}

		public bool Compare
		{
			get => compare;
			private set
			{
				compare = value;
				OnPropertyChange();
			}
		}

		public bool Go
		{
			get => go;
			private set
			{
				go = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region public methods

		public void SetFocusComparison()
		{
			LvFinal.Focus();
		}

	#endregion

	#region window events

		private void BtnTest1_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@test1");
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{

			Debug.WriteLine("@debug");
		}

		private void BtnReset_OnClick(object sender, RoutedEventArgs e)
		{
			pm.Reset();

			OnPropertyChange("pm");
		}

		private void BtnReadBoth_Click(object sender, RoutedEventArgs e)
		{
			BtnReadCurrent_OnClick(null, null);
			BtnReadRevision_OnClick(null, null);
		}

		private void BtnReadCurrent_OnClick(object sender, RoutedEventArgs e)
		{
			if (!pm.ReadCurrent()) return;

//			OnPropertyChange("pm");
		}

		private void BtnReadRevision_OnClick(object sender, RoutedEventArgs e)
		{
			if (!pm.ReadRevision()) return;

//			OnPropertyChange("pm");
		}

		private void BtnCompare_OnClick(object sender, RoutedEventArgs e)
		{
			Go = pm.Compare();

			SetFocusComparison();

			OnPropertyChange("pm");
		}

		private void BtnGo_OnClick(object sender, RoutedEventArgs e)
		{
			pm.RenameFiles();
		}

		private void BtnFavorites_OnClick(object sender, RoutedEventArgs e)
		{
			bool? result = favorites.ShowSavedFolderWin(SavedFolderOperation.MANAGEMENT);

			if (result == true)
			{
				HdrCurrent.SetFolder(favorites.Current);
				SetgMgr.SetPriorFolder(FolderType.CURRENT, favorites.Current);

				HdrRevision.SetFolder(favorites.Revision);
				SetgMgr.SetPriorFolder(FolderType.REVISION, favorites.Revision);
			}
		}

		private void BtnHistory_OnClick(object sender, RoutedEventArgs e)
		{
			bool? result = history.ShowSavedFolderWin(SavedFolderOperation.MANAGEMENT);

			if (result == true)
			{
				HdrCurrent.SetFolder(history.Current);
				SetgMgr.SetPriorFolder(FolderType.CURRENT, history.Current);

				HdrRevision.SetFolder(history.Revision);
				SetgMgr.SetPriorFolder(FolderType.REVISION, history.Revision);
			}
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			SetgMgr.SaveWindowLayout(WindowId.WINDOW_MAIN, this);

			this.Close();
		}

		private void rbtn_OnClick(object sender, RoutedEventArgs e)
		{
			SetFocusComparison();
		}

		private void Mainwin_Initialized(object sender, EventArgs e) { }

		private void Mainwin_Loaded(object sender, RoutedEventArgs e)
		{
			// get PM started - get events wired
			pm = new ProcessManager();

			// order matters - wire events
			HdrCurrent.PathChanged += OnPathChanged;
			HdrRevision.PathChanged += OnPathChanged;

			// then start up which uses the above events
			HdrCurrent.Start();
			HdrRevision.Start();

			OnPropertyChange("pm");
		}

	#endregion

	#region events handling

		internal void OnPathChanged(object sender, EventArgs e)
		{
			HeaderControl hc = sender as HeaderControl;

			pm.FolderChanged(hc.FolderType, hc.Folder);
		}

	#endregion

	#region events

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		#endregion

	}


	public class BoolTestConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool curr; //= (bool) (values?[0] ?? false);
			bool result = bool.TryParse((values?[0].ToString() ?? "false"), out curr);

			bool rev; // = (bool) (values?[1] ?? false);
			result = bool.TryParse((values?[0].ToString() ?? "false"), out rev);

			return curr && rev;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return null;
		}

	}
}