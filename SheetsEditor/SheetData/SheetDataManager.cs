#region + using

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Settings;
using SettingsManager;
using SheetEditor.SheetData;
using SheetEditor.SheetData;
using SheetEditor.Support;

using UtilityLibrary;

#endregion


namespace SheetEditor.SheetData
{
	public static class SheetDataManager2
	{
		// public static string DataFileName => SheetDataSet.DataFileName;
		public static string DataFilePath => Path.SettingFilePath;
		public static FilePath<FileNameSimple> DataPath => Path.FilePath;

		public static bool Initialized => Manager != null;
		public static bool Configured => Initialized && GotDataPath;

		public static bool GotDataPath => Path?.SettingFolderPathIsValid ?? false;
		public static bool GotDataSheets => SheetsCount > 0;
		public static int SheetsCount => Data?.SheetDataList?.Count ?? -1;
		public static bool SettingsFileExists => Path?.SettingFileExists ?? false;

		// public static Dictionary<string, SheetData.SheetData> SheetDataList => Data?.SheetDataList;
		
		public static DataManager<SheetDataSet> Manager { get; private set; }

		public static SheetDataSet Data => Manager?.Data ?? null;
		public static SettingsMgr<StorageMgrPath, StorageMgrInfo<SheetDataSet>, SheetDataSet> Admin => Manager?.Admin ?? null;
		public static StorageMgrInfo<SheetDataSet> Info => Manager?.Info ?? null;
		public static StorageMgrPath Path => Admin?.Path ?? null;

		// enumerators

		public static IEnumerable<KeyValuePair<string, SheetData.SheetData>> GetSheets()
		{
			foreach (KeyValuePair<string, SheetData.SheetData> kvp in Data.SheetDataList)
			{
				yield return kvp;

			}
		}

		public static IEnumerable<KeyValuePair<SheetRectId, SheetRectData<SheetRectId>>> GetRects(string sheet)
		{
			foreach (KeyValuePair<SheetRectId, SheetRectData<SheetRectId>> kvp in Data.SheetDataList[sheet].ShtRects)
			{
				yield return kvp;

			}
		}

		// info

		public static bool RectIsType(SheetRectType type, SheetRectType test)
		{
			return (type & test) != 0;
		}

		// operations

		public static void ResetSheetDataList()
		{
			Data.SheetDataList = new Dictionary<string, SheetData.SheetData>();
		}

		public static void UpdateHeader()
		{
			DM.DbxLineEx(0, "\tdata manager - update header");
			Info.FileType = SettingFileType.SETTING_MGR_DATA;
			Info.DataClassVersion = Data.DataFileVersion;
			Info.Description = Data.DataFileDescription;
		}

		public static bool Init(FilePath<FileNameSimple> filePath)
		{
			if (Configured) return false;

			// if (Manager != null || filePath == null) return false;

			DM.DbxLineEx(0, "\t\tdata manager - init");

			Manager = new DataManager<SheetDataSet>(filePath);

			return true;
		}

		// public static void Create(FilePath<FileNameSimple> filePath)
		public static void Create()
		{
			if (Manager == null) return;

			DM.DbxLineEx(0, "\tdata manager - create");
			
			// Manager = new DataManager<SheetDataSet>(filePath);

			UpdateHeader();

			ResetSheetDataList();

			Write();
		}
		
		// public static void Open(FilePath<FileNameSimple> filePath)
		public static void Open()
		{
			if (!Configured) return;

			DM.DbxLineEx(0, "\t\tdata manager - open");

			Read();
		}

		public static void Read()
		{
			DM.DbxLineEx(0, "\t\tdata manager - read");
			Admin.Read();
		}

		public static void Write()
		{
			DM.DbxLineEx(0, "\tdata manager - write");
			Admin.Write();
		}

		public static void Reset()
		{
			DM.DbxLineEx(0, "\tdata manager - reset");

			Manager.ResetPath();

			Manager.Reset();
			
			// UpdateHeader();
			//
			// Write();

			// Initialized = false;
		}

		public static void Close()
		{
			DM.DbxLineEx(0, "\tdata manager - close");

			UpdateHeader();

			Write();

			Manager.Reset();
			Manager.ResetPath();

			// Initialized = false;
		}

		public static void SeeData()
		{
			StorageMgrInfo<SheetDataSet> i1 = Manager.Info;
			StorageMgrInfo<SheetDataSet> i2 = Admin.Info;
			
			SheetDataSet d1 = Manager?.Data;
			SheetDataSet d2 = Info.Data;
			
			i1.Description += " + i1 (manager)";
			i2.Description += " + i2 (admin)";
			
			d1.DataFileDescription += " + d1 (manager)";
			d2.DataFileDescription += " + d2 (info)";

			// int a = 1;
		}
	}
}

