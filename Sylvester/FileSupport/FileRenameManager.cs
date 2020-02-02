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

				fi.MoveTo(newPath);
			}

			return true;
		}
	}
}