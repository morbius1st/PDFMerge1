#region + Using Directives

using System.Collections.Generic;
using Felix.Support;

#endregion


// projname: Tests2.FileListManager.DebugSupport
// itemname: FileListDebugSupport
// username: jeffs
// created:  11/24/2019 10:11:06 AM


namespace Felix.FileListManager.DebugSupport
{
	public class FileListDebugSupport
	{
		public void test(out List<FileItem> fileItems2)
		{
			fileItems2 = new List<FileItem>();

			FileItem f;

			FileItem.RootPath =
				new Route(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-00 flat\Individual PDFs");

			f = new FileItem(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-00 flat\Individual PDFs\Sub-Folder\A A1.0-1 This is a Test A30.pdf");
			fileItems2.Add(f);
			f = new FileItem(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-00 flat\Individual PDFs\A A1.1-0 This is a Test A10.pdf");
			fileItems2.Add(f);
			f = new FileItem(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-00 flat\Individual PDFs\A A1.2-0 This is a Test A20.pdf");
			fileItems2.Add(f);
			f = new FileItem(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-00 flat\Individual PDFs\A A1.3-0 This is a Test A30.pdf");
			fileItems2.Add(f);

		}
	}
}
