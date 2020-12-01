#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestWpf1.Data;

#endregion

// user name: jeffs
// created:   11/29/2020 2:45:43 PM

namespace TestWpf1.Adjust
{
	public class AdjustMergeItems
	{
		private int[] items;
		private int[] order;
		private int[] test;

		private int testIdx = 0;

		private SampleData sd;

		public void Process()
		{
			processTree(sd.TreeRoot);
		}

		public int Configure(SampleData sd)
		{
			this.sd = sd;

			this.sd.TreeRoot.exCount();
			int qty = createTestArray(this.sd.TreeRoot.ExtCount);
			// listTest(qty);

			return qty;
		}

		private void processTree(Node node)
		{
			foreach (Node child in node.ChildNodes)
			{
				if (testIdx >= test.Length) break;

				if (child.Number == test[testIdx])
				{
					// Debug.WriteLine("testing| " + child.Name);

					int qty = test[testIdx] % 10;
					testIdx++;

					child.ExtData.MergeInfo = new ObservableCollection<MergeData>();

					adjust(child, qty);
				}

				if (child.Count > 0) processTree(child);
			}
		}

		private void adjust(Node child, int qty)
		{
			for (int i = 0; i < qty; i++)
			{
				MergeData md = new MergeData("MergeData");

				child.ExtData.MergeInfo.Add(md);
			}

			child.ExtData.UpdateProperties();

			// Debug.WriteLine("adjusted| " + child.Name + " :: " + child.ExtData.ExtName + " (" + child.ExtData.MergeCount + " vs " + qty + ")");
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