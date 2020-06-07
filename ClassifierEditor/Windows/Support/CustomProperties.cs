#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

#endregion

// user name: jeffs
// created:   5/9/2020 10:23:38 PM


namespace ClassifierEditor.Windows.Support
{
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
