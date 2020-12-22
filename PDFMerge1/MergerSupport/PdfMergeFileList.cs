//using static PDFMerge1.UtilityLocal;
using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.Utils;
using PDFMerge1.FilesSupport;
using static PDFMerge1.FilesSupport.FileItemType;
using static PDFMerge1.MergerSupport.bookmarkType;
using static UtilityLibrary.MessageUtilities;


namespace PDFMerge1.MergerSupport
{
	internal class PdfMergeFileList
	{
		private const string WHO_AM_I = "@MrgFileLst";
		// flags
		internal bool MERGE_TAGS { get; set; } = false;
		internal bool MERGE_BOOKMARKS { get; set; } = false;

		// auto overwite existing output file // done
		internal static bool OVERWITE_EXISTING_FILE { get; set; } = true;
		// set headings to their collaspsed state // done & tested
		internal static bool COLLAPSE_FILE_BOOKMARKS { get; set; } = true;
		// add a page number to every bookmark // done & tested
		internal static bool ADD_PAGE_TO_ALL_BOOKMARKS { get; set; } = true;
		// add a heading for each imported PDF file
		internal static bool ADD_BOOKMARK_FOR_EACH_FILE { get; set; } = true;
		// add the existing bookmarks from the imported files
		// if ADD_BOOKMARK_FOR_EACH_FILE is true, the imported
		// bookmarks are added as children to the file bookmark
		internal static bool KEEP_IMPORT_BOOKMARKS { get; set; } = false;
		// what to do if a corrupted PDF file is discovered
		internal static bool CONTINUE_ON_CORRUPT_PDF { get; set; } = true;


		// marker for each new file
		private const string FILE_MARKER = "\u2401";

		private PdfDocument destPdf;

		// output pdf file
		//		private PdfDocument destPdf;

		// output outline tree
		private PdfOutline destPdfOutlines;

		private void listOptions()
		{
			logMsg("\n");
			logMsgFmtln("current options");
			logMsgFmtln("overwrite existing file", OVERWITE_EXISTING_FILE);
			logMsgFmtln("collapse file bookmarks", COLLAPSE_FILE_BOOKMARKS);
			logMsgFmtln("add page number to all bookmarks", ADD_PAGE_TO_ALL_BOOKMARKS);
			logMsgFmtln("add bookmark for each file", ADD_BOOKMARK_FOR_EACH_FILE);
			logMsgFmtln("keep imported bookmarks", KEEP_IMPORT_BOOKMARKS);
			logMsgFmtln("continue on corrupted PDF", CONTINUE_ON_CORRUPT_PDF);
			logMsg("\n");
		}

		internal static bool VerifyOutputFile(string outputFile)
		{
			if (outputFile == null || outputFile.Length < 3) { return false; }

			if (File.Exists(outputFile))
			{
				if (OVERWITE_EXISTING_FILE)
				{
					try
					{
						var f = File.OpenWrite(outputFile);
						f.Close();
						File.Delete(outputFile);

						logMsgFmtln("output file", "exists: overwrite allowed");
						return true;
					}
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
					catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
					{
						logMsgFmtln("result", "fail - file is not accessible or is open elsewhere");
						return false;
					}
				}
				else
				{
					logMsgFmtln("output file", "exists: overwrite disallowed");
					return false;
				}
			}

			logMsgFmtln("output file", "does not exists");
			return true;
		}

		internal PdfDocument Merge(string outputFile, PdfMergeTree mergeTree)
		{
			Form1.configProgressBar(mergeTree.count);

			listOptions();

			if (!VerifyOutputFile(outputFile))
			{
				throw new IOException("Invalid Output File");
			}

			PdfWriter writer = new PdfWriter(outputFile);

			destPdf = new PdfDocument(writer);

			destPdfOutlines = destPdf.GetOutlines(false);

			PdfMerger merger = new PdfMerger(destPdf, MERGE_TAGS, MERGE_BOOKMARKS);
			merger.SetCloseSourceDocuments(false);

			logMsgDbLn2("");

			int pageCount = merge(merger, mergeTree.GetMergeItems, 1) - 1;

			logMsgFmtln("total page count", pageCount);

			if ( pageCount < 1)
			{
				destPdf.Close();
				return null;
			}

			if (KEEP_IMPORT_BOOKMARKS && ADD_BOOKMARK_FOR_EACH_FILE)
			{
				// process and adjust bookmarks
				MergeOutlines(destPdf.GetOutlines(true).GetAllChildren(), 
					mergeTree.GetMergeItems);
			}

			if (!(KEEP_IMPORT_BOOKMARKS && !ADD_BOOKMARK_FOR_EACH_FILE))
			{
				// eliminate all of the current bookmarks
				destPdfOutlines = clearOutline(destPdf);
			}

			if (ADD_BOOKMARK_FOR_EACH_FILE)
			{
				// add the new bookmarks
				addOutline(mergeTree.GetMergeItems, destPdfOutlines, 0);
				
			}
			// all complete
			// leave open for further processing
			//			destPdf.Close();

			// return the final PDF document

			PdfCatalog destPdfCatalog = destPdf.GetCatalog();

			destPdfCatalog.SetPageMode(PdfName.UseOutlines);
			destPdfCatalog.SetPageLayout(PdfName.SinglePage);
			destPdfCatalog.SetOpenAction(PdfExplicitDestination.CreateFit(1));
			
			return destPdf;
		}

		private int merge(PdfMerger merger, List<MergeItem> mergeTreeList, int initialPageCount)
		{

			int pageCount = initialPageCount;
			PdfDocument src = null;

			foreach (MergeItem mi in mergeTreeList)
			{
				if (mi.fileItem.ItemType == FILE)
				{
					Form1.updateProgressBar(Form1.ProgressBarValue.isIncrement, 1,0);

					mi.pageNumber = pageCount;

					try
					{
						src = new PdfDocument(new PdfReader(mi.fileItem.getFullPath));

						if (!(KEEP_IMPORT_BOOKMARKS && !ADD_BOOKMARK_FOR_EACH_FILE))
						{
							destPdfOutlines.AddOutline(FILE_MARKER + mi.fileItem.getName());
						}

						merger.Merge(src, 1, src.GetNumberOfPages());

						if (src != null && !src.IsClosed())
						{
							logMsgDbLn2("adding file", ("adding (" + src.GetNumberOfPages() + ") pages").PadRight(20) + ":: " + mi.fileItem.getName());
							pageCount += src.GetNumberOfPages();
							src.Close();

						}
						else
						{
							logMsgln(" added(-no-) pages");
						}
					}
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
					catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
					{
						logMsgFmtln("corrupted PDF found");

						if (!CONTINUE_ON_CORRUPT_PDF) { return -1; }

						mi.fileItem.ItemType = MISSING;
						mi.pageNumber = -1;
					}
				}

				if (mi.mergeItems != null)
				{
					pageCount = merge(merger, mi.mergeItems, pageCount);

					if (pageCount < 1) break;
				}
			}

			return pageCount;
		}

		PdfOutline clearOutline(PdfDocument pdf)
		{
			// clear the current outline tree
			pdf.GetOutlines(false).GetContent().Clear();

			// initalize a new outline tree
			pdf.InitializeOutlines();

			return pdf.GetOutlines(true);
		}

		// copy existing outlines and add to the merge tree
		private void MergeOutlines(IList<PdfOutline> destOutlines, 
			List<MergeItem> mergeTreeList)
		{
			if (destOutlines.Count == 0) return;

			int i = 0;

			string currDestTitle;

			foreach (MergeItem mi in mergeTreeList)
			{
				currDestTitle = destOutlines[i].GetTitle().Substring(1);

				if (mi.bookmarkType == BRANCH && mi.mergeItems.Count > 0)
				{
					i = mergeOutlines(destOutlines, mi.mergeItems, i);

					if (i < 0) return;
				}
				else if (mi.bookmarkType == LEAF)
				{
					if (mi.bookmarkTitle.Equals(currDestTitle))
					{
						addExistBookmarks(destOutlines, mi, i + 1);

						i = findNextHeader(destOutlines, i);

						if (i < 0) return;
					}
				}
			}
		}

		private int findNextHeader(IList<PdfOutline> destOutlines, int currDestItem)
		{
			for (int i = currDestItem + 1; i < destOutlines.Count; i++)
			{
				if (destOutlines[i].GetTitle().Substring(0, 1).Equals(FILE_MARKER))
				{
					return i;
				}
			}

			return -1;
		}

		private int mergeOutlines(IList<PdfOutline> destOutlines,
			List<MergeItem> mergeTreeList, int currDestItem)
		{
			int i = currDestItem;

			string currDestTitle;

			foreach (MergeItem mi in mergeTreeList)
			{
				currDestTitle = destOutlines[i].GetTitle().Substring(1);

				if (mi.bookmarkType == BRANCH && mi.mergeItems.Count > 0)
				{
					i = mergeOutlines(destOutlines, mi.mergeItems, i);

					if (i < 0) return i;
				}
				else if (mi.bookmarkType == LEAF)
				{
					if (mi.bookmarkTitle.Equals(currDestTitle))
					{
						addExistBookmarks(destOutlines, mi, i + 1);

						i = findNextHeader(destOutlines, i);

						if (i < 0) return i;
					}
				}
			}
			return i;
		}

		private void addExistBookmarks(IList<PdfOutline> destOutlines, 
			MergeItem mi, int currDestItem)
		{
			int i = currDestItem;

			while (!destOutlines[i].GetTitle().StartsWith(FILE_MARKER))
			{
				addExistBookmark2(destOutlines[i], mi.mergeItems, mi.depth + 1);

				i++;

				if (i >= destOutlines.Count) return;
			}
		}

		private void addExistBookmark2(PdfOutline destOutline,
			List<MergeItem> mergeTreeList, int depth)
		{
			int pageNumber = destOutline.GetPageNumber(destPdf);

			List<MergeItem> newMergeTreeList = new List<MergeItem>(1);

			mergeTreeList.Add(new MergeItem(destOutline.GetTitle(),
					LEAF, newMergeTreeList, pageNumber, depth, new FileItem()));

			IList<PdfOutline> children = destOutline.GetAllChildren();

			if (children.Count > 0)
			{
				foreach (PdfOutline child in children)
				{
					addExistBookmark2(child, newMergeTreeList, depth + 1);
				}
				
			}
		}

		private void addOutline(List<MergeItem> mergeTree, 
			PdfOutline child, int currDepth)
		{
			int pageNumber;

			foreach (MergeItem mi in mergeTree)
			{
				if (mi.fileItem.isMissing) continue;

				pageNumber = findPageNumber(mi);

				if (pageNumber == -2) continue;

				PdfOutline grandChild =
					child.AddOutline(mi.bookmarkTitle);

				grandChild.SetOpen(!COLLAPSE_FILE_BOOKMARKS);


				if (pageNumber >= 0)
				{
					grandChild.AddDestination(
						PdfExplicitDestination.CreateFit(pageNumber));
				}

				if (mi.mergeItems != null)
				{
					addOutline(mi.mergeItems, grandChild, currDepth + 1);
				}
			}
		}

		int findPageNumber(MergeItem mi)
		{
			if (mi.fileItem.isMissing) return -2;

			if (mi.pageNumber >= 0) return mi.pageNumber;

			if (ADD_PAGE_TO_ALL_BOOKMARKS)
			{
				return !mi.hasChildren ? -1 : findPageNumber(mi.mergeItems[0]);
			}

			return -1;
		}

		private static int depth = 0;

		public void listOutline(PdfDocument pdfDoc, PdfOutline outline)
		{
			logMsgFmtln("bookmark| depth", formatBookmark(outline.GetTitle(), 
				depth, outline.GetPageNumber(pdfDoc)));

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
			return String.Format("{1,3} | page| {0,3} | {2}{3}",
				page, depth, StringExtensions.Repeat(" ", depth * 2), title);
		}
	}
}
