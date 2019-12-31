#region + Using Directives

using System;
using System.Collections.Generic;

using UtilityLibrary;

using static Test3.BranchSortOrder;


#endregion


// projname: Test3
// itemname: SortCode
// username: jeffs
// created:  12/26/2019 6:42:07 AM


// void - do not use
// use sequence code

namespace Test3
{
	public enum BranchSortOrder
	{
		MISSING = -1,
		SORTMIXED,
		SORTFIRST,
		SORTLAST
	}

	public class SortCode : IComparable<SortCode>, ICloneable
	{
		// start of class
		private const string ROOT = "--";

		private static BranchSortOrder sortBranchOrder = MISSING;

		private static SortCode root;
 
		private List<SortCodeUnit> sortCodes;

		public SortCode (string sort, bool isBranch = false)
		{
			sortCodes = new List<SortCodeUnit>();

			sortCodes.Add(new SortCodeUnit(sort, isBranch));
		}

		private SortCode ()
		{
			sortCodes = new List<SortCodeUnit>();
		}

		static SortCode()
		{
			if (root != null) return ;

			sortBranchOrder = SORTFIRST;

			root= new SortCode();

			root.sortCodes = new List<SortCodeUnit>();
			root.sortCodes.Add(new SortCodeUnit(ROOT, true));

			sortBranchOrder = MISSING;
		}

		public SortCode Root => root;

		public static BranchSortOrder BranchSortOrder
		{
			set => sortBranchOrder = value;
		}

		public SortCode Append(string sort)
		{
			SortCode sc = Clone() as SortCode;

			sc.sortCodes.Add(new SortCodeUnit(sort));

			return sc;
		}


		// before = -1, equal = 0, after = 1
		// if not the same length - the longer is after the shorter
		public int CompareTo(SortCode other)
		{
			int result = 0;

			int length = sortCodes.Count;

			if (length != other.sortCodes.Count)
			{
				if (length < other.sortCodes.Count) return  1; // other is after
				return -1; // other is before
			}

			for (int i = 0; i < length; i++)
			{
				result = sortCodes[i].CompareTo(other.sortCodes[i]);

				if (result != 0) break;
			}

			return result;
		}

		public object Clone()
		{
			SortCode sortCode = new SortCode();

			foreach (SortCodeUnit code in sortCodes)
			{
				sortCode.sortCodes.Add(code);
			}

			return sortCode;
		}

		public override string ToString()
		{
			int len = sortCodes?.Count ?? 0;

			if (len == 0) return "";
			if (len == 1) return sortCodes[0].Element;

			string result = "";

			int i = 0;

			for (; i < sortCodes.Count - 1 ; i++)
			{
				result += sortCodes[i].Element + "|";
			}

			result += sortCodes[i].Element;

			return result;
		}

		private struct SortCodeUnit : IComparable<SortCodeUnit>
		{
			private const int CODELENGTH = 2;
			private const char CODEPREFIX = '-';
			private const char BRANCHSORTFIRSTPREFIX = '-';
			private const char BRANCHSORTLASTPREFIX = 'ᛎ';
			private const char BRANCHFSORTMIXEDPREFIX = 'ᛎ';

			private string sortElement;

			public SortCodeUnit(string sort, bool isBranch = false)
			{
				sortElement = "---";

				if (sortBranchOrder == MISSING)
					throw new MissingMemberException();

				sortElement = formatSortCode(sort, isBranch);
			}

			public string Element => sortElement;

			private string formatSortCode(string sort, bool isBranch)
			{
				int len = sort?.Length ?? CODELENGTH + 1;

				if (len > CODELENGTH) throw new ArgumentException();

				string result = len < CODELENGTH
					? "0".Repeat(CODELENGTH - len) + sort
					: sort;

				char sortChar = ' ';

				if (isBranch)
				{
					result = branchSortCode() + sort;
				}
				else
				{
					result = CODEPREFIX + sort;
				}

				return result;
			}

			private string branchSortCode()
			{
				char sortChar = ' ';

				switch (sortBranchOrder)
				{
				case SORTFIRST:
					sortChar = BRANCHSORTFIRSTPREFIX;
					break;
				case SORTLAST:
					sortChar = BRANCHSORTLASTPREFIX;
					break;
				case SORTMIXED:
					sortChar = BRANCHFSORTMIXEDPREFIX;
					break;
				}

				return sortChar.ToString();
			}

			public int CompareTo(SortCodeUnit other)
			{
				return this.sortElement.CompareTo((string) other.sortElement);
			}
		}
	}
}