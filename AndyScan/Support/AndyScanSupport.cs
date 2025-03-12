#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Settings;
using SettingsManager;
using ShScan;
using ShTempCode.DebugCode;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  12/6/2024 6:33:14 PM

namespace AndyScan.Support
{
	public class AndyScanSupport
	{
		private const int STATUS_MARGIN = -30;
		private const int STATUS_OK_WIDTH = -7 - 8; // ok width + color widths

		// each adds 4 characters to the string width
		private const string NORMAL = "\u001b[0m"; 
		private const string RED = "\u001b[31m";
		private const string GREEN = "\u001b[32m";


	#region private fields

		private ShSamples smpl;

		private ITblkFmt iw;
		private bool canScan;

	#endregion

	#region ctor

		public AndyScanSupport(ITblkFmt w)
		{
			iw = w;

			smpl = new ShSamples();
		}

	#endregion

	#region public properties

		// objects

		public DataManager<SheetMetricData> ShtMetricDataMgr { get; private set; }

		public FilePath<FileNameSimple> InputPdfs { get; set; }
		public FilePath<FileNameSimple> OutputDataFile { get; set; }

		public List<FilePath<FileNameSimple>> InputPdfFileList { get; set; }

		public ScanManager ScanManager { get; set; }


		// values


		// status

		public bool ShtMetricDataMgrConfig => (ShtMetricDataMgr != null && ShtMetricDataMgr.IsInitialized);

		public bool CanScan
		{
			get => canScan;
			set => canScan = value;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void ShowStatus()
		{
			canScan = true;

			showInputPdfStatus();
			showInputPdfFileListStatus();
			showOutputFileStatus();
			showSheetMetricFileStatus();
			showCanScan();
		}
		
		/// <summary>
		/// true = got data<br/>
		/// false = not got data
		/// </summary>
		public bool SelectData()
		{
			if (smpl.SelectScanSample(-1, false) == false)
			{
				return false;
			}

			InputPdfs = smpl.Selected.ScanPDfFolder;
			OutputDataFile = smpl.Selected.DataFilePath;

			return true;
		}

		public bool PrepForScan()
		{
			if (!prepForScan())
			{
				iw.TblkMsgLine("scan prep failed - try again?");
				return false;
			}

			return true;
		}

		public bool Scan()
		{
			if (!canScan) { return false; }

			ScanManager = new ScanManager();
			return true;
		}

	#endregion

	#region private methods

		private void showCanScan()
		{
			string prefaceMsg =      "can scan status";
			string canScanStatusMsg;

			if (CanScan)
			{
				canScanStatusMsg = $"{green("is OK")} to scan";
			}
			else
			{
				canScanStatusMsg = $"{red("is NOT ok")} to scan";
			}


			iw.TblkMsg($"\n{prefaceMsg, STATUS_MARGIN} | {canScanStatusMsg}\n");
		}

		private void showInputPdfStatus()
		{
			bool notCfg = InputPdfs == null;
			bool isZero = !InputPdfs?.Exists ?? false;
			bool isZeroOk = false;

			string preface =      "input pdf's here";
			string notCfgMsg =    $"is {red("NOT")} selected";
			string preface2 =     "folder";
			string isZeroMsg =    $"{red("DOES NOT")} exist | file {OutputDataFile?.FileName ?? "na"} | folder {OutputDataFile?.FolderPath ?? "na"}";
			string abvZeroMsg =   $"{green("DOES")} exist | file {OutputDataFile?.FileName ?? "na"} | folder {OutputDataFile?.FolderPath ?? "na"}";


			statusMsg(notCfg, isZero, isZeroOk, preface, notCfgMsg, preface2, isZeroMsg, abvZeroMsg );
		}

		private void showInputPdfFileListStatus()
		{
			bool notCfg = InputPdfFileList == null;
			bool isZero = (InputPdfFileList?.Count ?? -1) == 0;
			bool isZeroOk = false;

			string preface =      "input PDF files";
			string notCfgMsg =    $"is {red("NOT")} configured";
			string preface2 =     "there are";
			string isZeroMsg =    $"{red("NO")} sheet metric files to process";
			string abvZeroMsg =   $"{green($"{InputPdfFileList?.Count ?? 0}")} sheet metric {((InputPdfFileList?.Count ?? 0)>1?"files":"file")} to process";


			statusMsg(notCfg, isZero, isZeroOk, preface, notCfgMsg, preface2, isZeroMsg, abvZeroMsg );
		}

		private void showOutputFileStatus()
		{
			bool notCfg = OutputDataFile == null;
			bool isZero = !OutputDataFile?.Exists ?? false;
			bool isZeroOk = false;

			string preface =      "output data file here";
			string notCfgMsg =    $"is {red("NOT")} configured";
			string preface2 =     "file";
			string isZeroMsg =    $"{red("DOES NOT")} exist | file {OutputDataFile?.FileName ?? "na"} | folder {OutputDataFile?.FolderPath ?? "na"}";
			string abvZeroMsg =   $"{green("DOES")} exist | file {OutputDataFile?.FileName ?? "na"} | folder {OutputDataFile?.FolderPath ?? "na"}";


			statusMsg(notCfg, isZero, isZeroOk, preface, notCfgMsg, preface2, isZeroMsg, abvZeroMsg );
		}

		private void showSheetMetricFileStatus()
		{
			bool notCfg = !ShtMetricDataMgrConfig;
			bool isZero = (ShtMetricDataMgr?.Data.SheetDataList.Count ?? -1) == 0;
			bool isZeroOk = true;

			string preface =      "sheet metric data file";
			string notCfgMsg =    $"is {red("NOT")} configured";
			string preface2 =     "is configured";
			string isZeroMsg =    $"but has {red("NO")} sheets";
			string abvZeroMsg =   $"and has {green($"{(ShtMetricDataMgr?.Data.SheetDataList.Count ?? 0)}")} {((ShtMetricDataMgr?.Data.SheetDataList.Count ?? 0) == 1 ? "sheet" : "sheets")}";


			statusMsg(notCfg, isZero, isZeroOk, preface, notCfgMsg, preface2, isZeroMsg, abvZeroMsg );

		}

		private void statusMsg(bool notConfig, bool isZero, bool isZeroOk, string prefaceMsg, string notConfigMsg, string preface2Msg, string isZeroMsg, string abvZeroMsg)
		{
			CanScan &= (!isZero && !notConfig);

			iw.TblkMsg($"{prefaceMsg, STATUS_MARGIN} | ");
			
			// iw.DebugMsgLine($"\n{canScanStatusStr(notConfig, isZero)}");
			// iw.DebugMsg($"{".", STATUS_MARGIN}");

			if (notConfig)
			{
				iw.TblkMsgLine($"{red("is NOT ok"),STATUS_OK_WIDTH} | {notConfigMsg}");
			} 
			else
			{
				if (!isZeroOk && isZero)
				{
					iw.TblkMsg($"{red("is NOT ok"),STATUS_OK_WIDTH} | {preface2Msg} | ");
					
				} else
				{
					iw.TblkMsg($"{green("is OK"),STATUS_OK_WIDTH} | {preface2Msg} | ");
				} 

				if (isZero)
				{
					iw.TblkMsgLine($"{isZeroMsg}");
				}
				else
				{
					iw.TblkMsgLine($"{abvZeroMsg}");
				}
			}
		}

		private string canScanStatusStr(bool notConfig, bool isZero)
		{
			string notCfgMsg = !notConfig ? $"{green($"{!notConfig}")}" : $"{red($"{!notConfig}")}";
			string isZeroMsg = !isZero ? $"{green($"{!isZero}")}" : $"{red($"{!isZero}")}";
			string canScanMsg = CanScan ? $"{green($"{CanScan}")}" : $"{red($"{CanScan}")}";

			return $"not cfg = {notCfgMsg} | is zero = {isZeroMsg} | can scan = {canScanMsg}";
		}

		private bool prepForScan()
		{
			// if (!SelectData()) return false;

			if (!getMetricFileList()) return false;

			if (!initSheetMetricFile()) return false;

			if (!getSheetMetricFile()) return false;

			return true;
		}

		private bool getMetricFileList()
		{
			if (InputPdfs == null) return false;
			if (!InputPdfs.Exists) return false;

			InputPdfFileList = new List<FilePath<FileNameSimple>>();

			string[] files = Directory.GetFiles(InputPdfs.FullFilePath, "*.pdf", SearchOption.TopDirectoryOnly);

			foreach (string file in files)
			{
				InputPdfFileList.Add(new FilePath<FileNameSimple>(file));
			}

			return true;
		}

		private bool getSheetMetricFile()
		{
			if (ShtMetricDataMgr == null) return false;

			ShtMetricDataMgr.Admin.Read();

			return true;
		}

		private bool initSheetMetricFile()
		{
			if (OutputDataFile == null || !OutputDataFile.Exists) return false;

			ShtMetricDataMgr = new DataManager<SheetMetricData>(OutputDataFile);

			return true;
		}
		
		private string red(string text)
		{
			return $"{RED}{text}{NORMAL}";
		}
		
		private string green(string text)
		{
			return $"{GREEN}{text}{NORMAL}";
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(AndyScanSupport)}";
		}

	#endregion

	#region switchboard main routines

	#endregion


	}
}