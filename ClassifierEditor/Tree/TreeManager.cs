#region using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

#endregion

// username: jeffs
// created:  5/2/2020 9:51:43 AM

namespace ClassifierEditor.Tree
{
	public class TreeManager : INotifyPropertyChanged
	{
	#region private fields

//		private TreeNode contextSelected;

	#endregion

	#region ctor

		public TreeManager()
		{
		}

	#endregion

	#region public properties

//		public TreeNode ContextSelected
//		{
//			get => contextSelected;
//			set
//			{
//				
//				if (value == contextSelected) return;
//
//				if (contextSelected != null)
//				{
//					contextSelected.IsContextSelected = false;
//					contextSelected.IsContextHighlighted = false;
//				}
//
//				// value is a reference to the collectionview
//				// convert to a reference in the children collection
//				contextSelected = value.Parent.Children[
//					value.Parent.Children.IndexOf(value)];
//
//				OnPropertyChange();
//
//				if (value == null) return;
//
//				contextSelected.IsContextSelected = true;
//				contextSelected.IsContextHighlighted = true;
//			}
//		}

	#endregion

	#region private properties

	#endregion

	#region public methods

//		public void ContextDeselect()
//		{
//			if (contextSelected != null)
//			{
//				contextSelected.IsContextSelected = false;
//				contextSelected.IsContextHighlighted = false;
//
//				contextSelected = null;
//			}
//		}
//
//		public void ContextDehighlight()
//		{
//			if (contextSelected != null)
//			{
//				contextSelected.IsContextHighlighted = false;
//			}
//		}

//
//		public bool AddChild(TreeNode selectedNode)
//		{
//			bool result = selectedNode.AddNode(TreeNode.TempTreeNode(selectedNode));
//			ContextDeselect();
//			return result;
//		}
//
////		public bool DeleteNode(TreeNode selectedNode)
////		{
////			bool result = selectedNode.DeleteNode(selectedNode);
////			ContextDeselect();
////			return result; 
////		}
//
//		public bool AddNodeBefore(TreeNode selectedNode)
//		{
//			bool result = selectedNode.AddBefore(TreeNode.TempTreeNode(selectedNode.Parent));
//			ContextDeselect();
//			return result; 
//		}
//
//		public bool AddNodeAfter(TreeNode selectedNode)
//		{
//			bool result = selectedNode.AddAfter(TreeNode.TempTreeNode(selectedNode.Parent));
//			ContextDeselect();
//			return result; 
//		}
//
//		public bool MoveNodeBefore(TreeNode existingNode, TreeNode selectedNode)
//		{
//			bool result = selectedNode.MoveBefore(existingNode, selectedNode);
//			ContextDeselect();
//			return result; 
//		}
//
//		public bool MoveNodeAfter(TreeNode existingNode, TreeNode selectedNode)
//		{
//			bool result = selectedNode.MoveAfter(existingNode, selectedNode);
//			ContextDeselect();
//			return result; 
//		}
//
//		public bool MoveNodeChild(TreeNode existingNode, TreeNode contectSelected)
//		{
//			// contextselected node is highlighted
//			// process - add node to new location
//			// delete the old node
//			// move the node as the first node
//			contectSelected.addNodeQuite(existingNode, 0.0f, NodePlacement.AFTER);
//
//			TreeNode parent = contectSelected.Parent;
//
//			existingNode.IsSelected = false;
//
//			parent.Children.Remove(existingNode);
//
//			contectSelected.ResequenceChildNodes();
//			contectSelected.NotifyChildrenChange();
//			
//			ContextDeselect();
//			return true; 
//		}
//
//

	#endregion

	#region private methods


	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is TreeManager";
		}

	#endregion
	}
}