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
		public readonly FilesCollection<BaseFile> BaseFileColl = null;
		public readonly FilesCollection<TestFile> TestFileColl = null;

		public FilesCollection<FinalFile> FinalFileColl = null;


		public ProcessFiles(FilesCollection<BaseFile> baseFileColl,
			FilesCollection<TestFile> testFileColl)
		{
			TestFileColl  = testFileColl;
			BaseFileColl  = baseFileColl;
		}

		public FilesCollection<FinalFile> Process()
		{
			if (TestFileColl == null || BaseFileColl == null) return null;

			FinalFileColl = new FilesCollection<FinalFile>();

			// step one - Match adjusted sheet numbers
			if (!MatchSheetsNumbers()) return null;

			return FinalFileColl;
		}

		private bool MatchSheetsNumbers()
		{
			BaseFile bfBase;
			FinalFile ff;

			foreach (TestFile bfTest in TestFileColl.TestFiles)
			{
				if (!bfTest.Selected) continue;
				if (bfTest.FileType != FileType.SHEET_PDF) continue;

				ff = (FinalFile) bfTest.Clone<FinalFile>();

				if ((bfBase = BaseFileColl.ContainsKey(bfTest.AdjustedSheetId)) != null)
				{
					ff.BaseFile = bfBase;
				}

				ff.UpdateSelectStatus();

				FinalFileColl.Add(ff);
			}

			return true;
		}


		public override string ToString()
		{
			return "test| " + TestFileColl.ToString() +
				"base| " + BaseFileColl.ToString();
		}
	}
}