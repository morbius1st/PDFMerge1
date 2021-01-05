using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace WpfShared.SavedFolders
{
	/// <summary>
	/// Interaction logic for SavedFoldersWin.xaml
	/// </summary>
	public partial class SavedFoldersWin : Window
	{
		public SavedFoldersWin()
		{
			InitializeComponent();
		}
	}

		[ValueConversion(null, typeof(Double))]
	public class InnerWidthConverter : IValueConverter
	{
		// value is the basic width (usually the actualwidth)
		// parameter is the the left and right margin amount
		// returned number is basic width - (2 * margin amount)
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (
				value == null || value.GetType() != typeof(Double) ||
				parameter == null || parameter.GetType() != typeof(string)
				) return null;

			Double width = (Double) (value);

			bool result = double.TryParse((string) parameter, out double margin  );

			if (!result) { margin = 0; }

			return width - (margin * 2);
		}

		public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
		{
			throw  new NotImplementedException();
		}
	}

	[ValueConversion(null, typeof(Viewbox))]
	public class IconNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null || parameter.GetType() != typeof(string)) return null;

			string index = (string) (value ?? parameter);

			Viewbox cx = new Viewbox();
			cx.Child =  (UIElement) Application.Current.Resources[index];

			return cx;
		}

		public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
		{
			throw  new NotImplementedException();
		}
	}

	[ValueConversion(typeof(int), typeof(bool))]
	public class IntGreaterThanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || value.GetType() != typeof(int)) return false;
			if (parameter == null || parameter.GetType() != typeof(string)) return false;

			int operand = (int) (value);
			int test;

			if (!Int32.TryParse((string) parameter, out test)) return false;

			bool result = operand > test;

			return result;
		}

		public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
		{
			throw  new NotImplementedException();
		}
	}

	[ValueConversion(typeof(string), typeof(bool))]
	public class StringEqualToBoolConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values[0] == null || values[1] == null ||
				values[0].GetType() != typeof(string) || values[1].GetType() != typeof(string)) return false;

			return ((string) values[0]).Equals((string) values[1]);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
