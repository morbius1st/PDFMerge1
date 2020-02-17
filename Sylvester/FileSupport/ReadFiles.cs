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
		public bool GetFiles<T>( FilesCollection<T> fc, bool preselect, FilePath<FileNameSimple> folder) where T : SheetNameInfo, new()
		{
			fc.Reset();

			foreach (string file in
				Directory.EnumerateFiles(folder.GetFullPath, "*.*",
					SearchOption.TopDirectoryOnly))
			{
				fc.Add(new FilePath<FileNameSimple>(file), preselect);
			}
			
			return fc.SheetPdfs > 0;
		}
	}
}