#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AODeliverable.Settings;
using Microsoft.Win32;

using AODeliverable.FileSelection;
using AODeliverable.OutlineSupport;

#endregion


// projname: AODeliverable
// itemname: Main
// username: jeffs
// created:  11/2/2019 1:45:15 PM


namespace AODeliverable
{
	static class Program
	{
		public static MainWindow win = null;

		private static SelectFolderMgr sfMgr = null;

		private static SelectFilesMgr filesMgr = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			sfMgr = SelectFolderMgr.Instance;
			filesMgr = SelectFilesMgr.Instance;

			win = new MainWindow();

			ShowDialog();
		}

		public static bool process()
		{

			win.rtbStatus.Document.Blocks.Clear();
			win.rtbSettings.Document.Blocks.Clear();

			// config();

			bool? result = sfMgr.ChoseFolder();

			if (result != true) return false;

			filesMgr.Clear();

			filesMgr.BaseFolder = new Route(sfMgr.BaseFolder);

			result = filesMgr.GetFiles();

			if (result != true) return false;

			OutlineMgr.Instance.AdjustOutlinePaths();

			filesMgr.Sort();

			return true;
		}

		private static bool ShowDialog()
		{
			if (!process()) return false;

			return win.ShowDialog() == true;
		}

		private static void config()
		{
//			UserSettings.Admin.Save();
		}

	}
}
