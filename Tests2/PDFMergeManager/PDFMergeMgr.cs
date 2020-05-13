#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
	public class PDFMergeMgr : INotifyPropertyChanged
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
#pragma warning disable CS1061 // 'PDFMergeTree' does not contain a definition for 'ListMergeTree' and no accessible extension method 'ListMergeTree' accepting a first argument of type 'PDFMergeTree' could be found (are you missing a using directive or an assembly reference?)
			MTree.ListMergeTree();
#pragma warning restore CS1061 // 'PDFMergeTree' does not contain a definition for 'ListMergeTree' and no accessible extension method 'ListMergeTree' accepting a first argument of type 'PDFMergeTree' could be found (are you missing a using directive or an assembly reference?)
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

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
