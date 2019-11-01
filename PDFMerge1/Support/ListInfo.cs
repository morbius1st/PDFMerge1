#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static UtilityLibrary.MessageUtilities;

#endregion


// projname: PDFMerge1.Support
// itemname: ListInfo
// username: jeffs
// created:  10/31/2019 7:53:49 PM


namespace PDFMerge1.Support
{
	internal class ListInfo
	{
		private static ListInfo _instance;

		public static ListInfo Instance
		{
			get
			{
				if (_instance == null) _instance = new ListInfo();
				return _instance;
			}
		}

		private ListInfo() { }


		public void ListFiles(FileList fileList)
		{
			logMsgln("");
			logMsgln("******************");
			logMsgDbLn2("FileList");
			logMsgDbLn2("FileList| root path", fileList.RootPath);
			logMsgDbLn2("FileList| gross count",fileList.GrossCount);
			logMsgDbLn2("FileList| net count", fileList.NetCount);
		

			foreach (FileItem fi in fileList)
			{
//				ListFileItem1(fi);
				ListFileItem2(fi);
			}
		}

		private void ListFileItem1(FileItem fi)
		{
			logMsgDbLn2("fileitem| name",
				fi.getName().PadRight(30)
				+ " :: depth| " + fi.Depth
				+ " :: type| " + fi.ItemType
				+ " :: path| " + fi.path
				);
		}

		private void ListFileItem2(FileItem fi)
		{
			logMsgln("***********", "************");
			logMsgDbLn2("fileitem| name", fi.getName());
			logMsgDbLn2("fileitem| path",fi.path);
			logMsgDbLn2("fileitem| outlinepath",fi.outlinePath);
			logMsgln("");
		}



		public void ListMergeTree(PdfMergeTree pdfMergeTree)
		{
			logMsgln("");
			logMsgln("******************");
			logMsgDbLn2("PdfMergeTree");

			logMsgDbLn2(pdfMergeTree.ToString());
		}

	}
}
