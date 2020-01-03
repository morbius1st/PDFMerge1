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
	public class SheetFileNames<T> : INotifyPropertyChanged where T : SheetIdTest, new()
	{
		private const string PATTERN =
			@"^(([A-Z0-9 ]+[A-Z]+[a-z\.0-9-]+)|[A-Z]+[a-z0-9\.-]+)([ -]+)((.*)(\(.*\))|.*\.|.*)";

		public ObservableCollection<T> Files { get; private set; }

		private Regex Pattern;

		public SheetFileNames()
		{
			Files = new ObservableCollection<T>();

			Pattern = new Regex(PATTERN, RegexOptions.Compiled | RegexOptions.Singleline);
		}

		public void Add(Route fileName)
		{
			T shtId = new T {FullFileName = fileName };

			Match match = Pattern.Match(fileName.FileName);

			if (match.Groups.Count < 6) return;

			shtId.SheetNumber = match.Groups[1].Value;
			shtId.Separator = match.Groups[3].Value;
			
			if (!string.IsNullOrWhiteSpace(match.Groups[6].Value))
			{
				shtId.Comment = match.Groups[6].Value;
				shtId.SheetName = match.Groups[5].Value;
			}
			else
			{
				shtId.SheetName = match.Groups[4].Value;
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