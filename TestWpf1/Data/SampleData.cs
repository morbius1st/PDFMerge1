#region + Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestWpf1.Windows;

#endregion

// user name: jeffs
// created:   11/29/2020 12:00:48 PM

namespace TestWpf1.Data
{
	// public static class Container
	// {
	// 	public static BaseOfTree Bot { get; set; }
	// }

	public class SampleData
	{

	#region fields

		private static int maxNodes = 15;
		private static int maxDepth = 3;

		private int[] items;
		private int[] order;
		private int[] test;

	#endregion

	#region ctor

		static SampleData()
		{
			SampleInfo(Root);
		}

		public SampleData()
		{
			Node.masterNumber = 0;
			ExtendedData.masterExtNumber = 0;

			SampleInfo(TreeRoot);
		}

	#endregion

	#region public properties

		public static BaseOfTree Root { get; set; }  = new BaseOfTree("Root of Tree");

		public BaseOfTree TreeRoot { get; set; } = new BaseOfTree("Tree Root");

		public int[] TestItems => this.test;

	#endregion

		public int CreateTestArray(int count)
		{
			return createTestArray(count);
		}


	#region private methods

		private static void SampleInfo(Node parent)
		{
			parent.ChildNodes = new ObservableCollection<Node>();
			parent.ExtData = new ExtendedData("ExtendedData");
			parent.ExtData.MergeInfo = new ObservableCollection<MergeData>();

			parent.ExtData.MergeInfo.CollectionChanged += MergeInfoOnCollectionChanged;

			sampleInfo(parent, 0, maxNodes);
		}

		private static void sampleInfo(Node parent, int depth, int numNodes)
		{
			if (depth == maxDepth) return;

			Node child;

			for (int i = 0; i < numNodes; i++)
			{
				child = new Node("ChildNode");
				child.ChildNodes = new ObservableCollection<Node>();
				child.ExtData = new ExtendedData("ExtendedData");
				child.ExtData.MergeInfo = new ObservableCollection<MergeData>();
				// child.ExtData.LockIdx = 0;

				// child.ExtData.MergeInfo.CollectionChanged += MergeInfoOnCollectionChanged;

				parent.ChildNodes.Add(child);

				if (depth < 1)
				{
					sampleInfo(child, depth + 1, maxNodes);
				}
				else
				{
					if (i == 0)
					{
						sampleInfo(child, depth + 1, maxNodes / 2);
					}
				}
			}
		}

		private static void MergeInfoOnCollectionChanged(object sender,
			NotifyCollectionChangedEventArgs e)
		{
			MergeData md = e.NewItems[0] as MergeData;
			MainWinTestWpf1.me.TbxMessage = "Collection Changed| " + md.MergeName;
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

	#endregion

	}
}
