#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndyShared.ClassificationDataSupport.TreeSupport;

#endregion

// user name: jeffs
// created:   9/21/2024 11:49:12 AM

namespace Test3.TreeNoteTests
{
	public class TriStateTreeSupport
	{

		public TriStateTree<T> MakeTriStateTree<T>()
			where T : class, INodeItem2
		{
			TriStateTree<T> tree = new TriStateTree<T>();

			tree.Item = null;
			
			tree.Initalize();

			return tree;
		}

		public override string ToString()
		{
			return $"this is {nameof(TriStateTreeSupport)}";
		}
	}
}
