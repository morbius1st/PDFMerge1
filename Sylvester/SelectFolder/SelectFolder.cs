#region + Using Directives

using Microsoft.WindowsAPICodePack.Dialogs;
using Sylvester.FileSupport;
using Sylvester.Settings;

#endregion


// projname: Test4.FileSupport
// itemname: SelectFolder
// username: jeffs
// created:  12/31/2019 3:01:00 PM


namespace Sylvester.SelectFolder
{
	public class SelectFolder
	{
		public SelectFolder()
		{
			cfd = new CommonOpenFileDialog("Select PDF Package Folder");
		}

		private CommonOpenFileDialog cfd = null;

		public Route GetFolder(Route initFolder)
		{

			if (!initFolder.IsValid) return Route.Invalid;

			cfd.InitialDirectory = initFolder.FullPath;
			cfd.IsFolderPicker = true;
			cfd.Multiselect = false;
			cfd.ShowPlacesList = true;
			cfd.AllowNonFileSystemItems = false;

			cfd.AllowPropertyEditing = AppSettings.Data.AllowPropertyEditing;
			AppSettings.Admin.Save();

			CommonFileDialogResult result = cfd.ShowDialog();

			if (result != CommonFileDialogResult.Ok)
			{
				return Route.Invalid;
			}

			return new Route(cfd.FileName);
		}
	}
}
