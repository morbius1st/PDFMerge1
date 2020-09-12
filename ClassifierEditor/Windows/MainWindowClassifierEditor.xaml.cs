#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
// using ClassifierEditor.ConfigSupport;
using ClassifierEditor.SampleData;
// using ClassifierEditor.Tree;
using SettingsManager;
using UtilityLibrary;

// using static ClassifierEditor.Tree.CompareOperations;
// using static ClassifierEditor.Tree.ComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.ComparisonOp;
using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.FileSupport;
using AndyShared.Support;
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

	#region private fields

		private ClassificationFile classificationFile;

		// private Configuration config;
		private static TreeNode userSelected;
		private TreeNode contextSelected;
		private TreeNode contextSelectedParent;

		private bool bypassContextDeHighlight = false;

		// private Orator.ConfRoom.Announcer OnCfInitAnnouncer;
		private Orator.ConfRoom.Announcer OnSavedAnnouncer;
		private Orator.ConfRoom.Announcer OnTnInitAnnouncer;

	#endregion


	#region ctor

		public MainWindowClassifierEditor()
		{
			InitializeComponent();

			OnSavedAnnouncer = Orator.GetAnnouncer(this, OratorRooms.SAVED, "Modifications have been saved");
			OnTnInitAnnouncer = Orator.GetAnnouncer(this, OratorRooms.TN_INIT, "Initialize");

			SampleData.SampleData SD = new SampleData.SampleData();

		}

	#endregion

	#region public properties

		public static SampleData.SampleData SD { get; set; } = new SampleData.SampleData();

		public ClassificationFile ClassificationFile
		{
			get => classificationFile;
			private set
			{
				classificationFile = value;
				OnPropertyChange();
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

				OnPropertyChange();
				OnPropertyChange("HasSelection");

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

				OnPropertyChange();

				if (value == null) return;

				contextSelected.IsContextSelected = true;
				contextSelected.IsContextHighlighted = true;
			}
		}

		public bool HasSelection => userSelected != null;

		public bool HasContextSelection => contextSelected != null;

		public SampleFileList FileList { get; private set; } = new SampleFileList();

		public bool CanSave
		{
			get
			{
				if (classificationFile == null ||
					!classificationFile.IsValid ||
					!classificationFile.IsInitialized) return false;

				bool a = userSelected == null;
				bool b = classificationFile.IsModified;

				return b && a;
			}
		}

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

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region window event methods

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			UserSettings.Admin.Read();
			UserSettings.Admin.Write();

			MachSettings.Admin.Read();
			MachSettings.Admin.Write();


			string sampleFileName;

			// true to create sample data and save to disk
			// false to read existing data
			if (false)
			{
				// SampleData.SampleData sd = new SampleData.SampleData();

#pragma warning disable CS0162 // Unreachable code detected
				classificationFile = ClassificationFileAssist.GetUserClassfFile("(jeffs) PdfSample 1A");
#pragma warning restore CS0162 // Unreachable code detected

				SampleData.SampleData.Sample(classificationFile.Data.BaseOfTree);

				classificationFile.Write();

				sampleFileName = "";
			}
			else
			{
				string fileId = UserSettings.Data.LastClassificationFileId;

				classificationFile = ClassificationFileAssist.GetUserClassfFile(fileId);

				Debug.WriteLine("@ mainwin|@ onload| initialize classFfile");
				classificationFile.Initialize();

				sampleFileName = classificationFile.SampleFilePath;

				string desc = classificationFile.HeaderDescFromMemory;

				OnPropertyChange("ClassificationFile");
			}

			FileList = new SampleFileList(sampleFileName);

			OnPropertyChange("FileList");

			Debug.WriteLine("@ mainwin|@ onload| cancel all modifications");
			
			// cancel any startup modifications
			OnTnInitAnnouncer.Announce(null);
		}

		private void MainWin_Closing(object sender, CancelEventArgs e)
		{
			Debug.WriteLine("@MainWin| is modified == | " + classificationFile.IsModified.ToString());

			if (classificationFile.IsModified == true)
			{
				MessageBoxResult result = MessageBox.Show(
					"There are changes that have not been saved\n"
					+ "Do you want to save your changes?",
					"Classifier Editor", MessageBoxButton.YesNo,
					MessageBoxImage.Warning);

				if (result == MessageBoxResult.Yes)
				{
					e.Cancel = true;
				}
			}
		}

	#endregion

	#region event consuming

	#region control event methods

		// when a selection has been made
		private void Tv1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (e.NewValue == null) return;

			TreeNode selected = (TreeNode) e.NewValue;

			if (selected != null && selected.Item.IsFixed || selected.Item.IsLocked)
			{
				e.Handled = true;
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

		private void Tv1ContextMenuAddChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTree.AddNewChild2(contextSelected);

			contextSelected.IsExpanded = true;


			ContextDeselect();
		}

		private void Tv1ContextMenuAddBefore_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTree.AddNewBefore2(contextSelected);

			ContextDeselect();
		}

		private void Tv1ContextMenuAddAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTree.AddNewAfter2(contextSelected);


			ContextDeselect();
		}

		private void Tv1ContextMenuMoveBefore_OnClick(object sender, RoutedEventArgs e)
		{
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTree.MoveBefore(contextSelected, userSelected);

			BaseOfTree.SelectedNode.IsNodeSelected = false;

			ContextDeselect();
		}

		private void Tv1ContextMenuMoveAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTree.MoveAfter(contextSelected, userSelected);

			BaseOfTree.SelectedNode.IsNodeSelected = false;

			ContextDeselect();
		}

		private void Tv1ContextMenuMoveChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTree.MoveAsChild(contextSelected, userSelected);

			BaseOfTree.SelectedNode.IsNodeSelected = false;

			ContextDeselect();
		}

		private void Tv1ContextMenuSelCopy_OnClick(object sender, RoutedEventArgs e)
		{
			// add contextselected after this leaf
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			TreeNode newNode = contextSelected.Clone() as TreeNode;

			BaseOfTree.AddAfter2(contextSelected, newNode);

			ContextDeselect();
		}

		private void Tv1ContextMenuCopySelAsChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			TreeNode newNode = userSelected.Clone() as TreeNode;

			BaseOfTree.AddChild2(contextSelected, newNode);

			ContextDeselect();
		}

		private void Tv1ContextMenuDelete_OnClick(object sender, RoutedEventArgs e)
		{
			bypassContextDeHighlight = true;

			string msg;

			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			int extChildCount = contextSelected.ExtendedChildCount;

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

			MessageBoxResult result = MessageBox.Show(
				"You are about to delete the category\n"
				+ title + "\nwith " + msg
				+ "\nIs this correct?",
				"Classifier Editor", MessageBoxButton.YesNo,
				MessageBoxImage.Warning );

			if (result == MessageBoxResult.Yes)
			{
				BaseOfTree.RemoveNode2(contextSelected);
			}
			ContextDeselect();
		}

		private void CkbxDisable_OnChecked(object sender, RoutedEventArgs e)
		{
			if (HasSelection)
			{
				int id = ((ComparisonOperation) ((CheckBox) sender).DataContext).Id;

				int idx = userSelected.Item.FindCompOp(id) - 1;


				userSelected.Item.CompareOps[idx].IsDisabled = true;
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

		private void BtnAddCondition_OnClick(object sender, RoutedEventArgs e)
		{
			if (HasSelection)
			{
				if (userSelected.Item.CompareOps.Count > 0)
				{
					userSelected.Item.CompareOps.Add(new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_AND]));
				}

				userSelected.Item.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) EQUALTO], "A"));
			}
		}

		private void BtnDeleteCondition_OnClick(object sender, RoutedEventArgs e)
		{
			if (HasSelection)
			{
				int id = ((ComparisonOperation) ((Button) sender).DataContext).Id;

				int idx = userSelected.Item.FindCompOp(id) ;

				userSelected.Item.RemoveCompOpAt(idx--);
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

			ClassificationFile = dialog.Selected;

		}


		private void BtnSave_OnClick(object sender, RoutedEventArgs e)
		{
			classificationFile.Write();

			// classificationFile.IsModified = false;

			OnSavedAnnouncer.Announce(true);
		}

		private void BtnTestAll_OnClick(object sender, RoutedEventArgs e) { }

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			List< ValueCompareOp > a = ValueCompareOps;
			List< LogicalCompareOp > b = LogicalCompareOps;


			ListView lv = Lv2;
			// ComboBox cbx = Lv2.ItemTemplate.FindName("Cbx1", Lv2) as ComboBox;


			Debug.WriteLine("at debug");
		}

	#endregion

	#endregion

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

		public void Connect(int connectionId, object target) { }

		private void Lv2_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
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

#region bool to "on" / "off" string value converter

	[ValueConversion(typeof(bool), typeof(string))]
	public class BoolToOnOffConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(object))
				throw new InvalidOperationException("The target must be a object");

			if ((bool) value)
			{
				return "On";
			}

			return "Off";
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

#endregion

	public class Lv1ConditionTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;

			if (element != null && item != null && item is ComparisonOperation)
			{
				ComparisonOperation taskitem = item as ComparisonOperation;


				if (taskitem.CompareOp is LogicalCompareOp)
				{
					return
						element.FindResource("Lv1DataTemplate2") as DataTemplate;
				}
				else if (taskitem.CompareOp.OpCodeValue == (int) NO_OP)
				{
					return
						element.FindResource("Lv1DataTemplate3") as DataTemplate;
				}
				else
				{
					return
						element.FindResource("Lv1DataTemplate1") as DataTemplate;
				}
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

				if (taskitem.CompareOp is LogicalCompareOp)
				{
					return
						element.FindResource("Lv2DataTemplate2") as DataTemplate;
				}
				else if (taskitem.CompareOp.OpCodeValue == (int) NO_OP)
				{
					return
						element.FindResource("Lv2DataTemplate3") as DataTemplate;
				}
				else
				{
					return
						element.FindResource("Lv2DataTemplate1") as DataTemplate;
				}
			}

			return null;
		}
	}
}
