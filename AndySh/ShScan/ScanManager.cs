#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Settings;
using SettingsManager;
using ShSheetData;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  12/8/2024 1:24:23 PM

namespace ShScan
{
	public class ScanManager
	{
	#region private fields

		private DataManager<SheetMetricData> dataMgr;

		private Dictionary<string, SheetData> shtMetsList;

		private List<FilePath<FileNameSimple>> inputList;

		private int shtRectsFound;
		private int optRectsFound;

	#endregion

	#region ctor

	#endregion

	#region public properties

		private Dictionary<string, SheetData> ShtMetricList
		{
			get => shtMetsList;
			set => shtMetsList = value;
		}

		private List<FilePath<FileNameSimple>> InputPdfFileList
		{
			get => inputList;
			set => inputList = value;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool ScanSheets(List<FilePath<FileNameSimple>> inputPdfFileList,
			Dictionary<string, SheetData> shtMetricsList)
		{
			this.inputList = inputPdfFileList;
			this.shtMetsList = shtMetricsList;

			if (inputList == null || inputList.Count == 0) return false;
			if (shtMetsList == null) return false;

			shtRectsFound = 0;
			optRectsFound = 0;

			



			return true;
		}

	#endregion

	#region private methods

		/// <summary>
		/// check if a file has been scanned
		/// </summary>
		private bool checkForDuplicate(string fileName)
		{
			return dataMgr.Data.SheetDataList.ContainsKey(fileName);
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(ScanManager)}";
		}

	#endregion
	}
}