
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndyScan.Samples;
using iText.Kernel.Geom;
using Settings;
using SettingsManager;
using ShSheetData;
using UtilityLibrary;



// user name: jeffs
// created:   6/7/2025 11:04:31 PM

namespace SuiteInfoEditor.Support
{
	public class SheetMetricDataMgr
	{
		private DataManager<SheetMetricData> metricsMgr;

		private CsFlowDocManager w = CsFlowDocManager.Instance;

		private Sample sample;
		private SampleData smplData;
		private int sampIdx;

		public bool IsConfig => metricsMgr != null && metricsMgr.IsInitialized && metricsMgr?.Data.SheetDataList != null;

		public FilePath<FileNameSimple> OutputDataFile;

		public SheetMetricDataMgr()
		{
			smplData = new SampleData();
		}

		private void selectSampleData(int idx)
		{
			sample = SampleData.SampleScanData[idx];

			metricsMgr = new DataManager<SheetMetricData>(sample.DataFilePath);

			metricsMgr.Admin.Read();
		}

		public void ShowZeroShtMetxFileStat()
		{
			GeneralSettingsMgr.ShowSetgColumn = 44;

			selectSampleData(0);

			showShtMetxFileStat();

			showSheetMetrics();

			w.AddDescTextLineTb("<repeat text='-' tocolumn='85'/>");
		}

		public void ShowAllShtMetxFileStat()
		{
			GeneralSettingsMgr.ShowSetgColumn = 44;

			int[] sampleList = [0, 1, 2, 7, 9];

			foreach (int i in sampleList)
			{
				sampIdx = i;

				selectSampleData(i);

				showShtMetxFileStat();

				showSheetMetrics();

				w.AddLineBreaksTb(1);
			}

			w.AddDescTextLineTb("<repeat text='-' tocolumn='85'/>");
		}

		private void showSampleHeader()
		{
			if (sample.DataFilePath == null)
			{
				w.AddDescTextLineTb("<darkgray>sheet metric data PATH is <red>NOT</red> configured</darkgray>");
				return;
			}

			w.AddDescTextTb("<darkgray>Sample Info for </darkgray>");
			w.AddDescTextLineTb($"[<cyan>{sample.Description}</cyan>]");

			w.AddLineBreaksTb(1);

			GeneralSettingsMgr.ShowSetting("DataFilePath", $"{sample.DataFilePath}");
			GeneralSettingsMgr.ShowSetting("Path Notes", "");

			w.AddDescTextTb("<indent spaces='+4'/>");
			w.AddDescTextLineTb(sample.DataFpNotes);
			w.AddDescTextTb("<indent spaces='-4'/>");

		}

		private void showShtMetxFileStat()
		{
			w.AddDescTextLineTb("<repeat text='-' tocolumn='85'/>");

			w.AddDescTextLineTb($"<cyan>for sample index of (<red>{sampIdx}</red>)</cyan><linebreak/>");

			w.AddDescTextLineTb("<darkgray>sheet metric data file</darkgray>");

			w.StartTb("<indent spaces ='4'/>");

			w.AddLineBreaksTb(1);

			showSampleHeader();

			w.AddLineBreaksTb(1);

			if (!IsConfig)
			{
				w.AddDescTextLineTb("<red>this sample is not configured</red>");
				w.AddDescTextTb("<indent spaces='-4'/>");
				w.AddLineBreaksTb(1);
				return;
			}

			bool isConfig = IsConfig;
			bool isZero = (metricsMgr?.Data.SheetDataList.Count ?? -1) == 0;

			if (isConfig)
			{
				w.AddDescTextTb("<lawngreen>IS</lawngreen> configured ");

				if (isZero) w.AddDescTextLineTb("but has <red>Zero</red> sheets");
				else w.AddDescTextLineTb($"and has (<lawngreen>{(metricsMgr?.Data.SheetDataList.Count ?? 0)}</lawngreen>) {((metricsMgr?.Data.SheetDataList.Count ?? 0)== 1? "Sheet":"Sheets")}");

				w.AddLineBreaksTb(1);

				GeneralSettingsMgr.ShowSetting("Description", $"{metricsMgr.Info.Description}");
				GeneralSettingsMgr.ShowSetting("Notes", $"{metricsMgr.Info.Notes}");
				GeneralSettingsMgr.ShowSetting("DataClassVersion", $"{metricsMgr.Info.DataClassVersion}");
				GeneralSettingsMgr.ShowSetting("FileType", $"{metricsMgr.Info.FileType}");
				GeneralSettingsMgr.ShowSetting("SavedBy", $"{metricsMgr.Info.Header.SavedBy}");
				GeneralSettingsMgr.ShowSetting("SaveDateTime", $"{metricsMgr.Info.Header.SaveDateTime}");
				GeneralSettingsMgr.ShowSetting("Full File Path", $"{metricsMgr.Path.FilePath.FullFilePath}");
				GeneralSettingsMgr.ShowSetting("Exists", $"{metricsMgr.Path.FilePath.Exists}");
			}
			else
			{
				w.AddDescTextLineTb("is <red>NOT</red> configured");
			}

			w.AddLineBreaksTb(1);

			w.AddDescTextLineTb("<indent/>");
		}

		private string fmtSingleArray(float[] fa)
		{
			StringBuilder sb = new StringBuilder("[ ");

			foreach ( float f in fa )
			{
				sb.Append($"{f.ToString("F4")} ");
			}

			sb.Append(" ]");

			return sb.ToString();
		}

		private void showSheetMetrics()
		{
			w.AddDescTextTb("<indent spaces='+4'/>");

			if (metricsMgr.Data.SheetDataList == null || metricsMgr.Data.SheetDataList.Count == 0)
			{
				w.AddDescTextLineTb("<darkgray>sheet data list is <red>NULL or EMPTY</red></darkgray>");

				w.AddDescTextTb("<indent spaces='-4'/>");

				return;
			}

			foreach ((string key, SheetData sd) in metricsMgr.Data.SheetDataList)
			{
				w.AddDescTextLineTb("<repeat text='-' tocolumn='85'/>");

				w.AddDescTextLineTb($"<lawngreen>{sd.Name}</lawngreen> <dimgray>(Sheet Name) [data from \"SheetData\"]</dimgray><linebreak/>");

				w.AddDescTextTb("<indent spaces='+4'/>");

				GeneralSettingsMgr.ShowSetting("Description", sd.Description);
				GeneralSettingsMgr.ShowSetting("CreatedDt", sd.Created);
				GeneralSettingsMgr.ShowSetting("SheetRotation", sd.SheetRotation.ToString());
				GeneralSettingsMgr.ShowSetting("PageSizeWithRotation", (sd.PageSizeWithRotation ?? new Rectangle(0f,0f)).ToStringRect());

				w.AddLineBreaksTb(1);


				if (sd.ShtRects.Count > 0)
				{
					GeneralSettingsMgr.ShowSetting("** ShtRects Count **", sd.ShtRects.Count.ToString(), "dimgray", "lawngreen");

					w.AddLineBreaksTb(1);

					showRects(sd.ShtRects);
				}
				else
				{
					w.AddDescTextLineTb("<limegreen>There are no ShtRects</limegreen>");
				}

				if (sd.OptRects.Count > 0)
				{
					GeneralSettingsMgr.ShowSetting("** OptRects Count **", sd.OptRects.Count.ToString(), "dimgray", "lawngreen");

					w.AddLineBreaksTb(1);

					showRects(sd.OptRects);
				}
				else
				{
					w.AddDescTextLineTb("<limegreen>There are no OptRects</limegreen>");
				}

				w.AddLineBreaksTb(1);

				w.AddDescTextTb("<indent spaces='-4'/>");
				
			}

			w.AddDescTextTb("<indent spaces='-4'/>");
		}

		private void showRects(Dictionary<SheetRectId, SheetRectData<SheetRectId>> srds)
		{
			foreach ((SheetRectId srid, SheetRectData<SheetRectId> srd) in srds)
			{
				w.AddDescTextLineTb($"{srid.ToString()} <dimgray>(Sht Rect Id) [Data from \"SheetRectData\"]</dimgray><linebreak/>");

				w.AddDescTextTb("<indent spaces='+4'/>");

				GeneralSettingsMgr.ShowSetting("SR Id", srd.Id.ToString());
				GeneralSettingsMgr.ShowSetting("SR Type", srd.Type.ToString());

				w.AddDescTextLineTb("<linebreak/>** Box Settings **<linebreak/>");

				w.AddDescTextTb("<indent spaces='+4'/>");
				showBoxSettings(srd.BoxSettings);

				w.AddDescTextTb("<indent spaces='-4'/>");
				w.AddDescTextLineTb("<linebreak/>** Text Settings **<linebreak/>");
				w.AddDescTextTb("<indent spaces='+4'/>");

				showTextSettings(srd.TextSettings);
				w.AddDescTextTb("<indent spaces='-4'/>");

				w.AddLineBreaksTb(1);

				w.AddDescTextTb("<indent spaces='-4'/>");
			}
		}



		private void showBoxSettings(BoxSettings bs)
		{

			GeneralSettingsMgr.ShowSetting("TextBoxRotation",bs.TextBoxRotation.ToString("F2"));
			GeneralSettingsMgr.ShowSetting("Rect", bs.Rect.ToStringRect());
			GeneralSettingsMgr.ShowSetting("Fill Color", bs.FillColorA.ToStringFa());
			GeneralSettingsMgr.ShowSetting("Fill Opacity", bs.FillOpacity.ToString("F2"));
			GeneralSettingsMgr.ShowSetting("Bdr Color", bs.BdrColorA.ToStringFa());
			GeneralSettingsMgr.ShowSetting("Bdr Opacity", bs.BdrOpacity.ToString("F2"));
			GeneralSettingsMgr.ShowSetting("Bdr Width", bs.BdrWidth.ToString("F2"));
			GeneralSettingsMgr.ShowSetting("Bdr Dash Patt", bs.BdrDashPattern.ToStringFa());
		}

		private void showTextSettings(TextSettings ts)
		{
			GeneralSettingsMgr.ShowSetting("FontFamily",ts.FontFamily.ToString());
			GeneralSettingsMgr.ShowSetting("InfoText",ts.InfoText);
			GeneralSettingsMgr.ShowSetting("UrlLink",ts.UrlLink);
			GeneralSettingsMgr.ShowSetting("Font Size",ts.FontStyle.ToString());
			GeneralSettingsMgr.ShowSetting("Text Size",ts.TextSize.ToString("F2"));
			GeneralSettingsMgr.ShowSetting("Text Vert Align",ts.TextVertAlignment.ToString());
			GeneralSettingsMgr.ShowSetting("Text Horiz Align",ts.TextHorizAlignment.ToString());
			GeneralSettingsMgr.ShowSetting("Text Weight",ts.TextWeight.ToString());
			GeneralSettingsMgr.ShowSetting("Text Color", ts.TextColorA.ToStringFa());
			GeneralSettingsMgr.ShowSetting("Text Opacity",ts.TextOpacity.ToString("F2"));
			GeneralSettingsMgr.ShowSetting("Text Decoration",ts.TextDecoration.ToString());
		}


		public override string ToString()
		{
			return $"this is {nameof(SheetMetricDataMgr)}";
		}
	}
}
