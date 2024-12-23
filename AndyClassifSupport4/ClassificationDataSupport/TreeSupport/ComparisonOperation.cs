﻿// #define SHOW

#region using directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.FileSupport.FileNameSheetPDF4;
using AndyShared.Support;
using UtilityLibrary;
using static AndyShared.ClassificationDataSupport.TreeSupport.ValueComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.LogicalComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations;
using  static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
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
	[KnownType(typeof(ValueCompOp))]
	public abstract class ComparisonOperation : INotifyPropertyChanged, ICloneable
	{
	#region private fields

		protected ValueCompareOp valueCompOp = ValueCompareOps[(int) VALUE_NO_OP];
		protected LogicalCompOpDef logicalCompOp = null;

		protected int compareComponentIndex;

		protected string compareValue;
		protected bool isDsableCompOp;

		protected bool isInitialized;
		protected bool isModified;

		protected LogicalComparisonOp logicalComparisonOpCode = LOGICAL_NO_OP;
		protected ValueComparisonOp valueComparisonOpCode = VALUE_NO_OP;

		private Orator.ConfRoom.Announcer OnModifiedAnnouncer;

	#endregion

	#region ctor

		public ComparisonOperation()
		{
			OnCreated();
		}

		private void OnCreated()
		{
			OnModifiedAnnouncer = Orator.GetAnnouncer(this, OratorRooms.MODIFIED);

			// listen to parent, changes have been saved
			Orator.Listen(OratorRooms.SAVED, OnOratorAnnouncedSaved);

			// listen to parent, initialize
			Orator.Listen(OratorRooms.TN_INIT, OnAnnounceTnInit);
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext c)
		{
			OnCreated();
		}

	#endregion

	#region public properties

		[IgnoreDataMember]
		public int Id { get; set; }

		[IgnoreDataMember]
		public string ValueCompareString => ValueCompareOp.Name;

		[IgnoreDataMember]
		public string LogicalCompareString => LogicalCompOpDef?.Name ?? "";

		[DataMember(Order = 0)]
		public int CompareComponentIndex
		{
			get => compareComponentIndex;
			set
			{
				if (value == compareComponentIndex) return;

				compareComponentIndex = value;

				OnPropertyChange();
				OnPropertyChange("CompareComponentName");
				OnPropertyChange("CompNameData");

				IsModified = true;
			}
		}

		[IgnoreDataMember]
		public ShtNumComps2 CompNameData => ShtIds.SheetNumComponentData[compareComponentIndex * 2];

		[IgnoreDataMember]
		public string CompareComponentName
		{
			get => ShtIds.SheetNumComponentData[compareComponentIndex * 2].Name;
		}
		//
		// [IgnoreDataMember]
		// public string CompareComponentDescPreface
		// {
		// 	get =>ShtIds.SheetNumberComponentTitles[compareComponentIndex].Neumonic.Preface;
		// }
		//
		// [IgnoreDataMember]
		// public string CompareComponentDescBody
		// {
		// 	get =>ShtIds.SheetNumberComponentTitles[compareComponentIndex].Neumonic.Body;
		// }
		//
		// [IgnoreDataMember]
		// public string CompareComponentDescSuffix
		// {
		// 	get =>ShtIds.SheetNumberComponentTitles[compareComponentIndex].Neumonic.Suffix;
		// }

		[DataMember(Order = 3)]
		public LogicalComparisonOp LogicalComparisonOpCode
		{
			get => logicalComparisonOpCode;
			// get => logicalCompOp?.OpCode ?? LOGICAL_NO_OP;
			set
			{
				logicalComparisonOpCode = value;

				LogicalCompOpCode = (int) value;
			}
		}

		[IgnoreDataMember]
		public abstract int LogicalCompOpCode { get; set; }

		// [DataMember(Order = 1)]
		[IgnoreDataMember]
		public LogicalCompOpDef LogicalCompOpDef
		{
			get => logicalCompOp;

			protected set
			{
				if (value == logicalCompOp) return;

				logicalCompOp = value;

				logicalComparisonOpCode = value.OpCode;

				OnPropertyChange();
				OnPropertyChange("LogicalCompareString");

				IsModified = true;
			}
		}

		[DataMember(Order = 4)]
		public ValueComparisonOp ValueComparisonOpCode
		{
			get => valueComparisonOpCode;
			// get => valueCompOp?.OpCode ?? VALUE_NO_OP;
			set
			{
				valueComparisonOpCode = value;

				ValueCompOpCode = (int) value;
			}
		}

		[IgnoreDataMember]
		public abstract int ValueCompOpCode { get; set; }

		// [DataMember(Order = 2)]
		[IgnoreDataMember]
		public ValueCompareOp ValueCompareOp
		{
			get => valueCompOp;

			protected set
			{
				if (value == valueCompOp) return;

				valueCompOp = value;

				valueComparisonOpCode = value.OpCode;

				OnPropertyChange();
				OnPropertyChange("ValueCompareString");

				IsModified = true;
			}
		}

		[DataMember(Order = 5)]
		public string CompareValue
		{
			get => compareValue;
			set 
			{
				if (value == compareValue) return;

				compareValue = value;

				OnPropertyChange();

				IsModified = true;
			}
		}

		[DataMember(Order = 15)]
		public bool IsDisabled
		{
			get => isDsableCompOp;
			set
			{
				isDsableCompOp = value;

				OnPropertyChange();

				IsModified = true;
			}
		}

		[IgnoreDataMember]
		public bool IsFirstCompOp
		{
			get => logicalCompOp == null;
		}

		[IgnoreDataMember]
		public bool IsModified
		{
			get => isModified;
			private set
			{
				if (value == isModified) return;

				isModified = value;

				OnPropertyChange();

				if (isInitialized)
				{
					OnModifiedAnnouncer.Announce(null);
				}
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

		private void OnOratorAnnouncedSaved(object sender, object value)
		{
			isModified = false;
		}


		private void OnAnnounceTnInit(object sender, object value)
		{
			isInitialized = true;
			isModified = false;
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ComparisonOperation";
		}

		public abstract object Clone();

	#endregion
	}

	[DataContract(Namespace = "")]
	public class ValueCompOp : ComparisonOperation
	{
//		public ValueCompOp(ValueCompareOp op, string value, bool isFirst = false) : this(op, value, isFirst) { }

//		public ValueCompOp(LogicalCompOpDef l_op,
//			ValueCompareOp v_op,
//			string value,
//			int compComponentIndex,
//			bool disable = false)
//		{
//			ValueComparisonOpCode = v_op.OpCode;
//			LogicalComparisonOpCode = l_op?.OpCode ?? LOGICAL_NO_OP;
//			// ValueCompareOp = v_op;
//			// LogicalCompOpDef = l_op;
//			CompareValue = value;
//			CompareComponentIndex = compComponentIndex;
//			IsDisabled = disable;
////			Id = row;
//		}

		public ValueCompOp(LogicalComparisonOp l_op_code,
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

			isInitialized = true;
		}

		[IgnoreDataMember]
		public override int ValueCompOpCode
		{
			get => valueCompOp?.OpCodeValue ?? 0;
			set
			{
				// ValueCompareOp = ValueCompareOps[value];
				if (value == 0)
				{
					ValueCompareOp = null;
				}
				else
				{
					ValueCompareOp = ValueCompareOps[value];
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
			ValueCompOp clone = new ValueCompOp(logicalComparisonOpCode, valueComparisonOpCode, compareValue,
				compareComponentIndex, isDsableCompOp);

			return clone;
		}
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
	// 		get => valueCompOp.OpCodeValue;
	// 		set
	// 		{
	// 			LogicalCompOpDef = LogicalCompareOps[value];
	//
	// 		}
	// 	}
	//
	// 	public override object Clone()
	// 	{
	// 		LogicalCompOp clone = new LogicalCompOp((LogicalCompOpDef) valueCompOp, isDsableCompOp);
	//
	// 		return clone;
	// 	}
	// }


	// value conditions
	// match conditions
	// comparison conditions
	// logical conditions

	[DataContract(Namespace = "")]
	[KnownType(typeof(ValueCompareOp))]
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
	}

	[DataContract(Namespace = "")]
	public class ValueCompareOp : ACompareOp<ValueComparisonOp>
	{
		public ValueCompareOp() { }

		public ValueCompareOp(string name, ValueComparisonOp op) : base(name, op) { }

		public override string Name { get; protected set; }

		public override ValueComparisonOp OpCode { get; set; }
	}

	[DataContract(Namespace = "")]
	public class LogicalCompOpDef : ACompareOp<LogicalComparisonOp>
	{
		public LogicalCompOpDef() { }

		public LogicalCompOpDef(string name, LogicalComparisonOp op) : base(name, op) { }

		public override string Name { get; protected set;  }

		public override LogicalComparisonOp OpCode { get; set;  }
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

		public static List<ValueCompareOp> ValueCompareOps { get; private set; }

		// public static ICollectionView ValueView { get; private set; }

		private static int depth = 0;

		public static int Depth
		{
			get => depth;
			set
			{
				depth = value * 2 + 2;
			}
		}

		public static bool Compare2(FileNameSheetPdf4 Pdf,  
			ObservableCollection<ComparisonOperation> compareOps)
		{
			int count = 0;
			bool result = false;
			int compIdx;
			string compValue = "";

		#if SHOW
			Debug.WriteLine("\t".Repeat(depth) + "Comparing| " + Pdf.SheetID);
			Debug.WriteLine("\t".Repeat(depth) + "Compare count| " + compareOps.Count);
		#endif
			
			foreach (ComparisonOperation compOp in compareOps)
			{
				if (compOp.IsDisabled) continue;

				compIdx = compOp.CompareComponentIndex;

				compValue = Pdf[compIdx];

			#if SHOW
				Debug.WriteLine("\t".Repeat(depth) + "Compare idx| " + compIdx + " :: " + compOp.CompareComponentName);
				Debug.WriteLine("\t".Repeat(depth) + "Compare value| " + compValue);
			#endif

				if (compOp.IsFirstCompOp)
				{
				#if SHOW
					Debug.WriteLine("\t".Repeat(depth) + count++ + "  1st Compare against| " + compOp.ValueCompareOp.Name + " value| " + compOp.CompareValue);
				#endif
					result = compare(compValue, compOp);
				}
				else
				{
					if (compOp.LogicalCompOpDef.OpCode.Equals(LOGICAL_AND))
					{
					#if SHOW
						Debug.WriteLine("\t".Repeat(depth) + count++ + "  AND Compare against| " + compOp.ValueCompareOp.Name + " value| " + compOp.CompareValue); 
					#endif
						result &= compare(compValue, compOp);
					}
					else
					{
					#if SHOW
						Debug.WriteLine("\t".Repeat(depth) + count++ + "  OR  Compare against| " + compOp.ValueCompareOp.Name + " value| " + compOp.CompareValue);
					#endif
						result |= compare(compValue, compOp);
					}

				#if SHOW
					Debug.WriteLine("\t".Repeat(depth) + "   *** Partial Result| " + result);
				#endif

					if (!result) break;
				}
			}

		#if SHOW
			Debug.WriteLine("\t".Repeat(depth) + "   *** Final Result| " + result);
		#endif
			return result;
		}


		public static bool Compare(string value, ObservableCollection<ComparisonOperation> CompareOps)
		{
			bool result = false;

			foreach (ComparisonOperation compareOp in CompareOps)
			{
				if (compareOp.IsDisabled) continue;

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

			// defineLogicalView();
		}

		// private static void defineLogicalView()
		// {
		// 	LogicalView = CollectionViewSource.GetDefaultView(LogicalCompareOps);
		//
		// 	LogicalView.Filter = item =>
		// 	{
		// 		LogicalCompOpDef op = item as LogicalCompOpDef;
		// 		return op.OpCode != LogicalComparisonOp.LOGICAL_NO_OP;
		// 	};
		// }

		private static void defineValueCompareOps()
		{
			ValueCompareOps = new List<ValueCompareOp>();

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
		// 	ValueCompareOp op = item as ValueCompareOp;
		// 	return op.OpCode != ValueComparisonOp.VALUE_NO_OP;
		// };
		// }

		private static void  setValueCompareOp(List<ValueCompareOp> list, string name, ValueComparisonOp op)
		{
			list[(int) op] = new ValueCompareOp(name, op);
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

			switch (compareOp.ValueCompareOp.OpCode)
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
}