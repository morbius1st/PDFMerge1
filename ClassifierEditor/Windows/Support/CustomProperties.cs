#region + Using Directives
using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

#endregion

// user name: jeffs
// created:   5/9/2020 10:23:38 PM


namespace ClassifierEditor.Windows.Support
{

	// <Setter Property="Background" Value="{custom:ScBrush color={StaticResource Gray.150}, A=#2f}"></Setter>

		[MarkupExtensionReturnType(typeof(SolidColorBrush))]
		public class ScBrush : MarkupExtension
		{
			private Color c;

			public ScBrush() {}

			public Color color
			{
				get => c;
				set
				{
					R = c.R;
					G = c.G;
					B = c.B;
				}
			}

			public byte R { get; set; }

			public byte G { get; set; }

			public byte B { get; set; }

			public byte? A { get; set; }

			public System.Windows.Media.SolidColorBrush ToBrush()
			{
				return new SolidColorBrush(Color.FromArgb(
					(byte) (A.HasValue ? A.Value : 255), R, G, B));

			}

			public override object ProvideValue(IServiceProvider serviceProvider)
			{
				return ToBrush();
			}
		}


	public class CustomProperties
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

//	#region GenericBoolOne
//
//		public static readonly DependencyProperty GenericBoolTwoProperty = DependencyProperty.RegisterAttached(
//			"GenericBoolTwo", typeof(bool), typeof(CustomProperties),
//			new PropertyMetadata(false));
//
//		public static void SetGenericBoolTwo(UIElement e, bool value)
//		{
//			e.SetValue(GenericBoolTwoProperty, value);
//		}
//
//		public static bool GetGenericBoolTwo(UIElement e)
//		{
//			return (bool) e.GetValue(GenericBoolTwoProperty);
//		}
//
//	#endregion

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

	}
}
