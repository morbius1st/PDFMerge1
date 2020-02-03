#region + Using Directives

using Sylvester.FileSupport;

#endregion


// projname: Sylvester.Process
// itemname: ProcessFiles
// username: jeffs
// created:  1/2/2020 10:09:15 PM


namespace Sylvester.Process
{
	public class ProcessFiles
	{
		public readonly FilesCollection<FileCurrent> FileCollectionCurrent = null;
		public readonly FilesCollection<FileRevision> FileCollectionRevision = null;

		public FilesCollection<FileFinal> FinalFileColl = null;


		public ProcessFiles(FilesCollection<FileCurrent> fileCollectionCurrent,
			FilesCollection<FileRevision> fileCollectionRevision)
		{
			FileCollectionRevision  = fileCollectionRevision;
			FileCollectionCurrent  = fileCollectionCurrent;
		}

		public FilesCollection<FileFinal> Process()
		{
			if (FileCollectionRevision == null || FileCollectionCurrent == null) return null;

			FinalFileColl = new FilesCollection<FileFinal>();

			// step one - Match adjusted sheet numbers
			if (!MatchSheetsNumbers()) return null;

			return FinalFileColl;
		}

		private bool MatchSheetsNumbers()
		{
			FileCurrent fc;
			FileFinal ff;

			foreach (FileRevision fr in FileCollectionRevision.Files)
			{
				if (!fr.Selected) continue;
				if (fr.FileType != FileType.SHEET_PDF) continue;

				ff = (FileFinal) fr.Clone<FileFinal>();

				if ((fc = FileCollectionCurrent.ContainsKey(fr.AdjustedSheetId)) != null)
				{
					ff.FileCurrent = fc;
				}

				ff.UpdateSelectStatus();

				FinalFileColl.Add(ff);
			}

			return true;
		}


		public override string ToString()
		{
			return "Revision| " + FileCollectionRevision.ToString() +
				   " Current| " + FileCollectionCurrent.ToString();
		}
	}
}