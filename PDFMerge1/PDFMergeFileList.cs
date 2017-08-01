using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

using static PDFMerge1.Utility;
using static PDFMerge1.FileList.FileItemType;
using static PDFMerge1.BookmarkTree;
using static PDFMerge1.BookmarkTree.bookmarkType;

namespace PDFMerge1
{
	internal class PDFMergeFileList
	{

		internal bool OVERWITE_OUTPUT { get; set; } = true;
		internal bool MERGE_TAGS { get; set; } = false;
		internal bool MERGE_BOOKMARKS { get; set; } = false;

		private bool verifyOutputFile(string outputFile)
		{
			if (outputFile == null || outputFile.Length < 3) { return false; }

			if (File.Exists(outputFile))
			{
				if (OVERWITE_OUTPUT)
				{
					try
					{
						var f = File.OpenWrite(outputFile);
						f.Close();
						logMsgFmtln("output file| ", "exists: overwrite allowed\n");
						return true;
					}
					catch (Exception ex)
					{
						logMsgFmtln("result| ", "fail - file is not accessable or is open elsewhere\n");
						return false;
					}
				}
				else
				{
					logMsgFmtln("output file| ", "exists: overwrite disallowed\n");
					return false;
				}
			}

			logMsgFmtln("output file| ", "does not exists\n");
			return true;
		}

		internal PdfDocument Merge(string outputFile, List<Bookmark> tree)
		{
			if (!verifyOutputFile(outputFile))
			{
				throw new IOException("Invalid Output File");
			}

			PdfDocument pdf = new PdfDocument(new PdfWriter(outputFile));

			// always merge tags and bookmarks and fix afterwards
			PdfMerger merger = new PdfMerger(pdf, true, true);
			merger.SetCloseSourceDocuments(false);

			if (merge(merger, tree, 0) < 1)
			{
				pdf.Close();
				return null;
			}

			// process and adjust bookmarks
			pdf.Close();

			return pdf;
		}

		private int merge(PdfMerger merger, List<Bookmark> bookmarks, int initialPageCount)
		{
			int pageCount = initialPageCount;
			PdfDocument src = null;
			bool doPageCount;

			foreach (Bookmark bm in bookmarks)
			{
				doPageCount = true;

				if (bm.fileItem.ItemType == FILE)
				{
					bm.pageNumber = pageCount;

					try
					{
						src = new PdfDocument(new PdfReader(bm.fileItem.getFullPath));

						logMsgln("merging| ", bm.fileItem.getFullPath);

						merger.Merge(src, 1, src.GetNumberOfPages());
					}
					catch (Exception ex)
					{
						logMsgln("NOT merging| ", ex.Message);
						bm.fileItem.ItemType = MISSING;
						bm.pageNumber = -1;
						doPageCount = false;
					}

					if (src != null && !src.IsClosed())
					{ 
						if (doPageCount)
						{
							pageCount += src.GetNumberOfPages();
						}

						src.Close();
					}
				}

				if (bm.bookmarks != null)
				{
					pageCount = merge(merger, bm.bookmarks, pageCount);
				}
			}

			return pageCount;
		}


	}
}
