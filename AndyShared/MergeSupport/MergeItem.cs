#region using

using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  9/26/2020 4:07:34 PM

namespace AndyShared.MergeSupport
{

	public class MergeItem
	{
		public int                             PageNumber { get; set; }
		public int                             NumberOfPages { get; set; }
		public FilePath<FileNameSheetPdf>   FilePath { get; set; }

		public MergeItem(int numberOfPages, FilePath<FileNameSheetPdf> filePath)
		{
			NumberOfPages = numberOfPages;
			FilePath = filePath;
		}
	}





	public class MergeItem2
	{

	#region private fields

		// internal string                       outlineTitle;
		internal NodeType                        nodeType;
		internal int                             pageNumber;
		internal int                             depth;
		internal FilePath<FileNameSimple>        filePath;
		internal Dictionary<string, MergeItem2>  mergeTreeBranch;

	#endregion

	#region ctor

		public MergeItem2(NodeType type, int depth) : this(type, null, 0, depth, null) { }

		public MergeItem2(NodeType type, Dictionary<string, MergeItem2> branch, 
			int page, int depth, FilePath<FileNameSimple> filepath)
		{
			nodeType = type;
			mergeTreeBranch = branch ?? new Dictionary<string, MergeItem2>();
			pageNumber = page;
			filePath = filepath;
			this.depth = depth;
		}

	#endregion

	#region public properties

		public int BranchCount => mergeTreeBranch?.Count ?? 0;

		public NodeType NodeType => nodeType;

		public bool HasChildNodes => BranchCount > 0;

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MergeItem";
		}

	#endregion
	}
}