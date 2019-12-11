#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Tests2.DebugSupport;
using Tests2.FileListManager;
using Tests2.PDFMergeManager;
using Tests2.Windows;

#endregion


// projname: Tests2.PDFMergeManager
// itemname: PDFMergeManager
// username: jeffs
// created:  11/27/2019 11:40:14 PM


namespace Tests2.PDFMergeManager
{
	public class PDFMergeMgr
	{
		Dispatcher d = Dispatcher.CurrentDispatcher;

		public PDFMergeTree MTree { get; private set; } = PDFMergeTree.Instance;

		public static PDFMergeMgr Instance { get; } = new PDFMergeMgr();

		static PDFMergeMgr() { }

		private PDFMergeMgr() { }

		public void CreateMergeList()
		{
			MainWinManager.mainWin.UpdateProgressStatus("Creating Merge List - Start");
//			MTree = PDFMergeTree.Instance;

			MTree.Initialize();

//			MainWinManager.mainWin.UpdateProgressStatus("Creating Merge List - Complete");
		}

		public void ListTree()
		{
			MTree.ListMergeTree();
		}

//		public async void test(int i)
//		{
//
//			MTree.test(i);
//

//			Thread.Sleep(100);
//
//			d.Invoke(() =>
//			{
//				MainWinManager.mainWin.pb1.Value = i;
//				MainWinManager.mainWin.pbStatus.Text = i.ToString();
//			});
//		}
	}
}
