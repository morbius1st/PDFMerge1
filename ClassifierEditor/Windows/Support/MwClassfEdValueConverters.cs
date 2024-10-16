// Solution:     PDFMerge1
// Project:       ClassifierEditor
// File:             MwClassfValueConverters.cs
// Created:      2024-10-14 (10:20 PM)

using System;
using System.Globalization;
using System.Windows.Data;
using UtilityLibrary;

namespace ClassifierEditor.Windows
{
	[ValueConversion(typeof(string), typeof(string))]
	public class ElipseString : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool result =	int.TryParse((string) parameter, out int len);

			len = result ? len : 30;

			return  CsStringUtil.EllipsisifyString(value as string, CsStringUtil.JustifyHoriz.RIGHT, len);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

	[ValueConversion(typeof(bool), typeof(bool))]
	public class NotBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(bool))
				throw new InvalidOperationException("The target must be a boolean");

			return !(bool) value;
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

	// #region bool to "on" / "off" string value converter
//
// 	[ValueConversion(typeof(bool), typeof(string))]
// 	public class BoolToOnOffConverter : IValueConverter
// 	{
// 		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
// 		{
// 			if (targetType != typeof(object))
// 				throw new InvalidOperationException("The target must be a object");
//
// 			if ((bool) value)
// 			{
// 				return "On";
// 			}
//
// 			return "Off";
// 		}
//
// 		public object ConvertBack(object value, Type targetType, object parameter,
// 			System.Globalization.CultureInfo culture)
// 		{
// 			throw new NotSupportedException();
// 		}
// 	}
//
// #endregion
}