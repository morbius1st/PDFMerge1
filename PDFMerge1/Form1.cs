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
using EnvDTE;
using EnvDTE80;
using iText.Kernel;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.Utils;


namespace PDFMerge1
{
	public partial class Form1 : Form
	{
		private int maxFileNameLen = 0;

		private string nl = Environment.NewLine;
		private const string spacer = "                                  ";

		private const string PDF_FOLDER = @"\Individual PDFs";

		private const string START_FOLDER = @"C:\2099-999 Sample Project\Publish\Bulletins";
		
		private const string NORMAL_FOLDER =	@"\2017-07-01 normal";
		private const string NO_PDF_FOLDER =	@"\2017-07-01 not in sub-folder and an empty sub-folders";
		private const string NO_PDFS =			@"\2017-07-01 no PDFs";
		private const string EMPTY_SUB_FOLDER = @"\2017-07-01 with an empty sub-folders";
		private const string CORRUPT_PDF =		@"\2017-07-01 with corrupted PDF";
		private const string NON_PDF =			@"\2017-07-01 with Non-PDF's";
		private const string ROOT_PDFS =		@"\2017-07-01 with PDFs in root";
		private const string PDF_FOLDER_SEL =	NORMAL_FOLDER + PDF_FOLDER;

		public Form1()
		{
			InitializeComponent();
		}

		enum Test { SEL_PATH, NORMAL, NO_PDF_FOLDER, NO_PDFS,
			EMPTY_SUB_FOLDER, CORRUPT_PDF , NON_PDF, ROOT_PDFS,
			PDF_FOLDER_SELECTED }

		private Test choice =
//			Test.SEL_PATH;
//			Test.NORMAL;
//			Test.NO_PDF_FOLDER;
//			Test.NO_PDFS;
//			Test.EMPTY_SUB_FOLDER;
//			Test.CORRUPT_PDF;
//			Test.NON_PDF;
			Test.ROOT_PDFS;
//			Test.PDF_FOLDER_SELECTED;

		class Bookmark
		{
			public string bookmarkPath { get; private set; }
			public string bookmark { get; }
			public int pages { get; set; }

			public Bookmark(string bookmarkPath, string bookmark)
			{
				this.bookmarkPath = bookmarkPath;
				this.bookmark = bookmark;
			}

			private void setBookmarkPath(string bookmarkPath)
			{
				this.bookmarkPath = bookmarkPath;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}


		private void btnSelectFolder_Click(object sender, EventArgs e)
		{
			clearConsole();

			bool result = true;
			bool keepOldBookmarks = false;
			bool keepOldTags = false;

			string path = START_FOLDER;

			string output_file;

			switch (choice)
			{
				case Test.SEL_PATH:
				{
					logMsgln("Select Path");
					selectPath();
					break;
				}
				case Test.NORMAL:
				{
					logMsgln("NORMAL folder");
					path += NORMAL_FOLDER;
					break;
				}
				case Test.NO_PDF_FOLDER:
				{
					logMsgln("NO PDF sub-folder");
					path += NO_PDF_FOLDER;
						break;
				}
				case Test.NO_PDFS:
				{
					logMsgln("NO PDFS");
					path += NO_PDFS;
						break;
				}
				case Test.EMPTY_SUB_FOLDER:
				{
					logMsgln("EMPTY sub_folder");
					path += EMPTY_SUB_FOLDER;
						break;
				}
				case Test.CORRUPT_PDF:
				{
					logMsgln("CORRUPT pdf");
					path += CORRUPT_PDF;
						break;
				}
				case Test.NON_PDF:
				{
					logMsgln("has NON_PDF");
					path += NON_PDF;

					break;
				}
				case Test.ROOT_PDFS:
				{
					logMsgln("as ROOT PDFs");
					path += ROOT_PDFS;
					break;
				}
				case Test.PDF_FOLDER_SELECTED:
				{
					logMsgln("PDF FOLDER selected");
					path += PDF_FOLDER_SEL;
					break;
				}
	
				default:
				{
					selectPath();
					break;
				}
			}

			if (choice > Test.SEL_PATH)
			{
				logMsgFmtln("proposed path| ", path);

				path = pathSet(path);

				if (path != null)
				{
					output_file = Path.GetDirectoryName(path) + "\\output.pdf";
					logMsgFmtln("output file| ", output_file);

					logMsgFmtln("final path| ", path);
					logMsg(nl);

					Dictionary<string, Bookmark> bookmarkList = createBookmarkList2(path);

					if (bookmarkList != null)
					{
//						listBookmarkTree(bookmarkList, path);
						mergePDF(bookmarkList, path, output_file, 
							keepOldTags, keepOldBookmarks);
					}
					else
					{
						logMsgFmtln("bookmark list| ", "is null");
					}
					
				}
			}


			logMsgFmtln("result| ", result.ToString());
		}

		string pathSet(string path)
		{
			string[] pathNames = splitPath(path);

			// is the base folder a PDF folder?
			if (!pathNames[pathNames.Length - 1].Equals(PDF_FOLDER.Substring(1)))
			{
				// create and test an adjusted path
				path += PDF_FOLDER;

				if (!Directory.Exists(path))
				{
					// a PDF folder does not exist - return null
					path = null;
				}
			}

			return path;
		}

		void selectPath()
		{
			FolderBrowserDialog f = new FolderBrowserDialog();

			f.SelectedPath = START_FOLDER;
			f.ShowNewFolderButton = false;
			f.Description = "Select Root PDF Folder";

			if (f.ShowDialog() == DialogResult.OK)
			{
				//				return f.SelectedPath;

				if (f.SelectedPath != null)
				{
					txInfo.Text = f.SelectedPath + nl;

					//				Console.WriteLine("enumerate directories");
					//				Console.WriteLine("\ttop level directories only");
					//				listDirectories(selectedPath, SearchOption.TopDirectoryOnly);
					//
					//				Console.WriteLine("\tall directories");
					//				listDirectories(selectedPath, SearchOption.AllDirectories);
					//
					//				Console.WriteLine(nl);
					//				Console.WriteLine("enumerate files");
					//				Console.WriteLine("\tall files");
					//				listFiles(selectedPath, SearchOption.AllDirectories);
					//
					//				Console.WriteLine(nl);
					//				Console.WriteLine("enumerate files objs");
					//				Console.WriteLine("\tall files");
					//				listFileObjs1(selectedPath, SearchOption.AllDirectories);

					Console.WriteLine(nl);
					Console.WriteLine("enumerate files objs");
					Console.WriteLine("\tall files");
					listFileObjs2(f.SelectedPath, SearchOption.AllDirectories);

				}
				else
				{
					txInfo.Text = "Not Selected";
				}
			}
		}

		// enumerate all directories (+ sub-directories) in a directory


		Dictionary<string, Bookmark> createBookmarkList1(string path)
		{
			int rootDirNameLength = path.Length + 1;
			int len;

			string key;
			string bookmarkPath;
			string bookmark;

			Dictionary<string, Bookmark> bookmarkList = new Dictionary<string, Bookmark>(10);

			try
			{
				DirectoryInfo diStart = new DirectoryInfo(path);

				foreach (DirectoryInfo di in diStart.EnumerateDirectories("*"))
				{
					List<FileInfo> fileInfos = new List<FileInfo>(di.EnumerateFiles("*.pdf", SearchOption.AllDirectories));

					foreach (FileInfo fi in fileInfos)
					{
						len = fi.FullName.Length - rootDirNameLength - 4;

						key = fi.FullName.Substring(rootDirNameLength, len);
						bookmarkPath = fi.DirectoryName.Substring(rootDirNameLength);
						bookmark = fi.Name.Substring(0, fi.Name.Length - 4);

						Bookmark bm = new Bookmark(bookmarkPath, bookmark);

						bookmarkList.Add(key, bm);
					}
				}
				if (bookmarkList.Count == 0)
				{
					bookmarkList = null;
				}
			}
			catch (Exception e)
			{
				bookmarkList = null;
			}

			return bookmarkList;
		}

		Dictionary<string, Bookmark> createBookmarkList2(string path)
		{
			int rootDirNameLength = path.Length + 1;
			int len;

			string key;
			string bookmarkPath = "";
			string bookmark;

			Dictionary<string, Bookmark> bookmarkList = new Dictionary<string, Bookmark>(10);

			try
			{
				DirectoryInfo diStart = new DirectoryInfo(path);
//
//				foreach (DirectoryInfo di in diStart.EnumerateDirectories("*"))
//				{
				List<FileInfo> fileInfos = new List<FileInfo>(diStart.EnumerateFiles("*.pdf", SearchOption.AllDirectories));

				foreach (FileInfo fi in fileInfos)
				{
					len = fi.FullName.Length - rootDirNameLength - 4;

					key = fi.FullName.Substring(rootDirNameLength, len);

					int dirNameLen = fi.DirectoryName.Length + 1;

					if (rootDirNameLength < dirNameLen)
					{
						bookmarkPath = fi.DirectoryName.Substring(rootDirNameLength);
					}

					len = fi.Name.Length;

					if (len > maxFileNameLen)
					{
						maxFileNameLen = len;
					}

					bookmark = fi.Name.Substring(0, len - 4);

					Bookmark bm = new Bookmark(bookmarkPath, bookmark);

					bookmarkList.Add(key, bm);
				}
//				}
				if (bookmarkList.Count == 0)
				{
					bookmarkList = null;
				}
			}
			catch (Exception e)
			{
				bookmarkList = null;
			}

			return bookmarkList;
		}

		void mergePDF(Dictionary<string, Bookmark> bookmarkList, 
			String rootPath, string outputName, bool keepOldTags, bool keepOldBookmarks)
		{
			const int maxBookmarkDepth = 32;

			int tabLevel = 1;
			int bookmarkSplitPathLength;
			int pagesDoc;
			int pagesTotal = 1;

			string[] bookmarkSplitPath;
			string priorBookmark = "";
			string bookmarkKey = "";
			string fileAndPath;

			string pattern = "{0,-" + (maxFileNameLen + 5) + "}";

			bool found;

			PdfOutline[] pdfOutlineList = new PdfOutline[maxBookmarkDepth];


			PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outputName));
			PdfMerger merger = new PdfMerger(pdfDoc, keepOldTags, keepOldBookmarks);
			merger.SetCloseSourceDocuments(false);

			pdfOutlineList[0] = pdfDoc.GetOutlines(false);

			foreach (KeyValuePair<string, Bookmark> kvp in bookmarkList)
			{
				bookmarkSplitPath = splitPath(kvp.Value.bookmarkPath);
				bookmarkSplitPathLength = bookmarkSplitPath.Length;

				if (!kvp.Value.bookmarkPath.Equals(priorBookmark))
				{
					// changing levels and bookmark depth
					priorBookmark = kvp.Value.bookmarkPath;
					tabLevel = bookmarkSplitPathLength;

					if (tabLevel > maxBookmarkDepth)
					{
						tabLevel = maxBookmarkDepth;
					}

					// list current bookmark key
					if (tabLevel >= 1)
					{
						bookmarkKey = bookmarkSplitPath[tabLevel - 1];

						logMsgln("tab level| " + tabLevel);
						logMsg(spacer.Substring(0, tabLevel * 3));
						logMsgln(bookmarkKey);

						tabLevel++;
					}
					else
					{
						tabLevel = 1;
						bookmarkKey = "";
					}
				}

// *******
//tabLevel = 1;

				

				fileAndPath = rootPath + "\\";

				if (kvp.Value.bookmarkPath.Length > 0)
				{
					fileAndPath += kvp.Value.bookmarkPath + "\\";
				}

				fileAndPath += kvp.Value.bookmark + ".pdf";

				found = File.Exists(fileAndPath);

				logMsg(spacer.Substring(0, tabLevel * 3));
				logMsgln(kvp.Value.bookmark + ".pdf");


				//				logMsgFmt("Merging| ", 
				//					String.Format(pattern, kvp.Value.bookmark + ".pdf") 
				//					+ "  found?| ");

				if (found)
				{
					try
					{
						PdfDocument src = new PdfDocument(new PdfReader(fileAndPath));
						pagesDoc = src.GetNumberOfPages();
						merger.Merge(src, 1, pagesDoc);
						src.Close();

//						pdfOutlineList[tabLevel] = pdfOutlineList[tabLevel - 1].AddOutline(kvp.Value.bookmark);
//						pdfOutlineList[tabLevel].AddDestination(PdfExplicitDestination.CreateFit(pdfDoc.GetPage(pagesTotal)));

						pagesTotal += pagesDoc;

//						logMsg("Yes", Color.Green, null);
//						logMsg("  pages| " + pagesDoc + "  total| " + pagesTotal);

					}
					catch (PdfException)
					{
//						logMsg("Corrupted", Color.Red, null);
					}
				}
				else
				{
//					logMsg("No", Color.Maroon, null);
				}

				
				
//				logMsg(nl);
			}

			try
			{
				pdfDoc.Close();
			}
			catch (Exception e)
			{
				logMsgln("PDF File Merge Failed - No File Created");
			}
		}

		void listDirectories(string path, SearchOption searchOption)
		{
			try
			{
				List<string> dirs = new List<string>(Directory.EnumerateDirectories(path, "*", searchOption));

				Console.WriteLine("found: " + dirs.Count);

				foreach (string dir in dirs)
				{
					Console.WriteLine(dir);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		void listFiles(string path, SearchOption searchOption)
		{
			try
			{
				List<string> files = new List<string>(Directory.EnumerateFiles(path, "*", searchOption));

				Console.WriteLine("found: " + files.Count);

				foreach (string file in files)
				{
					Console.WriteLine(file);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}


		void listBookmarkTree(Dictionary<string, Bookmark> bookmarkList, String rootPath)
		{
			int tabLevel = 0;
			int bookmarkSplitPathLength;

			string spacer = "  ";

			string[] bookmarkSplitPath;
			string priorBookmark = "";
			string bookmarkKey;
			string fileAndPath;
			string fileNameSpacer = spacer;

			bool found;


			for (int i = 0; i < 9; i++)
			{
				fileNameSpacer += spacer;
			}

			foreach (KeyValuePair<string, Bookmark> kvp in bookmarkList)
			{
				bookmarkSplitPath = splitPath(kvp.Value.bookmarkPath);
				bookmarkSplitPathLength = bookmarkSplitPath.Length;

				if (!kvp.Value.bookmarkPath.Equals(priorBookmark))
				{
					// changing levels and bookmark depth
					priorBookmark = kvp.Value.bookmarkPath;
					tabLevel = bookmarkSplitPathLength - 1;

					// list current bookmark key
					if (tabLevel >= 0)
					{
						bookmarkKey = bookmarkSplitPath[tabLevel];
						for (int i = 0; i < tabLevel; i++)
						{
							logMsg(spacer);
						}
					}
					else
					{
						tabLevel = 0;
						bookmarkKey = "";
					}
					logMsg("**>");
					logMsgln(bookmarkKey);
				}

				logMsg(fileNameSpacer);
				logMsg(kvp.Value.bookmark);
				logMsg("  (");

				fileAndPath = rootPath + "\\";

				if (kvp.Value.bookmarkPath.Length > 0)
				{
					fileAndPath += kvp.Value.bookmarkPath + "\\";
				}
				fileAndPath += kvp.Value.bookmark + ".pdf";

				logMsg(fileAndPath + " exists?| ");

				found = File.Exists(fileAndPath);

				if (found)
				{
					logMsg(found.ToString(), Color.Green, null);
				}
				else
				{
					logMsg(found.ToString(), Color.Red, null);
				}
				logMsg(nl);

			}
		}

		void listBookmarks(Dictionary<string, Bookmark> bookmarkList)
		{
			foreach (KeyValuePair<string, Bookmark> kvp in bookmarkList)
			{
				listBookmark(kvp);
				logMsg(nl);
			}

		}

		void listBookmark(KeyValuePair<string, Bookmark> kvp)
		{
			Bookmark bm = kvp.Value;

			string[] bookmarkPath = splitPath(bm.bookmarkPath);
			int len = bookmarkPath.Length;
			int idx = 1;

			string title0 = "bookmark key| ";
			string title1 = "bookmark path| ";
			string title2 = "bookmark path split| ";
			string title3 = "bookmark item| ";


			logMsgFmtln(title0, kvp.Key);
			logMsgFmtln(title1, bm.bookmarkPath);
			logMsgFmt(title2, "");

			foreach (string path in bookmarkPath)
			{
				logMsg(path);
				if (idx++ != len)
				{
					logMsg(" <> ");
				}
			}

			logMsg(nl);
			logMsgFmtln(title3, bm.bookmark);
		}

		void listFileObjs2(string path, SearchOption searchOption)
		{
			Console.WriteLine("starting path");
			Console.WriteLine(path);

			int rootDirNameLength = path.Length + 1;
			int len;

			string key;
			string bookmarkPath;
			string bookmark;

			Dictionary<string, Bookmark> bookmarkList = new Dictionary<string, Bookmark>(10);

			try
			{
				DirectoryInfo diStart = new DirectoryInfo(path);

				foreach (DirectoryInfo di in diStart.EnumerateDirectories("*"))
				{
					Console.Write(di.Name);
					Console.Write(nl);

					List<FileInfo> fileInfos = new List<FileInfo>(di.EnumerateFiles("*.pdf", searchOption));

					Console.WriteLine("holds " + fileInfos.Count + " files");

					foreach (FileInfo fi in fileInfos)
					{

						len = fi.FullName.Length - rootDirNameLength - 4;

						key = fi.FullName.Substring(rootDirNameLength, len);
						bookmarkPath = fi.DirectoryName.Substring(rootDirNameLength);
						bookmark = fi.Name.Substring(0, fi.Name.Length - 4);

						Bookmark bm = new Bookmark(bookmarkPath, bookmark);

						bookmarkList.Add(key, bm);

					}
				}

				// got the list of pdf files
				listBookmarks(bookmarkList);

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		void listFileObjs1(string path, SearchOption searchOption)
		{
			Console.WriteLine("starting path");
			Console.WriteLine(path);

			int rootDirNameLength = path.Length + 1;
			int len;

			List<string> pdfFiles = new List<string>(10);

			try
			{
				DirectoryInfo diStart = new DirectoryInfo(path);

				foreach (DirectoryInfo di in diStart.EnumerateDirectories("*"))
				{
					Console.Write(di.Name);
					//					Console.Write(" :: ");
					//					Console.Write(di.);
					Console.Write(nl);


					List<FileInfo> fileInfos = new List<FileInfo>(di.EnumerateFiles("*.pdf", searchOption));

					Console.WriteLine("holds " + fileInfos.Count + " files");

					foreach (FileInfo fi in fileInfos)
					{
						//						listFileObj3(fi, rootDirNameLength);
						//						Console.Write(nl);

						len = fi.FullName.Length - rootDirNameLength - 4;

						pdfFiles.Add(fi.FullName.Substring(rootDirNameLength, len));

					}

				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		void listFileObj3(FileInfo fi, int rootDirNameLength)
		{
			string subDirName = fi.DirectoryName.Substring(rootDirNameLength);
			string fileName = fi.Name;
			string fileAndSubDirName;

			int len = fi.FullName.Length - rootDirNameLength - 4;

			fileAndSubDirName = fi.FullName.Substring(rootDirNameLength, len);

			// this is what would be entered into the sorted list
			Console.Write("\tsort key: >");
			Console.Write(fileAndSubDirName);
			Console.Write("<" + nl);

			Console.Write("\t\tsub dir name : ");
			Console.Write(subDirName);
			Console.Write(nl);

			Console.Write("\t\t   file name : ");
			Console.Write(fileName);
			Console.Write(nl);

		}

		void listFileObj2(FileInfo fi, int rootDirNameLength)
		{
			string subDirName;
			string fileAndSubDirName;

			subDirName = fi.DirectoryName.Substring(rootDirNameLength);
			fileAndSubDirName = fi.FullName.Substring(rootDirNameLength);

			Console.Write("\tsub-directory path: ");
			Console.Write(subDirName);
			Console.Write(nl);
			Console.Write("\tfile name: ");
			Console.Write(fi.Name);
			Console.Write(nl);
			Console.Write("\tfile and sub dir name : ");
			Console.Write(fileAndSubDirName);
			Console.Write(nl);
		}

		void listFileObj1(FileInfo fi, int rootDirNameLength)
		{
			string subDirName;

			subDirName = fi.DirectoryName.Substring(rootDirNameLength);

			Console.Write("\tfile: ");
			Console.Write(fi.Name);
			Console.Write(nl);
			Console.Write("\tsub-directory path: ");
			Console.Write(subDirName);
			Console.Write(nl);
			Console.Write("\t\t******* dir name: ");
			Console.Write(fi.Directory.Name);
			Console.Write(nl);
			Console.Write("\t\t******* dir root: ");
			Console.Write(fi.Directory.Root);
			Console.Write(nl);
			Console.Write("\t\t******* dir parent: ");
			Console.Write(fi.Directory.Parent);
			Console.Write(nl);
			Console.Write("\t\t******* split: ");
			Console.Write(splitPath1(subDirName));
			Console.Write(nl);
		}


		string validateFileName(string proposedFileName)
		{
			return proposedFileName;
		}

		string[] splitPath(string fileAndPath)
		{
			return fileAndPath.Split('\\');
		}


		string splitPath1(string path)
		{
			string[] directories = path.Split('\\');
			StringBuilder sb = new StringBuilder();
			int len = directories.Length;
			int idx = 1;

			Console.Write("\t\t******* split path length: ");
			Console.Write(directories.Length);
			Console.Write(" :>>: ");

			foreach (string pathName in directories)
			{
				sb.Append(pathName);
				if (idx != len)
				{
					sb.Append(" <> ");
				}

				idx++;
			}

			return sb.ToString();

		}

		private int defColumn = 30;

		void logMsgFmtln(string msg1, string msg2, int column = -1)
		{
			logMsgFmt(msg1, msg2, column);
			logMsg(nl);
		}

		void logMsgFmt(string msg1, string msg2, int column = -1)
		{
			logMsg(fmtMsg(msg1, msg2, column));
		}

		void logMsgFmt(string msg1, string msg2, Color color, Font font, int column = -1)
		{
			logMsg(fmtMsg(msg1, msg2, column), color, font);
		}

		string fmtMsg(string msg1, string msg2, int column = -1)
		{
			if (column < 0) { column = defColumn; }

			return String.Format("{0," + column + "}{1}", msg1, msg2);
		}

		void logMsgln(string msg)
		{
			logMsg(msg + nl);
		}

		void logMsg(string msg)
		{
			int output = 0;

			if (output == 0)
			{
				txInfo.AppendText(msg);
			}
			else
			{
				Console.Write(msg);
			}
		}

		void logMsg(string msg, Color color, Font font)
		{
			int output = 0;

			if (output == 0)
			{
				txInfo.AppendText(msg, color, font);
			}
			else
			{
				Console.Write(msg);
			}
		}





		void clearConsole()
		{
			DTE2 ide = (DTE2) System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE");

			OutputWindow w = ide.ToolWindows.OutputWindow;

			w.ActivePane.Activate();
			w.ActivePane.Clear();
			
		}

	}


}
