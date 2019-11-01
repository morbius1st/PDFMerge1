using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using static UtilityLibrary.MessageUtilities;
using static PDFMerge1.Samples;

namespace PDFMerge1
{
	class SelectFolder
	{

		private int maxFileNameLen = 0;

		private string error;

		
		
		private const string PDF_FOLDER_SEL = NORMAL_FOLDER + PDF_FOLDER;

		internal enum Test
		{
			SEL_PATH,
			NORMAL,
			PDF_IN_INDIV_PDF_FOLDER,
			NO_PDFS,
			EMPTY_SUB_FOLDER,
			CORRUPT_PDF,		
			NON_PDF,
			ROOT_PDFS,
			PDF_FOLDER_SELECTED,
			NO_SUCH_FOLDER
		}

		internal string selectFolder()
		{
			string selectedPath = selectPath(Properties.Settings.Default.GetBasePath);

			if (string.IsNullOrEmpty(selectedPath)) return null;

			Properties.Settings.Default.SaveBasePath = selectedPath;


			System.Collections.Specialized.StringCollection Recents = Properties.Settings.Default.RecentBasePaths;

			if (Recents != null)
			{
				logMsgFmtln("string collection| size", Recents.Count);

				int i = 0;

				foreach (string recent in Recents)
				{
					logMsgFmtln("recent " + i + " ", recent);
				}
			}
			else
			{
				logMsgFmtln("string collection| size", "Recents is empty");
				Recents = new StringCollection();
				Recents.Add(selectedPath);
				Properties.Settings.Default.RecentBasePaths = Recents;
				Properties.Settings.Default.Save();
			}

			return selectedPath;
		}

		private string selectPath(string lastBasePath)
		{
			FolderBrowserDialog f = new FolderBrowserDialog();
			
			f.SelectedPath = lastBasePath;
			f.ShowNewFolderButton = false;
			f.Description = "Select Root PDF Folder";

			if (f.ShowDialog() == DialogResult.OK)
			{
				return f.SelectedPath;
			}

			return null;
		}

//
//		internal string selectFolderTest(Test choice)
//		{
//			bool result = false;
//			string path = BASE_FOLDER;
//			string message;
//			string folderName = "";
//
//			string output_file;
//
//			switch (choice)
//			{
//				case Test.SEL_PATH:
//				{
//					message = "Select Path";
//					folderName = null;
//					path = selectPath(BASE_FOLDER);
//					break;
//				}
//				case Test.NORMAL:
//				{
//					message = "NORMAL folder";
//					folderName = NORMAL_FOLDER;
//					break;
//				}
//				case Test.PDF_IN_INDIV_PDF_FOLDER:
//				{
//					message = "PDFs not in indiv pdf folder";
//					folderName = PDF_IN_INDIV_PDF_FOLDER;
//					break;
//				}
//				case Test.NO_PDFS:
//				{
//					message = "NO PDFs folder";
//					folderName = NO_PDFS;
//					break;
//				}
//				case Test.EMPTY_SUB_FOLDER:
//				{
//					message = "EMPTY sub_folder";
//					folderName = EMPTY_SUB_FOLDER;
//					break;
//				}
//				case Test.CORRUPT_PDF:
//				{
//					message = "CORRUPT pdf folder";
//					folderName = CORRUPT_PDF;
//					break;
//				}
//				case Test.NON_PDF:
//				{
//					message = "has NON_PDF folder";
//					folderName = NON_PDF;
//					break;
//				}
//				case Test.ROOT_PDFS:
//				{
//					message = "has ROOT PDFs folder";
//					folderName = ROOT_PDFS;
//					break;
//				}
//				case Test.PDF_FOLDER_SELECTED:
//				{
//					message = "PDF folder selected";
//					folderName = PDF_FOLDER_SEL;
//					break;
//				}
//				case Test.NO_SUCH_FOLDER:
//				{
//					message = "no such selected";
//					folderName = NO_SUCH_FOLDER;
//					break;
//				}
//				default:
//				{
//					message = "default";
//					folderName = null;
//					path = selectPath(BASE_FOLDER);
//					break;
//				}
//			}
//
//			if (folderName == null)
//			{
//				folderName = choice.ToString();
//			}
//			else
//			{
//				path += folderName;
//			}
//
//			logMsgln(message, folderName);
//			logMsgFmtln("path",path);
//
//			return pathSet(path);
//
//		}
//
//		// validate path
//		string pathSet(string path)
//		{
//			// is path null or too short or does not exist
//			if (path == null 
//				|| path.Length < 3
//				|| !Directory.Exists(path)) { return null; }
//
//			// break apart path
//			string[] pathNames = splitPath(path);
//
//			// is the path a PDF folder?
//			if (pathNames[pathNames.Length - 1].Equals(PDF_FOLDER.Substring(1)))
//			{
//				return path;
//			}
//
//			// can we find the "PDF folder"?
//			if (Directory.Exists(path + PDF_FOLDER))
//			{
//				return path + PDF_FOLDER;
//			}
//
//			// all else fails, folder does exist, just
//			// return the folder selected
//			return path;
//		}



	}
}
