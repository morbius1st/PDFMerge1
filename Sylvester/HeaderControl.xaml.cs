using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sylvester.FileSupport;
using Sylvester.FolderSupport;
using Sylvester.UserControls;


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

		public void SetPathChangeEventHandler(FolderRoute.PathChangedEventHandler f)
		{
			FldrRoute.PathChange += f;
		}
		
		public void SetSelectFolderEventHandler(FolderRoute.SelectFolderEventHandler f)
		{
			FldrRoute.SelectFolder += f;
		}
		
		public void SetFavoritesEventHandler(FolderRoute.FavoritesEventHandler f)
		{
			FldrRoute.Favorites += f;
		}

		public Route Path
		{
			get => FldrRoute.Path;
			set => FldrRoute.Path = value;
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


	}
}