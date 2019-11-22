/*
 * This program is free software; you can redistribute it and/or modify it under the terms of the 
 * GNU Affero General Public License version 3 as published by the Free Software Foundation with 
 * the addition of the following permission added to Section 15 as permitted in Section 7(a): 
 * FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY ITEXT GROUP NV, ITEXT GROUP 
 * DISCLAIMS THE WARRANTY OF NON INFRINGEMENT OF THIRD PARTY RIGHTS

 *	This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 *	without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
 *	See the GNU Affero General Public License for more details. You should have received a copy of the 
 * 	GNU Affero General Public License along with this program; if not, see http://www.gnu.org/licenses/ 
 *	or write to the Free Software Foundation, Inc., 
 *	51 Franklin Street, Fifth Floor, Boston, MA, 02110-1301 USA, 
 *	or download the license from the following URL: http://itextpdf.com/terms-of-use/
 */


using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using PDFMerge1.Support;
using UtilityLibrary;

using static PDFMerge1.FileList;
using static PDFMerge1.Samples;
using static PDFMerge1.SelectFolder;

using static UtilityLibrary.MessageUtilities;

namespace PDFMerge1
{
	public partial class Form1 : Form
	{
		private const bool OVERWITE_OUTPUT = true;
		private const string WHO_AM_I = "@Form1";

		private static ProgressBar pb;
		private static Label lb;

		private DebugSupport.DebugSupport dbs = null;


		public Form1()
		{
			InitializeComponent();

			UtilityLocal.txInfo = txInfo;

			pb = progressBar1;
			lb = label1;

			dbs = DebugSupport.DebugSupport.Instance;
		}

		private void listSamples()
		{
			logMsg(nl);
			logMsgln("original file list");
			listSample(Samples.orig, OUTPUT_ORIG);

			logMsg(nl);
			logMsgln("alternate 1 file list");
			listSample(Samples.alt1, OUTPUT_ALT1);

			logMsg(nl);
			logMsgln("alternate 2 file list");
			listSample(Samples.alt2, OUTPUT_ALT2);
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}


		private void btnSelectFolder_Click(object sender, EventArgs e)
		{
			string selectedFolder;
			string outputFile;

			FileList fileList;

			MessageUtilities.OutLocation = OutputLocation.TEXT_BOX;
			MessageUtilities.RichTxtBox = txInfo;
			MessageUtilities.AltColumn = 40;

			pb.Value = 0;

//			ClearConsole();

			logMsgFmtln(WHO_AM_I + "-0 selecting folder");

			SelectFolder selFolder = new SelectFolder();


			selectedFolder = selFolder.selectFolder();	

//			selectedFolder = new SelectFolder().selectFolderTest(Test.SEL_PATH);				// passed
//			selectedFolder = new SelectFolder().selectFolderTest(Test.NORMAL);					// passed
//			selectedFolder = new SelectFolder().selectFolderTest(Test.PDF_IN_INDIV_PDF_FOLDER);	// passed
//			selectedFolder = new SelectFolder().selectFolderTest(Test.NO_PDFS);                 // passed
//			selectedFolder = new SelectFolder().selectFolderTest(Test.EMPTY_SUB_FOLDER);		// passed
//			selectedFolder = new SelectFolder().selectFolderTest(Test.CORRUPT_PDF);				// passed
//			selectedFolder = new SelectFolder().selectFolderTest(Test.NON_PDF);					// passed
//			selectedFolder = new SelectFolder().selectFolderTest(Test.ROOT_PDFS);				// passed
//			selectedFolder = new SelectFolder().selectFolderTest(Test.PDF_FOLDER_SELECTED);		// passed
//			selectedFolder = new SelectFolder().selectFolderTest(Test.NO_SUCH_FOLDER);			// passed


			outputFile = OUTPUT_ORIG;

			if (!PdfMergeFileList.VerifyOutputFile(outputFile))
			{
				return;
			}

			if (selectedFolder == null)
			{
				logMsgFmtln("folder not found");
				return;
			}

			listFolders(selectedFolder, outputFile);

			fileList = new FileList(selectedFolder);

			fileList.Add("*.pdf", SearchOption.AllDirectories);

			if (fileList.NetCount == 0)
			{
				logMsgFmtln("no files found");
				return;
			}


			logMsgFmtln(WHO_AM_I + " @3 gross count", fileList.GrossCount);

			logMsgFmtln(WHO_AM_I + " @5 create merge tree");
			PdfMergeTree pdfMergeTree = new PdfMergeTree(fileList.RootPath);



//			ListInfo.Instance.ListBootmarkTitles(dbs.BookmarkTitles);

//			DebugSupport.DebugSupport.Instance.ModifyFileList(fileList);
//
//			fileList.Sort();

//			return;

//			ListInfo.Instance.ListFiles(fileList);



			logMsgFmtln(WHO_AM_I + " @7 add files to merge tree");
			pdfMergeTree.Add(fileList);


			ListInfo.Instance.ListMergeTree(pdfMergeTree);

			txInfo.Refresh();

//			return;

			PdfMergeFileList fileListMerger = new PdfMergeFileList();

			logMsgFmtln(WHO_AM_I + "-10 merge files");

			PdfDocument pdf = fileListMerger.Merge(outputFile, pdfMergeTree);

			if (pdf != null)
			{
				fileListMerger.listOutline(pdf, pdf.GetOutlines(false));
				logMsgFmtln("done and closed");

				pdf.Close();
			}
			else
			{
				logMsgFmtln("done and failed");
				updateProgressBar(ProgressBarValue.isAmount, pb.Maximum, 0);

				File.Delete(outputFile);

			}

		}

		void testInsert(PdfMergeTree tree)
		{
			string toFind = "Cover Page";
			string insert = "this is a test";
		}

		private FileList selectFiles(List<string> files, string rootFolder)
		{
			FileList fileList = new FileList(rootFolder);

			int loc = fileList.Add(@"B:\Programming\VisualStudioProjects\PDFMerge1\PDFMerge1\Study\Cover Page.pdf");

			foreach (String file in files)
			{
				fileList.Add(ROOT_FOLDER + file);
			}

			// add a couple of extra test items
			fileList.Add("this is junk");

			fileList.Move(loc, 0);

			return fileList;
		}

//		private bool verifyOutputFile(string outputFile)
//		{
//			if (outputFile == null || outputFile.Length < 3) { return false; }
//
//			if (File.Exists(outputFile))
//			{
//				if (OVERWITE_OUTPUT)
//				{
//					try
//					{
//						var f = File.OpenWrite(outputFile);
//						f.Close();
//						logMsgFmtln("output file", "exists: overwrite allowed\n");
//						return true;
//					}
//					catch (Exception ex)
//					{
//						logMsgFmtln("result", "fail - file is not accessable or is open elsewhere\n");
//						return false;
//					}
//				}
//				else
//				{
//					logMsgFmtln("output file", "exists: overwrite disallowed\n");
//					return false;
//				}
//			}
//
//			logMsgFmtln("output file", "does not exists\n");
//			return true;
//		}
//
		private void listFolders(string selectedFolder, string outputFile)
		{
			logMsgFmtln("input folder", selectedFolder);
			logMsgFmtln("output file", outputFile);
			logMsgln("");
		}

		internal static void configProgressBar(int fileCount)
		{
			if (fileCount > 0)
			{
				pb.Maximum = fileCount;
				lb.Text = fileCount.ToString();
				pb.Value = pb.Minimum;
			}
		}

		internal enum ProgressBarValue { isIncrement, isAmount }

		internal static void updateProgressBar(ProgressBarValue pbv, int value, int fileCount)
		{
			if (value >= 0)
			{
				if (pbv == Form1.ProgressBarValue.isIncrement)
				{
					if (pb.Value + value == pb.Maximum)
					{
						pb.Maximum += value + 1;
						pb.Value += value + 1;
						pb.Maximum = value;
					}
					else
					{
						pb.Value += value + 1;
						pb.Value -= 1;
					}
				}
				else
				{
					if (value == pb.Maximum)
					{
						pb.Maximum = value + 1;
						pb.Value = value + 1;
						pb.Maximum = value;
					}
					else
					{
						pb.Value = value + 1;
					}
					pb.Value = value;
				}
			}
			configProgressBar(fileCount);

		}
	}
}
