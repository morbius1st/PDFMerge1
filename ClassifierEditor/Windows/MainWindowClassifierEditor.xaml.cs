﻿#region using

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
using ClassifierEditor.ConfigSupport;

using ClassifierEditor.DataRepo;
using ClassifierEditor.FilesSupport;
using ClassifierEditor.Tree;
using SettingsManager;
#pragma warning disable CS0246 // The type or namespace name 'UtilityLibrary' could not be found (are you missing a using directive or an assembly reference?)
using UtilityLibrary;
#pragma warning restore CS0246 // The type or namespace name 'UtilityLibrary' could not be found (are you missing a using directive or an assembly reference?)
using static ClassifierEditor.Tree.CompareOperations;
using static ClassifierEditor.Tree.ComparisonOp;

#endregion

// projname: ClassifierEditor
// itemname: MainWindowClassifierEditor
// username: jeffs
// created:  5/2/2020 9:16:20 AM

//

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
		

	#region private fields

		private SheetCategoryDataManager categories = new SheetCategoryDataManager();
		private Configuration config;
		private static TreeNode userSelected;
		private TreeNode contextSelected;
		private TreeNode contextSelectedParent;

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

		private bool bypassContextDeHighlight = false;

	#endregion

	#region ctor

		static MainWindowClassifierEditor()
		{
			SampleData.SampleData sd = new SampleData.SampleData();
			sd.Sample(BaseOfTreeRoot);

//			SheetCategory sc = new SheetCategory("title", "description", "pattern");
//			temp = new TreeNode(
//				new BaseOfTree(), sc, false);

			sd.SampleFiles(FileList2);

			FilePath<FileNameSheetPdf> sheetname = new FilePath<FileNameSheetPdf>(
				@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs\A A1.0-0 This is a Test A10.pdf");

			MakeSample();
		}

		public MainWindowClassifierEditor()
		{
			InitializeComponent();
		}

		public static TreeNode temp { get; set; }

		public static void MakeSample()
		{
			Lv2ConditionTemplateSelector.MasterIdIdx = 0;

			temp = new TreeNode(new BaseOfTree(),
				new SheetCategory("title", "description")
				{
					CompareOps = new ObservableCollection<ComparisonOperation>()
					{
						new ValueCompOp(ValueCompareOps[(int) CONTAINS], "First", isFirst: true, ignore: false),
						new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_OR]),
						new ValueCompOp(ValueCompareOps[(int) DOES_NOT_EQUAL], "1000 to 500 to 1000 to 800"),
						new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_AND], true),
						new ValueCompOp(ValueCompareOps[(int) EQUALTO], "1000", ignore: true),
						new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_OR]),
						new ValueCompOp(ValueCompareOps[(int) MATCHES], "2000"),
						new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_OR]),
						new ValueCompOp(ValueCompareOps[(int) MATCHES], "2000"),
						new LogicalCompOp(LogicalCompareOps[(int) LOGICAL_AND]),
						new ValueCompOp(ValueCompareOps[(int) MATCHES], "2000")
					}
				}
				, false );
		}

		public static List<ValueCompareOp> vComps = new List<ValueCompareOp>()
		{
			new ValueCompareOp("Does Not Contain", DOES_NOT_CONTAIN),
			new ValueCompareOp("Contains", CONTAINS),
			new ValueCompareOp("Does not Match", DOES_NOT_MATCH),
			new ValueCompareOp("Match", MATCHES),
		};

	#endregion

	#region public properties

		// this is only for design time sample data
		public static BaseOfTree BaseOfTreeRoot { get; set; } = new BaseOfTree();


		// this is the live data store
		public SheetCategoryDataManager Categories
		{
			get => categories;
			private set
			{
				categories = value;
				OnPropertyChange();
			}
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


		public static SampleFileList FileList2 { get; private set; } = new SampleFileList();

		public SampleFileList FileList { get; private set; }

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
			config = new Configuration();

			string sampleFileName;

			// true to create sample data and save to disk
			// false to read existing data
			if (false)
			{

#pragma warning disable CS0162 // Unreachable code detected
				SampleData.SampleData sd = new SampleData.SampleData();
#pragma warning restore CS0162 // Unreachable code detected

				sd.Sample(categories.TreeBase);
				categories.Write();

				sampleFileName = "";
			}
			else
			{
				//todo: fix
				// config.ConfigureCategories(categories);

				categories.Configure(config.LastEditedClassificationFolderPath,
					config.LastEditedClassificationFileName);
				categories.Read();

				sampleFileName = config.LastEditedSampleFilePath;

				string desc = config.LastEditedClassificationFileDescription;
			}

			// todo: fix
			// sampleFileName =
			// 	UserSettings.Data.CatConfigSampleFolder + @"\" + UserSettings.Data.CatConfigSampleFile;

			FileList = new SampleFileList(sampleFileName);

			OnPropertyChange("FileList");
		}

		private void MainWin_Closing(object sender, CancelEventArgs e)
		{
			if (categories.IsModified == true)
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

	#region control event methods

		private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			categories.IsModified = true;
		}

		// when a selection has been made
		private void Tv1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			TreeNode selected = (TreeNode) e.NewValue;

			if (selected != null && selected.IsFixed || selected.IsLocked)
			{
				e.Handled = true;
				UserSelected = null;
				return;
			}

			UserSelected = selected;
			BaseOfTreeRoot.SelectedNode = userSelected;
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

			BaseOfTreeRoot.AddNewChild2(contextSelected);

			contextSelected.IsExpanded = true;

			ContextDeselect();
		}

		private void Tv1ContextMenuAddBefore_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTreeRoot.AddNewBefore2(contextSelected);

			ContextDeselect();
		}

		private void Tv1ContextMenuAddAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTreeRoot.AddNewAfter2(contextSelected);


			ContextDeselect();
		}

		private void Tv1ContextMenuMoveBefore_OnClick(object sender, RoutedEventArgs e)
		{
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTreeRoot.MoveBefore(contextSelected, userSelected);

			BaseOfTreeRoot.SelectedNode.IsNodeSelected = false;

			ContextDeselect();
		}

		private void Tv1ContextMenuMoveAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTreeRoot.MoveAfter(contextSelected, userSelected);

			BaseOfTreeRoot.SelectedNode.IsNodeSelected = false;

			ContextDeselect();
		}

		private void Tv1ContextMenuMoveChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTreeRoot.MoveAsChild(contextSelected, userSelected);

			BaseOfTreeRoot.SelectedNode.IsNodeSelected = false;

			ContextDeselect();
		}

		private void Tv1ContextMenuSelCopy_OnClick(object sender, RoutedEventArgs e)
		{
			// add contextselected after this leaf
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			TreeNode newNode = contextSelected.Clone() as TreeNode;

			BaseOfTreeRoot.AddAfter2(contextSelected, newNode);

			ContextDeselect();
		}

		private void Tv1ContextMenuCopySelAsChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			TreeNode newNode = userSelected.Clone() as TreeNode;

			BaseOfTreeRoot.AddChild2(contextSelected, newNode);

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
				BaseOfTreeRoot.RemoveNode2(contextSelected);
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

		private void BtnSave_OnClick(object sender, RoutedEventArgs e)
		{
			categories.Write();
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
			ComboBox cbx = Lv2.ItemTemplate.FindName("Cbx1", Lv2) as ComboBox;


			Debug.WriteLine("at debug");
		}

	#endregion

	#endregion

	#region event handeling

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
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