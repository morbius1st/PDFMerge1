using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Animation;
using static WpfShared.Windows.ResourceFiles.ScrollViewerEx.VertScrollBarLocation;
using static WpfShared.Windows.ResourceFiles.ScrollViewerEx.HorizScrollBarLocation;

namespace WpfShared.Windows.ResourceFiles.ScrollViewerEx
{
	public enum VertScrollBarLocation
	{
		Left = 0,
		Right = 2
	}
	
	public enum HorizScrollBarLocation
	{
		Top = 0,
		Bottom = 2
	}

	/// <summary>
	/// Interaction logic for ScrollViewerExtended.xaml
	/// </summary>
	public partial class ScrollViewerExtended : ScrollViewer
	{
		public ScrollViewerExtended()
		{
			InitializeComponent();
		}

		public void HorizScrollContents(HorizontalAlignment h)
		{
			if (h == HorizontalAlignment.Left)
			{
				this.ScrollToLeftEnd();
			} 
			else if (h == HorizontalAlignment.Right)
			{
				this.ScrollToRightEnd();
			}
			else
			{
				// this.ScrollToHorizontalOffset((this.ExtentWidth - this.ViewportWidth) / 2);
				double o = (this.ExtentWidth - this.ViewportWidth) / 2;
				this.ScrollToHorizontalOffset(o);
			}
		}

		private void initHorizContents()
		{
			if (HorizontalScrollAlignment == HorizontalAlignment.Left)
			{
				this.ScrollToLeftEnd();
			} 
			else if (HorizontalScrollAlignment == HorizontalAlignment.Right)
			{
				this.ScrollToRightEnd();
			}
			else
			{
				// this.ScrollToHorizontalOffset((this.ExtentWidth - this.ViewportWidth) / 2);
				double o = (this.ExtentWidth - this.ViewportWidth) / 2;
				this.ScrollToHorizontalOffset(o);
			}
		}


		private void ScrollViewerExtended_OnLoaded(object sender, RoutedEventArgs e)
		{
			initHorizContents();
		}


	#region HorizontalScrollAlignment

		public static readonly DependencyProperty HorizontalScrollAlignmentProperty = DependencyProperty.Register(
			"HorizontalScrollAlignment", typeof(HorizontalAlignment), typeof(ScrollViewerExtended), 
			new FrameworkPropertyMetadata(default(HorizontalAlignment), FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

		public HorizontalAlignment HorizontalScrollAlignment
		{
			get => (HorizontalAlignment) GetValue(HorizontalScrollAlignmentProperty);
			set =>SetValue(HorizontalScrollAlignmentProperty, value);

		}

	#endregion
		
	#region BackgroundSkewAngle

		public static readonly DependencyProperty BackgroundSkewAngleProperty = DependencyProperty.Register(
			"BackgroundSkewAngle", typeof(Double), typeof(ScrollViewerExtended), new PropertyMetadata(0.0));

		public Double BackgroundSkewAngle
		{
			get => (Double) GetValue(BackgroundSkewAngleProperty);
			set => SetValue(BackgroundSkewAngleProperty, value);
		}

	#endregion


	#region VerticalScrollBarLocation

		public static readonly DependencyProperty VerticalScrollBarLocationProperty = DependencyProperty.Register(
			"VerticalScrollBarLocation", typeof(VertScrollBarLocation), typeof(ScrollViewerExtended), new PropertyMetadata(Right));

		public VertScrollBarLocation VerticalScrollBarLocation
		{
			get => (VertScrollBarLocation) GetValue(VerticalScrollBarLocationProperty);
			set => SetValue(VerticalScrollBarLocationProperty, value);
		}

	#endregion

	#region HorizontalScrollBarLocation

		public static readonly DependencyProperty HorizontalScrollBarLocationProperty = DependencyProperty.Register(
			"HorizontalScrollBarLocation", typeof(HorizScrollBarLocation), typeof(ScrollViewerExtended), new PropertyMetadata(Bottom));

		public HorizScrollBarLocation HorizontalScrollBarLocation
		{
			get => (HorizScrollBarLocation) GetValue(HorizontalScrollBarLocationProperty);
			set => SetValue(HorizontalScrollBarLocationProperty, value);
		}

	#endregion
		
	#region ScrollBarWidth

		public static readonly DependencyProperty ScrollBarWidthProperty = DependencyProperty.Register(
			"ScrollBarWidth", typeof(double), typeof(ScrollViewerExtended), new PropertyMetadata(8.0));

		public double ScrollBarWidth
		{
			get => (double) GetValue(ScrollBarWidthProperty);
			set => SetValue(ScrollBarWidthProperty, value);
		}

	#endregion

	#region ThumbWidth

		public static readonly DependencyProperty ThumbWidthProperty = DependencyProperty.Register(
			"ThumbWidth", typeof(double), typeof(ScrollViewerExtended), new PropertyMetadata(4.0));

		public double ThumbWidth
		{
			get => (double) GetValue(ThumbWidthProperty);
			set => SetValue(ThumbWidthProperty, value);
		}

	#endregion

	#region RepeatButtonLength

		public static readonly DependencyProperty RepeatButtonLengthProperty = DependencyProperty.Register(
			"RepeatButtonLength", typeof(double), typeof(ScrollViewerExtended), new PropertyMetadata(8.0));

		public double RepeatButtonLength
		{
			get => (double) GetValue(RepeatButtonLengthProperty);
			set => SetValue(RepeatButtonLengthProperty, value);
		}

	#endregion

	#region RepeatButtonIconLength

		public static readonly DependencyProperty RepeatButtonIconLengthProperty = DependencyProperty.Register(
			"RepeatButtonIconLength", typeof(double), typeof(ScrollViewerExtended), new PropertyMetadata(8.0));

		public double RepeatButtonIconLength
		{
			get => (double) GetValue(RepeatButtonIconLengthProperty);
			set => SetValue(RepeatButtonIconLengthProperty, value);
		}

	#endregion

	#region RepeatButtonIconGirth

		public static readonly DependencyProperty RepeatButtonIconGirthProperty = DependencyProperty.Register(
			"RepeatButtonIconGirth", typeof(double), typeof(ScrollViewerExtended), new PropertyMetadata(6.0));

		public double RepeatButtonIconGirth
		{
			get => (double) GetValue(RepeatButtonIconGirthProperty);
			set => SetValue(RepeatButtonIconGirthProperty, value);
		}

	#endregion

	#region IconMarginTop

		public static readonly DependencyProperty IconMarginTopProperty = DependencyProperty.Register(
			"IconMarginTop", typeof(Thickness), typeof(ScrollViewerExtended), new PropertyMetadata(new Thickness(0,0,0,0)));

		public Thickness IconMarginTop
		{
			get => (Thickness) GetValue(IconMarginTopProperty);
			set => SetValue(IconMarginTopProperty, value);
		}

	#endregion

	#region IconMarginBottom

		public static readonly DependencyProperty IconMarginBottomProperty = DependencyProperty.Register(
			"IconMarginBottom", typeof(Thickness), typeof(ScrollViewerExtended), new PropertyMetadata(new Thickness(0,0,0,0)));

		public Thickness IconMarginBottom
		{
			get => (Thickness) GetValue(IconMarginBottomProperty);
			set => SetValue(IconMarginBottomProperty, value);
		}

	#endregion

	#region IconMarginLeft

		public static readonly DependencyProperty IconMarginLeftProperty = DependencyProperty.Register(
			"IconMarginLeft", typeof(Thickness), typeof(ScrollViewerExtended), new PropertyMetadata(new Thickness(0,0,0,0)));

		public Thickness IconMarginLeft
		{
			get => (Thickness) GetValue(IconMarginLeftProperty);
			set => SetValue(IconMarginLeftProperty, value);
		}

	#endregion

	#region IconMarginRight

		public static readonly DependencyProperty IconMarginRightProperty = DependencyProperty.Register(
			"IconMarginRight", typeof(Thickness), typeof(ScrollViewerExtended), new PropertyMetadata(new Thickness(0,0,0,0)));

		public Thickness IconMarginRight
		{
			get => (Thickness) GetValue(IconMarginRightProperty);
			set => SetValue(IconMarginRightProperty, value);
		}

	#endregion


		
	}

#region Enum to int value converter

	[ValueConversion(typeof(object), typeof(int))]
	public class EnumToInt : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is Enum)) return -1;

			return (int) value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

#endregion
	
}