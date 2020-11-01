using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.MergeSupport;
using AndyShared.SampleFileSupport;
using AndyShared.Support;
using SettingsManager;
using UtilityLibrary;

namespace ClassifierEditor.Windows
{
	/// <summary>
	/// Interaction logic for WindowClassifyTest.xaml
	/// </summary>
	public partial class WindowClassifyTest : Window, INotifyPropertyChanged
	{
		private bool displayDebugMsgs = false;


		private string[] TreeViewTitleList = new [] {"Initial BookMark Tree", "Final BookMark Tree"};


	#region private fields

		private int treeViewTitleIndex = 0;

		private Classify classify;

		private ClassificationFile classificationFile;

		// the list of files to categorize;
		private SheetFileList testFileList;

		private Orator.ConfRoom.Announcer announcer;

		private bool isConfigured;
		
		private bool isExpanded;

		private string tbx1Message;

		// private string phaseBldg;

		// private FileNameSheetPdf fp;
		// private static TreeNode userSelected;

	#endregion

	#region ctor

		public WindowClassifyTest()
		{
			InitializeComponent();

			isConfigured = false;

			// TreeViewTitleIndex = 0;
		}

	#endregion

	#region public properties


		public bool ShowNonApplicableFiles => (classify?.NonApplicableFilesTotalCount?? 0) > 0;

		public string NonApplicableFilesDescription
		{
			get
			{
				return classify == null ? null : 
					classify.NonApplicableFilesTotalCount.ToString("###0") + " Non-applicable Files";
			}
		}

		public string TestFileDescription => testFileList?.Description ?? "Unknown";

		public SheetFileList TestFileList => testFileList;

		public ClassificationFile ClassificationFile
		{
			get => classificationFile;

			private set
			{
				classificationFile = value;

				OnPropertyChange();
				OnPropertyChange("BaseOfTree");
			}
		}

		public BaseOfTree BaseOfTree
		{
			get => classificationFile?.TreeBase;
		}

		public Classify Classify => classify;

		// public Dictionary<string, List<FilePath<FileNameSheetPdf>>> NonApplicableFiles => classify?.NonApplicableFiles;

		public string TreeBaseTitle
		{
			get
			{
				if (testFileList == null) return "No Title Yet";
		
				string result = "";
		
				if (!testFileList.Building.IsVoid())
				{
					result = "Building " + testFileList.Building;
				}
				else
				{
					result = "Base of Tree";
				}
		
				if ((BaseOfTree?.Item.MergeItemCount ?? 0) > 0)
				{
					result += " / Un-categorized Sheet Files";
				}
		
				return result;
			}
		}
		
		public string TreeViewTitle
		{
			get
			{
				return TreeViewTitleList[treeViewTitleIndex];
			}
		}
		
		public int TreeViewTitleIndex
		{
			get => treeViewTitleIndex;
			set
			{
				treeViewTitleIndex = value;
				OnPropertyChange();
				OnPropertyChange("TreeViewTitle");
			}
		}

		public string PhaseBuilding => testFileList?.Building;

		public string IsConfigured => isConfigured.ToString();

		public bool IsExpandedEx
		{
			get => isExpanded;
			private set
			{
				isExpanded = value;
				OnPropertyChange();
			}
		}

		public string Tbx1Message
		{
			get => tbx1Message;

			private set
			{
				tbx1Message = value;

				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Configure(ClassificationFile classfFile)
		{
			isConfigured = false;

			if (classfFile != null && classfFile.TreeBase.HasChildren)
			{
				classificationFile = classfFile;

				classificationFile.Initialize();

				initFileList();

				isConfigured = true;

				updateProperties();
			}

			return isConfigured;
		}

	#endregion

	#region private methods

		private void updateProperties()
		{
			OnPropertyChange("TestFileList");
			OnPropertyChange("TestFileDescription");
			OnPropertyChange("ClassificationFile");
			OnPropertyChange("BaseOfTree");
			OnPropertyChange("PhaseBuilding");
			OnPropertyChange("IsConfigured");
		}



		private void initFileList()
		{
			testFileList = new SheetFileList();
			testFileList.ReadSampleSheetFileList(classificationFile.SampleFilePath);
		}

		private void go()
		{
			// Tbx1Message = "*** Classification Started ***\n";

			if (!classify.Configure(BaseOfTree, TestFileList)) return;

			// await Task.Run(() => { result = classify.Process(); });

			classify.Process();

			OnPropertyChange("Classify");
			OnPropertyChange("ShowNonApplicableFiles");
			OnPropertyChange("NonApplicableFilesDescription");

			// int c = BaseOfTree.CountExtItems();

			// Tbx1Message += "*** Classification Complete ***\n";
			//
			// BaseOfTree b = BaseOfTree;
			//
			// Tbx1Message += classify.FormatMergeList(BaseOfTree);
		}

	#endregion

	#region event consuming

		private void WinClassfTest_Closing(object sender, CancelEventArgs e)
		{
			UserSettings.Data.WinClassifyPos.X = (int) this.Left;
			UserSettings.Data.WinClassifyPos.Y = (int) this.Top;
		}

		private void WinClassfTest_Loaded(object sender, RoutedEventArgs e)
		{
			announcer = Orator.GetAnnouncer(this, "toClassify");

			classify = new Classify();
			// classify.OnFileChange += Classify_OnFileChange;
			// classify.OnTreeNodeChange += Classify_OnTreeNodeChange;

			Orator.Listen("fromClassify", OnGetAnnouncement);

			// tell classify to display debug messages
			announcer.Announce(displayDebugMsgs);
		}

		private void WinClassfTest_Initialized(object sender, EventArgs e)
		{
			this.Top = UserSettings.Data.WinClassifyPos.Y;
			this.Left = UserSettings.Data.WinClassifyPos.X;
		}

		private void OnGetAnnouncement(object sender, object value)
		{
			if (displayDebugMsgs)
			{
				if (value is string)
				{
					Tbx1Message += (string) value;
				}
			}
		}

		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			go();

			IsExpandedEx = true;

			TreeViewTitleIndex = 1;

			OnPropertyChange("TreeBaseTitle");

			// updateProperties();
			//
			// ClassificationFile.UpdateProperties();
			// BaseOfTree.UpdateProperties();
			// BaseOfTree.Item.UpdateProperties();
			// BaseOfTree.Item.UpdateMergeProperties();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{

			// BaseOfTree b = this.BaseOfTree;
			//
			this.Visibility = Visibility.Collapsed;

			Close();
		}

		private void BtnExpand_OnClick(object sender, RoutedEventArgs e)
		{
			IsExpandedEx = true;
		}


		// private void Classify_OnFileChange(object sender, FileChangeEventArgs e)
		// {
		// 	Tbx1Message += "classify / file changed| " + e.SheetFile.FileNameObject.SheetNumber + "\n";
		// }
		//
		// private void Classify_OnTreeNodeChange(object sender, TreeNodeChangeEventArgs e)
		// {
		// 	Tbx1Message += "classify / node changed| " + e.TreeNode.Item.Title + "\n";
		// }

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is Class1";
		}


		#endregion


			
	}
}