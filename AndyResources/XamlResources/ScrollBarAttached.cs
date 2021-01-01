#region + Using Directives

using System.Windows;

#endregion

// user name: jeffs
// created:   12/30/2020 11:11:08 PM

namespace AndyResources.XamlResources
{
	public class ScrollBarAttached : DependencyObject
	{
	#region ScrollBarWidth

		public static readonly DependencyProperty ScrollBarWidthProperty = DependencyProperty.RegisterAttached(
			"ScrollBarWidth", typeof(double), typeof(ScrollBarAttached), new PropertyMetadata(8.0));

		public static void SetScrollBarWidth(UIElement element, double value)
		{
			element.SetValue(ScrollBarWidthProperty, value);
		}

		public static double GetScrollBarWidth(UIElement element)
		{
			return (double) element.GetValue(ScrollBarWidthProperty);
		}

	#endregion

	#region TrackWidth

		public static readonly DependencyProperty TrackWidthProperty = DependencyProperty.RegisterAttached(
			"TrackWidth", typeof(double), typeof(ScrollBarAttached), new PropertyMetadata(4.0));

		public static void SetTrackWidth(UIElement e, double value)
		{
			e.SetValue(TrackWidthProperty, value);
		}

		public static double GetTrackWidth(UIElement e)
		{
			return (double) e.GetValue(TrackWidthProperty);
		}

	#endregion

	#region RepeatButtonLength

		public static readonly DependencyProperty RepeatButtonLengthProperty = DependencyProperty.RegisterAttached(
			"RepeatButtonLength", typeof(double), typeof(ScrollBarAttached), new PropertyMetadata(8.0));

		public static void SetRepeatButtonLength(UIElement e, double value)
		{
			e.SetValue(RepeatButtonLengthProperty, value);
		}

		public static double GetRepeatButtonLength(UIElement e)
		{
			return (double) e.GetValue(RepeatButtonLengthProperty);
		}

	#endregion

	#region RepeatButtonIconLength

		public static readonly DependencyProperty RepeatButtonIconLengthProperty = DependencyProperty.RegisterAttached(
			"RepeatButtonIconLength", typeof(double), typeof(ScrollBarAttached), new PropertyMetadata(6.0));

		public static void SetRepeatButtonIconLength(UIElement e, double value)
		{
			e.SetValue(RepeatButtonIconLengthProperty, value);
		}

		public static double GetRepeatButtonIconLength(UIElement e)
		{
			return (double) e.GetValue(RepeatButtonIconLengthProperty);
		}

	#endregion

	#region RepeatButtonIconGirth

		public static readonly DependencyProperty RepeatButtonIconGirthProperty = DependencyProperty.RegisterAttached(
			"RepeatButtonIconGirth", typeof(double), typeof(ScrollBarAttached), new PropertyMetadata(6.0));

		public static void SetRepeatButtonIconGirth(UIElement e, double value)
		{
			e.SetValue(RepeatButtonIconGirthProperty, value);
		}

		public static double GetRepeatButtonIconGirth(UIElement e)
		{
			return (double) e.GetValue(RepeatButtonIconGirthProperty);
		}

	#endregion

	#region IconMarginTop

		public static readonly DependencyProperty IconMarginTopProperty = DependencyProperty.RegisterAttached(
			"IconMarginTop", typeof(Thickness), typeof(ScrollBarAttached), new PropertyMetadata(new Thickness(0,0,0,0)));

		public static void SetIconMarginTop(UIElement e, Thickness value)
		{
			e.SetValue(IconMarginTopProperty, value);
		}

		public static Thickness GetIconMarginTop(UIElement e)
		{
			return (Thickness) e.GetValue(IconMarginTopProperty);
		}

	#endregion

	#region IconMarginBottom

		public static readonly DependencyProperty IconMarginBottomProperty = DependencyProperty.RegisterAttached(
			"IconMarginBottom", typeof(Thickness), typeof(ScrollBarAttached), new PropertyMetadata(new Thickness(0,0,0,0)));

		public static void SetIconMarginBottom(UIElement e, Thickness value)
		{
			e.SetValue(IconMarginBottomProperty, value);
		}

		public static Thickness GetIconMarginBottom(UIElement e)
		{
			return (Thickness) e.GetValue(IconMarginBottomProperty);
		}

	#endregion

		
	#region IconMarginLeft

		public static readonly DependencyProperty IconMarginLeftProperty = DependencyProperty.RegisterAttached(
			"IconMarginLeft", typeof(Thickness), typeof(ScrollBarAttached), new PropertyMetadata(new Thickness(0,0,0,0)));

		public static void SetIconMarginLeft(UIElement e, Thickness value)
		{
			e.SetValue(IconMarginLeftProperty, value);
		}

		public static Thickness GetIconMarginLeft(UIElement e)
		{
			return (Thickness) e.GetValue(IconMarginLeftProperty);
		}

	#endregion

		
	#region IconMarginRight

		public static readonly DependencyProperty IconMarginRightProperty = DependencyProperty.RegisterAttached(
			"IconMarginRight", typeof(Thickness), typeof(ScrollBarAttached), new PropertyMetadata(new Thickness(0,0,0,0)));

		public static void SetIconMarginRight(UIElement e, Thickness value)
		{
			e.SetValue(IconMarginRightProperty, value);
		}

		public static Thickness GetIconMarginRight(UIElement e)
		{
			return (Thickness) e.GetValue(IconMarginRightProperty);
		}

	#endregion

	}
}