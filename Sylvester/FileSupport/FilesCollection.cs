﻿#region + Using Directives
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UtilityLibrary;

#endregion


// projname: Sylvester.FileSupport
// itemname: FilesCollection
// username: jeffs
// created:  1/4/2020 10:20:45 PM

namespace Sylvester.FileSupport
{
	public class FilesCollection<T> : INotifyPropertyChanged where T : SheetNameInfo, new()
	{		
		private ObservableCollection<T> files;
		private string name;
		private FilePath<FileNameSimple> folder = FilePath<FileNameSimple>.Invalid;
		private int nonSheetPdfsFiles;
		private int otherFiles;
		private bool hasFolder;

		public FilesCollection()
		{
			Files = new ObservableCollection<T>();
		}

		public ObservableCollection<T> Files
		{
			get => files;
			private set
			{
				files = value;
				OnPropertyChange();
			}
		}

		public  string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChange();
			}
		}

		public FilePath<FileNameSimple> Folder
		{
			get => folder;
			set
			{
				folder = value;
				OnPropertyChange();

				Files = new ObservableCollection<T>();

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

		public int FilesFound => Files.Count;

		public int SheetPdfs
		{
			get => FilesFound - NonSheetPdfsFiles - OtherFiles; 
		}

		public int SelectedCount {
			get
			{ 
				int n= Files.Count(info => info.Selected);

				return n;
			}
	}

		public void Add(T tf)
		{
			Files.Add(tf);

			OnPropertyChange("FilesFound");
			OnPropertyChange("SheetPdfs");

		}

		public void Add(FilePath<FileNameSimple> r, bool preselect)
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
			foreach (T tf in Files)
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
			Files.Clear();
			Folder = FilePath<FileNameSimple>.Invalid;
			NonSheetPdfsFiles = 0;
			OtherFiles = 0;

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("Files");
			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("SheetPDFs");
			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("FilesFound");

		}

		public void Initialize()
		{
			Files.Clear();
			NonSheetPdfsFiles = 0;
			OtherFiles = 0;

			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("Files");
			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("SheetPDFs");
			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChange("FilesFound");

		}

		public void Update()
		{
			OnPropertyChange("SelectedCount");
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