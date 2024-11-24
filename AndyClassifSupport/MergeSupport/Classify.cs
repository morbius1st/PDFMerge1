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
using System.Windows.Documents;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FileSupport;
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

		// private Orator.ConfRoom.Announcer toParentAnnouncex;

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
		public SheetFileList fileList;

		private CancellationTokenSource cancelTokenSrc;
		private CancellationToken cancelToken;

		private List<Object> lockList;

		private IProgress<double> pbProgress;

		private Task task;

		private ClassifyStatus classifyStatus = ClassifyStatus.CREATED;

		private int compareCountTotal;
		private int compareCount;

		private TimeSpan ticks;

		private Classify me;

	#endregion

	#region ctor

		public Classify()
		{
		#if DML1
			DM.Start0();
		#endif

			me = this;

			// Orator.Listen("toClassify", OnGetAnnouncement);
			// toParentAnnounce =	Orator.GetAnnouncer(this, "fromClassify", "");

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

		public int CompareCountTotal => compareCountTotal;

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
			compareCountTotal = 0;

			if (Status != ClassifyStatus.CONFIGURED1
				&& Status != ClassifyStatus.CONFIGURED2) return;

			lockList = new List<object>();

			treeBase.Item.MergeItems = new ObservableCollection<MergeItem>();

			treeBase.Item.MergeItemLockIdx = lockList.Count;

			object loc = new object();

			BindingOperations.EnableCollectionSynchronization(treeBase.Item.MergeItems, loc);

			lockList.Add(loc);

			Status = ClassifyStatus.PREPROCESSING;

			preProcessFiles(treeBase);

			nonApplicableFiles = new Dictionary<string, List<FilePath<FileNameSheetPdf>>>();

			Status = ClassifyStatus.PREPROCESSED;
		}

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

			// Debug.WriteLine("Starting task");

			task = Task.Run(() => { processFiles3(); }, cancelToken);

			// Debug.WriteLine("awaiting task");

			await task;

			// Debug.WriteLine("ended task");

			postProcessMergeLists();

			UpdateNonApplicableProperties();

			Status = ClassifyStatus.SUCESSFUL;

			RaiseOnClassifyCompletionEvent();

			Debug.WriteLine($"\ntotal of {CompareCountTotal} comparisons");
			Debug.WriteLine($"total elapsed {ticks.TotalSeconds} seconds\n");
			

		}

		public void postProcessMergeLists()
		{
			sortMergeLists(treeBase);

			MrgSupport.ConsolidateMergeItems(treeBase);
		}

	#endregion

	#region private methods


		/// <summary>
		/// runs through the list of files and classify the file.
		/// includes extra operations relative to being in a separate task
		/// </summary>
		private void processFiles3()
		{
			Thread.CurrentThread.Name = "process files";

			Status = ClassifyStatus.RUNNING;

			int fileCount = 1;

			compareCountTotal = 0;

			Stopwatch a = new Stopwatch();

			a.Start();

		#if SHOW
			Debug.WriteLine($"\nbegin classify");
		#endif

			foreach (FilePath<FileNameSheetPdf> file in fileList.Files)
			{

				if (cancelToken.IsCancellationRequested)
				{
					Status = ClassifyStatus.CANCELED;
					break;
				}

				pbProgress?.Report(fileCount++);

				RaiseOnFileChangeEvent(new FileChangeEventArgs(file));

				if (file.FileNameObject.StatusCode != FileNameParseStatusCodes.SC_NONE)
				{
					addNonApplicableFile(file);

					continue;
				}


				if (usePhBldg)
				{
					if ((!file.FileNameObject.PhaseBldg?.Equals(assignedPhBldg)) ?? false)
					{
					#if SHOW
						Debug.WriteLine($"\n\add to non-applicable file list {file.FileName}");
					#endif

						addNonApplicableFile(file);
						continue;
					}
				}

				compareCount = 0;
				processDepth = 0;

				if (!classify3(treeBase, file, 0)) //, 0))
				{
				#if SHOW
					Debug.WriteLine($"\n\add to non-applicable file list {file.FileName}");
				#endif
					addNonApplicableFile(file);
				}

				compareCountTotal += compareCount;
			}

			Status = ClassifyStatus.COMPLETE;

			a.Stop();

			ticks = a.Elapsed;

			OnPropertyChange(nameof(NonApplicableFiles));

		}

		/* replaced
		/// <summary>
		/// process a single sheet pdf and apply the classification criteria.
		/// add matches to the list within the node
		/// </summary>
		private bool classify4(TreeNode treeNode, FilePath<FileNameSheetPdf> sheetFilePath, int depth, int count)
		{
			bool localMatchFlag;
			bool matchFlag = false;

			int tempCount = 0;
			int compCount = count;

			Debug.WriteLine($"\nclassify {sheetFilePath.FileNameNoExt} versus");

			foreach (TreeNode childNode in treeNode.Children)
			{
				// if (childNode.Item.ItemClass == Item_Class.IC_BOOKMARK)
				// {
				// 	currentHeading = childNode;
				// 	Debug.WriteLine($"current heading node = {currentHeading.Item.Title}");
				// }

				Debug.WriteLine($"\t{childNode.Item.Title}");

				CompareOperations.Depth = depth;

				localMatchFlag = false;

				RaiseTreeNodeChangeEvent(new TreeNodeChangeEventArgs(childNode));

				// bool result = CompareOperations.Compare2(sheetFilePath.FileNameObject, childNode.Item.CompareOps);

				if (CompareOperations.Compare2(sheetFilePath.FileNameObject, childNode.Item.CompareOps, out tempCount))
				{
					compCount += tempCount;

					Debug.WriteLine($"\tcompared count {tempCount} | total {compCount}");

					tempCount = 0;

					// may be a bug here
					// if item matches but is the top level item,
					// localMatchFlag match is false and the item will not be added
					// to the merge list
					if (childNode.HasChildren)
					{
						Debug.WriteLine($"\t\tgoing down");
						localMatchFlag = classify4(childNode, sheetFilePath, depth + 1, compCount);

					}

					compCount += tempCount;
					Debug.WriteLine($"\tchild compared count {tempCount} | total {compCount}");

					if (!localMatchFlag)
					{
						lock(lockList[childNode.Item.MergeItemLockIdx])
						{
							compareCount = compCount;

							Debug.WriteLine($"\t\tA saved count {compCount}");

							MergeItem mi = new MergeItem(0, sheetFilePath);
							mi.CompareCount = compareCount;

							childNode.Item.MergeItems.Add(mi);
							

							childNode.Item.UpdateMergeProperties();
						}
					}

					matchFlag = true;
					break;
				}

				compCount += tempCount;

				Debug.WriteLine($"\t\tloop total {compCount}");

			}

			// worst case, at depth zero and not matchflag is false
			if (!matchFlag && depth == 0)
			{
				lock(lockList[treeNode.Item.MergeItemLockIdx])
				{
					compareCount = compCount;

					Debug.WriteLine($"\tB saved count {compCount}");

					MergeItem mi = new MergeItem(0, sheetFilePath);
					mi.CompareCount = compareCount;

					treeNode.Item.MergeItems.Add(mi);
					treeNode.Item.UpdateMergeProperties();
				}

				matchFlag = true;
			}

			count = compCount;

			Debug.WriteLine($"\tfinal total {compCount}");

			// return true when categorized
			// else return false
			return matchFlag;
		}*/

		private int processDepth = 0;

		/// <summary>
		/// process a single sheet pdf and apply the classification criteria.
		/// add matches to the list within the node
		/// </summary>
		private bool classify3(TreeNode treeNode, FilePath<FileNameSheetPdf> sheetFilePath, int depth) //, int count)
		{
			bool localMatchFlag;
			bool matchFlag = false;

			int tempCount = 0;
			// int compCount = count;

			string margin0 = "  ".Repeat(processDepth++);
			string margin1 = "  ".Repeat(processDepth);
			string margin2 = "  ".Repeat(processDepth + 1);
			string margin3 = "  ".Repeat(processDepth + 2);
			string margin4 = "  ".Repeat(processDepth + 2);

		#if SHOW
			Debug.WriteLine($"\n{margin0}classify {sheetFilePath.FileNameNoExt} versus");
		#endif

			foreach (TreeNode childNode in treeNode.Children)
			{
				// if (childNode.Item.ItemClass == Item_Class.IC_BOOKMARK)
				// {
				// 	currentHeading = childNode;
				// 	Debug.WriteLine($"{margin1}current heading node = {currentHeading.Item.Title}");
				// }

			#if SHOW
				Debug.WriteLine($"{margin2}{childNode.Item.Title} | {childNode.Item.ItemClassName}");
			#endif

				CompareOperations.Depth = depth;

				localMatchFlag = false;

				RaiseTreeNodeChangeEvent(new TreeNodeChangeEventArgs(childNode));

				// bool result = CompareOperations.Compare2(sheetFilePath.FileNameObject, childNode.Item.CompareOps);

			#if SHOW
				Debug.WriteLine($"{margin3}compare next");
			#endif

				if (CompareOperations.Compare2(sheetFilePath.FileNameObject, childNode.Item.CompareOps, out tempCount))
				{
					compareCount += tempCount;

				#if SHOW
					Debug.WriteLine($"{margin3}compare done - got true");
					Debug.WriteLine($"{margin3}compared count {tempCount} | total {compareCount}");
				#endif

					// may be a bug here
					// if item matches but is the top level item,
					// localMatchFlag match is false and the item will not be added
					// to the merge list
					if (childNode.HasChildren)
					{
					#if SHOW
						Debug.WriteLine($"{margin4}going down");
					#endif
						localMatchFlag = classify3(childNode, sheetFilePath, depth + 1); //, compCount);

						// compCount += tempCount;
						// Debug.WriteLine($"{margin2}child compared count {tempCount} | total {compCount}");

					}

					if (!localMatchFlag)
					{
						lock(lockList[childNode.Item.MergeItemLockIdx])
						{
							// compareCount = compCount;
						#if SHOW
							Debug.WriteLine($"\n{margin3}A saved / final count {compareCount})");
						#endif

							MergeItem mi = new MergeItem(0, sheetFilePath);
							mi.CompareCount = compareCount;

							childNode.Item.MergeItems.Add(mi);

							childNode.Item.UpdateMergeProperties();
						}
					}

					matchFlag = true;
					break;
				} 
				else
				{
				#if SHOW
					Debug.WriteLine($"{margin3}compare done - got false");
				#endif
					compareCount += tempCount;
				}

				// compCount += tempCount;

			#if SHOW
				Debug.WriteLine($"{margin3}loop total {compareCount}");
			#endif

			}

			// worst case, at depth zero and not matchflag is false
			if (!matchFlag && depth == 0)
			{
				lock(lockList[treeNode.Item.MergeItemLockIdx])
				{
				#if SHOW
					Debug.WriteLine($"{margin3}B saved / final count {compareCount}");
				#endif

					MergeItem mi = new MergeItem(0, sheetFilePath);

					mi.CompareCount = compareCount;

					treeNode.Item.MergeItems.Add(mi);

					treeNode.Item.UpdateMergeProperties();
				}

				matchFlag = true;
			}

			// count = compCount;

			// Debug.WriteLine($"{margin2}inal total {compareCount}");

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

				child.Item.MergeItemLockIdx = lockList.Count;

				object loc = new object();

				BindingOperations.EnableCollectionSynchronization(child.Item.MergeItems, loc);

				lockList.Add(loc);
			}
		}

		private void sortMergeLists(TreeNode node)
		{
			foreach (TreeNode child in node.Children)
			{
				if (child.HasChildren) sortMergeLists(child);

				if (child.Item.MergeItemCount > 1)
				{
					List<MergeItem> items = child.Item.MergeItems.ToList();

					items.Sort((x, y) => x.SheetNumber.CompareTo(y.SheetNumber));

					child.Item.MergeItems = new ObservableCollection<MergeItem>(items);
				}
			}
		}

		public void UpdateNonApplicableProperties()
		{
			OnPropertyChange(nameof(NonApplicableFiles));
			OnPropertyChange(nameof(NonApplicableFilesTotalCount));
		}

		private void addNonApplicableFile(FilePath<FileNameSheetPdf> file)
		{
			string phBld = file.FileNameObject.SheetID;

			phBld = phBld.IsVoid() ?"Not Associated with a Building" : $"Associated with Building: {phBld}";

			if (!nonApplicableFiles.ContainsKey(phBld))
			{
				nonApplicableFiles.Add(phBld, new List<FilePath<FileNameSheetPdf>>());
				
				me.UpdateNonApplicableProperties();
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

		// private void OnGetAnnouncement(object sender, object value)
		// {
		// #if DML5
		// 	DM.Start0();
		// #endif
		// 	if (value is bool) displayDebugMsgs = (bool) value;
		//
		// 	toParentAnnounce.Announce("got announcement| " +
		// 		(displayDebugMsgs ? "display debug messages\n" : "do not display debug messages\n"));
		// #if DML5
		// 	DM.End0();
		// #endif
		// }

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