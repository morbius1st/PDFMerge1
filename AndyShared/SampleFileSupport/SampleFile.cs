#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilityLibrary;

using AndyShared.ClassificationFileSupport;

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

		private FilePath<FileNameSimple> sampleFilePath = 
			FilePath<FileNameSimple>.Invalid;

		private string sortName;

		private bool selected;

	#endregion

	#region ctor

		// public SampleFile()
		// {
		// }

	#endregion

	#region public properties

		public FilePath<FileNameSimple> SampleFilePath
		{
			get => sampleFilePath;
			set
			{
				sampleFilePath = value;

				OnPropertyChange();
			}
		}

		public const string SORT_NAME_PROP = "SortName";
		public string SortName { get; set; }

		/// <summary>
		/// FileName No Extension
		/// </summary>
		public string FileName => sampleFilePath.FileNameNoExt;

		public bool IsValid => sampleFilePath?.IsValid ?? false;

		public bool IsFound => sampleFilePath?.IsFound ?? false;

		public string FullFilePath => sampleFilePath?.FullFilePath;

		public bool Selected
		{
			get => selected;
			set
			{
				selected = value;

				OnPropertyChange();
			}

		}

		public string Description { get; private set; }

		// public string DescriptionFromFile => CsUtilities.ScanXmlForElementValue(FullFilePath, "Description", 0);

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool InitializeFromClassfFilePath(string classfFilePath)
		{
			SampleFilePath = ClassificationFileAssist.GetSampleFilePathFromFile(classfFilePath);

			return true;
		}

		public bool InitializeFromSampleFilePath(string filePathToSampleFile)
		{
			sampleFilePath = new FilePath<FileNameSimple>(filePathToSampleFile);

			Description = SampleFileAssist.DescriptionFromFile(filePathToSampleFile);

			return true;
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