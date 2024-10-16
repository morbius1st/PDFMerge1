#region + Using Directives
using SettingsManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

#endregion

// user name: jeffs
// created:   10/13/2024 10:19:25 PM

namespace DisciplineEditor.Windows.Support
{
#region multi string compare value converter

	[ValueConversion(typeof(object), typeof(int))]
	public class MultiObjectComparisonConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			int i;

			try
			{
				ListBox lb = (ListBox) values[0];
				ListBoxItem li = (ListBoxItem) values[1];

				i = lb.ItemContainerGenerator.IndexFromContainer(li);

				RecentItem ri = (RecentItem) li.Content;
				ri.Index = i;

			}
			catch
			{
				i = -1;
			}

			return i;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			string[] result = new [] { (string) value };
			return result;
		}
	}

#endregion
}
