#region + Using Directives
using Microsoft.WindowsAPICodePack.Dialogs;
using SettingsManager;
#endregion


// projname: AODeliverable.FolderSelector
// itemname: SelectFolder
// username: jeffs
// created:  11/2/2019 2:56:07 PM




namespace Tests2.FileListManager
{
	/// <summary>
	/// Using the CommonOpenFileDialog, have user select a folder
	/// </summary>
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
			AppSettings.Admin.Write();

			CommonFileDialogResult result = cfd.ShowDialog();

			if (result != CommonFileDialogResult.Ok)
			{
				return Route.Invalid;
			}

			return new Route(cfd.FileName);
		}

	}
}
