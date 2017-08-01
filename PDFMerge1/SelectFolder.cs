using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static PDFMerge1.Utility;
using static PDFMerge1.Samples;

namespace PDFMerge1
{
	class SelectFolder
	{

		private int maxFileNameLen = 0;

		private string error;

		
		
		private const string PDF_FOLDER_SEL = NORMAL_FOLDER + PDF_FOLDER;

		enum Test
		{
			SEL_PATH, NORMAL, NO_PDF_FOLDER, NO_PDFS,
			EMPTY_SUB_FOLDER, CORRUPT_PDF, NON_PDF, ROOT_PDFS,
			PDF_FOLDER_SELECTED
		}

		private Test choice =
			//			Test.SEL_PATH;
						Test.NORMAL;
			//			Test.NO_PDF_FOLDER;
			//			Test.NO_PDFS;
			//			Test.EMPTY_SUB_FOLDER;
			//			Test.CORRUPT_PDF;
			//			Test.NON_PDF;
//			Test.ROOT_PDFS;
		//			Test.PDF_FOLDER_SELECTED;

		internal string selectFolder()
		{
			bool result = false;
			string path = BASE_FOLDER;

			string output_file;

			switch (choice)
			{
				case Test.SEL_PATH:
				{
					logMsgln("Select Path");
					path = selectPath();
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
					path = selectPath();
					break;
				}
			}

			return pathSet(path);

		}

		string pathSet(string path)
		{
			if (path == null || path.Length < 3) { return null; }

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

		private string selectPath()
		{
			FolderBrowserDialog f = new FolderBrowserDialog();

			f.SelectedPath = BASE_FOLDER;
			f.ShowNewFolderButton = false;
			f.Description = "Select Root PDF Folder";

			if (f.ShowDialog() == DialogResult.OK)
			{
				return f.SelectedPath;
			}

			return null;
		}

	}
}
