#region using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
#endregion

// username: jeffs
// created:  5/31/2020 5:03:54 PM

namespace ClassifierEditor.Tree
{
	public enum ComparisonOp
	{
		DOES_NOT_END_WITH = -103,
		ENDS_WITH = -3,
		DOES_NOT_START_WITH = -102,
		STARTS_WITH = -2,
		DOES_NOT_CONTAIN = -101,
		CONTAINS = -1,
		NO_OP = 0,
		LOGICAL_AND = 1,
		LOGICAL_OR = 2,
		EQUALS = 101,
		DOES_NOT_EQUAL = 201,
		GREATER_THAN = 102,
		GREATER_THAN_OR_EQUAL = 103,
		LESS_THAN = 104,
		LESS_THAN_OR_EQUAL =105

	}



	public class ComparisonOperation : INotifyPropertyChanged
	{
		#region private fields

			private ComparisonOp compareOp = ComparisonOp.NO_OP;

			private string compareValue = null;


		#endregion

		#region ctor

			public ComparisonOperation(ComparisonOp op, string value)
			{
				CompareOp = op;
				CompareValue = value;

			}

		#endregion

		#region public properties

			public ComparisonOp CompareOp
			{
				get => compareOp;
				set
				{
					compareOp = value;
					OnPropertyChange();
					OnPropertyChange("ComparisonOpString");
				}
			}

			public string ComparisonOpString
			{
				get => comparisonOpString(compareOp);
			}

			public string CompareValue
			{
				get => compareValue;
				set
				{ 
					compareValue = value;

					OnPropertyChange();
				}
			}

		#endregion

		#region private properties

			private string comparisonOpString(ComparisonOp op)
			{
				string name = op.ToString();

				name = name.ToLower();

				name.Replace("_", " ");

				name = name.Substring(0, 1).ToUpper() + name.Substring(1);

				return name;
			}


		#endregion

		#region public methods



		#endregion

		#region private methods



		#endregion

		#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
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
