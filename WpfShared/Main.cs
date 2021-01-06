#region + Using Directives
using System;
using System.Diagnostics;
using WpfShared.Windows;

#endregion

// user name: jeffs
// created:   7/12/2020 5:06:31 PM


namespace WpfShared
{
	public class Program
	{
		public static WindowManager WinMgr = null;
		private static App app = null;

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

			// StartWin(WindowId.DIALOG_SELECT_CLASSF_FILE);
			StartWin(WindowId.WINDOW1);
		}

		private static void Initalize()
		{
			app = new App();
			app.InitializeComponent();

			WinMgr = new WindowManager();
		}

		public static void StartWin(WindowId which)
		{
			WinMgr.Show(which);
			}


	}
}
