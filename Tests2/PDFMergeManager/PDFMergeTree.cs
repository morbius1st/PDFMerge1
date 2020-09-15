#region + Using Directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Tests2.DebugSupport;
using Tests2.FileListManager;
using Tests2.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;
#endregion


// projname: Felix.PDFMerge.MergeTree
// itemname: MergeTree
// username: jeffs
// created:  11/26/2019 10:21:46 AM


// see at the bottom for the selection procedures


namespace Tests2.PDFMergeManager
{
	// the organized tree of items to merge
	// based on the configured outlines 
	// associated with each file to merge
	public class PDFMergeTree : INotifyPropertyChanged
	{
		Dispatcher d = Dispatcher.CurrentDispatcher;

		public ObservableCollection<KeyValuePair<string, PDFMergeItem>> MergeTree { get; private set; }
			= new ObservableCollection<KeyValuePair<string, PDFMergeItem>>();

		public static PDFMergeTree Instance { get; } = new PDFMergeTree();

		static PDFMergeTree() { }

		private PDFMergeTree() { }

		public bool Initalized { get; private set; } = false;

		public int Count => MergeTree.Count;

		public async void Initialize()
		{
			if (Initalized) throw new InvalidOperationException("Initalize only once");

			Initalized = true;

			Add();

			OnPropertyChange("MergeTree");
		}

		public void Reset()
		{
			Initalized = false;

			MergeTree = new ObservableCollection<KeyValuePair<string, PDFMergeItem>>();
		}

//		public async void test(int i)
//		{
//			test2(i);
//		}
//
//		public async void test2(int i)
//		{
//			Thread.Sleep(100);
//
//			d.Invoke(() =>
//			{
//				MainWinManager.mainWin.pb1.Value = i;
//				MainWinManager.mainWin.pbStatus.Text = i.ToString();
//			});
//		}

		private void Add()
		{
			MainWinManager.mainWin.StatusBar.Value = 0;
			MainWinManager.mainWin.UpdateProgressStatus("Adding FileItems - Start");

			foreach (FileItem item in FileItems.Instance)
			{
				AddItem(item, 0, MergeTree);
			}

			MainWinManager.mainWin.UpdateProgressStatus("Adding FileItems - Complete");
		}


		private void AddItem(FileItem item, int depth,
			ObservableCollection<KeyValuePair<string, PDFMergeItem>> mtree)
		{
			MainWinManager.mainWin.AddToProgress(1);

			// does the outline path have sub-outlines
			if (item.OutlinePath.FolderNamesCount - 1 >= depth)
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
				// do not add twice
				if (ContainsKey(item.OutlinePath.FileWithoutExtension, mtree)) return;


				PDFMergeItem pmi = new PDFMergeItem(
					item.OutlinePath.FileWithoutExtension,
					TreeNodeType.LEAF, 0, depth, item);

				this.PropertyChanged += pmi.E;

				// does not have sub-outlines
				// add the leaf to the tree
				mtree.Add(new KeyValuePair<string, PDFMergeItem>(
					item.OutlinePath.FileWithoutExtension, pmi));

//				OnPropertyChange("E");
				
//				mtree.Add(new KeyValuePair<string, PDFMergeItem>(
//					item.OutlinePath.FileWithoutExtension,
//					new PDFMergeItem(
//						item.OutlinePath.FileWithoutExtension,
//						TreeNodeType.LEAF,0, depth,item)));
			}
		}

		private ObservableCollection<KeyValuePair<string, PDFMergeItem>> FindBranch(FileItem item,
			ObservableCollection<KeyValuePair<string, PDFMergeItem>> mtree, int depth, out int finalDepth)
		{
			// does the outline path have sub-outlines?
			if (item.OutlinePath.FolderNamesCount - 1 >= depth)
			{
				// item has sub-outlines
				string key = item.OutlinePath.FolderNames[depth];

				PDFMergeItem innerItem;

				// the sub-outline needs to exist?
				if (TryGetValue(key, mtree, out innerItem))
				{
					// sub-outline does exist 
					// check next level
					return FindBranch(item, innerItem.MergeItems, ++depth, out finalDepth);
				} 

				// sub-outline not found - this is a failure;
				finalDepth = -1;
				return null;
			} 

			// no more sub-outlines
			// return the final branch
			finalDepth = depth;

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

		public void testevent()
		{
			OnPropertyChange("E");
			OnPropertyChange("Y");
			OnPropertyChange("Z");
		}

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


// selection routines
// scenarios
// 1 leaf selected - need to check its parent - partially or fully check - then check its parent, etc.
// 2 leaf de-selected - need to check its parent - partially or not check - then check its parent, etc.
// 3 branch selected (currently not selected) - select all below - need to check its parent - partially or fully check - then check its parent.
// 4 branch de-selected (currently selected) - de-select all below - need to check its parent - partially or fully check - then check its parent.
// 5 branch selected (currently partially selected) - change per 3
/*
☐ not checked
☑ checked
☒ partially checked

this only applies where the branch is partial
	op-1				op-2						op-3		
	➔ ✔ ➔			➔ ✔ ➔						➔ ✔ ➔
☒ a (partial)	☑ a	 (chk_was_partial)	☐ a (unchk_was_partial)		☒ a (partial)
☑ b	 (checked) 	☑ b	 (checked)			☐ b	 (unchk_was_checked)	☑ b (checked)
☒ c	 (partial) 	☑ c	 (chk_was_partial)	☐ c	 (unchk_was_partial)	☒ c (partial)
☐ d	 (uncheck)	☑ d	 (chk_was_uncheck)	☐ d	 (uncheck)				☐ d (uncheck)

procedures:
+ flag - tri-state = true
if (op-1)
	(tri-state = true)
	(partial) ➔ (chk_was_partial)
	(checked) ➔ (checked)
	(uncheck) ➔ (chk_was_uncheck)

if (op-2 & tri-state)
	(chk_was_partial) ➔ (unchk_was_partial)	
	(checked)		  ➔ (unchk_was_checked)
	(chk_was_uncheck) ➔ (unchecked)

if (op-3 & tri-state)
	(unchk_was_partial) ➔ (partial) 
	(unchk_was_checked) ➔ (checked)
	(unchecked)		    ➔ (uncheck)

tri-check process
	(partial) 10          ➔ (chk_was_partial) 11        ➔ (unchk_was_partial) -11      ➔ (partial) 10
	(checked) 2           ➔ (checked)  2                ➔ (unchk_was_checked) -2       ➔ (checked) 2
	(uncheck) 0           ➔ (chk_was_uncheck) 1         ➔ (uncheck)  0                 ➔ (uncheck) 0
	(chk_was_partial) = 11
	(chk_was_uncheck) = 1
	(unchk_was_partial) = -11
	(unchk_was_checked) = -2


	routine: bool? ChildBranchSelStatus: determine the status of the child branch
		return checked   == all checked on the child branch
		       unchecked == all uncheck on the child branch or no child branch
			   partial   == at least one uncheck and one checked on the child branch


leaf status == the checked status of the leaf's siblings (including current item, leaf or branch)

unchecking / checking a leaf

uncheck leaf (uncheck leaf first) -> verify parent

check leaf (check leaf first) -> verify parent


unchecking / checking a parent (note - lose focus, must set all tri-checks to final checks)

tri-uncheck a parent (tri-uncheck parent first) -> tri-uncheck all children -> verify grandparent

tri-check a parent (tri-check parent first) -> tri-check all children -> verify grandparent

tri-partial a parent (tri-partial parent first) -> tri-partial all children -> verify grandparent


verify parent (same as verify grandparent)

determine ChildBranchSelStatus

ChildBranchSelStatus is unchecked, parent to unchecked -> verify grandparent
ChildBranchSelStatus is checked, parent to checked -> verify grandparent
ChildBranchSelStatus is partial, parent to partial -> verify grandparent

*/
