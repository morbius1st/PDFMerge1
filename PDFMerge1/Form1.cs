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

using static PDFMerge1.Utility;
using static PDFMerge1.FileList;
using static PDFMerge1.Samples;

namespace PDFMerge1
{
	public partial class Form1 : Form
	{
		private const string OUTPUTFILE = @"\output.pdf";

		private const bool OVERWITE_OUTPUT = true;


		public Form1()
		{
			InitializeComponent();

			Utility.txInfo = txInfo;

			//			Utility.output = 1;

//						listSamples();
			//			pathTests()
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

//		private void pathTests()
//		{
//			string t1 = @"\1adf\2sdf\3sdf\4sddf\5sddf";
//			string t2 = @"\1adf\2sdf\3sdf\";
//			string t3 = @"\1adf";
//			string t4 = @"\";
//			string t5 = @"";
//
//			int depth = 1;
//			
//			testPaths(t1, depth);
//			testPaths(t2, depth);
//			testPaths(t3, depth);
//			testPaths(t4, depth);
//			testPaths(t5, depth);
//		}

//		void testPaths(string testPath, int depth)
//		{
//			FileItem fi = new FileItem(testPath, FileItem.FileItemType.FILE);
//
//			logMsgFmt("test depth| ", depth.ToString());
//			logMsgln("  test path| ", testPath);
//			logMsg(nl);
//			logMsgFmtln("subdir via getsub| ", ">" + testPath.GetSubDirectory(depth) + "<");
//
//			if (testPath.Equals(""))
//			{
//				logMsgFmtln("subdir via fileitem| ", "illegal");
//			}
//			else 
//			{
//				logMsgFmtln("subdir via fileitem| ", ">" + fi.getDirectoryNameToDepth(depth) + "<");
//			}
//			logMsg(nl);
//			logMsgFmtln("name via getsubname| ", ">" + testPath.GetSubDirectoryName(depth) + "<");
//			logMsgFmtln("name via fileitem| ", ">" + fi.getDirectoryNameAtDepth(depth) + "<");
//			logMsg(nl);
//
//		}



		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}


		private void btnSelectFolder_Click(object sender, EventArgs e)
		{
			string selectedFolder;
			string outputFile;

			FileList fileList;

			logMsgln("selecting folder");

			clearConsole();

			//			selectedFolder = new SelectFolder().selectFolder();
			//
			//			if (selectedFolder == null) { return; }
			//
			//			outputFile = selectedFolder + OUTPUTFILE;
			//
			//			if (!verifyOutputFile(outputFile)) { return;  }
			//
			//			listOutput(selectedFolder, outputFile);
			//
			//			fileList = new FileList(selectedFolder);

			//			fileList.Add();

			outputFile = OUTPUT_ORIG;

			fileList = selectFiles(Samples.orig, ROOT_FOLDER);

//			Utility.output = 1;

//			logMsgln(fileList.ToString());
//			logMsgln(fileList.listFiles());

			PdfMergeTree pdfMergeTree = new PdfMergeTree(fileList.RootPath);
			pdfMergeTree.Add(fileList);

//			logMsg(nl);
//			logMsgln(pdfMergeTree.ToString());

			PdfMergeFileList merger = new PdfMergeFileList();

			merger.Merge(outputFile, pdfMergeTree.GetMergeItems);

//			logMsg(nl);
//			logMsgln(pdfMergeTree.ToString());

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

		private void listOutput(string selectedFolder, string outputFile)
		{
			logMsgFmtln("output path| ", selectedFolder);
			logMsgFmtln("output file| ", outputFile);
			logMsgln("");
		}

}


}
