#region using

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ClassifierEditor.DataRepo;
using ClassifierEditor.FilesSupport;
using ClassifierEditor.SampleFiles;
using ClassifierEditor.Tree;
using ClassifierEditor.Windows.Support;
using SettingsManager;

#endregion

// projname: ClassifierEditor
// itemname: MainWindow
// username: jeffs
// created:  5/2/2020 9:16:20 AM

//

namespace ClassifierEditor.Windows
{
	/*
	 data chain

		MainWindow				(SheetCategoryDataManager  Categories)
		V
		SheetCategoryDataManager	(TreeNode TreeBase)
		V								  | ^
		StorageManager					  | +<- shtCategories [StorageManager<SheetCategories>]
		V								  |     +<- Data [SheetCategories]
		SheetCategories					  |         +<- TreeBase [TreeNode]
		V								  |
		TreeNode (TreeBase)				  |


		TreeManager
			- provides routines to manipulate the tree and elements
		^
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

	*/


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
	#region private fields

		private SheetCategoryDataManager categories = new SheetCategoryDataManager();
		private TreeNode userSelected;
		private TreeNode contextSelected;
		private TreeNode contextSelectedParent;

		public string ContextCmdDelete {get;}            = "delete";
		public string ContextCmdAddChild {get;}          = "addChild";
		public string ContextCmdAddBefore {get;}         = "addBefore";
		public string ContextCmdAddAfter {get;}          = "addAfter";
		public string ContextCmdMoveAsChild {get;}       = "moveAsChild";
		public string ContextCmdMoveBefore {get;}        = "moveBefore";
		public string ContextCmdMoveAfter {get;}         = "moveAfter";
		public string ContextCmdCopy {get;}              = "clone";
		public string ContextCmdCopyAsChild {get;}       = "cloneAsChild";
		public string ContextCmdExpand {get;}            = "Expand";
		public string ContextCmdCollapse {get;}          = "Collapse";

		private bool bypassContextDeHighlight = false;

	#endregion

	#region ctor

		static MainWindow()
		{
			SampleData sd = new SampleData();
			sd.Sample(BaseOfTreeRoot);

			sd.SampleFiles(FileList2);



		}

		

		public MainWindow()
		{
			InitializeComponent();

		}

	#endregion

	#region public properties

		public static BaseOfTree BaseOfTreeRoot { get; set; } = new BaseOfTree();
//		public static TreeNode TreeRoot { get; set; } = new TreeNode();

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

		public static SampleFileList FileList2 { get; private set; } = new SampleFileList();
//		public static SampleFileList FileList2 { get; private set; } = new SampleFileList(
//			@"C:\2099-999 Sample Project\Publish\Bulletins\2017-07-01 arch only\Individual PDFs");
		public SampleFileList FileList { get; private set; }


//		public string PatternHintText { get; private set; } = "Regex pattern";

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

			categories.Configure(UserSettings.Data.FileNameCategoryFolder,
				UserSettings.Data.FileNameCategoryFile);
			
//			categories.Configure(@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor",
//				"SheetCategories.xml");
//
//			SampleData sd = new SampleData();
//
//			sd.Sample(categories.TreeBase);
//
//			categories.Write();

			categories.Read();

			FileList = new SampleFileList(UserSettings.Data.FileNameCategoryFolder);

			OnPropertyChange("FileList");
		}

		private void MainWin_Closing(object sender, CancelEventArgs e)
		{
			if (categories.IsModified == true)
			{
				MessageBoxResult result = MessageBox.Show(
					"There are changes that have not been saved\n"
					+ "Do you want to quit and lose your changes?",
					"Classifier Editor", MessageBoxButton.YesNo,
					MessageBoxImage.Warning);

				if (result != MessageBoxResult.Yes)
				{
					e.Cancel = true;
				}
			}
		}

	#endregion

	#region control event methods

		// when a selection has been made
		private void Tv1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			UserSelected = (TreeNode) e.NewValue;
			BaseOfTreeRoot.SelectedNode = userSelected;
//			PatternHintText = "";
		}


		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{

		}

		private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			categories.IsModified = true;
		}

		private void TbxPattern_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			categories.IsModified = true;
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

//			contextSelected.AddNewChild();

			BaseOfTreeRoot.AddNewChild2(contextSelected);

			contextSelected.IsExpanded = true;

			ContextDeselect();
		}

		private void Tv1ContextMenuAddBefore_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

//			contextSelectedParent.AddNewBefore(contextSelected);

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

//			contextSelectedParent.MoveBefore(UserSelected, contextSelected);

			BaseOfTreeRoot.MoveBefore(contextSelected, userSelected);

			BaseOfTreeRoot.SelectedNode.IsSelected = false;

			ContextDeselect();
		}

		private void Tv1ContextMenuMoveAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

//			contextSelectedParent.MoveAfter(UserSelected, ContextSelected);

			BaseOfTreeRoot.MoveAfter(contextSelected, userSelected);

			BaseOfTreeRoot.SelectedNode.IsSelected = false;

			ContextDeselect();
		}

		private void Tv1ContextMenuMoveChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			BaseOfTreeRoot.MoveAsChild(contextSelected, userSelected);

			BaseOfTreeRoot.SelectedNode.IsSelected = false;

			ContextDeselect();
		}
		
		private void Tv1ContextMenuSelCopy_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			TreeNode newNode = userSelected.Clone() as TreeNode;

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

		private void BtnSave_OnClick(object sender, RoutedEventArgs e)
		{
			categories.Write();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
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

	public class DetailRowTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;
			if (element != null && item != null && item is TreeNode)
			{
				TreeNode taskitem = item as TreeNode;

				if (taskitem.HasChildren)
					return
						element.FindResource("Dtx") as DataTemplate;
				else
					return
						element.FindResource("Dty") as DataTemplate;
			}

			return null;
		}
	}
}