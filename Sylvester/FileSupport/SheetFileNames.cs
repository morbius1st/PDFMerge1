#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#endregion


// projname: Sylvester.FileSupport
// itemname: SheetFileNames
// username: jeffs
// created:  12/31/2019 8:43:58 PM


namespace Sylvester.FileSupport
{
	public class SheetFileNames<T> : INotifyPropertyChanged where T : SheetId, new()
	{
		private const string PATTERN =
			@"^(([0-9]*[A-Z]*)(?=[ -]+[A-Z])([ -]*)([A-Z0-9\.-]*[a-z]*)|([A-Z]+ *[0-9\.-]+[a-z]*){1})([- ]+)((.*)(\(.*\))(\.[Pp][dD][Ff])|(.*)(\.[Pp][dD][Ff])|(.*))";
//			@"^(([0-9]*[A-Z]*)(?= |-)([ -]*)([A-Z0-9\.-]*[a-z]*)|[A-Z]+[0-9\.-]+[a-z]*){1}([- ]+)((.*)(\(.*\))(\.[Pp][dD][Ff])|(.*)(\.[Pp][dD][Ff])|(.*))";

		public ObservableCollection<T> Files { get; private set; }

		private Regex Pattern;

		public SheetFileNames()
		{
			Files = new ObservableCollection<T>();

			Pattern = new Regex(PATTERN, RegexOptions.Compiled | RegexOptions.Singleline);


		}


		private const int PHASE_BLDG            = 2;
		private const int PHASE_BLDG_SEP        = 3;
		private const int SHEET_ID_GOT          = 4;
		private const int SHEET_ID_NOT          = 5;
		private const int SEPARATOR             = 6;
		private const int SHEET_NAME_COMMENT    = 8;
		private const int SHEET_NAME_NO_COMMENT = 11;
		private const int SHEET_NAME            = 7;
		private const int COMMENT               = 9;
		private const int EXTENSION_COMMENT     = 10;
		private const int EXTENSION_NO_COMMENT  = 12;

		public void Add(Route fileName)
		{
			T shtId = new T {FullFileName = fileName };

			Match match = Pattern.Match(fileName.FileName);

			GroupCollection m = match.Groups;

			string test;

			// phase-bldg
			test = m[PHASE_BLDG].Value;
			if (!string.IsNullOrEmpty(test))
			{
				shtId.PhaseBldg = test;
				shtId.PhaseBldgSep = m[PHASE_BLDG_SEP].Value;
			}

			// separator
			shtId.Separator = m[SEPARATOR].Value;

			// comment
			test = m[COMMENT].Value;
			if (!string.IsNullOrEmpty(test)) shtId.Comment = test;

			// sheet ID
			test = m[SHEET_ID_GOT].Value;
			shtId.SheetID = !string.IsNullOrEmpty(test) ? test : m[SHEET_ID_NOT].Value;

			// sheet name
			test = m[EXTENSION_COMMENT].Value;
			if (!string.IsNullOrEmpty(test))
			{
				shtId.SheetName = m[SHEET_NAME_COMMENT].Value;
			} 
			else if (!string.IsNullOrEmpty(m[EXTENSION_NO_COMMENT].Value))
			{
				shtId.SheetName = m[SHEET_NAME_NO_COMMENT].Value;
			}
			else
			{
				shtId.SheetName = m[SHEET_NAME].Value;
			}

			Files.Add(shtId);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}