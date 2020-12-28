using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using SettingsManager;
using UtilityLibrary;
using AndyShared.FileSupport;


// all user classification files
// all stored on the local machine in
// a common folder which is a sub-folder
// of the machine setting file


namespace AndyShared.ClassificationFileSupport
{
	public class ClassificationFiles : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<ClassificationFiles> instance =
			new Lazy<ClassificationFiles>(() => new ClassificationFiles());

		private  ObservableCollection<ClassificationFile> userClassificationFiles =
			new ObservableCollection<ClassificationFile>();

		private ICollectionView userClassifFilesView;

		private string allClassfFolderPath;

		private string userClassfFolderPath;

	#endregion

	#region ctor

		public ClassificationFiles() { }

	#endregion

	#region public properties

		public static ClassificationFiles Instance => instance.Value;

		public bool Initialized { get; set; }

		/// <summary>
		/// folder path to the local classification file
		/// </summary>
		public string AllClassifFolderPath => allClassfFolderPath;

		public string UserClassfFolderPath => userClassfFolderPath;

		public  ObservableCollection<ClassificationFile> UserClassificationFiles
		{
			get => userClassificationFiles; 
			set
			{
				userClassificationFiles = value;
				OnPropertyChange();
			}
		}

		public ICollectionView View
		{
			get => userClassifFilesView;
			// mst be public
			set
			{
				userClassifFilesView = value;

				OnPropertyChange();
			}
		}

		public bool UserClassificationFolderPathExists => Directory.Exists(userClassfFolderPath);

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			if (Initialized) return;

			Initialized = true;

			OnPropertyChange(nameof(Initialized));

			allClassfFolderPath = MachSettings.Path.SettingFolderPath +
				FilePathConstants.USER_STORAGE_FOLDER;

			userClassfFolderPath = FilePathUtil.AssembleFolderPath(false,
				AllClassifFolderPath, Environment.UserName);

			Reinitialize();
		}

		public void Reinitialize()
		{
			UpdateCollection();

			UpdateProperties();
		}

		public void UpdateCollection()
		{
			userClassificationFiles = new ObservableCollection<ClassificationFile>();

			if (!GetFiles()) return;

			UpdateView();

			UpdateViewProperties();
		}

		// return the folderpath to the requested 
		// classification file
		public ClassificationFile Find(string userName, string fileId)
		{
			foreach (CollectionViewGroup viewGroup in View.Groups)
			{
				if (((ClassificationFile) viewGroup.Items[0]).UserName.Equals(userName))
				{
					foreach (ClassificationFile userCfg in viewGroup.Items)
					{
						if (userCfg.FileId.Equals(fileId))
						{
							return userCfg;
						}
					}
				}
			}

			return null;
		}

	#endregion

	#region private methods

		private bool GetFiles()
		{
			foreach (string file in Directory.EnumerateFiles(AllClassifFolderPath,
				FilePathConstants.USER_STORAGE_PATTERN, SearchOption.AllDirectories))
			{
				ClassificationFile userFile = new ClassificationFile(file);

				if (!userFile.IsValid) continue;

				userClassificationFiles.Add(userFile);
			}

			OnPropertyChange("UserClassificationFiles");

			return userClassificationFiles.Count > 0;
		}

		private void UpdateProperties()
		{
			OnPropertyChange("AllClassifFolderPath");
			OnPropertyChange("UserClassfFolderPath");
			                  
			OnPropertyChange("UserClassificationFolderPathExists");
		}

		private void UpdateViewProperties()
		{
			OnPropertyChange("UserClassificationFiles");
		}

		public void UpdateView()
		{
			userClassifFilesView = CollectionViewSource.GetDefaultView(userClassificationFiles);

			userClassifFilesView.SortDescriptions.Clear();
			userClassifFilesView.SortDescriptions.Add(new SortDescription("FileName", ListSortDirection.Ascending));

			userClassifFilesView.GroupDescriptions.Clear();
			userClassifFilesView.GroupDescriptions.Add(new PropertyGroupDescription("UserName"));

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

		public void OnSeedSiteCollectionUpdated(object sender)
		{
			UpdateCollection();
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ClassificationFiles";
		}

	#endregion
	}
}