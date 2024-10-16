#region using
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AndyShared.ClassificationFileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.MergeSupport;
using AndyShared.Support;
using JetBrains.Annotations;

using static AndyShared.ClassificationDataSupport.TreeSupport.ValueComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.LogicalComparisonOp;
using  static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
#endregion

// ReSharper disable CommentTypo

// username: jeffs
// created:  5/2/2020 9:18:14 AM


/*
	notes:
	1. phase / building adds a description to the category but does not create a tree depth
	2. phase / building cannot be a condition
	3. the category list's top level starts with discipline

	adding conditions

	at treenode: 
	> replace pattern with a condition list
	> condition list has a maximum of 5 conditions
	> treenode depth defines to which sheet name component the conditions apply
		top = discipline / -1 = category / -2 = subcategory / -3 = modifier / -4 = submodifier
	> conditions
		all tests are case sensitive a != A
		{} = the value of the sheet component

		value conditions (vc)
			match conditions (mc)
				{} contains value, {} does not contain value, {} starts with value, {} does not start with value, {} ends with value, {} does not end with value

			comparison conditions (cc)
				{} == value, {} != value, {} > value, {} >= value, {} < value, {} <= value
		logical conditions (lc)
			condition AND condition, condition OR condition
	> condition list
		> condition, comparison value (for value conditions and for comparison values)
		> condition, null value (for logical conditions)
		> min list length = 1
		> list length will always be a odd number
		> ex lists: A) (vc); B) (vc) (lc) (vc)
	> list processing
		> evaluate resultA = (vc)
		> loop
		> has an (lc) no -> return resultA
		> evaluate resultB = next (vc)
		> evaluate resultA = resultA (lc) resultB
			
*/
// ReSharper restore CommentTypo

namespace AndyShared.ClassificationDataSupport.TreeSupport
{

	[DataContract(Name = "SheetCategoryDescription", Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
	public class SheetCategory : INotifyPropertyChanged, ITreeNodeItem
	{
		public enum ITEM_CHANGE
		{
			IC_COMP_OPS_MODIFIED,
			IC_IS_MODIFIED,
			IC_CLEAR_MODIFICATION
		}

	#region private fields

		// static fields
		protected static int tempIdx = 100;
		protected static int copyIdx;

		// not static fields
		private string title;
		private string description;
		private int depth;
		private ObservableCollection<ComparisonOperation> compareOps;

		// the actual sheet PDF files that get merged into the
		// final PDF set
		private ObservableCollection<MergeItem> mergeItems;

		private bool isInitialized;
		private bool shtCatModified;
		private bool isLocked;
		private bool isFixed;
		private bool isVisible = true;
		private bool modifyShtCat;

		// private Orator.ConfRoom.Announcer onModifiedAnnouncer;
		private TreeNode parent;
		private bool childCompOpModified;
		private ComparisonOperation compOpSelected;
		private int compOpSelectedIdx;

	#endregion

	#region ctor

		public SheetCategory(string title, string description)
		{
			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ ctor");

			this.title = title;
			this.description = description;

			CompareOps = new ObservableCollection<ComparisonOperation>();

			OnCreated();
		}

		private void OnCreated()
		{
			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ oncreated");

			compareOps.CollectionChanged += CompareOpsOnCollectionChanged;
			
			// listen to parent, initialize
			// Orator.Listen(OratorRooms.TN_INIT, OnAnnounceTnInit);
			//
			// // listen to parent, changes have been saved
			// Orator.Listen(OratorRooms.SAVED, OnAnnounceSaved);
			//
			// onModifiedAnnouncer = Orator.GetAnnouncer(this, OratorRooms.MODIFIED);

			mergeItems = new ObservableCollection<MergeItem>();

			foreach (ComparisonOperation compOp in compareOps)
			{
				compOp.Parent = this;
				compOp.IsInitialized = true;
			}

			// isInitialized = true;

			// foreach (ComparisonOperation cop in compareOps)
			// {
			// 	cop.ComparisonOpChanged += CopOnPropertyChanged;
			// }

			ID = ClassificationFile.M_IDX++.ToString("X");
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext c)
		{
			OnCreated();
		}

		// private void CopOnPropertyChanged(object sender, string memberName)
		// {
		// 	RaiseCompOpChangedEvent(ITEM_CHANGE.SCC_COMP_OPS_MODIFIED);
		// }


	#endregion

	#region public properties

		[IgnoreDataMember]
		public string ID { get; set; }

// track changes: yes
		[DataMember(Order = 1)]
		public string Title
		{
			get => title;
			set
			{
				if (value?.Equals(title) ?? false) return;

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ title| changed");
				title = value;
				OnPropertyChanged();

				// tracking
				if (isInitialized) ShtCatModified = true;

				if (description == null)
				{
					Description = value;
				}
			}
		}

// track changes: yes
		[DataMember(Order = 2)]
		public string Description
		{
			get => description;
			set
			{
				if (value?.Equals(description) ?? false) return;

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ description| changed");
				description = value;
				OnPropertyChanged();

				// tracking
				if (isInitialized) ShtCatModified = true;
			}
		}

// track changes: yes
		[DataMember(Order = 3)]
		public TreeNode Parent
		{
			get => parent;
			set
			{
				if (Equals(value, parent)) return;
				parent = value;
				OnPropertyChanged();

				// tracking
				if (isInitialized) ShtCatModified = true;
			}
		}

		// track changes: yes (but indirect)
		[DataMember(Order = 10)]
		public ObservableCollection<ComparisonOperation> CompareOps
		{
			get => compareOps;
			set
			{
				if (value?.Equals(compareOps) ?? false) return;

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ CompareOps| changed");
				compareOps = value;
				OnPropertyChanged();
				if (isInitialized) ShtCatModified = true;
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public ObservableCollection<MergeItem> MergeItems
		{
			get => mergeItems;
			set
			{
				mergeItems = value;
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public int MergeItemCount => mergeItems?.Count ?? 0;

		// track changes: no
		[IgnoreDataMember]
		public int CompOpCount => CompareOps.Count;

		// may be the depth into the comp type list
// track changes: yes?
		[IgnoreDataMember]
		public int Depth
		{
			get => depth;
			set
			{
				if (value == depth) return;

				depth = value;

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ depth| changed");

				// Cs++;

				// compareOps[0].CompareComponentIndex = depth;

				OnPropertyChanged();
				OnPropertyChanged("ComponentName");
				UpdateProperties();

				// tracking
				if (isInitialized) ShtCatModified = true;
			}
		}

		// track changes: no (const)
		[IgnoreDataMember]
		public string ComponentName
		{
			// get => SheetNumberComponentTitles[Depth].Name ?? ""; 
			get => FileNameSheetIdentifiers.SheetNumComponentData[Depth*2].Name ?? ""; 
		}

		[IgnoreDataMember]
		public int CompOpSelectedIdx
		{
			get => compOpSelectedIdx;
			set
			{
				compOpSelectedIdx = value;
				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public ComparisonOperation CompOpSelected
		{
			get => compOpSelected;
			set
			{
				if (Equals(value, compOpSelected)) return;
				compOpSelected = value;

				CompOpSelectedIdx = compareOps.IndexOf(value);
			}
		}


		// status / settings

		// track changes: no
		// this is the index for lock associated with the
		// merge items collection (thread safety)
		[IgnoreDataMember]
		public int MergeItemLockIdx { get; set; }



		// track changes: no
		[IgnoreDataMember]
		public bool IsInitialized
		{
			get => isInitialized;
			set
			{
				if (value == isInitialized) return;
				isInitialized = value;

				OnPropertyChanged();
			}
		}

// track changes: yes
		/// <summary>
		/// means this is item is a fixed node and cannot be<br/>
		/// deleted but may be locked (but not by the user)<br/>
		/// for this to happen, the XML file would have to be<br/>
		/// manually edited
		/// </summary>
		[DataMember(Order = 4)]
		public bool IsFixed
		{
			get => isFixed;

			set
			{
				if (value == isFixed) return;

				isFixed = value;
				OnPropertyChanged();
				
				// tracking
				if (isInitialized) ShtCatModified = true;
			}
		}

// track changes: yes
		/// <summary>
		/// locked node cannot be changed - deleted or modified<br/>
		/// but could be fixed.  However, a fixed and locked node<br/>
		/// has limited value. for this to happen, the XML file would<br/>
		/// have to be manually edited
		/// </summary>
		[DataMember(Order = 5)]
		public bool IsLocked
		{
			get => isLocked;

			set
			{
				if (value == isLocked) return;

				isLocked = value;
				OnPropertyChanged();

				// tracking
				if (isInitialized) ShtCatModified = true;
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool CanSelect
		{
			get => isFixed || isLocked; 
			set { }
		}

// track changes: yes
		[IgnoreDataMember]
		public bool IsVisible
		{
			get => isVisible;
			set
			{
				isVisible = value;

				OnPropertyChanged();

				// tracking
				if (isInitialized) ShtCatModified = true;
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool HasMergeItems => MergeItemCount > 0;

		// inter object communications - status only
		// that is, this ONLY holds the modification status
		// it does not cause up or down notifications

		// track changes: no
		// means that this object was modified and
		// NOT a child object
		[IgnoreDataMember]
		public bool ShtCatModified
		{
			get => shtCatModified;
			set
			{
				if (!isInitialized) return;

				// if (Common.SHOW_DEBUG_MESSAGE1)
				// 	Debug.WriteLine("@ sheetcat|@ ismodified| changed| value| " 
				// 	+ value + "| ismodified| " + shtCatModified + " | who| " + this.ToString());

				if (value == shtCatModified) return;

				shtCatModified = value;
				OnPropertyChanged();

				// if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ ismodified| changed| isinitialized| " + isInitialized);

				// if (isInitialized)
				// {
				// 	onModifiedAnnouncer.Announce(true);
				//
				// 	// RaiseCompOpChangedEvent(ITEM_CHANGE.IC_IS_MODIFIED);
				// }

				if (value) notifyParentOfItemChange(ITEM_CHANGE.IC_IS_MODIFIED);
			}
		}

		// track changes: n/a
		// means that a child object was modified and
		// not this object
		[IgnoreDataMember]
		public bool ChildCompOpModified
		{
			get => childCompOpModified;
			set
			{
				if (value == childCompOpModified) return;
				childCompOpModified = value;
				OnPropertyChanged();
			}
		}

		// track changes: n/a
		[IgnoreDataMember]
		public bool ModifyShtCat
		{
			get => modifyShtCat;
			set
			{
				if (value == modifyShtCat) return;

				Debug.WriteLine($"\n******* {nameof(ModifyShtCat)} *** set to {value} *********");
				modifyShtCat = value;
				OnPropertyChanged();

				if (value) notifyParentOfItemChange(ITEM_CHANGE.IC_IS_MODIFIED);
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void ItemChangeFromParentNode(ITEM_CHANGE change)
		{
			Debug.Write($"{Title,-26}|{nameof(ItemChangeFromParentNode),-30} | proceed? | ");

			if (change == ITEM_CHANGE.IC_CLEAR_MODIFICATION)
			{
				shtCatModified = false;
				OnPropertyChanged(nameof(ShtCatModified));

				// temp info
				modifyShtCat = false;
				OnPropertyChanged(nameof(ModifyShtCat));

				if (!childCompOpModified)
				{
					Debug.WriteLine("nope - STOP here (no child changes)");
					return;
				}

				Debug.WriteLine("yep - continue (got child changes))");

				childCompOpModified = false;
				OnPropertyChanged(nameof(ChildCompOpModified));

			}
			else
			{
				Debug.WriteLine("nope - STOP here (unknown ITEM_CHANGE)");
				return;
			}

			notifyChildCompOpsOfChange(change);
		}

		public void CompOpChangeFromChild(ITEM_CHANGE change)
		{
			Debug.WriteLine($"{Title,-26}|{$"CompOpChangeFromChild",-30} | {change}");
			childCompOpModified = true;
			OnPropertyChanged(nameof(ChildCompOpModified));
			
			notifyParentOfItemChange(change);

		}

		// ReSharper disable once UnusedMember.Global
		public int FindCompOp(int findId)
		{
			for (var i = 0; i < compareOps.Count; i++)
			{
				if (compareOps[i].Id == findId) return i;
			}

			return -1;
		}

		public void UpdateProperties()
		{
			OnPropertyChanged("Title");
			OnPropertyChanged("Description");
			OnPropertyChanged("Pattern");
		}

		public void UpdateMergeProperties()
		{
			OnPropertyChanged("MergeItems");
			OnPropertyChanged("MergeItemCount");
		}

		public static SheetCategory TempSheetCategory(int depth = 0)
		{
			// keep description null
			SheetCategory temp = new SheetCategory($"{tempIdx:D3} New Node Title", null);

			temp.depth = depth;

			temp.CompareOps.Add(new ValueCompOp(LOGICAL_NO_OP, EQUALTO, "1", temp.depth));

			return temp;
		}

		public void AddPrelimCompOp()
		{
			ValueCompOp vco = ComparisonOperation.CreateInitialCompOp(this);

			compareOps.Add(vco);

			OnPropertyChanged(nameof(CompareOps));
			
		}

		public void RemoveCompOpAt(int idx)
		{
			compareOps.RemoveAt(idx);
			OnPropertyChanged(nameof(CompareOps));

			ChildCompOpModified = true;
		}

		public bool DeleteSelectedCompOp()
		{
			if (CompOpSelectedIdx < 0) return false;
			if (CompOpCount == 1 || CompOpSelectedIdx >= CompOpCount) return false;

			if (CompOpSelectedIdx == 0)
			{
				compareOps[1].LogicalComparisonOpCode = LOGICAL_NO_OP;
			}

			compareOps.RemoveAt(CompOpSelectedIdx);

			// ShtCatModified = true;
			ChildCompOpModified = true;

			return true;
		}

	#endregion

	#region private methods

		private void notifyParentOfItemChange(ITEM_CHANGE change)
		{
			Debug.WriteLine($"{Title,-26}|{nameof(notifyParentOfItemChange),-30} | {change}");

			if (parent == null) return;

			shtCatModified = true;
			OnPropertyChanged(nameof(ShtCatModified));
			parent.ItemChangeFromChild(this, change);
		}

		private void notifyChildCompOpsOfChange(ITEM_CHANGE change)
		{
			CompOpSelectedIdx = -1;

			foreach (ComparisonOperation compOp in compareOps)
			{
				compOp.ClearCompOpModified(change);
			}
		}

	#endregion

	#region event consuming

		// private void OnAnnounceTnInit(object sender, object value)
		// {
		// 	if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ onann-tninit| received");
		// 	isInitialized = true;
		// 	shtCatModified = false;
		// }
		//
		// private void OnAnnounceSaved(object sender, object value)
		// {
		// 	shtCatModified = false;
		//
		// 	if (Common.SHOW_DEBUG_MESSAGE1)
		// 		Debug.WriteLine("@ sheetcat|@ onann-saved| received| isinitialized| " 
		// 		+ isInitialized + " | ismodified| " + ShtCatModified + " | who| " + this.ToString());
		// }

		private void CompareOpsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (isInitialized) ShtCatModified = true;
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string memberName = "") =>
			PropertyChanged?.Invoke(this,  new PropertyChangedEventArgs(memberName));

		
		// public delegate void CompOpChangedEventHandler(object sender, ITEM_CHANGE change);
		//
		// public event SheetCategory.CompOpChangedEventHandler CompOpChanged;
		//
		// protected virtual void RaiseCompOpChangedEvent(ITEM_CHANGE change)
		// {
		// 	CompOpChanged?.Invoke(this, change);
		// }

	#endregion

	#region system overrides

		public SheetCategory Clone(TreeNode parent)
		{
			copyIdx++;

			string t = title + $" (copy {copyIdx})";
			string d = description + $" (copy {copyIdx})";

			SheetCategory clone = new SheetCategory(t, d) {depth = depth };
			ComparisonOperation copy;

			foreach (ComparisonOperation compOp in compareOps)
			{
				copy = compOp.Clone(clone);
				clone.compareOps.Add(copy);
			}

			clone.mergeItems = new ObservableCollection<MergeItem>();

			foreach (MergeItem mergeItem in mergeItems)
			{
				clone.mergeItems.Add((MergeItem) mergeItem.Clone());
			}
			clone.isFixed = isFixed;
			clone.isLocked = isLocked;
			clone.isVisible = isVisible;

			clone.childCompOpModified = false;
			clone.shtCatModified = false;

			clone.isInitialized=true;
			clone.parent = parent;

			return clone;
		}

		public object Clone()
		{
			copyIdx++;

			string t = title + $" (copy {copyIdx})";
			string d = description + $" (copy {copyIdx})";

			SheetCategory clone = new SheetCategory(t, d) {depth = depth };
			ComparisonOperation copy;

			foreach (ComparisonOperation compOp in compareOps)
			{
				copy = (ValueCompOp) compOp.Clone();
				copy.IsInitialized = false;
				copy.Parent = clone;
				copy.IsInitialized = true;

				clone.compareOps.Add(copy);
			}

			clone.mergeItems = new ObservableCollection<MergeItem>();

			foreach (MergeItem mergeItem in mergeItems)
			{
				clone.mergeItems.Add((MergeItem) mergeItem.Clone());
			}

			clone.isFixed = isFixed;
			clone.isLocked = isLocked;
			clone.isVisible = isVisible;
			clone.shtCatModified = false;
			clone.childCompOpModified = false;

			// the clone is parentless - until added into the collection, 
			// the parent field does not make sense
			clone.parent = null;
			clone.childCompOpModified = false;
			clone.shtCatModified = false;
			clone.isInitialized=false;

			return clone;
		}

		public void MergeCompOps(SheetCategory newShtCat)
		{
			// 4 cases:
			// 1: both lists are the same size
			// 2: this list is larger than the new list
			//		* delete items
			// 3: this list is smaller than the new list
			//		* add new items
			// 4: new compop count is 0 - this is an error - return

			if (newShtCat.CompOpCount < 1) return;

			if (CompareOps.Count == newShtCat.CompOpCount)
			{
				mergeCompOps(0, CompareOps.Count, newShtCat);
			}
			else
			if (CompareOps.Count > newShtCat.CompOpCount)
			{
				// delete some
				// copy from 0 to end1
				// remove from end1 to end 3

				mergeCompOps(0, newShtCat.CompOpCount, newShtCat);
				removeCompOps(newShtCat.CompOpCount, CompareOps.Count);

			}
			else
			if (CompareOps.Count < newShtCat.CompOpCount)
			{
				// add some
				// copy from 0 to end1
				// add from end1 to end 2

				mergeCompOps(0, CompareOps.Count, newShtCat);
				addCompOps(CompareOps.Count, newShtCat.CompOpCount, newShtCat);
			}

			OnPropertyChanged(nameof(CompareOps));
		}

		private void mergeCompOps(int start, int end, SheetCategory newShtCat)
		{
			for (int i = start; i < end; i++)
			{
				if (!newShtCat.CompareOps[i].CompOpModified) continue;

				CompareOps[i].Merge(this, newShtCat.CompareOps[i]);
			}
		}

		private void addCompOps(int start, int end, SheetCategory newShtCat)
		{
			for (int i = start; i < end; i++)
			{
				newShtCat.CompareOps[i].CompOpModified = false;
				newShtCat.CompareOps[i].Parent = this;

				CompareOps.Add(newShtCat.CompareOps[i]);
			}

			if (start != end) ChildCompOpModified = true;
		}

		private void removeCompOps(int start, int end)
		{
			for (int i = end - 1; i >= start ; i--)
			{
				CompareOps.RemoveAt(i);
			}

			if (start != end) ChildCompOpModified = true;
		}

		public override string ToString() => "this is SheetCategory| " + title;

	#endregion
	}
}