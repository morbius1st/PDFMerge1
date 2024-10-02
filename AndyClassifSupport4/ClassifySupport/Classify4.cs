#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FileSupport.FileNameSheetPDF4;
using AndyShared.MergeSupport;
using AndyShared.SampleFileSupport;
using JetBrains.Annotations;
using UtilityLibrary;
using TreeNode = AndyShared.ClassificationDataSupport.TreeSupport.TreeNode;
#endregion

// username: jeffs
// created:  9/25/2024 7:28:36 PM

// update of "Classify" but using FileNameSheetPdf3

namespace AndyShared.ClassifySupport
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



	public class Classify4 : INotifyPropertyChanged
	{
	#region private fields

		// used as a filter, any sheet that is assigned to another
		// phase-building will not be included. However, the non-applicable
		// phase-uilding files will be tracked
		private string assignedPhBldg = null;

		// the files do have phase-building information and this needs to be taken
		// into account
		private bool usePhBldg = false;

		private Dictionary<string, List<FilePath<FileNameSheetPdf4>>> nonApplicableFiles;
		private BaseOfTree treeBase;
		private SheetFileList4 fileList;

		private ClassifyStatus classifyStatus = ClassifyStatus.CREATED;

	#endregion

	#region ctor

		public Classify4() { }

	#endregion

	#region public properties

		public Dictionary<string, List<FilePath<FileNameSheetPdf4>>> NonApplicableFiles => nonApplicableFiles;

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
				OnPropertyChanged();
			}
		}


	#endregion

	#region private properties

	#endregion

	#region public methods

		/// <summary>
		/// sets internal variables
		/// configs to use or not, building / phase
		/// </summary>
		public bool Configure(BaseOfTree treeBase, SheetFileList4 fileList)
		{
			Debug.Write("config ok? | ");
			if (treeBase == null || !treeBase.HasChildren ||
				fileList == null || fileList.Files.Count == 0)
			{
				Debug.WriteLine("no");

				string r1 = treeBase == null ? "tree null" : $"no children {!treeBase.HasChildren}";
				string r2 = fileList == null ? "tree null" : $"no files {fileList.Files.Count == 0}";

				Debug.WriteLine($"tree base? {r1}");
				Debug.WriteLine($"list list? {r2}");

				return false;
			}

			Debug.WriteLine("yes");

			this.treeBase = treeBase;
			this.fileList = fileList;

			assignedPhBldg = fileList.Building;

			if (!assignedPhBldg.IsVoid())
			{
				usePhBldg = true;
				nonApplicableFiles = new Dictionary<string, List<FilePath<FileNameSheetPdf4>>>();
			}

			Status = ClassifyStatus.CONFIGURED1;

			return true;
		}

		/// <summary>
		/// init merg item list
		/// init async lock
		/// runs the recursive preprocess routine for each child
		/// </summary>
		public void PreProcess()
		{
			Debug.Write("pre-process ok? | ");

			if (Status != ClassifyStatus.CONFIGURED1
				&& Status != ClassifyStatus.CONFIGURED2)
			{
				Debug.WriteLine("no");
				return;
			}

			Debug.WriteLine("yes");

			treeBase.Item.MergeItems = new ObservableCollection<MergeItem>();

			Status = ClassifyStatus.PREPROCESSING;

			preProcessFiles(treeBase);

			Status = ClassifyStatus.PREPROCESSED;
		}

		/// <summary>
		/// setup item processing.
		/// start the item processing task.
		/// runs the task and awaits it to complete
		/// </summary>
		public async void Process3()
		{
			Debug.Write("process3 ok? | ");
			if (Status != ClassifyStatus.PREPROCESSED)
			{
				Debug.WriteLine("no");
				return;
			}

			Debug.WriteLine("ok so far");

			processFiles3();

		}

	#endregion

	#region private methods

				/// <summary>
		/// runs through the list of files and classify the file.
		/// includes extra operations relative to being in a separate task
		/// </summary>
		private void processFiles3()
		{
			Debug.WriteLine("processFiles3");

			Thread.CurrentThread.Name = "process files";

			Status = ClassifyStatus.RUNNING;

			int fileCount = 1;

			foreach (FilePath<FileNameSheetPdf4> file in fileList.Files)
			{
				Debug.Write($"process {file.FileNameObject.SheetNumber} | ");

				if (usePhBldg)
				{
					if ((!file.FileNameObject.ShtNumber.PhaseBldg?.Equals(assignedPhBldg)) ?? false)
					{
						Debug.WriteLine("skipped");

						addNonApplicableFile(file);
						continue;
					}
				}

				if (classify3(treeBase, file, 0))
				{
					Debug.WriteLine("worked");
				}
				else
				{
					Debug.WriteLine("worked");
				}
					
			}

			Status = ClassifyStatus.COMPLETE;
		}

		/// <summary>
		/// process a single sheet pdf and apply the classification criteria.
		/// add matches to the list within the node
		/// </summary>
		private bool classify3(TreeNode treeNode, FilePath<FileNameSheetPdf4> sheetFilePath, int depth)
		{
			bool localMatchFlag;
			bool matchFlag = false;

			foreach (TreeNode childNode in treeNode.Children)
			{
				// Thread.Sleep(100);

				CompareOperations.Depth = depth;

				localMatchFlag = false;

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

					Debug.WriteLine("add item? ");

					if (!localMatchFlag)
					{
						MergeItem mi = new MergeItem(0, sheetFilePath);
						childNode.Item.MergeItems.Add(mi);
						childNode.Item.UpdateMergeProperties();

						Debug.WriteLine($"yes - 1 adding to {childNode.Item.Title}");
					}

					matchFlag = true;
				}
			}

			// worst case, at depth zero and not matchflag is false
			if (!matchFlag && depth == 0)
			{
				MergeItem mi = new MergeItem(0, sheetFilePath);
				treeNode.Item.MergeItems.Add(mi);
				treeNode.Item.UpdateMergeProperties();

				Debug.WriteLine($"2 adding to {treeNode.Item.Title}");

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

			}
		}

		private void UpdateNonApplicableProperties()
		{
			OnPropertyChanged(nameof(NonApplicableFiles));
			OnPropertyChanged(nameof(NonApplicableFilesTotalCount));
		}


		private void addNonApplicableFile(FilePath<FileNameSheetPdf4> file)
		{
			string phBld = file.FileNameObject.ShtNumber.PhaseBldg;

			if (!nonApplicableFiles.ContainsKey(phBld))
			{
				nonApplicableFiles.Add(phBld, new List<FilePath<FileNameSheetPdf4>>());
			}

			nonApplicableFiles[phBld].Add(file);
		}

		private int countIgnoredFiles()
		{
			if (nonApplicableFiles == null) return 0;

			int count = 0;

			foreach (KeyValuePair<string, List<FilePath<FileNameSheetPdf4>>> kvp in nonApplicableFiles)
			{
				count += kvp.Value.Count;
			}

			return count;
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(Classify4)}";
		}

	#endregion
	}
}