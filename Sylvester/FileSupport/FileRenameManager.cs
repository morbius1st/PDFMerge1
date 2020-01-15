#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



#endregion


// projname: Sylvester.FileSupport
// itemname: FileRenameManager
// username: jeffs
// created:  1/12/2020 10:02:23 AM


namespace Sylvester.FileSupport
{


	public class FileRenameManager
	{
		private string nl = System.Environment.NewLine;

		public bool RenameFiles(FilesCollection<FinalFile>  final)
		{
			FileInfo fi;

			string newPath;

			foreach (FinalFile tf in final.TestFiles)
			{
				if (!tf.Selected) continue;

				fi = new FileInfo(tf.FullFileRoute.FullPath);

				newPath = tf.FullFileRoute.Path + "\\"
					+ tf.NewFileName;

//				Debug.WriteLine("rename this file| "
//					+ tf.FullFileRoute.FullPath);
//				Debug.WriteLine("    to this file| "
//					+ newPath + nl);

				fi.MoveTo(newPath);

			}

			return true;
		}


	}
}
