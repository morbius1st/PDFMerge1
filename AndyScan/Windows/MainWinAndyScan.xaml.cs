using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using AndyScan.SbLocal;
using AndyScan.SbSystem;
using AndyScan.Support;
using JetBrains.Annotations;
using SettingsManager;
using UtilityLibrary;
using Clipboard = System.Windows.Clipboard;
using TextDataFormat = System.Windows.TextDataFormat;


// projname: AndyScan
// itemname: MainWindow
// username: jeffs
// created:  11/23/2024 3:58:37 PM

namespace AndyScan.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWinAndyScan : Window, INotifyPropertyChanged, IInputWin, IWinMain, IFdFmt //, IWin //, IInput
	{
	#region private fields

		private SwitchBoardManager sbm;

		// private int processKeyIdx;

		private string messages;
		private string input;

		private string keyUp;


		private ITblkSpanSupport Tblks = new CsTextBlockFormatting();
		private string onKey;

		private bool result;

		private List<Paragraph> pgs;

	#endregion

	#region ctor

		public MainWinAndyScan()
		{
			InitializeComponent();

			init();

			UserSettingDataFile dx;
		}

		private void init()
		{
			DM.init(3, this);
			DM.DbxSetIdx(0, 0);
			DM.DbxSetDefaultWhere(0, ShowWhere.DEBUG);

			MaxTblks = 2;
			MsgTblocks = new TextBlock[MaxTblks];

			MsgTblocks[0] = MsgBlk;
			MsgTblocks[1] = MsgBlk0;

			DM.Start0(true);

			sbm = new SwitchBoardManager(this, this, this);

			DM.End0();
		}

	#endregion

	#region public properties

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

		public TextBlock[] MsgTblocks { get; set; }
		public FlowDocument[] MsgFdocs { get; set; }
		public List<FlowDocData> FlowDocs { get; set; }
		public int MaxTblks { get; set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public int ProcessMsg(int idx)
		{
			DM.Start0();

			int result = -1;

			if (idx == -100)
			{
				TblkMsgClear();
				TblkMsgLine("waiting ... press a button to continue");

				result = -100;
			}

			DM.End0();

			return result;
		}

		public UIElement GetInputWindow()
		{
			return MsgBlk;
		}

	#endregion

	#region private methods

		private void processMainSb()
		{
			DM.Start0();
			TblkMsgLine("selecting options");

			sbm.ProcessSb(new SbMain(this, this));
			// sbm.ProcessSb();

			DM.End0();
		}

		private void closeApp()
		{
			this.Close();
		}


		private const int START_MSG_WIDTH = 26;

		private void startMsg()
		{
			string msg;
			int width;

			Debug.WriteLine("*".Repeat(START_MSG_WIDTH));

			msg = $"*** starting - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
			width = START_MSG_WIDTH - msg.Length - 4;

			Debug.WriteLine($"{msg}{" ".Repeat(width)} ***");

			Debug.WriteLine("*".Repeat(START_MSG_WIDTH));
			Debug.WriteLine("\n\n");
		}

	#endregion

	#region message methods (textblock)

		public void TblkMsgClear(SHOW_WHICH which = SHOW_WHICH.SW_TBLK0)
		{
			MsgTblocks[(int) which].Inlines.Clear();
			// MsgBlk.Inlines.Clear();
		}

		public void TblkMsgLine(string msg, SHOW_WHICH which = SHOW_WHICH.SW_TBLK0)
		{
			// Debug.WriteLine(msg);

			TblkMsg3(new Run(msg + "\n"), which);
		}

		public void TblkMsg(string msg, SHOW_WHICH which = SHOW_WHICH.SW_TBLK0)
		{
			// Debug.WriteLine(msg);

			TblkMsg3(new Run(msg), which);
		}

		public void TblkFmtdLine(string msg, SHOW_WHICH which = SHOW_WHICH.SW_TBLK0)
		{
			// Debug.WriteLine(msg);

			TblkMsg3(Tblks.ProcessText(msg + "\n"), which);
		}

		public void TblkFmtd(string msg, SHOW_WHICH which = SHOW_WHICH.SW_TBLK0)
		{
			// Debug.WriteLine(msg);

			TblkMsg3(Tblks.ProcessText(msg), which);
		}

		private void TblkMsg3(Span span, SHOW_WHICH which)
		{
			MsgTblocks[(int) which].Inlines.Add(span);
			// MsgBlk.Inlines.Add(span);

			Tblks.Reset();
		}

		private void TblkMsg3(Run run, SHOW_WHICH which)
		{

			MsgTblocks[(int) which].Inlines.Add(run);
			// MsgBlk.Inlines.Add(run);

			Tblks.Reset();
		}

		public bool TblkIsOk(SHOW_WHICH which)
		{
			return (which >= 0 && (int) which < MaxTblks);
		}


		public void DebugMsgLine(string msg)
		{
			// TblkMsgLine(msg);
		}

		public void DebugMsg(string msg)
		{
			// TblkMsg(msg);
		}

	#endregion

	#region message methods (textblock)

		public void Add(FlowDocument fDoc, ScrollViewer sv)
		{
			FlowDocs.Add(new (fDoc, sv));
		}

		public void GotoTop(SHOW_WHICH which = SHOW_WHICH.SW_FD0)
		{
			if (CanScroll(which)) FlowDocs[(int) which].Sv.ScrollToTop();
		}

		public void GotoBottom(SHOW_WHICH which = SHOW_WHICH.SW_FD0)
		{
			if (CanScroll(which)) FlowDocs[(int) which].Sv.ScrollToBottom();
		}

		private bool CanScroll(SHOW_WHICH which)
		{
			return FlowDocs[(int) which].Sv != null && FlowDocs[(int) which].Sv.CanContentScroll;
		}

		public void FdMsgLine(string msg, SHOW_WHICH which = SHOW_WHICH.SW_FD0)
		{
			if (!IsOk(which)) return;

			FdMsg3(new Run(msg + "\n"), which);
		}

		public void  FdMsg(string msg, SHOW_WHICH which = SHOW_WHICH.SW_FD0)
		{
			if (!IsOk(which)) return;

			FdMsg3(new Run(msg), which);
		}

		public void  FdFmtdLine(string msg, SHOW_WHICH which = SHOW_WHICH.SW_FD0)
		{
			if (!IsOk(which)) return;

			FdMsg3(Tblks.ProcessText(msg + "\n"), which);
		}

		public void  FdFmtd(string msg, SHOW_WHICH which = SHOW_WHICH.SW_FD0)
		{
			if (!IsOk(which)) return;

			FdMsg3(Tblks.ProcessText(msg), which);
		}

		public void FdMsgClear(SHOW_WHICH which = SHOW_WHICH.SW_FD0)
		{
			if (!IsOk(which)) return;

			FlowDocs[(int) which].Fd.Blocks.Clear();
		}

		private void FdMsg3(Inline iline, SHOW_WHICH which)
		{
			pgs[(int) which].Inlines.Add(iline);
		}

		public bool IsOk(SHOW_WHICH which)
		{
			if (pgs.Count - 1 < (int) which) return false;

			bool result = pgs[(int) which] != null;

			return result;
		}

	#endregion

	#region event consuming

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

	#endregion

	#region event publishing

		public delegate void ProcessKeyUpEventHandler(object sender);

		public event MainWinAndyScan.ProcessKeyUpEventHandler ProcessKeyUp;

		protected virtual void RaiseProcessKeyUpEvent()
		{
			ProcessKeyUp?.Invoke(this);
		}

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
			return "this is MainWindow";
		}

	#endregion


		// control methods

		private void BtnSbTest1_OnClick(object sender, RoutedEventArgs e)
		{
			startMsg();

			DM.Start0(true);
			DM.Stat0("button pressed");

			// sb.Proceed();
			// sb.Test();
			// it.SelectOption();

			processMainSb();

			DM.Stat0("process SB return", true);

			DM.End0();
		}

		private void BtnTest1_OnClick(object sender, RoutedEventArgs e)
		{
			AndyScanSupport aSupport = new AndyScanSupport(this);

			aSupport.CreateSampleScanData();

		}

		private void BtnTest2_OnClick(object sender, RoutedEventArgs e)
		{
			AndyScanSupport aSupport = new AndyScanSupport(this);

			aSupport.ReadSampleScanData();

		}


		// private async void BtnTest1_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	// testTextBlockFormatting();
		//
		// 	// Messages += $"{("- this is horizontal text".Repeat(15))}\n";
		// 	//
		// 	// for (int i = 0; i < 50; i++)
		// 	// {
		// 	// 	Messages += "this is vertical text\n";
		// 	// }
		// }

		private void BtnCopy_OnClick(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(MsgBlk.Text, TextDataFormat.Text);
		}


		// private void testTextBlockFormatting()
		// {
		// 	Tblks.Reset();
		// 	TblkFmtdLine($"<linebreak/>");
		//
		// string option = "AA";
		// string title = "option AA";
		//
		// TblkFmtdLine($"<red>│</red>    <White><black>{option,-2}</black></White>   <red>│</red> <green>{title}</green>");

		// string test;
		// string testA1 = "A";
		// string testA2 = "option A";

		// TblkFmtdLine("1 <bold><red>A  </red></bold><margin spaces={6}/><bold>option A</bold>");
		// TblkFmtdLine("1 <White><red>A  </red></White><margin spaces={6}/><bold>option A</bold>");

		// TblkFmtdLine($"<White><black> {testA1,-3}</black><White><margin spaces={{6}}/><bold>{testA2}</bold>");
		// TblkFmtdLine($"<White><black> {testA1,-3}</black></White><margin spaces={{4}}/><bold>{testA2}</bold>");
		// TblkFmtdLine($"<White><black> {testA1,-3}<black></White><margin spaces={{6}}/><bold>{testA2}</bold>");

		// test =
		// 	"<margin spaces={10}/>text A text <bold>text B <textformat foreground = {0,0,0} ; background={255,255,255} ; weight={normal} ; size={16} > text C text <bold> text C1 text</bold> text C2</textformat> text D </bold> text E text <red>text F text </red>text Z";
		// // DebugMsgLine($"\n\ntesting = {test}");
		// TblkFmtdLine(test);
		//
		//
		// test = "text <red> red text</red> text text <blue> blue text</blue> text";
		// // DebugMsgLine($"\n\ntesting = {test}");
		// TblkFmtdLine(test);
		//
		// test = "<red>red text</red> text text <blue> blue text</blue> text";
		// // DebugMsgLine($"\n\ntesting = {test}");
		// TblkFmtdLine(test);

		// }
	}
}

// tests for textblock formatting
//
// private void testTextBlockFormatting()
// {
// 	Tblks.Reset();
//
// 	string test;
//
// 	string testA1 = "A";
// 	string testA2 = "option A";
//
// 	// FmtMsgLine("1 <bold><red>A  </red></bold><margin spaces={6}/><bold>option A</bold>");
// 	// FmtMsgLine("1 <White><red>A  </red></White><margin spaces={6}/><bold>option A</bold>");
//
// 	FmtMsgLine($"<White><black> {testA1,-3}</black><White><margin spaces={{6}}/><bold>{testA2}</bold>");
// 	FmtMsgLine($"<White><black> {testA1,-3}</black></White><margin spaces={{4}}/><bold>{testA2}</bold>");
// 	FmtMsgLine($"<White><black> {testA1,-3}<black></White><margin spaces={{6}}/><bold>{testA2}</bold>");
//
// 	// test =
// 	// 	"<margin spaces={10}/>text A text <bold>text B <textformat foreground = {0,0,0} ; background={255,255,255} ; weight={normal} ; size={16} > text C text <bold> text C1 text</bold> text C2</textformat> text D </bold> text E text <red>text F text </red>text Z";
// 	// // DebugMsgLine($"\n\ntesting = {test}");
// 	// FmtMsgLine(test);
// 	//
// 	//
// 	// test = "text <red> red text</red> text text <blue> blue text</blue> text";
// 	// // DebugMsgLine($"\n\ntesting = {test}");
// 	// FmtMsgLine(test);
// 	//
// 	// test = "<red>red text</red> text text <blue> blue text</blue> text";
// 	// // DebugMsgLine($"\n\ntesting = {test}");
// 	// FmtMsgLine(test);
//
// }

// test =
// 	"line combine test - part 2a (add NL)\nline combine test - part 2b (add NL)\nline combine test - part 2c (add NL)\n";
// DebugMsgLine($"\n\ntesting = {test}");
// FmtMsgLine(test);
// 			
// 			test =
// 				"line combine test - part 3a (add NL)\nline combine test - part 3b (no NL)";
// 			DebugMsg($"\n\ntesting = {test}");
// 			DebugFmt(test);
// 			
// 			test =
// 				"line combine test - part 3c (add NL)\nline combine test - part 3d (no NL)";
// 			DebugMsg($"\n\ntesting = {test}");
// 			DebugFmt(test);
// 			
// 			test =
// 				"line combine test - part 3e (add NL)\nline combine test - part 3f (add NL)";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
//
// 			test =
// 				"line break test<linebreak/>this tests if line breaks are added here<linebreak/>and one here<linebreak/> and, finally, at the very end<linebreak/>";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
//
//
// 			test =
// 				"line combine test - part 1a ";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmt(test);
//
// 			
// 			test =
// 				"line combine test - part 1b ";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmt(test);
//
// 			test =
// 				"line combine test - 1c last part - new line here";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
//
//
//
// /*			
// 			test =
// 				"<margin spaces={20}/>margin test - this tests if a margin added here<margin spaces={20}/> works suffix text here";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
// 			
// 			test =
// 				"margin test<margin spaces={20}/>this tests if margin works when in the middle";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
// 			
// 			test =
// 				"<margin spaces={20}/>margin test - this tests margin works as a prefix";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
// 			
// 			test =
// 				"<margin tabs={3}/>margin test - this tests if the tabs margin works";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
// 			
// 			test =
// 				"<margin tabs={3}/>|<margin spaces={20}/>margin test - this tests if the tabs margin and spaces margin can work together works";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
// 			
// 			test =
// 				"plain text test - there are no formatting codes - this checks that this too can pass";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
//
//
// 			test =
// 				"<margin spaces={10}/>text A text <bold>text B <textformat foreground = {0,0,0} ; background={255,255,255} ; weight={normal} ; size={16} > text C text <bold> text C1 text</bold> text C2</textformat> text D </bold> text E text <red>text F text </red>text Z";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
//
// 			
// 			test =
// 				"<margin spaces={10}/>text A text <bold>text B <textformat foreground = {0,0,0} ; background={255,255,255} ; weight={normal} ; size={16} > text C text is extra<linebreak test={0}/> long for testing<bold> text C1 text</bold> text C2</textformat> text D </bold> text E text <red>text F text </red>text Z";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
//
//
// 			test = "text <red> red text</red> text text <blue> blue text</blue> text";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
//
// 			test = "text <foreground color={0,255,0} > green text</foreground> white text? text <background color = {92, 92, 92}> dark grey background text</background> text";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
//
// 			test = "text <bold><Dimgray><red> red text on dim gray background text</red></Dimgray></bold> text text";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
//
//
// 			test = "text <underline> underline text</underline> text text <strikethrough> strikethrough text</strikethrough> text text <overline> overline text</overline> text";
// 			DebugMsgLine($"\n\ntesting = {test}");
// 			DebugFmtLine(test);
// */
//
//
//
//
// 			// testRegex();
// 			//
// 			// showTestRegex();
//
// 			// timedTestRegex();
// 		}
//
//
// 		// 59.2 mS
// 		private static string fmtCodePattern0 =  @"(?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.+)|(?>(?'AA'.*?)(?'S'<.*?/>)(?'ZZ'.+|$))|(?'ZZ'.+)";
//
// 		// 1094.7 mS
// 		private static string fmtCodePattern1 =  @"(?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.+)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.+)|(?>(?'AA'.*?)(?'S'<.*?/>)(?'ZZ'.+|$))|(?'ZZ'.+)";
// 		//                                                                               ^^
// 		private static string  difMarker1       = "                                      ^^";
// 		
// 		// this is the best
// 		// 27.6 mS
// 		private static string fmtCodePattern2 =  @"(?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.+)|(?>(?'AA'.*?)(?'S'<.*?/>)(?'ZZ'.*?|$))|(?'ZZ'.+)"; 
// 		//																																														 ^^
// 		private static string  difMarker2       = "                                                                                                                                              ^^";
// 		
// 		// 39.9 mS
// 		private static string fmtCodePattern3 =  @"(?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.+)(?'z1'</\k'C'>)))))(?'ZZ'.+)|(?>(?'AA'.*?)(?'S'<.*?/>)(?'ZZ'.+|$))|(?'ZZ'.+)";
// 		//                                                                                                                        ^^
// 		private static string  difMarker3       = "                                                                               ^^";
//
// 	//	private static string fmtCodePattern2 =  @"(?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.+)|(?>(?'AA'.*?)(?'S'<.*?/>)(?'ZZ'.*?|$))|(?'ZZ'.+)"; 
// 		private static string fmtCodePattern4 =  @"(?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.+)|^(?>(?'AA'.*?)(?'S'<.*?/>)(?'ZZ'.+|$))|(?'ZZ'.+)";
// 		//                                                                                                                                                       ^^                               ^^
// 		private static string  difMarker4       = "                                                                                                              ^^                               ^^";
//
//
// 		// // fails - do not use
// 		// private static string  difMarker4       = "                                                                               vv";
// 		// //                                                                                                                        vv
// 		// private static string fmtCodePattern4 =  @"(?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*?)(?'z1'</\k'C'>)))))(?'ZZ'.+)|(?>(?'AA'.*?)(?'S'<.*?/>)(?'ZZ'.+|$))|(?'ZZ'.+)";
//
// 		private static string[] patts = new [] { fmtCodePattern0, fmtCodePattern1, fmtCodePattern2, fmtCodePattern3, fmtCodePattern4 };
// 		private static string[] difs  = new [] { "", difMarker1, difMarker2, difMarker3, difMarker4 };
//
//
// 		private string[] testStrings = new []
// 		{
// 			// 0
// 			"<linebreak/>",
// 			// 1
// 			"text <linebreak/> text text <textformat foreground = {200,0,255,255} ; background={255,255,255} ; weight={normal} > text C <linebreak/> text </textformat> text D <linebreak/> text",
// 			// 2
// 			"text <textformat foreground = {200,0,255,255} ; background={255,255,255} ; weight={normal} > text C text </textformat> text D",
// 			// 3
// 			"text A text <bold>text B <textformat foreground = {200,0,255,255} ; background={255,255,255} ; notbold > text C text <bold> text C1 text</bold> text C2</textformat> text D </bold> text E text <red>text F text </red>text Z",
// 			
// // /*
// 			// 4
// 			"text B <textformat foreground = {200,0,255,255} ; background={255,255,255} ; notbold > text C text <bold> text C1 text</bold> text C2</textformat> text D ",
// 			"text text <linebreak/> text text <linebreak/> text text<red>text F text </red>text Z",
// 			"text text <linebreak/> text text <linebreak/> text text",
// 			"<linebreak/>text text <linebreak/> text text <linebreak/>",
// 			"text text <linebreak/> text",
// 			"</textformat> text D </bold> text E text <red>text F text </red>text Z",
// 			"text text <codeC> text </codeC> text text",
// 			"text text <codeC text = {p1 , p2} > text </codeC><codeC>text = {p1 , p2}</codeC> ",
// 			"<codeC text ={p1, p2}; text={p1}; text; > text </codeC> text text  text <codeC text={ p1 } > text </codeC> text",
// 			"text <codeC text={ p1 } > text </codeC> text <codeC text={ p1 } > text </codeC> text",
// 			"<codeC text = {p1, p2, p3} > text </codeC>",
// 			"   <codeC> text  = { p1 , p2,  p3 } > text    </codeC>  ",
// 			"this is text",
// 			"<codeC> text </codeD>",
// 			"<codeC></codeC>",
// //  */
// 		};

//
// 		private List<string> groupOrder = new List<string>()
// 		{
// 			"AA", "F", "a", "C", "A", "T", "z1", "ZZ", "S"
// 		};
//
//
// 		// array per pattern - first pattern, second pattern, etc.
// 		// tuple with original test string and
// 		//  a list of matches in a dictionary
// 		//    dictionary with string (match code & match string)
// 		private List<Tuple<string, Dictionary<string, string>>>[] patternResults =
// 			new List<Tuple<string, Dictionary<string, string>>>[patts.Length];
//
// 		private void timedTestRegex()
// 		{
// 			int count;
// 			int mcount;
// 			Regex testRegex;
// 			Match match;
// 			Stopwatch sw;
// 			Stopwatch msw;
//
// 			string test = testStrings[3];
//
//
// 			for (int i = 0; i < patts.Length; i++)
// 			{
// 				mcount = 0;
// 				Debug.WriteLine($"\n\ntesting pattern {i} | {patts[i]}");
// 				Debug.WriteLine(    $"pattern diff      | {difs[i]}");
//
// 				count = 0;
//
// 				testRegex = new Regex(patts[i]);
//
// 				sw = Stopwatch.StartNew();
// 				sw.Start();
//
// 				for (int j = 0; j < 1000; j++)
// 				{
// 					match = testRegex.Match(test);
//
// 					if (match.Success)
// 					{
// 						// Debug.Write(".");
// 						count++;
// 					}
// 				}
//
// 				sw.Stop();
//
// 				Debug.WriteLine($"loops = {count} | time (total) = {sw.Elapsed.TotalMicroseconds} mS  | time (per loop) = {sw.Elapsed.TotalMicroseconds/count} mS");
// 			}
// 			
// 		}
//
//
//
// 		private void testRegex()
// 		{
// 			Regex testRegex;
// 			Match match;
//
// 			// one per test string
// 			Tuple<string, Dictionary<string, string>> testInfo;
// 			
// 			// one per test string
// 			Dictionary<string, string> resultDict;
//
// 			// traverse each pattern
// 			for (int i = 0; i < patts.Length; i++)
// 			{
// 				testRegex = new Regex(patts[i]);
//
// 				patternResults[i] = new List<Tuple<string, Dictionary<string, string>>>();
//
// 				// traverse each test string
// 				for (int j = 0; j < testStrings.Length; j++)
// 				{
// 					match = testRegex.Match(testStrings[j]);
// 					
// 					resultDict = parseMatches(match);
// 					testInfo = new Tuple<string, Dictionary<string, string>>(testStrings[j], resultDict);
//
// 					patternResults[i].Add(testInfo);
// 				}
// 			}
// 		}
//
// 		private Dictionary<string, string> parseMatches(Match match)
// 		{
// 			Dictionary<string, string> matches = new Dictionary<string, string>();
// 			Group grp;
// 			string answer;
//
// 			for (int i = 0; i < groupOrder.Count; i++)
// 			{
// 				answer = "";
// 				grp = match.Groups[groupOrder[i]];
//
// 				if (grp.Captures.Count != 0)
// 				{
// 					answer = grp.Captures[0].Value;
// 				}
//
// 				matches.Add(groupOrder[i], answer);
// 			}
//
// 			return matches;
// 		}
//
// 		private void showTestRegex()
// 		{
// 			bool result;
// 			bool finalresult;
//
// 			string answer1;
// 			string answer2;
//
// 			// process the array of results for each pattern
// 			for (var i = 1; i < patternResults.Length; i++)
// 			{
// 				finalresult = true;
//
// 				List<Tuple<string, Dictionary<string, string>>> p = patternResults[i];
//
// 				if (p == null || p.Count == 0) continue;
//
// 				Debug.WriteLine($"\n\n control pattern | {patts[0]}");
// 				Debug.WriteLine(    $"       patt diff | {difs[i]}");
// 				Debug.WriteLine(    $"vs. test pattern | {patts[i]}\n");
//
//
// 				Dictionary<string, string> dict1 = null;
// 				Dictionary<string, string> dict2 = null;
//
// 				// process the list for each test string
// 				for (int j = 0; j < p.Count; j++)
// 				{
// 					Debug.WriteLine($"\ntest string | {p[j].Item1}");
//
// 					dict2 = p[j].Item2;
// 					dict1 = patternResults[0][j].Item2;
//
// 					Debug.Write("\t");
//
// 					int idx = 0;
//
// 					result = true;
//
// 					foreach (string g in groupOrder)
// 					{
// 						answer1 = dict1.ContainsKey(g) ? dict1[g] : "";
// 						answer2 = dict2.ContainsKey(g) ? dict2[g] : "";
//
// 						int c = answer1.CompareTo(answer2);
//
// 						// Debug.Write($"\tfor group {g,-3} | match? {c,-2} | {(c==0 ? "YES" : "NO")}");
// 						// Debug.Write($"grp {g,-3} {(c==0 ? "(yes)" : "(no)")} ");
//
// 						if (!answer1.IsVoid() && !answer2.IsVoid() )
// 						{
// 							if (c != 0)
// 							{
// 								result = false;
// 								finalresult = false;
//
// 								Debug.Write("\n");
// 								Debug.WriteLine($"grp {g,-3} ** FAILED ** ");
// 								Debug.WriteLine($"\t\tcontrol value | >[{answer1}]<");
// 								Debug.WriteLine($"\t\t   test value | >[{answer2}]<");
// 								
// 							}
// 						} 
// 						// else
// 						// {
// 						// 	Debug.Write(" [both are void] ");
// 						// 	
// 						// }
//
// 						// if (++idx != groupOrder.Count) Debug.Write(" | ");
// 						
// 					}
//
// 					
// 					Debug.WriteLine($"\tresult {(result ? "all GOOD" : "some BAD")}");
// 					
// 					// Debug.Write("\n");
// 				}
//
// 				Debug.WriteLine($"\nfinal result {(finalresult ? "all GOOD" : "some BAD")}");
// 			}
// 		}