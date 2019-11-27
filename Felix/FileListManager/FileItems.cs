#region + Using Directives

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Felix.FileListManager.DebugSupport;

#endregion


// projname: Tests2.FileListManager
// itemname: FileListMgr
// username: jeffs
// created:  11/12/2019 6:40:28 PM


namespace Felix.FileListManager
{
	/// <summary>
	/// this object holds the list of selected files
	/// </summary>
	/// 
	// this creates a FileItem for each
	// selected file and also processes
	// each file item to adjust the bookmark
	// based on the user's settings
	public class FileItems : INotifyPropertyChanged, IEnumerable<FileItem>
	{

	#region data fields

		private List<FileItem> fileItems;

	#if DEBUG
		public static List<FileItem> fileItems2 { get; private set; }
	#endif

	#endregion

		private static FileListDebugSupport flds = new FileListDebugSupport();

	#region ctor

		private static readonly FileItems instance = new FileItems();

		public static FileItems Instance => instance;

		static FileItems()
		{
		#if DEBUG
			flds.test(out List<FileItem> fileItems3);
			fileItems2 = fileItems3;
		#endif

		}

	#endregion

	#region public properties

//		public List<FileItem> FileListItems => fileItems;

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

		public void Add(List<FileItem> fileItems)
		{
			foreach (FileItem fileItem in fileItems)
			{
				this.fileItems.Add(fileItem);
			}

			OnPropertyChange("fileItems");
		}

		public void Add(FileItem fileItem)
		{
			fileItems.Add(fileItem);
		}

		public void Sort()
		{
			fileItems.Sort();
		}

	#endregion

	#region system routines

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
