// #define SHOW

#region using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.FileSupport.FileNameSheetPDF4;
using AndyShared.Support;
using UtilityLibrary;
using static AndyShared.ClassificationDataSupport.TreeSupport.LogicalComparisonOp4;
using static AndyShared.ClassificationDataSupport.TreeSupport.ValueComparisonOp4;
using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations4;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;

#endregion

// username: jeffs
// created:  5/31/2020 5:03:54 PM

namespace AndyShared.ClassificationDataSupport.TreeSupport
{
	public enum LogicalComparisonOp4
	{
		LOGICAL_NO_OP         = 0,
		LOGICAL_OR            = 1,
		LOGICAL_AND           ,
		LOGICAL_COUNT
	}

	public enum ValueComparisonOp4
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
	[KnownType(typeof(ValueCompOp4))]
	public abstract class ComparisonOperation4 : INotifyPropertyChanged, ICloneable
	{
	#region private fields

		protected ValueCompareOp4 ValueCompOp4 = ValueCompareOps[(int) VALUE_NO_OP];
		protected LogicalCompareOp4 logicalCompOp = null;

		protected int compareComponentIndex;

		protected string compareValue;
		protected bool isDsableCompOp;

		protected bool isInitialized;
		protected bool isModified;

		protected LogicalComparisonOp4 logicalComparisonOpCode = LOGICAL_NO_OP;
		protected ValueComparisonOp4 valueComparisonOpCode = VALUE_NO_OP;

		private Orator.ConfRoom.Announcer OnModifiedAnnouncer;

	#endregion

	#region ctor

		public ComparisonOperation4()
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
		public string ValueCompareString => ValueCompareOp4.Name;

		[IgnoreDataMember]
		public string LogicalCompareString => LogicalCompareOp4?.Name ?? "";

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
		public ShtNumberCompName CompNameData => ShtIds.SheetNumberComponentTitles[compareComponentIndex];

		[IgnoreDataMember]
		public string CompareComponentName
		{
			get => ShtIds.SheetNumberComponentTitles[compareComponentIndex].Name;
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
		public LogicalComparisonOp4 LogicalComparisonOpCode
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
		public LogicalCompareOp4 LogicalCompareOp4
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
		public ValueComparisonOp4 ValueComparisonOpCode
		{
			get => valueComparisonOpCode;
			// get => ValueCompOp4?.OpCode ?? VALUE_NO_OP;
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
		public ValueCompareOp4 ValueCompareOp4
		{
			get => ValueCompOp4;

			protected set
			{
				if (value == ValueCompOp4) return;

				ValueCompOp4 = value;

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
			return "this is ComparisonOperation4";
		}

		public abstract object Clone();

	#endregion
	}

	[DataContract(Namespace = "")]
	public class ValueCompOp4 : ComparisonOperation4
	{
//		public ValueCompOp4(ValueCompareOp4 op, string value, bool isFirst = false) : this(op, value, isFirst) { }

//		public ValueCompOp4(LogicalCompareOp4 l_op,
//			ValueCompareOp4 v_op,
//			string value,
//			int compComponentIndex,
//			bool disable = false)
//		{
//			ValueComparisonOpCode = v_op.OpCode;
//			LogicalComparisonOpCode = l_op?.OpCode ?? LOGICAL_NO_OP;
//			// ValueCompareOp4 = v_op;
//			// LogicalCompareOp4 = l_op;
//			CompareValue = value;
//			CompareComponentIndex = compComponentIndex;
//			IsDisabled = disable;
////			Id = row;
//		}

		public ValueCompOp4(LogicalComparisonOp4 l_op_code,
			ValueComparisonOp4 v_op_code,
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
			get => ValueCompOp4?.OpCodeValue ?? 0;
			set
			{
				// ValueCompareOp4 = ValueCompareOps[value];
				if (value == 0)
				{
					ValueCompareOp4 = null;
				}
				else
				{
					ValueCompareOp4 = ValueCompareOps[value];
				}
			}
		}

		[IgnoreDataMember]
		public override int LogicalCompOpCode
		{
			get => logicalCompOp?.OpCodeValue ?? 0;
			set
			{
				// LogicalCompareOp4 = LogicalCompareOps[value];
				if (value == 0)
				{
					LogicalCompareOp4 = null;
				}
				else
				{
					LogicalCompareOp4 = LogicalCompareOps[value];
				}
			}
		}

		public override object Clone()
		{
			ValueCompOp4 clone = new ValueCompOp4(logicalComparisonOpCode, valueComparisonOpCode, compareValue,
				compareComponentIndex, isDsableCompOp);

			return clone;
		}
	}

	// [DataContract(Namespace = "")]
	// public class LogicalCompOp : ComparisonOperation4
	// {
	// 	public LogicalCompOp(LogicalCompareOp4 op)
	// 	{
	// 		LogicalCompareOp4 = op;
	// 		CompareValue = null;
	// 		IsDisabled = ignore;
	// 	}
	//
	// 	[IgnoreDataMember]
	// 	public override int CompareOpCode
	// 	{
	// 		get => ValueCompOp4.OpCodeValue;
	// 		set
	// 		{
	// 			LogicalCompareOp4 = LogicalCompareOps[value];
	//
	// 		}
	// 	}
	//
	// 	public override object Clone()
	// 	{
	// 		LogicalCompOp clone = new LogicalCompOp((LogicalCompareOp4) ValueCompOp4, isDsableCompOp);
	//
	// 		return clone;
	// 	}
	// }


	// value conditions
	// match conditions
	// comparison conditions
	// logical conditions

	[DataContract(Namespace = "")]
	[KnownType(typeof(ValueCompareOp4))]
	[KnownType(typeof(LogicalCompareOp4))]
	public abstract class ACompareOp4<T> where T : Enum
	{
		public ACompareOp4() { }

		public ACompareOp4(string name, T op)
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
	public class ValueCompareOp4 : ACompareOp4<ValueComparisonOp4>
	{
		public ValueCompareOp4() { }

		public ValueCompareOp4(string name, ValueComparisonOp4 op) : base(name, op) { }

		public override string Name { get; protected set; }

		public override ValueComparisonOp4 OpCode { get; set; }
	}

	[DataContract(Namespace = "")]
	public class LogicalCompareOp4 : ACompareOp4<LogicalComparisonOp4>
	{
		public LogicalCompareOp4() { }

		public LogicalCompareOp4(string name, LogicalComparisonOp4 op) : base(name, op) { }

		public override string Name { get; protected set;  }

		public override LogicalComparisonOp4 OpCode { get; set;  }
	}

	/// <summary>
	/// Static methods using / supporting the compare ops
	/// </summary>
	public static class CompareOperations4
	{
		static  CompareOperations4()
		{
			defineLogicalCompareOps();
			defineValueCompareOps();
		}

	#region public properties

		public static List<LogicalCompareOp4> LogicalCompareOps { get; private set; }

		// public static ICollectionView LogicalView { get; private set; }

		public static List<ValueCompareOp4> ValueCompareOps { get; private set; }

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
			ObservableCollection<ComparisonOperation4> compareOps)
		{
			int count = 0;
			bool result = false;
			int compIdx;
			string compValue = "";

		#if SHOW
			Debug.WriteLine("\t".Repeat(depth) + "Comparing| " + Pdf.SheetID);
			Debug.WriteLine("\t".Repeat(depth) + "Compare count| " + compareOps.Count);
		#endif
			
			foreach (ComparisonOperation4 compOp in compareOps)
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
					Debug.WriteLine("\t".Repeat(depth) + count++ + "  1st Compare against| " + compOp.ValueCompareOp4.Name + " value| " + compOp.CompareValue);
				#endif
					result = compare(compValue, compOp);
				}
				else
				{
					if (compOp.LogicalCompareOp4.OpCode.Equals(LOGICAL_AND))
					{
					#if SHOW
						Debug.WriteLine("\t".Repeat(depth) + count++ + "  AND Compare against| " + compOp.ValueCompareOp4.Name + " value| " + compOp.CompareValue); 
					#endif
						result &= compare(compValue, compOp);
					}
					else
					{
					#if SHOW
						Debug.WriteLine("\t".Repeat(depth) + count++ + "  OR  Compare against| " + compOp.ValueCompareOp4.Name + " value| " + compOp.CompareValue);
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


		public static bool Compare(string value, ObservableCollection<ComparisonOperation4> CompareOps)
		{
			bool result = false;

			foreach (ComparisonOperation4 compareOp in CompareOps)
			{
				if (compareOp.IsDisabled) continue;

				if (compareOp.IsFirstCompOp)
				{
					result = compare(value, compareOp);
				}
				else if (compareOp.LogicalCompareOp4.OpCode.Equals(LOGICAL_AND))
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
			LogicalCompareOps = new List<LogicalCompareOp4>();

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
		// 		LogicalCompareOp4 op = item as LogicalCompareOp4;
		// 		return op.OpCode != LogicalComparisonOp4.LOGICAL_NO_OP;
		// 	};
		// }

		private static void defineValueCompareOps()
		{
			ValueCompareOps = new List<ValueCompareOp4>();

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
		// 	ValueCompareOp4 op = item as ValueCompareOp4;
		// 	return op.OpCode != ValueComparisonOp4.VALUE_NO_OP;
		// };
		// }

		private static void  setValueCompareOp(List<ValueCompareOp4> list, string name, ValueComparisonOp4 op)
		{
			list[(int) op] = new ValueCompareOp4(name, op);
		}

		private static void setLogicalCompareOp(List<LogicalCompareOp4> list, string name, LogicalComparisonOp4 op)
		{
			list[(int) op] = new LogicalCompareOp4(name, op);
		}

		private static void configureCompareOpList<T>(List<T> conditionList, int count)  where T : new()
		{
			for (var i = 0; i < count; i++)
			{
				conditionList.Add(new T());
			}
		}

		private static bool compare(string value, ComparisonOperation4 compareOp)
		{
			bool result = false;

			if (value == null) return result;

			switch (compareOp.ValueCompareOp4.OpCode)
			{
			case ValueComparisonOp4.LESS_THAN_OR_EQUAL:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) <= 0;
					break;
				}
			case ValueComparisonOp4.LESS_THAN:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) < 0;
					break;
				}
			case ValueComparisonOp4.GREATER_THAN_OR_EQUAL:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) >= 0;
					break;
				}
			case ValueComparisonOp4.GREATER_THAN:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) > 0;
					break;
				}
			case ValueComparisonOp4.EQUALTO:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) == 0;
					break;
				}
			case ValueComparisonOp4.DOES_NOT_EQUAL:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) != 0;
					break;
				}
			case ValueComparisonOp4.CONTAINS:
				{
					result = value.Contains(compareOp.CompareValue);
					break;
				}
			case ValueComparisonOp4.DOES_NOT_CONTAIN:
				{
					result = !value.Contains(compareOp.CompareValue);
					break;
				}
			case ValueComparisonOp4.STARTS_WITH:
				{
					result = value.StartsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ValueComparisonOp4.DOES_NOT_START_WITH:
				{
					result = !value.StartsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ValueComparisonOp4.ENDS_WITH:
				{
					result = value.EndsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ValueComparisonOp4.DOES_NOT_END_WITH:
				{
					result = !value.EndsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ValueComparisonOp4.MATCHES:
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
			case ValueComparisonOp4.DOES_NOT_MATCH:
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