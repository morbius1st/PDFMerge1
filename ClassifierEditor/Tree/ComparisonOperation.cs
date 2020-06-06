#region using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using static ClassifierEditor.Tree.ComparisonOp;

#endregion

// username: jeffs
// created:  5/31/2020 5:03:54 PM

namespace ClassifierEditor.Tree
{
	// value conditions
	// match conditions
	// comparison conditions
	// logical conditions

	[DataContract(Namespace = "")]
	[KnownType(typeof(ValueCondition))]
	[KnownType(typeof(LogicalCondition))]
	public abstract class ACompareCondition
	{
		public ACompareCondition() {}

		public ACompareCondition(string name, ComparisonOp op)
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
	public class ValueCondition : ACompareCondition
	{
		public ValueCondition() { }

		public ValueCondition(string name, ComparisonOp op) : base(name, op) { }

		public override string Name { get; set; }

		public override ComparisonOp OpCode { get; set; }
	}

	[DataContract(Namespace = "")]
	public class LogicalCondition : ACompareCondition
	{
		public LogicalCondition() {}

		public LogicalCondition(string name, ComparisonOp op) : base(name, op) { }

		public override string Name { get; set;  }

		public override ComparisonOp OpCode { get; set;  }
	}

	public static class CompareConditions
	{
		private static List<LogicalCondition> logicalConditionList;
		private static List<ValueCondition> valueConditionList;



		static CompareConditions()
		{
			defineLogicalConditions();
			defineValueConditions();

//			logicalConditionList = new List<LogicalCondition>();
//
//			logicalConditionList.Add(new LogicalCondition("Or", LOGICAL_OR));
//			logicalConditionList.Add(new LogicalCondition("And", LOGICAL_AND));
//
//
//			valueConditionList = new List<ValueCondition>();
//
//			valueConditionList.Add(new ValueCondition("No Op", NO_OP));

		}

		private static void defineLogicalConditions()
		{
			logicalConditionList = new List<LogicalCondition>();

			configureConditionList(logicalConditionList, 2);

			setLogicalCondition(logicalConditionList, "And", LOGICAL_AND);
			setLogicalCondition(logicalConditionList, "Or", LOGICAL_OR);
		}
		
		private static void defineValueConditions()
		{
			valueConditionList = new List<ValueCondition>();

			configureConditionList(valueConditionList, 2);

			setValueCondition(valueConditionList, "No Op", NO_OP);

		}

		private static void setValueCondition(List<ValueCondition> list, string name, ComparisonOp op)
		{
			list[(int) op] = new ValueCondition(name, op);
		}
		
		private static void setLogicalCondition(List<LogicalCondition> list, string name, ComparisonOp op)
		{
			list[(int) op] = new LogicalCondition(name, op);
		}

		private static void configureConditionList<T>(List<T> conditionList, int count)  where T : new()
		{
			for (var i = 0; i <count; i++)
			{
				conditionList.Add(new T());
			}
		}



		public static LogicalCondition LogicalOr     { get; set; } = new LogicalCondition("Or", LOGICAL_OR);
		public static LogicalCondition LogicalAnd     { get; set; } = new LogicalCondition("And", LOGICAL_AND);

		public static ValueCondition NoOp             { get; set; } = new ValueCondition("No Op", NO_OP);

		public static ValueCondition LessThanOrEq     { get; set; } =
			new ValueCondition("Is Less Than or Equal", LESS_THAN_OR_EQUAL);

		public static ValueCondition LessThan         { get; set; } = new ValueCondition("Is Less Than", LESS_THAN);

		public static ValueCondition GreaterThanOrEq  { get; set; } =
			new ValueCondition("Is Greater Than or Equal", GREATER_THAN_OR_EQUAL);

		public static ValueCondition GreaterThan      { get; set; } =
			new ValueCondition("Is Greater Than", GREATER_THAN);

		public static ValueCondition EqualTo               { get; set; } =
			new ValueCondition("Is Exactly Equal to the Phrase", EQUALS);

		public static ValueCondition DoesNotEq        { get; set; } =
			new ValueCondition("Is Not Exactly Equal to the Phrase", DOES_NOT_EQUAL);

		public static ValueCondition Contains         { get; set; } =
			new ValueCondition("Contains the Phrase", CONTAINS);

		public static ValueCondition DoesNotContain   { get; set; } =
			new ValueCondition("Does Not Contain the Phrase", DOES_NOT_CONTAIN);

		public static ValueCondition StartsWith       { get; set; } =
			new ValueCondition("Starts with the Phrase", STARTS_WITH);

		public static ValueCondition DoesNotStartWith { get; set; } =
			new ValueCondition("Does Not Start with the Phrase", DOES_NOT_START_WITH);

		public static ValueCondition EndsWith         { get; set; } =
			new ValueCondition("Ends with the Phrase", ENDS_WITH);

		public static ValueCondition DoesNotEndWidth  { get; set; } =
			new ValueCondition("Does Not End with the Phrase", DOES_NOT_END_WITH);

		public static ValueCondition Matches          { get; set; } =
			new ValueCondition("Matches this Pattern", MATCHES);

		public static ValueCondition DoesNotMatch     { get; set; } =
			new ValueCondition("Does Not Match this Pattern", DOES_NOT_MATCH);
	}

	public enum ComparisonOp
	{
		LOGICAL_OR            = 0,
		LOGICAL_AND           ,
		NO_OP                  = 0,
		LESS_THAN_OR_EQUAL    ,
		LESS_THAN             ,
		GREATER_THAN_OR_EQUAL ,
		GREATER_THAN          ,
		EQUALS                ,
		DOES_NOT_EQUAL        ,
		CONTAINS              ,
		STARTS_WITH           ,
		DOES_NOT_START_WITH   ,
		ENDS_WITH             ,
		DOES_NOT_END_WITH     ,
		MATCHES               ,
		DOES_NOT_MATCH        ,
		DOES_NOT_CONTAIN      ,
	}


	[DataContract(Namespace = "")]
	public class ComparisonOperation : INotifyPropertyChanged
	{
	#region private fields

		private ACompareCondition coOp = CompareConditions.NoOp;

		private string compareValue = null;


	#endregion

	#region ctor

		public ComparisonOperation(ValueCondition op, string value)
		{
			CompareCondition = op;
			CompareValue = value;
		}
		
		public ComparisonOperation(LogicalCondition op)
		{
			CompareCondition = op;
			CompareValue = null;
		}

	#endregion

	#region public properties

		[IgnoreDataMember]
		public string CompareString => CompareCondition.Name;

		[DataMember(Order = 2)]
		public string CompareValue
		{
			get => compareValue;
			set
			{
				compareValue = value;

				OnPropertyChange();
			}
		}

		[DataMember(Order = 1)]
		public ACompareCondition CompareCondition
		{
			get => coOp;

			set
			{
				coOp = value;
				OnPropertyChange();
				OnPropertyChange("CompareString");
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this,  new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ComparisonOperation";
		}

	#endregion
	}
}