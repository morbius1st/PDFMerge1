#region + Using Directives

using System;
using System.IO;
using UtilityLibrary;
using AndyShared;
using static Test3.BranchSortOrder;

#endregion


// projname: Test3
// itemname: SequenceCode
// username: jeffs
// created:  12/26/2019 10:54:24 PM


namespace Test3
{
	public class SequenceCode
	{
		public const string SORTCODEFILLER  = "-";
		public const string GENERALSORTCODEPREFIX  = "!";
		public const string BRANCHSORTFIRSTPREFIX  = "-";
		public const string BRANCHSORTLASTPREFIX   = "ᛎ";
		public const string BRANCHSORTMIXEDPREFIX = GENERALSORTCODEPREFIX;

#pragma warning disable CS0414 // The field 'SequenceCode.branchSortOrder' is assigned but its value is never used
		private static BranchSortOrder branchSortOrder = MISSING;
#pragma warning restore CS0414 // The field 'SequenceCode.branchSortOrder' is assigned but its value is never used

		private static SequenceCode root;

		private SeqCodeUnit me;

		public SequenceCode ( string sortCode,
			SequenceCode parent = null,
			bool isBranch = false)
		{
			if (parent != null)
			{
				if (parent.IsBranch)
					Parent = parent ?? root;
				else
					throw new ArgumentException();
			}

			me = (new SeqCodeUnit(sortCode, isBranch));
		}

		private SequenceCode () { }

		static SequenceCode() { }

		public static void Initialize(int sortCodeLength,
			string generalSortCodePrefix, string branchSortCodePrefix)
		{
			SeqCodeUnit.Initialize(sortCodeLength, generalSortCodePrefix,
				branchSortCodePrefix);

			root = new SequenceCode(BRANCHSORTFIRSTPREFIX.Repeat(branchSortCodePrefix.Length), null, true);

			root.Description = "Root";
		}

		public SequenceCode Root => root;
		public SequenceCode Parent { get; private set; }
		public string Description { get; private set; }
		public string GeneralSeqCodePrefix => SeqCodeUnit.GeneralSeqCodePrefix;
		public string BranchSeqCodePrefix => SeqCodeUnit.BranchSeqCodePrefix;
		public int SeqCodeLength => SeqCodeUnit.SeqCodeLength;
		public bool IsBranch => me.IsBranch;
		public string SeqCodeString
		{
			get
			{
				return
					Parent == null ? me.Element : 
						Parent.SeqCodeString + "│" + me.Element;
			}
		}

		public void Rename(string sortCode)
		{
			me = new SeqCodeUnit(sortCode, me.IsBranch);
		}

		public override string ToString()
		{
			return SeqCodeString;
		}

		private struct SeqCodeUnit
		{
			public static string GeneralSeqCodePrefix { get; private set; }
			public static string BranchSeqCodePrefix { get; private set; }

			public static int SeqCodePrefixLength { get; private set; } = 2;
			public static int SeqCodeLength { get; private set; } = 2;

			private string sortElement;

			public SeqCodeUnit(string sort, bool isBranch = false)
			{
				sortElement = "";

				this.IsBranch = isBranch;
				sortElement = formatSortCode(sort);
			}

			public static void Initialize(int sortCodeLength,
				string generalSortCodePrefix = GENERALSORTCODEPREFIX,
				string branchSortCodePrefix = BRANCHSORTFIRSTPREFIX)
			{
				SeqCodeLength = sortCodeLength;
				GeneralSeqCodePrefix = generalSortCodePrefix;
				BranchSeqCodePrefix = branchSortCodePrefix;

				SeqCodePrefixLength = GeneralSeqCodePrefix.Length;

				if (SeqCodePrefixLength != BranchSeqCodePrefix.Length)
					throw new InvalidDataException();
			}

			public string Element => sortElement;
			public bool IsBranch { get; private set; }

			private string formatSortCode(string sortCode)
			{
				int len = sortCode?.Length ?? throw new ArgumentException();

				if (len > SeqCodeLength) throw new ArgumentException();

				string result = len < SeqCodeLength
					? SORTCODEFILLER.Repeat(SeqCodeLength - len) + sortCode
					: sortCode;

				if (IsBranch)
				{
					result = BranchSeqCodePrefix + result;
				}
				else
				{
					result = GeneralSeqCodePrefix + result;
				}

				return result;
			}
		}
	}
}