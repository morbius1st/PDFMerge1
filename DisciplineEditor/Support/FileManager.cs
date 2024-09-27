#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AndyShared.FileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.Support;
using CSLibraryIo.CommonFileFolderDialog;
using JetBrains.Annotations;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  8/14/2024 8:53:48 PM

namespace DisciplineEditor.Support
{
	public class FileManager : INotifyPropertyChanged
	{
	#region private fields

		private FileAndFolderDialog dlg;

		private string selectedDataFile;

		private Window w;
		private FilePath<FileNameSimple> selectedFilePath;

	#endregion

	#region ctor

		public FileManager(Window w )
		{
			this.w = w;
		}

	#endregion

	#region public properties

		public FilePath<FileNameSimple> SelectedFilePath
		{
			get => selectedFilePath;
			set
			{
				selectedFilePath = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool GetExistFile(string title,
			FilePath<FileNameSimple> filePath,
			CommonFileDialogFilter filter)
		{
			return GetExistFile(
				title,
				filePath.FolderPath,
				filePath.FileNameNoExt,
				filePath.FileExtensionNoSep,
				filter);
		}

		public bool GetExistFile(
			string title,
			string initDir,
			string defFileName,
			string extn,
			CommonFileDialogFilter filter
			)
		{
			// CommonFileDialogFilter filter = new CommonFileDialogFilter();
			//
			// filter.DisplayName = "Discipline Data File";
			// filter.Extensions.Add("xml");
			// filter.ShowExtensions = true;

			dlg = new FileAndFolderDialog();
			dlg.Filters.Add(filter);

			dlg.Title = title;
			dlg.Settings.AddToMostRecentlyUsedList = false;
			dlg.Settings.AllowPropertyEditing = false;
			dlg.Settings.AllowNonFileSysItems = false;
			dlg.Settings.ShowPlacesList = true;

			dlg.Dialog.IsFolderPicker = false;
			dlg.Dialog.Multiselect = false;
			dlg.Dialog.InitialDirectory = initDir;

			dlg.Dialog.DefaultFileName = defFileName;
			dlg.Dialog.DefaultExtension = extn;
			dlg.Dialog.EnsureFileExists = true;

			if (dlg.Dialog.ShowDialog(w) != CommonFileDialogResult.Ok)
			{
				return false;
			}

			SelectedFilePath = new FilePath<FileNameSimple>( dlg.Dialog.FileName);

			return true;
		}

		public void selectFolder()
		{
			CommonFileDialogFilter filter = new CommonFileDialogFilter();

			// filter.DisplayName = "Discipline Data File";
			// filter.Extensions.Add("xml");
			// filter.ShowExtensions = true;

			dlg = new FileAndFolderDialog();


			// dlg.Filters.Add(filter);

			dlg.Title = "Select Discipline Data File";
			dlg.Settings.AddToMostRecentlyUsedList = false;
			dlg.Settings.AllowPropertyEditing = false;
			dlg.Settings.AllowNonFileSysItems = false;
			dlg.Settings.ShowPlacesList = true;

			dlg.Dialog.IsFolderPicker = true;
			dlg.Dialog.Multiselect = false;
			dlg.Dialog.InitialDirectory = UserSettings.Data.DisciplineDataFileFolder;

			dlg.Dialog.DefaultFileName = $"{UserSettings.Data.DisciplineDataFileName}.xml";
			dlg.Dialog.DefaultExtension = "xml";
			dlg.Dialog.EnsureFileExists = true;

			CommonFileDialogResult a =  dlg.Dialog.ShowDialog(w);

			selectedDataFile = dlg.Dialog.FileName;
		}

		public bool GetNewFile(string title,
			FilePath<FileNameSimple> filePath,
			CommonFileDialogFilter filter)
		{
			return GetNewFile(
				title,
				filePath.FolderPath,
				filePath.FileNameNoExt,
				filePath.FileExtensionNoSep,
				filter);
		}

		public bool GetNewFile(
			string title, 
			string initDir,
			string defFileName,
			string extn,
			CommonFileDialogFilter filter
			)
		{
			CommonSaveFileDialog sdg = new CommonSaveFileDialog();

			// sdg.Title = "Save New Discipline File";
			// sdg.InitialDirectory = UserSettings.Data.DisciplineDataFileFolder;
			// sdg.DefaultFileName = UserSettings.Data.DisciplineDataFileName;
			// sdg.DefaultExtension = "xml";
			
			sdg.Title = title;
			sdg.InitialDirectory = initDir;
			sdg.DefaultFileName = defFileName;
			sdg.DefaultExtension = extn;

			// CommonFileDialogFilter filter = new CommonFileDialogFilter();
			// filter.DisplayName = "Discipline files";
			// filter.Extensions.Add("xml");

			sdg.Filters.Add(filter);


			sdg.AllowPropertyEditing = false;
			sdg.AlwaysAppendDefaultExtension = true;
			sdg.EnsurePathExists = true;
			sdg.OverwritePrompt = true;
			sdg.ShowPlacesList = true;
			sdg.IsExpandedMode = true;

			if (sdg.ShowDialog(w) != CommonFileDialogResult.Ok)
			{
				// TaskDialogManager.DisciplineDataFileMissing();
				return false;
			}

			SelectedFilePath = new FilePath<FileNameSimple>(sdg.FileName);

			return true;
		}


		/// <summary>
		/// get permissions to overwrite an existing file if it exists<br/>
		/// true = ok to overwrite<br/>
		/// false = not ok to overwrite<br/>
		/// null = file does not exist
		/// </summary>
		public bool? VerifyOverwriteExistingFile(FilePath<FileNameSimple> existFile)
		{
			if (existFile.Exists)
			{
				if (!TaskDialogManager.VerifyOverwriteDisciplineDataFile(existFile.FullFilePath))
				{
					return false;
				}

				return true;
			}

			return null;
		}

		/// <summary>
		/// Delete an existing file<br/>
		/// true = worked<br/>
		/// false = failed
		/// </summary>
		public bool DeleteExistingFile(FilePath<FileNameSimple> existFile)
		{
			if (existFile.Exists)
			{
				try
				{
					File.Delete(existFile.FullFilePath);
				}
				catch
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// copy the existFile to the newFile<br/>
		/// if allowOverwrite is true, replaces an existing file if it exists<br/>
		/// true = worked<br/>
		/// false = failed
		/// </summary>
		public bool CopyFile(FilePath<FileNameSimple> origFile, 
			FilePath<FileNameSimple> newFile, bool allowOverwrite)
		{
			try
			{
				File.Copy(origFile.FullFilePath, newFile.FullFilePath, allowOverwrite);
			}
			catch
			{
				return false;
			}

			return true;
		}

	#endregion

	#region private methods



	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		[DebuggerStepThrough]
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(FileManager)}";
		}

	#endregion
	}
}