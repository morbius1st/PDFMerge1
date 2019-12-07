#region + Using Directives

#endregion


// projname: Felix.PDFMerge.MergeTree
// itemname: MergeTree
// username: jeffs
// created:  11/26/2019 10:21:46 AM


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using Tests2.DebugSupport;
using Tests2.FileListManager;
using Tests2.Windows;


namespace Tests2.PDFMergeManager
{
	// the organized tree of items to merge
	// based on the configured outlines 
	// associated with each file to merge
	public class PDFMergeTree : INotifyPropertyChanged
	{
		public ObservableCollection<KeyValuePair<string, PDFMergeItem>> MergeTree { get; private set; }
			= new ObservableCollection<KeyValuePair<string, PDFMergeItem>>();

//		public Dictionary<string, PDFMergeItem> 
//			MergeTree { get; private set; } = 
//				new Dictionary<string, PDFMergeItem>();

#pragma warning disable CS0414 // The field 'PDFMergeTree.leaf' is assigned but its value is never used
		private int leaf = 0;
#pragma warning restore CS0414 // The field 'PDFMergeTree.leaf' is assigned but its value is never used

		public static PDFMergeTree Instance { get; } = new PDFMergeTree();

		static PDFMergeTree() { }

		private PDFMergeTree() { }

		public int Count => MergeTree.Count;

		public void Add()
		{
			foreach (FileItem item in FileItems.Instance)
			{
				AddItem(item, 0, MergeTree);
			}

			OnPropertyChange("MergeTree");
		}

		// add a new item to the end of the primary list
		public void Add(FileItem item)
		{
			AddItem(item, 0, MergeTree);

			OnPropertyChange("MergeTree");
		}

		public void Add(FileItem item, int index)
		{
			if (index < 0 || 
				index >= MergeTree.Count) throw new ArgumentOutOfRangeException();

			// expectation is to add a brand new level 0 tree item
			if ( ContainsKey(item.OutlinePath.FolderNames[0], MergeTree)) return;


			AddItem(item, 0, MergeTree);

			OnPropertyChange("MergeTree");
		}

		private void AddItem(FileItem item, int depth,
			ObservableCollection<KeyValuePair<string, PDFMergeItem>> mtree)
		{
			// does the outline path have sub-outlines
			if (item.OutlinePath.FolderCount - 1 >= depth)
			{
				// item has sub-outline
				string key = item.OutlinePath.FolderNames[depth];

				// does the sub-outline already not exist?
				if (!ContainsKey(item.OutlinePath.FolderNames[depth], mtree))
				{
					// sub-outline does not exist 
					// create the node - then add go to the next level

					PDFMergeItem mi = new PDFMergeItem(key,
						TreeNodeType.BRANCH, 0, depth, item);

					mi.AddMergeItems();

					mtree.Add(new KeyValuePair<string, PDFMergeItem>(key, mi));
				}

				// did exist or now exists - goto next level
				PDFMergeItem innerItem;

				if (TryGetValue(key, mtree, out innerItem))
				{
					AddItem(item, depth + 1, innerItem.MergeItems);
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
				mtree.Add(new KeyValuePair<string, PDFMergeItem>(
					item.OutlinePath.FileWithoutExtension,
					new PDFMergeItem(
						item.OutlinePath.FileWithoutExtension,
						TreeNodeType.LEAF,0, depth,item))
					);
			}
		}


		private ObservableCollection<KeyValuePair<string, PDFMergeItem>> FindBranch(FileItem item,
			ObservableCollection<KeyValuePair<string, PDFMergeItem>> mtree, int depth)
		{
			// does the outline path have sub-outlines?
			if (item.OutlinePath.FolderCount - 1 >= depth)
			{
				// item has sub-outlines
				string key = item.OutlinePath.FolderNames[depth];

				PDFMergeItem innerItem;

				// the sub-outline needs to exist?
				if (TryGetValue(key, mtree, out innerItem))
				{
					// sub-outline does exist 
					// check next level
					return FindBranch(item, innerItem.MergeItems, depth++);
				} 

				// sub-outline not found - this is a failure;
				return null;
			} 

			// no more sub-outlines
			// return the final branch
			return mtree;
		}

		public bool TryGetValue(string key, 
			ObservableCollection<KeyValuePair<string, 
				PDFMergeItem>> tree, out PDFMergeItem value)
		{
			foreach (KeyValuePair<string, PDFMergeItem> kvp in tree)
			{
				if (kvp.Key.Equals(key))
				{
					value = kvp.Value;
					return true;
				}
			}

			value = null;
			return false;
		}

		// this is a single level search
		public bool ContainsKey(string searchKey, ObservableCollection<KeyValuePair<string, PDFMergeItem>> tree)
		{
			foreach (KeyValuePair<string, PDFMergeItem> kvp in tree)
			{
				if (kvp.Key.Equals(searchKey)) return true;
			}

			return false;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


	#if DEBUG

		public void ListMergeTree()
		{
			MainWinManager.MessageAppendLine("");
			MainWinManager.MessageAppendLine("merge tree list\n");

			ListMergeTree(MergeTree);
		}

		private void ListMergeTree(ObservableCollection<KeyValuePair<string, PDFMergeItem>> mergeTree)
		{
			MainWinManager.MessageAppendLine("going down");

			foreach (KeyValuePair<string, PDFMergeItem> kvp in mergeTree)
			{
				ListMergeTreeItem(kvp);

				if (kvp.Value.TreeNodeType == TreeNodeType.BRANCH)
				{
					ListMergeTree(kvp.Value.MergeItems);
				}
			}

			MainWinManager.MessageAppendLine("going up");
		}

		private void ListMergeTreeItem(KeyValuePair<string, PDFMergeItem> kvp)
		{
			MainWinManager.MessageAppendLine(
				"key           | " + kvp.Key);
			MainWinManager.MessageAppendLine(
				"type          | " + kvp.Value.TreeNodeType);
			MainWinManager.MessageAppendLine(
				"outline depth | " + kvp.Value.Depth);
			MainWinManager.MessageAppendLine(
				"outline title | " + kvp.Value.BookmarkTitle);
			MainWinManager.MessageAppendLine(
				"file path     | " + kvp.Value.FileItem.FilePath.FullPath ?? "no branch path");

			if (kvp.Value.TreeNodeType == TreeNodeType.BRANCH)
			{
				MainWinManager.MessageAppendLine(
					"outline count | " + kvp.Value.MergeItems.Count);
			}

			MainWinManager.MessageAppendLine("");
		}

	#endif
	}
}