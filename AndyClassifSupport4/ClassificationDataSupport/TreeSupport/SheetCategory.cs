#region using
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AndyShared.MergeSupport;
using AndyShared.Support;
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
	#region private fields

		// static fields
		private static int tempIdx = 100;
		private static int copyIdx;

		// not static fields
		private string title;
		private string description;
		private int depth;
		private ObservableCollection<ComparisonOperation> compareOps;

		// the actual sheet PDF files that get merged into the
		// final PDF set
		private ObservableCollection<MergeItem> mergeItems;

		private bool isInitialized;
		private bool isModified;
		private bool isLocked;
		private bool isFixed;
		private bool isVisible = true;


		private Orator.ConfRoom.Announcer onModifiedAnnouncer;

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
			Orator.Listen(OratorRooms.TN_INIT, OnAnnounceTnInit);

			// listen to parent, changes have been saved
			Orator.Listen(OratorRooms.SAVED, OnAnnounceSaved);

			onModifiedAnnouncer = Orator.GetAnnouncer(this, OratorRooms.MODIFIED);

			mergeItems = new ObservableCollection<MergeItem>();

			// IsInitialized = true;
		}

		[OnDeserialized]
		private void OnDeserializing(StreamingContext c)
		{
			OnCreated();
		}


	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public string Title
		{
			get => title;
			set
			{
				if (value?.Equals(title) ?? false) return;

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ title| changed");
				title = value;
				OnPropertyChange();
				IsModified = true;

				if (description == null)
				{
					Description = value;
				}
			}
		}

		[DataMember(Order = 2)]
		public string Description
		{
			get => description;
			set
			{
				if (value?.Equals(description) ?? false) return;

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ description| changed");
				description = value;
				OnPropertyChange();
				IsModified = true;
			}
		}

		[DataMember(Order = 10)]
		public ObservableCollection<ComparisonOperation> CompareOps
		{
			get => compareOps;
			set
			{
				if (value?.Equals(compareOps) ?? false) return;

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ CompareOps| changed");
				compareOps = value;
				OnPropertyChange();
				IsModified = true;
			}
		}

		[IgnoreDataMember]
		public ObservableCollection<MergeItem> MergeItems
		{
			get => mergeItems;
			set
			{
				mergeItems = value;
			}
		}

		[IgnoreDataMember]
		public int MergeItemCount => mergeItems?.Count ?? 0;

		[IgnoreDataMember]
		public int Count => MergeItemCount;

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

				OnPropertyChange();
				OnPropertyChange("ComponentName");
				UpdateProperties();
				IsModified = true;
			}
		}

		[IgnoreDataMember]
		public string ComponentName
		{
			// get => SheetNumberComponentTitles[Depth].Name ?? ""; 
			get => ShtIds.SheetNumComponentData[Depth * 2].Name ?? ""; 
		}

		[IgnoreDataMember]
		public bool IsInitialized
		{
			get => isInitialized;
			set => isInitialized = value;
		}

		[IgnoreDataMember]
		public bool IsModified
		{
			get => isModified;
			set
			{
				if (!isInitialized) return;

				if (Common.SHOW_DEBUG_MESSAGE1)
					Debug.WriteLine("@ sheetcat|@ ismodified| changed| value| " 
					+ value + "| ismodified| " + isModified + " | who| " + this.ToString());

				if (value == isModified) return;

				isModified = value;
				OnPropertyChange();

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ ismodified| changed| isinitialized| " + isInitialized);

				if (isInitialized)
				{
					onModifiedAnnouncer.Announce(true);
				}
			}
		}

		/// <summary>
		/// means this is item is a fixed node and cannot be<br/>
		/// deleted but may be locked (but not be the user)<br/>
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
				IsModified = true;
				OnPropertyChange();
			}
		}

		/// <summary>
		/// locked node cannot be changed - deleted or modified<br/>
		/// but could be fixed.  However, a fixed and locked note<br/>
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
				IsModified = true;
				OnPropertyChange();

			}
		}

		[IgnoreDataMember]
		public bool CanSelect
		{
			get => isFixed || isLocked; 
			set { }
		}

		[IgnoreDataMember]
		public bool IsVisible
		{
			get => isVisible;
			set
			{
				isVisible = value;

				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public bool HasMergeItems => MergeItemCount > 0;

		// this is the index for lock associated with the
		// merge items collection
		[IgnoreDataMember]
		public int MergeItemLockIdx { get; set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		// ReSharper disable once UnusedMember.Global
		public int FindCompOp(int findId)
		{
			for (var i = 0; i < compareOps.Count; i++)
			{
				if (compareOps[i].Id == findId) return i;
			}

			return -1;
		}

		public void RemoveCompOpAt(int idx)
		{
			compareOps.RemoveAt(idx);
			OnPropertyChange("CompareOps");
		}

		public void UpdateProperties()
		{
			OnPropertyChange("Title");
			OnPropertyChange("Description");
			OnPropertyChange("Pattern");
		}

		public void UpdateMergeProperties()
		{
			OnPropertyChange("MergeItems");
			OnPropertyChange("MergeItemCount");
		}

		public static SheetCategory TempSheetCategory(int depth = 0)
		{
			// SheetCategory temp = new SheetCategory($"{tempIdx:D3} New Node Title",
			// 	$"{tempIdx++:D3} New Node Description");

			// keep description null
			SheetCategory temp = new SheetCategory($"{tempIdx:D3} New Node Title", null);

			temp.depth = depth;

			temp.CompareOps.Add(new ValueCompOp(LOGICAL_NO_OP, EQUALTO, "1", temp.depth));

			return temp;
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

		private void OnAnnounceTnInit(object sender, object value)
		{
			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ sheetcat|@ onann-tninit| received");
			isInitialized = true;
			isModified = false;
		}

		private void OnAnnounceSaved(object sender, object value)
		{
			isModified = false;

			if (Common.SHOW_DEBUG_MESSAGE1)
				Debug.WriteLine("@ sheetcat|@ onann-saved| received| isinitialized| " 
				+ isInitialized + " | ismodified| " + IsModified + " | who| " + this.ToString());
		}

		private void CompareOpsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			IsModified = true;
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange([CallerMemberName] string memberName = "") =>
			PropertyChanged?.Invoke(this,  new PropertyChangedEventArgs(memberName));

	#endregion

	#region system overrides

		public object Clone()
		{
			copyIdx++;

			string t = title + $" (copy {copyIdx})";
			string d = description + $" (copy {copyIdx})";

			SheetCategory clone = new SheetCategory(t, d) {depth = depth };

			foreach (ComparisonOperation compOp in compareOps)
			{
				clone.compareOps.Add((ValueCompOp) compOp.Clone());
				// if (compOp.GetType() == typeof(ValueCompOp))
				// {
				// 	
				// }
				// else
				// {
				// 	clone.compareOps.Add((LogicalCompOp) compOp.Clone());
				// }
			}

			return clone;
		}

		public override string ToString() => "this is SheetCategory| " + title;

	#endregion
	}
}