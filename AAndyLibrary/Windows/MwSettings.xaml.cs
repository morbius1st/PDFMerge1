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

using static AAndyLibrary.Windows.Support.MwSupport;

namespace AAndyLibrary.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MwSettings : Window, ITblkFmtSupport
	{
	#region fields

		private IFdFmt mwTw;

		private MwSetgProcessManager pm;

	#endregion

	#region ctor

		public MwSettings(IFdFmt mwTw)
		{
			InitializeComponent();

			this.mwTw = mwTw;

			init();
		}

		private void init()
		{
			TbkF = new CsTextBlockFmtgSupport([MsgBlk, StatBlk]);
			pm = new MwSetgProcessManager(TbkF);

			DM.init(3, this.TbkF);
			DM.DbxSetIdx(0, 0);
			DM.DbxSetDefaultWhere(0, ShowWhere.DEBUG);

			startMsg();

			DM.Start0();

			mwTw.TblkFmtdLine("<green>MainWindow initialized</green>");

			TbkF.TblkFmtdLine("<green>MainWindow initialized</green>");
			TbkF.TblkFmtdLine("<green>MainWindow initialized</green>", SHOW_WHICH.SW_TBLK1);

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

		public IFdFmt TbkF { get; set; }

		// public TextBlock[] MsgTblocks { get; set; }
		// public int MaxTblks {get; set; }


	#endregion

	#region private properties

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