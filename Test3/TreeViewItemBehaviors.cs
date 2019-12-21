#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

#endregion


// projname: Test3
// itemname: TreeViewItemBehaviors
// username: jeffs
// created:  12/20/2019 7:19:45 PM


namespace Test3
{
	public static class TreeViewItemBehaviors
	{
	#region IsBroughtIntoViewWhenSelected

		public static bool GetIsBroughtIntoViewWhenSelected(TreeViewItem treeViewItem)
		{
			MainWindow.Instance.AppendLineTbk2("\nbring into view - got value");

			return (bool) treeViewItem.GetValue(IsBroughtIntoViewWhenSelectedProperty);
		}

		public static void SetIsBroughtIntoViewWhenSelected(
			TreeViewItem treeViewItem, bool value
			)
		{
			treeViewItem.SetValue(IsBroughtIntoViewWhenSelectedProperty, value);
		}

		public static readonly DependencyProperty IsBroughtIntoViewWhenSelectedProperty =
			DependencyProperty.RegisterAttached(
				"IsBroughtIntoViewWhenSelected",
				typeof(bool),
				typeof(TreeViewItemBehaviors),
				new UIPropertyMetadata(false, OnIsBroughtIntoViewWhenSelectedChanged));

		static void OnIsBroughtIntoViewWhenSelectedChanged(
			DependencyObject depObj, DependencyPropertyChangedEventArgs e
			)
		{
			TreeViewItem item = depObj as TreeViewItem;
			if (item == null)
				return;

			if (e.NewValue is bool == false)
				return;

			if ((bool) e.NewValue)
				item.Selected += OnTreeViewItemSelected;
			else
				item.Selected -= OnTreeViewItemSelected;

			MainWindow.Instance.AppendLineTbk2("\nbring into view - event set");
		}

		static void OnTreeViewItemSelected(object sender, RoutedEventArgs e)
		{
			// Only react to the Selected event raised by the TreeViewItem
			// whose IsSelected property was modified. Ignore all ancestors
			// who are merely reporting that a descendant's Selected fired.
			if (!Object.ReferenceEquals(sender, e.OriginalSource))
				return;

			MainWindow.Instance.AppendLineTbk2("\nbring into view?");

			TreeViewItem item = e.OriginalSource as TreeViewItem;
			if (item != null)
			{
				item.BringIntoView();
				MainWindow.Instance.AppendLineTbk2("brought into view\n");
			}
		}

	#endregion // IsBroughtIntoViewWhenSelected

	}
}
