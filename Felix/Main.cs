#region + Using Directives

using System;
using Felix.Windows;

#endregion


// projname: Felix
// itemname: Main
// username: jeffs
// created:  11/26/2019 9:49:27 AM


namespace Felix
{
	public class Program
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