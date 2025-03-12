// #define SHOWTICKS
// #define DML1

#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using AndyShared.ClassificationDataSupport.SheetSupport;
using ClassifierEditor.SampleData;
using SettingsManager;
using UtilityLibrary;
using AndyShared.ClassificationFileSupport;
using AndyShared.SampleFileSupport;
using AndyShared.Support;
using Microsoft.WindowsAPICodePack.Dialogs;
using static AndyShared.ClassificationDataSupport.TreeSupport.LogicalComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.ValueComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ConfigSupport;
using AndyShared.Settings;

using JetBrains.Annotations;
using WpfShared.Windows;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using System.Windows.Interop;

#endregion

// projname: ClassifierEditor
// itemname: MainWindowClassifierEditor
// username: jeffs
// created:  5/2/2020 9:16:20 AM

/*

inter-class communications

				announce		listen		procedure

when mainwindow tells classfFile to init:
classfFile		TN_INIT
all treenodes					TN_INIT		respond: none / set initialized = true / set ismodified = false
all sheetCategories				TN_INIT		respond: none / set initialized = true / set ismodified = false


at the below / at ismodified:
if (new & current values match) return, no update
if (isinitialized) announce modified


****
event: treeview item (treenode) selected / userselected set at mainwindow
											userselected edit controls populated



****
event: sheetcategory monitored item changed (e.g. description)
	userselected/sheetcategory/{control}		
												ismodified = true;
				MODIFIED							if (isinitialized) announce modified

classfFile						MODIFIED	if (initialized && ismodified != value) 
												ismodified = true
												


 
 *****
event: sheetcategory.CompareOps collection changed
	userselected/sheetcategory/CompareOpsOnCollectionChanged	
												ismodified = true;
													if (isinitialized) announce modified
				MODIFIED

classfFile						MODIFIED	if (initialized && ismodified != value) 
												ismodified = true



****
event: baseoftree / treenode monitored item changed (e.g. parent)	
												ismodified = true;
				MODIFIED							if (isinitialized) announce modified

classfFile						MODIFIED	if (initialized && ismodified != value) 
												ismodified = true
												

 *****
event: baseoftree / treenode collection changed
	event: ChildrenOnCollectionChanged fired	
												ismodified = true
													if (isinitialized) announce modified
				MODIFIED
classfFile						MODIFIED	if (initialized && ismodified != value) 
												ismodified = true

****
event: button saved pressed (classfFile.ismodified is true)

mainwindow										save data
				SAVED

classfFile							SAVED		ismodified = false
treenode/baseoftree					SAVED		ismodified = false
sheetcategory						SAVED		ismodified = false



****
needed:
mainwindow:		✔ (cancel) announce / CF_INIT (just do this directly)
				✔ announce / SAVED
				✔ listen {none}

classfFile		✔ (cancel) listen / CF_INIT (just do this directly)
				✔ announce / TN_INIT (maybe combine the above)
				✔ listen / MODIFIED 
				✔ listen / SAVED
				✔ add TreeNodeModified to correct properties

baseoftree/treenode
				✔ listen / TN_INT
				✔ listen / SAVED
				✔ announce / MODIFIED (from TreeNodeModified)
				✔ add TreeNodeModified to correct properties

sheetcategory
				✔ listen / TN_INIT
				✔ listen / SAVED
				✔ announce / MODIFIED (from TreeNodeModified)
				✔ add TreeNodeModified to correct properties

*/


namespace ClassifierEditor.Windows
{
	/*
	 data chain

		MainWindowClassifierEditor				(SheetCategoryDataManager  Categories)
		V
		SheetCategoryDataManager	(TreeNode TreeBase)
		V								  | ^
		StorageManager					  | +<- shtCategories [StorageManager<SheetCategories>]
		V								  |     +<- Data [SheetCategories]
		SheetCategories					  |         +<- TreeBase [TreeNode]
		V								  |
		TreeNode (TreeBase)				  |


		SheetCategoryDataManager
			- holds one or more storage managers
		^
		StorageManager (T Data <= DataStore.Data)
			- generic
			- holder of a DataStore 
			- configures a DataStore
		^
		setting manager (DataStore)
			- read & write / serialize / deserialize
			- actual holder of the data
				- one of the data pieces is TreeBase, the root node
		^
		datafile class (Data of type SheetCategories)
		^
		datafile.xml

	file usage

		stored: site settings 
			organization base files     | seed files to set up an initial organization configuration
										| needs name for identification / selection
										| needs to identify associated seed file
			sample file                 | file with folder / file name samples for testing

		stored: suite settings to allow other suite programs to use
			SiteRootPath				| the location of the site setting file
			Organization setting file	| the user's organization configuration
										| needs to have a name + user's name to allow identification

		stored: in app settings

		stored: in user's settings
			personal sample files		| ditto except personal

	example (in order)

	read suite:
		has site setting file location
		has all organization config files: "OrgConfigFiles"

	set site path
	read site
		has see files location

	read app
		? nothing

	read user
		has the name of the organization config file being used: "OrgConfigFileName"

	use "OrgConfigFileName" + user's name to get the 
		correct entry in "OrgConfigFiles" => "UserOrgConfigFile"

	configure "categories" with the "UserOrgConfigFile"

	initial setup:
	required:
	suite setting file in place with the site location set
	site setting file in place with the seed files set
	seed files in place




	start process:
		all empty / has list of organization base files / has list of organization setting files
		user selects a organization base file
		user provides location to save organization setting file


	*/


	/// <summary>
	/// Interaction logic for MainWindowClassifierEditor.xaml
	/// </summary>
	public partial class MainWindowClassifierEditor : Window, INotifyPropertyChanged, IWin
	{
		public static int objIdx = 0;

	#region public fields

		public string ContextCmdDelete { get; }            = "delete";
		public string ContextCmdAddChild { get; }          = "addChild";
		public string ContextCmdAddBefore { get; }         = "addBefore";
		public string ContextCmdAddAfter { get; }          = "addAfter";
		public string ContextCmdMoveAsChild { get; }       = "moveAsChild";
		public string ContextCmdMoveBefore { get; }        = "moveBefore";
		public string ContextCmdMoveAfter { get; }         = "moveAfter";
		public string ContextCmdCopy { get; }              = "clone";
		public string ContextCmdCopyAsChild { get; }       = "cloneAsChild";
		public string ContextCmdExpand { get; }            = "Expand Category";
		public string ContextCmdCollapse { get; }          = "Collapse Category";
		public string ContextCmdExpandAll { get; }         = "Expand All";
		public string ContextCmdCollapseAll { get; }       = "Collapse All";

	#endregion

		// public AndyShared.ClassificationDataSupport.TreeSupport.CheckedState bot;

	#if SHOWTICKS
		private long ticksStart;
		private long tickPrior;
		private long tickNow;
	#endif

	#region private fields

		public static string[] ContextMoveTitles { get; set; } = new []
		{
			"Cancel Move Operation", "Move Before this Category", "Move After this Category",
			"Move as Sub-Category to this Category"
		};


		// private SettingsMgr<UserSettingPath, UserSettingInfo<UserSettingDataFile>, 
		// 	UserSettingDataFile> us = UserSettings.Admin;

		private ClassificationFile classificationFile;

		public static MainWindowClassifierEditor Me { get; set; }
		
		private MwClassfEdSupport mws;

		// private Configuration config;
		// private TreeNode nodeSelected;
		// private TreeNode nodeCopy;
		private TreeNode nodeContextTarget;
		private TreeNode contextSelectedTargetParent;

		// private bool bypassContextDeHighlight = false;
		private string fileIdNew;
		private bool cancelEnabled;
		private bool saveEnabled;
		private TreeNode nodeContextSource;

		private int moveOp = 0;
		private bool cannotSelect;

		private bool expanderGotMouse = false;

		private bool justSelected = false;

		// private Orator.ConfRoom.Announcer OnCfInitAnnouncer;
		// private Orator.ConfRoom.Announcer OnSavedAnnouncerx;
		// private Orator.ConfRoom.Announcer OnTnInitAnnouncerx;
		// private Orator.ConfRoom.Announcer OnRemExCollapseStateAnnouncerx;
		// private Orator.ConfRoom.Announcer OnSavingAnnouncerx;

	#endregion

	#region ctor

		public MainWindowClassifierEditor()
		{
		#if SHOWTICKS
			tickNow = System.DateTime.Now.Ticks;
			ticksStart = tickNow;
			tickPrior = tickNow;

			showTicks("start @1");
		#endif
			Me = this;

			ItemClassDef.init();

			mws = new MwClassfEdSupport(this);

			InitializeComponent();

			WinHandle = ScreenParameters.GetWindowHandle(Common.GetCurrentWindow());

			DM.init(5, this);

			DM.DbxSetIdx(0, 0);
			DM.DbxSetDefaultWhere(0, ShowWhere.DEBUG);

		}

	#endregion

	#region public properties

		public static IntPtr WinHandle { get; private set; }

		public ClassificationFile ClassificationFile
		{
			get => classificationFile;
			private set
			{
				if (value == null) return;

			#if DML1
				DM.Start0("(property)", true, "at mainwin property");
			#endif

				classificationFile = value;

				// if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ mainwin|@ onload| initialize classFfile");

				classificationFile.Initialize2();

				FileList = new SheetFileList();
				FileList.ReadSampleSheetFileList(classificationFile.SampleFilePath);

				string desc = classificationFile.HeaderDescFromMemory;

				UpdateProperties();
				OnPropertyChanged(nameof(FileList));

			#if DML1
				DM.End0("(property)", true);
			#endif
			}
		}

		public BaseOfTree BaseOfTree
		{
			get => classificationFile?.TreeBase ?? null;
		}

		// public TreeNode NodeSelected
		// {
		// 	get => nodeSelected;
		// 	set
		// 	{
		// 		ContextDeHighlight();
		//
		// 		nodeSelected = value;
		//
		// 		OnPropertyChanged();
		// 		OnPropertyChanged(nameof(HasSelection));
		// 		OnPropertyChanged(nameof(ClassificationFile));
		//
		//
		// 		Lv2ConditionTemplateSelector.MasterIdIdx = 0;
		// 	}
		// }

		public TreeNode NodeContextTarget
		{
			get => nodeContextTarget;
			private set
			{
				if (value == nodeContextTarget) return;

				if (value == null)
				{
					nodeContextTarget.IsContextTarget = false;
					// nodeContextTarget.IsContextSource = false;
					nodeContextTarget = null;
				}
				else
				{
					nodeContextTarget = value;
					nodeContextTarget.IsContextTarget = true;
				}

				OnPropertyChanged();
				OnPropertyChanged(nameof(HasContextSelection));
			}
		}

		public TreeNode NodeContextSource
		{
			get => nodeContextSource;
			set
			{
				if (Equals(value, nodeContextSource)) return;


				if (value == null)
				{
					nodeContextSource.IsContextTarget = false;
					nodeContextSource = null;
				}
				else
				{
					nodeContextSource = value;
					nodeContextSource.IsContextSource = true;
				}

				OnPropertyChanged();
				OnPropertyChanged(nameof(HasContextSelection));
			}
		}

		public SheetFileList FileList { get; private set; } = new SheetFileList();

		public MwClassfEdSupport Mws => mws;

		public string FileIdNew
		{
			get => fileIdNew;
			set
			{
				if (value == fileIdNew) return;
				fileIdNew = value;
				OnPropertyChanged();
			}
		}

		// status

		public string MoveTitle => ContextMoveTitles[MoveOp];

		public int MoveOp
		{
			get => moveOp;
			set
			{
				if (value == moveOp) return;
				moveOp = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(MoveTitle));
			}
		}

		public bool HasEditingChanges => mws.NodeEditing?.TreeNodeChildItemModified ?? false;

		public bool HasSelection => mws.NodeSelected != null;

		public bool HasContextSelection => nodeContextTarget != null;

		public bool HasContextSourceSel => nodeContextSource != null;

		public bool UserIsAdmin => Security.Instance.UserIsAdministrator;

		public bool CannotSelect
		{
			get => cannotSelect;
			set
			{
				if (value == cannotSelect) return;
				cannotSelect = value;
				OnPropertyChanged();
			}
		}

		public string Messages
		{
			get => messages;
			set
			{
				if (value == messages) return;
				messages = value;
				OnPropertyChanged();
			}
		}

		public void DebugMsgLine(string msg)
		{
			Messages += msg + "\n";
		}
		public void DebugMsg(string msg)
		{
			Messages += msg;

		}

	#region public property settings

		public bool RememberCollapseState
		{
			get => UserSettings.Data.RememberNodeExpandState;
			private set
			{
				UserSettings.Data.RememberNodeExpandState = value;
				UserSettings.Admin.Write();
				OnPropertyChanged();
			}
		}

		// public bool CancelReady { get; set; }


		// public bool CanSave
		// {
		// 	get
		// 	{
		// 		if (classificationFile == null ||
		// 			!classificationFile.IsValid ||
		// 			!classificationFile.IsInitialized) return false;
		//
		// 		bool a = nodeSelected == null;
		// 		bool b = classificationFile.TreeNodeModified;
		//
		// 		return b && a;
		// 	}
		// }

		/*
		cansave conditions

		1. userselected == null && classfFile.ismodified == true
		 * button: enabled
		 * tool tip: Save Changes
		2.userselected != null && classfFile.ismodified == true
		 * button: disabled
		 * tool tip: Finish Editing the Selected Category First
		3.userselected == null && classfFile.ismodified == false
		 * button: disabled
		 * tool tip: No Changes Yet
		4.userselected != null && classfFile.ismodified == false
		 * button: disabled
		 * tool tip: No Changes Yet
		*/

	#endregion

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void initSettings()
		{

		#if SHOWTICKS
			showTicks("setg @1");
		#endif

			DM.Suspend = true;

			UserSettings.Admin.Read();

		#if SHOWTICKS
			showTicks("setg @2");
		#endif

			SettingsMgr<UserSettingPath, UserSettingInfo<UserSettingDataFile>, UserSettingDataFile> i =
				UserSettings.Admin;

			UserSettingInfo<UserSettingDataFile> u = UserSettings.Info;

			UserSettingDataFile ud = UserSettings.Data;

		#if SHOWTICKS
			showTicks("setg @5");
		#endif

			OnPropertyChanged("RememberCollapseState");

		#if SHOWTICKS
			showTicks("setg @6");
		#endif

			UserSettings.Admin.Write();

		#if SHOWTICKS
			showTicks("setg @7");
		#endif

			MachSettings.Admin.Read();

		#if SHOWTICKS
			showTicks("setg @8");
		#endif

			MachSettings.Admin.Write();

		#if SHOWTICKS
			showTicks("setg @9");
		#endif

			SiteSettings.Admin.Read();
			SiteSettings.Admin.Write();

			DM.Suspend = false;

			string s = SiteSettings.Data.AdminUsers[0];

			string un = CsUtilities.UserName;
		}

		private void UpdateProperties()

		{
			OnPropertyChanged("ClassificationFile");
			OnPropertyChanged("BaseOfTree");
			OnPropertyChanged("NodeContextTarget");
			OnPropertyChanged("HasSelection");
			OnPropertyChanged("FileList");
		}

		public void UpdateProperties2()

		{
			OnPropertyChanged(nameof(Me));
			OnPropertyChanged(nameof(Mws));
			OnPropertyChanged(nameof(HasSelection));
			OnPropertyChanged(nameof(ClassificationFile));
		}

		private void OpenClassifFile(string fileidnew)
		{
			ClassificationFile test = ClassificationFile.Open(fileidnew);

			if (test != null)
			{
				ClassificationFile = test;
				OnPropertyChanged(nameof(ClassificationFile));
			}
		}

	#endregion

	#region window event methods

		private void Window_Initialized(object sender, EventArgs e)
		{
			DM.init(5, this);

			DM.DbxSetIdx(0, 0);
			DM.DbxSetDefaultWhere(0, ShowWhere.DBG_TBX);


		#if DML1
			DM.Start0(true);
		#endif

		#if SHOWTICKS
			showTicks("init @1");
		#endif

			initSettings();

			this.Top = UserSettings.Data.MainWinPos.Y;
			this.Left = UserSettings.Data.MainWinPos.X;

		#if SHOWTICKS
			showTicks("init @2");
		#endif

		#if DML1
			DM.End0();
		#endif
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
		#if DML1
			DM.Start0(true);
		#endif

		#if SHOWTICKS
			showTicks("load @1");
		#endif
			OnPropertyChanged(nameof(UserIsAdmin));


			// SampleData.SampleData sd = SampleData.SampleData.sd;

			// true to create sample data and save to disk
			// false to read existing data
			if (false)
			{
				// temp code to create a new classification file 
				classificationFile = ClassificationFile.GetUserClassfFile("PdfSample 1A");

				UpdateProperties();

				classificationFile.Initialize();

				if (!classificationFile.FilePathLocal.IsFound)
				{
					SampleData.SampleData.Sample(classificationFile.Data.BaseOfTree);
					classificationFile.Write();
				}
			}
			else
			{
				if (false)
				{
				#if SHOWTICKS
					showTicks("load @3");
				#endif

					ClassificationFile = ClassificationFile.GetUserClassfFile("PdfSample 1");

				#if SHOWTICKS
									showTicks("load @3.1");
				#endif

					WindowClassifyTest winTest = new WindowClassifyTest();

				#if SHOWTICKS
					showTicks("load @3.2");
				#endif

					bool init = winTest.Configure(ClassificationFile);

				#if SHOWTICKS
					showTicks("load @3.3");
				#endif

					if (init) winTest.ShowDialog();
				}
				else
				{
				#if SHOWTICKS
					showTicks("load @4");
				#endif

					UserSettingDataFile a = UserSettings.Data;

					// Debug.WriteLine("@MainWin load| load user settings");
					string fileId = UserSettings.Data.LastClassificationFileId;

					// Debug.WriteLine("@MainWin load| announce remember");
					// inform all of the current setting
					// OnRemExCollapseStateAnnouncer.Announce(UserSettings.Data.RememberNodeExpandState);

					TreeNode.RememberExpCollapseState = UserSettings.Data.RememberNodeExpandState;


					if (true)
					{
						// Debug.WriteLine("@MainWin load| load classification file");
						ClassificationFile = ClassificationFile.GetUserClassfFile(fileId);
					}
					else
					{
						// comment the above and un-comment this to open a specific file
						// ClassificationFile = ClassificationFile.GetUserClassfFile("Pdf Test 1");
						// ClassificationFile = ClassificationFile.GetUserClassfFile("Pdf Test 2");
						ClassificationFile = ClassificationFile.GetUserClassfFile("Pdf Test 3");
					}
				}
			}

			// if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ mainwin|@ onload| cancel all modifications");

			// string f = SampleData.SampleData.FullFilePath;

			// cancel any startup modifications

		#if SHOWTICKS
			showTicks("load @5");
		#endif

			// OnTnInitAnnouncer.Announce(null);

		#if SHOWTICKS
			showTicks("load @6");
		#endif
		#if DML1
			DM.End0();
		#endif

		}

		private void MainWin_Closing(object sender, CancelEventArgs e)
		{
			UserSettings.Data.MainWinPos.X = (int) this.Left;
			UserSettings.Data.MainWinPos.Y = (int) this.Top;
			UserSettings.Admin.Write();

			// if (Common.SHOW_DEBUG_MESSAGE1)
			// 	Debug.WriteLine("@MainWin| is modified == | " + classificationFile.CanSave.ToString());


			if (classificationFile.CanSave == true)
			{
				TaskDialogResult result =
					CommonTaskDialogs.CommonWarningDialog(
						"Classifier Editor",
						"There are changes that have not been saved",
						"Do you want to save your changes?", WinHandle,
						(TaskDialogStandardButtons.Cancel | TaskDialogStandardButtons.No)
						);

				// MessageBoxResult result = MessageBox.Show(
				// 	"There are changes that have not been saved\n"
				// 	+ "Do you want to save your changes?",
				// 	"Classifier Editor", MessageBoxButton.YesNo,
				// 	MessageBoxImage.Warning);
				//
				// if (result == MessageBoxResult.Yes)

				if (result == TaskDialogResult.Cancel)
				{
					e.Cancel = true;
				}
			}
		}

		private void listCompOps(TreeNode parentNode)
		{
			foreach (TreeNode childNode in parentNode.Children)
			{
				if (childNode.HasChildren) listCompOps(childNode);

				Debug.WriteLine("");
				MessageUtilities.logMsgDbLn2("node.title", childNode.Item.Title);

				foreach (ComparisonOperation compOp in childNode.Item.CompareOps)
				{
					Debug.WriteLine("");
					MessageUtilities.logMsgDbLn2("Id", compOp.Id);

					if (compOp.ValueCompOpDef != null)
					{
						MessageUtilities.logMsgDbLn2("ValueCompOpDef.name", compOp.ValueCompOpDef.Name);
						MessageUtilities.logMsgDbLn2("ValueCompOpDef.opcode", compOp.ValueCompOpDef.OpCode.ToString());
					}
					else
					{
						MessageUtilities.logMsgDbLn2("ValueCompOpDef", "is null");
					}

					MessageUtilities.logMsgDbLn2("ValueCompareString", compOp.ValueCompareString);


					if (compOp.LogicalCompOpDef != null)
					{
						MessageUtilities.logMsgDbLn2("LogicalCompOpDef.name", compOp.LogicalCompOpDef.Name);
						MessageUtilities.logMsgDbLn2("LogicalCompOpDef.opcode",
							compOp.LogicalCompOpDef.OpCode.ToString());
					}
					else
					{
						MessageUtilities.logMsgDbLn2("LogicalCompOpDef", "is null");
					}

					MessageUtilities.logMsgDbLn2("LogicalCompareString", compOp.LogicalCompareString);

					MessageUtilities.logMsgDbLn2("CompareComponentIndex", compOp.CompareComponentIndex);
					MessageUtilities.logMsgDbLn2("CompareComponentName", compOp.CompareComponentName);
					MessageUtilities.logMsgDbLn2("CompareValue", compOp.CompareValue);
				}
			}
		}

		// private void fixCompOps(TreeNode parentNode)
		// {
		// 	foreach (TreeNode childNode in parentNode.Children)
		// 	{
		// 		if (childNode.HasChildren) fixCompOps(childNode);
		//
		//
		// 		foreach (ComparisonOperation compOp in childNode.Item.CompareOps)
		// 		{
		// 			if (compOp.LogicalCompOpDef != null)
		// 				compOp.LogicalCompOpDef =
		// 					CompareOperations.LogicalCompareOps[compOp.LogicalCompOpCode];
		//
		// 			if (compOp.ValueCompOpDef != null)
		// 				compOp.ValueCompOpDef =
		// 					CompareOperations.ValueCompareOps[compOp.ValueCompOpCode];
		// 		}
		// 	}
		// }

	#endregion

	#region event consuming

	#region control event methods

		// private void Lv2_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
		// private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		// {
		// 	Grid g = (sender as Grid);
		// 	TreeViewItem tvi = (g.Tag as TreeViewItem);
		// 	TreeNode t = ((TreeNode) tvi?.DataContext) ?? null;
		//
		// 	if (tvi != null) tvi.IsSelected = true;
		// }

		// treeview


		private void TreeViewItem_OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem tvi = sender as TreeViewItem;
			TreeNode tn = tvi.DataContext as TreeNode;


			// Debug.Write($"treeview item got on mouse up {sender?.GetType().Name ?? "is null"}  ");

			// if (tn != null)
			// {
			// 	Debug.WriteLine($"{$"@ mw 1| treeview mouse UP |",-50} {tn.Item.Title} ({tn.ID})");
			//
			// 	Debug.WriteLine(
			// 		$"{$"@ mw 7| treeview mouse UP | selected item |",-50} {((TreeNode) Tv1.SelectedItem)?.Item.Title ?? "null"} ({((TreeNode) Tv1.SelectedItem)?.ID ?? "null"})");
			//
			// 	Debug.Write("\n");
			// }
			// else
			// {
			// 	Debug.WriteLine($"@ mw 2| treeview mouse UP | tn is null");
			// }

			e.Handled = true;

			if (justSelected)
			{
				justSelected = false;
				return;
			}

			if (expanderGotMouse) return;
			if (nodeContextSource != null) return;
			if (mws.NodeEditing?.Item.NeedsSaving ?? false) return;
			if (mws.NodeSelected == null) return;

			mws.NodeSelected.IsNodeSelected = false;
			DeSelect();
		}

		private void TreeViewItem_OnSelected(object sender, RoutedEventArgs e)
		{
			TreeViewItem tvi = sender as TreeViewItem;
			TreeNode tn = tvi.DataContext as TreeNode;

			// Debug.Write($"treeview itemselected {sender?.GetType().Name ?? "is null"}  ");
			// if (tn != null)
			// {
			// 	Debug.WriteLine($"{"@ mw 3| treeview item selected |",-50} {tn.Item.Title} ({tn.ID})  ");
			// }
			// else
			// {
			// 	Debug.WriteLine($"@ mw 4| treeview item selected | tn is null  ");
			// }

			e.Handled = true;

			if (expanderGotMouse) return;
			if (nodeContextSource != null) return;
			if (mws.NodeEditing?.Item.NeedsSaving ?? false)
			{
				return;
			}

			justSelected = true;


			if (tn != null && tn.Item.IsFixed && UserIsAdmin == false) return;

			mws.SelectNode(tn);
		}

		private void TreeViewItem_OnUnSelected(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			// return;

			TreeViewItem tvi = sender as TreeViewItem;
			TreeNode tn = tvi.DataContext as TreeNode;

			// if (tn != null)
			// {
			// 	Debug.WriteLine($"{"@ mw 5| treeview item UN-selected |",-50} {tn.Item.Title} ({tn.ID}) ");
			// }
			// else
			// {
			// 	Debug.WriteLine($"@mw 6| treeview item UN-selected | tn is null  ");
			// }

			e.Handled = true;

			if (!mws.NodeEditing?.Item.NeedsSaving ?? false)
			{
				tn.IsNodeSelected = false;
			}
		}

		// private void Tv1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		// {
		// 	if (mws.NodeEditing?.Item.NeedsSaving ?? false)
		// 	{
		// 		TreeNode tn = e.NewValue  as TreeNode;
		//
		// 		e.Handled = true;
		// 	}
		// }


		// when a selection has been made
		// private void Tv1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		// {
		// 	// Debug.WriteLine($"@ node selected changed | {(e.NewValue as TreeNode)?.Item?.Title ?? "is null"}");
		//
		// 	// e.Handled = true;
		//
		// 	if (nodeContextSource != null) return;
		//
		// 	TreeNode temp = e.NewValue as TreeNode;
		//
		// 	if (temp !=null && temp.Item.IsFixed && UserIsAdmin==false) return;
		//
		// 	// if (temp == null || temp.Parent == null)
		// 	// {
		// 	// 	return;
		// 	// }
		//
		// 	// mws.SelectNode(temp);
		// }

		// context menu

		// context menu events

		private void Tv1ContextMenu_OnOpened(object sender, RoutedEventArgs e)
		{
			// sender is contextmenu
			// sender.datacontect is the treenode

			// Debug.WriteLine($"got ctx opened {sender?.GetType().Name ?? "is null"}");

			if (HasEditingChanges) return;

			TreeNode tn = ((ContextMenu) sender).DataContext as TreeNode;

			if (tn.CannotSelect) return;

			if (tn == null) return;

			if (nodeContextSource != null && tn.Equals(nodeContextSource)) return;

			ContextSelect(tn);

			// NodeContextTarget = (TreeNode) ((ContextMenu) sender).DataContext;
		}

		private void Tv1ContextMenu_OnClosed(object sender, RoutedEventArgs e)
		{
			// sender is contextmenu
			// sender.datacontect is the treenode

			// Debug.WriteLine($"got closed {sender?.GetType().Name ?? "is null"}");


			// if (!bypassContextDeHighlight) ContextDeHighlight();

			// NodeContextTarget = null;


			contextTargetDeselect();
		}

		private void Expander_MouseEnter(object sender, MouseEventArgs e)
		{
			// Debug.WriteLine("expander enter");
			expanderGotMouse = true;
		}

		private void Expander_MouseLeave(object sender, MouseEventArgs e)
		{
			// Debug.WriteLine("expander leave");
			expanderGotMouse = false;
		}

		// causes the nested listbox to transfer wheel movements to the parent treeview
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

		// /// <summary>
		// /// used to select and de-select notes on mouse down
		// /// </summary>
		// private void TreeViewItem_OnPreviewLeftMouseDown(object sender, MouseButtonEventArgs e)
		// {
		// 	// note that BaseOfTree.SelectedNode is set in the selection change event
		//
		// 	if (expanderGotMouse) return;
		//
		// 	bool gotReturn = false;
		//
		// 	TreeViewItem tvi = sender as TreeViewItem;
		// 	TreeNode tn = tvi.DataContext as TreeNode;
		//
		// 	// Debug.Write($"treeview item got on preview left mouse down {sender?.GetType().Name ?? "is null"}  ");
		// 	if (tn != null)
		// 	{
		// 		Debug.Write($"treeview item got on preview left mouse down {tn.Item.Title} ({tn.ID})  ");
		// 	} else
		// 	{
		// 		Debug.Write($"treeview item got on preview left mouse down | tn is null  ");
		// 	}
		//
		// 	// must be first
		// 	// this is the first event - nothing currently selected
		// 	// so select the node provided - the selection process is top down
		// 	// that is, the highest level item is clicked first, then the next level down
		// 	if (mws.NodeSelected == null)
		// 	{
		// 		// if (tn.IsNodeSelected) tn.IsNodeSelected = false;
		//
		// 		Debug.WriteLine("| node selected is null");
		// 		gotReturn = true;
		//
		// 		// e.Handled = true;  nope
		// 		return;
		// 	}
		//
		// 	if (mws.NodeEditing.TreeNodeModified || mws.NodeEditing.TreeNodeChildItemModified)
		// 	{
		// 		Debug.WriteLine("| editing treeode is modified / handled is true");
		// 		gotReturn = true;
		//
		// 		// e.Handled = true;
		// 		return;
		// 	}
		//
		// 	if (mws.NodeSelected.Equals(tn))
		// 	{
		// 		Debug.WriteLine("| selected treeode equals selected / handled is true");
		// 		gotReturn = true;
		//
		// 		// mws.NodeSelected.IsNodeSelected = false;
		//
		// 		// kills further processing
		// 		// e.Handled = true;
		// 	}
		//
		// 	if (!gotReturn) Debug.Write("\n");
		//
		// 	
		// }


		private void CbxLabel_OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			ComboBox cbx = sender as ComboBox;

			if (cbx is null) return;

			cbx.IsDropDownOpen = !cbx.IsDropDownOpen;
		}


	#region treeview editor context menu functions

		// context menu commands

		/*  status: developing
		*/
		private void Tv1ContextMenuExpColAll_OnClick(object sender, RoutedEventArgs e)
		{
			if (BaseOfTree.HasGrandChildrenExpanded)
			{
				BaseOfTree.CollapseAll();
			}
			else
			{
				BaseOfTree.ExpandAll();
			}
		}

		/*  status: ok
		*/
		private void Tv1ContextMenuExpand_OnClick(object sender, RoutedEventArgs e)
		{
			string command = ((MenuItem) sender).CommandParameter as string;

			// add a child to this leaf - also make a branch.
			// NodeContextTarget = (TreeNode) ((MenuItem) sender).DataContext;

			// if (nodeContextTarget == null) return;

			if (command.Equals(ContextCmdExpand))
			{
				nodeContextTarget.IsExpanded = true;
			}
			else
			{
				nodeContextTarget.IsExpanded = false;
			}
		}

		/*  status: ok
		*/
		private void Tv1ContextMenuAddChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			// NodeContextTarget = (TreeNode) ((MenuItem) sender).DataContext;
			//
			// if (nodeContextTarget == null) return;

			// NodeContextTarget.Item.CompOpSelectedIdx += 1;

			TreeNode newNode = BaseOfTree.AddNewChild2(nodeContextTarget);

			OnPropertyChanged(nameof(BaseOfTree));
			OnPropertyChanged(nameof(NodeContextTarget));

			nodeContextTarget.IsExpanded = true;

			mws.ValidateTree2();

			mws.SelectNode(newNode);
		}

		/* status: testing
		*/
		private void Tv1ContextMenuAddBefore_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			// NodeContextTarget = (TreeNode) ((MenuItem) sender).DataContext;
			//
			// if (nodeContextTarget == null) return;

			TreeNode newNode = BaseOfTree.AddNewBefore2(nodeContextTarget);

			OnPropertyChanged(nameof(BaseOfTree));
			OnPropertyChanged(nameof(NodeContextTarget));

			mws.ValidateTree2();

			mws.SelectNode(newNode);
		}

		/*  status: testing
		*/
		private void Tv1ContextMenuAddAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			// NodeContextTarget = (TreeNode) ((MenuItem) sender).DataContext;
			//
			// if (nodeContextTarget == null) return;

			TreeNode newNode = BaseOfTree.AddNewAfter2(nodeContextTarget);

			OnPropertyChanged(nameof(BaseOfTree));
			OnPropertyChanged(nameof(NodeContextTarget));

			mws.ValidateTree2();

			mws.SelectNode(newNode);
		}

		/* status: ok
		*/
		private void Tv1ContextMenuMoveBefore_OnClick(object sender, RoutedEventArgs e)
		{
			// if (nodeContextTarget == null || mws.NodeSelected == null) return;

			NodeContextSource = nodeContextTarget;

			DeSelect();

			NodeContextTarget = null;

			MoveOp = 1;
		}

		/* status: ok
		*/
		private void Tv1ContextMenuMoveAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			// NodeContextTarget = (TreeNode) ((MenuItem) sender).DataContext;
			//
			// if (nodeContextTarget == null || mws.NodeSelected == null) return;

			// BaseOfTree.MoveAfter(nodeContextTarget, mws.NodeSelected);

			NodeContextSource = nodeContextTarget;

			DeSelect();

			NodeContextTarget = null;

			MoveOp = 2;
		}

		/* status: ok
		*/
		private void Tv1ContextMenuMoveChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			// NodeContextTarget = (TreeNode) ((MenuItem) sender).DataContext;
			//
			// if (nodeContextTarget == null || mws.NodeSelected == null) return;

			// BaseOfTree.MoveAsChild(nodeContextTarget, mws.NodeSelected);

			NodeContextSource = nodeContextTarget;

			DeSelect();

			NodeContextTarget = null;

			MoveOp = 3;
		}

		/* status: ok
		*/
		private void Tv1ContextMenuDuplicate_OnClick(object sender, RoutedEventArgs e)
		{
			// add contextselected after this leaf
			// NodeContextTarget = (TreeNode) ((MenuItem) sender).DataContext;
			//
			// if (nodeContextTarget == null) return;

			TreeNode newNode = nodeContextTarget.Clone() as TreeNode;
			newNode.Item.Title = $"{newNode.Item.Title} (copy)";

			BaseOfTree.AddAfter2(nodeContextTarget, newNode);

			DeSelect();

			// ContextDeselect();

			OnPropertyChanged(nameof(BaseOfTree));
			OnPropertyChanged(nameof(NodeContextTarget));

			mws.ValidateTree2();

			mws.SelectNode(newNode);
		}

		/* status: ok
		*/
		private void Tv1ContextMenuDuplicateBranch_OnClick(object sender, RoutedEventArgs e)
		{
			TreeNode newNode = nodeContextTarget.CloneEx() as TreeNode;
			newNode.Item.Title = $"{newNode.Item.Title} (copy)";

			BaseOfTree.AddAfter2(nodeContextTarget, newNode);

			DeSelect();

			mws.ValidateTree2();

			mws.SelectNode(newNode);
		}

		/* status: testing
		*/
		private void Tv1ContextMenuDuplicateAsChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			// NodeContextTarget = (TreeNode) ((MenuItem) sender).DataContext;
			//
			// if (nodeContextTarget == null) return;

			TreeNode newNode  = nodeContextTarget.Clone(2) as TreeNode;
			newNode.Item.Title = $"{newNode.Item.Title} (copy)";

			BaseOfTree.AddChild2(nodeContextTarget, newNode);

			DeSelect();

			mws.ValidateTree2();

			mws.SelectNode(newNode);
		}

		/*  status:  ok
		 */
		private void Tv1ContextMenuDelete_OnClick(object sender, RoutedEventArgs e)
		{
			// bypassContextDeHighlight = true;

			// NodeContextTarget = (TreeNode) ((MenuItem) sender).DataContext;
			//
			// if (NodeContextTarget ==null) return;

			mws.DeleteNode(NodeContextTarget);

/*
			string msg;


			int extChildCount = nodeContextTarget.ExtChildCount;

			if (extChildCount > 0)
			{
				msg = "a total of " + extChildCount + " categories "
					+ "and sub-categories.";
			}
			else
			{
				msg = "no categories or sub-categories.";
			}

			string title = nodeContextTarget.Item.Title;

			TaskDialogResult result = CommonTaskDialogs.CommonWarningDialog(
				"Classifier Editor",
				"You are about to delete the category\n\"" + title + "\"",
				"The category \"" + title + "\" has " + msg + "\nIs it OK to delete this category?");


			//
			// TaskDialog td = new TaskDialog();
			//
			// td.Caption = "Classifier Editor";
			// td.InstructionText = 
			// td.Text = ;
			// td.Icon = TaskDialogStandardIcon.Warning;
			//
			//
			//
			// MessageBoxResult result = MessageBox.Show(
			// 	"You are about to delete the category\n"
			// 	+ title + "\nwith " + msg
			// 	+ "\nIs this correct?",
			// 	"Classifier Editor", MessageBoxButton.YesNo,
			// 	MessageBoxImage.Warning );

			// if (result == MessageBoxResult.Yes)

			if (result == TaskDialogResult.Ok)
			{
				BaseOfTree.RemoveNode2(nodeContextTarget);
			}
*/
			mws.ValidateTree2();

			OnPropertyChanged(nameof(BaseOfTree));
		}

		/* status: ok
		 */
		private void Tv1ContextMenuApply_OnClick(object sender, RoutedEventArgs e)
		{
			if (moveOp == 0)
			{
				return;
			}

			switch (moveOp)
			{
			case 1:
				{
					BaseOfTree.MoveBefore(nodeContextTarget, nodeContextSource);
					break;
				}
			case 2:
				{
					BaseOfTree.MoveAfter(nodeContextTarget, nodeContextSource);
					break;
				}
			case 3:
				{
					BaseOfTree.MoveAsChild(nodeContextTarget, nodeContextSource);
					break;
				}
			}

			moveOp = 0;

			mws.ValidateTree2();

			contextSourceDeselect();
		}

		/* status: ok
		*/
		private void Tv1ContextMenuCancel_OnClick(object sender, RoutedEventArgs e)
		{
			contextSourceDeselect();
		}

	#endregion

		private void CkbxDisable_OnChecked(object sender, RoutedEventArgs e)
		{
			if (HasSelection)
			{
				// ComparisonOperation c =  ((ComparisonOperation) ((CheckBox) sender).DataContext);

				int id = ((ComparisonOperation) ((CheckBox) sender).DataContext).Id;

				// int idx = nodeSelected.Item.FindCompOp(id) - 1;


				mws.NodeSelected.Item.CompareOps[id].IsDisabled = true;
			}
		}

		// private void CkbxLocked_OnChecked(object sender, RoutedEventArgs e)
		// {
		// 	CheckBox ckbx = (CheckBox) sender;
		//
		// 	if (ckbx.IsChecked.Value != true) return;
		//
		// 	// TreeNode selected = ckbx.DataContext as TreeNode;
		// 	//
		// 	// if (selected == null) return;
		//
		// 	// has been checked
		//
		// 	mws.NodeSelected.IsNodeSelected = false;
		// }

		/// <summary>
		/// de-select base of tree selected node and support node selected
		/// </summary>
		private void DeSelect()
		{
			if (BaseOfTree.SelectedNode != null)
			{
				BaseOfTree.SelectedNode.IsNodeSelected = false;
			}

			if (mws.NodeSelected != null)
			{
				mws.NodeSelected = null;
				mws.NodeEditing = null;
			}
		}

		private void ContextHighlight()
		{
			// ContextMenu cm =sender as ContextMenu;

			// nodeContextTarget.IsContextHighlighted = true;
		}

		private void ContextDeHighlight()
		{
			// if (nodeContextTarget != null)
			// 	nodeContextTarget.IsContextHighlighted = false;
		}

		private void ContextSelect(TreeNode selected)
		{
			NodeContextTarget = selected;

			contextSelectedTargetParent = nodeContextTarget.Parent;
			nodeContextTarget.IsContextTarget = true;
			// nodeContextTarget.IsContextSource = false;
		}

		private void contextTargetDeselect()
		{
			if (nodeContextTarget == null) return;

			nodeContextTarget.IsContextTarget = false;
			nodeContextTarget.IsContextSource = false;
			contextSelectedTargetParent = null;

			NodeContextTarget = null;
		}

		private void contextSourceDeselect()
		{
			if (nodeContextSource == null) return;

			nodeContextSource.IsContextTarget = false;
			nodeContextSource.IsContextSource = false;

			NodeContextSource = null;
		}


		// private void ContextDeselect()
		// {
		// 	// bypassContextDeHighlight = false;
		//
		// 	nodeContextTarget.IsContextTarget = false;
		// 	nodeContextTarget.IsContextSource = false;
		// 	contextSelectedTargetParent = null;
		//
		// 	NodeContextTarget = null;
		// }

		/// <summary>
		/// move ContextSource to before ContextTarget
		/// </summary>
		private void ContextMoveBefore() { }


		// context item disable
		// all branch items when the item is not a branch
		// is (node.cannot select)
		// any "to a sub-category" when depth + 1 > max depth

		private void setCannotContextSelect(TreeNode selected)
		{
			// cannot select because
			// if selected == source
			// if selected.cannotselect (ie., is fixed, or locked, or ismax depth
			// if doing a branch move to one of the branches own children
			// 
		}

	#region buttons

		private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
		{
			TreeNode tn = ((Button) sender).DataContext as TreeNode;

			if (tn == null) return;

			mws.DeleteNode(tn);
		}

		private void BtnRemExCollapseState_OnClick(object sender, RoutedEventArgs e)
		{
			RememberCollapseState = !RememberCollapseState;

			// OnRemExCollapseStateAnnouncer.Announce(UserSettings.Data.RememberNodeExpandState);
		}

		private void BtnAddCondition_OnClick(object sender, RoutedEventArgs e)
		{
			if (HasSelection)
			{
				if (mws.NodeEditing.Item.CompareOps.Count == 0)
				{
					mws.NodeEditing.Item.CompareOps.Add(
						new ComparisonOp(LOGICAL_NO_OP, EQUALTO, "A", mws.NodeEditing.Depth));
					mws.NodeEditing.Item.ChildCompOpModified = true;
				}
				else
				{
					mws.NodeEditing.Item.CompareOps.Add(
						new ComparisonOp(LOGICAL_OR, EQUALTO, "A",
							mws.NodeEditing.Item.CompareOps[0].CompareComponentIndex));
					mws.NodeEditing.Item.ChildCompOpModified = true;
				}
			}
		}

		private void BtnDeleteCondition_OnClick(object sender, RoutedEventArgs e)
		{
			if (HasSelection)
			{
				int id = ((ComparisonOperation) ((Button) sender).DataContext).Id;

				int idx = mws.NodeEditing.Item.FindCompOp(id) ;

				// nodeSelected.Item.RemoveCompOpAt(idx--);
				mws.NodeEditing.Item.RemoveCompOpAt(idx);
			}
		}

		private void BtnDoneEditing_OnClick(object sender, RoutedEventArgs e)
		{
			mws.NodeSelected.IsNodeSelected = false;
			mws.NodeSelected = null;
			mws.NodeEditing = null;
		}

		private void BtnCancelEdit_OnClick(object sender, RoutedEventArgs e)
		{
			// mws.NodeSelected.IsNodeSelected = false;
			// mws.NodeSelected = null;
			// mws.NodeEditing = null;

			DeSelect();
		}

		private void BtnSaveEdit_OnClick(object sender, RoutedEventArgs e)
		{
			mws.NodeSelected.Item.Merge(mws.NodeEditing.Item);

			// mws.NodeSelected.IsNodeSelected = false;
			// mws.NodeSelected = null;
			// mws.NodeEditing = null;

			DeSelect();
		}


		private void BtnSelect_OnClick(object sender, RoutedEventArgs e)
		{
			ClassificationFileSelector
				dialog = new ClassificationFileSelector();

			bool? result = dialog.ShowDialog();

			if (result != true) return;

			ClassificationFile = ClassificationFile.GetUserClassfFile(dialog.SelectedFileId);

			UserSettings.Data.LastClassificationFileId = dialog.SelectedFileId;
			UserSettings.Admin.Write();

			// OnTnInitAnnouncer.Announce(null);

			// dialog.Selected;
		}

		private void BtnCancelChanges_OnClick(object sender, RoutedEventArgs e)
		{
			ClassificationFile c = classificationFile;

			classificationFile = null;

			ClassificationFile = c;

			// OnTnInitAnnouncer.Announce(null);
		}

		private void BtnClassify_OnClick(object sender, RoutedEventArgs e)
		{
		#if DML1
			DM.Start0(true);
		#endif
			WindowClassifyTest winTest = new WindowClassifyTest();

			bool init = winTest.Configure(classificationFile);

			winTest.Owner = this;


			if (init) winTest.ShowDialog();

		#if DML1
			DM.End0();
		#endif
		}


		private void BtnSave_OnClick(object sender, RoutedEventArgs e)
		{
			// OnSavingAnnouncer.Announce(true);

			classificationFile.Write(true);

			BaseOfTree.NotifyChangeFromParent(INTERNODE_MESSAGES.IM_CLEAR_NODE_MODIFICATION);
			BaseOfTree.NotifyChangeFromParent(INTERNODE_MESSAGES.IM_CLEAR_ITEM_MODIFICATION);

			// BaseOfTree.IsNodeSelected = false;
			// BaseOfTree.TreeNodeChildItemModified = false;
			//
			// ClassificationFile.IsModified = false;

			// OnSavedAnnouncer.Announce(true);
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
		#if DML1
			DM.Start0(true);
		#endif
			this.Close();
		}

		private void BtnUpdateClassifFile_OnClick(object sender, RoutedEventArgs e)
		{
			if (fileIdNew.IsVoid()) return;

			OpenClassifFile(fileIdNew);
		}

		private void BtnValidate_OnClick(object sender, RoutedEventArgs e)
		{
			mws.ValidateTree();

			Debug.WriteLine("\n*** Validation Complete ***\n\n");
		}

		private void BtnRepair_OnClick(object sender, RoutedEventArgs e)
		{
			mws.RepairTree();

			Debug.WriteLine("\n*** Repair Complete ***");

			OnPropertyChanged(nameof(BaseOfTree));
		}

		// debug

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			List< ValueCompOpDef > v = ValueCompareOps;
			List< LogicalCompOpDef > l = LogicalCompareOps;

			ListView lv = Lv2;
			// ComboBox cbx = Lv2.ItemTemplate.FindName("Cbx1", Lv2) as ComboBox;

			UserSettingInfo<UserSettingDataFile> u = UserSettings.Info;

			BaseOfTree bot = BaseOfTree;

			bool tnMod = bot.TreeNodeModified;
			bool itemMod = bot.TreeNodeChildItemModified;

			bool b = ClassificationFile.IsModified;
			bool c = ClassificationFile.CanSave;
			bool ed = ClassificationFile.CanEdit;

			SheetFileList f = FileList;

			TreeNode tns = mws.NodeSelected;
			TreeNode tne = mws.NodeEditing;
			TreeNode tnct = NodeContextTarget;
			TreeNode tncs = NodeContextSource;

			bool isSel = mws?.NodeSelected?.IsNodeSelected ?? false;

			bool isAdmin = this.UserIsAdmin;

			string ic = tns?.Item.ItemClassName ?? "null";

			List<LogicalCompOpDef> lc = CompareOperations.LogicalCompareOps;
			List<ValueCompOpDef> vc = CompareOperations.ValueCompareOps;

			MainWindowClassifierEditor m = Me;

			Debug.WriteLine("at debug");
		}

	#endregion

	#endregion

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

		// private void CbxItemClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
		// {
		//
		// }

		// private void Tv1ContextMenu_OnContextMenuClosing(object sender, ContextMenuEventArgs e)
		// {
		// 	Debug.WriteLine($"got closed ctxm {sender?.GetType().Name ?? "is null"}");
		// }
		//
		// private void Tv1ContextMenu_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		// {
		// 	Debug.WriteLine($"got open ctxm {sender?.GetType().Name ?? "is null"}");
		// }


		private bool expcolwhich = false;
		private string messages;

		// private void tbx32_TextChanged(object sender, TextChangedEventArgs e) { }

		private void BtnDisableTv1_OnClick(object sender, RoutedEventArgs e)
		{
			Tv1.IsEnabled = !Tv1.IsEnabled;
		}
	}
}