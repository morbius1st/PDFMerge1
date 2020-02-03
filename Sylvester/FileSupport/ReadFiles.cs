#region + Using Directives

using System.IO;

#endregion


// projname: Sylvester.FileSupport
// itemname: ReadFiles
// username: jeffs
// created:  1/4/2020 11:05:28 PM


namespace Sylvester.FileSupport
{
	public class ReadFiles
	{
		public bool GetFiles<T>(Route Folder, bool preselect,
			FilesCollection<T> fc) where T : SheetNameInfo, new()
		{
			fc.Reset();

			foreach (string file in
				Directory.EnumerateFiles(Folder.FullPath, "*.*",
					SearchOption.TopDirectoryOnly))
			{
				fc.Add(new Route(file), preselect);
			}
			
			return fc.SheetPdfs > 0;
		}
	}
}