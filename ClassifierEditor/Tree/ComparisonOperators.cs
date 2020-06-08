using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using static ClassifierEditor.Tree.EValueCompOp;
using static ClassifierEditor.Tree.ELogicalCompOp;

namespace ClassifierEditor.Tree
{
	public enum ELogicalCompOp
	{
		LOGICAL_OR            ,
		LOGICAL_AND           ,
		LOGICAL_COUNT
	}

	public enum EValueCompOp
	{
		NO_OP                 ,
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

		public ACompareOp(string name)
		{
			Name = name;
		}

		[DataMember]
		public string Name { get; set;  }

		[DataMember]
		public abstract T OpCode { get; set;  }

		[IgnoreDataMember]
		public abstract int OpCodeValue { get; set; }
	}

	[DataContract(Namespace = "")]
	public class ValueCompareOp : ACompareOp<EValueCompOp>
	{
		public ValueCompareOp() { }

		public ValueCompareOp(string name, EValueCompOp op) : base(name)
		{
			OpCode = op;
		}

		public override EValueCompOp OpCode { get; set; }

		public override int OpCodeValue
		{
			get => (int) OpCode;

			set { OpCode = (EValueCompOp) value; }
		}
	}

	[DataContract(Namespace = "")]
	public class LogicalCompareOp : ACompareOp<ELogicalCompOp>
	{
		public LogicalCompareOp() {}

		public LogicalCompareOp(string name, ELogicalCompOp op) : base(name)
		{
			OpCode = op;
		}

		public override ELogicalCompOp OpCode { get; set;  }


		public override int OpCodeValue
		{
			get => (int) OpCode;

			set { OpCode = (ELogicalCompOp) value; }
		}
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
			setValueCompareOp(ValueCompareOps, "Is Exactly Equal to the Phrase", EQUALTO);
			setValueCompareOp(ValueCompareOps, "Is Not Exactly Equal to the Phrase", DOES_NOT_EQUAL);
			setValueCompareOp(ValueCompareOps, "Contains the Phrase", CONTAINS );
			setValueCompareOp(ValueCompareOps, "Does Not Contain the Phrase", DOES_NOT_CONTAIN );
			setValueCompareOp(ValueCompareOps, "Starts with the Phrase", STARTS_WITH );
			setValueCompareOp(ValueCompareOps, "Does Not Start with the Phrase", DOES_NOT_START_WITH);
			setValueCompareOp(ValueCompareOps, "Ends with the Phrase", ENDS_WITH);
			setValueCompareOp(ValueCompareOps, "Does Not End with the Phrase", DOES_NOT_END_WITH);
			setValueCompareOp(ValueCompareOps, "Matches this Pattern", MATCHES);
			setValueCompareOp(ValueCompareOps, "Does Not Match this Pattern", DOES_NOT_MATCH);
		}

		private static void  setValueCompareOp(List<ValueCompareOp> list, string name, EValueCompOp op)
		{
			list[(int) op] = new ValueCompareOp(name, op);
		}
		
		private static void setLogicalCompareOp(List<LogicalCompareOp> list, string name, ELogicalCompOp op)
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
