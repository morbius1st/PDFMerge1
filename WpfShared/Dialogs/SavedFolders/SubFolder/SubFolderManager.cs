﻿#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SettingsManager;
using WpfShared.Dialogs.FolderSupport;
// using Sylvester.Process;
// using Sylvester.Settings;
// using Sylvester.UserControls;
using UtilityLibrary;

#endregion


// projname: Sylvester.SavedFolders.SubFolder
// itemname: SubFolderManager
// username: jeffs
// created:  2/29/2020 8:46:02 AM


namespace WpfShared.Dialogs.SavedFolders.SubFolder
{
	public class SubFolderManager : INotifyPropertyChanged
	{
		private FolderRoute FolderRoute;

		private FilePath<FileNameSimple> fromSelectFolder = null;

		private SelectFolder sf;

		private FolderType FolderType;

		private int FolderPathType;

		public SubFolderManager(FolderRoute fr)
		{
			FolderRoute = fr;

			FolderRoute.PathChange += onPathPathChangeEvent;
			FolderRoute.SelectFolder += onPathSelectFolderEvent;

			sf = new SelectFolder();
		}

	#region public properties

		public bool HasFolder => FolderRoute.Path.IsValid;

		public FilePath<FileNameSimple> Folder
		{
			get => FolderRoute.Path;

			set
			{
				if (value == null || !value.IsValid )
				{
					FolderRoute.Path = null;
					return;
				}

				FolderRoute.Path = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private methods

		private void SelectFolder(string titleSuffix)
		{
			fromSelectFolder = sf.GetFolder(Folder, titleSuffix);
			if (!fromSelectFolder.IsValid) return;

			SetgMgr.SetPriorFolder(FolderType, fromSelectFolder);

			Folder = fromSelectFolder;
		}

	#endregion

	#region event handeling

		internal void onPathPathChangeEvent(object sender, PathChangeArgs e)
		{
			if (!e.SelectedPath.FullFilePath.IsVoid())
			{
				OnPropertyChange("Folder");
			}
		}

		internal void onPathSelectFolderEvent(object sender, EventArgs e)
		{
			if (FolderType == FolderType.CURRENT)
			{
				SelectFolder("Current Folder");
			}
			else
			{
				SelectFolder("Revision Folder");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}
}