#region + Using Directives

using Microsoft.WindowsAPICodePack.Dialogs;
using Sylvester.FileSupport;
using Sylvester.Settings;
using UtilityLibrary;

#endregion


// projname: Sylvester.FileSupport
// itemname: SelectFolder
// username: jeffs
// created:  12/31/2019 3:01:00 PM

namespace Sylvester.FolderSupport
{
	public class SelectFolder //: IDisposable
	{
		public FilePath<FileNameAsSheet> GetFolder(FilePath<FileNameAsSheet> initFolder)
		{
			using (CommonOpenFileDialog cfd = new CommonOpenFileDialog("Select PDF Package Folder"))
			{
				if (!initFolder.IsValid) return FilePath<FileNameAsSheet>.Invalid;

				cfd.InitialDirectory = initFolder.GetFullPath;
				cfd.IsFolderPicker = true;
				cfd.Multiselect = false;
				cfd.ShowPlacesList = true;
				cfd.AllowNonFileSystemItems = false;

				cfd.AllowPropertyEditing = AppSettings.Data.AllowPropertyEditing;
				AppSettings.Admin.Write();

				CommonFileDialogResult	result = cfd.ShowDialog();

				if (result != CommonFileDialogResult.Ok)
				{
					return FilePath<FileNameAsSheet>.Invalid;
				}

				return new FilePath<FileNameAsSheet>(cfd.FileName);
			}
		}
	}
}
