#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAPICodePack.Dialogs;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  8/8/2020 8:27:21 AM

namespace AndyShared.FileSupport
{
	public class FileOrFolderSelect
	{
	#region private fields

		private string title;
		private string startingFolder;

		private bool selectingFolder;
		private bool multiselect;
		private bool showPlacesList = true;
		private bool allowNonFileSysItems = false;
		private bool allowPropertyEditing = false;

		private FileDialogFilterCollection filters = null;

		private CommonOpenFileDialog cfd;

		private CommonFileDialogResult result;
		private bool hasSelection = false;

	#endregion

	#region ctor

	#endregion

	#region public properties

		public string Title => title;

		public string StartingFolder => startingFolder;

		public FileDialogFilterCollection Filters => filters;

		public CommonFileDialogResult Result => result;

		public bool HasSelection => hasSelection;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public string GetFile(string title, string startingFolder,
			FileDialogFilterCollection filters = null)
		{
			this.title = title;
			this.startingFolder = startingFolder;
			this.filters = filters;

			// these are the pre-set values
			// selectingFolder = false;
			// multiselect = false;
			// showPlacesList = true;
			// allowNonFileSysItems = false;
			// allowPropertyEditing = false;

			if (!SelectFileOrFolder())
			{
				hasSelection = false;
				return null;
			}

			return cfd.FileName;
		}
		
		public IEnumerable<string> GetFiles(string title, string startingFolder,
			FileDialogFilterCollection filters = null)
		{
			this.title = title;
			this.startingFolder = startingFolder;
			this.filters = filters;

			multiselect = true;

			// these are the pre-set values
			// selectingFolder = false;
			// showPlacesList = true;
			// allowNonFileSysItems = false;
			// allowPropertyEditing = false;


			if (!SelectFileOrFolder())
			{
				hasSelection = false;
				return null;
			}

			return cfd.FileNames;
		}


		public string GetFolder(string title, string startingFolder,
			FileDialogFilterCollection filters = null)
		{
			this.title = title;
			this.startingFolder = startingFolder;
			this.filters = filters;

			selectingFolder = true;

			// these are the pre-set values
			// multiselect = false;
			// showPlacesList = true;
			// allowNonFileSysItems = false;
			// allowPropertyEditing = false;

			if (!SelectFileOrFolder())
			{
				hasSelection = false;
				return null;
			}

			return cfd.FileName;
		}

		public IEnumerable<string> GetFolders(string title, string startingFolder,
			FileDialogFilterCollection filters = null)
		{
			this.title = title;
			this.startingFolder = startingFolder;
			this.filters = filters;

			selectingFolder = true;
			multiselect = true;

			// these are the pre-set values
			// showPlacesList = true;
			// allowNonFileSysItems = false;
			// allowPropertyEditing = false;

			if (!SelectFileOrFolder())
			{
				hasSelection = false;
				return null;
			}

			return cfd.FileNames;
		}


	#endregion

	#region private methods

		private bool SelectFileOrFolder()
		{
			try
			{
				using (CommonOpenFileDialog cfd = new CommonOpenFileDialog(title))
				{
					if (!selectingFolder && Filters != null)
					{
						foreach (CommonFileDialogFilter filter in filters)
						{
							cfd.Filters.Add(filter);
						}

					}

					cfd.InitialDirectory = startingFolder;

					cfd.IsFolderPicker = selectingFolder;
					cfd.Multiselect = multiselect;
					cfd.ShowPlacesList = showPlacesList;
					cfd.AllowNonFileSystemItems = allowNonFileSysItems;
					cfd.AllowPropertyEditing = allowPropertyEditing;

					result = cfd.ShowDialog();

					if (result != CommonFileDialogResult.Ok) return false;

					IEnumerable<string> s = cfd.FileNames;
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FileOrFolderSelect";
		}

	#endregion

		public class FileDialogFilterCollection : List<CommonFileDialogFilter> { }
	}
}