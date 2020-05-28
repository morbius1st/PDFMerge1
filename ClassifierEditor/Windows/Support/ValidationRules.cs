#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls;

#endregion

// username: jeffs
// created:  5/26/2020 6:36:50 AM

namespace ClassifierEditor.Windows.Support
{
	public class PatternValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			try
			{
				Regex p = new Regex((string) value);
			}

			catch 

			{
				return new ValidationResult(false, (string) value + " is not a valid pattern");
			}
			return ValidationResult.ValidResult;
		}
	}
}
