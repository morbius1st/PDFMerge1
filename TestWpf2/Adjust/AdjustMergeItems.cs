#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using TestWpf2.Data;
using TestWpf2.Windows;
using ThreadState = System.Threading.ThreadState;

#endregion

// user name: jeffs
// created:   11/29/2020 2:45:43 PM

namespace TestWpf2.Adjust
{
	public enum PauseState
	{
		WAITING,
		RUNNING,
		PAUSED
	}

	public class AdjustMergeItems : INotifyPropertyChanged
	{
		private IProgress<double> pb1ProgressDbl;
		private IProgress<double> pb2ProgressDbl;

		private List<object> lockList = new List<object>();

		private int testIdx = 0;
		private int itemIdx = 0;

		private CancellationTokenSource cancelTokenSrc;
		private CancellationToken cancelToken;

		private SampleData sd;

		private string[] pauseStateMsg = new [] {"Waiting", "Pause", "Resume"};
		private PauseState pauseState = PauseState.WAITING;
		private bool pauseTask;

		private Task task;

	#region ctor

		// public AdjustMergeItems()
		// {
		// 	PauseStateValue = PauseState.WAITING;
		// }

	#endregion

	#region public properties

		// public CancellationToken CancelToken => cancelTokenSrc.Token;

		public TaskStatus? ProcessStatus => task?.Status ?? TaskStatus.WaitingToRun;

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

	#region public methods

		public void CancelTask()
		{
			if (ProcessStatus == TaskStatus.Running)
				cancelTokenSrc.Cancel();
		}

		// public bool PauseTask(bool pause)
		// {
		// 	if (MainWinTestWpf2.me.ProcessStatus != TaskStatus.Running) return false;
		//
		// 	pb2ProgressDbl.Report(0);
		//
		// 	pauseTask = pause;
		//
		// 	return true;
		// }

		public bool? PauseTask(bool? pause)
		{
			if (ProcessStatus != TaskStatus.Running
				|| pauseState == PauseState.WAITING) return null;

			if (pauseState == PauseState.RUNNING && (pause == null || pause.Value))
			{
				pauseTask = true;
				return true;
			} 
			else if (pauseState == PauseState.PAUSED && (pause == null || !pause.Value))
			{
				pauseTask = false;
				return false;
			}

			return null;
		}


		public void UpdateTaskStatus2(PauseState state)
		{
			PauseStateValue = state;

			OnStatusChange("ProcessStatus");
		}

		public int Configure(SampleData sdx,
			IProgress<double> pb1Dbl,
			IProgress<double> pb2Dbl)
		{
			pb1ProgressDbl = pb1Dbl;
			pb2ProgressDbl = pb2Dbl;

			sd = sdx;

			sd.TreeRoot.exCount();

			// return createTestArray(sd.TreeRoot.ExtCount);

			return sd.CreateTestArray(sd.TreeRoot.ExtCount);
		}

		public async void Process2()
		{
			task?.Dispose();
			task = null;

			PreProcess2(sd.TreeRoot);

			setCancelToken2();

			task = Task.Run(() => { ProcessTree2(); }, cancelToken);

			await task;
		}

		public void ProcessTree2()
		{
		#if DEBUG

			Thread.CurrentThread.Name = "ProcessAsync";

			MainWinTestWpf2.me.AppendMsgLine("Process started");
			MainWinTestWpf2.me.AppendMsgLine("thread curr id| "
				+ Thread.CurrentThread.ManagedThreadId);

			MainWinTestWpf2.me.AppendMsgLine("thread curr state| " +
				Thread.CurrentThread.ThreadState.ToString());

			Debug.WriteLine("*** process tree| ***");

		#endif

			sd.TreeRoot.ExtData.MergeInfo = null;
			sd.TreeRoot.ExtData.ToBeProcessed = false;

			testIdx = 0;
			itemIdx = 0;

			UpdateTaskStatus2(PauseState.RUNNING);

			processTree2(sd.TreeRoot);

			UpdateTaskStatus2(PauseState.WAITING);
		}

		public void PreProcess2(Node parent)
		{
			testIdx = 0;
			itemIdx = 0;

			preProcess2(parent);
		}

		// public void Process(CancellationToken cancelToken)
		// {
		// 	Thread.CurrentThread.Name = "ProcessAsync";
		//
		// 	this.cancelToken = cancelToken;
		//
		// 	// MainWinTestWpf2.me.UpdateProcessStatus();
		//
		// 	MainWinTestWpf2.me.AppendMsgLine("Process started");
		// 	MainWinTestWpf2.me.AppendMsgLine("thread curr id| "
		// 		+ Thread.CurrentThread.ManagedThreadId);
		//
		// 	MainWinTestWpf2.me.AppendMsgLine("thread curr state| " +
		// 		Thread.CurrentThread.ThreadState.ToString());
		//
		// 	sd.TreeRoot.ExtData.MergeInfo = null;
		// 	sd.TreeRoot.ExtData.ToBeProcessed = false;
		//
		// 	testIdx = 0;
		// 	itemIdx = 0;
		//
		// 	Debug.WriteLine("*** process tree| ***");
		//
		//
		// 	processTree(sd.TreeRoot);
		// }

	#endregion

	#region private methods

		private void puaseTask()
		{
			// process status must == running
			// pause state must == running
			// then pauseTask = true;
			

		}

		private void setCancelToken2()
		{
			cancelTokenSrc = new CancellationTokenSource();

			cancelToken = cancelTokenSrc.Token;
		}

		private void preProcess2(Node parent)
		{
			foreach (Node child in parent.ChildNodes)
			{
				Debug.WriteLine("pre processing child| " + child.Name + " :: " + child.Number);
		
				if (testIdx >= sd.TestItems.Length)
				{
					Debug.WriteLine("pre processing child| break ***");
					break;
				}
		
				if (child.Number == sd.TestItems[testIdx])
				{
					Debug.WriteLine("pre processing| " + testIdx + "  (" + sd.TestItems[testIdx] + ")");
		
					child.ExtData.ToBeProcessed = true;
					child.ExtData.MergeInfo = new ObservableCollection<MergeData>();
					child.ExtData.MergeInfo.CollectionChanged += MergeInfo_CollectionChanged;
		
		
					child.ExtData.LockIdx = lockList.Count;
		
					object loc = new object();
		
					BindingOperations.EnableCollectionSynchronization(child.ExtData.MergeInfo, loc);
		
					lockList.Add(loc);
		
					testIdx++;
				}
				else
				{
					child.ExtData.ToBeProcessed = false;
					child.ExtData.MergeInfo = null;
				}
		
				if (child.Count > 0) preProcess2(child);
			}
		}

		private void processTree2(Node node)
		{
			int qty;

			foreach (Node child in node.ChildNodes)
			{
				UpdateTaskStatus2(PauseState.RUNNING);

				if (cancelToken.IsCancellationRequested)
				{
					MainWinTestWpf2.me.AppendMsgLine("preoceetree2| cancel requested");
					break;
				}

				if (pauseTask) pause();

				if (child.ExtData.ToBeProcessed)
				{
					qty = child.Number % 10;

					modify2(child, qty);

					itemIdx++;
				}

				testIdx++;

				if (child.Count > 0) processTree2(child);
			}
		}

		private void modify2(Node child, int qty)
		{
			pb1ProgressDbl.Report(itemIdx);

			Debug.WriteLine("adjusting| " + child.Name + "  (" + child.Number + ")" + "  (" + itemIdx + ")");

			object loc = lockList[child.ExtData.LockIdx];

			lock (loc)
			{
				for (int i = 0; i < qty; i++)
				{
					MergeData md = new MergeData("MergeData");
					child.ExtData.MergeInfo.Add(md);

					Thread a = Thread.CurrentThread;
					Context b = Thread.CurrentContext;

					Thread.Sleep(10);
				}
			}

			child.ExtData.UpdateProperties();
		}

		private void pause()
		{
			UpdateTaskStatus2(PauseState.PAUSED);

			double i = MainWinTestWpf2.me.Pb2MaxValue;

			while (pauseTask)
			{
				pb2ProgressDbl.Report(i);

				Thread.Sleep(50);

				if (--i < 0 ) pauseTask = false;
			}

			UpdateTaskStatus2(PauseState.RUNNING);
		}

	#endregion

	#region event consuming

		private void MergeInfo_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Debug.WriteLine("collection changed| " + e.Action.ToString());
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public event PropertyChangedEventHandler StatusChanged;

		private void OnStatusChange([CallerMemberName] string memberName = "")
		{
			StatusChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}
}