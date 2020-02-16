#region + Using Directives

using System.IO;

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

		public bool RenameFiles(FilesCollection<FileFinal>  final)
		{
			FileInfo fi;

			string newPath;

			foreach (FileFinal tf in final.Files)
			{
				if (!tf.Selected) continue;

				fi = new FileInfo(tf.FullFileRoute.GetFullPath);

				newPath = tf.FullFileRoute.GetPath + "\\"
					+ tf.NewFileName;

				fi.MoveTo(newPath);
			}

			return true;
		}
	}
}