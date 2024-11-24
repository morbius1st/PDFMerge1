#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.Support;
using JetBrains.Annotations;
using Microsoft.WindowsAPICodePack.Dialogs;

#endregion

// username: jeffs
// created:  10/14/2024 10:24:19 PM

namespace ClassifierEditor.Windows
{
	public class MwClassfEdSupport : INotifyPropertyChanged
	{
	#region private fields

		public int mwsIdx = 0;

		private MainWindowClassifierEditor mw;
		private TreeNode nodeEditing;
		private TreeNode nodeSelected;
		// private bool selNodeNeedsSaving;

		private static MwClassfEdSupport me;

		private List<TreeNode> invalidParentNode;
		private List<TreeNode> invalidParentItem;

	#endregion

	#region ctor

		public MwClassfEdSupport(MainWindowClassifierEditor mw)
		{
			this.mw = mw;

			mwsIdx = MainWindowClassifierEditor.objIdx++;

			me = this;
		}

	#endregion

	#region public properties

		/// <summary>
		/// the node selected in the treeview
		/// </summary>
		public TreeNode NodeSelected
		{
			get => nodeSelected;
			set
			{
				if (Equals(value, nodeSelected)) return;
				nodeSelected = value;
				OnPropertyChanged();

				mw.UpdateProperties2();
			}
		}

		/// <summary>
		/// the node used by the category editor
		/// </summary>
		public TreeNode NodeEditing
		{
			get => nodeEditing;
			set
			{
				nodeEditing = value;
				OnPropertyChanged();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void SelectNode(TreeNode node)
		{
			// if (node == null) return;

			NodeSelected = node;

			if (nodeSelected == null)
			{
				NodeEditing = null;
				return;
			}

			// Debug.WriteLine($"@ select node | {nodeSelected.Item.Title} is selected = {nodeSelected?.IsNodeSelected}");

			nodeSelected.IsNodeSelected = true;

			mw.BaseOfTree.SelectedNode = nodeSelected;

			// OnPropertyChanged(nameof(NodeSelected));

			nodeEditing = nodeSelected.Clone(1);

			OnPropertyChanged(nameof(NodeEditing));

		}

		public void DeleteNode(TreeNode node)
		{
			string msg;
			int extChildCount = node.ExtChildCount;

			if (extChildCount > 0)
			{
				msg = "a total of " + extChildCount + " categories "
					+ "and sub-categories.";
			}
			else
			{
				msg = "no categories or sub-categories.";
			}

			string title = node.Item.Title;

			TaskDialogResult result = CommonTaskDialogs.CommonWarningDialog(
				"Classifier Editor",
				"You are about to delete the category\n\"" + title + "\"",
				"The category \"" + title + "\" has " + msg + "\nIs it OK to delete this category?"
				, MainWindowClassifierEditor.WinHandle
				);


			if (result == TaskDialogResult.Ok)
			{
				mw.BaseOfTree.RemoveNode2(node);
			}
		}

		public void ValidateTree()
		{
			validateTree(mw.BaseOfTree, mw.BaseOfTree);
		}

		public void ValidateTree2()
		{
			invalidParentNode = new List<TreeNode>();
			invalidParentItem = new List<TreeNode>();

			validateTree2(mw.BaseOfTree, mw.BaseOfTree);

			showValidateWarningDialog();

		}

		public void RepairTree()
		{
			repairTree(mw.BaseOfTree, mw.BaseOfTree);
		}

	#endregion

	#region private methods

		private void validateTree(TreeNode trueParentNode, TreeNode node)
		{
			foreach (TreeNode childNode in node.Children)
			{
				if (childNode.HasChildren)
				{
					validateTree(childNode, childNode);
				}

				if (!childNode.Parent.Equals(trueParentNode))
				{
					Debug.Write($"child.parent vs. true parent mis-match found: node: {childNode.Item.Title}/{childNode.Item.Description} ({childNode.ID}) ");
					Debug.Write($"has parent {childNode.Parent.Item.Title}/{childNode.Parent.Item.Description} ({childNode.Parent.ID}) ");
					Debug.Write($"but should be {trueParentNode.Item.Title}/{trueParentNode.Item.Description} ({trueParentNode.Parent.ID})\n");
				}

				if (!childNode.Equals(childNode.Item.Parent))
				{
					Debug.WriteLine($"child / item.parent mis-match found: node: {childNode.Item.Title}/{childNode.Item.Description} ({childNode.ID}) ");
					Debug.WriteLine($"item has parent {childNode.Parent.Item.Parent.Item.Title}/{childNode.Parent.Item.Parent.Item.Description} ({childNode.Parent.Item.Parent.ID}) ");
					Debug.WriteLine($"but should match\n");
				}

			}
		}

		private void validateTree2(TreeNode trueParentNode, TreeNode node)
		{
			foreach (TreeNode childNode in node.Children)
			{
				if (childNode.HasChildren)
				{
					validateTree(childNode, childNode);
				}

				if (!childNode.Parent.Equals(trueParentNode))
				{
					invalidParentNode.Add(childNode);

					childNode.Parent = trueParentNode;
				}

				if (!childNode.Equals(childNode.Item.Parent))
				{
					invalidParentItem.Add(childNode);

					childNode.Item.Parent = childNode;

				}

			}
		}

		private void repairTree(TreeNode trueParentNode, TreeNode node)
		{
			foreach (TreeNode childNode in node.Children)
			{
				if (childNode.HasChildren)
				{
					repairTree(childNode, childNode);
				}

				if (!childNode.Parent.Equals(trueParentNode))
				{
					Debug.Write($"child.parent vs. true parent mis-match fixed: node: {childNode.Item.Title}/{childNode.Item.Description} ({childNode.ID})\n");

					childNode.Parent = trueParentNode;

				}

				if (!childNode.Equals(childNode.Item.Parent))
				{
					Debug.WriteLine($"child / item.parent mis-match fixed: node: {childNode.Item.Title}/{childNode.Item.Description} ({childNode.ID})\n");

					childNode.Item.Parent = childNode;
				}

			}
		}

		private void showValidateWarningDialog()
		{
			int count = invalidParentNode.Count + invalidParentItem.Count;

			if (count > 0)
			{
				CommonTaskDialogs.CommonWarningDialog(
					"Classifier Editor",
					"Intranode referencing errors were detected",
					$"A total of {count} node errors were found and repaired.\n"
					, MainWindowClassifierEditor.WinHandle
					);
			}

		}

	#endregion

	#region event consuming

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

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(MwClassfEdSupport)}";
		}

	#endregion
	}
}