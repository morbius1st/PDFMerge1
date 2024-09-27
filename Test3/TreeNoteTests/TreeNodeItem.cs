#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



#endregion

// user name: jeffs
// created:   9/21/2024 9:36:28 AM

namespace Test3.TreeNoteTests
{
	public class TreeNodeItem : INodeItem2
	{
		private static int tempIdx = 0;

		private TreeNodeItem()
		{
			Id = $"Temp_{tempIdx++}";
		}

		public TreeNodeItem(string id, bool isFixed, bool isLocked)
		{
			Id = id;
			IsFixed = isFixed;
			IsLocked = isLocked;
		}


		public string Id { get; set; }
		public int Depth { get; set; }

		public int Count { get; set; }
		public int MergeItemCount { get; set; }

		public bool IsFixed { get; set; }
		public bool IsLocked { get; set; }


		public INodeItem2 TempNodeItem(int depth)
		{
			return new TreeNodeItem();
		}

		public INodeItem2 Clone()
		{
			return null;
		}


		public override string ToString()
		{
			return $"this is {nameof(TreeNodeItem)}";
		}
	}
}
