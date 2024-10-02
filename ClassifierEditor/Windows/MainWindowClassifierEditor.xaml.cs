// #define SHOWTICKS

#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
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
using JetBrains.Annotations;
using WpfShared.Windows;

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
				✔ add IsModified to correct properties

baseoftree/treenode
				✔ listen / TN_INT
				✔ listen / SAVED
				✔ announce / MODIFIED (from IsModified)
				✔ add IsModified to correct properties

sheetcategory
				✔ listen / TN_INIT
				✔ listen / SAVED
				✔ announce / MODIFIED (from IsModified)
				✔ add IsModified to correct properties

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
	public partial class MainWindowClassifierEditor : Window, INotifyPropertyChanged
	{
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
		public string ContextCmdExpand { get; }            = "Expand";
		public string ContextCmdCollapse { get; }          = "Collapse";

	#endregion

		// public AndyShared.ClassificationDataSupport.TreeSupport.CheckedState bot;

	#if SHOWTICKS
		private long ticksStart;
		private long tickPrior;
		private long tickNow;
	#endif

	#region private fields

		private SettingsMgr<UserSettingPath, UserSettingInfo<UserSettingDataFile>, UserSettingDataFile> us =
			UserSettings.Admin;

		private ClassificationFile classificationFile;

		// private Configuration config;
		private static TreeNode userSelected;
		private TreeNode contextSelected;
		private TreeNode contextSelectedParent;

		private bool bypassContextDeHighlight = false;

		// private Orator.ConfRoom.Announcer OnCfInitAnnouncer;
		private Orator.ConfRoom.Announcer OnSavedAnnouncer;
		private Orator.ConfRoom.Announcer OnTnInitAnnouncer;
		private Orator.ConfRoom.Announcer OnRemExCollapseStateAnnouncer;
		private Orator.ConfRoom.Announcer OnSavingAnnouncer;

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

			InitializeComponent();

		#if SHOWTICKS
			showTicks("start @2");
		#endif

			OnSavedAnnouncer = Orator.GetAnnouncer(this, OratorRooms.SAVED, "Modifications have been saved");
			OnSavingAnnouncer = Orator.GetAnnouncer(this, OratorRooms.SAVING, "Before Modifications get saved");
			OnTnInitAnnouncer = Orator.GetAnnouncer(this, OratorRooms.TN_INIT, "Initialize");
			OnRemExCollapseStateAnnouncer = Orator.GetAnnouncer(this, OratorRooms.TN_REM_EXCOLLAPSE_STATE,
				"Remember Exp-Collapse State");

			// SampleData.SampleData SD = new SampleData.SampleData();

		#if SHOWTICKS
			showTicks("start @3");
		#endif
		}

	#endregion

	#if SHOWTICKS
		private void showTicks(string title)
		{
			tickNow = DateTime.Now.Ticks;
			Debug.Print(title + "|  " + (tickNow - ticksStart)
				+ "  diff| " + (tickNow - tickPrior));
			tickPrior =  tickNow;
		}
	#endif

	#region public properties

		public ClassificationFile ClassificationFile
		{
			get => classificationFile;
			private set
			{
				if (value == null) return;

				classificationFile = value;

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ mainwin|@ onload| initialize classFfile");


				classificationFile.Initialize();

				FileList = new SheetFileList();
				FileList.ReadSampleSheetFileList(classificationFile.SampleFilePath);

				string desc = classificationFile.HeaderDescFromMemory;

				UpdateProperties();
				OnPropertyChanged(nameof(FileList));
			}
		}

		public BaseOfTree BaseOfTree
		{
			get => classificationFile.TreeBase;
		}

		public TreeNode UserSelected
		{
			get => userSelected;
			set
			{
				ContextDeHighlight();

				userSelected = value;

				OnPropertyChanged();
				OnPropertyChanged(nameof(HasSelection));
				OnPropertyChanged(nameof(ClassificationFile));


				Lv2ConditionTemplateSelector.MasterIdIdx = 0;
			}
		}

		public TreeNode ContextSelected
		{
			get => contextSelected;
			private set
			{
				if (value == contextSelected) return;

				if (contextSelected != null)
				{
					contextSelected.IsContextSelected = false;
					contextSelected.IsContextHighlighted = false;
				}

				contextSelected = value;
				contextSelectedParent = contextSelected.Parent;

				OnPropertyChanged();

				if (value == null) return;

				contextSelected.IsContextSelected = true;
				contextSelected.IsContextHighlighted = true;
			}
		}

		public TreeNode SelectedItem { get; set; }

		public bool HasSelection => userSelected != null;

		public bool HasContextSelection => contextSelected != null;

		public SheetFileList FileList { get; private set; } = new SheetFileList();


		public string test { get; set; }

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


		// public bool CanSave
		// {
		// 	get
		// 	{
		// 		if (classificationFile == null ||
		// 			!classificationFile.IsValid ||
		// 			!classificationFile.IsInitialized) return false;
		//
		// 		bool a = userSelected == null;
		// 		bool b = classificationFile.IsModified;
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
		}

		private void UpdateProperties()

		{
			OnPropertyChanged("ClassificationFile");
			OnPropertyChanged("BaseOfTree");
			OnPropertyChanged("UserSelected");
			OnPropertyChanged("ContextSelected");
			OnPropertyChanged("HasSelection");
			OnPropertyChanged("FileList");
		}

	#endregion

	#region window event methods

		private void Window_Initialized(object sender, EventArgs e)
		{
		#if SHOWTICKS
			showTicks("init @1");
		#endif

			initSettings();

			this.Top = UserSettings.Data.MainWinPos.Y;
			this.Left = UserSettings.Data.MainWinPos.X;

		#if SHOWTICKS
			showTicks("init @2");
		#endif
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
		#if SHOWTICKS
			showTicks("load @1");
		#endif

			// true to create sample data and save to disk
			// false to read existing data
			if (false)
			{
				// temp code to create a new classification file 
				classificationFile = ClassificationFileAssist.GetUserClassfFile("PdfSample 1A");

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

					ClassificationFile = ClassificationFileAssist.GetUserClassfFile("PdfSample 1");

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
					OnRemExCollapseStateAnnouncer.Announce(UserSettings.Data.RememberNodeExpandState);

					// Debug.WriteLine("@MainWin load| load classification file");
					ClassificationFile = ClassificationFileAssist.GetUserClassfFile(fileId);
				}
			}

			if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ mainwin|@ onload| cancel all modifications");

			// string f = SampleData.SampleData.FullFilePath;

			// cancel any startup modifications

		#if SHOWTICKS
			showTicks("load @5");
		#endif

			OnTnInitAnnouncer.Announce(null);

		#if SHOWTICKS
			showTicks("load @6");
		#endif
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

					if (compOp.ValueCompareOp != null)
					{
						MessageUtilities.logMsgDbLn2("ValueCompareOp.name", compOp.ValueCompareOp.Name);
						MessageUtilities.logMsgDbLn2("ValueCompareOp.opcode", compOp.ValueCompareOp.OpCode.ToString());
					}
					else
					{
						MessageUtilities.logMsgDbLn2("ValueCompareOp", "is null");
					}

					MessageUtilities.logMsgDbLn2("ValueCompareString", compOp.ValueCompareString);


					if (compOp.LogicalCompareOp != null)
					{
						MessageUtilities.logMsgDbLn2("LogicalCompareOp.name", compOp.LogicalCompareOp.Name);
						MessageUtilities.logMsgDbLn2("LogicalCompareOp.opcode",
							compOp.LogicalCompareOp.OpCode.ToString());
					}
					else
					{
						MessageUtilities.logMsgDbLn2("LogicalCompareOp", "is null");
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
		// 			if (compOp.LogicalCompareOp != null)
		// 				compOp.LogicalCompareOp =
		// 					CompareOperations.LogicalCompareOps[compOp.LogicalCompOpCode];
		//
		// 			if (compOp.ValueCompareOp != null)
		// 				compOp.ValueCompareOp =
		// 					CompareOperations.ValueCompareOps[compOp.ValueCompOpCode];
		// 		}
		// 	}
		// }


		private void MainWin_Closing(object sender, CancelEventArgs e)
		{
			UserSettings.Data.MainWinPos.X = (int) this.Left;
			UserSettings.Data.MainWinPos.Y = (int) this.Top;
			UserSettings.Admin.Write();

			if (Common.SHOW_DEBUG_MESSAGE1)
				Debug.WriteLine("@MainWin| is modified == | " + classificationFile.IsModified.ToString());


			if (classificationFile.IsModified == true)
			{
				TaskDialogResult result =
					CommonTaskDialogs.CommonWarningDialog(
						"Classifier Editor",
						"There are changes that have not been saved",
						"Do you want to save your changes?",
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

	#endregion

	#region event consuming

	#region control event methods

		// private void Lv2_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
		private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Grid g = (sender as Grid);
			TreeViewItem tvi = (g.Tag as TreeViewItem);
			TreeNode t = ((TreeNode) tvi?.DataContext) ?? null;

			if (tvi != null) tvi.IsSelected = true;
		}

		// when a selection has been made
		private void Tv1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			e.Handled = true;

			if (e.NewValue == null) return;

			TreeNode selected = (TreeNode) e.NewValue;

			if (selected != null && selected.Item.IsFixed || selected.Item.IsLocked)
			{
				UserSelected = null;
				return;
			}

			UserSelected = selected;
			BaseOfTree.SelectedNode = userSelected;
		}

		// context menu events
		private void Tv1ContextMenu_OnOpened(object sender, RoutedEventArgs e)
		{
			// sender is contextmenu
			// sender.datacontect is the treenode
			ContextSelected = (TreeNode) ((ContextMenu) sender).DataContext;
		}


		private void Tv1ContextMenu_OnClosed(object sender, RoutedEventArgs e)
		{
			// sender is contextmenu
			// sender.datacontect is the treenode

			if (!bypassContextDeHighlight) ContextDeHighlight();
		}

		// context menu commands
		private void Tv1ContextMenuExpand_OnClick(object sender, RoutedEventArgs e)
		{
			string command = ((MenuItem) sender).CommandParameter as string;

			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			if (command.Equals(ContextCmdExpand))
			{
				contextSelected.IsExpanded = true;
			}
			else
			{
				contextSelected.IsExpanded = false;
			}
		}


	#region treeview editor functions

		/*
		* status: 
		*/
		private void Tv1ContextMenuAddChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			if (contextSelected == null) return;

			BaseOfTree.AddNewChild2(contextSelected);

			OnPropertyChanged("BaseOfTree");
			OnPropertyChanged("ContextSelected");

			contextSelected.IsExpanded = true;

			ContextDeselect();
		}

		/*
		* status: 
		*/
		private void Tv1ContextMenuAddBefore_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			if (contextSelected == null) return;

			BaseOfTree.AddNewBefore2(contextSelected);

			OnPropertyChanged("BaseOfTree");
			OnPropertyChanged("ContextSelected");

			ContextDeselect();
		}

		/*
		* status: 
		*/
		private void Tv1ContextMenuAddAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			if (contextSelected == null) return;

			BaseOfTree.AddNewAfter2(contextSelected);

			OnPropertyChanged("BaseOfTree");
			OnPropertyChanged("ContextSelected");

			ContextDeselect();
		}

		/*
		* status: 
		*/
		private void Tv1ContextMenuMoveBefore_OnClick(object sender, RoutedEventArgs e)
		{
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			if (contextSelected == null || userSelected == null) return;

			BaseOfTree.MoveBefore(contextSelected, userSelected);

			DeSelect();

			ContextDeselect();
		}

		/*
		* status: 
		*/
		private void Tv1ContextMenuMoveAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			if (contextSelected == null || userSelected == null) return;

			BaseOfTree.MoveAfter(contextSelected, userSelected);

			DeSelect();

			ContextDeselect();
		}

		/*
		* status: 
		*/
		private void Tv1ContextMenuMoveChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			if (contextSelected == null || userSelected == null) return;

			BaseOfTree.MoveAsChild(contextSelected, userSelected);

			DeSelect();

			ContextDeselect();
		}

		/*
		* status: 
		*/
		private void Tv1ContextMenuSelCopy_OnClick(object sender, RoutedEventArgs e)
		{
			// add contextselected after this leaf
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			if (contextSelected == null) return;

			TreeNode newNode = contextSelected.Clone() as TreeNode;

			BaseOfTree.AddAfter2(contextSelected, newNode);

			DeSelect();

			ContextDeselect();
		}

		/*
		* status: 
		*/
		private void Tv1ContextMenuCopySelAsChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			if (contextSelected == null) return;

			TreeNode newNode = userSelected.Clone() as TreeNode;

			BaseOfTree.AddChild2(contextSelected, newNode);

			DeSelect();

			ContextDeselect();
		}

		/*
		* status: 
		*/
		private void Tv1ContextMenuDelete_OnClick(object sender, RoutedEventArgs e)
		{
			bypassContextDeHighlight = true;

			string msg;

			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			int extChildCount = contextSelected.ExtChildCount;

			if (extChildCount > 0)
			{
				msg = "a total of " + extChildCount + " categories "
					+ "and sub-categories.";
			}
			else
			{
				msg = "no categories or sub-categories.";
			}

			string title = contextSelected.Item.Title;

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
				BaseOfTree.RemoveNode2(contextSelected);
			}

			ContextDeselect();
		}

	#endregion


		private void CkbxDisable_OnChecked(object sender, RoutedEventArgs e)
		{
			if (HasSelection)
			{
				int id = ((ComparisonOperation) ((CheckBox) sender).DataContext).Id;

				// int idx = userSelected.Item.FindCompOp(id) - 1;


				userSelected.Item.CompareOps[id].IsDisabled = true;
			}
		}

		private void CkbxLocked_OnChecked(object sender, RoutedEventArgs e)
		{
			CheckBox ckbx = (CheckBox) sender;

			if (ckbx.IsChecked.Value != true) return;

			TreeNode selected = ckbx.DataContext as TreeNode;

			if (selected == null) return;

			// has been checked

			selected.IsNodeSelected = false;
		}

		private void DeSelect()
		{
			BaseOfTree.SelectedNode.IsNodeSelected = false;

			UserSelected = null;
		}

		private void ContextHighlight()
		{
			contextSelected.IsContextHighlighted = true;
		}

		private void ContextDeHighlight()
		{
			if (contextSelected != null)
				contextSelected.IsContextHighlighted = false;
		}

		private void ContextDeselect()
		{
			bypassContextDeHighlight = false;

			if (contextSelected != null)
			{
				contextSelected.IsContextSelected = false;
				contextSelected.IsContextHighlighted = false;

				contextSelected = null;
			}
		}

	#region buttons

		private void BtnRemExCollapseState_OnClick(object sender, RoutedEventArgs e)
		{
			RememberCollapseState = !RememberCollapseState;

			OnRemExCollapseStateAnnouncer.Announce(UserSettings.Data.RememberNodeExpandState);
		}

		private void BtnAddCondition_OnClick(object sender, RoutedEventArgs e)
		{
			if (HasSelection)
			{
				if (userSelected.Item.CompareOps.Count == 0)
				{
					userSelected.Item.CompareOps.Add(
						new ValueCompOp(LOGICAL_NO_OP, EQUALTO, "A", userSelected.Depth));
				}
				else
				{
					userSelected.Item.CompareOps.Add(
						new ValueCompOp(LOGICAL_OR, EQUALTO, "A",
							userSelected.Item.CompareOps[0].CompareComponentIndex));
				}
			}
		}

		private void BtnDeleteCondition_OnClick(object sender, RoutedEventArgs e)
		{
			if (HasSelection)
			{
				int id = ((ComparisonOperation) ((Button) sender).DataContext).Id;

				int idx = userSelected.Item.FindCompOp(id) ;

				// userSelected.Item.RemoveCompOpAt(idx--);
				userSelected.Item.RemoveCompOpAt(idx);
			}
		}

		private void BtnDoneEditing_OnClick(object sender, RoutedEventArgs e)
		{
			UserSelected.IsNodeSelected = false;
			UserSelected = null;
		}

		private void BtnSelect_OnClick(object sender, RoutedEventArgs e)
		{
			ClassificationFileSelector
				dialog = new ClassificationFileSelector();

			bool? result = dialog.ShowDialog();

			if (result != true) return;

			ClassificationFile = ClassificationFileAssist.GetUserClassfFile(dialog.SelectedFileId);

			OnTnInitAnnouncer.Announce(null);

			// dialog.Selected;
		}

		private void BtnCancelChanges_OnClick(object sender, RoutedEventArgs e)
		{
			ClassificationFile c = classificationFile;

			classificationFile = null;

			ClassificationFile = c;

			OnTnInitAnnouncer.Announce(null);
		}


		private void BtnSave_OnClick(object sender, RoutedEventArgs e)
		{
			OnSavingAnnouncer.Announce(true);

			classificationFile.Write();

			OnSavedAnnouncer.Announce(true);
		}

		private void BtnClassify_OnClick(object sender, RoutedEventArgs e)
		{
			WindowClassifyTest winTest = new WindowClassifyTest();

			bool init = winTest.Configure(classificationFile);

			if (init) winTest.Show();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			List< ValueCompareOp > v = ValueCompareOps;
			List< LogicalCompareOp > l = LogicalCompareOps;

			ListView lv = Lv2;
			// ComboBox cbx = Lv2.ItemTemplate.FindName("Cbx1", Lv2) as ComboBox;

			UserSettingInfo<UserSettingDataFile> u = UserSettings.Info;

			BaseOfTree bot = BaseOfTree;

			TreeNode tn = userSelected;
			bool b = ClassificationFile.IsModified;

			SheetFileList f = FileList;

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

	}


	#region NotBool value converter

	[ValueConversion(typeof(bool), typeof(bool))]
	public class NotBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(bool))
				throw new InvalidOperationException("The target must be a boolean");

			return !(bool) value;
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

#endregion

//
// #region bool to "on" / "off" string value converter
//
// 	[ValueConversion(typeof(bool), typeof(string))]
// 	public class BoolToOnOffConverter : IValueConverter
// 	{
// 		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
// 		{
// 			if (targetType != typeof(object))
// 				throw new InvalidOperationException("The target must be a object");
//
// 			if ((bool) value)
// 			{
// 				return "On";
// 			}
//
// 			return "Off";
// 		}
//
// 		public object ConvertBack(object value, Type targetType, object parameter,
// 			System.Globalization.CultureInfo culture)
// 		{
// 			throw new NotSupportedException();
// 		}
// 	}
//
// #endregion

	public class Lv1ConditionTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;

			if (element != null && item != null && item is ComparisonOperation)
			{
				ComparisonOperation taskitem = item as ComparisonOperation;

				if (taskitem.ValueCompareOp.OpCodeValue == (int) VALUE_NO_OP)
				{
					return
						element.FindResource("Lv1DataTemplate3") as DataTemplate;
				}

				return
					element.FindResource("Lv1DataTemplate0") as DataTemplate;


				// if (taskitem.ValueCompareOp is LogicalCompareOp)
				// {
				// 	return
				// 		element.FindResource("Lv1DataTemplate2") as DataTemplate;
				// }
				// else if (taskitem.ValueCompareOp.OpCodeValue == (int) NO_OP)
				// {
				// 	return
				// 		element.FindResource("Lv1DataTemplate3") as DataTemplate;
				// }
				// else
				// {
				// 	return
				// 		element.FindResource("Lv1DataTemplate1") as DataTemplate;
				// }
			}

			return null;
		}
	}

	public class Lv2ConditionTemplateSelector : DataTemplateSelector
	{
		public static int MasterIdIdx;


		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;

			if (element != null && item != null && item is ComparisonOperation)
			{
				ComparisonOperation taskitem = item as ComparisonOperation;

				taskitem.Id = MasterIdIdx++;

				// if (taskitem.ValueCompOpCode == (int) VALUE_NO_OP)
				if (taskitem.ValueCompOpCode == (int) VALUE_NO_OP)
				{
					return
						element.FindResource("Lv2DataTemplate3") as DataTemplate;
				}

				return
					element.FindResource("Lv2DataTemplate0") as DataTemplate;
			}

			return null;
		}
	}
}