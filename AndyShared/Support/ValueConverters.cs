using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows.Data;
using UtilityLibrary;

namespace AndyShared.Support
{

#region pass-through converter

	// for debugging only

	[ValueConversion(typeof(object), typeof(object))]
	public class PassThroughConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

#endregion

#region bool to "on" / "off" string value converter

	[ValueConversion(typeof(bool), typeof(string))]
	public class BoolToOnOffConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// if (targetType != typeof(object))
			// 	throw new InvalidOperationException("The target must be an object");

			return ((bool) value) ? "On" : "Off";

		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

#endregion

#region bool to "on" / "off" string value converter

	[ValueConversion(typeof(bool), typeof(string))]
	public class BoolToYesNoConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// if (targetType != typeof(object))
			// 	throw new InvalidOperationException("The target must be an object");

			return ((bool) value) ? "Yes" : "No";
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

#endregion

#region not bool value converter

	[ValueConversion(typeof(bool), typeof(bool))]
	public class InvertBool : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !((bool) value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

#endregion

#region null to bool value converter

	[ValueConversion(typeof(object), typeof(bool))]
	public class NullObjToBool : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

#endregion

#region null to string value converter

	[ValueConversion(typeof(string), typeof(string))]
	public class NullStringToMessage : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null) parameter = "is null";

			return (string) value ?? (string) parameter;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

#endregion
	
#region null object to string value converter

	[ValueConversion(typeof(object), typeof(string))]
	public class NullObjToMessage : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null) parameter = "is null";

			return value ?? (string) parameter;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

#endregion

#region string value converter

	/// <summary>
	/// based on the string provided return<br/>
	/// if null: the first string<br/>
	/// if not null: the second string<br/>
	/// this uses a specialized collection which must be referenced<br/>
	/// here as well as in the XAML file ('xmlns:cs="clr-namespace:System.Collections.Specialized;assembly=System" ')
	/// </summary>
	[ValueConversion(typeof(string), typeof(string))]
	public class StringToMessage : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null) return "";

			string[] p = new string[((StringCollection) parameter).Count];
			((StringCollection) parameter).CopyTo(p, 0);

			if (p[1].IsVoid()) p[1] = (string) value;

			return ((string) value).IsVoid() ? p[0] : p[1];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

#endregion

}
