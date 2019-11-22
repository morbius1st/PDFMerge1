#region + Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AODeliverable.FileSelection;
using AODeliverable.Settings;
using AODeliverable.Support;
using iText.Kernel.Pdf;
using static UtilityLibrary.MessageUtilities;
using Path = System.IO.Path;

#endregion


// projname: AODeliverable.PdfMerge
// itemname: PdfMergeManager
// username: jeffs
// created:  11/3/2019 12:45:45 PM


namespace AODeliverable.PdfMerge
{
	public class PdfMergeMgr
	{
		private PdfMergeMgr()
		{
			sfMgr = SelectFolderMgr.Instance;
			fileMgr = SelectFilesMgr.Instance;
		}

		private static PdfMergeMgr instance = null;

		private SelectFolderMgr sfMgr;
		private SelectFilesMgr fileMgr;
		private string outputPathAndFile;

		public static PdfMergeMgr Instance
		{
			get
			{
				if (instance == null) instance = new PdfMergeMgr();

				return instance;
			}
		}

		public string OutputPathAndFile
		{
			get
			{
				if (!Path.GetExtension(outputPathAndFile).ToLower().Equals("pdf"))
				{
					return outputPathAndFile+".pdf";
				}

				return outputPathAndFile;
			}
			set => outputPathAndFile = value;
		}

		// required:
		// folder selected and files loaded into fileMgr
		public bool Process()
		{
			ListSettings();

			Program.win.configProgressBar.Minimum = 0;
			Program.win.configProgressBar.Maximum = fileMgr.ItemCount;

			if (!sfMgr.Initialized || !fileMgr.Initialized) return false;
			
			bool result;

			// step 1: validate output file
			result = Utilities.VerifyOutputFile(OutputPathAndFile);
			SendStatusMessage(logMsgDbS("Output File", "Ok to proceed| " + result));

			// step 2: convert filelist to merge tree
			PdfMergeTree mergeTree = new PdfMergeTree();
			mergeTree.Add();

			SendStatusMessage(mergeTree.ToString());

			PdfMergeFiles fileMerger = new PdfMergeFiles();

			SendStatusMessage("Merging Files");
			PdfDocument pdf = fileMerger.Merge(mergeTree);

			if (pdf == null)
			{
				SendStatusMessage("pdf NOT made / failed - closed");

				File.Delete(PdfMergeMgr.Instance.OutputPathAndFile);

				return false;
			}

			SendStatusMessage("pdf made and closed");

			fileMerger.listOutline(pdf, pdf.GetOutlines(false));

			pdf.Close();
			return true;
		}

		private void ListSettings()
		{
			List<string> message = new List<string>();

			message.Add(logMsgDbS("Merging", fileMgr.ItemCount + " PDF files"));
			message.Add(nl);
			message.Add(logMsgDbS("Output File", OutputPathAndFile));
			message.Add(nl);
			message.Add(logMsgDbS("Folder Mgr init", sfMgr.Initialized.ToString()));
			message.Add(nl);
			message.Add(logMsgDbS("File Mgr Init", fileMgr.Initialized.ToString()));
			message.Add(nl);
			message.Add(logMsgDbS("Setting", "Overwriting of an existing output file is OK| " + 
				UserSettings.Data.AllowOverwriteOutputFile));
			message.Add(nl);

			SendSettingMessage(message);

		}

		private void SendStatusMessage(string message)
		{
			Program.win.AppendStatusMessage(new []{message});
		}

		private void SendStatusMessage(List<string> message)
		{
			Program.win.AppendStatusMessage(message.ToArray());
		}

		private void SendSettingMessage(string message)
		{
			Program.win.AppendSettingMessage(new []{message});
		}

		private void SendSettingMessage(List<string> message)
		{
			Program.win.AppendSettingMessage(message.ToArray());
		}


	}
}
