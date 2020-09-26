#region using directives

// using ClassifierEditor.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AndyShared.Support;
using static AndyShared.ClassificationDataSupport.TreeSupport.ComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations;

#endregion

// username: jeffs
// created:  5/31/2020 5:03:54 PM

namespace AndyShared.ClassificationDataSupport.TreeSupport
{

	public enum ComparisonOp
	{
		LOGICAL_OR            = 0,
		LOGICAL_AND           ,
		LOGICAL_COUNT         ,
		NO_OP                 = 0,
		LESS_THAN_OR_EQUAL    ,
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

		protected ValueCompareOp valueCompOp = ValueCompareOps[(int) NO_OP];
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
	public abstract class ACompareOp
	{
		public ACompareOp() {}

		public ACompareOp(string name, ComparisonOp op)
		{
			Name = name;
			OpCode = op;
		}

		[DataMember]
		public abstract string Name { get; set;  }

		[DataMember]
		public abstract ComparisonOp OpCode { get; set;  }

		[IgnoreDataMember]
		public int OpCodeValue => (int) OpCode;
	}




	[DataContract(Namespace = "")]
	public class ValueCompareOp : ACompareOp
	{
		public ValueCompareOp() { }

		public ValueCompareOp(string name, ComparisonOp op) : base(name, op) { }

		public override string Name { get; set; }

		public override ComparisonOp OpCode { get; set; }
	}




	[DataContract(Namespace = "")]
	public class LogicalCompareOp : ACompareOp
	{
		public LogicalCompareOp() {}

		public LogicalCompareOp(string name, ComparisonOp op) : base(name, op) { }

		public override string Name { get; set;  }

		public override ComparisonOp OpCode { get; set;  }
	}




	public static class CompareOperations
	{
		public static List<LogicalCompareOp> LogicalCompareOps { get; private set; }
		public static List<ValueCompareOp> ValueCompareOps { get; private set; }

		static  CompareOperations()
		{
			defineLogicalCompareOps();
			defineValueCompareOps();
		}

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

			setValueCompareOp(ValueCompareOps, "No Op", NO_OP);
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

		private static void  setValueCompareOp(List<ValueCompareOp> list, string name, ComparisonOp op)
		{
			list[(int) op] = new ValueCompareOp(name, op);
		}
		
		private static void setLogicalCompareOp(List<LogicalCompareOp> list, string name, ComparisonOp op)
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
	}

}