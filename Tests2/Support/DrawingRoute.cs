#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests2.FileListManager;

#endregion


// projname: Tests2.Support
// itemname: DrawingRoute
// username: jeffs
// created:  12/8/2019 11:28:42 AM


namespace Tests2.Support
{
	public class DrawingRoute : Route
	{
		public string SheetNumber { get; private set; }
		public string SheetName { get; private set; }

		public DrawingRoute(string initialRoute) : base(initialRoute)
		{


		}
	}
}
