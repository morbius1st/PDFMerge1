#region + Using Directives

#endregion


// projname: Felix.PDFMerge.MergeTree
// itemname: MergeTree
// username: jeffs
// created:  11/26/2019 10:21:46 AM


using System.Collections;
using System.Collections.Generic;
using System.Data;
using Felix.FileListManager;

namespace Felix.PDFMergeManager.PDFMergeTree
{
	// the organized tree of items to merge
	// based on the configured outlines 
	// associated with each file to merge
	public class PDFMergeTree : 
		IEnumerable<KeyValuePair<string, PDFMergeItem>>
	{
		private Dictionary<string, PDFMergeItem> mergeTree;

		private int leaf = 0;

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

		public void Add(FileItem item, int depth,
			Dictionary<string, PDFMergeItem> mtree)
		{
			if (item.OutlinePath.FolderCount >= depth)
			{
				// item has sub-outline
				
				string key = item.OutlinePath.FolderNames[depth];

				// does the sub-outline already not exist?
				if (!mtree.ContainsKey(item.OutlinePath.FolderNames[depth]))
				{
					// sub-outline does not exist 
					// create the node - then add go to the next level



					mtree.Add(key, 
						new PDFMergeItem(key, TreeNodeType.BRANCH, 0, 
							depth, item));

				}

				// did exist or now exists - goto next level
				PDFMergeItem innerItem;

				if (mtree.TryGetValue(key, out innerItem))
				{
					Add(item, depth + 1, innerItem.mergeItems);
				}
				else
				{
					throw new MissingPrimaryKeyException();
				}

			}
			else
			{
				// does not have sub-outlines
				// add the leaf to the tree
				mtree.Add("leaf " + leaf++, 
					new PDFMergeItem(
						item.OutlinePath.FileWithoutExtension,
						TreeNodeType.LEAF,
						0, depth,
						item
						)
					);

			}
			
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
