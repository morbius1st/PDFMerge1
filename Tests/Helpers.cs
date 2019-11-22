#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

#endregion


// projname: Tests
// itemname: Helpers
// username: jeffs
// created:  11/6/2019 9:31:58 PM


namespace Tests
{
	public class Helpers
	{
		public childItem FindNamedVisualChild<childItem>(DependencyObject obj, string name)
			where childItem : FrameworkElement
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(obj, i);
				if (child != null && child is childItem && ((FrameworkElement) child).Name.Equals(name))
				{
					return (childItem) child;
				}
				else
				{
					childItem childOfChild = FindNamedVisualChild<childItem>(child, name);
					if (childOfChild != null)
						return childOfChild;
				}
			}

			return null;
		}

		public childItem FindVisualChild<childItem>(DependencyObject obj)
			where childItem : DependencyObject
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(obj, i);
				if (child != null && child is childItem)
				{
					return (childItem) child;
				}
				else
				{
					childItem childOfChild = FindVisualChild<childItem>(child);
					if (childOfChild != null)
						return childOfChild;
				}
			}

			return null;
		}
	}
}
