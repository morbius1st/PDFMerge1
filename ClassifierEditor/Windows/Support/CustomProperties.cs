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


//	#region HasChildren
//
//		public static readonly DependencyProperty HasChildrenProperty = DependencyProperty.Register(
//			"HasChildren", typeof(bool), typeof(CheckBox),
//			new PropertyMetadata(false));
//
//		public static void SetHasChildren(UIElement e, bool value)
//		{
//			e.SetValue(HasChildrenProperty, value);
//		}
//
//		public static bool GetHasChildren(UIElement e)
//		{
//			return (bool) e.GetValue(HasChildrenProperty);
//		}
//
//	#endregion
	}


}
