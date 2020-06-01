#region using

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  5/25/2020 12:15:18 PM

namespace ClassifierEditor.FilesSupport
{
	public class SampleFileList: INotifyPropertyChanged
	{
		#region private fields

			private string fileName;


			public ObservableCollection<FilePath<FileNameSheetPdf>> Files { get; private set; }
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

			public void AddPath(FilePath<FileNameSheetPdf> path)
			{
				Files.Add(path);
				OnPropertyChange("Files");
			}



		#endregion

		#region private properties



		#endregion

		#region public methods

			public void GetFiles()
			{
//				Files = new ObservableCollection<FilePath<FileNameAsSheetFile>>();

				string pattern = "*.pdf";

				foreach (string file in
					Directory.EnumerateFiles(fileName, pattern,
						SearchOption.AllDirectories))
				{
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
