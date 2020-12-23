using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
	public enum Exp_Collapse_State
	{
		EXP_ALL,		// exp tree & show listviews
		EXP_TREE,		// exp tree & hide listviews
		COLLAPSE_ALL	// collapse all

	}

	/// <summary>
	/// Interaction logic for WindowClassifyTest.xaml
	/// </summary>
	public partial class WindowClassifyTest : Window, INotifyPropertyChanged
	{
		private bool displayDebugMsgs = false;

	#region private fields

		private string[] TreeViewTitleList = new [] {"Initial BookMark Tree", "Final BookMark Tree"};
		private string[] expCollapseStateList = new [] {"Collapse to Tree", "Collapse All", "Expand All"};

		private int treeViewTitleIndex = 0;

		private Classify classify;

		private ClassificationFile classificationFile;

		// the list of files to categorize;
		private SheetFileList testFileList;

		private Orator.ConfRoom.Announcer announcer;

		private bool isConfigured;
		
		private bool isExpandedEx;

		private string tbx1Message;

		private Exp_Collapse_State expCollapseState = Exp_Collapse_State.EXP_TREE;

		private double pb1Value;
		private double pb1MaxValue = 0;
		private Progress<double> pb1ProgressValue;

	#endregion

	#region ctor

		public WindowClassifyTest()
		{
			InitializeComponent();

			isConfigured = false;

			pb1ProgressValue = new Progress<double>(value => Pb1Value = value);
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

		public string ExpCollapseState => expCollapseStateList[(int) expCollapseState];

		public string IsConfigured => isConfigured.ToString();

		public string Tbx1Message
		{
			get => tbx1Message;

			private set
			{
				tbx1Message = value;

				OnPropertyChange();
			}
		}

		public double Pb1MaximumValue
		{
			get => pb1MaxValue;
			set
			{
				pb1MaxValue = value;
				OnPropertyChange();
			}
		}

		public double Pb1Value
		{
			get => pb1Value;
			set
			{
				pb1Value = value;

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

		private void updateClassifyProperties()
		{
			OnPropertyChange("Classify");
			OnPropertyChange("BaseOfTree");
			OnPropertyChange("TreeBaseTitle");
			OnPropertyChange("ExpCollapseState");
			OnPropertyChange("ShowNonApplicableFiles");
			OnPropertyChange("NonApplicableFilesDescription");
		}

		private void initFileList()
		{
			testFileList = new SheetFileList();
			testFileList.ReadSampleSheetFileList(classificationFile.SampleFilePath);
		}

		private async void go()
		{
			BaseOfTree.Item.Description = "TreeBase from ClassifyTest";

			classify = new Classify();

			if (!classify.Configure(BaseOfTree, TestFileList)) return;

			classify.ConfigureAsyncReporting(pb1ProgressValue);

			// todo: fix run() => classify.process()
			// await Task.Run(() => { classify.Process(); });

			// classify.Process();

			expCollapseState = Exp_Collapse_State.EXP_ALL;

			ExpandCollapseTree(BaseOfTree);

			TreeViewTitleIndex = 1;

			updateClassifyProperties();

			Debug.WriteLine("@classify-go / 1| ext merge item count| " + BaseOfTree.ExtMergeItemCountCurrent);

		}

		private void ExpandCollapseTree(TreeNode node)
		{
			ExpCollapseNode(node);

			expandCollapseTree(node);
		}

		private void expandCollapseTree(TreeNode parent)
		{
			foreach (TreeNode child in parent.ChildrenView)
			{
				if (child.HasChildren) ExpandCollapseTree(child);

				ExpCollapseNode(child);
			}
		}

		private void ExpCollapseNode(TreeNode node)
		{
			if (expCollapseState == Exp_Collapse_State.COLLAPSE_ALL)
			{
				node.IsExpandedAlt = false;
			}
			else
			{
				node.IsExpandedAlt = true;
			}

			if (node.Item.HasMergeItems)
			{
				if (expCollapseState == Exp_Collapse_State.EXP_ALL)
				{
					node.Item.IsVisible = true;
				}
				else
				{
					node.Item.IsVisible = false;
				}

			}
		}

		private void adjustExpCollapseState()
		{
			if (expCollapseState == Exp_Collapse_State.COLLAPSE_ALL)
			{
				expCollapseState = Exp_Collapse_State.EXP_ALL;
			}
			else
			{
				expCollapseState = (Exp_Collapse_State) (1 + (int) expCollapseState);
			}

			OnPropertyChange("ExpCollapseState");
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
			// Debug.WriteLine("@classify-test / 1| ext merge item count| " + BaseOfTree.ExtMergeItemCountCurrent);

			// await Task.Run(() => { go(); });
			go();

			// Debug.WriteLine("@classify-test / 2| ext merge item count| " + BaseOfTree.ExtMergeItemCountCurrent);

			// IsExpandedEx = true;

			// expCollapseState = Exp_Collapse_State.EXP_ALL;
			//
			// ExpandCollapse(BaseOfTree);
			//
			// TreeViewTitleIndex = 1;
			//
			// OnPropertyChange("TreeBaseTitle");
			// OnPropertyChange("BaseOfTree");

			// Debug.WriteLine("@classify-test / 3| ext merge item count| " + BaseOfTree.ExtMergeItemCountCurrent);

		}
		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			// Debug.WriteLine("@classify-test / 11| ext merge item count| " + BaseOfTree.ExtMergeItemCountCurrent);

			OnPropertyChange("BaseOfTree");

			Debug.WriteLine("@debug");
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
			// IsExpandedEx = !isExpandedEx;

			adjustExpCollapseState();

			ExpandCollapseTree(BaseOfTree);
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