using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UtilityLibrary;
using AAndyLibrary.Windows.Support;
using SharedWPF.ShSupport;
using static AAndyLibrary.Windows.Support.MwSupport;

namespace AAndyLibrary.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, ITblkFmtSupport  //, ITblkFmt
	{

	#region fields

		private MwProcessManager pm;

		private CsTextBlockFmtgSupport tblkf;
		private CsTextBlockFmtgSupport fdf;

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			
		}

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			init();
		}

		private void init()
		{
			tblkf = new CsTextBlockFmtgSupport([MsgBlk, StatBlk]);
			fdf = new CsTextBlockFmtgSupport([FdMsg]);

			fdf.Add(FdMsg, FdSv1, SHOW_WHICH.SW_FD0);

			fdf.SetCurrent(SHOW_WHICH.SW_FD0);

			DM.init(3, TbkF);
			DM.DbxSetIdx(0, 0);
			DM.DbxSetDefaultWhere(0, ShowWhere.DEBUG);

			startMsg();

			DM.Start0();

			// TbkF.TblkFmtdLine("<green>MainWindow initialized</green>");
			FdF.FdFmtdLine("<green>MainWindow initialized</green>");

			pm = new MwProcessManager(TbkF);

			// testRtb();

			DM.End0();

		}

		private void startMsg()
		{
			string startInfo;

			Debug.WriteLine("*".Repeat(STARTMSGWIDTH));
			Debug.WriteLine($"{"*** start AndyLib ",-STARTTTLWIDTH}***");

			startInfo = $"*** {DateTime.Now.ToString()}";

			Debug.WriteLine($"{startInfo}{" ".Repeat(STARTMSGWIDTH - startInfo.Length - 4)} ***");
			Debug.WriteLine("*".Repeat(STARTMSGWIDTH));
		}
		
		// private T? findControl<T>(string name, FrameworkElement parent) where T : Control
		// {
		// 	T? control = Template.FindName(name, parent) as T;
		//
		// 	return control;
		// }

	#endregion

	#region public properties

		public string Messages { get; set; }

		public IFdFmt TbkF
		{
			get => tblkf;
			set => tblkf = (CsTextBlockFmtgSupport) value;
		}

		public IFdFmt FdF
		{
			get => fdf;
			set => fdf = (CsTextBlockFmtgSupport) value;
		}

	#endregion

	#region private properties

	#endregion

		/*
	#region message methods

		public void TblkMsgClear(SHOW_WHICH which)
		{
			if (!IsShowWhichOk(which)) return;

			MsgTblocks[(int) which].Inlines.Clear();
		}

		public void TblkMsgLine(string msg, SHOW_WHICH which = SHOW_WHICH.SW_TBLK0)
		{
			if (!IsShowWhichOk(which)) return;

			TblkMsg3(new Run(msg + "\n"), which);
		}

		public void TblkMsg(string msg, SHOW_WHICH which = SHOW_WHICH.SW_TBLK0)
		{
			if (!IsShowWhichOk(which)) return;

			TblkMsg3(new Run(msg), which);
		}

		public void TblkFmtdLine(string msg, SHOW_WHICH which = SHOW_WHICH.SW_TBLK0)
		{
			if (!IsShowWhichOk(which)) return;

			TblkMsg3(Tblks.ProcessText(msg + "\n"), which = SHOW_WHICH.SW_TBLK0);
		}

		public void TblkFmtd(string msg, SHOW_WHICH which)
		{
			if (!IsShowWhichOk(which)) return;

			TblkMsg3(Tblks.ProcessText(msg), which);
		}

		private void TblkMsg3(Span span, SHOW_WHICH which)
		{
			if (!IsShowWhichOk(which)) return;

			MsgTblocks[(int) which].Inlines.Add(span);

			Tblks.Reset();
		}

		private void TblkMsg3(Run run, SHOW_WHICH which)
		{
			if (!IsShowWhichOk(which)) return;

			MsgTblocks[(int) which].Inlines.Add(run);

			Tblks.Reset();
		}

		public bool IsShowWhichOk(SHOW_WHICH which)
		{
			return (which >=0 && (int) which < MaxTblks);
		}

	#endregion
		*/

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(MainWindow)}";
		}

	#endregion

		private void Test1_OnClick(object sender, RoutedEventArgs e)
		{
			
		}

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnSettings_OnClick(object sender, RoutedEventArgs e)
		{
			pm.ShowSettings();
		}


	}
}