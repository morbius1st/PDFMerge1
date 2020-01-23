using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sylvester.FileSupport;
using Sylvester.FolderSupport;


namespace Sylvester
{
	/// <summary>
	/// Interaction logic for HeaderControl.xaml
	/// </summary>
	public partial class HeaderControl : UserControl
	{
		public HeaderControl()
		{
			InitializeComponent();
		}

		public void SetPathChangeEventHandler(FolderPath.PathChangedEventHandler f)
		{
			FpPath.PathChange += f;
		}
		
		public void SetSelectFolderEventHandler(FolderPath.SelectFolderEventHandler f)
		{
			FpPath.SelectFolder += f;
		}
		
		public void SetFavoritesEventHandler(FolderPath.FavoritesEventHandler f)
		{
			FpPath.Favorites += f;
		}

		public Route Path
		{
			get => FpPath.Path;
			set => FpPath.Path = value;
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

	}
}