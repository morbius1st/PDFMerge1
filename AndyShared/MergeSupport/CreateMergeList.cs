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
using AndyShared.FileSupport.SheetPDF;
using AndyShared.SampleFileSupport;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  9/29/2020 7:06:05 AM


namespace AndyShared.MergeSupport
{
	/// <summary>
	/// Process the list of PDF sheet files and classify<br/>
	/// based on the ClassifFile's Classification rules
	/// </summary>
	public class CreateMergeList
	{
	#region private fields

	#endregion

	#region ctor

		public CreateMergeList() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods


		/// <summary>
		/// Process the list of sheet files, using the rules in the<br/>
		/// ClassificationFile, and place the file into the<br/>
		/// Classification tree
		/// </summary>

		public bool Process(BaseOfTree treeBase, SheetFileList fileList)
		{
			// process each file in the list and categorize
			foreach (FilePath<FileNameSheetPdf> file in fileList.Files)
			{
				
			}


			return true;
		}

	#endregion

	#region private methods


		private void classify(TreeNode classfNode, FileNameSheetPdf sheetFile)
		{
			bool matchFlag = false;

			foreach (TreeNode childClassfNode in classfNode.Children)
			{
				// set flag = false;
				// scan through each node and determine if the sheetfile matches one of the nodes rules
				// if it matches:
				//    if there are no children treenodes, set flag = false
				//    if there are children, scan the next level
				//        if the next level returns true, sheetfile was classified in a lower level, set flag = return value == true
				//        if the next level return false (this will not happen)
				//
				// if flag = false, it does not match, add to this level's sheet category.mergeitems / return true (was classified)

				// does the current sheetfile match the current
				// if sheetfile matches childclassfnode.sheetcategory.compareops
				if (CompareOperations.Compare(sheetFile.Category, childClassfNode.Item.CompareOps))
				{
					
				}




			}

			if (!matchFlag)
			{
				// add to classfFile.sheetcategory.mergeitems
			}

		}



	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is CreateMergeList";
		}

	#endregion
	}
}