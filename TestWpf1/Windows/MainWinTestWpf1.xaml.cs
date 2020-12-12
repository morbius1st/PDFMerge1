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
	
	public enum PauseState
	{
		WAITING,
		RUNNING,
		PAUSED
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWinTestWpf1 : Window, INotifyPropertyChanged
	{
	#region private fields

		private string[] pauseStateMsg = new [] {"Waiting", "Pause", "Resume"};

		private PauseState pauseState;

		private int testCount;

		private static string tbxMessage = null;
		public static MainWinTestWpf1 me;

		private double pb1Value;
		private double pb1MaxValue;
		private Progress<double> pb1Double;
		
		private double pb2Value;
		private double pb2MaxValue;
		private Progress<double> pb2Double;

		private Task task;

		private CancellationTokenSource cancelToken;

		private AdjustMergeItems adj;

	#endregion

	#region ctor

		public MainWinTestWpf1()
		{
			InitializeComponent();

			me = this;

			adj = new AdjustMergeItems();

			Pb1Value = 0;
			Pb1MaxValue = 100;
			pb1Double = new Progress<double>(value => Pb1.Value = value);

			Pb2Value = 0;
			Pb2MaxValue = 100;
			pb2Double = new Progress<double>(value => Pb2.Value = value);

			PauseStateValue = PauseState.WAITING;

			UpdateProcessStatus();
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

		public double Pb2Value
		{
			get => pb2Value;
			set
			{
				pb2Value = value;
				OnPropertyChange();
			}
		}

		public double Pb2MaxValue
		{
			get => pb2MaxValue;
			set
			{
				pb2MaxValue = value;
				OnPropertyChange();
			}
		}

		public TaskStatus? ProcessStatus => task?.Status ?? null;

		public string PauseStateMessage => pauseStateMsg[(int) pauseState];

		public PauseState PauseStateValue
		{
			get => pauseState;
			set
			{
				if (value == pauseState) return;
				if (value < PauseState.WAITING || value > PauseState.PAUSED) return;

				pauseState = value;

				OnPropertyChange();
				OnPropertyChange("PauseStateMessage");
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

		public void UpdateProcessStatus()
		{
			OnPropertyChange("ProcessStatus");
		}

	#endregion

	#region private methods

		private void clear()
		{
			PauseStateValue = PauseState.WAITING;

			Pb1Value = 0;
			Pb1MaxValue = 100;
			
			Pb2Value = 0;
			Pb2MaxValue = 100;

			SetMsg("");

			Sd.TreeRoot.ClrMergeItems();
			updateCounts();

			task.Dispose();
			task = null;

			UpdateProcessStatus();
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

		private async void modifyAsync()
		{
			TestCount = adj.Configure(Sd, pb1Double, pb2Double);

			TbxMessage = "Pre-process started\n";

			adj.PreProcess(Sd.TreeRoot);

			TbxMessage = "Process started\n";
			AppendMsgLine("thread main id| " +
				Thread.CurrentThread.ManagedThreadId);
			AppendMsgLine("thread main state| " +
				Thread.CurrentThread.ThreadState.ToString());

			Pb1Value = 0;
			Pb1MaxValue = testCount;

			cancelToken = new CancellationTokenSource();

			CancellationToken ct = cancelToken.Token;

			task = Task.Run(() => { adj.Process(ct); }, ct);
			
			PauseStateValue = PauseState.RUNNING;

			UpdateProcessStatus();

			await task;

			taskComplete();

			UpdateProcessStatus();

			PauseStateValue = PauseState.WAITING;

			TbxMessage = "Test complete\n";
			AppendMsgLine("thread main state| " +
				Thread.CurrentThread.ThreadState.ToString());
		}

		private void taskComplete()
		{
			TbxMessage = "task completed\n";

			if (cancelToken.IsCancellationRequested)
			{
				AppendMsgLine("task complete| task cancel requested");
			}
			else
			{
				updateCounts();
			}
		}

	#endregion

	#region event consuming

		private void BtnCancelTask_OnClick(object sender, RoutedEventArgs e)
		{
			cancelToken.Cancel();
		}

		private void BtnPauseResumeTask_OnClick(object sender, RoutedEventArgs e)
		{
			if (PauseStateValue == PauseState.RUNNING)
			{
				if (!adj.PauseTask(true)) return;

				PauseStateValue = PauseState.PAUSED;
			}
			else
			{
				if (!adj.PauseTask(false)) return;

				PauseStateValue = PauseState.RUNNING;
			}
		}

		private void BtnTestProgBar_OnClick(object sender, RoutedEventArgs e)
		{
			testPb1Async();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			SampleData sd = Sd;

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

		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			TbxMessage = "btn test started\n";

			modifyAsync();

			TbxMessage = "btn test complete\n";
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