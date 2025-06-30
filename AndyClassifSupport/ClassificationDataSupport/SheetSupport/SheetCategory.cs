#region using

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Data;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.MergeSupport;
using JetBrains.Annotations;
using UtilityLibrary;
using static AndyShared.ClassificationDataSupport.TreeSupport.ValueComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.LogicalComparisonOp;
using  static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
using static AndyShared.ClassificationDataSupport.SheetSupport.SheetCategory;
using static AndyShared.ClassificationDataSupport.SheetSupport.ItemClassDef;

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


namespace  AndyShared.ClassificationDataSupport.SheetSupport
{
	// messages between notes and between node items
	public enum INTERNODE_MESSAGES
	{
		// item / item-comp op messages
		IM_COMP_OPS_MODIFIED        = -3,
		IM_CLEAR_ITEM_MODIFICATION  = -2,
		IM_PARENT_FIXED_CHANGED     = -1,

		// both messages		    
		IM_IS_MODIFIED              = 0,

		// node messages		    
		IM_CLEAR_NODE_MODIFICATION  = 1,
		IM_EXPAND_ALL               = 2,
		IM_COLLAPSE_ALL             = 3,
	}

	public enum Item_Class
	{
		// IC_UNASSIGNED = 0,
		IC_BOOKMARK = 1,
		IC_COMPONENT = 2
	}

	[DataContract(Namespace = "")]
	public class ItemClassDef : ACompareOp<Item_Class>
	{
		public ItemClassDef() { }
		public ItemClassDef(string name, Item_Class op) : base(name, op, "") { }

		public override string Name { get; protected set; }
		public override Item_Class OpCode { get; set; }

		public override object Clone()
		{
			return new ItemClassDef(Name, OpCode);
		}

		public static Dictionary<int, ItemClassDef> ItemClassDefs { get; private set; }

		static ItemClassDef()
		{
			init();
		}

		public static void init()
		{
			ItemClassDefs = new Dictionary<int, ItemClassDef>();

			// ItemClassDefs.Add(new ItemClassDef("unassigned", Item_Class.IC_UNASSIGNED));
			ItemClassDefs.Add((int) Item_Class.IC_BOOKMARK, new ItemClassDef("Bookmark Item", Item_Class.IC_BOOKMARK));
			ItemClassDefs.Add((int) Item_Class.IC_COMPONENT,
				new ItemClassDef("Component Item", Item_Class.IC_COMPONENT));
		}
	}

	[DataContract(Name = "SheetCategoryDescription", Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
	public class SheetCategory : INotifyPropertyChanged //, ITreeNodeItem
	{
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

		private ListCollectionView mergeItemsView;

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
		private bool needsSaving;

		private Item_Class itemClass;

		private string sortCode;
		private bool isMatched;
		private bool isModified;

	#endregion

	#region ctor

		public SheetCategory(string title, string description)
		{
			// if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ ctor");

			this.title = title;
			this.description = description;
			this.itemClass = Item_Class.IC_BOOKMARK;
			this.sortCode = "MM";

			CompareOps = new ObservableCollection<ComparisonOperation>();

			OnCreated();
		}

		private void OnCreated()
		{
			compareOps.CollectionChanged += CompareOpsOnCollectionChanged;

			mergeItems = new ObservableCollection<MergeItem>();

			foreach (ComparisonOperation compOp in compareOps)
			{
				compOp.Parent = this;
				compOp.IsInitialized = true;
			}

			ID = ClassificationFile.M_IDX++.ToString("X");

			if (ItemClass == 0) itemClass = Item_Class.IC_BOOKMARK;

			if (sortCode.IsVoid()) sortCode = "MM";

			UpdateInternalProperties();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext c)
		{
			OnCreated();
		}

		// private void CopOnPropertyChanged(object sender, string memberName)
		// {
		// 	RaiseCompOpChangedEvent(INTERNODE_MESSAGES.SCC_COMP_OPS_MODIFIED);
		// }

	#endregion

	#region public properties

		/* data members*/

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

				// if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ title| changed");
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

				// if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ description| changed");
				description = value;
				OnPropertyChanged();

				// tracking
				if (isInitialized) ShtCatModified = true;
			}
		}

// track changes: yes
		[DataMember(Order = 3)]
		public Item_Class ItemClass
		{
			get => itemClass;
			set
			{
				if (value == itemClass) return;
				itemClass = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ItemClassName));
				OnPropertyChanged(nameof(ItemClassIndex));

				// tracking
				if (isInitialized) ShtCatModified = true;
			}
		}

// track changes: yes
		[DataMember(Order = 5)]
		public string SortCode
		{
			get => sortCode;
			set
			{
				if (value == sortCode) return;
				sortCode = value;
				OnPropertyChanged();

				// tracking
				if (isInitialized) ShtCatModified = true;
			}
		}

// track changes: yes
		[DataMember(Order = 10)]
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

				UpdateInternalProperties();
			}
		}

		// track changes: yes (but indirect)
		[DataMember(Order = 15)]
		public ObservableCollection<ComparisonOperation> CompareOps
		{
			get => compareOps;
			set
			{
				if (value?.Equals(compareOps) ?? false) return;

				// if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ CompareOps| changed");
				compareOps = value;
				OnPropertyChanged();
				if (isInitialized) ShtCatModified = true;
			}
		}


		/* non-data members*/

		// track changes: no
		[IgnoreDataMember]
		public ObservableCollection<MergeItem> MergeItems
		{
			get => mergeItems;
			set
			{
				mergeItems = value;

				OnPropertyChanged();
			}
		}

		// [IgnoreDataMember]
		// public ListCollectionView MergeItemsView
		// {
		// 	get => mergeItemsView;
		// 	set => mergeItemsView = value;
		// }

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

				// if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ depth| changed");

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
			get => FileNameSheetIdentifiers.SheetNumComponentData[Depth * 2].Name ?? "";
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

		[IgnoreDataMember]
		public string ItemClassName => ItemClassDef.ItemClassDefs[(int) ItemClass].Name;

		[IgnoreDataMember]
		public int ItemClassIndex
		{
			get => (int) itemClass;
			set => ItemClass = (Item_Class) (value);
		}

		/* status & settings*/

		/* data members*/

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
				OnPropertyChanged(nameof(CannotSelect));

				// tracking
				if (isInitialized)
				{
					ShtCatModified = true;
					notifyParentOfItemChange(INTERNODE_MESSAGES.IM_PARENT_FIXED_CHANGED);
				}
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
				OnPropertyChanged(nameof(CannotSelect));

				// tracking
				if (isInitialized) ShtCatModified = true;
			}
		}

		/* non-data members*/

		[IgnoreDataMember]
		public bool IsParentExCannotSelect => IsExParent(2);

		[IgnoreDataMember]
		public bool IsParentExLocked => IsExParent(0);

		[IgnoreDataMember]
		public bool IsParentExFixed
		{
			get
			{
				string name = title;

				bool result = IsExParent(1);

				return result;
			}
		}
		
		// public bool PopupIsOpen
		// {
		// 	get => popupIsOpen;
		// 	set
		// 	{
		// 		popupIsOpen = value;
		// 		OnPropertyChanged();
		// 	}
		// }

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

		[IgnoreDataMember]
		public bool IsMatched
		{
			get => isMatched;
			set
			{
				if (value == isMatched) return;
				isMatched = value;
				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public bool IsModified
		{
			get => isModified;
			set
			{
				if (value == isModified) return;
				isModified = value;
				OnPropertyChanged();
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool HasMergeItems => MergeItemCount > 0;

// track changes: no
		[IgnoreDataMember]
		public bool IsVisible
		{
			get => isVisible;
			set
			{
				isVisible = value;

				OnPropertyChanged();

				// tracking
				// if (isInitialized) ShtCatModified = true;
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool CannotSelect
		{
			get => isFixed || isLocked;
			set { }
		}

		[IgnoreDataMember]
		public bool NeedsSaving => ShtCatModified || ChildCompOpModified;

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

				if (value == shtCatModified) return;

				shtCatModified = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(NeedsSaving));
				Parent.UpdateProperties();

				if (value) notifyParentOfItemChange(INTERNODE_MESSAGES.IM_IS_MODIFIED);
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
				OnPropertyChanged(nameof(NeedsSaving));

				Parent.UpdateProperties();
			}
		}

	#if Test4
		// track changes: n/a
		[IgnoreDataMember]
		public bool ModifyShtCat
		{
			get => modifyShtCat;
			set
			{
				if (value == modifyShtCat) return;

				// Debug.WriteLine($"\n******* {nameof(ModifyShtCat)} *** set to {value} *********");
				modifyShtCat = value;
				OnPropertyChanged();

				if (value) notifyParentOfItemChange(INTERNODE_MESSAGES.IM_IS_MODIFIED);
			}
		}

	#endif

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void GetChangeFromParent(INTERNODE_MESSAGES modification)
		{
			// Debug.Write($"{Title,-26}|{nameof(GetChangeFromParent),-30} | proceed? | ");

			if (modification == INTERNODE_MESSAGES.IM_CLEAR_ITEM_MODIFICATION)
			{
				shtCatModified = false;

				// OnPropertyChanged(nameof(ShtCatModified));
				// OnPropertyChanged(nameof(NeedsSaving));

				// temp info
				modifyShtCat = false;

			#if Test4
				OnPropertyChanged(nameof(ModifyShtCat));

			#endif

				if (!childCompOpModified)
				{
					// Debug.WriteLine("nope - STOP here (no child changes)");
					return;
				}

				// Debug.WriteLine("yep - continue (got child changes))");

				ChildCompOpModified = false;
			}
			else
			{
				// Debug.WriteLine("nope - STOP here (unknown INTERNODE_MESSAGES)");
				return;
			}

			notifyChildCompOpsOfChange(modification);
		}

		public void CompOpChangeFromChild(INTERNODE_MESSAGES modification)
		{
			OnPropertyChanged(nameof(ShtCatModified));

			ChildCompOpModified = true;

			notifyParentOfItemChange(modification);
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

		public void UpdateInternalProperties()
		{
			OnPropertyChanged(nameof(IsParentExCannotSelect));
			OnPropertyChanged(nameof(IsParentExFixed));
			OnPropertyChanged(nameof(IsParentExLocked));
			OnPropertyChanged(nameof(CannotSelect));
		}

		public void UpdateProperties()
		{
			OnPropertyChanged("Title");
			OnPropertyChanged("Description");
		}

		public void UpdateMergeProperties()
		{
			OnPropertyChanged("MergeItems");
			OnPropertyChanged("MergeItemCount");
		}

		public static SheetCategory TempSheetCategory(int depth = 0, int compIdx = 0 )
		{
			// keep description null
			SheetCategory temp = new SheetCategory($"{tempIdx++:D3} New Node Title", null);

			temp.depth = depth;

			ComparisonOp tempCo = (new ComparisonOp(LOGICAL_NO_OP, EQUALTO, "1", temp.depth));
			tempCo.CompareComponentIndex = compIdx + 1;

			// temp.CompareOps.Add(new ComparisonOp(LOGICAL_NO_OP, EQUALTO, "1", temp.depth));
			temp.CompareOps.Add(tempCo);

			return temp;
		}

		public void AddPrelimCompOp()
		{
			ComparisonOp vco = ComparisonOperation.CreateInitialCompOp(this);

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

		public bool IsExParent(int which)
		{
			if (which == 0) return IsParentLocked();
			if (which == 1) return IsParentFixed();
			if (which == 2) return IsParentCannotSelect();

			return false;
		}

		/* removed
		public void ConfigMergeItems()
		{
			
			mergeItems = new ObservableCollection<MergeItem>();

			mergeItemsView = CollectionViewSource.GetDefaultView(mergeItems) as ListCollectionView;
				
			mergeItemsView.SortDescriptions.Add(
				new SortDescription(nameof(MergeItem.SheetNumber), ListSortDirection.Ascending));
		}*/

	#endregion

	#region private methods

		private void notifyParentOfItemChange(INTERNODE_MESSAGES modification)
		{
			// Debug.WriteLine($"{Title,-26}|{nameof(notifyParentOfItemChange),-30} | {modification}");

			if (parent == null) return;

			shtCatModified = true;
			OnPropertyChanged(nameof(ShtCatModified));
			OnPropertyChanged(nameof(NeedsSaving));

			parent.ItemChangeFromChild(this, modification);
		}

		private void notifyChildCompOpsOfChange(INTERNODE_MESSAGES modification)
		{
			CompOpSelectedIdx = -1;

			foreach (ComparisonOperation compOp in compareOps)
			{
				compOp.ClearCompOpModified(modification);
			}
		}

		private bool IsParentLocked()
		{
			TreeNode testParent = this.parent.Parent;

			for (int i = depth - 1; i >= 1 ; i--)
			{
				if (testParent.Item.IsLocked) return true;

				testParent = testParent.Parent;
			}

			return false;
		}

		private bool IsParentFixed()
		{
			TreeNode testParent = this.parent.Parent;

			for (int i = depth - 1; i >= 1 ; i--)
			{
				if (testParent.Item.IsFixed) return true;

				testParent = testParent.Parent;
			}

			return false;
		}

		private bool IsParentCannotSelect()
		{
			// TreeNode firstParent = this.parent.Parent;
			// TreeNode lastParent;

			TreeNode testParent = this.parent.Parent;

			// Debug.WriteLine($"\nstart test parent = {testParent.Item.Title} | depth={depth} vs ({testParent.Depth})");

			for (int i = depth - 1; i >= 1 ; i--)
			{
				if (testParent.CannotSelect) return true;

				testParent = testParent.Parent;

				// Debug.WriteLine($"next  test parent = {testParent.Item.Title}");

				// lastParent = testParent;
			}

			return false;
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


		// public delegate void CompOpChangedEventHandler(object sender, INTERNODE_MESSAGES modification);
		//
		// public event SheetCategory.CompOpChangedEventHandler CompOpChanged;
		//
		// protected virtual void RaiseCompOpChangedEvent(INTERNODE_MESSAGES modification)
		// {
		// 	CompOpChanged?.Invoke(this, modification);
		// }

	#endregion

	#region system overrides

		public SheetCategory Clone(TreeNode parent)
		{
			// SheetCategory clone = new SheetCategory($"{title} (copy {copyIdx++})", description) { depth = depth };
			SheetCategory clone = new SheetCategory(title, description) { depth = depth };
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

			clone.itemClass = itemClass;
			clone.sortCode = sortCode;
			clone.isFixed = isFixed;
			clone.isLocked = isLocked;
			clone.isVisible = isVisible;
			clone.childCompOpModified = false;
			clone.shtCatModified = false;

			clone.parent = parent;

			clone.childCompOpModified = false;
			clone.shtCatModified = false;

			clone.isInitialized = true;

			UpdateInternalProperties();

			return clone;
		}

		public object Clone()
		{
			copyIdx++;

			// string t = title + $" (copy {copyIdx})";
			// string d = description + $" (copy {copyIdx})";

			// SheetCategory clone = new SheetCategory($"{title} (copy {copyIdx++})", description) { depth = depth };
			SheetCategory clone = new SheetCategory(title, description) { depth = depth };
			ComparisonOperation copy;

			foreach (ComparisonOperation compOp in compareOps)
			{
				copy = (ComparisonOp) compOp.Clone();
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

			clone.itemClass = itemClass;
			clone.sortCode = sortCode;
			clone.isFixed = isFixed;
			clone.isLocked = isLocked;
			clone.isVisible = isVisible;
			clone.shtCatModified = false;


			// the clone is parentless - until added into the collection, 
			// the parent field does not make sense
			clone.parent = null;
			clone.childCompOpModified = false;
			clone.shtCatModified = false;
			clone.isInitialized = false;

			UpdateInternalProperties();

			return clone;
		}

		public void Merge(SheetCategory updatedShtCat)
		{
			if (updatedShtCat.ShtCatModified) merge(updatedShtCat);

			if (updatedShtCat.ChildCompOpModified) mergeCompOps(updatedShtCat.CompareOps);
		}

		private void mergeCompOps(ObservableCollection<ComparisonOperation> newCompOps)
		{
			// 4 cases:
			// 1: both lists are the same size
			// 2: this list is larger than the new list
			//		* delete items
			// 3: this list is smaller than the new list
			//		* add new items
			// 4: new compop count is 0 - this is an error - return

			if (newCompOps.Count < 1) return;

			if (CompareOps.Count == newCompOps.Count)
			{
				mergeCompOps(0, CompareOps.Count, newCompOps);
			}
			else if (CompareOps.Count > newCompOps.Count)
			{
				// delete some
				// copy from 0 to end1
				// remove from end1 to end 3

				mergeCompOps(0, newCompOps.Count, newCompOps);
				removeCompOps(newCompOps.Count, CompareOps.Count);
			}
			else if (CompareOps.Count < newCompOps.Count)
			{
				// add some
				// copy from 0 to end1
				// add from end1 to end 2

				mergeCompOps(0, CompareOps.Count, newCompOps);
				addCompOps(CompareOps.Count, newCompOps.Count, newCompOps);
			}

			OnPropertyChanged(nameof(CompareOps));
		}

		private void merge(SheetCategory updatedShtCat)
		{
			Title = updatedShtCat.Title;
			Description = updatedShtCat.Description;
			ShtCatModified = true;
			ItemClass = updatedShtCat.itemClass;

			SortCode = updatedShtCat.sortCode;

			IsFixed = updatedShtCat.isFixed;
			IsLocked = updatedShtCat.isLocked;
		}

		private void mergeCompOps(int start, int end, ObservableCollection<ComparisonOperation> newCompOps)
		{
			for (int i = start; i < end; i++)
			{
				if (!newCompOps[i].CompOpModified) continue;

				CompareOps[i].Merge(this, newCompOps[i]);
			}
		}

		private void addCompOps(int start, int end, ObservableCollection<ComparisonOperation> newCompOps)
		{
			for (int i = start; i < end; i++)
			{
				newCompOps[i].CompOpModified = false;
				newCompOps[i].Parent = this;

				CompareOps.Add(newCompOps[i]);
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

		public override string ToString() => $"SheetCategory| {ID,-5} | {title}";

	#endregion
	}
}