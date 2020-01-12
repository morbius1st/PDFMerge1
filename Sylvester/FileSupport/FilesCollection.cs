#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

#endregion


// projname: Sylvester.FileSupport
// itemname: FilesCollection
// username: jeffs
// created:  1/4/2020 10:20:45 PM

namespace Sylvester.FileSupport
{
	public class FilesCollection<T> : INotifyPropertyChanged where T : SheetId, new()
	{
		public const string FILE_TYPE_EXT = ".pdf";

		private string name;
		private Route directory = Route.Invalid;
		private int nonSheetPdfsFiles = 0;
		private int otherFiles = 0;

		public  string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChange();
			}
		}

		public Route Directory
		{
			get => directory;
			set
			{
				directory = value;
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

		public int SheetPDFs
		{
			get => FilesFound - NonSheetPdfsFiles - OtherFiles; 
		}

		public ObservableCollection<T>
			TestFiles { get; private set; }

		public FilesCollection()
		{
			TestFiles = new ObservableCollection<T>();
		}

		public void Add(T tf)
		{
			TestFiles.Add(tf);
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
				if (tf.AdjustedSheetID == findKey)
				{
					return tf;
				}
			}

			return null;
		}

		public void Reset()
		{
			TestFiles.Clear();
			Directory = Route.Invalid;
			NonSheetPdfsFiles = 0;
			OtherFiles = 0;

			OnPropertyChange("TestFiles");
			OnPropertyChange("SheetPDFs");
			OnPropertyChange("FilesFound");

		}

		public override string ToString()
		{
			return Name;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}