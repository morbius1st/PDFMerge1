#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		private int[] items;
		private int[] order;
		private int[] test;

		private List<object> lockList = new List<object>();

		private int testIdx = 0;
		private int itemIdx = 0;


		private SampleData sd;

		public async void testPbUpdate(IProgress<double> pb1Double, double max)
		{
			pb1ProgressDbl = pb1Double;

			await Task.Run(
				() =>
				{
					tstPb(max);
				});
		}

		private void tstPb(double max)
		{
			for (double i = 0; i < max; i++)
			{
				Thread.Sleep(50);
				pb1ProgressDbl.Report(i + 1);
			}
		}

		public int Configure(SampleData sd, IProgress<double> pb1Double)
		{
			this.pb1ProgressDbl = pb1Double;

			this.sd = sd;

			this.sd.TreeRoot.exCount();
			int qty = createTestArray(this.sd.TreeRoot.ExtCount);

			return qty;
		}

		public void Process(Thread t)
		{
			// Thread.CurrentThread.IsBackground = false;

			MainWinTestWpf1.me.TbxMessage = "Process started\n";
			MainWinTestWpf1.me.AppendMsgLine("thread id| "
				+ Thread.CurrentThread.ManagedThreadId);

			MainWinTestWpf1.me.AppendMsgLine("thread main| " + 
				t.ThreadState.ToString());
			MainWinTestWpf1.me.AppendMsgLine("thread curr| " + 
				Thread.CurrentThread.ThreadState.ToString());

			sd.TreeRoot.ExtData.MergeInfo = null;
			sd.TreeRoot.ExtData.ToBeProcessed = false;

			// testIdx = 0;

			// Debug.WriteLine("*** pre-processing| ***");
			// preProcess(sd.TreeRoot);

			testIdx = 0;
			itemIdx = 0;

			Debug.WriteLine("*** process tree| ***");

			// await Task.Run(() => { processTree(sd.TreeRoot); });
			processTree2(sd.TreeRoot);

			MainWinTestWpf1.me.TbxMessage = "Process ended\n";
		}

		
		private void processTree2(Node node)
		{
			int qty ; //= 3;

			foreach (Node child in node.ChildNodes)
			{
				// Debug.WriteLine("processing child| " + child.Name + " :: " + child.Number + " (" + testIdx + ")");

				// if (testIdx >= test.Length) break;

				if (child.ExtData.ToBeProcessed)
				{
					// Debug.WriteLine("processing| item index" + itemIdx + "  (" + child.Number + ")");

					qty = child.Number % 10;

					// await Task.Run(() => { adj(child, qty); });
					// adjust(child, qty);
					adj2(child, qty);

					itemIdx++;
				}

				testIdx++;

				if (child.Count > 0) processTree2(child);

			}
		}
		
		private void adj2(Node child, int qty)
		{
			pb1ProgressDbl.Report(itemIdx);

			Debug.WriteLine("adjusting| " + child.Name + "  (" + child.Number);

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


		private void processTree1(Node node)
		{
			int qty ; //= 3;

			foreach (Node child in node.ChildNodes)
			{
				Debug.WriteLine("processing child| " + child.Name + " :: " + child.Number);

				if (testIdx >= test.Length) break;

				if (child.ExtData.ToBeProcessed)
				{
					Debug.WriteLine("processing| " + testIdx + "  (" + test[testIdx] + ")");

					qty = test[testIdx] % 10;

					// await Task.Run(() => { adj(child, qty); });
					adjust(child, qty);

					testIdx++;
				}

				if (child.Count > 0) processTree1(child);
			}
		}

		private async void adjust(Node child, int qty)
		{
			await Task.Run(
				() => { adj(child, qty); });
		}

		private void adj(Node child, int qty)
		{
			pb1ProgressDbl.Report(testIdx);

			Debug.WriteLine("adjusting| " + testIdx + "  (" + test[testIdx]);

			object loc = lockList[child.ExtData.LockIdx];

			lock (loc)
			{
				for (int i = 0; i < qty; i++)
				{
					MergeData md = new MergeData("MergeData");
					child.ExtData.MergeInfo.Add(md);

					// Thread.Sleep(20);
				}
			}

			child.ExtData.UpdateProperties();
		}



		public void preProcess(Node parent)
		{
			foreach (Node child in parent.ChildNodes)
			{
				Debug.WriteLine("pre processing child| " + child.Name + " :: " + child.Number);

				if (testIdx >= test.Length)
				{
					Debug.WriteLine("pre processing child| break ***");
					break;
				}
				// if (testIdx > 10) break;

				// if (true)
				if (child.Number == test[testIdx])
				{
					Debug.WriteLine("pre processing| " + testIdx + "  (" + test[testIdx] + ")");

					child.ExtData.ToBeProcessed = true;
					child.ExtData.MergeInfo = new ObservableCollection<MergeData>();

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

		private int createTestArray(int qty)
		{
			items = new int[qty];
			order = new int[qty];

			Random rnd = new Random();

			for (int i = 0; i < qty; i++)
			{
				items[i] = i;
				order[i] = rnd.Next();
			}

			Array.Sort(order, items);

			int amt = (1 * qty) / 4;

			test = new int[amt];

			for (int i = 0; i < amt; i++)
			{
				test[i] = items[i];
			}

			Array.Sort(test);

			return amt;
		}

		private void listTest(int[] array)
		{
			Debug.WriteLine("*** array length | " + array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				Debug.WriteLine("index| " + i + " :: item number| " + array[i]);
			}
		}
	}
}