#region + Using Directives
using System; 
using System.Windows;
using System.Windows.Media;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;

#endregion

// user name: jeffs
// created:   5/9/2020 10:23:38 PM


namespace ClassifierEditor.Windows.Support
{
	public class CustomProperties : DependencyObject
	{
	#region GenericBoolOne

		public static readonly DependencyProperty GenericBoolOneProperty = DependencyProperty.RegisterAttached(
			"GenericBoolOne", typeof(bool), typeof(CustomProperties),
			new PropertyMetadata(false));

		public static void SetGenericBoolOne(UIElement e, bool value)
		{
			e.SetValue(GenericBoolOneProperty, value);
		}

		public static bool GetGenericBoolOne(UIElement e)
		{
			return (bool) e.GetValue(GenericBoolOneProperty);
		}

	#endregion

	#region GenericBoolOne

		public static readonly DependencyProperty GenericBoolTwoProperty = DependencyProperty.RegisterAttached(
			"GenericBoolTwo", typeof(bool), typeof(CustomProperties),
			new PropertyMetadata(false));

		public static void SetGenericBoolTwo(UIElement e, bool value)
		{
			e.SetValue(GenericBoolTwoProperty, value);
		}

		public static bool GetGenericBoolTwo(UIElement e)
		{
			return (bool) e.GetValue(GenericBoolTwoProperty);
		}

	#endregion

	#region DropDownWidth

		public static readonly DependencyProperty DropDownWidthProperty = DependencyProperty.RegisterAttached(
			"DropDownWidth", typeof(double), typeof(CustomProperties), new PropertyMetadata(100.0));

		public static void SetDropDownWidth(UIElement e, double value)
		{
			e.SetValue(DropDownWidthProperty, value);
		}

		public static double GetDropDownWidth(UIElement e)
		{
			return (double) e.GetValue(DropDownWidthProperty);
		}

	#endregion

	#region MouseOverBrush

		public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.RegisterAttached(
			"MouseOverBrush", typeof(SolidColorBrush), typeof(CustomProperties), new PropertyMetadata(default(SolidColorBrush)));

		public static void SetMouseOverBrush(UIElement e, SolidColorBrush value)
		{
			e.SetValue(MouseOverBrushProperty, value);
		}

		public static SolidColorBrush GetMouseOverBrush(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(MouseOverBrushProperty);
		}

	#endregion
		
	#region DropDownBrush

		public static readonly DependencyProperty  DropDownBrushProperty = DependencyProperty.RegisterAttached(
			"DropDownBrush", typeof(SolidColorBrush), typeof(CustomProperties), new PropertyMetadata(default(SolidColorBrush)));

		public static void SetDropDownBrushh(UIElement e, SolidColorBrush value)
		{
			e.SetValue(DropDownBrushProperty, value);
		}

		public static SolidColorBrush GetDropDownBrush(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(DropDownBrushProperty);
		}

	#endregion

	#region NotEnabledBrush

		public static readonly DependencyProperty NotEnabledBrushProperty = DependencyProperty.RegisterAttached(
			"NotEnabledBrush", typeof(SolidColorBrush), typeof(CustomProperties), new PropertyMetadata(default(SolidColorBrush)));

		public static void SetNotEnabledBrush(UIElement e, SolidColorBrush value)
		{
			e.SetValue(NotEnabledBrushProperty, value);
		}

		public static SolidColorBrush GetNotEnabledBrush(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(NotEnabledBrushProperty);
		}

	#endregion




	}

	

	
}
