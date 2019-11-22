#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tests2.Windows;

#endregion


// projname: Tests2
// itemname: Main
// username: jeffs
// created:  11/12/2019 9:01:02 PM


namespace Tests2
{
	static class Program
	{
		public static MainWinManager MainWinMgr = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Start();
		}

		private static void Initalize()
		{
			MainWinMgr = new MainWinManager();
		}

		public static void Start()
		{
			Initalize();

			MainWinMgr.Start();
		}


	}
}
