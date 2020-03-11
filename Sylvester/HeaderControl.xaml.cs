using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sylvester.FolderSupport;
using Sylvester.Process;
using Sylvester.SavedFolders;
using Sylvester.Settings;
using Sylvester.UserControls;
using UtilityLibrary;


namespace Sylvester
{
	/// <summary>
	/// Interaction logic for HeaderControl.xaml
	/// </summary>
	public partial class HeaderControl : UserControl
	{
	#region fields

		private FilePath<FileNameSimple> fromSelectFolder = null;

		private SelectFolder sf;

		private SetgMgr sm;

		public static SavedFolderManager[] sfm = new SavedFolderManager[SavedFolderType.COUNT.Value()];

	#endregion

	#region ctor

		public HeaderControl()
		{
			InitializeComponent();

			sm = SetgMgr.Instance;

			sf = new SelectFolder();

			sfm[SavedFolderType.HISTORY.Value()] = SavedFolderManager.GetHistoryManager();

			sfm[SavedFolderType.FAVORITES.Value()] = SavedFolderManager.GetFavoriteManager();
		}

	#endregion

	#region public properties

		public bool HasFolder => FolderRoute.Path.IsValid;

		public FilePath<FileNameSimple> Folder => FolderRoute.Path;

	#endregion

	#region public methods

		public void Start()
		{
			FolderRoute.PathChange += onPathPathChangeEvent;
			FolderRoute.Favorites += onPathFavoriteEvent;
			FolderRoute.History += onPathHistoryEvent;
			FolderRoute.SelectFolder += onPathSelectFolderEvent;

			FolderRoute.Path = SetgMgr.GetPriorFolder(FolderType);


			configureFolderRoute();
		}

		public void SetFolder(FilePath<FileNameSimple> path)
		{
			FolderRoute.Path = path;
		}

	#endregion

	#region private methods

		private SavedFolderOperation GetFolderOp()
		{
			if (FolderType == FolderType.CURRENT) return SavedFolderOperation.GET_CURRENT;

			return SavedFolderOperation.GET_REVISION;
		}

		private void SelectFolder()
		{
			fromSelectFolder = sf.GetFolder(Folder);
			if (!fromSelectFolder.IsValid) return;

//			tempGetPriorFolder();

			SetgMgr.SetPriorFolder(FolderType, fromSelectFolder);

			FolderRoute.Path = fromSelectFolder;

			configureFolderRoute();
		}

		private void configureFolderRoute()
		{
			int folderPathType;

			// always show the select folder button
			// always show the favorites button (how else could they
			// make a new favorite
			folderPathType = ObliqueButtonType.SELECTFOLDER.Value() +
				ObliqueButtonType.FAVORITES.Value();

			if (HasFolder)
			{
				folderPathType += ObliqueButtonType.TEXT.Value();
			}

			folderPathType += sfm[SavedFolderType.HISTORY.Value()].HasSavedFolders
				? ObliqueButtonType.HISTORY.Value()
				: 0;
//			folderPathType += sfm[SavedFolderType.FAVORITES.Value()].HasSavedFolders ? ObliqueButtonType.FAVORITES.Value() : 0;

			FolderPathType = folderPathType;
		}

	#endregion


	#region temp methods

		private void tempGetPriorFolder()
		{
			if (FolderType == FolderType.CURRENT)
			{
				tempPriorCurrentFolder();
			}
			else
			{
				tempPriorRevisionFolder();
			}
		}

		private void tempPriorCurrentFolder()
		{
			fromSelectFolder = new FilePath<FileNameSimple>(
				@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Base");
		}

		private void tempPriorRevisionFolder()
		{
			fromSelectFolder = new FilePath<FileNameSimple>(
				@"C:\2099-999 Sample Project\Publish\9999 Current\Individual Sheets\Test");
		}

	#endregion

	#region event processing

		internal void onPathPathChangeEvent(object sender, PathChangeArgs e)
		{
			Debug.WriteLine("folderManager, path changed");
			Debug.WriteLine("folderManager| index     | " + e.Index);
			Debug.WriteLine("folderManager| sel folder| " + e.SelectedFolder);
			Debug.WriteLine("folderManager| sel path  | " + e.SelectedPath.GetFullPath);

			RaiseFolderChangedEvent();
		}

		internal void onPathSelectFolderEvent(object sender, EventArgs e)
		{
			Debug.WriteLine("Header Control, Select Folder");

			SelectFolder();
		}

		internal void onPathFavoriteEvent(object sender, EventArgs e)
		{
			sfm[(int) SavedFolderType.FAVORITES].ShowSavedFolderWin(GetFolderOp());

//			Debug.WriteLine("folderManager, Favorites");

//			sfm[SavedFolderType.FAVORITES.Value()].test();

//			SelectFolder();
		}

		internal void onPathHistoryEvent(object sender, EventArgs e)
		{
			sfm[(int) SavedFolderType.HISTORY].ShowSavedFolderWin(GetFolderOp());

//			Debug.WriteLine("folderManager, History");

//			sfm[SavedFolderType.HISTORY.Value()].test();

//			SelectFolder();
		}

	#endregion

	#region events

		public delegate void FolderChangedEventHandler(object sender, EventArgs e);

		public event HeaderControl.FolderChangedEventHandler PathChanged;

		protected virtual void RaiseFolderChangedEvent()
		{
			PathChanged?.Invoke(this, new EventArgs());
		}

	#endregion

	#region public control properties

		public static readonly DependencyProperty FolderTypeProperty = DependencyProperty.Register(
			"FolderType", typeof(FolderType), typeof(HeaderControl), new PropertyMetadata(FolderType.UNASSIGNED));

		public FolderType FolderType
		{
			get { return (FolderType) GetValue(FolderTypeProperty); }
			set { SetValue(FolderTypeProperty, value); }
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string),
				typeof(HeaderControl),
				new PropertyMetadata("")
				);

		public string Title
		{
			get { return (string) GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		// non-sheet pdf count visibility
		public static readonly DependencyProperty NonSheetPdfCountVisibilityProperty =
			DependencyProperty.Register("NonSheetPdfCountVisibility", typeof(Visibility),
				typeof(HeaderControl), new PropertyMetadata(Visibility.Visible));

		public Visibility NonSheetPdfCountVisibility
		{
			get { return (Visibility) GetValue(NonSheetPdfCountVisibilityProperty); }
			set { SetValue(NonSheetPdfCountVisibilityProperty, value); }
		}

		// other-file count visibility
		public static readonly DependencyProperty OtherFileCountVisibilityProperty =
			DependencyProperty.Register("OtherFileCountVisibility", typeof(Visibility),
				typeof(HeaderControl), new PropertyMetadata(Visibility.Visible));

		public Visibility OtherFileCountVisibility
		{
			get { return (Visibility) GetValue(OtherFileCountVisibilityProperty); }
			set { SetValue(OtherFileCountVisibilityProperty, value); }
		}

		// title bar left margin
		public static readonly DependencyProperty TitleBarLeftMarginProperty =
			DependencyProperty.Register("TitleBarLeftMargin", typeof(double),
				typeof(HeaderControl), new PropertyMetadata(default(double)));

		public static readonly DependencyProperty TitleBarMarginProperty = DependencyProperty.Register(
			"TitleBarMargin", typeof(Thickness), typeof(HeaderControl), new PropertyMetadata(default(Thickness)));

		public Thickness TitleBarMargin
		{
			get { return (Thickness) GetValue(TitleBarMarginProperty); }
			set { SetValue(TitleBarMarginProperty, value); }
		}


		public static readonly DependencyProperty FolderPathTypeProperty = DependencyProperty.Register(
			"FolderPathType", typeof(int), typeof(HeaderControl), new PropertyMetadata(7));

		public int FolderPathType
		{
			get { return (int) GetValue(FolderPathTypeProperty); }
			set { SetValue(FolderPathTypeProperty, value); }
		}


		public static readonly DependencyProperty HideDirectoryProperty = DependencyProperty.Register(
			"HideDirectory", typeof(bool), typeof(HeaderControl), new PropertyMetadata(default(bool)));

		public bool HideDirectory
		{
			get { return (bool) GetValue(HideDirectoryProperty); }
			set { SetValue(HideDirectoryProperty, value); }
		}

		public static readonly DependencyProperty FolderRouteHeightProperty = DependencyProperty.Register(
			"FolderRouteHeight", typeof(double), typeof(HeaderControl), new PropertyMetadata(18.0));

		public double FolderRouteHeight
		{
			get { return (double) GetValue(FolderRouteHeightProperty); }
			set { SetValue(FolderRouteHeightProperty, value); }
		}

		public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
			"TextMargin", typeof(Thickness), typeof(HeaderControl), new PropertyMetadata(new Thickness(0)));

		public Thickness TextMargin
		{
			get { return (Thickness) GetValue(TextMarginProperty); }
			set { SetValue(TextMarginProperty, value); }
		}

		public static readonly DependencyProperty TextFontSizeProperty = DependencyProperty.Register(
			"TextFontSize", typeof(double), typeof(HeaderControl), new PropertyMetadata(8.0));

		public double TextFontSize
		{
			get { return (double) GetValue(TextFontSizeProperty); }
			set { SetValue(TextFontSizeProperty, value); }
		}


		public static readonly DependencyProperty ObliqueButtonHeightProperty = DependencyProperty.Register(
			"ObliqueButtonHeight", typeof(double), typeof(HeaderControl), new PropertyMetadata(13.0));

		public double ObliqueButtonHeight
		{
			get { return (double) GetValue(ObliqueButtonHeightProperty); }
			set { SetValue(ObliqueButtonHeightProperty, value); }
		}

		public static readonly DependencyProperty FontBrushProperty = DependencyProperty.Register(
			"FontBrush", typeof(SolidColorBrush), typeof(HeaderControl), new PropertyMetadata(Brushes.White));

		public SolidColorBrush FontBrush
		{
			get { return (SolidColorBrush) GetValue(FontBrushProperty); }
			set { SetValue(FontBrushProperty, value); }
		}

	#endregion
	}
}