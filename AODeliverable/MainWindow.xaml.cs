using System.Windows;
using System.Windows.Documents;
using AODeliverable.FileSelection;
using AODeliverable.PdfMerge;
using static UtilityLibrary.MessageUtilities;

namespace AODeliverable
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private SelectFolderMgr sfMgr = null;
		private SelectFilesMgr filesMgr = null;
		private  PdfMergeMgr pdfMergeMgr = null;

		public MainWindow()
		{
			InitializeComponent();

			sfMgr = SelectFolderMgr.Instance;
			filesMgr = SelectFilesMgr.Instance;
			pdfMergeMgr = PdfMergeMgr.Instance;
		}

//		public void AppendStatusText(string text)
//		{
//			rtbStatus.AppendText(text);
//		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			tbxInFolder.Text = sfMgr.BaseFolder;
			tbxOutFolder.Text = sfMgr.LeftFolder(sfMgr.FolderCount - 1);
			tbxOutFile.Text = "2019-01-01 PDF Collection";

			ListFilesInStatus();

			pdfMergeMgr.OutputPathAndFile = tbxOutFolder.Text + @"\" + tbxOutFile.Text;

		}

		private void ListFilesInStatus()
		{
			rtbStatus.Document.Blocks.Clear();

			if (filesMgr.Initialized)
			{
				string[] message = new string[3];

				foreach (FileItem fi in filesMgr)
				{
					message[0] = logMsgDbS("Including", fi.getName() + nl);
					message[1] = logMsgDbS("Outline Path", "\t" + fi.outlinePath + nl);
					message[2] = nl;

					AppendStatusMessage(message);
				}
			}
		}

		public void AppendStatusMessage(string[] message)
		{
			AltColumn = 20;

			Paragraph p = new Paragraph();

			foreach (string s in message)
			{
				p.Inlines.Add(s);
			}

			rtbStatus.Document.Blocks.Add(p);

			AltColumn = -1;
		}

		public void AppendSettingMessage(string[] message)
		{
			AltColumn = 12;

			Paragraph p = new Paragraph();

			foreach (string s in message)
			{
				p.Inlines.Add(s);
			}

			rtbSettings.Document.Blocks.Add(p);

			AltColumn = -1;
		}

		private void btnGo_Click(object sender, RoutedEventArgs e)
		{
			// proceed with creating the PDF

			// at this point, just need to merge

			pdfMergeMgr.Process();
		}

		private void BtnSelFolder_Click(object sender, RoutedEventArgs e)
		{
			Program.process();
		}
	}
}
