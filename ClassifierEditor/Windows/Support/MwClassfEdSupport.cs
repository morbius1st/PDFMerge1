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
using JetBrains.Annotations;

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
		private TreeNode nodeCopy;
		private TreeNode nodeSelected;

	#endregion

	#region ctor

		public MwClassfEdSupport(MainWindowClassifierEditor mw)
		{
			this.mw = mw;

			mwsIdx = MainWindowClassifierEditor.objIdx++;
		}

	#endregion

	#region public properties

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

		public TreeNode NodeCopy
		{
			get => nodeCopy;
			set
			{
				nodeCopy = value;
				OnPropertyChanged();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void SelectNode(TreeNode node)
		{
			nodeSelected = node;

			if (nodeSelected == null || 
				(nodeSelected != null &&
				nodeSelected.Item.IsFixed ||
				nodeSelected.Item.IsLocked))
			{
				NodeSelected = null;
				NodeCopy = null;
				return;
			}


			nodeSelected.IsNodeSelected = true;

			mw.BaseOfTree.SelectedNode = nodeSelected;

			OnPropertyChanged(nameof(NodeSelected));

			nodeCopy = nodeSelected.Clone(false);

			OnPropertyChanged(nameof(NodeCopy));

		}


	#endregion

	#region private methods

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