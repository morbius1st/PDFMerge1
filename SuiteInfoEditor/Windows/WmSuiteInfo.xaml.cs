#region using

	using System;
	using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using AndyShared.ConfigMgrShared;
using JetBrains.Annotations;
using Settings;
using SettingsManager;
using SuiteInfoEditor.Support;
using UtilityLibrary;

#endregion

// projname: SuiteInfoEditor
// itemname: MainWindow
// username: jeffs
// created:  11/28/2024 1:00:00 PM

namespace SuiteInfoEditor.Windows
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class WmSuiteInfo : Window, INotifyPropertyChanged // , ITblkFmtSupport //, IWin
	{

	#region fields

		private static InlineCollection tbIlc;

		private string messages;
		private FlowDocument fd;

		private static CsFlowDocManager fdMgr;

		private static WmSuiteInfo instance;

	#endregion


	#region ctor

		public WmSuiteInfo()
		{
			InitializeComponent();
			//
			// TbkF = new CsTextBlockFmtgSupport([FdMsg]);
			//
			// op1 = new Operations1(this.TbkF);
			// Identifiers.Instance.Init(this.TbkF);

			Config();

		}

		private void Config()
		{
			fdMgr = CsFlowDocManager.Instance;
		}

	#endregion

	#region public properties

		public static WmSuiteInfo Instance => instance ??= new WmSuiteInfo();

		public string Messages
		{
			get => messages;
			set
			{
				if (value == messages) return;
				messages = value;
				OnPropertyChanged();
			}
		}

		public InlineCollection TbIlc
		{
			get => tbIlc;
			// set
			// {
			// 	if (value == tbIlc) return;
			// 	tbIlc = value;
			// 	OnPropertyChanged();
			// }
		}

		public void UpdateIlc(Inline il)
		{
			tbIlc.Add(il);
			OnPropertyChanged(nameof(TbIlc));
		}

		public static void Test(Inline il)
		{
			
		}

		// public IFdFmt TbkF { get; set; }

		public FlowDocument FD
		{
			get => fd;
			set
			{
				// if (Equals(value, fd)) return;
				// fd = value;
				OnPropertyChanged();
			}
		}

	#endregion

		#region private properties

		#endregion

		#region public methods

		public void DebugMsgLine(string msg)
		{
			Messages += msg + "\n";
		}
		public void DebugMsg(string msg)
		{
			Messages += msg;
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		//
		// public static event PropertyChangedEventHandler PropertyChangedS;
		//
		// [DebuggerStepThrough]
		// [NotifyPropertyChangedInvocator]
		// private static void OnPropertyChanged([CallerMemberName] string memberName = "")
		// {
		// 	PropertyChangedS?.Invoke(, new PropertyChangedEventArgs(memberName));
		// }


	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MainWindow";
		}

	#endregion


		private void WmSuiteInfo_OnLoaded(object sender, RoutedEventArgs e)
		{
			fdMgr.Register(FdSv01, FdVn.FIRST_FDSV);
			fdMgr.Register(Tblk01, SvTblk01, TbVn.FIRST_TBLK);

			tbIlc = Tblk01.Inlines;
		}



		private void BtnShowFlowDoc_OnClick(object sender, RoutedEventArgs e)
		{
			fdMgr.ShowInfoFd();
		}

		private void BtnShowTbIls_OnClick(object sender, RoutedEventArgs e)
		{
			fdMgr.ShowInfoTb();
		}

		private void BtnLocations1_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsManager.FileLocationShowInfo.ShowLocations();
		}

		private void BtnLocations2_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsManager.FileLocationShowInfo.ShowLocations2();
		}

		private void BtnCompConsts_OnClick(object sender, RoutedEventArgs e)
		{
			Identifiers.Instance.showShtNumComponentConstants1();
		}
		
		private void BtnCompConsts2_OnClick(object sender, RoutedEventArgs e)
		{
			Identifiers.Instance.showShtNumComponentConstants2();
		}
		
		private void BtnCompNameInfo1_OnClick(object sender, RoutedEventArgs e)
		{
			Identifiers.Instance.ShowCompNameInfo1();
		}

		private void BtnSheetNumComponentData_OnClick(object sender, RoutedEventArgs e)
		{
			Identifiers.Instance.ShowSheetNumComponentData();
		}

		private void BtnClrFd_OnClick(object sender, RoutedEventArgs e)
		{
			fdMgr.Clear(FdVn.FIRST_FDSV);
			
		}

		private void BtnClrTb_OnClick(object sender, RoutedEventArgs e)
		{
			fdMgr.Clear(TbVn.FIRST_TBLK);
		}

		private void BtnCopyFd_OnClick(object sender, RoutedEventArgs e)
		{
			TextRange tr = new TextRange(FdMsg.ContentStart, FdMsg.ContentEnd);

			Clipboard.SetDataObject(tr.Text);
		}

		private void BtnCopyTb_OnClick(object sender, RoutedEventArgs e)
		{
			Clipboard.SetDataObject(Tblk01.Text);
		}

		private void BtnCommonSettings_OnClick(object sender, RoutedEventArgs e)
		{
			GeneralSettingsMgr gsMgr = new GeneralSettingsMgr();

			UserSettingsMgr usMgr = new UserSettingsMgr();

			gsMgr.ShowCommonSettings();
		}

		private void BtnUserSettings_OnClick(object sender, RoutedEventArgs e)
		{
			GeneralSettingsMgr gsMgr = new GeneralSettingsMgr();

			UserSettingsMgr usMgr = new UserSettingsMgr();

			usMgr.ShowUserSettings();
		}

		private void BtnSheetMetricsStatus_OnClick(object sender, RoutedEventArgs e)
		{
			SheetMetricDataMgr mxMgr = new SheetMetricDataMgr();

			mxMgr.ShowZeroShtMetxFileStat();

		}

		private void BtnSheetMetricsStatusAll_OnClick(object sender, RoutedEventArgs e)
		{
			SheetMetricDataMgr mxMgr = new SheetMetricDataMgr();

			mxMgr.ShowAllShtMetxFileStat();
		}

		private void BtnClassifFiles_OnClick(object sender, RoutedEventArgs e)
		{
			ClassifFilesMgr cfm = new ClassifFilesMgr();

			cfm.ShowFileInfo();
		}


		private void BtnClassifFilesSummary_OnClick(object sender, RoutedEventArgs e)
		{
			ClassifFilesMgr cfm = new ClassifFilesMgr();

			cfm.ShowFileInfoSummary();
		}
	}

	public class InlineConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is InlineCollection inlines) {
				return inlines;
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}