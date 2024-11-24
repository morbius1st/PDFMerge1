#region using

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
using UtilityLibrary;
using System.Runtime.Serialization;

#endregion

// username: jeffs
// created:  9/26/2020 4:07:34 PM

namespace AndyShared.MergeSupport
{

	/* canceled
	public struct SheetCode
	{
		private const int DEFAULT_LENGTH = 2;
		private const string DEFAULT_SEPERATOR = ".";
		private const string DEFAULT_FILLER = " ";


		// the sort code object
		public ShtNumber ShtCode { get; set; }

		// max length of each sub-code
		public static List<int> MaxLength {get; set; }

		public string SortCode => sortCode();

		private string sortCode()
		{
			StringBuilder shtCode = new StringBuilder();

			string code = "";

			int count = (VI_SORT_SUFFIX - 1);

			shtCode.Append(fmtCode(ShtCode.ShtNumComps[0],0)).Append(DEFAULT_SEPERATOR);

			for (int i = 1; i < count; i+=2)
			{
				code = ShtCode.ShtNumComps[i];

				shtCode.Append(fmtCode(ShtCode.ShtNumComps[i],i));

				if (i < count - 1) shtCode.Append(DEFAULT_SEPERATOR);
			}

			if (!ShtCode.ShtNumComps[VI_SORT_SUFFIX].IsVoid())
			{
				shtCode.Append(DEFAULT_SEPERATOR);
				shtCode.Append(fmtCode(ShtCode.ShtNumComps[VI_SORT_SUFFIX],VI_SORT_SUFFIX));
			}

			return shtCode.ToString();
		}

		private string fmtCode(string code, int idx)
		{
			string result = code;

			int repeat = MaxLength[idx] == 0 || code.IsVoid()
				? DEFAULT_LENGTH
				: MaxLength[idx] - code.Length;

			if (repeat > 0) result = $"{DEFAULT_FILLER.Repeat(repeat)}{code}";

			return result;
		}
	}
	*/

	/* canceled
	public enum PropType
	{
		PT_OBJ,
		PT_STRING,
		PT_BOOL,
		PT_DOUBLE,
		PT_INT,
	}

	public struct PropertyMeta
	{
		public int PropIdx { get; set; }
		public string PropName { get; set; }
		public string PropDesc { get; set; }

		public PropType PropType { get; set; }

		public Object PropValue { get; set; }

	}
	*/


	[DataContract(Namespace = "")]
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

		[IgnoreDataMember]
		public int                          PageNumber { get; set; }
		
		[IgnoreDataMember]
		public int                          NumberOfPages { get; set; }
		
		[IgnoreDataMember]
		public FilePath<FileNameSheetPdf>   FilePath { get; set; }
		
		[IgnoreDataMember]
		public string SheetNumber			=> FilePath.FileNameObject.SheetNumber;

		[IgnoreDataMember]
		public int                          CompareCount { get; set; }

		[DataMember(Order = 1)]
		public PageProperties				PageProperties { get; set; }

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MergeItem| " + FilePath.FileNameObject.SheetNumber + ":" + 
				FilePath.FileNameObject.SheetTitle;
		}

		public object Clone()
		{
			MergeItem mi = new MergeItem(NumberOfPages, new FilePath<FileNameSheetPdf>(FilePath.FullFilePath));
			mi.PageProperties = PageProperties?.Clone() as PageProperties;
			
			return mi;
		}

	#endregion
	}

}