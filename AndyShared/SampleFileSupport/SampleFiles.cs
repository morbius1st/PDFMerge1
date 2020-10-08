#region using directives

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using UtilityLibrary;
using AndyShared.FileSupport;

#endregion

// username: jeffs
// created:  8/17/2020 6:17:55 AM

namespace AndyShared.SampleFileSupport
{
	/// <summary>
	/// Creates a list of the user's sample files
	/// </summary>
	public class SampleFiles : INotifyPropertyChanged
	{
		public const string SAMPLE_CBX_FIRST_ITEM = "**FirstItem**";
	
	#region private fields
	
		private static readonly Lazy<SampleFiles> instance =
			new Lazy<SampleFiles>(() => new SampleFiles());
	
		private  ObservableCollection<SampleFile> userSampleFiles =
			new ObservableCollection<SampleFile>();
	
		private ICollectionView sampleFilesView;
	
		private string sampleFileFolderPath;
	
	#endregion
	
	#region ctor
	
		public SampleFiles() { }
	
	#endregion
	
	#region public properties
	
		public static SampleFiles Instance => instance.Value;
	
		public bool Initialized { get; set; }
	
		public ObservableCollection<SampleFile> UserSampleFiles => userSampleFiles;
	
		public string SampleFileFolderPath => sampleFileFolderPath;
	
		public ICollectionView View
		{
			get => sampleFilesView;
	
			set
			{
				sampleFilesView = value;
	
				OnPropertyChange();
			}
		}
	
	#endregion
	
	#region private properties
	
	#endregion
	
	#region public methods
	
		public void Initialize(string userClassfFolderPath)
		{
			if (userClassfFolderPath.IsVoid()) return;
	
			if (Initialized) return;
	
			Initialized = true;
	
			OnPropertyChange("Initialized");
	
			sampleFileFolderPath = SampleFileAssist.GetSampleFolderPath(userClassfFolderPath);
	
			UpdateCollection();
		}
	
		public void reinitialize()
		{
			UpdateCollection();
	
			UpdateProperties();
		}
	
		public void UpdateCollection()
		{
			InitializeCollection();
	
			if (!GetFiles()) return;
	
			UpdateView();
	
			UpdateViewProperties();
		}
	
	#endregion
	
	#region private methods
	
		private void InitializeCollection()
		{
			userSampleFiles = new ObservableCollection<SampleFile>();
	
			SampleFile sampleFile = new SampleFile();
	
			sampleFile.SampleFilePath = FilePath<FileNameSimple>.Invalid;
	
			// sampleFile.SampleFilePath.MakeFilePathInfo("Sample file\\No Sample File Assigned.txt");
			sampleFile.SampleFilePath.FileNameObject.FileNameNoExt = "No Sample File Assigned";
	
			sampleFile.SortName = SAMPLE_CBX_FIRST_ITEM;
	
			userSampleFiles.Add(sampleFile);
		}
	
		private bool GetFiles()
		{
			foreach (string file in Directory.EnumerateFiles(SampleFileFolderPath, 
				SampleFileAssist.SAMPLE_FILE_PATTERN, SearchOption.TopDirectoryOnly))
			{
				// SampleFile sampleFile = new SampleFile(file);
				SampleFile sampleFile = new SampleFile();
	
				sampleFile.InitializeFromSampleFilePath(file);
	
				sampleFile.SortName = sampleFile.FileName;
	
				userSampleFiles.Add(sampleFile);
	
			}
	
			OnPropertyChange("UserSampleFiles");
			
			return userSampleFiles.Count > 0;
		}
	
		private void UpdateProperties()
		{
			OnPropertyChange("SampleFileFolderPath");
		}
	
		private void UpdateView()
		{
			sampleFilesView = CollectionViewSource.GetDefaultView(userSampleFiles);
	
			sampleFilesView.SortDescriptions.Clear();
			sampleFilesView.SortDescriptions.Add(
				new SortDescription(FilePathConstants.SORT_NAME_PROP, ListSortDirection.Ascending));
	
			OnPropertyChange("View");
		}
	
		public void UpdateViewProperties()
		{
			OnPropertyChange("UserSampleFiles");
			OnPropertyChange("View");
			
		}
	
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
			return "this is SampleFiles";
		}
	
	#endregion
	}
}