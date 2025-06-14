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
using _TemplateProject.Windows.Support;

namespace _TemplateProject.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, ITblkFmt
	{
	#region fields

		private const int STARTMSGWIDTH = 40;
		private const int STARTTTLWIDTH = STARTMSGWIDTH - 3;

		private ITblkSpanSupport Tblks = new CsTextBlockFormatting();

		private ProcessManager pm;

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			init();
		}

		private void init()
		{
			DM.init(3, this);
			DM.DbxSetIdx(0, 0);
			DM.DbxSetDefaultWhere(0, ShowWhere.DEBUG);

			MaxTblks = 2;
			MsgTblocks = new TextBlock[MaxTblks];

			MsgTblocks[0] = MsgBlk;
			MsgTblocks[1] = StatBlk;

			startMsg();

			DM.Start0();

			pm = new ProcessManager(this);

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

	#endregion

	#region public properties

		public string Messages { get; set; }

		public TextBlock[] MsgTblocks { get; set; }
		public int MaxTblks {get; set; }


	#endregion

	#region private properties

	#endregion

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
	}
}