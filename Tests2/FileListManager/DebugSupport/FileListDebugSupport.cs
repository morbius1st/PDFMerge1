#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Tests2.Windows;

#endregion


// projname: Tests2.FileListManager.DebugSupport
// itemname: FileListDebugSupport
// username: jeffs
// created:  11/24/2019 10:11:06 AM


namespace Tests2.FileListManager.DebugSupport
{
	public class FileListDebugSupport
	{
		public void test(out List<FileItem> fileItems2)
		{
			string root = @"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-00 flat\Individual PDFs";
			
			

			fileItems2 = new List<FileItem>();

			FileListMgr.BaseFolder = new Route(root);

			FileItem.RootPath =
				new Route(root);

			FileItem f;

			f = new FileItem(root + @"\Sub-Folder\A A1.0-1 This is a Test A30.pdf");
			fileItems2.Add(f);
			f = new FileItem(root + @"\A A1.1-0 This is a Test A10.pdf");
			fileItems2.Add(f);
			f = new FileItem(root + @"\A A1.2-0 This is a Test A20.pdf");
			fileItems2.Add(f);
			f = new FileItem(root + @"\A A1.3-0 This is a Test A30.pdf");
			fileItems2.Add(f);

		}
	}
}
