#region using

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TestWpf1.Adjust;
using TestWpf1.Data;

#endregion

// projname: TestWpf1
// itemname: MainWindow
// username: jeffs
// created:  11/29/2020 11:58:44 AM

namespace TestWpf1.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWinTestWpf1 : Window, INotifyPropertyChanged
	{
	#region private fields

		private int testCount;

		private static string tbxMessage = null;
		public static MainWinTestWpf1 me;

		private double pb1Value;
		private double pb1MaxValue;

		private Progress<double> pb1Double;

		public delegate void pbxTest();

	#endregion

	#region ctor

		public MainWinTestWpf1()
		{
			InitializeComponent();

			me = this;

			// pb1Double = new Progress<double>(value => Pb1Value = value);
			pb1Double = new Progress<double>(value => Pb1.Value = value);

			Pb1Value = 0;
			Pb1MaxValue = 100;
		}

	#endregion

	#region public properties

		public string TbxMessage
		{
			get => tbxMessage;
			set
			{
				tbxMessage += value;
				OnPropertyChange();
			}
		}

		public int TestCount
		{
			get => testCount;
			set
			{
				testCount = value;
				OnPropertyChange();
			}
		}

		public static SampleData Sd { get; set; } = new SampleData();

		public int ExtendedCount => Sd?.TreeRoot.ExtCount ?? 0;

		public int ExtendedMergeCount => Sd?.TreeRoot.ExtMergeCount ?? 0;

		public double Pb1Value
		{
			get => pb1Value;
			set
			{
				pb1Value = value;
				OnPropertyChange();
			}
		}

		public double Pb1MaxValue
		{
			get => pb1MaxValue;
			set
			{
				pb1MaxValue = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void SetMsg(string msg)
		{
			tbxMessage = msg;
			OnPropertyChange("TbxMessage");
		}

		public void AppendMsg(string msg)
		{
			tbxMessage += msg;
			OnPropertyChange("TbxMessage");
		}

		public void AppendMsgLine(string msg)
		{
			tbxMessage += msg + "\n";
			OnPropertyChange("TbxMessage");
		}

	#endregion

	#region private methods

		private void UpdateCounts()
		{
			Sd.TreeRoot.exCount();
			Sd.TreeRoot.exMergeCount();

			OnPropertyChange("ExtendedCount");
			OnPropertyChange("ExtendedMergeCount");
		}

		private async void testPb1()
		{
			Pb1Value = 0;
			Pb1MaxValue = 100;

			await Task.Run(
				() => { tstPb(pb1Double); }
				);
		}

		private void tstPb(IProgress<double> progress)
		{
			for (double i = Pb1Value; i < Pb1MaxValue; i++)
			{
				Thread.Sleep(50);
				progress.Report(i + 1);
			}
		}

		private void testPb2()
		{
			AdjustMergeItems adj = new AdjustMergeItems();

			Pb1Value = 0;
			Pb1MaxValue = 100;

			adj.testPbUpdate(pb1Double, Pb1MaxValue);
		}

	#endregion

	#region event consuming

		private void BtnTestProgBar_OnClick(object sender, RoutedEventArgs e)
		{
			testPb1();

			// testPb2();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			SampleData sd = Sd;

			Debug.WriteLine("@debug");
		}


		private void BtnClear_OnClick(object sender, RoutedEventArgs e)
		{
			Pb1Value = 0;
			Pb1MaxValue = 100;

			SetMsg("");

			Sd.TreeRoot.ClrMergeItems();
			UpdateCounts();
		}

		private void BtnCount_OnClick(object sender, RoutedEventArgs e)
		{
			UpdateCounts();
		}

		private async void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			AdjustMergeItems adj = new AdjustMergeItems();

			TestCount = adj.Configure(Sd, pb1Double);

			Pb1Value = 0;
			Pb1MaxValue = testCount;

			MainWinTestWpf1.me.TbxMessage = "Pre-process started\n";

			adj.preProcess(Sd.TreeRoot);

			MainWinTestWpf1.me.TbxMessage = "Process started\n";
			MainWinTestWpf1.me.AppendMsgLine("thread id| " +
				Thread.CurrentThread.ManagedThreadId);
			MainWinTestWpf1.me.AppendMsgLine("thread main| " + 
				Thread.CurrentThread.ThreadState.ToString());
			
			await Task.Run(() => { adj.Process(Thread.CurrentThread); } );
			// adj.Process();

			MainWinTestWpf1.me.TbxMessage = "Test complete\n";
			MainWinTestWpf1.me.AppendMsgLine("thread main| " + 
				Thread.CurrentThread.ThreadState.ToString());

			UpdateCounts();
		}

	#endregion

	#region event publishing

		public static event PropertyChangedEventHandler PropertyChangedStatic;

		private static void OnPropertyChangeStatic([CallerMemberName] string memberName = "")
		{
			PropertyChangedStatic?.Invoke(me, new PropertyChangedEventArgs(memberName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
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
	}
}