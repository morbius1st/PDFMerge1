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

				ff = (TestFile) bfTest.Clone<TestFile>();

				if ((bfBase = BaseFileColl.ContainsKey(bfTest.AdjustedSheetID)) != null)
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