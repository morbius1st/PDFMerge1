#region + Using Directives

using System.IO;
using UtilityLibrary;

#endregion


// projname: Sylvester.FileSupport
// itemname: ReadFiles
// username: jeffs
// created:  1/4/2020 11:05:28 PM


namespace Sylvester.FileSupport
{
	public class ReadFiles
	{
		public bool GetFiles<T>(FilePath<FileNameAsSheet> Folder, bool preselect,
			FilesCollection<T> fc) where T : SheetNameInfo, new()
		{
			fc.Reset();

			foreach (string file in
				Directory.EnumerateFiles(Folder.GetFullPath, "*.*",
					SearchOption.TopDirectoryOnly))
			{
				fc.Add(new FilePath<FileNameAsSheet>(file), preselect);
			}
			
			return fc.SheetPdfs > 0;
		}
	}
}