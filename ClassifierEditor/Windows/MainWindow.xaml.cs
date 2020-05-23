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
using ClassifierEditor.Tree;
using ClassifierEditor.Windows.Support;

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
		private TreeManager tm = new TreeManager();
		private TreeNode userSelected;

	#endregion

	#region ctor

		static MainWindow()
		{
			SampleData sd = new SampleData();
			sd.Sample(RootNode);

		}

		public MainWindow()
		{
			InitializeComponent();
		}

	#endregion

	#region public properties

		public static TreeNode RootNode { get; set; } = new TreeNode();

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
				tm.ContextDehighlight();

				userSelected = value;
				OnPropertyChange();
			}
		}

//		public TreeNode ContextSelected
//		{
//			get => contextSelected;
//			private set
//			{
//				contextSelected = value;
//				OnPropertyChange();
//			}
//		}

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
			categories.Configure(@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor",
				"SheetCategories.xml");

			SampleData sd = new SampleData();

			sd.Sample(categories.TreeBase);
			
			categories.Write();

//			categories.Read();
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

		private void Tv1ContextMenu_OnOpened(object sender, RoutedEventArgs e)
		{
			// sender is contextmenu
			// sender.datacontect is the treenode

			tm.ContextSelected = (TreeNode) ((ContextMenu) sender).DataContext;
		}

		private void Tv1ContextMenu_OnClosed(object sender, RoutedEventArgs e)
		{
			// sender is contextmenu
			// sender.datacontect is the treenode

			tm.ContextDehighlight();
		}


		// when a selection has been made
		private void Tv1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			UserSelected = (TreeNode) e.NewValue;
		}

		private void Tv1ContextMenuDelete_OnClick(object sender, RoutedEventArgs e)
		{
//			string command =
//				((MenuItem) sender).CommandParameter as string;
			tm.ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			tm.DeleteNode(tm.ContextSelected);
		}

		private void Tv1ContextMenuAddChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			tm.ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

//			categories.AddChildNode(contextSelected, SheetCategory.TempSheetCategory());

//			TreeNode newNode = new TreeNode(contextSelected, SheetCategory.TempSheetCategory(), false);

			tm.AddChild(tm.ContextSelected);
		}

		private void Tv1ContextMenuAddBefore_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			tm.ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			tm.AddNodeBefore(tm.ContextSelected);
		}

		private void Tv1ContextMenuAddAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			tm.ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			tm.AddNodeAfter(tm.ContextSelected);
		}

		private void Tv1ContextMenuMoveBefore_OnClick(object sender, RoutedEventArgs e)
		{
			tm.ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			tm.MoveNodeBefore(UserSelected, tm.ContextSelected);
		}
		
		private void Tv1ContextMenuMoveAfter_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			tm.ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			tm.MoveNodeAfter(UserSelected, tm.ContextSelected);
		}
		
		private void Tv1ContextMenuMoveChild_OnClick(object sender, RoutedEventArgs e)
		{
			// add a child to this leaf - also make a branch.
			tm.ContextSelected = (TreeNode) ((MenuItem) sender).DataContext;

			tm.MoveNodeChild(UserSelected, tm.ContextSelected);
			
		}

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
//
//		private void BtnFloatWin_OnClick(object sender, RoutedEventArgs e)
//		{
//			TreeViewSelector.TreeViewSelector tvs = new TreeViewSelector.TreeViewSelector(this, BtnFloatWin);
//			tvs.Orientation = WindowPosition.BOTTOM;
//			tvs.Categories = Categories;
//			tvs.ShowDialog();
//		}
//
//		private void BtnPopupWin_OnClick(object sender, RoutedEventArgs e)
//		{
//			TvSelect.IsOpen = true;
//		}
//
//		private void Tvz_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
//		{
//			TvSelect.IsOpen = false;
//		}
//
//		private void TvSelect_Opened(object sender, EventArgs e)
//		{
//			TbkPopupWinStatus.Text = "opened";
//		}
//
//		private void TvSelect_Closed(object sender, EventArgs e)
//		{
//			TbkPopupWinStatus.Text = "closed";
//		}

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

		//		public static readonly DependencyProperty HasChildrenProperty = DependencyProperty.RegisterAttached(
		//			"HasChildren",
		//			typeof(bool),
		//			typeof(MainWindow),
		//			new FrameworkPropertyMetadata(false, null)
		//			);
		//
		//		public static void SetHasChildren(UIElement element, bool value)
		//		{
		//			element.SetValue(HasChildrenProperty, value);
		//		}
		//
		//		public static bool GetHasChildren(UIElement element)
		//		{
		//			return (bool) element.GetValue(HasChildrenProperty);
		//		}



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