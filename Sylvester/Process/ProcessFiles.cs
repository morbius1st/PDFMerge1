#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Sylvester.FileSupport;
using UtilityLibrary;

#endregion


// projname: Sylvester.Process
// itemname: ProcessFiles
// username: jeffs
// created:  1/2/2020 10:09:15 PM


namespace Sylvester.Process
{
	public class ProcessFiles
	{
		private class SheetMatch
		{
			public SheetIdTest ShtIdTest { get; set; }
			public SheetIdBase ShtIdBase { get; set; }

			public SheetMatch(SheetIdTest test)
			{
				ShtIdTest = test;
				ShtIdBase = new SheetIdBase();
				ShtIdBase.SheetID = "";
			}

			public override string ToString()
			{
				

				return "*** test| " + ShtIdTest.ToString() +
					"  *** base| " + ShtIdBase.ToString();
			}
		}

		public SelectFiles<SheetIdBase> BaseFiles;
		public SelectFiles<SheetIdTest> TestFiles;

		private SortedDictionary<string, SheetMatch>
			sheets = new SortedDictionary<string, SheetMatch>();


		public ProcessFiles(
			SelectFiles<SheetIdBase> baseFiles,
			SelectFiles<SheetIdTest> testFiles
			)
		{
			BaseFiles = baseFiles;
			TestFiles = testFiles;
		}

		public bool Process()
		{
			// step 1 - add all test records to dictionary
			AddRecords();

			// step 2 - run through each record in base to see if there
			if (!MatchSheetsNumbers()) return false;



			return true;
		}

		private void AddRecords()
		{
			// key is the adjusted sheet id
			foreach (SheetIdTest shtIdTest in TestFiles.SheetFiles.Files)
			{
				SheetMatch sm = new SheetMatch(shtIdTest);
				sheets.Add(shtIdTest.AdjustedSheetID, sm);
			}
		}

		private bool MatchSheetsNumbers()
		{

			// is a match in the dictionary
			foreach (SheetIdBase shtIdBase in BaseFiles.SheetFiles.Files)
			{

				if (sheets.ContainsKey(shtIdBase.AdjustedSheetID))
				{
					string idx = shtIdBase.AdjustedSheetID;

					SheetIdTest test = sheets[idx].ShtIdTest;

					if (string.IsNullOrEmpty(
						sheets[idx].ShtIdBase.SheetID))
					{
						sheets[idx].ShtIdBase = shtIdBase;

						test.MatchedSheetNumber =
							sheets[idx].ShtIdBase.SheetNumber;

						test.MatchedSeparator =
							sheets[idx].ShtIdBase.Separator;

						test.MatchedSheetName =
							sheets[idx].ShtIdBase.SheetName;

						test.Comment =
							sheets[idx].ShtIdBase.Comment;
					}
					else
					{
						return false;
					}
				}
			}

			return true;
		}

		public override string ToString()
		{
			return "test| " + TestFiles.SheetFiles.Files.ToString() +
				"base| " + BaseFiles.SheetFiles.Files.ToString();
		}
	}
}
