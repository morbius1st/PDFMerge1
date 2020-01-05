#region + Using Directives

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Sylvester.FileSupport;
using Sylvester.Settings;

#endregion


// projname: Sylvester.FileSupport
// itemname: SelectBaseFiles
// username: jeffs
// created:  12/31/2019 4:39:46 PM


namespace Sylvester.FileSupport
{
	public class SelectFiles<T> : INotifyPropertyChanged where T : SheetId, new()
	{
		public SheetFileNames<T> SheetFiles { get; private set;}

		private SelectFolder.SelectFolder sf;

		public SelectFiles()
		{
			sf = new SelectFolder.SelectFolder();

			SheetFiles = new SheetFileNames<T>();
		}

		public bool GetFiles(Route Folder)
		{
			foreach (string file in 
				Directory.EnumerateFiles(Folder.FullPath, "*.pdf", 
					SearchOption.AllDirectories))
			{
				SheetFiles.Add(new Route(file));
			}

			return true;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
