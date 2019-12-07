#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests2.DebugSupport;
using Tests2.FileListManager;
using Tests2.PDFMergeManager;

#endregion


// projname: Tests2.PDFMergeManager
// itemname: PDFMergeManager
// username: jeffs
// created:  11/27/2019 11:40:14 PM


namespace Tests2.PDFMergeManager
{
	public class PDFMergeMgr
	{
		public PDFMergeTree MTree { get; private set; }

		public static PDFMergeMgr Instance { get; } = new PDFMergeMgr();

		static PDFMergeMgr() { }

		private PDFMergeMgr() { }

		public void CreateMergeList()
		{
			MTree = PDFMergeTree.Instance;

			MTree.Add();
		}

		public void ListTree()
		{
			MTree.ListMergeTree();
		}
	}
}
