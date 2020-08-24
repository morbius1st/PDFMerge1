#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  8/17/2020 6:17:26 AM

namespace AndyShared.SampleFileSupport
{
	public class SampleFile : INotifyPropertyChanged
	{
		public const string SAMPLE_FILE_EXT = "sample";
		public const string SAMPLE_FOLDER = "Sample Files";

	#region private fields

		private FilePath<FileNameSimple> sampleFilePath = FilePath<FileNameSimple>.Invalid;

#pragma warning disable CS0169 // The field 'SampleFile.isSelected' is never used
		private bool isSelected;
#pragma warning restore CS0169 // The field 'SampleFile.isSelected' is never used

	#endregion

	#region ctor

		public SampleFile(string classfFilePath)
		{

			SampleFilePath = FullPathFromFile(classfFilePath);

			// bool v = sampleFilePath.IsValid;
		}

	#endregion

	#region public properties

		public bool IsValid => sampleFilePath?.IsValid ?? false;

		public FilePath<FileNameSimple> SampleFilePath
		{
			get => sampleFilePath;
			set
			{
				sampleFilePath = value;

				OnPropertyChange();
			}
		}

		public string SampleFileFullFilePath => sampleFilePath.FullFilePath;


	#endregion

	#region private properties

	#endregion

	#region public methods

		// read the sample filename (no extension) from the file
		public static string FileNameFromFile(string classifFilePath)
		{
			string fileName =
				CsUtilities.ScanXmlForElementValue(classifFilePath, "SampleFile", 0);

			return fileName;

		}

		public FilePath<FileNameSimple> FullPathFromFile(string classfFilePath)
		{
			// this is just the filename + extension
			string sampleFileName = FileNameFromFile(classfFilePath);

			if (sampleFileName.IsVoid())
			{
				return FilePath<FileNameSimple>.Invalid;
			}

			// FilePath<FileNameSimple> samplePath = new FilePath<FileNameSimple>();
			FilePath<FileNameSimple> samplePath = DeriveFolderPath(classfFilePath);

			// samplePath.Up();
			samplePath.ChangeFileName(sampleFileName, SAMPLE_FILE_EXT);

			return samplePath;
		}

		public FilePath<FileNameSimple> DeriveFolderPath(string classfFilePath)
		{
			FilePath<FileNameSimple> sampleFolderPath = new FilePath<FileNameSimple>(classfFilePath);

			sampleFolderPath.Down((SAMPLE_FOLDER));

			return sampleFolderPath;
		}

	#endregion

	#region private methods

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleFile";
		}

	#endregion
	}
}