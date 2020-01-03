#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



#endregion


// projname: Sylvester.FileSupport
// itemname: SheetIdTest
// username: jeffs
// created:  1/1/2020 8:03:25 AM


namespace Sylvester.FileSupport
{
	public class SheetIdBase : SheetIdTest
	{
		private bool sheetNumberMatches = true;
		private bool separationMatches = true;
		private bool sheetNameMatches = true;

		public bool HasDiferences { get; set; }
		public bool MakeChanges { get; set; } = false;
		public string MatchedSheetNumber { get; set; }
		public string MatchedSheetSeparation { get; set; }
		public string MatchedSheetName { get; set; }
		public string ProposedSheetNumber { get; set; }
		public string ProposedSheetSeparation { get; set; }
		public string ProposedSheetName { get; set; }

		public bool SheetNumberMatches
		{
			get => sheetNumberMatches;
			set => sheetNumberMatches = value;
		}

		public bool SeparationMatches
		{
			get => separationMatches;
			set => separationMatches = value;
		}

		public bool SheetNameMatches
		{
			get => sheetNameMatches;
			set => sheetNameMatches = value;
		}

		public new string SheetNumber
		{
			get => sheetNumber;
			set
			{
				sheetNumber = value;
				OnPropertyChange();

				AdjustedSheetNumber = AdjustSheetNumber(sheetNumber);
			}
		}

		public new string AdjustSheetNumber(string sheetId)
		{
			return regex.Replace(sheetId, SUBST_PATTERN) + "-base";
		}
	}
}
