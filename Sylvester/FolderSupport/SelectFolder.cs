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
		public Route GetFolder(Route initFolder)
		{
			using (CommonOpenFileDialog cfd = new CommonOpenFileDialog("Select PDF Package Folder"))
			{
				if (!initFolder.IsValid) return Route.Invalid;

				cfd.InitialDirectory = initFolder.FullPath;
				cfd.IsFolderPicker = true;
				cfd.Multiselect = false;
				cfd.ShowPlacesList = true;
				cfd.AllowNonFileSystemItems = false;

				cfd.AllowPropertyEditing = AppSettings.Data.AllowPropertyEditing;
				AppSettings.Admin.Write();

				CommonFileDialogResult	result = cfd.ShowDialog();

				if (result != CommonFileDialogResult.Ok)
				{
					return Route.Invalid;
				}

				return new Route(cfd.FileName);
			}
		}
	}
}
