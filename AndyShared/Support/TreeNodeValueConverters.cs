#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AndyShared.ClassificationDataSupport.TreeSupport;

#endregion

// user name: jeffs
// created:   11/9/2024 7:16:23 AM

namespace AndyShared.Support
{
#region multi "equals to" value converter

	[ValueConversion(typeof(TreeNode), typeof(bool))]
	public class IsChildOf : IMultiValueConverter
	{
		// [0] is the target node / context node
		// [1] is the source node
		// check if source is a child of target
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length < 2  || values[0]==null || values[1]== null ||
				!( (values[0]?.GetType())== values[1]?.GetType())) return false;


			// Debug.Write($"from {(string) parameter} ");
			// Debug.Write($"| target [0] = {((TreeNode) values[0]).Item.Title} ");
			// Debug.Write($"| source [1] = {((TreeNode) values[1]).Item.Title} ");

			bool result = ((TreeNode) values[0]).IsChildOf((TreeNode) values[1]);

			// Debug.Write($"| result = {result} (target is a child of source)\n");

			return ((TreeNode) values[0]).IsChildOf((TreeNode) values[1]);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

#endregion
}
