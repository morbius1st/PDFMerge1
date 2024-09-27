// #define DML0 // not yet used
// #define DML1 // do not use here ** defined in properties *** start and end
// #define DML2 // turns on or off bool flags / button enable flags only / listbox idex set
// #define DML3 // various status messages
// #define DML4 // update status status messages
// #define DML5 // orator routines
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
using DebugCode;

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
		#if DML1
			DM.Start0();
		#endif

			Orator.Listen("toClassify", OnGetAnnouncement);
			toParentAnnounce =	Orator.GetAnnouncer(this, "fromClassify", "");

		#if DML1
			DM.End0();
		#endif
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

		/// <summary>
		/// sets internal variables
		/// configs to use or not, building / phase
		/// </summary>
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

		/// <summary>
		/// config's async vars
		/// </summary>
		public void ConfigureAsyncReporting(IProgress<double> pbDouble)
		{
			this.pbProgress = pbDouble;

			Status = ClassifyStatus.CONFIGURED2;
		}

		/// <summary>
		/// init merg item list
		/// init async lock
		/// runs the recursive preprocess routine for each child
		/// </summary>
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

		/// <summary>
		/// setup item processing.
		/// start the item processing task.
		/// runs the task and awaits it to complete
		/// </summary>
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

		/// <summary>
		/// runs through the list of files and classify the file.
		/// includes extra operations relative to being in a separate task
		/// </summary>
		private void processFiles3()
		{
			Thread.CurrentThread.Name = "process files";

			Status = ClassifyStatus.RUNNING;

			int fileCount = 1;

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

		/// <summary>
		/// process a single sheet pdf and apply the classification criteria.
		/// add matches to the list within the node
		/// </summary>
		private bool classify3(TreeNode treeNode, FilePath<FileNameSheetPdf> sheetFilePath, int depth)
		{
			bool localMatchFlag;
			bool matchFlag = false;

			foreach (TreeNode childNode in treeNode.Children)
			{
				// Thread.Sleep(100);

				CompareOperations.Depth = depth;

				localMatchFlag = false;

				RaiseTreeNodeChangeEvent(new TreeNodeChangeEventArgs(childNode));

				if (CompareOperations.Compare2(sheetFilePath.FileNameObject, childNode.Item.CompareOps))
				{
					// may be a bug here
					// if item matches but is the top level item,
					// localMatchFlag match is false and the item will not be added
					// to the merge list
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

		/// <summary>
		/// preprocess each child in the tree (recursive)
		/// init each child's merge item list
		/// init each child's async lock
		/// init collection sync used for async operations
		/// </summary>
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

		private void UpdateNonApplicableProperties()
		{
			OnPropertyChange(nameof(NonApplicableFiles));
			OnPropertyChange(nameof(NonApplicableFilesTotalCount));
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
			if (nonApplicableFiles == null) return 0;

			int count = 0;

			foreach (KeyValuePair<string, List<FilePath<FileNameSheetPdf>>> kvp in nonApplicableFiles)
			{
				count += kvp.Value.Count;
			}

			return count;
		}

		private void setCancelToken()
		{
			cancelTokenSrc = new CancellationTokenSource();

			cancelToken = cancelTokenSrc.Token;
		}

	#endregion

	#region event consuming

		private void OnGetAnnouncement(object sender, object value)
		{
		#if DML5
			DM.Start0();
		#endif
			if (value is bool) displayDebugMsgs = (bool) value;

			toParentAnnounce.Announce("got announcement| " +
				(displayDebugMsgs ? "display debug messages\n" : "do not display debug messages\n"));
		#if DML5
			DM.End0();
		#endif
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