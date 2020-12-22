#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion

// username: jeffs
// created:  12/20/2020 9:58:16 PM

namespace StoreAndRead.TestClasses
{
	public class Derived1 : BaseClass
	{

		public Derived1(string name) : base(name) { }

		public override void RaiseHand()
		{
			Debug.WriteLine("I can do this Derived 1| " + name);
		}

	}
}