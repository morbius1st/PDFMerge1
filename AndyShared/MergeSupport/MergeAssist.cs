#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using AndyShared.SampleFileSupport;

#endregion

// username: jeffs
// created:  9/27/2020 6:39:16 AM


namespace AndyShared.MergeSupport
{
	/// <summary>
	/// mergeitem & mergetree support routines<br/>
	/// - creates the merge tree from based on the classfFile<br/>
	///   and a filelist
	/// </summary>
	public class MergeAssist
	{
	#region private fields

	#endregion

	#region ctor

		public MergeAssist() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		/// <summary>
		/// populate the mergetree
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="classfFile"></param>
		/// <param name="testFileList"></param>
		/// <returns></returns>
		public MergeTree MergeTreePopulate(ClassificationFile classfFile, 
			SampleFileList testFileList)
		{
			// classfFile defines the categories and the order
			// testfilelist is the list of items to add to the merge tree

			// step 1 - setup the merge tree with all categories
			MergeTree tree = mergeTreeSetup(classfFile);

			if (tree == null) return null;



			return new MergeTree();
		}


	#endregion

	#region private methods

		private MergeTree mergeTreeSetup(ClassificationFile classfFile)
		{
			if (classfFile.TreeBase.ChildCount == 0) return null;

			MergeTree tree = new MergeTree();

			// CreateTreeBranch(tree, classfFile.TreeBase, 0);

			return tree;
		}

		// private MergeTree CreateTreeBranch(MergeTree tree, TreeNode node, int depth)
		// {
		//
		// }

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MergeAssist";
		}

	#endregion
	}
}