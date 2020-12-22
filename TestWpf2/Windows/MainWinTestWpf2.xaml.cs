#region using

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using TestWpf2.Adjust;
using TestWpf2.Data;

#endregion

// projname: TestWpf2
// itemname: MainWindow
// username: jeffs
// created:  11/29/2020 11:58:44 AM

namespace TestWpf2.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWinTestWpf2 : Window, INotifyPropertyChanged
	{
	#region private fields

		public static MainWinTestWpf2 me;

		private int testCount;

		private static string tbxMessage = null;

		private double pb1Value;
		private double pb1MaxValue;
		private Progress<double> pb1Double;

		private double pb2Value;
		private double pb2MaxValue;
		private IProgress<double> pb2Double;

		private double pb2BrkPt1;
		private double pb2BrkPt2;
		private double pb2BrkPt3;

		private int pb2BrushIdx;

		private SolidColorBrush[] pb2FgBrushes = new []{Brushes.SpringGreen, 
			Brushes.Yellow, Brushes.DarkOrange, Brushes.Red};

		private bool pauseTask;

		// private string[] pauseStateMsg = new [] {"Waiting", "Pause", "Resume"};
		// private PauseState pauseState;
		// private Task task;
		// private CancellationTokenSource cancelToken;

		private AdjustMergeItems adj;

	#endregion

	#region ctor

		static MainWinTestWpf2()
		{
			Sd = new SampleData();
		}

		public MainWinTestWpf2()
		{
			InitializeComponent();

			me = this;

			adj = new AdjustMergeItems();
			OnPropertyChange("Adjust");

			adj.UpdateTaskStatus2(PauseState.WAITING);

			Pb1Value = 0;
			Pb1MaxValue = 100;
			pb1Double = new Progress<double>(value => Pb1Value = value);
			// pb1Double = new Progress<double>(value => Pb1.Value = value);

			Pb2Value = 0;
			Pb2MaxValue = 100;
			pb2Double = new Progress<double>(value => Pb2.Value = value);

			// PauseStateValue = PauseState.WAITING;

			// UpdateProcessStatus();
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

		public static SampleData Sd { get; set; } // = new SampleData();

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

		public double Pb2Value
		{
			get => pb2Value;
			set
			{
				pb2Value = value;
				OnPropertyChange();

				setPb2FgColor(value);
			}
		}

		public double Pb2MaxValue
		{
			get => pb2MaxValue;
			set
			{
				pb2MaxValue = value;
				OnPropertyChange();

				pb2BrkPt1 = value * 0.60;
				pb2BrkPt2 = value * 0.40;
				pb2BrkPt3 = value * 0.20;
			}
		}

		public SolidColorBrush Pb2ForeGround => pb2FgBrushes[pb2BrushIdx];

		public AdjustMergeItems Adjust => adj;


		// public TaskStatus? ProcessStatus => adj?.ProcessStatus ?? null;
		//
		// public string PauseStateMessage => adj?.PauseStateMessage ?? null;
		//
		// public PauseState PauseStateValue2 => adj?.PauseStateValue ?? PauseState.WAITING;
		//
		// public PauseState PauseStateValue
		// {
		// 	get => pauseState;
		// 	set
		// 	{
		// 		pauseState = value;
		//
		// 		OnPropertyChange();
		// 	}
		//
		// }

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

		// public void UpdateProcessStatus()
		// {
		// 	OnPropertyChange("ProcessStatus");
		// }

	#endregion

	#region private methods

		private void setPb2FgColor(double value)
		{
			int newIdx;

			if (value > pb2BrkPt1)
			{
				newIdx = 0;
			}
			else if (value > pb2BrkPt2)
			{
				newIdx = 1;
			} 
			else if (value > pb2BrkPt3)
			{
				newIdx = 2;
			}
			else
			{
				newIdx = 3;
			}

			if (newIdx == pb2BrushIdx) return;

			pb2BrushIdx = newIdx;

			OnPropertyChange("Pb2ForeGround");
		}

		private void clear()
		{
			// PauseStateValue = PauseState.WAITING;

			Pb1Value = 0;
			Pb1MaxValue = 100;

			Pb2Value = 0;
			Pb2MaxValue = 100;

			SetMsg("");

			Sd.TreeRoot.ClrMergeItems();
			updateCounts();

			// task.Dispose();
			// task = null;

			// UpdateProcessStatus();
		}

		private void updateCounts()
		{
			Sd.TreeRoot.exCount();
			Sd.TreeRoot.exMergeCount();

			OnPropertyChange("ExtendedCount");
			OnPropertyChange("ExtendedMergeCount");
		}

		private async void testPb1Async()
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

		private void modify()
		{
		#if DEBUG
			SetMsg("Process started\n");

			AppendMsgLine("thread main id| " +
				Thread.CurrentThread.ManagedThreadId);
			AppendMsgLine("thread main state| " +
				Thread.CurrentThread.ThreadState.ToString());
		#endif

			TestCount = adj.Configure(Sd, pb1Double, pb2Double);

			Pb1Value = 0;
			Pb1MaxValue = testCount;

			adj.Process2();

			AppendMsgLine("Test complete");
			AppendMsgLine("thread main state| " +
				Thread.CurrentThread.ThreadState.ToString());
		}

		// private async void modifyAsync()
		// {
		// 	TestCount = adj.Configure(Sd, pb1Double, pb2Double);
		//
		// 	TbxMessage = "Pre-process started\n";
		//
		// 	adj.PreProcess2(Sd.TreeRoot);
		//
		// 	TbxMessage = "Process started\n";
		// 	AppendMsgLine("thread main id| " +
		// 		Thread.CurrentThread.ManagedThreadId);
		// 	AppendMsgLine("thread main state| " +
		// 		Thread.CurrentThread.ThreadState.ToString());
		//
		// 	Pb1Value = 0;
		// 	Pb1MaxValue = testCount;
		//
		// 	cancelToken = new CancellationTokenSource();
		//
		// 	CancellationToken ct = cancelToken.Token;
		//
		// 	task = Task.Run(() => { adj.Process(ct); }, ct);
		//
		// 	// PauseStateValue = PauseState.RUNNING;
		//
		// 	// UpdateProcessStatus();
		//
		// 	await task;
		//
		// 	taskComplete();
		//
		// 	// UpdateProcessStatus();
		//
		// 	// PauseStateValue = PauseState.WAITING;
		//
		// 	TbxMessage = "Test complete\n";
		// 	AppendMsgLine("thread main state| " +
		// 		Thread.CurrentThread.ThreadState.ToString());
		// }

		// private void taskComplete()
		// {
		// 	TbxMessage = "task completed\n";
		//
		// 	if (cancelToken.IsCancellationRequested)
		// 	{
		// 		AppendMsgLine("task complete| task cancel requested");
		// 	}
		// 	else
		// 	{
		// 		updateCounts();
		// 	}
		// }

		private void updatePb2(bool? pause)
		{
			if (pause == null) return;

			if (!pause.Value) Pb2Value = 0;
		}

		private void pausePb()
		{
			if (!pauseTask)
			{
				pauseTask = true;

				Task.Run(() => { pauseTimer(); } );
			}
			else
			{
				pauseTask = false;
			}

			updatePb2(adj.PauseTask2(pauseTask));
		}

		private void pauseTimer()
		{
			double d = pb2MaxValue;

			while (pauseTask)
			{
				pb2Double.Report(d);

				Thread.Sleep(50);

				if (--d < 0)
				{
					pausePb();

				}

			}
		}


	#endregion

	#region event consuming

		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			TbxMessage = "btn test started\n";

			// modifyAsync();
			modify();

			TbxMessage = "btn test complete\n";
		}

		private void BtnCancelTask_OnClick(object sender, RoutedEventArgs e)
		{
			// cancelToken.Cancel();
			adj.CancelTask();
		}

		private void BtnPauseResumeTask_OnClick(object sender, RoutedEventArgs e)
		{
			pausePb();
		}

		private void BtnTestProgBar_OnClick(object sender, RoutedEventArgs e)
		{
			testPb1Async();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			SampleData sd = Sd;

			AdjustMergeItems ami = adj;

			Debug.WriteLine("@debug");
		}

		private void BtnClear_OnClick(object sender, RoutedEventArgs e)
		{
			clear();
		}

		private void BtnCount_OnClick(object sender, RoutedEventArgs e)
		{
			updateCounts();
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