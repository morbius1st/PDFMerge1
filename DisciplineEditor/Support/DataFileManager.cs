#define DML0 // not yet used
#define DML1 // not yet used
// #define DML2  // turns on or off bool flags / button enable flags only / listbox idex set
// #define DML3  // various status messages
// #define DML4  // not yet used

#region using directives

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.Support;
using DebugCode;
using DisciplineEditor.Windows;
using JetBrains.Annotations;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using SettingsManager;
using UtilityLibrary;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

#endregion

// username: jeffs
// created:  8/14/2024 8:52:44 PM

namespace DisciplineEditor.Support
{
	public class DataFileManager : INotifyPropertyChanged
	{
		public const string  DEF_DISCIPLINE_DATAFILE_FILENAME = "DisciplineData-01";

		private IKnownFolder docFolder;

	#region private fields

		private FileManager fm;
		private SettingsSupport uss;

		private FilePath<FileNameSimple> dataFilePath;
		private FilePath<FileNameSimple> defaultFilePath;

		private DataManager<DisciplinesDataSet> disciplineMgr;

		private MainWindow mainWin;

		// true - opened
		// false - closed
		private bool? disciplineDataFileStatus;

		private bool dataFileOpened;
		private int headerChangeCount;
		private bool headerHasChanges;
		private bool ddInfoDescChgStat;
		private bool ddInfoClsVerChgStat;
		private bool ddInfoNotesChgStat;
		private bool ddInfoHdrSetgVerStat;
		private bool ddInfoHdrSaveByStat;
		private bool ddInfoHdrSaveDateTimeStat;
		private bool ddInfoHdrAssembVerStat;
		private bool ddInfoHdrAssembNameStat;

	#endregion

	#region ctor

		public DataFileManager(MainWindow mw, FileManager fm, SettingsSupport uss)
		{
			this.uss = uss;
			this.fm = fm;
			this.mainWin = mw;

			setDefaultFilePath();

			disciplineMgr = new DataManager<DisciplinesDataSet>(DefaultFolderPath);
		}

	#endregion

	#region data manager properties

		public FilePath<FileNameSimple> DefaultFilePath
		{
			get => defaultFilePath;
			set
			{
				if (Equals(value, defaultFilePath)) return;
				defaultFilePath = value;
				OnPropertyChange();
			}
		}

		public FilePath<FileNameSimple> DefaultFolderPath { get; set; }

		public DataManager<DisciplinesDataSet> DisciplineMgr
		{
			get => disciplineMgr;
			set
			{
				if (Equals(value, disciplineMgr)) return;
				disciplineMgr = value;
				OnPropertyChange();
			}
		}

		public DisciplinesDataSet Dd => disciplineMgr?.Data ?? null;

		public ObservableDictionary<string, DisciplineListData> DdList => disciplineMgr?.Data.DisciplineList ?? null;

		public ICollectionView DisciplineView => disciplineMgr?.Data.DisciplineView ?? null;

		public FilePath<FileNameSimple> DataFilePath
		{
			get => dataFilePath;
			set
			{
				if (Equals(value, dataFilePath)) return;
				dataFilePath = value;
				OnPropertyChange();
				OnPropertyChange(nameof(DisciplineDataFilePathEllipsised));
			}
		}

		public bool DisciplineDataMgrIsInit => disciplineMgr?.IsInitialized ?? false;

		public bool DisciplineDataFileExists => disciplineMgr?.Path?.Exists ?? false;

		public bool DataFileStatus2 => DdList != null;

	#endregion

	#region discipline data file info

		public Dictionary<string, bool> HeaderChanges { get; set; } // = new Dictionary<string, bool>();

		public StorageMgrInfo<DisciplinesDataSet> HeaderInfoOrig { get; set; }

		/// <summary>
		/// status of the data file<br/>
		/// true = open / false = close
		/// </summary>
		public bool DataFileOpened
		{
			get => dataFileOpened;
			set
			{
				if (value == dataFileOpened) return;
				dataFileOpened = value;
				OnPropertyChange();
				ResetHeaderChanges(value);
			}
		}

		/// <summary>
		/// save a copy or the original header information to check
		/// against for changes
		/// </summary>
		private void updateOrigHeader(bool opened)
		{
			if (opened)
			{
				HeaderInfoOrig = Copy(disciplineMgr.Info);
			}
			else
			{
				HeaderInfoOrig = null;
			}
		}

		// the symbols displayed when the bool is true or false (resp.)
		public static StringCollection BoolSymbols1 => new StringCollection()
		{
			" ", "✔"
		};

		public bool HeaderHasChanges
		{
			get
			{
				mainWin.HeaderChanged = HeaderChangeCount > 0;

				return HeaderChangeCount > 0;
			}
		}

		public string HeaderHasChangesString =>  HeaderHasChanges ? "Info Data (Modified)" : "Info Data";

		public int HeaderChangeCount
		{
			get => headerChangeCount;
			set
			{
				if (value == headerChangeCount) return;
				headerChangeCount = value;
				OnPropertyChange();
				OnPropertyChange(nameof(HeaderHasChanges));
				OnPropertyChange(nameof(HeaderHasChangesString));
			}
		}


		// data file header properties
		public string DdInfoDesc
		{
			get => disciplineMgr?.Info.Description;
			set
			{
				disciplineMgr.Info.Description = value;

				OnPropertyChange();

				DdInfoDescChgStat = !value.Equals(HeaderInfoOrig.DataClassVersion);

				updatePropChangeStatus(nameof(DdInfoDesc), DdInfoDescChgStat == true);
			}
		}

		public bool DdInfoDescChgStat
		{
			get => ddInfoDescChgStat;
			set
			{
				if (value == ddInfoDescChgStat) return;
				ddInfoDescChgStat = value;
				OnPropertyChange();
			}
		}

		public string DdInfoNotes
		{
			get => disciplineMgr?.Info.Notes;
			set
			{
				disciplineMgr.Info.Notes = value;

				OnPropertyChange();

				DdInfoNotesChgStat = !value.Equals(HeaderInfoOrig.DataClassVersion);

				updatePropChangeStatus(nameof(DdInfoNotes), DdInfoNotesChgStat == true);
			}
		}

		public bool DdInfoNotesChgStat
		{
			get => ddInfoNotesChgStat;
			set
			{
				if (value == ddInfoNotesChgStat) return;
				ddInfoNotesChgStat = value;
				OnPropertyChange();
			}
		}

		public string DdInfoClsVer
		{
			get => disciplineMgr?.Info.DataClassVersion;
			set
			{
				disciplineMgr.Info.DataClassVersion = value;

				OnPropertyChange();

				DdInfoClsVerChgStat = !value.Equals(HeaderInfoOrig.DataClassVersion);

				updatePropChangeStatus(nameof(DdInfoClsVer), DdInfoClsVerChgStat == true);
			}
		}

		public bool DdInfoClsVerChgStat
		{
			get => ddInfoClsVerChgStat;
			set
			{
				if (value == ddInfoClsVerChgStat) return;
				ddInfoClsVerChgStat = value;
				OnPropertyChange();
			}
		}

		public string DdInfoHdrAssembName
		{
			get => disciplineMgr?.Info.Header.AssemblyName;
			set
			{
				disciplineMgr.Info.Header.AssemblyName = value;

				OnPropertyChange();

				DdInfoHdrAssembNameStat = !value.Equals(HeaderInfoOrig.Header.AssemblyName);

				updatePropChangeStatus(nameof(DdInfoHdrAssembName), DdInfoHdrAssembNameStat);
			}

		}

		public bool DdInfoHdrAssembNameStat
		{
			get => ddInfoHdrAssembNameStat;
			set
			{
				if (value == ddInfoHdrAssembNameStat) return;
				ddInfoHdrAssembNameStat = value;
				OnPropertyChange();
			}
		}

		public string DdInfoHdrAssembVer
		{
			get => disciplineMgr?.Info.Header.AssemblyVersion;
			set
			{
				disciplineMgr.Info.Header.AssemblyVersion = value;

				OnPropertyChange();

				DdInfoHdrAssembVerStat = !value.Equals(HeaderInfoOrig.Header.AssemblyVersion);

				updatePropChangeStatus(nameof(DdInfoHdrAssembVer), DdInfoHdrAssembVerStat);
			}
		}

		public bool DdInfoHdrAssembVerStat
		{
			get => ddInfoHdrAssembVerStat;
			set
			{
				if (value == ddInfoHdrAssembVerStat) return;
				ddInfoHdrAssembVerStat = value;
				OnPropertyChange();
			}
		}

		public string DdInfoHdrSaveDateTime
		{
			get => disciplineMgr?.Info.Header.SaveDateTime;
			set
			{
				disciplineMgr.Info.Header.SaveDateTime = value;

				OnPropertyChange();

				DdInfoHdrSaveDateTimeStat = !value.Equals(HeaderInfoOrig.Header.SaveDateTime);

				updatePropChangeStatus(nameof(DdInfoHdrSaveDateTime), DdInfoHdrSaveDateTimeStat);
			}
		}

		public bool DdInfoHdrSaveDateTimeStat
		{
			get => ddInfoHdrSaveDateTimeStat;
			set
			{
				if (value == ddInfoHdrSaveDateTimeStat) return;
				ddInfoHdrSaveDateTimeStat = value;
				OnPropertyChange();
			}
		}

		public string DdInfoHdrSaveBy
		{
			get => disciplineMgr?.Info.Header.SavedBy;
			set
			{
				disciplineMgr.Info.Header.SavedBy = value;

				OnPropertyChange();

				DdInfoHdrSaveByStat = !value.Equals(HeaderInfoOrig.Header.SavedBy);

				updatePropChangeStatus(nameof(DdInfoHdrSaveBy), DdInfoHdrSaveByStat);
			}
		}

		public bool DdInfoHdrSaveByStat
		{
			get => ddInfoHdrSaveByStat;
			set
			{
				if (value == ddInfoHdrSaveByStat) return;
				ddInfoHdrSaveByStat = value;
				OnPropertyChange();
			}
		}

		public string DdInfoHdrSetgVer
		{
			get => disciplineMgr?.Info.Header.SettingsVersion;
			set
			{
				disciplineMgr.Info.Header.SettingsVersion = value;

				OnPropertyChange();

				DdInfoHdrSetgVerStat = !value.Equals(HeaderInfoOrig.Header.SettingsVersion);

				updatePropChangeStatus(nameof(DdInfoHdrSetgVer), DdInfoHdrSetgVerStat);
			}
		}

		public bool DdInfoHdrSetgVerStat
		{
			get => ddInfoHdrSetgVerStat;
			set
			{
				if (value == ddInfoHdrSetgVerStat) return;
				ddInfoHdrSetgVerStat = value;
				OnPropertyChange();
			}
		}

		// fixed / readonly properties
		public string DdInfoFileType => disciplineMgr?.Info.FileType.ToString();

		public void ResetHeaderChanges(bool opened)
		{
			updateOrigHeader(opened);

			HeaderChanges = new Dictionary<string, bool>();

			HeaderChangeCount = 0;

			DdInfoDescChgStat = false;
			DdInfoNotesChgStat = false;
			DdInfoClsVerChgStat = false;
			DdInfoHdrAssembVerStat = false;
			DdInfoHdrAssembNameStat = false;
			DdInfoHdrSaveDateTimeStat = false;
			DdInfoHdrSaveByStat = false;
			DdInfoHdrSetgVerStat = false;
		}

		private void updatePropChangeStatus(string propName, bool changed)
		{
			if (!HeaderChanges.ContainsKey(propName))
			{
				HeaderChanges.Add(propName, false);
			}

			if (changed)
			{
				// if is true, no change
				if (HeaderChanges[propName]) return;

				// is false, update
				HeaderChanges[propName] = true;
				HeaderChangeCount++;
			}
			else
			{
				// if is false, no change
				if (!HeaderChanges[propName]) return;

				// is true, update
				HeaderChanges[propName] = false;
				HeaderChangeCount--;
			}
		}

	#endregion

	#region public properties

		public string DisciplineDataFileName => dataFilePath?.FileNameNoExt ?? "null";

		public string DisciplineDataFilePathEllipsised => CsStringUtil.EllipsisifyString(
			disciplineMgr?.Path?.SettingFilePath ?? "null",
			CsStringUtil.JustifyHoriz.CENTER, 80);

		public string DisciplineDataFilePath => disciplineMgr?.Path?.SettingFilePath;

		public IKnownFolder Documents => docFolder;

	#endregion

	#region datamanager functions

		/// <summary>
		/// select an existing discipline file then start the
		/// discipline manager using this file
		/// save this path to userSettings / update the ui
		/// </summary>
		/// <returns></returns>
		public bool? OpenA()
		{
			DM.Start0();
			if (!GetExistDataFile())
			{
				// user canceled - ignore
				UpdateStatus();
				DM.End0("end 1");
				return null;
			}

			if (open() != true)
			{
				dataFilePath = null;
				TaskDialogManager.DisciplineDataFileMissing();
				DM.End0("end 2");
				return false;
			}

			updateSettingsFilePath();

			UpdateStatus();

			DM.End0();
			return true;
		}

		public bool OpenB(string fileName, string folderPath)
		{
			DM.Start0();
			// uss.UpdateDataFilePath(fileName, folderPath);
			//
			// setfilePathFromUsrSetg();

			setDataFilePath(fileName, folderPath);

			if (dataFilePath != null && !dataFilePath.Exists)
			{
				bool result = requestReplaceMissingDisciplineDataFile();

				if (!result)
				{
					DM.End0("end 1");
					return false;
				}
			}

			bool? answer = open();

			if (!answer.HasValue) // is null
			{
				DM.End0("end 1");

				uss.RemoveFromRecent(dataFilePath);

				return false;
			}

			if (answer != true)
			{
				dataFilePath = null;
				TaskDialogManager.DisciplineDataFileMissing();
				DM.End0("end 2");
				return false;
			}

			UpdateStatus();

			DM.End0();
			return true;
		}

		// done
		/// <summary>
		/// open the pre-config'd discipline file
		/// pre-config'd means that userSettings has this 
		/// info in its vars
		/// </summary>
		/// <returns></returns>
		public bool OpenPreConfgd()
		{
			DM.Start0();

			bool? result = open();

			if (!result.HasValue) // is null
			{
				DM.End0("end 1");
				return false;
			}

			if (result != true)
			{
				dataFilePath = null;
				TaskDialogManager.DisciplineDataFileMissing();
				DM.End0("end 1");
				return false;
			}

			UpdateStatus();

			DM.End0();
			return true;
		}

		// done
		/// <summary>
		/// start discipline manager using the provided
		/// data file path (previously set) but is an
		/// intermediate routine and does  not setStatus<br/>
		/// true = file created
		/// false = file was not created
		/// </summary>
		/// <returns></returns>
		private bool? open()
		{
			DM.Start0();

			try
			{
				disciplineMgr.Close();

			#if DML3
				DM.Stat0($"opening {dataFilePath.FileName}");
			#endif

				// opens and reads the data file
				disciplineMgr.Open(dataFilePath,
					dataFilePath.FileName, null);
			}
			catch (Exception e)
			{
				TaskDialogManager.GeneralExceptionError(e);
				return null;
			}

		#if DML3
			DM.Stat0($"@app / data manager| path id is {disciplineMgr.Path.id}");
			DM.Stat0($"@app / data manager| admin.path id is {disciplineMgr.Admin.Path.id}");
		#endif

			bool result = DisciplineDataFileExists;

			if (result)
			{
			#if DML3
				DM.Stat0("data file exists");
			#endif

				updateSettingsFilePath();

			#if DML3
				DM.Stat0($"opened: {dataFilePath.FileNameNoExt}");
			#endif

				// disciplineMgr.Info.DataClassVersion = Dd.DataFileVersion;
				// disciplineMgr.Info.Description = $"{Dd.DataFileDescription} for {dataFilePath.FileNameNoExt}";
				// disciplineMgr.Info.Notes = $"{Dd.DataFileNotes} for {dataFilePath.FileNameNoExt}";

				disciplineMgr.Admin.Write();
			}

		#if DML3
			DM.Stat0("file open");
			DM.Stat0($"item count {DisciplineMgr.Data.DisciplineList.Count}");
		#endif


			DM.End0();
			return result;
		}

		// done
		/// <summary>
		/// save the current data and update the ui status
		/// </summary>
		public void Save()
		{
			DM.Start0();
			Write();
			UpdateStatus();
			DM.End0();
		}

		// done
		/// <summary>
		/// creates a copy of the current discipline data file and opens this file for editing<br/>
		/// true = worked<br/>
		/// false = failed<br/>
		/// null = canceled
		/// </summary>
		public bool? SaveAs()
		{
			DM.Start0();
			FilePath<FileNameSimple> filePath = GetSaveAsDataFile();

			bool? result = fm.VerifyOverwriteExistingFile(filePath);

			if (result == false)
			{
				DM.End0("end 1");
				return null; // user canceled}
			}

			if (!fm.CopyFile(dataFilePath, filePath, true))
			{
				DM.End0("end 2");
				return false;
			}

			Close();

			dataFilePath = filePath;

			open();

			// update the file path info and save the info
			updateSettingsFilePath();

			UpdateStatus();

			DM.End0();
			return true;
		}

		// done
		/// <summary>
		/// close the current discipline data file
		/// </summary>
		public void Close()
		{
			DM.Start0();

			if (!DataFileStatus2)
			{
				DM.End0("end 1", "data file status2 is false (no data)");
				return;
			}

			// DM.Stat0($"is initialized? {disciplineMgr.IsInitialized}");

			disciplineMgr.Admin.Write();

			dataFilePath = null;

			// reset and set DataFileStatus to false
			Reset();

			UpdateStatus();

			DM.End0();
		}

		/// <summary>
		/// closed (resets) the current data set without saving first
		/// </summary>
		private void close()
		{
			DM.Start0();

			if (!DataFileStatus2)
			{
				DM.End0("end 1", "data file status2 is false (no data)");
				return;
			}

			dataFilePath = null;

			// reset and set DataFileStatus to false
			Reset();

			UpdateStatus();

			DM.End0();
		}

		// done
		/// <summary>
		/// delete the existing discipline data file<br/>
		/// true = worked
		/// false = failed
		/// </summary>
		public bool? Delete()
		{
			DM.Start0();
			// use task dialog to verify first
			if (!TaskDialogManager.VerifyDeleteDisciplineDataFile())
			{
				// user canceled - do nothing
				UpdateStatus();
				DM.End0("end 1");
				return null;
			}

			try
			{
				File.Delete(dataFilePath.FullFilePath);
			}
			catch
			{
				DM.End0("end 2");
				return false;
			}

			Reset();

			UpdateStatus();

			DM.End0();
			return true;
		}

		// done
		/// <summary>
		/// eliminate changes since the last save and restore the data
		/// </summary>
		public void Cancel()
		{
			DM.Start0();

			disciplineMgr.Admin.Read();

			UpdateStatus();
			DM.End0();
		}

		// done
		/// <summary>
		/// select a new discipline file then start the
		/// discipline manager using this file
		/// save this path to userSettings / update the ui
		/// </summary>
		/// <returns></returns>
		public bool CreateNew()
		{
			DM.Start0();
			if (!GetNewDataFile())
			{
				TaskDialogManager.DisciplineDataFileMissing();
				DM.End0("end 1");
				return false;
			}

			if (open() != true)
			{
				dataFilePath = null;
				TaskDialogManager.DisciplineDataFileMissing();
				DM.End0("end 2");
				return false;
			}

			disciplineMgr.Info.DataClassVersion = Dd.DataFileVersion;
			disciplineMgr.Info.Description = $"{Dd.DataFileDescription} for {dataFilePath.FileNameNoExt}";
			disciplineMgr.Info.Notes = $"{Dd.DataFileNotes} for {dataFilePath.FileNameNoExt}";

			disciplineMgr.Admin.Write();

			updateSettingsFilePath();

			UpdateStatus();

			DM.End0();

			return true;
		}

		// done
		/// <summary>
		/// create & open a new discipline data file using the current path information<br/>
		/// that is, got path but file does not exist<br/>
		/// true = all ok
		/// false = could not create the data file
		/// </summary>
		public bool CreateNew2()
		{
			DM.Start0();

			if (open() != true)
			{
				dataFilePath = null;
				TaskDialogManager.DisciplineDataFileMissing();
				DM.End0("end 1");
				return false;
			}

			disciplineMgr.Info.DataClassVersion = Dd.DataFileVersion;
			disciplineMgr.Info.Description = $"{Dd.DataFileDescription} for {dataFilePath.FileNameNoExt}";
			disciplineMgr.Info.Notes = $"{Dd.DataFileNotes} for {dataFilePath.FileNameNoExt}";

			disciplineMgr.Admin.Write();

			updateSettingsFilePath();

			UpdateStatus();

			DM.End0();

			return true;
		}


		public bool GetNewDataFile()
		{
			DM.Start0();
			if (!RequestNewDataFile())
			{
				DataFilePath = null;

				DM.End0("end 1");
				return false;
			}

			DataFilePath = fm.SelectedFilePath;

			DM.End0();

			return true;
		}

		public bool GetExistDataFile()
		{
			if (!RequestExistDataFile())
			{
				return false;
			}

			DataFilePath = fm.SelectedFilePath;

			return true;
		}


		/// <summary>
		/// Get the filePath for the a new discipline data file
		/// </summary>
		public FilePath<FileNameSimple> GetSaveAsDataFile()
		{
			if (!RequestNewDataFile())
			{
				DataFilePath = null;
				return null;
			}

			return fm.SelectedFilePath;
			;
		}

		/// <summary>
		/// requests user to select a new discipline data file
		/// </summary>
		public bool RequestNewDataFile()
		{
			DM.Start0();
			CommonFileDialogFilter filter = new CommonFileDialogFilter();
			filter.DisplayName = "New Discipline Data File";
			filter.Extensions.Add(defaultFilePath.FileExtensionNoSep);


			DM.End0();
			return fm.GetNewFile(
				"New Discipline Data File",
				defaultFilePath,
				filter);
		}

		public bool RequestExistDataFile()
		{
			CommonFileDialogFilter filter = new CommonFileDialogFilter();
			filter.DisplayName = "Exist Discipline Data File";
			filter.Extensions.Add(defaultFilePath.FileExtensionNoSep);

			return fm.GetExistFile(
				"New Discipline Data File",
				defaultFilePath,
				filter);
		}

	#endregion

	#region datamanager ops

		public bool Remove(string key)
		{
			DM.Start0();
			bool result = disciplineMgr.Data.Remove(key);
			DM.End0();
			return result;
		}

		public bool IsNewKey(string key)
		{
			return disciplineMgr.Data.IsNewKey(key);
		}

		public bool? UpdateKey(string currKey, string newKey)
		{
			return disciplineMgr.Data.UpdateKey(currKey, newKey);
		}

		public DisciplineListData Find(string key)
		{
			return disciplineMgr.Data.Find(key);
		}

		public void Read()
		{
			DM.InOut0();
			disciplineMgr.Admin.Read();

			disciplineMgr.Data.UpdateDisciplineView();
		}

		public void Write()
		{
			DM.InOut0();

			// disciplineMgr.Info.Header.DataClassVersion = Dd.DataFileVersion;
			// disciplineMgr.Info.Header.Description = Dd.DataFileDescription;
			// disciplineMgr.Info.Header.Notes = Dd.DataFileNotes;

			disciplineMgr.Admin.Write();
		}

		/// <summary>
		/// resets the data / closes the data file / data manager
		/// </summary>
		public void Reset()
		{
			DM.Start0();

			disciplineMgr.Close();

			uss.UpdateDataFilePath(null);

			DM.End0();
		}

		public DisciplineListData Add( string key, string disciplineCode, string title, string dispName,
			string desc, bool locked)
		{
			return disciplineMgr.Data.Add( key,  disciplineCode, title,  dispName, desc, locked);
		}

		public void AddCombo(string key)
		{
			disciplineMgr.Data.ComboDisciplines.Add(key);
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		/// <summary>
		/// set the status of the discipline data file (exist or not) and the
		/// discipline data file path info (null or not)<br/>
		/// true = both parts found (file exists and the path info is good)
		/// </summary>
		public void SetDataFileStatus()
		{
			DM.Start0();

			if (!uss.HasDisciplineDataFilePath)
			{
				// if (!getAltPathFromRecent())
				// {
				// 	// one or both parts are no good
				// 	dataFilePath = null;
				// 	DisciplineDataFileStatus = null;
				//
				// 	return;
				// }

				// one or both parts are no good
				dataFilePath = null;
				disciplineDataFileStatus = null;

				DM.End0("end 1", "NO data file path / NO data file");
				return;
			}

			// got path info - both parts
			// assign -> dataFilePath
			setfilePathFromUsrSetg();

			if (dataFilePath.Exists)
			{
				// both parts good - got path and file exists
				disciplineDataFileStatus = true;
			#if DML3
				DM.Stat0("GOT data file path / got data file path");
			#endif
			}
			else
			{
				// got path but the file does not exist
				disciplineDataFileStatus = false;
			#if DML3
				DM.Stat0("GOT data file path / NO data file path");
			#endif
			}

			DM.End0();
		}

		private bool getAltPathFromRecent()
		{
			if (uss.RecentList == null || uss.RecentList.Count == 0) return false;

			UserSettings.Data.DisciplineDataFileName =
				uss.RecentList[0].FileNameNoExt;

			UserSettings.Data.DisciplineDataFileFolder =
				uss.RecentList[0].FolderPath;

			return true;
		}

		/*
		// before getting here - the user settings file must exist
		// and have been read however, the discipline data
		// file only may exist.
		public bool startupValidatex()
		{
			// case 1 - datafile info exists and the data
			// file exists
			setfilePathFromUsrSetg();

			if (DataFilePath == null || !DataFilePath.IsFound)
			{
				// datafile information does not exist in the user settings
				// collection
				// need to get this from the user
				if (!GetNewDataFile()) return false;

				// selected file is in DataFilePath
				// 1. update setting path info
				// 2. save settings
				UserSettings.Data.DisciplineDataFileFolder =
					DataFilePath.FullFilePath;

				UserSettings.Data.DisciplineDataFileName = 
					DataFilePath.FileNameNoExt;

				UserSettings.Admin.Write();
			}

			disciplineMgr.Configure("", "", null);

			disciplineMgr.Configure(DataFilePath,
				DataFilePath.FileNameNoExt, null);

			return true;
		}

		*/

		public bool InitDataFile()
		{
			DM.Start0();

			if (!disciplineDataFileStatus.HasValue) // is null
			{
				// no path and no data file
				// Debug.WriteLine("@ initdatafile / createnew()");
				// mainWin.tbl("@dfm.InitDataFile - no path - CreateNew()");
				// return CreateNew();
			#if DML3
				DM.Stat0("found: no path - not created");
			#endif

				DM.End0();
				return true;
			}
			else if (disciplineDataFileStatus == false)
			{
				if (!requestReplaceMissingDisciplineDataFile()) return false;

				// Debug.WriteLine("@ initdatafile / CreateNew2()");
				// mainWin.tbl("@dfm.InitDataFile - got path but no file - CreateNew2()");
			#if DML3
				DM.Stat0("found: got path but no file - CreateNew2()");
			#endif

				DM.End0();
				return CreateNew2();
			}

			// Debug.WriteLine("@ initdatafile / OpenPreConfgd()");
			// mainWin.tbl("@dfm.InitDataFile - got path and file - openPreConfgd");
		#if DML3
			DM.Stat0("found: got path and file - openPreConfgd");
		#endif

			bool result = OpenPreConfgd();

			DM.End0();

			return result;
		}

		public void UpdateStatus()
		{
			OnPropertyChange(nameof(DisciplineDataMgrIsInit));
			OnPropertyChange(nameof(DisciplineDataFileExists));
			OnPropertyChange(nameof(DataFileStatus2));
		}

	#endregion

	#region private methods

		private void updateSettingsFilePath()
		{
			DM.Start0();
			uss.UpdateDataFilePath(dataFilePath);

			mainWin.UpdateCollectionProps();
			DM.End0();
		}

		private bool requestReplaceMissingDisciplineDataFile()
		{
			return TaskDialogManager.RequestReplaceMissingDiscDataFile(
				dataFilePath.FileNameNoExt, dataFilePath.FolderPath) == TaskDialogResult.Yes;
		}

		public void setfilePathFromUsrSetg()
		{
			DM.InOut0();

			DataFilePath = new FilePath<FileNameSimple>(
				new []
				{
					UserSettings.Data.DisciplineDataFileFolder,
					UserSettings.Data.DisciplineDataFileName + "." +
					UserSettingDataFile.DATAFILE_EXTN
				}
				);
		}

		private void setDataFilePath(string fileName, string folderPath)
		{
			DM.InOut0();

			DataFilePath = new FilePath<FileNameSimple>(
				new []
				{
					folderPath, fileName + "." + UserSettingDataFile.DATAFILE_EXTN
				}
				);
		}

		private void setDefaultFilePath()
		{
			DM.Start0();

			docFolder = KnownFolders.Documents;

			if (!docFolder.PathExists)
			{
				return;
			}

			string path = docFolder.Path;

			DefaultFolderPath = new FilePath<FileNameSimple>(path);

			DefaultFilePath = new FilePath<FileNameSimple>(
				$"{path}\\{DEF_DISCIPLINE_DATAFILE_FILENAME}.{UserSettingDataFile.DATAFILE_EXTN}");

			DM.End0();
		}

		public StorageMgrInfo<DisciplinesDataSet> Copy(StorageMgrInfo<DisciplinesDataSet> info)
		{
			StorageMgrInfo<DisciplinesDataSet> i = new StorageMgrInfo<DisciplinesDataSet>();

			i.FileType          = info.FileType;
			i.Description       = info.Description;
			i.Notes             = info.Notes;
			i.DataClassVersion  = info.DataClassVersion;

			i.Header.AssemblyVersion  = info.Header.AssemblyVersion;
			i.Header.SaveDateTime  = 	 info.Header.SaveDateTime;
			i.Header.SavedBy  = 		 info.Header.SavedBy;
			i.Header.SettingsVersion  = info.Header.SettingsVersion;

			return i;
		}

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
			return $"this is {nameof(DataFileManager)}";
		}

	#endregion
	}
}