using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
using static _TemplateProject.Windows.Support.MwSupport;
using System.Windows.Interop;
using _TemplateProject.Menus;
using _TemplateProject.Menus.SbMain;
using JetBrains.Annotations;

namespace _TemplateProject.Windows
{


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window , INotifyPropertyChanged
	{

	#region fields

		private MwProcessManager pm;
		private string messages;
		// private InlineCollection ilc;

		private CsFlowDocManager fdMgr;

		private ScrollViewer tbSv;
		private ScrollViewer fdSv;

		#endregion

		#region ctor

		public MainWindow()
		{
			InitializeComponent();

			init();
		}

		private void init()
		{

			DM.init(3);
			DM.DbxSetIdx(0, 0);
			DM.DbxSetDefaultWhere(0, ShowWhere.DEBUG);

			DM.Start0();

			startMsg();

			fdMgr = CsFlowDocManager.Instance;

			pm = new MwProcessManager();

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

		public string Messages
		{
			get => "\x02";
			set
			{
				if (value == null || value.Length < 2) return;

				if (value[0] != '\x02')
				{
					Tblk1.Inlines.Clear();
				} 
				else
				{
					value = value.Substring(1); // remove the leading \x02
				}

				if (value.Length > 0) Tblk1.Inlines.Add(CsFlowDocManager.Instance.GetSpan(value));
			}
		}


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

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(MainWindow)}";
		}

	#endregion

		private void BtnTxBlkShow_OnClick(object sender, RoutedEventArgs e)
		{
			fdMgr.ShowTbInlines();
		}

		private void BtnTxBlkTest_OnClick(object sender, RoutedEventArgs e)
		{
			CsFlowDocUtilities util = new ();

			util.reportRowAndIndentTest();

		}

		private void BtnTxBlkAdd_OnClick(object sender, RoutedEventArgs e)
		{
			Messages += "<blue>this is a</blue> <lawngreen>second test of the system</lawngreen>\n";
		}

		private void BtnTxBlkAddNull_OnClick(object sender, RoutedEventArgs e)
		{
			Messages += null;
		}

		private void BtnTxBlkSet_OnClick(object sender, RoutedEventArgs e)
		{
			Messages = "<red>this is a</red> <dimgray>test of the system</dimgray>\n";
		}

		private void BtnMenuTest1_OnClick(object sender, RoutedEventArgs e)
		{
			fdMgr.GetMenuSelection(SbPdfPrimeManager.MENU_NAME, SbPdfPrimeManager.SbPdfPrime.MENU_NAME, OnMenuChoice);
		}

		private void OnMenuChoice(string menumgrname, string menuname, string choice)
		{
			fdMgr.AddTextLineFd($"got menu choice| {choice}");

		}

		private void BtnMenuStart_OnClick(object sender, RoutedEventArgs e)
		{
			SbMainMgr.Instance.Start();
		}

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			DM.Start0();

			fdMgr.Register(FdSv0, FdVn.FIRST_FDSV);
			fdMgr.Register(Tblk1, TblkSv1, TbVn.FIRST_TBLK);
			fdMgr.RegisterMenuManager(SbPdfPrimeManager.MENU_NAME, SbPdfPrimeManager.Instance);
			fdMgr.ConfigMenuMgr(SbPdfPrimeManager.MENU_NAME);

			// SbMainMgr.Instance.Config();

			tbSv = fdMgr.GetSv(TbVn.FIRST_TBLK);
			fdSv = fdMgr.GetSv(FdVn.FIRST_FDSV);

			DM.End0();

		}

		private void BtnSuiteSetg_OnClick(object sender, RoutedEventArgs e)
		{
			MwSettings mws = new MwSettings();
			mws.Owner = this;
			mws.ShowDialog();
		}
		private void BtnPtojSetg_OnClick(object sender, RoutedEventArgs e)
		{
			
		}

		private void BtnClrTblk_OnClick(object sender, RoutedEventArgs e)
		{
			fdMgr.Clear(TbVn.FIRST_TBLK);

		}
		private void BtnClrFd_OnClick(object sender, RoutedEventArgs e)
		{
			fdMgr.Clear(FdVn.FIRST_FDSV);
		}

		private void BtnRight_OnClick(object sender, RoutedEventArgs e)
		{
			string which = ((Button)sender).Tag?.ToString() ?? "tblk";

			if (which.Equals("tblk")) tbSv.ScrollToRightEnd();
			else fdSv.ScrollToRightEnd();
		}

		private void BtnDn_OnClick(object sender, RoutedEventArgs e)
		{
			string which = ((Button)sender).Tag?.ToString() ?? "tblk";

			if (which.Equals("tblk")) tbSv.ScrollToBottom();
			else fdSv.ScrollToBottom();
		}

		private void BtnUp_OnClick(object sender, RoutedEventArgs e)
		{
			string which = ((Button)sender).Tag?.ToString() ?? "tblk";

			if (which.Equals("tblk")) tbSv.ScrollToHome();
			else fdSv.ScrollToHome();
		}

		private void BtnLeft_OnClick(object sender, RoutedEventArgs e)
		{
			string which = ((Button)sender).Tag?.ToString() ?? "tblk";

			if (which.Equals("tblk")) tbSv.ScrollToLeftEnd();
			else fdSv.ScrollToLeftEnd();
		}

		private void BtnTestScroll_OnClick(object sender, RoutedEventArgs e)
		{
			fdMgr.Clear(FdVn.FIRST_FDSV);
			fdMgr.Clear(TbVn.FIRST_TBLK);

			CsFlowDocUtilities util = new ();

			util.scrollViewerTest();
		}
	}
}