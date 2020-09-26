#region + Using Directives

using Microsoft.WindowsAPICodePack.Dialogs;
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
		public FilePath<FileNameSimple> GetFolder(FilePath<FileNameSimple> initFolder, string titleSuffix)
		{
			using (CommonOpenFileDialog cfd = new CommonOpenFileDialog("Select PDF Package - " 
				+ titleSuffix))
			{

				if (!initFolder.IsValid)
				{
					initFolder = new FilePath<FileNameSimple>("");
				}

				cfd.InitialDirectory = initFolder.AssemblePath(-2);

				cfd.IsFolderPicker = true;
				cfd.Multiselect = false;
				cfd.ShowPlacesList = true;
				cfd.AllowNonFileSystemItems = false;

				cfd.AllowPropertyEditing = SetgMgr.AllowPropertyEditing;
				AppSettings.Admin.Write();

				CommonFileDialogResult	result = cfd.ShowDialog();

				if (result != CommonFileDialogResult.Ok)
				{
					return FilePath<FileNameSimple>.Invalid;
				}

				return new FilePath<FileNameSimple>(cfd.FileName);
			}
		}

	}
}
