#region using

using System;
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

	public class MergeItem : ICloneable
	{	
	#region private fields

	#endregion

	#region ctor
		public MergeItem(int numberOfPages, FilePath<FileNameSheetPdf> filePath)
		{
			NumberOfPages = numberOfPages;
			FilePath = filePath;
		}
	#endregion

	#region public properties

		public int                          PageNumber { get; set; }
		public int                          NumberOfPages { get; set; }
		public FilePath<FileNameSheetPdf>   FilePath { get; set; }

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MergeItem| " + FilePath.FileNameObject.SheetNumber + ":" + 
				FilePath.FileNameObject.SheetTitle;
		}

		public object Clone()
		{
			return new MergeItem(NumberOfPages, new FilePath<FileNameSheetPdf>(FilePath.FullFilePath));
		}

	#endregion
	}

}