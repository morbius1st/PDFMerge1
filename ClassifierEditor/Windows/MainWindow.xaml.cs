#region using

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using System.Windows;
using System.Windows.Controls;
using ClassifierEditor.NumberComponent;
using ClassifierEditor.Windows.Support;

#endregion

// projname: ClassifierEditor
// itemname: MainWindow
// username: jeffs
// created:  5/2/2020 9:16:20 AM

namespace ClassifierEditor.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
	#region private fields

		private TreeManager tm;

		private string selectedTitle;

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			TreeManager = new TreeManager();

		}

	#endregion

	#region public properties

		public static TreeManager Tmx { get; private set; } = new TreeManager();

		public TreeManager TreeManager
		{
			get => tm;
			set
			{
				tm = value;
				OnPropertyChange();
			}
		}

		public string SelectedTitle
		{
			get => selectedTitle;
			set
			{
				selectedTitle = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region control methods

		private void CheckBox_LostFocus5(object sender, RoutedEventArgs e)
		{
			CheckBox cbx = (CheckBox) sender;

			TreeNode node = (TreeNode) cbx.DataContext;

			node.TriStateReset();
		}

		private void BtnRowDetailDone_OnClick(object sender, RoutedEventArgs e)
		{
			Button b = (Button) sender;

			DataGridRow r = (DataGridRow) b.Tag;

			r.IsSelected = false;
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Dg1.CurrentCell = new DataGridCellInfo(Dg1.Items[0], Dg1.Columns[1]);

			Dg1.BeginEdit();

			Debug.WriteLine("at debug");
		}

		private void DataGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			TextBox tbx = (TextBox) e.EditingElement;

			SelectedTitle = (tbx.Text as string) ?? "got null";

		}

		private void DataGrid_OnPreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
		{
			TextBox tbx = (TextBox) e.EditingElement;

			SelectedTitle = (tbx.Text as string) ?? "got null";
		}

		private CheckBox priorCheckbox = null;
		private DataGridRow priorRow = null;

		private void CbxExpander_OnClick(object sender, RoutedEventArgs e)
		{
			// the current values
			CheckBox checkbox = (CheckBox) sender;

			if (priorCheckbox != null)
			{
				priorRow.DetailsVisibility = Visibility.Collapsed;

				if (checkbox != priorCheckbox) priorCheckbox.IsChecked = false;
			}

			priorCheckbox = checkbox;
			priorRow = (DataGridRow) checkbox.Tag;

			DetailVisibility(priorCheckbox, priorRow);
		}

		private void DetailVisibility(CheckBox cbx, DataGridRow row)
		{
			row.DetailsVisibility =
				cbx.IsChecked == true
					? Visibility.Visible
					: Visibility.Collapsed;

		}

		private void BtnAddDetailRow_OnClick(object sender, RoutedEventArgs e)
		{
			DataGridRow r = (DataGridRow) ((Button) sender).Tag;

		}


	#endregion


	#region event handeling

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			TreeManager = new TreeManager();
		}

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