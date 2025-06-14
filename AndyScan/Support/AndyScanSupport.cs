#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Settings;
using SettingsManager;
using ShScan;
using UtilityLibrary;
using AndyScan.Samples;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using ShSheetData;

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

		private SampleData smpl;

		private IFdFmt iw;
		private bool canScan;

	#endregion

	#region ctor

		public AndyScanSupport(IFdFmt w)
		{
			iw = w;

			smpl = new SampleData();
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

		public void ReadSampleScanData()
		{
			string error;

				// FilePath<FileNameSimple> fp = new FilePath<FileNameSimple>(@"C:\Users\jeffs\Documents\Programming\VisualStudioProjects\PDF SOLUTIONS\_SamplesScan\SheetMetricA.xml");
				FilePath<FileNameSimple> fp = new FilePath<FileNameSimple>(@"C:\Users\jeffs\Documents\Programming\VisualStudioProjects\PDF SOLUTIONS\_SamplesScan\SheetMetric0.xml");
			
				ShtMetricDataMgr = new DataManager<SheetMetricData>(fp);

				ShtMetricDataMgr.Admin.Read();

				// string t0 = "SheetData1";
				// string t0 = "A0.1.0 - COVER SHEET";
				string t0 = "TestBoxes";

				Dictionary<SheetRectId, SheetRectData<SheetRectId>> sr = ShtMetricDataMgr.Data.SheetDataList[t0].ShtRects;
				Dictionary<SheetRectId, SheetRectData<SheetRectId>> or = ShtMetricDataMgr.Data.SheetDataList[t0].OptRects;
	
		}


		public void CreateSampleScanData()
		{
			FilePath<FileNameSimple> fp = new FilePath<FileNameSimple>(@"C:\Users\jeffs\Documents\Programming\VisualStudioProjects\PDF SOLUTIONS\_SamplesScan\SheetMetricA.xml");
			
			ShtMetricDataMgr = new DataManager<SheetMetricData>(fp);

			SheetRectId srid;

			TextSettings ts = new TextSettings("Arial", 1, 12.0f);
			ts.InfoText = "hello world";
			ts.TextColor = Color.CreateColorWithColorSpace([1.0f,1.0f,0.5f,0.2f]);

			BoxSettings bs = new BoxSettings(new Rectangle(100.0f, 200.0f));
			bs.TextBoxRotation = 30.0f;
			bs.BdrColor = Color.CreateColorWithColorSpace([1.0f,0.5f,0.5f,0.2f]);



			SheetRectData<SheetRectId> srd;

			Dictionary<SheetRectId, SheetRectData<SheetRectId>> sr;
			sr = new Dictionary<SheetRectId, SheetRectData<SheetRectId>>();

			srid = SheetRectId.SM_AUTHOR;

			srd = new SheetRectData<SheetRectId>(SheetRectType.SRT_BOX, srid, new Rectangle(100.0f, 200.0f));
			srd.TextSettings = ts;
			srd.BoxSettings = bs;
			srd.SheetRotation = 90;

			sr.Add(srid, srd);

			srid = SheetRectId.SM_BANNER_1ST;

			srd = new SheetRectData<SheetRectId>(SheetRectType.SRT_BOX, srid, new Rectangle(50.0f, 200.0f));
			srd.TextSettings = ts;
			srd.BoxSettings = bs;
			srd.SheetRotation = 90;

			sr.Add(srid, srd);;

			srid = SheetRectId.SM_WATERMARK2;

			srd = new SheetRectData<SheetRectId>(SheetRectType.SRT_BOX, srid, new Rectangle(300.0f, 900.0f));
			srd.TextSettings = ts;
			srd.BoxSettings = bs;
			srd.SheetRotation = 90;

			sr.Add(srid, srd);



			Dictionary<SheetRectId, SheetRectData<SheetRectId>> or;
			or = new Dictionary<SheetRectId, SheetRectData<SheetRectId>>();

			srid = SheetRectId.SM_OPT0;

			srd = new SheetRectData<SheetRectId>(SheetRectType.SRT_BOX, srid, new Rectangle(200.0f, 100.0f));
			srd.TextSettings = ts;
			srd.BoxSettings = bs;
			srd.SheetRotation = 90;

			or.Add(srid, srd);

			srid = SheetRectId.SM_OPT1;

			srd = new SheetRectData<SheetRectId>(SheetRectType.SRT_BOX, srid, new Rectangle(200.0f, 50.0f));
			srd.TextSettings = ts;
			srd.BoxSettings = bs;
			srd.SheetRotation = 90;

			or.Add(srid, srd);

			srid = SheetRectId.SM_OPT2;

			srd = new SheetRectData<SheetRectId>(SheetRectType.SRT_BOX, srid, new Rectangle(900.0f, 300.0f));
			srd.TextSettings = ts;
			srd.BoxSettings = bs;
			srd.SheetRotation = 90;

			or.Add(srid, srd);

			ShtMetricDataMgr.Data.SheetDataList = new Dictionary<string, SheetData>();

			SheetData sd = new SheetData("SheetData1", "SheetData1 Sample Scan Data");

			sd.PageSizeWithRotation = new Rectangle(3000.0f, 4200.0f);
			sd.SheetRotation = 90;
			sd.ShtRects = sr;
			sd.OptRects = or;

			ShtMetricDataMgr.Data.SheetDataList.Add("SheetData1", sd);

			ShtMetricDataMgr.Admin.Write();

			ShtMetricDataMgr = null;
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


			iw.TblkMsg($"\n{prefaceMsg, STATUS_MARGIN} | {canScanStatusMsg}\n", SHOW_WHICH.SW_TBLK1);
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

			KeyValuePair<string, SheetData>? a = ShtMetricDataMgr?.Data.SheetDataList.First();

			// iw.TblkFmtdLine($"<red>box count</red> for {a.Value.Key} | {a.Value.Value.ShtRects.Count.ToString()}");

		}

		private void statusMsg(bool notConfig, bool isZero, bool isZeroOk, string prefaceMsg, string notConfigMsg, string preface2Msg, string isZeroMsg, string abvZeroMsg)
		{
			CanScan &= (!isZero && !notConfig);

			iw.TblkMsg($"{prefaceMsg, STATUS_MARGIN} | ", SHOW_WHICH.SW_TBLK1);
			
			// iw.DebugMsgLine($"\n{canScanStatusStr(notConfig, isZero)}");
			// iw.DebugMsg($"{".", STATUS_MARGIN}");

			if (notConfig)
			{
				iw.TblkMsgLine($"{red("is NOT ok"),STATUS_OK_WIDTH} | {notConfigMsg}", SHOW_WHICH.SW_TBLK1);
			} 
			else
			{
				if (!isZeroOk && isZero)
				{
					iw.TblkMsg($"{red("is NOT ok"),STATUS_OK_WIDTH} | {preface2Msg} | ", SHOW_WHICH.SW_TBLK1);
					
				} else
				{
					iw.TblkMsg($"{green("is OK"),STATUS_OK_WIDTH} | {preface2Msg} | ", SHOW_WHICH.SW_TBLK1);
				} 

				if (isZero)
				{
					iw.TblkMsgLine($"{isZeroMsg}", SHOW_WHICH.SW_TBLK1);
				}
				else
				{
					iw.TblkMsgLine($"{abvZeroMsg}", SHOW_WHICH.SW_TBLK1);
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
			// return $"{RED}{text}{NORMAL}";
			return text;
		}
		
		private string green(string text)
		{
			// return $"{GREEN}{text}{NORMAL}";
			return text;
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