// #define SHOW

#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.SampleFileSupport;
using AndyShared.Support;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  9/29/2020 7:06:05 AM


namespace AndyShared.MergeSupport
{
	/// <summary>
	/// Process the list of PDF sheet files and classify<br/>
	/// based on the ClassifFile's Classification rules<br/>
	/// that are contained in the tree: treebase
	/// </summary>
	public class Classify : INotifyPropertyChanged //, IEnumerable<MergeItem> //, IEnumerable<TreeNode>
	{
	#region private fields

		private Orator.ConfRoom.Announcer toParentAnnounce;

		private bool displayDebugMsgs = false;

		// used as a filter, any sheet that is assigned to another
		// phase-building will not be included. However, the non-applicable
		// phase-uilding files will be tracked
		private string assignedPhBldg = null;

		// the files do have phase-building information and this needs to be taken
		// into account
		private bool usePhBldg = false;
		private bool isConfigured;

		private Dictionary<string, List<FilePath<FileNameSheetPdf>>> nonApplicableFiles;

		private BaseOfTree treeBase;
		private SheetFileList fileList;

		private List<Object> mergeTreeLocks;

		private Progress<double> pbDouble;
		// private Progress<string> pbString;

	#endregion

	#region ctor

		public Classify()
		{
			Orator.Listen("toClassify", OnGetAnnouncement);
			toParentAnnounce =	Orator.GetAnnouncer(this, "fromClassify", "");

			isConfigured = false;
			this.treeBase = null;
			assignedPhBldg = null;
		}

	#endregion

	#region public properties

		public Dictionary<string, List<FilePath<FileNameSheetPdf>>> NonApplicableFiles => nonApplicableFiles;

		public bool HasNonApplicableFiles => NonApplicableFilesTotalCount > 0;

		public int NonApplicableFilesTotalCount => countIgnoredFiles();

		public string PhaseBuilding => assignedPhBldg;

		public bool UsePhaseBulding => usePhBldg;

		public bool IsConfigured => isConfigured;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Configure(BaseOfTree treeBase, SheetFileList fileList)
		{
			isConfigured = false;

			this.treeBase = treeBase;
			this.fileList = fileList;

			if (treeBase != null && treeBase.HasChildren &&
				fileList != null && fileList.Files.Count > 0)
			{
				assignedPhBldg = fileList.Building;

				if (!assignedPhBldg.IsVoid())
				{
					usePhBldg = true;
					nonApplicableFiles = new Dictionary<string, List<FilePath<FileNameSheetPdf>>>();
				}

				isConfigured = true;
			}

			return isConfigured;
		}

		public void ConfigureAsyncReporting(Progress<double> pbDouble) // , Progress<string> pbString)
		{
			this.pbDouble = pbDouble;
			// this.pbString = pbString;
		}

		/// <summary>
		/// Process the list of sheet files, using the rules in the<br/>
		/// ClassificationFile, and place the file into the<br/>
		/// Classification tree
		/// </summary>
		public void Process()
		{
			if (!isConfigured) return;

			classifyFiles();

			UpdateProperties();
		}

	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("NonApplicableFiles");
			OnPropertyChange("NonApplicableFilesTotalCount");
		}

		private void classifyFiles()
		{
			TreeNode t = treeBase;

			preProcessMergeItems();

			processFiles();

			// await Task.Run(processFiles);

			treeBase.CountExtItems();
		}

		// go thorugh the whole tree and prep for a new set of merge items
		// 1. reset the merge items collection
		private void preProcessMergeItems()
		{
			treeBase.Item.MergeItems = new ObservableCollection<MergeItem>();

			preProcessMI(treeBase);
		}

		private void preProcessMI(TreeNode parent)
		{
			foreach (TreeNode child in parent.Children)
			{
				if (child.HasChildren) preProcessMI(child);

				child.Item.MergeItems = new ObservableCollection<MergeItem>();
			}
		}

		private void processFiles()
		{
			int fileCount = 0;
			string msg;

			foreach (FilePath<FileNameSheetPdf> file in fileList.Files)
			{
				// msg = "Processing file| " + fileCount.ToString("000");
				// ((IProgress<string>) pbString)?.Report(msg);
				((IProgress<double>) pbDouble)?.Report(fileCount++);

				Thread.Sleep(10);

				// Debug.WriteLine("*** Processing| " + file.FileNameObject.SheetID);
				// raise an event when the file being process changes - allow the
				// parent to do something if needed
				RaiseOnFileChangeEvent(new FileChangeEventArgs(file));

				if (usePhBldg)
				{
					if ((!file.FileNameObject.PhaseBldg?.Equals(assignedPhBldg)) ?? false)
					{
						if (displayDebugMsgs)
							toParentAnnounce.Announce("ignored file| " +
								file.FileNameObject.SheetNumber + "\n");

						addNonApplicableFile(file);
						continue;
					}
				}

				if (displayDebugMsgs)
					toParentAnnounce.Announce("\nprocess file| " +
						file.FileNameObject.SheetNumber + "\n");

				classify2(treeBase, file, 0);
			}
		}

		/// <summary>
		/// classify each sheet file against the list of all<br/>
		/// criteria from treebase down
		/// </summary>
		/// <param name="treeNode"></param>
		/// <param name="sheetFilePath"></param>
		/// <param name="depth"></param>
		/// <returns></returns>
		private bool classify2(TreeNode treeNode, FilePath<FileNameSheetPdf> sheetFilePath, int depth)
		{
			bool localMatchFlag;
			bool matchFlag = false;

		#if SHOW
			if (depth == 0) 
			{
				Debug.WriteLine("\n\n*** Classifying| " + sheetFilePath.FileNameObject.SheetID);
				Debug.Write("\n");
			}
		#endif


			// run through the tree and look for classify matches
			foreach (TreeNode childNode in treeNode.Children)
			{
			#if SHOW
				Debug.WriteLine("\t\t".Repeat(depth) + "Classifying against| " + childNode.Item.Title);
			#endif

				CompareOperations.Depth = depth;

				localMatchFlag = false;

				RaiseTreeNodeChangeEvent(new TreeNodeChangeEventArgs(childNode));

				if (CompareOperations.Compare2(sheetFilePath.FileNameObject, childNode.Item.CompareOps))
				{
				#if SHOW
					Debug.WriteLine("***"+"\t\t".Repeat(depth) + "\tClassifying| compare is| true");
				#endif

					if (childNode.HasChildren)
					{
					#if SHOW
						Debug.WriteLine("\t\t".Repeat(depth) + "\tClassifying| check children");
					#endif

						localMatchFlag = classify2(childNode, sheetFilePath, depth + 1);
					}


					if (!localMatchFlag)
					{
					#if SHOW
						// Debug.WriteLine("\t\t".Repeat(depth) + "\t\tClassifying| ************************");
						Debug.WriteLine("***" + "\t\t".Repeat(depth) + "\t\tClassifying| *** save merge item to| " + childNode.Item.Title);
						// Debug.WriteLine("\t\t".Repeat(depth) + "\t\tClassifying| ************************");
					#endif

						MergeItem mi = new MergeItem(0, sheetFilePath);
						childNode.Item.MergeItems.Add(mi);
						childNode.Item.UpdateMergeProperties();
					}

					// now categorized
					matchFlag = true;
				}
			}

			// worst case, at depth zero and not matchflag is false
			if (!matchFlag && depth == 0)
			{
			#if SHOW
				Debug.WriteLine("***" + "\t\t".Repeat(depth) + "\t\tClassifying| *** save merge item to PARENT| " + treeNode.Item.Title);
			#endif
				MergeItem mi = new MergeItem(0, sheetFilePath);
				treeNode.Item.MergeItems.Add(mi);
				treeNode.Item.UpdateMergeProperties();

				matchFlag = true;
			}

			// return true when categorized
			// else return false
			return matchFlag;
		}


		/*

		/// <summary>
		/// classify each sheet file against the list of all<br/>
		/// criteria from treebase down
		/// </summary>
		/// <param name="treeNode"></param>
		/// <param name="sheetFilePath"></param>
		/// <param name="depth"></param>
		/// <returns></returns>
		private bool classify(TreeNode treeNode,
			FilePath<FileNameSheetPdf> sheetFilePath, int depth)
		{
			bool matchFlag = false;

			if (displayDebugMsgs)
				toParentAnnounce.Announce("classifying|\n");

			// run through the tree and look for classify matches
			foreach (TreeNode childNode in treeNode.Children)
			{
				RaiseTreeNodeChangeEvent(new TreeNodeChangeEventArgs(childNode));
				// set flag = false;
				// scan through each node and determine if the sheetfile matches one of the nodes rules
				// if it matches:
				//    if there are no children treenodes, set matchflag = false
				//    if there are children, scan the next level
				//        if the next level returns true, sheetfile was classified in a lower level, set flag = return value == true
				//        if the next level return false (this will not happen)
				//
				// if flag = false, it does not match, add to this level's sheet category.mergeitems / return true (was classified)

				// does the current sheetfile match the current
				// if sheetfile matches childclassfnode.sheetcategory.compareops

				if (CompareOperations.Compare(sheetFilePath.FileNameObject[depth], childNode.Item.CompareOps))
				{
					matchFlag = classify(childNode, sheetFilePath, depth + 1);

					break;
				}
			}

			// if match flag is already true, do not do the following
			// which will return true
			// else, do the following which will also cause true to be returned
			// false cannot be returned
			if (!matchFlag)
			{
				if (displayDebugMsgs)
					toParentAnnounce.Announce("adding to| " +
						treeNode.Item.Title + "\n\n");

				MergeItem mi = new MergeItem(0, sheetFilePath);

				treeNode.Item.MergeItems.Add(mi);
				treeNode.Item.UpdateMergeProperties();
				// treeNode.UpdateProperties();

				matchFlag = true;
			}

			return matchFlag;
		}

		*/

		/*
		private async void classifyFiles3()
		{
			TreeNode t = treeBase;

			preProcessMergeItems3(true);

			// processFiles3();

			Task.Run(() => { processFiles3(); });

			// treeBase.CountExtItems();

			preProcessMergeItems3(false);


			// treeBase.UpdateProperties();
			// treeBase.Item.UpdateProperties();
			//
			// foreach (TreeNode child in treeBase.Children)
			// {
			// 	child.UpdateProperties();
			// 	child.Item.UpdateProperties();
			// }
		}

		// go thorugh the whole tree and prep for a new set of merge items
		// 1. reset the merge items collection
		// 2. create a collection lock
		// 3. save the merge item lock's idx in the collection
		private void preProcessMergeItems3(bool preOrPostProcess)
		{
			if (preOrPostProcess)
			{
				mergeTreeLocks = new List<object>();
				treeBase.Item.MergeItems = new ObservableCollection<MergeItem>();
				treeBase.Item.MergeItemLockIdx = mergeTreeLocks.Count;

				mergeTreeLocks.Add(new object());

				BindingOperations.EnableCollectionSynchronization(treeBase.Item.MergeItems,
					mergeTreeLocks[treeBase.Item.MergeItemLockIdx]);
			}
			else
			{
				treeBase.Item.UpdateMergeProperties();
				treeBase.Item.UpdateProperties();
				treeBase.UpdateProperties();

				BindingOperations.DisableCollectionSynchronization(treeBase.Item.MergeItems);
			}

			preProcessMI3(treeBase, preOrPostProcess);
		}

		private void preProcessMI3(TreeNode parent, bool preOrPostProcess)
		{
			if (preOrPostProcess)
			{
				parent.ChildrenCollectLockIdx = mergeTreeLocks.Count;
				mergeTreeLocks.Add(new object());

				BindingOperations.EnableCollectionSynchronization(parent.Children,
					mergeTreeLocks[parent.ChildrenCollectLockIdx]);
			}
			else
			{
				BindingOperations.DisableCollectionSynchronization(parent.Children);
			}

			foreach (TreeNode child in parent.Children)
			{
				if (child.HasChildren) preProcessMI3(child, preOrPostProcess);

				if (preOrPostProcess)
				{
					child.Item.MergeItems = new ObservableCollection<MergeItem>();

					// since add is always to the end of the list, the count here
					// is the index of the future addition
					child.Item.MergeItemLockIdx = mergeTreeLocks.Count;

					// add an object to be a future lock
					mergeTreeLocks.Add(new object());

					BindingOperations.EnableCollectionSynchronization(child.Item.MergeItems,
						mergeTreeLocks[child.Item.MergeItemLockIdx]);
				}
				else
				{
					BindingOperations.DisableCollectionSynchronization(child.Item.MergeItems);

					child.Item.UpdateMergeProperties();
					child.Item.UpdateProperties();
					child.UpdateProperties();
				}
			}
		}


		private async void processFiles3()
		{
			int fileCount = 0;

			foreach (FilePath<FileNameSheetPdf> file in fileList.Files)
			{
				((IProgress<double>) pbDouble)?.Report(fileCount++);

				RaiseOnFileChangeEvent(new FileChangeEventArgs(file));

				if (usePhBldg)
				{
					if ((!file.FileNameObject.PhaseBldg?.Equals(assignedPhBldg)) ?? false)
					{
						addNonApplicableFile(file);
						continue;
					}
				}

				// await Task.Run(() => { classify3(treeBase, file, 0); });

				classify3(treeBase, file, 0);
			}
		}

		/// <summary>
		/// classify each sheet file against the list of all<br/>
		/// criteria from treebase down
		/// </summary>
		/// <param name="treeNode"></param>
		/// <param name="sheetFilePath"></param>
		/// <param name="depth"></param>
		/// <returns></returns>
		private bool classify3(TreeNode treeNode, FilePath<FileNameSheetPdf> sheetFilePath, int depth)
		{
			bool localMatchFlag;
			bool matchFlag = false;

		#if SHOW
			if (depth == 0) 
			{
				Debug.WriteLine("\n\n*** Classifying| " + sheetFilePath.FileNameObject.SheetID);
				Debug.Write("\n");
			}
		#endif

			// run through the tree and look for classify matches
			foreach (TreeNode childNode in treeNode.Children)
			{
			#if SHOW
				Debug.WriteLine("\t\t".Repeat(depth) + "Classifying against| " + childNode.Item.Title);
			#endif

				CompareOperations.Depth = depth;

				localMatchFlag = false;

				RaiseTreeNodeChangeEvent(new TreeNodeChangeEventArgs(childNode));

				if (CompareOperations.Compare2(sheetFilePath.FileNameObject, childNode.Item.CompareOps))
				{
				#if SHOW
					Debug.WriteLine("***"+"\t\t".Repeat(depth) + "\tClassifying| compare is| true");
				#endif

					if (childNode.HasChildren)
					{
					#if SHOW
						Debug.WriteLine("\t\t".Repeat(depth) + "\tClassifying| check children");
					#endif

						localMatchFlag = classify2(childNode, sheetFilePath, depth + 1);
					}

					if (!localMatchFlag)
					{
					#if SHOW
						// Debug.WriteLine("\t\t".Repeat(depth) + "\t\tClassifying| ************************");
						Debug.WriteLine("***" + "\t\t".Repeat(depth) + "\t\tClassifying| *** save merge item to| " + childNode.Item.Title);
						// Debug.WriteLine("\t\t".Repeat(depth) + "\t\tClassifying| ************************");
					#endif

						lock (mergeTreeLocks[childNode.ChildrenCollectLockIdx])
						{
							lock (mergeTreeLocks[childNode.Item.MergeItemLockIdx])
							{
								MergeItem mi = new MergeItem(0, sheetFilePath);
								childNode.Item.MergeItems.Add(mi);
							}
						}
					}

					// now categorized
					matchFlag = true;
				}
			}

			// worst case, at depth zero and not matchflag is false
			if (!matchFlag && depth == 0)
			{
			#if SHOW
				Debug.WriteLine("***" + "\t\t".Repeat(depth) + "\t\tClassifying| *** save merge item to PARENT| " + treeNode.Item.Title);
			#endif

				lock (mergeTreeLocks[treeNode.ChildrenCollectLockIdx])
				{
					lock (mergeTreeLocks[treeNode.Item.MergeItemLockIdx])
					{
						MergeItem mi = new MergeItem(0, sheetFilePath);
						treeNode.Item.MergeItems.Add(mi);
					}
				}

				matchFlag = true;
			}

			// return true when categorized
			// else return false
			return matchFlag;
		}
		*/


		private void addNonApplicableFile(FilePath<FileNameSheetPdf> file)
		{
			string phBld = file.FileNameObject.PhaseBldg;

			if (!nonApplicableFiles.ContainsKey(phBld))
			{
				nonApplicableFiles.Add(phBld, new List<FilePath<FileNameSheetPdf>>());
			}

			nonApplicableFiles[phBld].Add(file);
		}

		private int countIgnoredFiles()
		{
			int count = 0;

			foreach (KeyValuePair<string, List<FilePath<FileNameSheetPdf>>> kvp in nonApplicableFiles)
			{
				count += kvp.Value.Count;
			}

			return count;
		}

		public string FormatMergeList(TreeNode node)
		{
			if (node.ExtItemCountLast == 0) return null;

			StringBuilder sb = new StringBuilder();

			sb.Append(" ".Repeat(node.Depth * 2));
			sb.Append("\\").AppendLine(node.Item.Title);

			if (node.ItemCount > 0)
			{
				foreach (MergeItem item in node.Item.MergeItems)
				{
					string shtnum = item.FilePath.FileNameObject.SheetNumber;

					if (!shtnum.IsVoid())
					{
						sb.Append(" ".Repeat(node.Depth * 2 + 4));
						sb.Append(">");
						sb.Append(item.FilePath.FileNameObject.SheetNumber);
						sb.AppendLine();
					}
				}
			}

			if (node.ChildCount > 0)
			{
				foreach (TreeNode childNode in node.Children)
				{
					string result = FormatMergeList(childNode);

					if (!result.IsVoid())
					{
						sb.Append(result);
					}
				}
			}

			return sb.Length > 0 ? sb.ToString() : null;
		}

		private	IEnumerable<MergeItem> GetNtMergeItem(TreeNode node)
		{
			if (node.ExtItemCountLast == 0) yield return null;

			if (node.ItemCount > 0)
			{
				foreach (MergeItem mi in node.Item.MergeItems)
				{
					yield return  mi;
				}
			}

			if (node.ChildCount > 0)
			{
				foreach (TreeNode childNode in node.Children)
				{
					if (childNode.ExtItemCountLast > 0)
					{
						foreach (MergeItem mergeItem in GetNtMergeItem(childNode))
						{
							yield return mergeItem;
						}
					}
				}
			}

			yield return null;
		}

		private IEnumerable<TreeNode> GetMergeNodes(TreeNode node)
		{
			if (node.ExtItemCountLast == 0) yield return null;

			yield return node;

			if (node.ChildCount > 0)
			{
				foreach (TreeNode childNode in node.Children)
				{
					foreach (TreeNode mergeNode in GetMergeNodes(childNode))
					{
						if ((mergeNode?.ExtItemCountLast ?? 0) > 0)
						{
							yield return mergeNode;
						}
					}
				}
			}

			yield return null;
		}

	#endregion

	#region event consuming

		private void OnGetAnnouncement(object sender, object value)
		{
			if (value is bool) displayDebugMsgs = (bool) value;

			toParentAnnounce.Announce("got announcement| " +
				(displayDebugMsgs ? "display debug messages\n" : "do not display debug messages\n"));
		}

	#endregion

	#region event publishing

		public delegate void OnFileChangeEventHandler(object sender, FileChangeEventArgs e);

		public event Classify.OnFileChangeEventHandler OnFileChange;

		protected virtual void RaiseOnFileChangeEvent(FileChangeEventArgs e)
		{
			OnFileChange?.Invoke(this, e);
		}

		public delegate void TreeNodeChangeEventHandler(object sender, TreeNodeChangeEventArgs e);

		public event Classify.TreeNodeChangeEventHandler OnTreeNodeChange;

		protected virtual void RaiseTreeNodeChangeEvent(TreeNodeChangeEventArgs e)
		{
			OnTreeNodeChange?.Invoke(this, e);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public IEnumerable<MergeItem> EnumerateMergeItems()
		{
			foreach (MergeItem mi in GetNtMergeItem(treeBase))
			{
				if (mi != null) yield return mi;
			}

			yield break;
		}

		public IEnumerable<TreeNode> EnumerateMergeNodes()
		{
			foreach (TreeNode node in GetMergeNodes(treeBase))
			{
				if (node != null) yield return node;
			}


			yield break;
		}

		public override string ToString()
		{
			return "this is Classify";
		}

	#endregion
	}

	public class FileChangeEventArgs : EventArgs
	{
		public FilePath<FileNameSheetPdf> SheetFile { get; private set; }

		public FileChangeEventArgs(FilePath<FileNameSheetPdf> sheetFile)
		{
			SheetFile = sheetFile;
		}
	}

	public class TreeNodeChangeEventArgs : EventArgs
	{
		public TreeNode TreeNode { get; private set; }

		public TreeNodeChangeEventArgs(TreeNode treeNode)
		{
			TreeNode = treeNode;
		}
	}
}