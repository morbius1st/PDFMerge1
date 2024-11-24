#region using directives
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Data;
using AndyShared.ClassificationDataSupport.SheetSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.Support;
using JetBrains.Annotations;
using SettingsManager;
using static AndyShared.ClassificationDataSupport.SheetSupport.SheetCategory;
using static AndyShared.ClassificationDataSupport.SheetSupport.ItemClassDef;
using static UtilityLibrary.MessageUtilities;
#endregion

// username: jeffs
// created:  5/2/2020 9:28:15 AM

namespace AndyShared.ClassificationDataSupport.TreeSupport
{
#region TreeNode

	[DataContract(Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	public class TreeNode : INotifyPropertyChanged, ICloneable
	{
	#region private fields

		// properties
		protected SheetCategory item;

		/// <summary>
		/// the list of child TreeNodes
		/// </summary>
		protected ObservableCollection<TreeNode> children = new ObservableCollection<TreeNode>();

		private ListCollectionView childrenView;

		private TreeNode parent;
		private int depth;
		// private static int maxDepth = 7;

		private int extMergeItemCount;
		// protected int extItemCountCurrent;
		// private int extMgrCountCurr;

		private CheckedState checkedState = CheckedState.UNCHECKED;
		private CheckedState triState = CheckedState.UNSET;

		private bool isInitialized;

		private bool treeNodeModified;
		private bool isExpanded;
		private bool isExpandedAlt;
		private bool isNodeSelected;
		private bool isContextTarget;
		private bool isContextSource;
		
		
		private int checkedChildCount;

		// private bool RememberExpCollapseState;

		// private bool isSaving;

		// fields

		//										                 mixed->checked->unchecked->mixed
		//										                   0        1        2        3 (0)
		private static readonly bool?[] BoolList = new bool?[] {null,     true,    false,   null};
		private bool mixesStateBeenTold;

		// private Orator.ConfRoom.Announcer onModifiedAnnouncer;
		private bool treeNodeChildItemModified;
		private bool modifyTreeNode;
		private bool treeNodeChildNodeModified;
		private bool modifyTreeNodeItem;
		private static bool rememberExpCollapseState = false;
		
	#endregion

	#region ctor

		public TreeNode(TreeNode parent, SheetCategory item, bool isExpanded)
		{
			// Debug.WriteLine("@TreeNode ctor| item.title| " + parent.Item.Title 
			// 	+ " parent depth| " + parent.depth );

			this.parent = parent;
			this.item = item;
			Depth = parent?.depth + 1 ?? 0;
			this.isExpanded = isExpanded;

			OnCreated();

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		}

		public TreeNode(TreeNode parent, SheetCategory item)
		{
			// Debug.WriteLine("@TreeNode ctor| item.title| " + parent.Item.Title 
			// 	+ " parent depth| " + parent.depth );

			this.parent = parent;
			this.item = item;
			Depth = parent?.depth + 1 ?? 0;

			OnCreated();

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		}

		// public TreeNode() : this(null, null, false)
		public TreeNode() : this(null, null)
		{
			// Debug.WriteLine("@TreeNode ctor| empty treenode created");

		}

		protected  TreeNode(SheetCategory item, bool isExpanded)
		{
			this.item = item;
			this.isExpanded = isExpanded;
			
			OnCreated();

			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		}

		// public TreeNode(bool isExpanded)
		// {
		// 	this.isExpanded=isExpanded;
		//
		// 	OnCreated();
		//
		// 	childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;
		// }


		private void OnCreated()
		{
			Children.CollectionChanged += ChildrenOnCollectionChanged;

			// childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;	

			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ treenode|@ oncreated");

			// // listen to parent, initialize
			// Orator.Listen(OratorRooms.TN_INIT, OnAnnouncedTnInit);
			//
			// // listen to parent, changes have been saved
			// Orator.Listen(OratorRooms.SAVED, OnAnnouncedSaved);
			//
			// // listen to parent, changes have been saved
			// Orator.Listen(OratorRooms.SAVING, OnSavingAnnounce);
			//
			// RemExCollapseListenConfig();
			//
			// onModifiedAnnouncer = Orator.GetAnnouncer(this, OratorRooms.MODIFIED);

			IsInitialized = true;

			if (item != null)
			{
				item.Parent = this;
				item.IsInitialized = true;
			}

			ID = ClassificationFile.M_IDX++.ToString("X");

			// if (item != null) item.CompOpChanged += ItemOnCompOpChanged;

			UpdateParentProperties();

		}

		// private void ItemOnCompOpChanged(object sender, INTERNODE_MESSAGES modification)
		// {
		// 	notifyParentOfItemChange(item, modification);
		// }

		[OnDeserializing]
		private void OnDeserializing(StreamingContext c)
		{
			Children = new ObservableCollection<TreeNode>();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext c)
		{
			OnCreated();

			// Debug.WriteLine("@TreeNode onDeserialized| is expanded?");
			if (!RememberExpCollapseState) isExpanded = false;
		}

		// [OnSerializing]
		// private void Onerializing(StreamingContext c)
		// {
		// 	if (!RememberExpCollapseState) isExpanded = false;
		// }

		// private void RemExCollapseListenConfig()
		// {
		// 	// listen to parent, changes have been saved
		// 	Orator.Listen(OratorRooms.TN_REM_EXCOLLAPSE_STATE, OnAnnouncedRemExCollapseState);
		//
		// 	RememberExpCollapseState = (bool) (Orator.GetLastValue(OratorRooms.TN_REM_EXCOLLAPSE_STATE) ?? false);
		//
		// }


	#endregion

	#region public properties

		[IgnoreDataMember]
		public string ID { get; set; }

		// public static int Ct = 0;

		// track changes: yes - but indirect
		// the actual tree data item
		[DataMember(Order = 20)]
		public SheetCategory Item
		{
			get => item;

			private set
			{
				item = value;
			
				if (item == null) return;
				
				item.Depth = depth;
			
				OnPropertyChanged();
			}
		}

		// track changes: no const
		[IgnoreDataMember]
		public NodeType NodeType
		{
			get
			{
				if (parent == null && ChildCount > 0) return NodeType.ROOT;
				if (ChildCount == 0) return NodeType.LEAF;

				return NodeType.BRANCH;
			}
		}

// track changes: yes
		[DataMember(Order = 2)]
		public TreeNode Parent
		{
			get => parent;
			set
			{
				if (value?.Equals(parent) ?? false) return;

				parent = value;
				OnPropertyChanged();
				UpdateParentProperties();

				if (isInitialized) NodeChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);

				// if (isInitialized) TreeNodeModified = true;
			}
		}

		[IgnoreDataMember]
		public TreeNode RootParent => GetGrandParent();

// track changes: yes
		[DataMember(Order = 3)]
		public int Depth
		{
			get => depth;
			set
			{
				if (value == depth) return;

				depth = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(CannotSelect));

				if (item != null) item.Depth = value;

				if (isInitialized) NodeChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);

				// if (isInitialized) TreeNodeModified = true;
			}
		}

// track changes: yes
		[DataMember(Order = 5)]
		public CheckedState CheckedState
		{
			get => checkedState;

			private set
			{
				if (value == CheckedState.UNSET)
				{
					checkedState = value;
					if (isInitialized) NodeChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
					// if (isInitialized) TreeNodeModified = true;
				}
				else if (value != checkedState)
				{
					checkedState = value;
					OnPropertyChanged("Checked");

					parent?.UpdateChildCount(checkedState);

					if (isInitialized) NodeChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
					// if (isInitialized) TreeNodeModified = true;
				}
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool? Checked
		{
			get
			{
				if (checkedState == CheckedState.UNSET) return null;

				return BoolList[(int) checkedState];
			}

			set
			{
				processStateChange(selectStateFromBool(value));
				OnPropertyChanged();
			}
		}

// track changes: yes
		[DataMember(Order = 7)]
		public CheckedState TriState
		{
			get => triState;

			set
			{
				if (value != triState)
				{
					triState = value;
					OnPropertyChanged();
					if (isInitialized) NodeChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
					// if (isInitialized) TreeNodeModified = true;
				}
			}
		}

// track changes: yes
		[DataMember(Order = 30, Name = "SubCategories")]
		public ObservableCollection<TreeNode> Children
		{
			get => children;

			private set
			{
				if (value?.Equals(children) ?? false) return;

				children = value;
				NotifyChildrenChange();

				// if (isInitialized) NodeChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
				// if (isInitialized) TreeNodeModified = true;
			}
		}

		// public ICollectionView ChildrenView
		// track changes: no
		[IgnoreDataMember]
		public ListCollectionView ChildrenView
		{
			get => childrenView;

			private set
			{
				childrenView = value;

				OnPropertyChanged();
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool HasChildren => ChildCount > 0;

		// track changes: no
		[IgnoreDataMember]
		public int ChildCount => Children?.Count ?? 0;

		// [IgnoreDataMember]
		// public int ExtendedChildCount => ExtendedChildrenCount(this);
		// track changes: no
		public int ExtChildCount => ExtChildrenCount();

		// track changes: no
		[IgnoreDataMember]
		public int CheckedChildCount
		{
			get => checkedChildCount;

			set
			{
				if (value != checkedChildCount)
				{
					checkedChildCount = value;
					OnPropertyChanged();
				}
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public object ChildrenCollectLock { get; set; }

		// track changes: no
		[IgnoreDataMember]
		public int ChildrenCollectLockIdx { get; set; }



	#region item properties

		// track changes: no
		[IgnoreDataMember]
		public int MergeItemCount => item.MergeItemCount;

		// track changes: no
		[IgnoreDataMember]
		public int ExtMergeItemCount
		{
			get => extMergeItemCount;
			set
			{
				extMergeItemCount = value;
				OnPropertyChanged();

				OnPropertyChanged(nameof(ChildrenMergeItemCount));
			}
		}

		public int ChildrenMergeItemCount => extMergeItemCount - item.MergeItemCount;



	#endregion

	#region status properties

		public static bool RememberExpCollapseState
		{
			get => rememberExpCollapseState;
			set
			{
				if (value == rememberExpCollapseState) return;
				rememberExpCollapseState = value;
			}
		}


		/// <summary>
		/// controls whether the node is expanded or not
		/// </summary>

// track changes: yes
		[DataMember(Order = 15)]
		public bool IsExpanded
		{
			get => isExpanded;

			set
			{
				if (value == isExpanded) return;

				isExpanded = value;

				OnPropertyChanged();

				if (RememberExpCollapseState)
				{
					if (isInitialized) NodeChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
					// TreeNodeModified = true;
				}
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool IsExpandedAlt
		{
			get => isExpandedAlt;
			set
			{
				isExpandedAlt = value;
				OnPropertyChanged();
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool CanExpand => ChildCount > 0;

		// track changes: n/a
		[IgnoreDataMember]
		public bool IsNodeSelected
		{
			get => isNodeSelected;
			set
			{
				// this is set in the WPF style
				// this occurs before SelectionChanged event

				// Debug.WriteLine($"{$"** @ tn 1| node isSelected as | {value} |",-40} {item.Title} ({ID})");

				isNodeSelected = value;
				// Debug.WriteLine($"@ node is selected | set to {isNodeSelected}");

				OnPropertyChanged();
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool IsContextTarget
		{
			get => isContextTarget;
			set
			{
				isContextTarget = value;
				OnPropertyChanged();
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool IsContextSource
		{
			get => isContextSource;
			set
			{
				isContextSource = value;
				OnPropertyChanged();
			}
		}

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

		// track changes: no
		[IgnoreDataMember]
		public bool CannotSelect => Item.CannotSelect; // || IsMaxDepth;

		[IgnoreDataMember]
		public bool IsParentExCannotSelect => IsParent(2);


		// // track changes: no
		// [IgnoreDataMember]
		// public bool IsMaxDepth => depth >= maxDepth;

		// track changes: n/a
		[IgnoreDataMember]
		public virtual bool TreeNodeModified
		{
			get
			{
				return treeNodeModified; // || Item.TreeNodeModified;
			}
			set
			{
				if (!isInitialized || value == treeNodeModified) return;

				treeNodeModified = value;
				OnPropertyChanged();

				// Debug.WriteLine($"{item.Title,-26}|{$"TreeNodeModified",-30} | {value} (isinit| {isInitialized})");

				// if (isInitialized)
				// {
				// 	onModifiedAnnouncer.Announce(null);
				//
				// }
			}
		}

		// track changes: n/a
		[IgnoreDataMember]
		public virtual bool TreeNodeChildItemModified
		{
			get => treeNodeChildItemModified;
			set
			{
				if (value == treeNodeChildItemModified) return;
				treeNodeChildItemModified = value;
				OnPropertyChanged();

			}
		}


	#if Test4

		// track changes: n/a
		[IgnoreDataMember]
		public bool ModifyTreeNode
		{
			get => modifyTreeNode;
			set
			{
				if (value == modifyTreeNode) return;
				// Debug.WriteLine($"{this.item.Title,-26}|{nameof(ModifyTreeNode),-30}");
				modifyTreeNode = value;
				OnPropertyChanged();

				if (!value)
				{
					// Debug.WriteLine($"\n******* {nameof(ModifyTreeNode)} *** set false *********");
					
					NodeChangeFromParent(INTERNODE_MESSAGES.IM_CLEAR_NODE_MODIFICATION);
				}

				if (value)
				{
					// Debug.WriteLine($"\n******* {nameof(ModifyTreeNode)} *** set true *********");

					NodeChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
				}
			}
		}

		// track changes: n/a
		[IgnoreDataMember]
		public bool ModifyTreeNodeItem
		{
			get => modifyTreeNodeItem;
			set
			{
				if (value == modifyTreeNodeItem) return;
				// Debug.WriteLine($"{this.item.Title,-26}|{nameof(ModifyTreeNodeItem),-30}");
				modifyTreeNodeItem = value;
				OnPropertyChanged();

				if (!value)
				{
					// Debug.WriteLine($"\n******* {nameof(ModifyTreeNodeItem)} *** set false *********");

					GetChangeFromParent(INTERNODE_MESSAGES.IM_CLEAR_ITEM_MODIFICATION);
				}
			}
		}
		
	#endif


	#endregion

	#endregion

	#region private properties

	#endregion

	#region public methods

		public TreeNode GetGrandParent()
		{
			TreeNode grandParent = this;

			for (int i = depth - 1; i >= 0 ; i--)
			{
				if (grandParent.parent.NodeType == NodeType.ROOT) break;

				grandParent = grandParent.parent;

			}

			return grandParent;
		}

		public bool IsChildOf(TreeNode testParent)
		{
			TreeNode checkParent = parent;

			for (int i = depth - 1; i >= 0 ; i--)
			{
				if (checkParent.Equals(testParent)) return true;

				checkParent = checkParent.parent;

			}

			return false;
		}

		public bool IsParent(int which)
		{
			if (which == 2) return IsParentCannotSelect();

			return false;
		}

		private bool IsParentCannotSelect()
		{
			TreeNode testParent = this;

			for (int i = depth - 1; i >= 1 ; i--)
			{
				if (testParent.CannotSelect) return true;
			}

			return false;
		}

		public int CountExtMergeItems()
		{
			int count = item.MergeItemCount;

			foreach (TreeNode childNode in children)
			{
				count += childNode.CountExtMergeItems();
			}

			ExtMergeItemCount = count;

			return count;
		}

		public void InitializeAllChildrenView()
		{
			childrenView = CollectionViewSource.GetDefaultView(children) as ListCollectionView;

			foreach (TreeNode node in Children)
			{
				node.InitializeAllChildrenView();
				node.IsInitialized = true;
			}
		}

		public void UpdateChildCount(CheckedState childState)
		{
			if (childState == CheckedState.MIXED)
			{
				if (!mixesStateBeenTold)
				{
					mixesStateBeenTold = true;
					CheckedChildCount++;
					return;
				}
			}

			mixesStateBeenTold = false;

			if (childState == CheckedState.CHECKED)
			{
				CheckedChildCount++;
				return;
			}

			CheckedChildCount--;
		}

		public static TreeNode TempTreeNode(TreeNode parent, TreeNode selected)
		{
			int idx = selected.Item.CompareOps.Count > 0 ? selected.Item.CompareOps[0].CompareComponentIndex : 0;

			TreeNode temp = new TreeNode(parent, 
				SheetCategory.TempSheetCategory(parent.depth, idx), false);

			return temp;
		}

		public void ResetNode()
		{
			CheckedState = CheckedState.UNCHECKED;
			TriState = CheckedState.UNSET;
		}

		public void ResetTree()
		{
			if (NodeType == NodeType.BRANCH)
			{
				// reset children then myself
				foreach (TreeNode node in ChildrenView )
				{
					node.ResetTree();
				}
			}

			ResetNode();
		}

		public void TriStateReset()
		{
			TriState = CheckedState.UNSET;
		}

		public void UpdateProperties()
		{
			OnPropertyChanged(nameof(Item));
			OnPropertyChanged(nameof(ChildrenView));
			OnPropertyChanged(nameof(HasChildren));
			OnPropertyChanged(nameof(ChildCount));
			OnPropertyChanged(nameof(ExtChildCount));
			OnPropertyChanged(nameof(MergeItemCount));
			OnPropertyChanged(nameof(ExtMergeItemCount));

			
			// OnPropertyChanged(nameof(ExtMergeItemCountCurrent));

			// OnPropertyChanged("ExtItemCount");
		}

		public void UpdateParentProperties()
		{
			OnPropertyChanged(nameof(RootParent));
			OnPropertyChanged(nameof(IsParentExCannotSelect));
		}

		// inter node communications routines

		// state changes
		// get notification from parent of a state modification
		public void StateChangeFromParent(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			if (useTriState)
			{
				if (triState == CheckedState.UNSET)
				{
					// first time through - save current
					triState = checkedState;
				}

				if (newState == CheckedState.MIXED) newState = triState;
			}
			else
			{
				triState = CheckedState.UNSET;
			}

			CheckedState = newState;

			notifyChildrenOfStateChange(newState, oldState, useTriState);
		}

		// get notification from child of a state modification
		public void StateChangeFromChild(CheckedState newState, CheckedState oldState, bool useTristate)
		{
			CheckedState priorState = checkedState;
			CheckedState finalState = CheckedState.UNSET;


			if (!useTristate)
			{
				triState = CheckedState.UNSET;
			}
			else if (triState == CheckedState.UNSET)
			{
				// usetristate is true
				// starting tristate process - save the current state
				triState = checkedState;
			}

			switch (newState)
			{
			// newstate is checked - child is checked
			case CheckedState.CHECKED:
				if (checkedChildCount == (Children?.Count ?? 0))
				{
					finalState = CheckedState.CHECKED;
				}
				else if (checkedState != CheckedState.MIXED)
				{
					finalState = CheckedState.MIXED;
				}

				break;
			// newstate is unchecked - child is unchecked
			case CheckedState.UNCHECKED:
				if (checkedChildCount == 0)
				{
					finalState = CheckedState.UNCHECKED;
				}
				else if (checkedState == CheckedState.CHECKED)
				{
					finalState = CheckedState.MIXED;
				}

				break;
			// newstate is checked - child is mixed
			case CheckedState.MIXED:
				// must evaluate whether coming from checked to mixed or
				// from unchecked to mixed

				// i am checked, flip from checked to
				// mixed.  inform parent.
				if (checkedState == CheckedState.CHECKED ||
					checkedState == CheckedState.UNCHECKED)
				{
					finalState = CheckedState.MIXED;
				}

				break;
			}

			if (finalState != CheckedState.UNSET)
			{
				CheckedState = finalState;
				notifyParentOfStateChange(finalState, priorState, useTristate);
			}
		}

		// inform children to modification
		public void ItemChangeFromParent(INTERNODE_MESSAGES modification)
		{
			if (item == null || ChildrenView == null)
			{
				return;
			}

			if (modification == INTERNODE_MESSAGES.IM_CLEAR_ITEM_MODIFICATION)
			{
				if (!TreeNodeChildItemModified)
				{
					return;
				}

				TreeNodeChildItemModified = false;
			}
			else 
			if (modification == INTERNODE_MESSAGES.IM_PARENT_FIXED_CHANGED && item!= null)
			{
				item.UpdateInternalProperties();
			}
			else
			{
				return;
			}

			item.GetChangeFromParent(modification);

			notifyChildNodesOfItemChange(modification);
		}

		// get notification from child of an item modification
		// for this modification can come from a child node or
		// can come from a child item
		public void ItemChangeFromChild(SheetCategory item, INTERNODE_MESSAGES change)
		{

			TreeNodeChildItemModified = true;

			modifyTreeNodeItem = true;

			if (change == INTERNODE_MESSAGES.IM_PARENT_FIXED_CHANGED)
			{
				notifyChildNodesOfItemChange(change);
			} 
			else
			{
				notifyParentOfItemChange(item, change);
			}

		#if Test4
			OnPropertyChanged(nameof(ModifyTreeNodeItem));
		#endif

		}

		public void NodeChangeFromChild(INTERNODE_MESSAGES modification)
		{
			// Debug.Write($"{this.item.Title,-26}|{$"ItemChangeFromChild",-30} | proceed? |");

			if (modification == INTERNODE_MESSAGES.IM_IS_MODIFIED)
			{
				// Debug.WriteLine("yep - continue");

				// treeNodeModified = true;
				// OnPropertyChanged(nameof(TreeNodeModified));

				TreeNodeModified = true;

				// modifyTreeNode = true;
				// OnPropertyChanged(nameof(ModifyTreeNode));

				notifyParentOfNodeChange(modification);
			}
			// else
			// {
			// 	Debug.WriteLine("nope - STOP here (invalid modification type)");
			// 	return;
			// }

		}

		public void NodeChangeFromParent(INTERNODE_MESSAGES modification)
		{
			// Debug.Write($"{this.item.Title,-26}|{nameof(NodeChangeFromParent),-30} | proceed? | ");

			if (ChildrenView == null)
			{
				// Debug.WriteLine("nope - STOP here (childview null)");
				return;
			}

			if (modification == INTERNODE_MESSAGES.IM_CLEAR_NODE_MODIFICATION)
			{
				if (!TreeNodeModified)
				{
					return;
				}

				TreeNodeModified = false;

				modifyTreeNode = false;

				#if Test4
				OnPropertyChanged(nameof(ModifyTreeNode));
				#endif

			}
			else
			if (modification == INTERNODE_MESSAGES.IM_EXPAND_ALL)
			{
				IsExpanded = true;
			}
			else
			if (modification == INTERNODE_MESSAGES.IM_COLLAPSE_ALL)
			{
				IsExpanded = false;
			}
			else
			{
				return;
			}

			notifyChildNodesOfNodeChange(modification);
		}

		// notify window of changes
		public void NotifyChildrenChange()
		{
			OnPropertyChanged("Children");
			OnPropertyChanged("ChildrenView");
			OnPropertyChanged("ChildCount");
			OnPropertyChanged("HasChildren");
			// OnPropertyChanged("ExtendedChildCount");

			if (isInitialized) NodeChangeFromChild(INTERNODE_MESSAGES.IM_IS_MODIFIED);
		}

	#endregion

	#region private methods

		/* inter node communications*/

		// notify parent / children of state changes
		private void notifyParentOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			parent?.StateChangeFromChild(newState, oldState, useTriState);
		}

		private void notifyChildrenOfStateChange(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			if (ChildrenView == null) return;

			foreach (TreeNode node in ChildrenView)
			{
				node.StateChangeFromParent(newState, oldState, useTriState);
			}
		}

		private void notifyParentOfItemChange(SheetCategory item, INTERNODE_MESSAGES modification)
		{
			// Debug.WriteLine($"{this.item.Title,-26}|{$"notifyParentOfItemChange",-30} | {modification}");

			parent?.ItemChangeFromChild(item, modification);
		}

		private void notifyParentOfNodeChange(INTERNODE_MESSAGES modification)
		{
			// Debug.WriteLine($"{this.item.Title,-26}|{nameof(notifyParentOfNodeChange),-30}");

			parent?.NodeChangeFromChild(modification);
		}

		private void notifyChildNodesOfItemChange(INTERNODE_MESSAGES modification)
		{
			// Debug.WriteLine($"{this.item.Title,-26}|{nameof(notifyChildNodesOfItemChange),-30}");

			foreach (TreeNode node in ChildrenView)
			{
				node.ItemChangeFromParent(modification);
			}

		}

		private void notifyChildNodesOfNodeChange(INTERNODE_MESSAGES modification)
		{
			// Debug.WriteLine($"{this.item.Title,-26}|{nameof(notifyChildNodesOfNodeChange),-30}");

			foreach (TreeNode node in ChildrenView)
			{
				node.NodeChangeFromParent(modification);
			}

		}


		// process routines

		private int ExtChildrenCount()
		{
			if (children.Count == 0) return 0;

			int count = children.Count;

			foreach (TreeNode child in children)
			{
				count += child.ExtChildCount;
			}

			return count;
		}

		private void processStateChangeLeaf(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			CheckedState = newState;

			notifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void processStateChangeBranch(CheckedState newState, CheckedState oldState, bool useTriState)
		{
			CheckedState = newState;

			notifyChildrenOfStateChange(newState, oldState, useTriState);
			notifyParentOfStateChange(newState, oldState, useTriState);
		}

		private void processStateChange(CheckedState newState)
		{
			CheckedState oldState = checkedState;

			// process when leaf
			if (NodeType == NodeType.LEAF)
			{
				processStateChangeLeaf(newState, checkedState, false);

				return;
			}

			// node type is branch
			if (triState == CheckedState.UNSET)
			{
				// not currently tristate processing

				if (checkedState == CheckedState.CHECKED ||
					checkedState == CheckedState.UNCHECKED)
				{
					processStateChangeBranch(newState, oldState, false);
				}
				// state is mixed // start using tri-state
				else
				{
					triState = checkedState;

					processStateChangeBranch(newState, oldState,  true);
				}
			}
			else
				// selectStateOriginal is set
				// processing a tri-state modification
				// mixed to checked
				// checked to unchecked
				// unchecked to mixed

				// current is mixed -> modification to checked
				// current is checked -> modification to unchecked
				// current is unchecked -> modification to mixed
			{
				CheckedState proposed =
					selectStateFromBool(BoolList[(int) oldState + 1]);

				CheckedState = proposed;

				notifyParentOfStateChange(proposed, oldState, true);
				notifyChildrenOfStateChange(proposed, oldState, true);
			}
		}

		private int indexInBoolList(bool? test)
		{
			for (var i = 0; i < BoolList.Length; i++)
			{
				if (BoolList[i] == test) return i;
			}

			return 0;
		}

		private CheckedState selectStateFromBool(bool? test)
		{
			return (CheckedState) indexInBoolList(test);
		}


		// private void ExtMergeItemCount(TreeNode node)
		// {
		// 	// count my merge items
		// 	int count = node.item.MergeItems.Count;
		//
		// 	Debug.Write("  ".Repeat(tabDepth));
		// 	Debug.WriteLine("@ ext count start | " + count.ToString("##0") + "  " + item.Title );
		//
		// 	tabDepth++;
		//
		// 	// sum the count from each child recursevly
		// 	foreach (TreeNode child in node.children)
		// 	{
		// 		count += child.ExtMergeItemCount(child);
		// 	}
		//
		// 	tabDepth--;
		//
		// 	Debug.Write("  ".Repeat(tabDepth));
		// 	Debug.WriteLine("@ ext count end   | " + count.ToString("###") + "  " + item.Title);
		//
		//
		// 	ExtMergeItemCount = count;
		// }

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event consuming

		// private void OnSavingAnnounce(object sender, object value)
		// {
		// 	// isSaving = true;
		// }
		//
		// private void OnAnnouncedRemExCollapseState(object sender, object value)
		// {
		// 	
		// 	RememberExpCollapseState = (bool) (value ?? false);
		//
		// 	Debug.WriteLine("@TreeNode onAnnounced| remember?| " + RememberExpCollapseState.ToString());
		// }
		//
		// private void OnAnnouncedTnInit(object sender, object value)
		// {
		// 	if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ treenode|@ onann-tninit| received");
		// 	isInitialized = true;
		// 	treeNodeModified = false;
		// }
		//
		// private void OnAnnouncedSaved(object sender, object value)
		// {
		// 	if (Common.SHOW_DEBUG_MESSAGE1)
		// 		Debug.WriteLine("@     treenode|@ onann-saved| received| isinitialized| "
		// 			+ isInitialized + " | ismodified| " + TreeNodeModified + " | who| " + this.ToString());
		// 	treeNodeModified = false;
		// 	// isSaving = false;
		// }

		private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			TreeNodeModified = true;
		}

	#endregion

	#region system overrides

		// creates a partial clone - does not clone the children
		public object Clone()
		{
			TreeNode newNode = new TreeNode(parent, (SheetCategory) item.Clone(), false);

			newNode.checkedState = checkedState;
			newNode.isExpanded = isExpanded;
			newNode.triState = triState;

			newNode.depth = depth;

			return newNode;
		}

		public TreeNode Clone(int whichParent)
		{
			// TreeNode newNode= new TreeNode(null, false);
			TreeNode newNode = null;

			if (whichParent==0)
			{
				newNode= new TreeNode(null, false);
				newNode.item = item.Clone(newNode);
			}
			else
			if (whichParent==1)
			{
				newNode = new TreeNode(parent, (SheetCategory) item.Clone(), false);
			}
			else
			if (whichParent==2)
			{
				newNode = new TreeNode(this, (SheetCategory) item.Clone(), false);
				
			} 
			else
			{
				return null;
			}

			newNode.checkedState = checkedState;
			newNode.isExpanded = isExpanded;
			newNode.triState = triState;
			// newNode.isContextSelected = false;
			newNode.depth = depth;

			return newNode;
		}

		// create a complete clone node + all children
		public TreeNode CloneEx()
		{
			return Clone(this);
		}

		private TreeNode Clone(TreeNode parentNode)
		{
			TreeNode copy = (TreeNode) parentNode.Clone();

			foreach (TreeNode node in parentNode.children)
			{
				copy.children.Add(Clone(node));
			}

			return copy;
		}

		public override string ToString()
		{
			return NodeType + "::" + item.Title + "::" + checkedState + "expanded | " + isExpanded + "exp alt | " + isExpandedAlt;
		}

	#endregion
	}

#endregion

#region BaseOfTree

	[DataContract(Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
	public class BaseOfTree : TreeNode
	{
	#region private fields

		private TreeNode selectedNode;

		private bool treeNodeModified1;

		private bool treeNodeChildItemModified1;
		// private int extMergeItemCountCurrent;

	#endregion

	#region ctor

		public BaseOfTree() : base(null, false)
		{
			Depth = 0;

			item = new SheetCategory("Initial Item", "Initial Item");

		}

		public void Initalize()
		{
			
			IsInitialized = true;

			InitializeAllChildrenView();
		}

	#endregion

	#region public properties

		public TreeNode SelectedNode
		{
			get => selectedNode;
			set
			{
				selectedNode = value;
				OnPropertyChanged();
				OnPropertyChanged("HasSelection");
			}
		}

		public bool HasSelection => SelectedNode != null;

		public override bool TreeNodeModified
		{
			get => treeNodeModified1;
			set
			{
				if (value == treeNodeModified1) return;
				treeNodeModified1 = value;
				OnPropertyChanged();

				RaiseTreeModifiedEvent();
			}
		}

		public override bool TreeNodeChildItemModified
		{
			get => treeNodeChildItemModified1;
			set
			{
				if (value == treeNodeChildItemModified1) return;
				treeNodeChildItemModified1 = value;
				OnPropertyChanged();

				RaiseTreeItemModifiedEvent();
			}
		}

		public bool HasGrandChildren => hasGrandChildren();

		public bool HasGrandChildrenExpanded => hasGrandChildrenExpanded();

	#endregion

	#region public methods

		/*inter node communications */

		public void NotifyChangeFromParent(INTERNODE_MESSAGES mod)
		{
			if ((int) mod >= 0)
			{
				NodeChangeFromParent(mod);
			} 

			if ((int) mod <= 0)
			{
				ItemChangeFromParent(mod);
			}



			// if (mod==INTERNODE_MESSAGES.IM_CLEAR_NODE_MODIFICATION)
			// {
			// 	
			// } 
			// else 
			// if (mod==INTERNODE_MESSAGES.IM_CLEAR_ITEM_MODIFICATION)
			// {
			// 	GetChangeFromParent(mod);
			// }
		}


		/* expansion control */

		public void ExpandAll()
		{
			NotifyChangeFromParent(INTERNODE_MESSAGES.IM_EXPAND_ALL);

			OnPropertyChanged(nameof(HasGrandChildrenExpanded));
		}

		public void CollapseAll()
		{
			NotifyChangeFromParent(INTERNODE_MESSAGES.IM_COLLAPSE_ALL);

			OnPropertyChanged(nameof(HasGrandChildrenExpanded));
		}


		/* node adjustments */

		public void RemoveNode2(TreeNode contextNode)
		{
			TreeNode parent = contextNode.Parent;
			parent.Children.Remove(contextNode);

			NotifyChildrenChange();
		}

		public TreeNode AddNewChild2(TreeNode contextNode)
		{
			TreeNode temp = TempTreeNode(contextNode, contextNode);
			
			// temp.IsNodeSelected = true;

			contextNode.Children.Add(temp);

			contextNode.NotifyChildrenChange();

			return temp;
		}

		public void AddChild2(TreeNode contextNode, TreeNode toAddNode)
		{
			toAddNode.Depth = ++contextNode.Depth;
			contextNode.Children.Add(toAddNode);

			NotifyChildrenChange();
		}

		// adds a new, temp node before the selected node
		public TreeNode AddNewBefore2(TreeNode contextNode)
		{
			TreeNode parent = contextNode.Parent;
			TreeNode temp = TempTreeNode(parent, parent);

			// temp.IsNodeSelected = true;

			AddAt2(parent, temp, parent.Children.IndexOf(contextNode));

			return temp;
		}

		// adds without notifying of the revision
		public void AddBefore2(TreeNode contextNode, TreeNode toAddNode)
		{
			toAddNode.Depth = ++contextNode.Depth;
			TreeNode parent = contextNode.Parent;

			AddAt2(parent, toAddNode, parent.Children.IndexOf(contextNode));
		}

		// adds a new, temp node before the selected node
		public TreeNode AddNewAfter2(TreeNode contextNode)
		{
			TreeNode parent = contextNode.Parent;

			TreeNode temp = TempTreeNode(parent, parent);

			// temp.IsNodeSelected = true;

			AddAt2(parent, temp, parent.Children.IndexOf(contextNode) + 1);

			return temp;
		}

		// adds without notifying of the revision
		public void AddAfter2(TreeNode contextNode, TreeNode toAddNode)
		{
			TreeNode parent = contextNode.Parent;
			toAddNode.Depth = parent.Depth + 1;

			AddAt2(parent, toAddNode, parent.Children.IndexOf(contextNode) + 1);
		}

		public void AddNode(TreeNode node)
		{
			node.Depth = node.Parent.Depth + 1;

			node.Parent.Children.Add(node); 
			
			NotifyChildrenChange();
		}

		/// <summary>
		/// move sourceNode to before targetNode - at the same depth / same parent
		/// </summary>
		/// <param name="targetNode">the selected node</param>
		/// <param name="sourceNode">the existing node being relocated</param>
		public void MoveBefore(TreeNode targetNode, TreeNode sourceNode)
		{
			sourceNode.Depth = targetNode.Depth;

			moveNode2(targetNode, sourceNode, NodePlacement.BEFORE);
		}

		/// <summary>
		/// move the existing node after the context selected node
		/// </summary>
		/// <param name="contextNode">the selected node</param>
		/// <param name="existingNode">the existing node being relocated</param>
		public void MoveAfter(TreeNode contextNode, TreeNode existingNode)
		{
			existingNode.Depth = contextNode.Depth;

			moveNode2(contextNode, existingNode, NodePlacement.AFTER);
		}

		/// <summary>
		/// move the existing node to be a child of the selected node
		/// </summary>
		/// <param name="contextNode">the selected node</param>
		/// <param name="existingNode">the node being relocated</param>
		public void MoveAsChild(TreeNode contextNode, TreeNode existingNode)
		{
			moveAsChild2(contextNode, existingNode);

		}

		// public int ExtMergeItemCountCurrent
		// {
		// 	get => extMergeItemCountCurrent;
		// 	set
		// 	{
		// 		if (value != -1) return;
		//
		// 		int count = item.MergeItemCount;
		// 		
		// 		foreach (TreeNode childNode in children)
		// 		{
		// 			childNode.ExtMergeItemCountCurrent = -1;
		// 			count += childNode.ExtMergeItemCountCurrent;
		// 		}
		//
		// 		extMergeItemCountCurrent = count;
		//
		// 		OnPropertyChanged();
		// 	}
		// }

	#endregion

	#region private methods

		private void CannotAddChildError(IntPtr handle =  default)
		{
			CommonTaskDialogs.CommonErrorDialog(
				"Cannot Add Sub-Category",
				"A sub-category cannot be added",
				"The maximum sheet classification depth has" + nl
				+ "been reached and a sub-category cannot be added",
				handle);
		}

		/// <summary>
		/// inserts a child node at the index provided<br/>
		/// includes notify children of change
		/// </summary>
		private void AddAt2(TreeNode parent, TreeNode toAddNode, int index)
		{
			parent.Children.Insert(index, toAddNode);
			
			NotifyChildrenChange();
		}

		// parent is the parent of the new location
		// moves withiin a parent's collection - or -
		// from one parent to another
		// move the source note to be a child of the target node's parent
		private void moveNode2(TreeNode targetNode, TreeNode sourceNode, NodePlacement how)
		{
			// if (!targetNode.Parent.Equals(sourceNode.Parent)) return;

			TreeNode parent = targetNode.Parent;

			int contextIdx = parent.Children.IndexOf(targetNode);

			contextIdx = how == NodePlacement.AFTER ? ++contextIdx : contextIdx;

			if (sourceNode.Parent.Equals(parent))
			{
				int existIdx = parent.Children.IndexOf(sourceNode);

				if (contextIdx < parent.Children.Count )
				{
					// simple move within the same children collection
					parent.Children.Move(existIdx, contextIdx);

				} 
				else
				{ 
					// simple move within the same children collection
					// but gets placed at the end of the collection
					parent.Children.Insert(contextIdx, sourceNode);
					parent.Children.RemoveAt(existIdx);
				}

				NotifyChildrenChange();
			}
			else
			{
				// parents are different - moving from one 
				// parent's collection to another parent's collection

				// however, must check that the new parent is not a child 
				// of the source

				// complex move from one collection to another
				// this means, add then delete

				if (targetNode.IsChildOf(sourceNode))
				{
					return;
				}

				TreeNode exParent = sourceNode.Parent;

				// update the parent of the existing node
				sourceNode.Parent = targetNode.Parent;

				// add in the parent collection of he selected node
				AddAt2(parent, sourceNode, contextIdx);

				// remove from the original collection
				exParent.Children.Remove(sourceNode);
			}
		}

		// move to be the child of targetNode
		private void moveAsChild2(TreeNode contextNode, TreeNode existingNode)
		{
			// save the original parent for later use
			TreeNode exParent = existingNode.Parent;

			// update the parent of the existing node
			existingNode.Parent = contextNode;

			// add as first node
			AddAt2(contextNode, existingNode, 0);

			exParent.Children.Remove(existingNode);
		}

		private bool hasGrandChildren()
		{
			foreach (TreeNode child in children)
			{
				if (child.ChildCount > 0) return true;
			}

			return false;
		}

		/// <summary>
		/// check to see if any child nodes are expanded
		/// </summary>
		/// <returns></returns>
		private bool hasGrandChildrenExpanded()
		{
			foreach (TreeNode child in children)
			{
				if (child.ChildCount > 0 && child.IsExpanded) return true;
			}
		
			return false;
		}


	#endregion

	#region event issuing

		public delegate void TreeModifiedEventHandler(object sender);

		public event BaseOfTree.TreeModifiedEventHandler TreeModified;

		protected virtual void RaiseTreeModifiedEvent()
		{
			TreeModified?.Invoke(this);
		}

		public delegate void TreeItemModifiedEventHandler(object sender);

		public event BaseOfTree.TreeItemModifiedEventHandler TreeItemModified;

		protected virtual void RaiseTreeItemModifiedEvent()
		{
			TreeItemModified?.Invoke(this);
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return NodeType + ":: ** class BaseOfTree ** ::" + CheckedState;
		}

	#endregion
	}

#endregion
}