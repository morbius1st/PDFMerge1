#region + Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



#endregion


// projname: Sylvester.FileSupport
// itemname: ReadFiles
// username: jeffs
// created:  1/4/2020 11:05:28 PM


namespace Sylvester.FileSupport
{
	public class ReadFiles
	{
		public bool GetFiles<T>(Route Folder, FilesCollection<T> fc) where T: SheetId, new()
		{
//			foreach (string file in
//				Directory.EnumerateFiles(Folder.FullPath, "*.pdf",
//					SearchOption.AllDirectories))
				foreach (string file in
				Directory.EnumerateFiles(Folder.FullPath, "*.*",
					SearchOption.TopDirectoryOnly))
			{
				fc.Add(new Route(file));
			}

			return true;
		}

	}
}
