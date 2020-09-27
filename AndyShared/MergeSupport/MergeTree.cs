#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

// username: jeffs
// created:  9/26/2020 4:33:22 PM

namespace AndyShared.MergeSupport
{
	public class MergeTree
	{
	#region private fields

		private Dictionary<string, MergeItem> treeRoot;

	#endregion

	#region ctor

		public MergeTree()
		{
			treeRoot = new Dictionary<string, MergeItem>();


		}

	#endregion

	#region public properties

		public Dictionary<string, MergeItem> TreeRoot => treeRoot ?? null;

		public int Count => fullCount(treeRoot, true);
		public int LeafCount => fullCount(treeRoot);
		public int BranchCount => Count - LeafCount;
	#endregion

	#region private properties

		/// <summary>
		/// Count the total number of nodes (of just the leaves)
		/// </summary>
		private int fullCount(Dictionary<string, MergeItem> items, bool? countWhich = false)
		{
			// count which
			// true == count leaves
			// null == count all
			// false == count branches

			int count = 0;

			foreach (KeyValuePair<string, MergeItem> kvp in treeRoot)
			{
				if (kvp.Value.HasChildNodes)
				{
					count += fullCount(kvp.Value.mergeTreeBranch, countWhich);

					if (countWhich.Value != true) count++;
				}
				else
				{
					if (countWhich.Value != false) count++;
				}
			}

			return count;
		}

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MergeTree";
		}

	#endregion
	}
}