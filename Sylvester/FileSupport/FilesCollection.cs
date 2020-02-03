#region + Using Directives
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion


// projname: Sylvester.FileSupport
// itemname: FilesCollection
// username: jeffs
// created:  1/4/2020 10:20:45 PM

namespace Sylvester.FileSupport
{
	public class FilesCollection<T> : INotifyPropertyChanged where T : SheetNameInfo, new()
	{
		private string name;
		private Route folder = Route.Invalid;
		private int nonSheetPdfsFiles;
		private int otherFiles;
		private bool hasFolder;

		public FilesCollection()
		{
			TestFiles = new ObservableCollection<T>();
		}


		public ObservableCollection<T>
			TestFiles { get; private set; }


		public  string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChange();
			}
		}

		public Route Folder
		{
			get => folder;
			set
			{
				folder = value;
				OnPropertyChange();

				HasFolder = folder != null && folder.IsValid;
			}
		}

		public bool HasFolder
		{
			get => hasFolder;
			set
			{
				hasFolder = value;
				OnPropertyChange();
			}
		}

		public int NonSheetPdfsFiles
		{
			get => nonSheetPdfsFiles;
			set
			{
				nonSheetPdfsFiles = value;
				OnPropertyChange();
			}
		}

		public int OtherFiles
		{
			get => otherFiles;
			set
			{
				otherFiles = value;
				OnPropertyChange();
			}
		}

		public int FilesFound => TestFiles.Count;

		public int SheetPdfs
		{
			get => FilesFound - NonSheetPdfsFiles - OtherFiles; 
		}

		public void Add(T tf)
		{
			TestFiles.Add(tf);

			OnPropertyChange("FilesFound");
			OnPropertyChange("SheetPdfs");

		}

		public void Add(Route r, bool preselect)
		{
			T tf = new T();

			tf.PreSelect = preselect;

			tf.FullFileRoute = r;

			tf.Initalize();

			switch (tf.FileType)
			{
				case FileType.NON_SHEET_PDF:
					NonSheetPdfsFiles++;
					break;
				case FileType.OTHER:
					OtherFiles++;
					break;
			}
			Add(tf);
		}

		public T ContainsKey(string findKey)
		{
			foreach (T tf in TestFiles)
			{
				if (tf.AdjustedSheetId == findKey)
				{
					return tf;
				}
			}

			return null;
		}

		public void Reset()
		{
			TestFiles.Clear();
			Folder = Route.Invalid;
			NonSheetPdfsFiles = 0;
			OtherFiles = 0;

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("TestFiles");
			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("SheetPDFs");
			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("FilesFound");

		}

		public override string ToString()
		{
			return Name;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		// ReSharper disable once InconsistentNaming
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}