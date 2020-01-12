#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Microsoft.WindowsAPICodePack.Taskbar;
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
//		private class SheetMatch
//		{
//			public SheetIdTest ShtIdTest { get; set; }
//			public BaseFile ShtIdBase { get; set; }
//
//			public SheetMatch(SheetIdTest test)
//			{
//				ShtIdTest = test;
//				ShtIdBase = new BaseFile();
//				ShtIdBase.SheetID = "";
//			}
//
//			public override string ToString()
//			{
//				return "*** test| " + ShtIdTest.ToString() +
//					"  *** base| " + ShtIdBase.ToString();
//			}
//		}

//		private SortedDictionary<string, SheetMatch>
//			sheets = new SortedDictionary<string, SheetMatch>();

		public readonly FilesCollection<BaseFile> BaseFileColl = null;
		public readonly FilesCollection<BaseFile> TestFileColl = null;

		public FilesCollection<TestFile> FinalFileColl = null;


		public ProcessFiles(FilesCollection<BaseFile> baseFileColl,
			FilesCollection<BaseFile> testFileColl)
		{
			TestFileColl  = testFileColl;
			BaseFileColl  = baseFileColl;
		}

		public FilesCollection<TestFile> Process()
		{
			if (TestFileColl == null || BaseFileColl == null) return null;

			FinalFileColl = new FilesCollection<TestFile>();

			// step one - Match adjusted sheet numbers
			if (!MatchSheetsNumbers()) return null;

			return FinalFileColl;
		}

		private bool MatchSheetsNumbers()
		{
			BaseFile bfBase;
			TestFile ff;

			foreach (BaseFile bfTest in TestFileColl.TestFiles)
			{
				if (!bfTest.IsSelected) continue;

				if ((bfBase = BaseFileColl.ContainsKey(bfTest.AdjustedSheetID)) != null)
				{
					ff = (TestFile) bfTest.Clone<TestFile>();
					ff.BaseFile = bfBase;

					FinalFileColl.Add(ff);
				}
			}

			return true;
		}


		public override string ToString()
		{
			return "test| " + TestFileColl.ToString() +
				"base| " + BaseFileColl.ToString();
		}


//		public SelectFiles<BaseFile> BaseFiles;
//		public SelectFiles<SheetIdTest> TestFiles;
//
//		private FilesCollection<TestFile> tfc;

//		public ProcessFiles(
//			SelectFiles<BaseFile> baseFiles,
//			SelectFiles<SheetIdTest> testFiles,
//			FilesCollection<TestFile> fileCollection
//			)
//		{
//			BaseFiles = baseFiles;
//			TestFiles = testFiles;
//			tfc = fileCollection;
//		}
//
//		public bool Process2()
//		{
//			return MatchSheetsNumbers2();
//		}
//
//		private bool MatchSheetsNumbers2()
//		{
//			TestFile tf;
//
//			foreach (BaseFile shtIdBase in BaseFiles.SheetFiles.Files)
//			{
//				if ((tf = tfc.ContainsKey(shtIdBase.AdjustedSheetID)) != null)
//				{
//					tf.BaseFile = shtIdBase;
//				}
//			}
//
//			return true;
//		}
//
//		public bool Process()
//		{
//			// step 1 - add all test records to dictionary
//			AddRecords();
//
//			// step 2 - run through each record in base to see if there
//			if (!MatchSheetsNumbers()) return false;
//
//			return true;
//		}
//
//		private void AddRecords()
//		{
//			// key is the adjusted sheet id
//			foreach (SheetIdTest shtIdTest in TestFiles.SheetFiles.Files)
//			{
//				SheetMatch sm = new SheetMatch(shtIdTest);
//				sheets.Add(shtIdTest.AdjustedSheetID, sm);
//			}
//		}
//
//		private bool MatchSheetsNumbers()
//		{
//
//			// is a match in the dictionary
//			foreach (BaseFile shtIdBase in BaseFiles.SheetFiles.Files)
//			{
//
//				if (sheets.ContainsKey(shtIdBase.AdjustedSheetID))
//				{
//					string idx = shtIdBase.AdjustedSheetID;
//
//					SheetIdTest test = sheets[idx].ShtIdTest;
//
//					if (string.IsNullOrEmpty(
//						sheets[idx].ShtIdBase.SheetID))
//					{
//						sheets[idx].ShtIdBase = shtIdBase;
//
//						test.MatchedSheetNumber =
//							sheets[idx].ShtIdBase.SheetNumber;
//
//						test.MatchedSeparator =
//							sheets[idx].ShtIdBase.Separator;
//
//						test.MatchedSheetName =
//							sheets[idx].ShtIdBase.SheetName;
//
//						test.Comment =
//							sheets[idx].ShtIdBase.Comment;
//					}
//					else
//					{
//						return false;
//					}
//				}
//			}
//
//			return true;
//		}
//
//		public override string ToString()
//		{
//			return "test| " + TestFiles.SheetFiles.Files.ToString() +
//				"base| " + BaseFiles.SheetFiles.Files.ToString();
//		}
	}
}