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
		private TestFilesCollection tfc;

		public ReadFiles(TestFilesCollection tfc)
		{
			this.tfc = tfc;
		}

		public bool GetFiles(Route Folder)
		{
			foreach (string file in
				Directory.EnumerateFiles(Folder.FullPath, "*.pdf",
					SearchOption.AllDirectories))
			{
				tfc.Add(new Route(file));
			}

			return true;
		}

	}
}
