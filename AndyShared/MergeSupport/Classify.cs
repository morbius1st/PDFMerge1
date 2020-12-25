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
	public enum ClassifyStatus
	{
		CREATED,
		CONFIGURED1,
		CONFIGURED2,
		PREPROCESSING,
		PREPROCESSED,
		RUNNING,
		PAUSED,
		CANCELED,
		COMPLETE,
		SUCESSFUL,
		INVALID
	}


	/// <summary>
	/// Process the list of PDF sheet files and classify<br/>
	/// based on the ClassifFile's Classification rules<br/>
	/// that are contained in the tree: treebase
	/// </summary>
	public class Classify : INotifyPropertyChanged
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

		private Dictionary<string, List<FilePath<FileNameSheetPdf>>> nonApplicableFiles;

		private BaseOfTree treeBase;
		private SheetFileList fileList;

		private CancellationTokenSource cancelTokenSrc;
		private CancellationToken cancelToken;

		private List<Object> lockList;

		private IProgress<double> pbProgress;

		private Task task;

		private ClassifyStatus classifyStatus = ClassifyStatus.CREATED;

	#endregion

	#region ctor

		public Classify()
		{
			Orator.Listen("toClassify", OnGetAnnouncement);
			toParentAnnounce =	Orator.GetAnnouncer(this, "fromClassify", "");
		}

	#endregion

	#region public properties

		public Dictionary<string, List<FilePath<FileNameSheetPdf>>> NonApplicableFiles => nonApplicableFiles;

		public bool HasNonApplicableFiles => NonApplicableFilesTotalCount > 0;

		public int NonApplicableFilesTotalCount => countIgnoredFiles();

		public string PhaseBuilding => assignedPhBldg;
		
		public bool UsePhaseBulding => usePhBldg;

		public ClassifyStatus Status
		{
			get => classifyStatus;
			set
			{
				classifyStatus = value;
				OnPropertyChange();
			}
		}

		public TaskStatus TaskStatus => task?.Status ?? TaskStatus.WaitingToRun;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Configure(BaseOfTree treeBase, SheetFileList fileList)
		{
			if (treeBase == null || !treeBase.HasChildren ||
				fileList == null || fileList.Files.Count == 0) return false;

			this.treeBase = treeBase;
			this.fileList = fileList;

			assignedPhBldg = fileList.Building;

			if (!assignedPhBldg.IsVoid())
			{
				usePhBldg = true;
				nonApplicableFiles = new Dictionary<string, List<FilePath<FileNameSheetPdf>>>();
			}

			Status = ClassifyStatus.CONFIGURED1;

			return true;
		}

		public void ConfigureAsyncReporting(IProgress<double> pbDouble)
		{
			this.pbProgress = pbDouble;

			Status = ClassifyStatus.CONFIGURED2;
		}

		public void PreProcess()
		{
			if (Status != ClassifyStatus.CONFIGURED1
				&& Status != ClassifyStatus.CONFIGURED2) return;

			treeBase.Item.MergeItems = new ObservableCollection<MergeItem>();

			lockList = new List<object>();

			Status = ClassifyStatus.PREPROCESSING;

			preProcessFiles(treeBase);

			Status = ClassifyStatus.PREPROCESSED;
		}

	#endregion

	#region private methods

		public async void Process3()
		{
			if (Status != ClassifyStatus.PREPROCESSED) return;

			task?.Dispose();
			task = null;

			setCancelToken();

			task = Task.Run(() => { processFiles3(); }, cancelToken);

			await task;

			UpdateNonApplicableProperties();

			Status = ClassifyStatus.SUCESSFUL;

			RaiseOnClassifyCompletionEvent();

		}

		private void processFiles3()
		{
			Thread.CurrentThread.Name = "process files";

			Status = ClassifyStatus.RUNNING;

			int fileCount = 0;

			foreach (FilePath<FileNameSheetPdf> file in fileList.Files)
			{
				if (cancelToken.IsCancellationRequested)
				{
					Status = ClassifyStatus.CANCELED;
					break;
				}

				pbProgress?.Report(fileCount++);

				RaiseOnFileChangeEvent(new FileChangeEventArgs(file));

				if (usePhBldg)
				{
					if ((!file.FileNameObject.PhaseBldg?.Equals(assignedPhBldg)) ?? false)
					{
						addNonApplicableFile(file);
						continue;
					}
				}

				classify3(treeBase, file, 0);
			}

			Status = ClassifyStatus.COMPLETE;
		}

		private bool classify3(TreeNode treeNode, FilePath<FileNameSheetPdf> sheetFilePath, int depth)
		{
			bool localMatchFlag;
			bool matchFlag = false;

			foreach (TreeNode childNode in treeNode.Children)
			{
				CompareOperations.Depth = depth;

				localMatchFlag = false;

				RaiseTreeNodeChangeEvent(new TreeNodeChangeEventArgs(childNode));

				if (CompareOperations.Compare2(sheetFilePath.FileNameObject, childNode.Item.CompareOps))
				{

					if (childNode.HasChildren)
					{
						localMatchFlag = classify3(childNode, sheetFilePath, depth + 1);
					}

					if (!localMatchFlag)
					{
						lock(lockList[childNode.Item.MergeItemLockIdx])
						{
							MergeItem mi = new MergeItem(0, sheetFilePath);
							childNode.Item.MergeItems.Add(mi);
							childNode.Item.UpdateMergeProperties();
							
						}
					}

					matchFlag = true;
				}
			}

			// worst case, at depth zero and not matchflag is false
			if (!matchFlag && depth == 0)
			{
				lock(lockList[treeNode.Item.MergeItemLockIdx])
				{
					MergeItem mi = new MergeItem(0, sheetFilePath);
					treeNode.Item.MergeItems.Add(mi);
					treeNode.Item.UpdateMergeProperties();
				}

				matchFlag = true;
			}

			// return true when categorized
			// else return false
			return matchFlag;
		}

		private void UpdateNonApplicableProperties()
		{
			OnPropertyChange(nameof(NonApplicableFiles));
			OnPropertyChange(nameof(NonApplicableFilesTotalCount));
		}

		private void preProcessFiles(TreeNode parent)
		{
			foreach (TreeNode child in parent.Children)
			{
				if (child.HasChildren) preProcessFiles(child);

				child.Item.MergeItems = new ObservableCollection<MergeItem>();
				child.Item.MergeItemLockIdx = lockList.Count;
				
				object loc = new object();

				BindingOperations.EnableCollectionSynchronization(child.Item.MergeItems, loc);

				lockList.Add(loc);
			}
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

		// public string FormatMergeList(TreeNode node)
		// {
		// 	if (node.ExtMergeItemCount == 0) return null;
		//
		// 	StringBuilder sb = new StringBuilder();
		//
		// 	sb.Append(" ".Repeat(node.Depth * 2));
		// 	sb.Append("\\").AppendLine(node.Item.Title);
		//
		// 	if (node.ItemCount > 0)
		// 	{
		// 		foreach (MergeItem item in node.Item.MergeItems)
		// 		{
		// 			string shtnum = item.FilePath.FileNameObject.SheetNumber;
		//
		// 			if (!shtnum.IsVoid())
		// 			{
		// 				sb.Append(" ".Repeat(node.Depth * 2 + 4));
		// 				sb.Append(">");
		// 				sb.Append(item.FilePath.FileNameObject.SheetNumber);
		// 				sb.AppendLine();
		// 			}
		// 		}
		// 	}
		//
		// 	if (node.ChildCount > 0)
		// 	{
		// 		foreach (TreeNode childNode in node.Children)
		// 		{
		// 			string result = FormatMergeList(childNode);
		//
		// 			if (!result.IsVoid())
		// 			{
		// 				sb.Append(result);
		// 			}
		// 		}
		// 	}
		//
		// 	return sb.Length > 0 ? sb.ToString() : null;
		// }

		// private	IEnumerable<MergeItem> GetNtMergeItem(TreeNode node)
		// {
		// 	if (node.ExtMergeItemCount == 0) yield return null;
		//
		// 	if (node.ItemCount > 0)
		// 	{
		// 		foreach (MergeItem mi in node.Item.MergeItems)
		// 		{
		// 			yield return  mi;
		// 		}
		// 	}
		//
		// 	if (node.ChildCount > 0)
		// 	{
		// 		foreach (TreeNode childNode in node.Children)
		// 		{
		// 			if (childNode.ExtMergeItemCount > 0)
		// 			{
		// 				foreach (MergeItem mergeItem in GetNtMergeItem(childNode))
		// 				{
		// 					yield return mergeItem;
		// 				}
		// 			}
		// 		}
		// 	}
		//
		// 	yield return null;
		// }

		// private IEnumerable<TreeNode> GetMergeNodes(TreeNode node)
		// {
		// 	if (node.ExtMergeItemCount == 0) yield return null;
		//
		// 	yield return node;
		//
		// 	if (node.ChildCount > 0)
		// 	{
		// 		foreach (TreeNode childNode in node.Children)
		// 		{
		// 			foreach (TreeNode mergeNode in GetMergeNodes(childNode))
		// 			{
		// 				if ((mergeNode?.ExtMergeItemCount ?? 0) > 0)
		// 				{
		// 					yield return mergeNode;
		// 				}
		// 			}
		// 		}
		// 	}
		//
		// 	yield return null;
		// }

		private void setCancelToken()
		{
			cancelTokenSrc = new CancellationTokenSource();

			cancelToken = cancelTokenSrc.Token;
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

		public delegate void OnClassifyCompletionEventHandler(object sender);

		public event OnClassifyCompletionEventHandler OnClassifyCompletion;

		protected virtual void RaiseOnClassifyCompletionEvent()
		{
			OnClassifyCompletion?.Invoke(this);
		}


	#region file change event

		public delegate void OnFileChangeEventHandler(object sender, FileChangeEventArgs e);

		public event OnFileChangeEventHandler OnFileChange;

		protected virtual void RaiseOnFileChangeEvent(FileChangeEventArgs e)
		{
			OnFileChange?.Invoke(this, e);
		}

	#endregion

	#region treenode change event

		public delegate void TreeNodeChangeEventHandler(object sender, TreeNodeChangeEventArgs e);

		public event TreeNodeChangeEventHandler OnTreeNodeChange;

		protected virtual void RaiseTreeNodeChangeEvent(TreeNodeChangeEventArgs e)
		{
			OnTreeNodeChange?.Invoke(this, e);
		}

	#endregion

	#region prop changed event

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#endregion

	#region system overrides

		// public IEnumerable<MergeItem> EnumerateMergeItems()
		// {
		// 	foreach (MergeItem mi in GetNtMergeItem(treeBase))
		// 	{
		// 		if (mi != null) yield return mi;
		// 	}
		//
		// 	yield break;
		// }
		//
		// public IEnumerable<TreeNode> EnumerateMergeNodes()
		// {
		// 	foreach (TreeNode node in GetMergeNodes(treeBase))
		// 	{
		// 		if (node != null) yield return node;
		// 	}
		//
		//
		// 	yield break;
		// }

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