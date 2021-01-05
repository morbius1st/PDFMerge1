#region + Using Directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilityLibrary;
using WpfShared.Dialogs.DialogSupport;


#endregion


// projname: Sylvester.SavedFolders.SubFolder
// itemname: SubFolderManager
// username: jeffs
// created:  2/29/2020 8:46:02 AM


namespace WpfShared.SavedFolders.SubFolder
{
	public class SubFolderManager : INotifyPropertyChanged
	{
		// private FolderRoute FolderRoute;

#pragma warning disable CS0414 // The field 'SubFolderManager.fromSelectFolder' is assigned but its value is never used
		private FilePath<FileNameSimple> fromSelectFolder = null;
#pragma warning restore CS0414 // The field 'SubFolderManager.fromSelectFolder' is assigned but its value is never used

#pragma warning disable CS0169 // The field 'SubFolderManager.sf' is never used
		private SelectFolder sf;
#pragma warning restore CS0169 // The field 'SubFolderManager.sf' is never used

#pragma warning disable CS0649 // Field 'SubFolderManager.FolderType' is never assigned to, and will always have its default value
		private FolderType FolderType;
#pragma warning restore CS0649 // Field 'SubFolderManager.FolderType' is never assigned to, and will always have its default value

#pragma warning disable CS0169 // The field 'SubFolderManager.FolderPathType' is never used
		private int FolderPathType;
#pragma warning restore CS0169 // The field 'SubFolderManager.FolderPathType' is never used

		// public SubFolderManager(FolderRoute fr)
		// {
		// 	FolderRoute = fr;
		//
		// 	FolderRoute.PathChange += onPathPathChangeEvent;
		// 	FolderRoute.SelectFolder += onPathSelectFolderEvent;
		//
		// 	sf = new SelectFolder();
		// }

	#region public properties

		// public bool HasFolder => FolderRoute.Path.IsValid;

		// public FilePath<FileNameSimple> Folder
		// {
		// 	get => FolderRoute.Path;
		//
		// 	set
		// 	{
		// 		if (value == null || !value.IsValid )
		// 		{
		// 			FolderRoute.Path = null;
		// 			return;
		// 		}
		//
		// 		FolderRoute.Path = value;
		// 		OnPropertyChange();
		// 	}
		// }

	#endregion

	#region private methods

		// private void SelectFldr(string titleSuffix)
		// {
		// 	fromSelectFolder = sf.GetFolder(Folder, titleSuffix);
		// 	if (!fromSelectFolder.IsValid) return;
		//
		// 	SetgMgr.SetPriorFolder(FolderType, fromSelectFolder);
		//
		// 	Folder = fromSelectFolder;
		// }

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
				// SelectFldr("Current Folder");
			}
			else
			{
				// SelectFldr("Revision Folder");
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