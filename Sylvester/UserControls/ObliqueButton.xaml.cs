using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using static Sylvester.UserControls.ObliqueButtonType;

namespace Sylvester.UserControls
{
	/// <summary>
	/// Interaction logic for ObliqueButton.xaml
	/// </summary>
	public partial class ObliqueButton : UserControl
	{
		public ObliqueButton()
		{
			InitializeComponent();
		}


		// index value
		public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
			"Index", typeof(int), typeof(ObliqueButton), new PropertyMetadata(-1));

		public int Index
		{
			get { return (int) GetValue(IndexProperty); }
			set { SetValue(IndexProperty, value); }
		}

		// the name of the selected folder
		public static readonly DependencyProperty SelectedFolderProperty = DependencyProperty.Register(
			"SelectedFolder", typeof(string), typeof(ObliqueButton), new PropertyMetadata(default(string)));

		public string SelectedFolder
		{
			get { return (string) GetValue(SelectedFolderProperty); }
			set { SetValue(SelectedFolderProperty, value); }
		}
		
		// text value
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(ObliqueButton), new PropertyMetadata(default(string)));

		public string Text
		{
			get { return (string) GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
			"TextMargin", typeof(Thickness), typeof(ObliqueButton), new PropertyMetadata(new Thickness(0)));

		public Thickness TextMargin
		{
			get { return (Thickness) GetValue(TextMarginProperty); }
			set { SetValue(TextMarginProperty, value); }
		}



		// show the text?
		public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
			"ShowText", typeof(bool), typeof(ObliqueButton), new PropertyMetadata(false));

		public bool ShowText
		{
			get { return (bool) GetValue(ShowTextProperty); }
			set { SetValue(ShowTextProperty, value); }
		}

		// show the arrow?
		public static readonly DependencyProperty ShowArrowProperty = DependencyProperty.Register(
			"ShowArrow", typeof(bool), typeof(ObliqueButton), new PropertyMetadata(false));

		public bool ShowArrow
		{
			get { return (bool) GetValue(ShowArrowProperty); }
			set { SetValue(ShowArrowProperty, value); }
		}

		// show the favorites symbol
		public static readonly DependencyProperty ShowFavoriteProperty = DependencyProperty.Register(
			"ShowFavorite", typeof(bool), typeof(ObliqueButton), new PropertyMetadata(false));

		public bool ShowFavorite
		{
			get { return (bool) GetValue(ShowFavoriteProperty); }
			set { SetValue(ShowFavoriteProperty, value); }
		}

		// show the plus symbol
		public static readonly DependencyProperty ShowPlusProperty = DependencyProperty.Register(
			"ShowPlus", typeof(bool), typeof(ObliqueButton), new PropertyMetadata(false));

		public bool ShowPlus
		{
			get { return (bool) GetValue(ShowPlusProperty); }
			set { SetValue(ShowPlusProperty, value); }
		}

		public static readonly DependencyProperty ShowHistoryProperty = DependencyProperty.Register(
			"ShowHistory", typeof(bool), typeof(ObliqueButton), new PropertyMetadata(false));

		public bool ShowHistory
		{
			get { return (bool) GetValue(ShowHistoryProperty); }
			set { SetValue(ShowHistoryProperty, value); }
		}

		// show the border?
		public static readonly DependencyProperty ShowBorderProperty = DependencyProperty.Register(
			"ShowBorder", typeof(bool), typeof(ObliqueButton), new PropertyMetadata(true));

		public bool ShowBorder
		{
			get { return (bool) GetValue(ShowBorderProperty); }
			set { SetValue(ShowBorderProperty, value); }
		}



		// color for the font
		public static readonly DependencyProperty FontBrushProperty = DependencyProperty.Register(
			"FontBrush", typeof(SolidColorBrush), typeof(ObliqueButton), new PropertyMetadata(Brushes.White));

		public SolidColorBrush FontBrush
		{
			get { return (SolidColorBrush) GetValue(FontBrushProperty); }
			set { SetValue(FontBrushProperty, value); }
		}

		// color for the favorites symbol
		public static readonly DependencyProperty FavoritesBrushProperty = DependencyProperty.Register(
			"FavoritesBrush", typeof(SolidColorBrush), typeof(ObliqueButton), new PropertyMetadata(Brushes.OrangeRed));

		public SolidColorBrush FavoritesBrush
		{
			get { return (SolidColorBrush) GetValue(FavoritesBrushProperty); }
			set { SetValue(FavoritesBrushProperty, value); }
		}

		// color for the favorites symbol
		public static readonly DependencyProperty PlusBrushProperty = DependencyProperty.Register(
			"PlusBrush", typeof(SolidColorBrush), typeof(ObliqueButton),
			new PropertyMetadata(Brushes.Lime));

		public SolidColorBrush PlusBrush
		{
			get { return (SolidColorBrush) GetValue(PlusBrushProperty); }
			set { SetValue(PlusBrushProperty, value); }
		}

		// color for the history symbol
		public static readonly DependencyProperty HistoryBrushProperty = DependencyProperty.Register(
			"HistoryBrush", typeof(SolidColorBrush), typeof(ObliqueButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x00, 0xef, 0xff))));

		public SolidColorBrush HistoryBrush
		{
			get { return (SolidColorBrush) GetValue(HistoryBrushProperty); }
			set { SetValue(HistoryBrushProperty, value); }
		}



//		// type of oblique button
//		public static readonly DependencyProperty ObliqueButtonTypeProperty = DependencyProperty.Register(
//			"ObliqueButtonType", typeof(int), typeof(ObliqueButton), new PropertyMetadata(0));
//
//		public int ObliqueButtonType
//		{
//			get { return (int) GetValue(ObliqueButtonTypeProperty); }
//			set { SetValue(ObliqueButtonTypeProperty, value); }
//		}


		public static readonly DependencyProperty ObliqueButtonTypeProperty = DependencyProperty.Register(
			"ObliqueButtonType", typeof(ObliqueButtonType), typeof(ObliqueButton), new PropertyMetadata(TEXT));

		public ObliqueButtonType ObliqueButtonType
		{
			get { return (ObliqueButtonType) GetValue(ObliqueButtonTypeProperty); }
			set { SetValue(ObliqueButtonTypeProperty, value); }
		}



		// skew the first part of the contents
		public static readonly DependencyProperty SkewBeginningProperty = DependencyProperty.Register(
			"SkewBeginning", typeof(bool), typeof(ObliqueButton), new PropertyMetadata(false));

		public bool SkewBeginning
		{
			get { return (bool) GetValue(SkewBeginningProperty); }
			set { SetValue(SkewBeginningProperty, value); }
		}

		// skew the first part of the contents
		public static readonly DependencyProperty SkewMiddleProperty = DependencyProperty.Register(
			"SkewMiddle", typeof(bool), typeof(ObliqueButton), new PropertyMetadata(false));

		public bool SkewMiddle
		{
			get { return (bool) GetValue(SkewMiddleProperty); }
			set { SetValue(SkewMiddleProperty, value); }
		}

		// skew the first part of the contents
		public static readonly DependencyProperty SkewEndProperty = DependencyProperty.Register(
			"SkewEnd", typeof(bool), typeof(ObliqueButton), new PropertyMetadata(true));

		public bool SkewEnd
		{
			get { return (bool) GetValue(SkewEndProperty); }
			set { SetValue(SkewEndProperty, value); }
		}
	}

}
