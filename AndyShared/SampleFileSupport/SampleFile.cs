#region using directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using AndyShared.ClassificationFileSupport;
using DebugCode;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  8/17/2020 6:17:26 AM

namespace AndyShared.SampleFileSupport
{
	public class SampleFile : INotifyPropertyChanged
	{
	#region private fields

		private FilePath<FileNameSimple> sampleFilePath = 
			FilePath<FileNameSimple>.Invalid;

		private bool selected;

	#endregion

	#region ctor

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

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool InitializeFromClassfFilePath(string classfFilePath)
		{
		#if DML1
			DM.InOut0();
		#endif
			SampleFilePath = ClassificationFileAssist.GetSampleFilePathFromFile(classfFilePath);

		#if DML1
			DM.End0();
		#endif

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
			return $"this is {FileName} ({SortName})";
		}

	#endregion
	}
}