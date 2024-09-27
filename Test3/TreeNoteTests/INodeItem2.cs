#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   9/21/2024 9:10:11 AM

namespace Test3.TreeNoteTests
{
	public interface INodeItem2
	{
		string Id { get; set; }
		int Depth { get; set; }

		// required
		bool IsFixed { get; set; }
		bool IsLocked { get; set; }

		// required
		INodeItem2 TempNodeItem(int depth);
		
		// required
		INodeItem2 Clone();

	}
}
