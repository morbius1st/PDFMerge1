#region using

#endregion

// username: jeffs
// created:  8/16/2020 9:25:34 PM

using UtilityLibrary;

namespace AndyShared.ClassificationFileSupport
{
	// assist methods associated with Sample Files
	public class SampleFileAssist
	{
	#region private fields

	#endregion

	#region ctor

		public SampleFileAssist() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static string SampleFileNameFromFile(string classifFilePath) => 
			CsUtilities.ScanXmlForElementValue(classifFilePath, "SampleFile", 1);

		public static string GetFullPath(string classfFilePth)
		{
			string sampleFileName = SampleFileNameFromFile(classfFilePth);

			if (sampleFileName.IsVoid()) return null;

			FilePath<FileNameSimple> classFilePath = new FilePath<FileNameSimple>(classfFilePth);



			return "";
		}

	#endregion

	#region private methods

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleFileAssist";
		}

	#endregion
	}
}