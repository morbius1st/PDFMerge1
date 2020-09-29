#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#endregion

// username: jeffs
// created:  9/29/2020 8:09:06 AM

namespace AndyShared.ClassificationDataSupport.TreeSupport
{
	/// <summary>
	/// Compare a sheet file's component against the compareop criteria
	/// </summary>
	public class CompOp
	{
	#region private fields

	#endregion

	#region ctor

		public CompOp() { }

	#endregion

	#region public properties

		public static bool Compare(string value, ObservableCollection<ComparisonOperation> CompareOps)
		{
			bool result = false;

			foreach (ComparisonOperation compareOp in CompareOps)
			{
				
				if (compareOp.IsFirstCompOp)
				{
					result = compare(value, compareOp);
				}
				else if (compareOp.LogicalCompareOp.OpCode.Equals(ComparisonOp.LOGICAL_AND))
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

		private static bool compare(string value, ComparisonOperation compareOp)
		{
			bool result = false;

			switch (compareOp.ValueCompareOp.OpCode)
			{
			case ComparisonOp.LESS_THAN_OR_EQUAL:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) <= 0;
					break;
				}
			case ComparisonOp.LESS_THAN:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) < 0;
					break;
				}
			case ComparisonOp.GREATER_THAN_OR_EQUAL:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) >= 0;
					break;
				}
			case ComparisonOp.GREATER_THAN:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) > 0;
					break;
				}
			case ComparisonOp.EQUALTO:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) == 0;
					break;
				}
			case ComparisonOp.DOES_NOT_EQUAL:
				{
					result = string.Compare(value, compareOp.CompareValue, StringComparison.CurrentCulture) != 0;
					break;
				}
			case ComparisonOp.CONTAINS:
				{
					result = value.Contains(compareOp.CompareValue);
					break;
				}
			case ComparisonOp.DOES_NOT_CONTAIN:
				{
					result = !value.Contains(compareOp.CompareValue);
					break;
				}
			case ComparisonOp.STARTS_WITH:
				{
					result = value.StartsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ComparisonOp.DOES_NOT_START_WITH:
				{
					result = !value.StartsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ComparisonOp.ENDS_WITH:
				{
					result = value.EndsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ComparisonOp.DOES_NOT_END_WITH:
				{
					result = !value.EndsWith(compareOp.CompareValue, StringComparison.CurrentCulture);
					break;
				}
			case ComparisonOp.MATCHES:
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
			case ComparisonOp.DOES_NOT_MATCH:
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

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is CompareOpSupport";
		}

	#endregion
	}
}