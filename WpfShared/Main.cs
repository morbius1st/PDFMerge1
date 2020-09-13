#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfShared.Windows;

#endregion

// user name: jeffs
// created:   7/12/2020 5:06:31 PM


namespace WpfShared
{
	public class Program
	{
		public static WindowManager WinMgr = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Debug.WriteLine("\nWpfShared started\n");
			Start();
		}

		public static void Start()
		{
			Initalize();

			WinMgr.Start();
		}

		private static void Initalize()
		{
			WinMgr = new WindowManager();
		}

	}
}
