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

		/// <summary>
		/// Process the list of sheet files, using the rules in the<br/>
		/// ClassificationFile, and place the file into the<br/>
		/// Classification tree
		/// </summary>
		public void Process()
		{
			if (!isConfigured) return;

			// await Task.Run(() => { classifyFiles(); });

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
			preProcessMergeItems();

			processFiles();

			treeBase.CountExtItems();
		}

		private void preProcessMergeItems()
		{
			treeBase.Item.MergeItems = new ObservableCollection<MergeItem>();

			foreach (TreeNode childNode in treeBase.Children)
			{
				childNode.Item.MergeItems = new ObservableCollection<MergeItem>();
			}

		}

		private void processFiles()
		{
			foreach (FilePath<FileNameSheetPdf> file in fileList.Files)
			{
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

				classify(treeBase, file, 1);
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