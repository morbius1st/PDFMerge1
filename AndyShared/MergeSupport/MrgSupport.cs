#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndyShared.ClassificationDataSupport.TreeSupport;
using UtilityLibrary;


#endregion

// user name: jeffs
// created:   12/25/2020 6:59:25 AM

namespace AndyShared.MergeSupport
{
	public static class MrgSupport
	{
		
		public static IEnumerable<MergeItem> EnumerateMergeItems(BaseOfTree treeBase)
		{
			foreach (MergeItem mi in GetNtMergeItem(treeBase))
			{
				if (mi != null) yield return mi;
			}
		
			yield break;
		}
		
		public static IEnumerable<TreeNode> EnumerateMergeNodes(BaseOfTree treeBase)
		{
			foreach (TreeNode node in GetMergeNodes(treeBase))
			{
				if (node != null) yield return node;
			}
			yield break;
		}


		public static string FormatMergeList(TreeNode node)
		{
			if (node.ExtMergeItemCount == 0) return null;
		
			StringBuilder sb = new StringBuilder();
		
			sb.Append(" ".Repeat(node.Depth * 2));
			sb.Append("\\").AppendLine(node.Item.Title);
		
			if (node.ItemCount > 0)
			{
				foreach (MergeItem item in node.Item.MergeItems)
				{
					string shtnum = item.FilePath.FileNameObject.SheetNumber;
		
					if (!shtnum.IsVoid())
					{
						sb.Append(" ".Repeat(node.Depth * 2 + 4));
						sb.Append(">");
						sb.Append(item.FilePath.FileNameObject.SheetNumber);
						sb.AppendLine();
					}
				}
			}
		
			if (node.ChildCount > 0)
			{
				foreach (TreeNode childNode in node.Children)
				{
					string result = FormatMergeList(childNode);
		
					if (!result.IsVoid())
					{
						sb.Append(result);
					}
				}
			}
		
			return sb.Length > 0 ? sb.ToString() : null;
		}

		private	static IEnumerable<MergeItem> GetNtMergeItem(TreeNode node)
		{
			if (node.ExtMergeItemCount == 0) yield return null;
		
			if (node.ItemCount > 0)
			{
				foreach (MergeItem mi in node.Item.MergeItems)
				{
					yield return  mi;
				}
			}
		
			if (node.ChildCount > 0)
			{
				foreach (TreeNode childNode in node.Children)
				{
					if (childNode.ExtMergeItemCount > 0)
					{
						foreach (MergeItem mergeItem in GetNtMergeItem(childNode))
						{
							yield return mergeItem;
						}
					}
				}
			}
		
			yield return null;
		}

		private static IEnumerable<TreeNode> GetMergeNodes(TreeNode node)
		{
			if (node.ExtMergeItemCount == 0) yield return null;
		
			yield return node;
		
			if (node.ChildCount > 0)
			{
				foreach (TreeNode childNode in node.Children)
				{
					foreach (TreeNode mergeNode in GetMergeNodes(childNode))
					{
						if ((mergeNode?.ExtMergeItemCount ?? 0) > 0)
						{
							yield return mergeNode;
						}
					}
				}
			}
		
			yield return null;
		}
	}
}
