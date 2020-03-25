#region + Using Directives
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

#endregion


// projname: Sylvester.Support
// itemname: Converters
// username: jeffs
// created:  3/24/2020 9:08:54 PM


namespace Sylvester.Support
{
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

	[ValueConversion(typeof(bool), typeof(bool))]
	public class BoolInverterConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || value.GetType() != typeof(bool)) return false;

			return !((bool) value);
		}

		public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
		{
			throw  new NotImplementedException();
		}
	}

	[ValueConversion(typeof(bool[]), typeof(bool))]
	public class MultiBoolToBoolConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool result = true;

			if (values.Length > 0)
			{
				foreach (object value in values)
				{
					if (value is bool b)
					{
						result &= (bool) b;
					}
				}
			}

			return result;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return new object[] { };
		}
	}

}
