// #define SHOW

#region using directives

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AndyShared.ClassificationDataSupport.SheetSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.Support;
using JetBrains.Annotations;
using UtilityLibrary;
using static AndyShared.ClassificationDataSupport.TreeSupport.ValueComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.LogicalComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
using static AndyShared.ClassificationDataSupport.SheetSupport.SheetCategory;
using static AndyShared.ClassificationDataSupport.SheetSupport.ItemClassDef;
using System.Drawing.Printing;

#endregion

// username: jeffs
// created:  5/31/2020 5:03:54 PM


namespace AndyShared.ClassificationDataSupport.TreeSupport
{
	public enum LogicalComparisonOp
	{
		LOGICAL_NO_OP         = 0,
		LOGICAL_OR            = 1,
		LOGICAL_AND           ,
		LOGICAL_COUNT
	}

	public enum ValueComparisonOp
	{
		VALUE_NO_OP           = 0,
		LESS_THAN_OR_EQUAL    = 1,
		LESS_THAN             ,
		GREATER_THAN_OR_EQUAL ,
		GREATER_THAN          ,
		EQUALTO               ,
		DOES_NOT_EQUAL        ,
		CONTAINS              ,
		STARTS_WITH           ,
		DOES_NOT_START_WITH   ,
		ENDS_WITH             ,
		DOES_NOT_END_WITH     ,
		MATCHES               ,
		DOES_NOT_MATCH        ,
		DOES_NOT_CONTAIN      ,
		VALUE_COUNT
	}

	[DataContract(Namespace = "")]
	[KnownType(typeof(ComparisonOp))]
	public abstract class ComparisonOperation : INotifyPropertyChanged, ICloneable
	{
	#region private fields

		protected ValueCompOpDef valueCompOpDef = ValueCompareOps[(int) VALUE_NO_OP];
		protected LogicalCompOpDef logicalCompOp = null;

		protected int compareComponentIndex;

		protected string compareValue;
		protected bool isDsableCompOp;

		protected bool isInitialized;
		protected bool compOpModified;

		protected LogicalComparisonOp logicalComparisonOpCode = LOGICAL_NO_OP;
		protected ValueComparisonOp valueComparisonOpCode = VALUE_NO_OP;

		// private Orator.ConfRoom.Announcer OnModifiedAnnouncer;
		protected SheetCategory parent;
		private bool modifyCompOp;
		protected bool isSelected;

	#endregion

	#region ctor

		public ComparisonOperation()
		{
			OnCreated();
		}

		private void OnCreated()
		{
			// OnModifiedAnnouncer = Orator.GetAnnouncer(this, OratorRooms.MODIFIED);

			// listen to parent, changes have been saved
			// Orator.Listen(OratorRooms.SAVED, OnOratorAnnouncedSaved);

			// listen to parent, initialize
			// Orator.Listen(OratorRooms.TN_INIT, OnAnnounceTnInit);

			// IsInitialized = true;

			ID = ClassificationFile.M_IDX++.ToString("X");
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext c)
		{
			OnCreated();
		}

	#endregion

	#region public properties

		// track changes: yes
		// parent object - does not need modification management here
		[IgnoreDataMember]
		public SheetCategory Parent
		{
			get => parent;
			set
			{
				if (value == parent) return;
				parent = value;

				if (IsInitialized) CompOpModified = true;
			}
		}

		[IgnoreDataMember]
		public string ID { get; set; }

		// track changes: no
		[IgnoreDataMember]
		public int Id { get; set; }

		// track changes: yes
		[DataMember(Order = 0)]
		public int CompareComponentIndex
		{
			get => compareComponentIndex;
			set
			{
				if (value == compareComponentIndex) return;

				compareComponentIndex = value;

				OnPropertyChanged();
				OnPropertyChanged("CompareComponentName");
				OnPropertyChanged("CompNameData");

				if (IsInitialized) CompOpModified = true;
			}
		}

		// track changes: no
		// type is constants - does not modification
		[IgnoreDataMember]
		public ShtNumComps2 CompNameData => FileNameSheetIdentifiers.SheetNumComponentData[compareComponentIndex * 2];

		// track changes: no
		[IgnoreDataMember]
		public string CompareComponentName
		{
			get => FileNameSheetIdentifiers.SheetNumComponentData[compareComponentIndex * 2].Name;
		}


		// value comparisons

		// track changes: no - track via ValueCompOpDef
		// enum
		/// <summary>
		/// primary property that defines the value comparison
		/// </summary>
		[DataMember(Order = 4)]
		public ValueComparisonOp ValueComparisonOpCode
		{
			get => valueComparisonOpCode;
			// get => valueCompOpDef?.OpCode ?? VALUE_NO_OP;
			set
			{
				if (value == valueComparisonOpCode) return;

				valueComparisonOpCode = value;

				ValueCompOpCode = (int) value;

				OnPropertyChanged();

				if (IsInitialized) CompOpModified = true;
			}
		}

		// track changes: no
		/// <summary>
		/// secondary member<br/>
		/// set using 'ValueComparisonOpCode'<br/>
		/// represents ???
		/// </summary>
		[IgnoreDataMember]
		public abstract int ValueCompOpCode { get; set; }

		// track changes: no
		/// <summary>
		/// secondary member<br/>
		/// set using 'ValueComparisonOpCode'<br/>
		/// represents the readable form of the compare op code
		/// </summary>
		[IgnoreDataMember]
		public string ValueCompareString => ValueCompOpDef.Name;

		// track changes: yes
		// consts - object does not modification

		/// <summary>
		/// secondary member<br/>
		/// set using 'ValueComparisonOpCode'<br/>
		/// represents the data about the comp op code
		/// </summary>
		[IgnoreDataMember]
		public ValueCompOpDef ValueCompOpDef
		{
			get => valueCompOpDef;

			protected set
			{
				if (value == valueCompOpDef) return;

				valueCompOpDef = value;

				valueComparisonOpCode = value.OpCode;

				OnPropertyChanged();
				OnPropertyChanged(nameof(ValueComparisonOpCode));
				OnPropertyChanged(nameof(ValueCompareString));

				if (IsInitialized) CompOpModified = true;
			}
		}


		// track changes: yes
		/// <summary>
		/// the comparison value
		/// </summary>
		[DataMember(Order = 5)]
		public string CompareValue
		{
			get => compareValue;
			set
			{
				if (value == compareValue) return;

				compareValue = value;

				OnPropertyChanged();

				if (IsInitialized) CompOpModified = true;
			}
		}


		// logical comparisons

		// track changes: no - track via LogicalCompOpDef
		// enum
		/// <summary>
		/// primary property that defines the value comparison
		/// </summary>
		[DataMember(Order = 3)]
		public LogicalComparisonOp LogicalComparisonOpCode
		{
			get => logicalComparisonOpCode;
			// get => logicalCompOp?.OpCode ?? LOGICAL_NO_OP;
			set
			{
				if (value == logicalComparisonOpCode) return;

				logicalComparisonOpCode = value;

				LogicalCompOpCode = (int) value;

				if (IsInitialized) CompOpModified = true;
			}
		}

		// track changes: no
		/// <summary>
		/// secondary member<br/>
		/// set using 'LogicalComparisonOpCode'<br/>
		/// represents ???
		/// </summary>
		[IgnoreDataMember]
		public abstract int LogicalCompOpCode { get; set; }

		// track changes: no
		/// <summary>
		/// secondary member<br/>
		/// set using 'LogicalComparisonOpCode'<br/>
		/// represents the readable form of the compare op code
		/// </summary>
		[IgnoreDataMember]
		public string LogicalCompareString => LogicalCompOpDef?.Name ?? "";

		// track changes: yes
		// [DataMember(Order = 1)]
		// consts - object does not modification
		/// <summary>
		/// secondary member<br/>
		/// set using 'ValueComparisonOpCode'<br/>
		/// represents the data about the comp op code
		/// </summary>
		[IgnoreDataMember]
		public LogicalCompOpDef LogicalCompOpDef
		{
			get => logicalCompOp;

			protected set
			{
				if (value == logicalCompOp) return;

				logicalCompOp = value;

				logicalComparisonOpCode = value?.OpCode ?? LOGICAL_NO_OP;

				OnPropertyChanged();
				OnPropertyChanged(nameof(LogicalComparisonOpCode));
				OnPropertyChanged(nameof(LogicalCompareString));

				if (IsInitialized) CompOpModified = true;
			}
		}


		// flags

		// track changes: yes
		[DataMember(Order = 15)]
		public bool IsDisabled
		{
			get => isDsableCompOp;
			set
			{
				if (value == isDsableCompOp) return;

				isDsableCompOp = value;

				OnPropertyChanged();

				if (IsInitialized) CompOpModified = true;
			}
		}

		// track changes: no
		[IgnoreDataMember]
		public bool IsFirstCompOp
		{
			get => logicalCompOp == null;
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

		[IgnoreDataMember]
		public bool IsSelected
		{
			get => isSelected;
			set
			{
				if (value == isSelected) return;
				isSelected = value;
				OnPropertyChanged();
			}
		}

		/*inter object communication*/

		// track changes: no
		// means that this object has been modified
		// but only this object
		[IgnoreDataMember]
		public bool CompOpModified
		{
			get => compOpModified;
			set
			{
				if (value == compOpModified) return;

				// Debug.WriteLine($"{"comp op",-26}|{$"CompOpModified bool"}");

				compOpModified = value;

				OnPropertyChanged();

				if (value) notifyParentOfCompOpChange(INTERNODE_MESSAGES.IM_IS_MODIFIED);
			}
		}

		// track changes: no
		// temp routine to modify this object
		[IgnoreDataMember]
		public bool ModifyCompOp
		{
			get => modifyCompOp;
			set
			{
				if (value == modifyCompOp || !value) return;
				// Debug.WriteLine($"\n******* {nameof(ModifyCompOp)} *** set to {value} *********");
				modifyCompOp = value;
				OnPropertyChanged();

				if (value) notifyParentOfCompOpChange(INTERNODE_MESSAGES.IM_IS_MODIFIED);
			}
		}

		// ui properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void ClearCompOpModified(INTERNODE_MESSAGES modification)
		{
			// Debug.WriteLine($"{CompNameData.Name,-26}|{$"ClearCompOpModified"}");
			if (!compOpModified) return;
			compOpModified = false;
			OnPropertyChanged(nameof(CompOpModified));

			modifyCompOp = false;
			OnPropertyChanged(nameof(ModifyCompOp));
		}

		/// <summary>
		/// modification the parent of the compOp without flagging a modification
		/// </summary>
		/// <param name="newParent"></param>
		public void ChangeParentNoMod(SheetCategory newParent)
		{
			parent = newParent;
			OnPropertyChanged(nameof(Parent));
		}

		public void UpdateProps()
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(null));
		}

		public static ComparisonOp CreateInitialCompOp(SheetCategory item)
		{
			ComparisonOp vco = new ComparisonOp(LOGICAL_AND, DOES_NOT_EQUAL, "1", 2);
			vco.isInitialized = true;
			vco.Parent = item;

			return vco;
		}

		public void Deselect()
		{
			IsSelected = false;
			CompOpModified = false;
		}

	#endregion

	#region private methods

		private void notifyParentOfCompOpChange(INTERNODE_MESSAGES modification)
		{
			// Debug.WriteLine($"{CompNameData.Name,-26}|{nameof(notifyParentOfCompOpChange),-30} | {modification}");

			compOpModified = true;
			OnPropertyChanged(nameof(CompOpModified));

			parent.CompOpChangeFromChild(modification);
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


		// public delegate void ComparisonOpChangedEventHandler(object sender, string memberName);
		//
		// public event ComparisonOpChangedEventHandler ComparisonOpChanged;
		//
		// protected virtual void RaiseComparisonOpChangedEvent([CallerMemberName] string memberName = "")
		// {
		// 	ComparisonOpChanged?.Invoke(this, memberName);
		// }

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ComparisonOperation";
		}

		public abstract object Clone();

		public abstract ComparisonOp Clone(SheetCategory parent);

		public abstract void Merge(SheetCategory parent, ComparisonOperation op);

	#endregion
	}

	[DataContract(Namespace = "")]
	public class ComparisonOp : ComparisonOperation
	{
//		public ComparisonOp(ValueCompOpDef op, string value, bool isFirst = false) : this(op, value, isFirst) { }

//		public ComparisonOp(LogicalCompOpDef l_op,
//			ValueCompOpDef v_op,
//			string value,
//			int compComponentIndex,
//			bool disable = false)
//		{
//			ValueComparisonOpCode = v_op.OpCode;
//			LogicalComparisonOpCode = l_op?.OpCode ?? LOGICAL_NO_OP;
//			// ValueCompOpDef = v_op;
//			// LogicalCompOpDef = l_op;
//			CompareValue = value;
//			CompareComponentIndex = compComponentIndex;
//			IsDisabled = disable;
////			Id = row;
//		}

		public ComparisonOp(
			LogicalComparisonOp l_op_code,
			ValueComparisonOp v_op_code,
			string value,
			int compComponentIndex,
			bool disable = false)
		{
			ValueComparisonOpCode = v_op_code;
			LogicalComparisonOpCode = l_op_code;

			CompareValue = value;
			CompareComponentIndex = compComponentIndex;
			IsDisabled = disable;

			// IsInitialized = true;
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext c)
		{
			// IsInitialized = true;
		}

		[IgnoreDataMember]
		public override int ValueCompOpCode
		{
			get => valueCompOpDef?.OpCodeValue ?? 0;
			set
			{
				// ValueCompOpDef = ValueCompareOps[value];
				if (value == 0)
				{
					ValueCompOpDef = null;
				}
				else
				{
					ValueCompOpDef = ValueCompareOps[value];
				}
			}
		}

		[IgnoreDataMember]
		public override int LogicalCompOpCode
		{
			get => logicalCompOp?.OpCodeValue ?? 0;
			set
			{
				// LogicalCompOpDef = LogicalCompareOps[value];
				if (value == 0)
				{
					// LogicalCompOpDef = LogicalCompareOps[(int) LOGICAL_NO_OP];
					LogicalCompOpDef = null;
				}
				else
				{
					LogicalCompOpDef = LogicalCompareOps[value];
				}
			}
		}

		public override object Clone()
		{
			ComparisonOp clone = new ComparisonOp(
				logicalComparisonOpCode,
				valueComparisonOpCode,
				compareValue,
				compareComponentIndex,
				isDsableCompOp);

			clone.compOpModified = false;
			clone.Id = Id;
			clone.isSelected = isSelected;

			clone.isInitialized = false;
			clone.parent = null;

			return clone;
		}

		public override ComparisonOp Clone(SheetCategory parent)
		{
			ComparisonOp clone = (ComparisonOp) Clone();
			clone.parent = parent;
			clone.isInitialized = true;

			return clone;
		}

		public override void Merge(SheetCategory parent, ComparisonOperation op)
		{
			logicalComparisonOpCode = op.LogicalComparisonOpCode;
			// valueComparisonOpCode = op.ValueComparisonOpCode;
			compareValue = op.CompareValue;
			compareComponentIndex = op.CompareComponentIndex;
			isDsableCompOp = op.IsDisabled;

			Id = op.Id;

			isSelected = false;

			ValueCompOpDef = op.ValueCompOpDef;
			LogicalCompOpDef = op.LogicalCompOpDef;

			this.parent = parent;

			isInitialized = true;
			compOpModified = false;
			CompOpModified = true;

			UpdateProps();
		}
	}


	[DataContract(Namespace = "")]
	[KnownType(typeof(ValueCompOpDef))]
	[KnownType(typeof(LogicalCompOpDef))]
	public abstract class ACompareOp<T> where T : Enum
	{
		public ACompareOp() { }

		public ACompareOp(string name, T op)
		{
			Name = name;
			OpCode = op;
		}

		[DataMember]
		public abstract string Name { get; protected set;  }

		[DataMember]
		public abstract T OpCode { get; set;  }

		[IgnoreDataMember]
		public int OpCodeValue =>  Convert.ToInt32(OpCode);

		public abstract object Clone();
	}

	[DataContract(Namespace = "")]
	public class ValueCompOpDef : ACompareOp<ValueComparisonOp>
	{
		public ValueCompOpDef() { }

		public ValueCompOpDef(string name, ValueComparisonOp op) : base(name, op) { }

		public override string Name { get; protected set; }

		public override ValueComparisonOp OpCode { get; set; }

		public override object Clone()
		{
			return new ValueCompOpDef(Name, OpCode);
		}
	}

	[DataContract(Namespace = "")]
	public class LogicalCompOpDef : ACompareOp<LogicalComparisonOp>
	{
		public LogicalCompOpDef() { }

		public LogicalCompOpDef(string name, LogicalComparisonOp op) : base(name, op) { }

		public override string Name { get; protected set;  }

		public override LogicalComparisonOp OpCode { get; set;  }

		public override object Clone()
		{
			return new LogicalCompOpDef(Name, OpCode);
		}
	}


	/// <summary>
	/// Static methods using / supporting the compare ops
	/// </summary>
	public static class CompareOperations
	{
		static  CompareOperations()
		{
			defineLogicalCompareOps();
			defineValueCompareOps();
		}

	#region public properties

		public static List<LogicalCompOpDef> LogicalCompareOps { get; private set; }

		// public static ICollectionView LogicalView { get; private set; }

		public static List<ValueCompOpDef> ValueCompareOps { get; private set; }

		// public static ICollectionView ValueView { get; private set; }

		private static int depth = 0;

		public static int Depth
		{
			get => depth;
			set { depth = value * 2 + 2; }
		}

		public static bool Compare2(FileNameSheetPdf Pdf,
			ObservableCollection<ComparisonOperation> compareOps, out int compareCount)
		{
			string marginStr = "  ";

			int count = 0;
			bool result = false;
			int compIdx;
			string compValue = "";

		#if SHOW
			Debug.WriteLine(marginStr.Repeat(depth) + "\t** start compare testing");
			Debug.WriteLine(marginStr.Repeat(depth) + "\tComparing| " + Pdf.SheetID);
			Debug.WriteLine(marginStr.Repeat(depth) + "\tCompare count| " + compareOps.Count);
		#endif

			foreach (ComparisonOperation compOp in compareOps)
			{
			#if SHOW
				Debug.WriteLine($"{marginStr.Repeat(depth)}\t** compare # {compOp.CompNameData.Name} #");
			#endif

				if (compOp.IsDisabled == true)
				{
					count++;

				#if SHOW
					Debug.WriteLine(marginStr.Repeat(depth) + "\tcont a");
				#endif

					continue;
				}

				compIdx = compOp.CompareComponentIndex;

				compValue = Pdf[compIdx];


			#if SHOW
				Debug.WriteLine(marginStr.Repeat(depth) + "\tCompare idx| " + compIdx + " :: " + compOp.CompareComponentName);
				Debug.WriteLine(marginStr.Repeat(depth) + "\tCompare test value| >" + compValue + "<");
			#endif

				if (compOp.IsFirstCompOp)
				{
				#if SHOW
					Debug.WriteLine(marginStr.Repeat(depth) + "\t" + count++ + "  1st Compare against| " + compOp.ValueCompOpDef.Name + " value| " + compOp.CompareValue);
				#endif
					result = compare(compValue, compOp);

					// 1 for first comp, 1 for compare
					count += 2;
				}
				else
				{
					if (compOp.LogicalCompOpDef.OpCode.Equals(LOGICAL_AND))
					{
					#if SHOW
						Debug.WriteLine(marginStr.Repeat(depth) + "\t" + count++ + "  AND Compare against| " + compOp.ValueCompOpDef.Name + " value| " + compOp.CompareValue);
					#endif
						result &= compare(compValue, compOp);

						// 1 for first comp, 1 for and, 1 for compare
						count += 3;
					}
					else
					{
					#if SHOW
						Debug.WriteLine(marginStr.Repeat(depth) + "\t" + count++ + "  OR  Compare against| " + compOp.ValueCompOpDef.Name + " value| " + compOp.CompareValue);
					#endif
						result |= compare(compValue, compOp);

						// 1 for first comp, 1 for and, 1 for compare
						count += 3;
					}

				#if SHOW
					Debug.WriteLine(marginStr.Repeat(depth) + "\t" + "   *** Partial Result| " + result);
				#endif

					if (!result) break;
				}
			}

		#if SHOW
			Debug.WriteLine(marginStr.Repeat(depth) + "\t" + "   *** Final Result| " + result);
		#endif

			compareCount = count;

		#if SHOW
			Debug.WriteLine(marginStr.Repeat(depth) + "\t** end compare testing");
		#endif

			return result;
		}

		public static bool Compare(string value, ObservableCollection<ComparisonOperation> CompareOps)
		{
			bool result = false;

			foreach (ComparisonOperation compareOp in CompareOps)
			{
				if (compareOp.IsDisabled == true) continue;

				if (compareOp.IsFirstCompOp)
				{
					result = compare(value, compareOp);
				}
				else if (compareOp.LogicalCompOpDef.OpCode.Equals(LOGICAL_AND))
				{
					result &= compare(value, compareOp);
				}
				else
				{
					result |= compare(value, compareOp);
				}

				if (!result) break;
			}

			return result;
		}

	#endregion

	#region private methods

		private static void defineLogicalCompareOps()
		{
			LogicalCompareOps = new List<LogicalCompOpDef>();

			configureCompareOpList(LogicalCompareOps, (int) LOGICAL_COUNT);

			setLogicalCompareOp(LogicalCompareOps, "No Op", LOGICAL_NO_OP);
			setLogicalCompareOp(LogicalCompareOps, "And", LOGICAL_AND);
			setLogicalCompareOp(LogicalCompareOps, "Or", LOGICAL_OR);
		}

		private static void defineValueCompareOps()
		{
			ValueCompareOps = new List<ValueCompOpDef>();

			configureCompareOpList(ValueCompareOps, (int) VALUE_COUNT);

			setValueCompareOp(ValueCompareOps, "No Op", VALUE_NO_OP);
			setValueCompareOp(ValueCompareOps, "Is Less Than or Equal", LESS_THAN_OR_EQUAL);
			setValueCompareOp(ValueCompareOps, "Is Less Than", LESS_THAN);
			setValueCompareOp(ValueCompareOps, "Is Greater Than or Equal", GREATER_THAN_OR_EQUAL);
			setValueCompareOp(ValueCompareOps, "Is Greater Than", GREATER_THAN);
			setValueCompareOp(ValueCompareOps, "Is Exactly Equal", EQUALTO);
			setValueCompareOp(ValueCompareOps, "Is Not Exactly Equal", DOES_NOT_EQUAL);
			setValueCompareOp(ValueCompareOps, "Contains", CONTAINS );
			setValueCompareOp(ValueCompareOps, "Does Not Contain", DOES_NOT_CONTAIN );
			setValueCompareOp(ValueCompareOps, "Starts with", STARTS_WITH );
			setValueCompareOp(ValueCompareOps, "Does Not Start with", DOES_NOT_START_WITH);
			setValueCompareOp(ValueCompareOps, "Ends with", ENDS_WITH);
			setValueCompareOp(ValueCompareOps, "Does Not End with", DOES_NOT_END_WITH);
			setValueCompareOp(ValueCompareOps, "Matches the Pattern", MATCHES);
			setValueCompareOp(ValueCompareOps, "Does Not Match the Pattern", DOES_NOT_MATCH);

			// defineValueView();
		}

		// private static void defineValueView()
		// {
		// ValueView = CollectionViewSource.GetDefaultView(ValueCompareOps);
		//
		// ValueView.Filter = item =>
		// {
		// 	ValueCompOpDef op = item as ValueCompOpDef;
		// 	return op.OpCode != ValueComparisonOp.VALUE_NO_OP;
		// };
		// }

		private static void  setValueCompareOp(List<ValueCompOpDef> list, string name, ValueComparisonOp op)
		{
			list[(int) op] = new ValueCompOpDef(name, op);
		}

		private static void setLogicalCompareOp(List<LogicalCompOpDef> list, string name, LogicalComparisonOp op)
		{
			list[(int) op] = new LogicalCompOpDef(name, op);
		}

		private static void configureCompareOpList<T>(List<T> conditionList, int count)  where T : new()
		{
			for (var i = 0; i < count; i++)
			{
				conditionList.Add(new T());
			}
		}

		private static bool compare(string value, ComparisonOperation compareOp)
		{
			bool result = false;

			if (value == null) return result;

			switch (compareOp.ValueCompOpDef.OpCode)
			{
			case ValueComparisonOp.LESS_THAN_OR_EQUAL:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) <= 0;
					break;
				}
			case ValueComparisonOp.LESS_THAN:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) < 0;
					break;
				}
			case ValueComparisonOp.GREATER_THAN_OR_EQUAL:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) >= 0;
					break;
				}
			case ValueComparisonOp.GREATER_THAN:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) > 0;
					break;
				}
			case ValueComparisonOp.EQUALTO:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) == 0;
					break;
				}
			case ValueComparisonOp.DOES_NOT_EQUAL:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) != 0;
					break;
				}
			case ValueComparisonOp.CONTAINS:
				{
					result = value.Contains(compareOp.CompareValue);
					break;
				}
			case ValueComparisonOp.DOES_NOT_CONTAIN:
				{
					result = !value.Contains(compareOp.CompareValue);
					break;
				}
			case ValueComparisonOp.STARTS_WITH:
				{
					result = value.StartsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ValueComparisonOp.DOES_NOT_START_WITH:
				{
					result = !value.StartsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ValueComparisonOp.ENDS_WITH:
				{
					result = value.EndsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ValueComparisonOp.DOES_NOT_END_WITH:
				{
					result = !value.EndsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ValueComparisonOp.MATCHES:
				{
					try
					{
						result = (Regex.Match(value, compareOp.CompareValue)).Success;
					}
					catch
					{
						result = false;
					}

					break;
				}
			case ValueComparisonOp.DOES_NOT_MATCH:
				{
					try
					{
						result = !(Regex.Match(value, compareOp.CompareValue)).Success;
					}
					catch
					{
						result = false;
					}

					break;
				}
			}

			return result;
		}

	#endregion
	}


	// [DataContract(Namespace = "")]
	// public class LogicalCompOp : ComparisonOperation
	// {
	// 	public LogicalCompOp(LogicalCompOpDef op)
	// 	{
	// 		LogicalCompOpDef = op;
	// 		CompareValue = null;
	// 		IsDisabled = ignore;
	// 	}
	//
	// 	[IgnoreDataMember]
	// 	public override int CompareOpCode
	// 	{
	// 		get => valueCompOpDef.OpCodeValue;
	// 		set
	// 		{
	// 			LogicalCompOpDef = LogicalCompareOps[value];
	//
	// 		}
	// 	}
	//
	// 	public override object Clone()
	// 	{
	// 		LogicalCompOp clone = new LogicalCompOp((LogicalCompOpDef) valueCompOpDef, isDsableCompOp);
	//
	// 		return clone;
	// 	}
	// }


	// value conditions
	// match conditions
	// comparison conditions
	// logical conditions
}