#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using ClassifierEditor.Windows;

using static ClassifierEditor.Tree.EValueCompOp;
using static ClassifierEditor.Tree.ELogicalCompOp;
using static ClassifierEditor.Tree.CompareOperations;

#endregion

// username: jeffs
// created:  5/31/2020 5:03:54 PM

namespace ClassifierEditor.Tree
{

	[DataContract(Namespace = "")]
//	[KnownType(typeof(ValueCompOp))]
//	[KnownType(typeof(LogicalCompOp))]
	public class CompareOperation : INotifyPropertyChanged
	{
	#region private fields

		protected LogicalCompareOp logicalCompOp = LogicalCompareOps[(int) LOGICAL_AND];
		protected ValueCompareOp valueCompOp = ValueCompareOps[(int) NO_OP];

		private string compareValue;
		private bool ignoreCompOp = false;

	#endregion

	#region ctor

		public CompareOperation(
			LogicalCompareOp lop,
			ValueCompareOp vop,
			string compareValue)
		{
			LogicalCompOp = lop;
			ValueCompOp = vop;
			CompareValue = compareValue;


		}

	#endregion

	#region public properties

		// logical - if null, first in list
		// this defines the logical comparison for this value
		// versus culmination of the prior comparisons
		[IgnoreDataMember]
		public string LogicalCompOpName => LogicalCompOp.Name;
		
		[IgnoreDataMember]
		public int LogicalCompOpValue => LogicalCompOp.OpCodeValue;


		[DataMember(Order = 2)]
		public LogicalCompareOp LogicalCompOp
		{
			get => logicalCompOp;

			set
			{
				logicalCompOp = value;
				OnPropertyChange();
				OnPropertyChange("LogicalCompOpName");
				OnPropertyChange("LogicalCompOpValue");
			}
		}



		// value
		[IgnoreDataMember]
		public string ValueCompOpName => ValueCompOp.Name;

		[IgnoreDataMember]
		public int ValueCompOpValue => ValueCompOp.OpCodeValue;

		[DataMember(Order = 1)]
		public ValueCompareOp ValueCompOp
		{
			get => valueCompOp;

			set
			{
				valueCompOp = value;
				OnPropertyChange();
				OnPropertyChange("ValueCompOpName");
				OnPropertyChange("ValueCompOpValue");
			}
		}


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

		[IgnoreDataMember]
		public bool IsFirstCompOp
		{
			get => LogicalCompOp == null;

		}

		[DataMember(Order = 4)]
		public bool IgnoreCompOp
		{
			get => ignoreCompOp;
			set
			{
				ignoreCompOp = value;

				OnPropertyChange();
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

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this,  new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is CompareOperation";
		}

	#endregion
	}

//	[DataContract(Namespace = "")]
//	public class ValueCompOp : CompareOperation
//	{
//		public ValueCompOp(ValueCompareOp op, string value, bool isFirst = false)
//		{
//			CompareOp = op;
//			CompareValue = value;
//			IsFirstCompOp = isFirst;
//		}
//
//		[IgnoreDataMember]
//		public override int CompareOpCode
//		{
//
//			get => coOp.OpCodeValue;
//			set
//			{
//				CompareOp = ValueCompareOps[value];
//
//			}
//		}
//	}
//
//	[DataContract(Namespace = "")]
//	public class LogicalCompOp : CompareOperation
//	{
//		public LogicalCompOp(LogicalCompareOp op)
//		{
//			CompareOp = op;
//			CompareValue = null;
//		}
//
//		[IgnoreDataMember]
//		public override int CompareOpCode
//		{
//
//			get => coOp.OpCodeValue;
//			set
//			{
//				CompareOp = LogicalCompareOps[value];
//
//			}
//		}
//	}





}