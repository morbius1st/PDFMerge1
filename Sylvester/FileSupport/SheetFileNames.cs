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
	public class SheetFileNames<T> where T : SheetId, new()
	{
		public ObservableCollection<T> Files { get; private set; }

		public SheetFileNames()
		{
			Files = new ObservableCollection<T>();
		}

		public void Add(Route fileName)
		{
			Files.Add(new T {FullFileRoute = fileName });
		}
	}
}