#region + Using Directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

#endregion


// projname: Tests2.FileListManager
// itemname: FileListMgr
// username: jeffs
// created:  11/12/2019 6:40:28 PM


namespace Tests2.FileListManager
{
	/// <summary>
	/// this object holds the list of selected files
	/// </summary>
	/// 
	// this creates a FileItem for each
	// selected file and also processes
	// each file item to adjust the bookmark
	// based on the user's settings
	public class FileList : INotifyPropertyChanged, IEnumerable<FileItem>
	{
	#region ctor

		private static readonly FileList instance = new FileList();

		public static FileList Instance => instance;

		static FileList()
		{
		#if DEBUG
			test();
		#endif

		}

	#if DEBUG
		private static void test()
		{
			fileItems2 = new List<FileItem>();


			FileItem f;

			FileItem.RootPath = new Route(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-00 flat\Individual PDFs");

			f = new FileItem(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-00 flat\Individual PDFs\A A1.0-0 This is a Test A10.pdf");
			fileItems2.Add(f);
			f = new FileItem(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-00 flat\Individual PDFs\A A1.0-0 This is a Test A20.pdf");
			fileItems2.Add(f);
			f = new FileItem(@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-00 flat\Individual PDFs\A A1.0-0 This is a Test A30.pdf");
			fileItems2.Add(f);

		}
	#endif

	#endregion

	#region private fields

		public List<FileItem> fileItems { get; private set; }
		public static List<FileItem> fileItems2 { get; private set; }

	#endregion

	#region public properties

		public ICollectionView Vue { get; private set; }
//
//		public Route RootPath { get; private set; }

		public bool IsInitialized { get; private set; }

		public int Count => fileItems.Count;

		public FileItem this[int index] => fileItems[index];

	#endregion


	#region public methods

		public void Initialize()
		{
			fileItems = new List<FileItem>();
			IsInitialized = true;
		}

		public void VueSort()
		{
			SortDescription sd = new SortDescription("outlinePath", ListSortDirection.Ascending);

			Vue.SortDescriptions.Add(sd);
		}

		public void VueSortClear()
		{
			Vue.SortDescriptions.Clear();
		}

	#endregion

		public void Add(List<FileItem> fileItems)
		{
			foreach (FileItem fileItem in fileItems)
			{
				this.fileItems.Add(fileItem);
			}

			Vue = CollectionViewSource.GetDefaultView(fileItems);

			OnPropertyChange("fileItems");

			OnPropertyChange("Vue");
		}

		public void Add(FileItem fileItem)
		{
			fileItems.Add(fileItem);

			OnPropertyChange("Vue");
		}


	#region imported

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public IEnumerator<FileItem> GetEnumerator()
		{
			return fileItems.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	#endregion
	}
}
