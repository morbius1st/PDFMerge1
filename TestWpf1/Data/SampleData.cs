#region + Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
		private static int maxNodes = 20;
		private static int maxDepth = 4;

		private int extCount;
		private int extMrgCount;

		public static BaseOfTree Root { get; set; }  = new BaseOfTree("Root of Tree");

		public BaseOfTree TreeRoot { get; set; } = new BaseOfTree("Tree Root");

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

		// // public int ExtendedCount() => extendedCount(Root);
		// public int ExtendedCount => extCount;
		//
		// public int ExtendedMergeCount => extMrgCount;
		//
		// public void UpdateCounts()
		// {
		// 	extCount = TreeRoot.ExtCount;
		// 	extMrgCount = TreeRoot.ExtMergeCount;
		// }

		private static void SampleInfo(Node node)
		{
			node.ChildNodes = new ObservableCollection<Node>();
			node.ExtData = new ExtendedData("ExtendedData");

			sampleInfo(node, 0, maxNodes);
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

				// Debug.WriteLine("ExtData made for| " + child.ExtData.ExtName + "  (" + child.Name + ")");

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

	}
}
