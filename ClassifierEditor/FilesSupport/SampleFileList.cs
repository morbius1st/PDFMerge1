#region using

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
#pragma warning disable CS0246 // The type or namespace name 'UtilityLibrary' could not be found (are you missing a using directive or an assembly reference?)
using UtilityLibrary;
#pragma warning restore CS0246 // The type or namespace name 'UtilityLibrary' could not be found (are you missing a using directive or an assembly reference?)

#endregion

// username: jeffs
// created:  5/25/2020 12:15:18 PM

namespace ClassifierEditor.FilesSupport
{
	public class SampleFileList : INotifyPropertyChanged
	{
	#region private fields

		private string fileName;


#pragma warning disable CS0246 // The type or namespace name 'FilePath<>' could not be found (are you missing a using directive or an assembly reference?)
		public ObservableCollection<FilePath<FileNameSheetPdf>> Files { get; private set; }
#pragma warning restore CS0246 // The type or namespace name 'FilePath<>' could not be found (are you missing a using directive or an assembly reference?)
			= new ObservableCollection<FilePath<FileNameSheetPdf>>();

	#endregion

	#region ctor

		public SampleFileList()
		{
			OnPropertyChange("Files");
		}


		public SampleFileList(string fileName)
		{
			this.fileName = fileName;

			GetFiles();

			OnPropertyChange("Files");
		}

	#endregion

	#region public properties

#pragma warning disable CS0246 // The type or namespace name 'FilePath<>' could not be found (are you missing a using directive or an assembly reference?)
		public void AddPath(FilePath<FileNameSheetPdf> path)
#pragma warning restore CS0246 // The type or namespace name 'FilePath<>' could not be found (are you missing a using directive or an assembly reference?)
		{
			Files.Add(path);
			OnPropertyChange("Files");
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

//		public void GetFiles()
//		{
//			string pattern = "*.pdf";
//
//			foreach (string file in
//				Directory.EnumerateFiles(fileName, pattern,
//					SearchOption.AllDirectories))
//			{
//				Files.Add(new FilePath<FileNameSheetPdf>(file));
//			}
//		}

		public void GetFiles()
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}

			string file;

			StreamReader fileStream = new StreamReader(fileName);

			while ((file = fileStream.ReadLine()) != null)
			{
				file = file.Trim();

				if (file.Length == 0 || file.Substring(0,2).Equals(@"\\")) continue;

				Files.Add(new FilePath<FileNameSheetPdf>(file));
			}
		}

	#endregion

	#region private methods

	#endregion

	#region event processing

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleFileList";
		}

	#endregion
	}
}