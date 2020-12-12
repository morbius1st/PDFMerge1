#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using TestWpf1.Data;
using TestWpf1.Windows;
using ThreadState = System.Threading.ThreadState;

#endregion

// user name: jeffs
// created:   11/29/2020 2:45:43 PM

namespace TestWpf1.Adjust
{
	public class AdjustMergeItems
	{
		private IProgress<double> pb1ProgressDbl;
		private IProgress<double> pb2ProgressDbl;

		private List<object> lockList = new List<object>();

		private int testIdx = 0;
		private int itemIdx = 0;

		private CancellationToken cancelToken;

		private SampleData sd;

		private bool pauseTask;

	#region public methods

		public bool PauseTask(bool pause)
		{
			if (MainWinTestWpf1.me.ProcessStatus != TaskStatus.Running) return false;

			pb2ProgressDbl.Report(0);

			pauseTask = pause;

			return true;
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

		public void PreProcess(Node parent)
		{
			testIdx = 0;
			itemIdx = 0;

			preProcess(parent);
		}

		public void Process(CancellationToken cancelToken)
		{
			Thread.CurrentThread.Name = "ProcessAsync";

			this.cancelToken = cancelToken;

			MainWinTestWpf1.me.UpdateProcessStatus();

			MainWinTestWpf1.me.TbxMessage = "Process started\n";
			MainWinTestWpf1.me.AppendMsgLine("thread curr id| "
				+ Thread.CurrentThread.ManagedThreadId);

			MainWinTestWpf1.me.AppendMsgLine("thread curr state| " +
				Thread.CurrentThread.ThreadState.ToString());

			sd.TreeRoot.ExtData.MergeInfo = null;
			sd.TreeRoot.ExtData.ToBeProcessed = false;

			testIdx = 0;
			itemIdx = 0;

			Debug.WriteLine("*** process tree| ***");


			processTree2(sd.TreeRoot);
		}

	#endregion

	#region private methods

		private void preProcess(Node parent)
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

				if (child.Count > 0) preProcess(child);
			}
		}

		private void processTree2(Node node)
		{
			int qty;

			foreach (Node child in node.ChildNodes)
			{
				MainWinTestWpf1.me.UpdateProcessStatus();

				if (cancelToken.IsCancellationRequested)
				{
					MainWinTestWpf1.me.AppendMsgLine("preoceetree2| cancel requested");
					break;
				}

				if (pauseTask) pause();

				if (child.ExtData.ToBeProcessed)
				{
					qty = child.Number % 10;

					modify(child, qty);

					itemIdx++;
				}

				testIdx++;

				if (child.Count > 0) processTree2(child);
			}
		}

		private void modify(Node child, int qty)
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

					Thread.Sleep(10);
				}
			}

			child.ExtData.UpdateProperties();
		}

		private void pause()
		{
			double i = MainWinTestWpf1.me.Pb2MaxValue;

			while (pauseTask)
			{
				pb2ProgressDbl.Report(i);

				Thread.Sleep(500);

				if (--i < 0 ) break;
			}
		}

	#endregion

	#region event consuming

		private void MergeInfo_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Debug.WriteLine("collection changed| " + e.Action.ToString());
		}

	#endregion

	}
}