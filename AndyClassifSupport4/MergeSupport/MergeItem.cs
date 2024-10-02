#region using

using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.FileSupport.FileNameSheetPDF4;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  9/26/2020 4:07:34 PM

namespace AndyShared.MergeSupport
{

	public class MergeItem
	{	
	#region private fields

	#endregion

	#region ctor
		public MergeItem(int numberOfPages, FilePath<FileNameSheetPdf4> filePath)
		{
			NumberOfPages = numberOfPages;
			FilePath = filePath;
		}
	#endregion

	#region public properties

		public int                          PageNumber { get; set; }
		public int                          NumberOfPages { get; set; }
		public FilePath<FileNameSheetPdf4>   FilePath { get; set; }

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MergeItem| " + FilePath.FileNameObject.SheetNumber + ":" + 
				FilePath.FileNameObject.SheetTitle;
		}

	#endregion
	}




}