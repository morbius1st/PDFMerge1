using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sylvester
{
    /// <summary>
    /// Interaction logic for SkewedButton.xaml
    /// </summary>
    public partial class SkewedButton : UserControl
    {
        public SkewedButton()
        {
            InitializeComponent();
        }

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(SkewedButton), new PropertyMetadata(default(string)));

		public string Text
		{
			get { return (string) GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
			"Index", typeof(int), typeof(SkewedButton), new PropertyMetadata(-1));

		public int Index
		{
			get { return (int) GetValue(IndexProperty); }
			set { SetValue(IndexProperty, value); }
		}

		public static readonly DependencyProperty ShowBorderProperty = DependencyProperty.Register(
			"ShowBorder", typeof(bool), typeof(SkewedButton), new PropertyMetadata(true));

		public bool ShowBorder
		{
			get { return (bool) GetValue(ShowBorderProperty); }
			set { SetValue(ShowBorderProperty, value); }
		}

		public static readonly DependencyProperty ShowArrowProperty = DependencyProperty.Register(
			"ShowArrow", typeof(bool), typeof(SkewedButton), new PropertyMetadata(true));

		public bool ShowArrow
		{
			get { return (bool) GetValue(ShowArrowProperty); }
			set { SetValue(ShowArrowProperty, value); }
		}

		public static readonly DependencyProperty ShowFavoriteProperty = DependencyProperty.Register(
			"ShowFavorite", typeof(bool), typeof(SkewedButton), new PropertyMetadata(false));

		public bool ShowFavorite
		{
			get { return (bool) GetValue(ShowFavoriteProperty); }
			set { SetValue(ShowFavoriteProperty, value); }
		}

		public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register(
			"FontColor", typeof(SolidColorBrush), typeof(SkewedButton), new PropertyMetadata(Brushes.White));

		public SolidColorBrush FontColor
		{
			get { return (SolidColorBrush) GetValue(FontColorProperty); }
			set { SetValue(FontColorProperty, value); }
		}

		public static readonly DependencyProperty FavoritesBrushProperty = DependencyProperty.Register(
			"FavoritesBrush", typeof(SolidColorBrush), typeof(SkewedButton), new PropertyMetadata(default(SolidColorBrush)));

		public SolidColorBrush FavoritesBrush
		{
			get { return (SolidColorBrush) GetValue(FavoritesBrushProperty); }
			set { SetValue(FavoritesBrushProperty, value); }
		}

		public static readonly DependencyProperty SkewedButtonTypeProperty = DependencyProperty.Register(
			"SkewedButtonType", typeof(int), typeof(SkewedButton), new PropertyMetadata(0));

		public int SkewedButtonType
		{
			get { return (int) GetValue(SkewedButtonTypeProperty); }
			set { SetValue(SkewedButtonTypeProperty, value); }
		}

		public static readonly DependencyProperty DisablePathProperty = DependencyProperty.Register(
			"DisablePath", typeof(bool), typeof(SkewedButton), new PropertyMetadata(false));

		public bool DisablePath
		{
			get { return (bool) GetValue(DisablePathProperty); }
			set { SetValue(DisablePathProperty, value); }
		}
	}
}
