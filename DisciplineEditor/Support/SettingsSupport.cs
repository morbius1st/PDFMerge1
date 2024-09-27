#define DML0 // not yet used
#define DML1 // not yet used
// #define DML2  // turns on or off bool flags / button enable flags only / listbox idex set
// #define DML3  // various status messages
// #define DML4  // update status status messages

#region using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using AndyShared.Support;
using DebugCode;
using JetBrains.Annotations;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  8/15/2024 6:48:58 PM

namespace DisciplineEditor.Support
{
	public class SettingsSupport : INotifyPropertyChanged
	{
		public string UserSettingsFilePath => 
			CsStringUtil.EllipsisifyString(
				UserSettings.Path?.SettingFilePath ?? "unavailable",
				CsStringUtil.JustifyHoriz.CENTER, 80);

		public string UserSettingsFileExists => (UserSettings.Path?.Exists ?? false) == false ? "No" : "Yes";

		public string SettingsDataFilePathEllipsised => CsStringUtil.EllipsisifyString(
			UserSettings.Data.DisciplineDataFileFolder ?? "null",
			CsStringUtil.JustifyHoriz.RIGHT, 70);

		public string SettingDataFileName => UserSettings.Data.DisciplineDataFileName;

		/// <summary>
		/// indicates that the path information is not null
		/// </summary>
		public bool HasDisciplineDataFilePath => gotValidDisciplineDataFilePath();

		/// <summary>
		/// determine the status of the user settings file
		/// make sure it exists and is read or is created<br/>
		/// return false if cannot access the settings file<br/>
		/// which is a hard fail
		/// </summary>
		public bool GetUserSettingsFile()
		{
			DM.Start0();

			if (!UserSettings.Path.Exists)
			{
				UserSettings.Admin.Write();

				if (!UserSettings.Path.Exists)
				{
					DM.End0("end 1");
					return false;
				}
			}
			else
			{
				UserSettings.Admin.Read();
			#if DML3
				DM.Stat0($"user setting file read | file   {UserSettings.Data.DisciplineDataFileName}");
				DM.Stat0($"user setting file read | folder {UserSettings.Data.DisciplineDataFileFolder}");
			#endif
			}

			InitRecentList();

			updateProps();

			DM.End0();

			return true;
		}

		/// <summary>
		/// update the discipline data file path info and save the settings file
		/// </summary>
		public void UpdateDataFilePath(FilePath<FileNameSimple> filePath)
		{
			DM.Start0();

			if (filePath != null)
			{
				UserSettings.Data.DisciplineDataFileFolder = filePath.FolderPath;
				UserSettings.Data.DisciplineDataFileName = filePath.FileNameNoExt;

				AddToRecent(filePath);
			}
			else
			{
				UserSettings.Data.DisciplineDataFileFolder = "";
				UserSettings.Data.DisciplineDataFileName = "";
			}

			UserSettings.Admin.Write();

			DM.End0();
		}

		// public void UpdateDataFilePath(string fileNameNoExt, string folderPath)
		// {
		// 	DM.Start0();
		// 	UserSettings.Data.DisciplineDataFileFolder = folderPath;
		// 	UserSettings.Data.DisciplineDataFileName = fileNameNoExt;
		//
		// 	DM.End0();
		// }

		private bool gotValidDisciplineDataFilePath()
		{
			if (UserSettings.Data.DisciplineDataFileFolder.IsVoid()) return false;
			if (UserSettings.Data.DisciplineDataFileName.IsVoid()) return false;

			return true;
		}

		private void updateProps()
		{
			DM.Start0();
			OnPropertyChange(nameof(UserSettingsFilePath));
			OnPropertyChange(nameof(UserSettingsFileExists));
			OnPropertyChange(nameof(SettingsDataFilePathEllipsised));
			OnPropertyChange(nameof(SettingDataFileName));
			OnPropertyChange(nameof(HasDisciplineDataFilePath));

			OnPropertyChange(nameof(RecentList));
			DM.End0();
		}

		public event PropertyChangedEventHandler PropertyChanged;

	#region setting file header info access
		
		// settings
		public string UstgFileName          => UserSettings.Data.DisciplineDataFileName;
		public string UstgFolderPath        => UserSettings.Data.DisciplineDataFileFolder;
		public string UstgLastClassfField   => UserSettings.Data.LastClassificationFileId;

		// info data
		public string UstgInfoFileType   => UserSettings.Info.FileType.ToString();
		public string UstgInfoDesc       => UserSettings.Info.Description;
		public string UstgInfoNotes      => UserSettings.Info.Notes;
		public string UstgInfoClsVer     => UserSettings.Info.DataClassVersion;

		// header info
		public string UstgInfoHdrAssembVer     => UserSettings.Info.Header.AssemblyVersion;
		public string UstgInfoHdrSaveDateTime  => UserSettings.Info.Header.SaveDateTime;
		public string UstgInfoHdrSaveBy        => UserSettings.Info.Header.SavedBy;
		public string UstgInfoHdrSetgVer       => UserSettings.Info.Header.SettingsVersion;




	#endregion

	#region recent list routines

		private int maxRecentItems = 10;

		public ObservableCollection<RecentItem> RecentList => UserSettings.Data?.RecentList ?? null;

		private int recentFoundIdx = -1;

		public void InitRecentList()
		{
			DM.InOut0();

			if (UserSettings.Data.RecentList == null) UserSettings.Data.RecentList = new ObservableCollection<RecentItem>();
		}

		public void AddToRecent(FilePath<FileNameSimple> filePath)
		{
			DM.Start0();

			if (filePath == null || filePath.FullFilePath.IsVoid()) return;

			RecentItem recent = new RecentItem(filePath.FolderPath, filePath.FileNameNoExt);

			bool result = RecentList.Contains(recent);

			if (RecentList.Contains(recent))
			{
				int idx = RecentList.IndexOf(recent);
				RecentList.RemoveAt(idx);
			} 
			else
			if (RecentList.Count + 1 > maxRecentItems)
			{
				RecentList.RemoveAt(RecentList.Count - 1);
			}

			RecentList.Insert(0,recent);

			UserSettings.Admin.Write();

			OnPropertyChange(nameof(RecentList));

			DM.End0();
		}

		private bool recentListContainsKey(string filePathNoExt)
		{
			int idx = 0;

			foreach (RecentItem ri in RecentList)
			{
				if (ri.filePathNoExt.Equals(filePathNoExt))
				{
					recentFoundIdx = idx;
					return true;
				}

				idx++;
			}

			return false;
		}

		public void RemoveFromRecent(RecentItem path)
		{
			DM.Start0();
			if (!RecentList.Contains(path)) return;

			RecentList.Remove(path);

			UserSettings.Admin.Write();

			DM.End0();
		}

		public void RemoveFromRecent(int idx)
		{
			DM.Start0();

			RecentList.RemoveAt(idx);

			UserSettings.Admin.Write();

			DM.End0();
		}

		public void RemoveFromRecent(FilePath<FileNameSimple> filePath)
		{
			RemoveFromRecent(new RecentItem(filePath.FolderPath, filePath.FileNameNoExt));
		}

		public RecentItem MakeRecentItem(FilePath<FileNameSimple> filePath)
		{
			return new RecentItem(filePath.FolderPath, filePath.FileNameNoExt);
		}
		

	#endregion

		[NotifyPropertyChangedInvocator]
		[DebuggerStepThrough]
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}