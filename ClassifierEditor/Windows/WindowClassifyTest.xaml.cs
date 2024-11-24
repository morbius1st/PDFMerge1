using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AndyShared.ClassificationDataSupport.SheetSupport;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.MergeSupport;
using AndyShared.SampleFileSupport;
using AndyShared.Support;
using DebugCode;
using SettingsManager;
using UtilityLibrary;

namespace ClassifierEditor.Windows
{
	public enum Exp_Collapse_State
	{
		EXP_TREE,     // exp tree & hide listviews
		COLLAPSE_ALL, // collapse all
		EXP_ALL,      // exp tree & show listviews
		COUNT
	}

	/// <summary>
	/// Interaction logic for WindowClassifyTest.xaml
	/// </summary>
	public partial class WindowClassifyTest : Window, INotifyPropertyChanged
	{
		private bool displayDebugMsgs = false;

	#region private fields

		private string[] TreeViewTitleList = new [] { "Initial BookMark Tree", "Final BookMark Tree" };
		private string[] expCollapseStateList = new [] { "Collapse to Tree ", "Collapse All ", "Expand All" };

		private int treeViewTitleIndex = 0;

		private Classify classify;

		private ClassificationFile classificationFile;

		// the list of files to categorize;
		private SheetFileList testFileList;

		// private Orator.ConfRoom.Announcer announcerx;

		private bool isConfigured;

		// private bool isExpandedEx;

		private string tbx1Message = null;

		private Exp_Collapse_State expCollapseStateIdx ; // = Exp_Collapse_State.EXP_TREE;

		// private double pb1Value;
		private double pb1MaxValue = 0;
		private Progress<double> pb1ProgressValue;

	#endregion

	#region ctor

		public WindowClassifyTest()
		{
			InitializeComponent();

			isConfigured = false;

			pb1ProgressValue = new Progress<double>(value => Pb1.Value = value);
		}

	#endregion

	#region public properties

		public bool ShowNonApplicableFiles => (classify?.NonApplicableFilesTotalCount ?? 0) > 0;

		public string NonApplicableFilesDescription
		{
			get
			{
				return classify == null
					? null
					: "Found (" + classify.NonApplicableFilesTotalCount.ToString("###0") + ") Non-applicable Files";
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
			get { return TreeViewTitleList[treeViewTitleIndex]; }
		}

		public int TreeViewTitleIndex
		{
			get => treeViewTitleIndex;
			set
			{
				treeViewTitleIndex = value;
				OnPropertyChange();
				OnPropertyChange(nameof(TreeViewTitle));
			}
		}

		public string PhaseBuilding => testFileList?.Building;

		public string ExpCollapseState => expCollapseStateList[(int) expCollapseStateIdx];

		public Exp_Collapse_State ExpCollapseStateIdx
		{
			get => expCollapseStateIdx;
			set
			{
				expCollapseStateIdx = value;
				OnPropertyChange();
				OnPropertyChange(nameof(ExpCollapseState));
			}
		}

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
			// get => pb1Value;
			set
			{
				// pb1Value = value;

				// OnPropertyChanged();

				((IProgress<double>) pb1ProgressValue).Report(value);
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Configure(ClassificationFile classfFile)
		{
		#if DML1
			DM.Start0();
		#endif

			isConfigured = false;

			if (classfFile != null && classfFile.TreeBase.HasChildren)
			{
				classificationFile = classfFile;

				classificationFile.Initialize();

				initFileList();

				isConfigured = true;

				updateProperties();
			}

		#if DML1
			DM.End0();
		#endif

			return isConfigured;
		}

	#endregion

	#region private methods

		private void updateProperties()
		{
			OnPropertyChange(nameof(BaseOfTree));
			OnPropertyChange(nameof(TestFileList));
			OnPropertyChange(nameof(TestFileDescription));
			OnPropertyChange(nameof(ClassificationFile));
			OnPropertyChange(nameof(PhaseBuilding));
			OnPropertyChange(nameof(IsConfigured));
		}

		private void updateClassifyProperties()
		{
			OnPropertyChange(nameof(Classify));
			OnPropertyChange(nameof(BaseOfTree));
			OnPropertyChange(nameof(TreeBaseTitle));
			OnPropertyChange(nameof(ShowNonApplicableFiles));
			OnPropertyChange(nameof(NonApplicableFilesDescription));
		}

		private void initFileList()
		{
			testFileList = new SheetFileList();
			testFileList.ReadSampleSheetFileList(classificationFile.SampleFilePath);
		}

		private void go()
		{
			Pb1Value = 0;
			Pb1MaximumValue = testFileList.Files.Count;

			BaseOfTree.Item.Description = "TreeBase from ClassifyTest";

			if (!classify.Configure(BaseOfTree, TestFileList)) return;

			classify.ConfigureAsyncReporting(pb1ProgressValue);

			classify.PreProcess();

			// Debug.WriteLine("start process3");

			classify.Process3();

			// Debug.WriteLine("after process3");
			// Debug.WriteLine("after process3 - start post process");

			// classify.postProcessMergeLists();
			//
			// Debug.WriteLine("after post process");

			OnPropertyChange(nameof(Classify));
		}

		private void ExpandCollapseTree(TreeNode node)
		{
			expCollapseNode(node);

			expandCollapseTree(node);

			adjustExpCollapseState();
		}

		private void expandCollapseTree(TreeNode parent)
		{
			foreach (TreeNode child in parent.ChildrenView)
			{
				if (child.HasChildren) expandCollapseTree(child);

				expCollapseNode(child);
			}
		}

		private void expCollapseNode(TreeNode node)
		{
			if (expCollapseStateIdx == Exp_Collapse_State.COLLAPSE_ALL)
			{
				node.IsExpandedAlt = false;
			}
			else
			{
				node.IsExpandedAlt = true;
			}

			if (node.Item.HasMergeItems)
			{
				if (expCollapseStateIdx == Exp_Collapse_State.EXP_ALL)
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
			int e = ((int) expCollapseStateIdx) + 1;

			if (e >= (int) Exp_Collapse_State.COUNT )
			{
				e = TreeViewTitleIndex == 0 ? 1 : 0;
			}

			ExpCollapseStateIdx = (Exp_Collapse_State) e;
		}

		private int depth = 0;

		private void listTree(TreeNode node)
		{
			foreach (TreeNode child in node.Children)
			{
				if (child.Item.ItemClass == Item_Class.IC_BOOKMARK)
				{
					Tbx1Message += $"{"  ".Repeat(depth)}{child.Item.Title} ({child.Item.Description})\n";
				}

				if (child.Item.HasMergeItems)
				{
					listMergeItems(child.Item.MergeItems);
				} 

				if (child.HasChildren)
				{
					depth++;
					listTree(child);
					depth--;
				}
			}

		}

		private void listMergeItems(ObservableCollection<MergeItem> mItems)
		{
			foreach (MergeItem mi in mItems)
			{
				Tbx1Message += $"{"  ".Repeat(depth+1)}{mi.FilePath.FileNameObject.SheetNumber,-14} {mi.FilePath.FileNameObject.SheetName}\n";
			}
		}

		private void listMergeItems(ListCollectionView view)
		{
			foreach (MergeItem mi in view)
			{
				Tbx1Message += $"{"  ".Repeat(depth+1)}{mi.FilePath.FileNameObject.SheetNumber,-14} {mi.FilePath.FileNameObject.SheetName} (comparisons {mi.CompareCount})\n";
			}
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
			Pb1Value = 0;
			Pb1MaximumValue = 100;
			ExpCollapseStateIdx = Exp_Collapse_State.EXP_ALL;

			// announcer = Orator.GetAnnouncer(this, "toClassify");

			classify = new Classify();
			classify.OnClassifyCompletion += Classify_OnClassifyCompletion;
			// classify.OnFileChange += Classify_OnFileChange;
			// classify.OnTreeNodeChange += Classify_OnTreeNodeChange;

			// Orator.Listen("fromClassify", OnGetAnnouncement);

			// tell classify to display debug messages
			// announcer.Announce(displayDebugMsgs);
		}

		private void WinClassfTest_Initialized(object sender, EventArgs e)
		{
			this.Top = UserSettings.Data.WinClassifyPos.Y;
			this.Left = UserSettings.Data.WinClassifyPos.X;
		}

		// private void OnGetAnnouncement(object sender, object value)
		// {
		// 	if (displayDebugMsgs)
		// 	{
		// 		if (value is string)
		// 		{
		// 			Tbx1Message += (string) value;
		// 		}
		// 	}
		// }

		// buttons

		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			go();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			// Debug.WriteLine("@classify-test / 11| ext merge item count| " + BaseOfTree.ExtMergeItemCountCurrent);

			OnPropertyChange("BaseOfTree");

			Classify c = classify;

			Debug.WriteLine("@debug");
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			// BaseOfTree b = this.BaseOfTree;
			//
			this.Visibility = Visibility.Collapsed;

		#if DML1
			DM.Start0(true);
		#endif
		#if DML1
			DM.End0();
		#endif

			Close();
		}

		private void BtnExpand_OnClick(object sender, RoutedEventArgs e)
		{
			// IsExpandedEx = !isExpandedEx;

			ExpandCollapseTree(BaseOfTree);

			// adjustExpCollapseState();
		}

		private void BtnReport_OnClick(object sender, RoutedEventArgs e)
		{
			depth = 0;

			Tbx1Message += $"{"  ".Repeat(depth)}{BaseOfTree.Item.Title} ({BaseOfTree.Item.Description})\n";

			listTree(BaseOfTree);

			Tbx1Message += $"\n{"  ".Repeat(depth)}total of {classify.CompareCountTotal} comparisons\n";
		}


		// other

		private void Classify_OnClassifyCompletion(object sender)
		{
			if (classify.Status != ClassifyStatus.SUCESSFUL) return;

			BaseOfTree.CountExtMergeItems();

			BaseOfTree.UpdateProperties();

			TreeViewTitleIndex = 1;

			ExpCollapseStateIdx = Exp_Collapse_State.EXP_ALL;

			ExpandCollapseTree(BaseOfTree);

			updateClassifyProperties();
		}

		private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				MouseWheelEventArgs eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
				eventArg.RoutedEvent = UIElement.MouseWheelEvent;
				eventArg.Source = sender;
				var parent = ((Control)sender).Parent as UIElement;
				parent.RaiseEvent(eventArg);
			}
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