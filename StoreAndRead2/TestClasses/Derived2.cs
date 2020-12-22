#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   12/20/2020 10:54:13 PM

namespace StoreAndRead.TestClasses
{
	public class Derived2 : BaseClass
	{
		public Derived2(string name) : base(name) { }

		public override void RaiseHand()
		{
			Debug.WriteLine("I can do this Derived2| " + name);
		}
	}
}
