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
using static PDFMerge1.PdfMergeTree;
using static PDFMerge1.PdfMergeTree.bookmarkType;

namespace PDFMerge1
{
	internal class PdfMergeFileList
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

		internal PdfDocument Merge(string outputFile, List<MergeItem> tree)
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

//			listOutline(pdf, pdf.GetOutlines(false));

			// process and adjust bookmarks
			// eliminate all of the current bookmarks
			PdfOutline destPdfOutlines;
			destPdfOutlines = clearOutline(pdf);

//			listOutline(pdf, destPdfOutlines);


			// all complete
			pdf.Close();

			// return the final PDF document
			return pdf;
		}

		PdfOutline clearOutline(PdfDocument pdf)
		{
			// clear the current outline tree
			pdf.GetOutlines(false).GetContent().Clear();

			// initalize a new outline tree
			pdf.InitializeOutlines();

			return pdf.GetOutlines(true);
		}

		private int merge(PdfMerger merger, List<MergeItem> bookmarks, int initialPageCount)
		{
			int pageCount = initialPageCount;
			PdfDocument src = null;
//			bool doPageCount;

			foreach (MergeItem bm in bookmarks)
			{
//				doPageCount = true;

				if (bm.fileItem.ItemType == FILE)
				{
					bm.pageNumber = pageCount;

					try
					{
						src = new PdfDocument(new PdfReader(bm.fileItem.getFullPath));

						logMsgln("merging| ", bm.fileItem.getFullPath);

						merger.Merge(src, 1, src.GetNumberOfPages());

						if (src != null && !src.IsClosed())
						{
							pageCount += src.GetNumberOfPages();
							src.Close();
						}
					}
					catch (Exception ex)
					{
						logMsgln("NOT merging| ", ex.Message 
							+ "  (" + bm.fileItem.getFullPath + ")");
						bm.fileItem.ItemType = MISSING;
						bm.pageNumber = -1;
//						doPageCount = false;
					}

//					if (src != null && !src.IsClosed())
//					{ 
//						if (doPageCount)
//						{
//							pageCount += src.GetNumberOfPages();
//						}
//
//						src.Close();
//					}
				}

				if (bm.mergeItems != null)
				{
					pageCount = merge(merger, bm.mergeItems, pageCount);
				}
			}

			return pageCount;
		}

		private static int depth = 0;

		public void listOutline(PdfDocument pdfDoc, PdfOutline outline)
		{
			logMsg(formatBookmark(outline.GetTitle(), depth, outline.GetPageNumber(pdfDoc)));

			logMsg(nl);

			IList<PdfOutline> kids = outline.GetAllChildren();

			if (kids.Count != 0)
			{
				depth++;

				for (int i = 0; i < kids.Count; i++)
				{
					listOutline(pdfDoc, kids[i]);
				}

				depth--;

			}

		}

		string formatBookmark(string title, int depth, int page)
		{
			return String.Format("bookmark|  depth| {1,3} | page| {0,3} | {2}{3}",
				page, depth, " ".Repeat(depth * 2), title);
		}
	}
}
