#region + Using Directives

using System.IO;
using AODeliverable.Settings;
using Microsoft.WindowsAPICodePack.Dialogs;

#endregion


// projname: AODeliverable.FolderSelector
// itemname: SelectFolder
// username: jeffs
// created:  11/2/2019 2:56:07 PM


namespace AODeliverable.FileSelection
{
	public class SelectFolder
	{
		private static SelectFolder instance = null;

		private CommonOpenFileDialog cfd = null;

		private bool Selected = false;

		public static SelectFolder Instance
		{
			get
			{
				if (instance == null) instance = new SelectFolder();

				return instance;
			}
		}

		private SelectFolder()
		{
			cfd = new CommonOpenFileDialog("Select PDF Package Folder");
		}

		public bool GetFolder()
		{
			cfd.IsFolderPicker = true;
			cfd.Multiselect = false;
			cfd.ShowPlacesList = true;
			cfd.AllowNonFileSystemItems = false;
			
			cfd.AllowPropertyEditing = AppSettings.Data.AllowPropertyEditing;
			AppSettings.Admin.Save();

			CommonFileDialogResult result = cfd.ShowDialog();

			if (result != CommonFileDialogResult.Ok)
			{
				return false;
			}

			Selected = true;

			return true;
		}

		public string SelectedFolder
		{
			get
			{
				if (Selected)
				{
					return cfd.FileName;
				}

				return null;
			}
		}

		public string InitialFolder
		{
			get { return cfd.InitialDirectory; }
			set
			{
				if (string.IsNullOrWhiteSpace(value)) return;

				cfd.InitialDirectory = value;
			}
		}

	}
}
