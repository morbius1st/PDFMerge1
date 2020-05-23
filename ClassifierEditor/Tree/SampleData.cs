#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#endregion

// username: jeffs
// created:  5/2/2020 2:06:09 PM

namespace ClassifierEditor.Tree
{
	public class SampleData : INotifyPropertyChanged
	{
	#region private fields

	#endregion

	#region ctor

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Sample(TreeNode tn)
		{
			SheetCategory item = new SheetCategory("Tree Base", "Tree Base", null);

			tn.Item = item;

			MakeChildren(tn, 0);
		}

	#endregion

	#region private methods

		private static int MAX_DEPTH = 4;
		private static int TOPMAX = 15;
		private static int CHILDMAX = 5;
		private static int BRANCH = 0;

		private void MakeChildren(TreeNode parent, int depth)
		{
			int max = depth == 0 ? TOPMAX : CHILDMAX;

			if (depth >= MAX_DEPTH) return;

			for (int i = 0; i < max; i++)
			{
				if (depth == 0)
				{
					BRANCH = i;
				}

				SheetCategory item = new SheetCategory($"node title {BRANCH:D2}:{depth:D2}:{i:D2}", $"node description", @"(?<=[A-Z])([ -]+)(?=[0-9])");
				TreeNode node = new TreeNode(parent, item, false);

				if (i == 3 || i == 5)
				{
					// make a new branch
					// this branch is still associated with the parent branch
					// but its children are associated with the new branch

					MakeChildren(node, depth + 1);
				}

				parent.AddNode(node);
			}
		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleData";
		}

	#endregion
	}
}