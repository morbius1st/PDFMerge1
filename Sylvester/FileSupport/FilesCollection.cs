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

		public  string Name { get; set; }

		public Route Directory { get; set; } = Route.Invalid;

		public int NonSheetPdfsFiles { get; set; } = 0;
		public int OtherFiles { get; set; } = 0;
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

			if (tf.FullFileRoute.FileExtension.Equals(FILE_TYPE_EXT))
			{
				if (tf.ParseFile())
				{
					tf.FileType = FileType.SHEET_PDF;
				}
				else
				{
					NonSheetPdfsFiles++;
					tf.FileType = FileType.NON_SHEET_PDF;
				}
			}
			else
			{
				OtherFiles++;
				tf.FileType = FileType.OTHER;
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