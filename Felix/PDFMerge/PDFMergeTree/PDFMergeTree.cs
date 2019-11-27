#region + Using Directives

#endregion


// projname: Felix.PDFMerge.MergeTree
// itemname: MergeTree
// username: jeffs
// created:  11/26/2019 10:21:46 AM


using System.Collections;
using System.Collections.Generic;
using Felix.FileListManager;
using iText.Layout.Element;

namespace Felix.PDFMerge.PDFMergeTree
{
	// the organized tree of items to merge
	// based on the configured outlines 
	// associated with each file to merge
	public class PDFMergeTree : 
		IEnumerable<KeyValuePair<string, PDFMergeItem>>
	{
		private Dictionary<string, PDFMergeItem> mergeTree;

		public PDFMergeTree()
		{
			mergeTree = new Dictionary<string, PDFMergeItem>();
		}

		public int Count => mergeTree.Count;

		public void Add()
		{
			foreach (FileItem item in FileItems.Instance)
			{
				Add(item, 0, mergeTree);
			}
		}

		public void Add(FileItem item, int currDepth,
			Dictionary<string, PDFMergeItem> mtree)
		{
			
			
		}

		public IEnumerator<KeyValuePair<string,
			PDFMergeItem>> GetEnumerator()
		{
			return mergeTree.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
