#region using directives

// using ClassifierEditor.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using AndyShared.Support;
using static AndyShared.ClassificationDataSupport.TreeSupport.LogicalComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.ValueComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations;

#endregion

// username: jeffs
// created:  5/31/2020 5:03:54 PM

namespace AndyShared.ClassificationDataSupport.TreeSupport
{

	public enum LogicalComparisonOp
	{
		LOCICAL_NO_OP         = 0,
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


	//
	// public enum ComparisonOp
	// {
	// 	LOGICAL_OR            = 0,
	// 	LOGICAL_AND           ,
	// 	LOGICAL_COUNT         ,
	// 	NO_OP                 = 0,
	// 	LESS_THAN_OR_EQUAL    ,
	// 	LESS_THAN             ,
	// 	GREATER_THAN_OR_EQUAL ,
	// 	GREATER_THAN          ,
	// 	EQUALTO               ,
	// 	DOES_NOT_EQUAL        ,
	// 	CONTAINS              ,
	// 	STARTS_WITH           ,
	// 	DOES_NOT_START_WITH   ,
	// 	ENDS_WITH             ,
	// 	DOES_NOT_END_WITH     ,
	// 	MATCHES               ,
	// 	DOES_NOT_MATCH        ,
	// 	DOES_NOT_CONTAIN      ,
	// 	VALUE_COUNT
	// }


	[DataContract(Namespace = "")]
	[KnownType(typeof(ValueCompOp))]
	public abstract class ComparisonOperation : INotifyPropertyChanged, ICloneable
	{
	#region private fields

		protected ValueCompareOp valueCompOp = ValueCompareOps[(int) VALUE_NO_OP];
		protected LogicalCompareOp logicalCompOp = null;

		protected string compareValue;
		protected bool isDsableCompOp;

		protected bool isInitialized;
		protected bool isModified;

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
		public string LogicalCompareString => LogicalCompareOp?.Name ?? "";

		[DataMember(Order = 1)]
		public ValueCompareOp ValueCompareOp
		{
			get => valueCompOp;

			set
			{
				if (value == valueCompOp) return;

				valueCompOp = value;
				OnPropertyChange();
				OnPropertyChange("ValueCompareString");

				IsModified = true;
			}
		}

		[IgnoreDataMember]
		public abstract int ValueCompOpCode { get; set; }

		[DataMember(Order = 2)]
		public LogicalCompareOp LogicalCompareOp
		{
			get => logicalCompOp;

			set
			{
				if (value == logicalCompOp) return;

				logicalCompOp = value;
				OnPropertyChange();
				OnPropertyChange("LogicalCompareString");

				IsModified = true;
			}
		}

		[IgnoreDataMember]
		public abstract int LogicalCompOpCode { get; set; }

		[DataMember(Order = 5)]
		public string CompareValue
		{
			get => compareValue;
			set
			{
				compareValue = value;

				OnPropertyChange();
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
			}
		}

		[IgnoreDataMember]
		public bool IsFirstCompOp
		{
			get => logicalCompOp == null;
		}

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

		public ValueCompOp(LogicalCompareOp l_op,
			ValueCompareOp v_op,
			string value, 
			bool disable = false)
		{
			ValueCompareOp = v_op;
			LogicalCompareOp = l_op;
			CompareValue = value;
			IsDisabled = disable;
//			Id = row;
		}

		[IgnoreDataMember]
		public override int ValueCompOpCode
		{
			get => valueCompOp.OpCodeValue;
			set
			{
				ValueCompareOp = ValueCompareOps[value];
			}
		}
		
		[IgnoreDataMember]
		public override int LogicalCompOpCode
		{
			get => logicalCompOp?.OpCodeValue ?? 0;
			set
			{
				LogicalCompareOp = LogicalCompareOps[value];
			}
		}

		public override object Clone()
		{
			ValueCompOp clone = new ValueCompOp(logicalCompOp, valueCompOp, compareValue, isDsableCompOp);

			return clone;
		}
	}

	// [DataContract(Namespace = "")]
	// public class LogicalCompOp : ComparisonOperation
	// {
	// 	public LogicalCompOp(LogicalCompareOp op)
	// 	{
	// 		LogicalCompareOp = op;
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
	// 			LogicalCompareOp = LogicalCompareOps[value];
	//
	// 		}
	// 	}
	//
	// 	public override object Clone()
	// 	{
	// 		LogicalCompOp clone = new LogicalCompOp((LogicalCompareOp) valueCompOp, isDsableCompOp);
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
	[KnownType(typeof(LogicalCompareOp))]
	public abstract class ACompareOp<T> where T : Enum
	{
		public ACompareOp() {}

		public ACompareOp(string name, T op)
		{
			Name = name;
			OpCode = op;
		}

		[DataMember]
		public abstract string Name { get; set;  }

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

		public override string Name { get; set; }

		public override ValueComparisonOp OpCode { get; set; }
	}




	[DataContract(Namespace = "")]
	public class LogicalCompareOp : ACompareOp<LogicalComparisonOp>
	{
		public LogicalCompareOp() {}

		public LogicalCompareOp(string name, LogicalComparisonOp op) : base(name, op) { }

		public override string Name { get; set;  }

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

		public static List<LogicalCompareOp> LogicalCompareOps { get; private set; }

		public static List<ValueCompareOp> ValueCompareOps { get; private set; }

		public static bool Compare(string value, ObservableCollection<ComparisonOperation> CompareOps)
		{
			bool result = false;

			foreach (ComparisonOperation compareOp in CompareOps)
			{
				
				if (compareOp.IsFirstCompOp)
				{
					result = compare(value, compareOp);
				}
				else if (compareOp.LogicalCompareOp.OpCode.Equals(LogicalComparisonOp.LOGICAL_AND))
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

	#region private properties

		private static void defineLogicalCompareOps()
		{
			LogicalCompareOps = new List<LogicalCompareOp>();

			configureCompareOpList(LogicalCompareOps, (int) LOGICAL_COUNT);

			setLogicalCompareOp(LogicalCompareOps, "And", LOGICAL_AND);
			setLogicalCompareOp(LogicalCompareOps, "Or", LOGICAL_OR);
		}
		
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
		}

		private static void  setValueCompareOp(List<ValueCompareOp> list, string name, ValueComparisonOp op)
		{
			list[(int) op] = new ValueCompareOp(name, op);
		}
		
		private static void setLogicalCompareOp(List<LogicalCompareOp> list, string name, LogicalComparisonOp op)
		{
			list[(int) op] = new LogicalCompareOp(name, op);
		}

		private static void configureCompareOpList<T>(List<T> conditionList, int count)  where T : new()
		{
			for (var i = 0; i <count; i++)
			{
				conditionList.Add(new T());
			}
		}

		private static bool compare(string value, ComparisonOperation compareOp)
		{
			bool result = false;

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