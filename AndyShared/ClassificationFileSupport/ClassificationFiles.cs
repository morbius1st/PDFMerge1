using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;
using AndyShared.ClassificationFileSupport;
using AndyShared.ConfigSupport;
using SettingsManager;
using UtilityLibrary;


// all user classification files
// all stored on the local machine in
// a common folder which is a sub-folder
// of the machine setting file


namespace AndyShared.ConfigMgrShared
{
	public class ClassificationFiles : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<ClassificationFiles> instance =
			new Lazy<ClassificationFiles>(() => new ClassificationFiles());

		private  ObservableCollection<ClassificationFile> userClassificationFiles =
			new ObservableCollection<ClassificationFile>();

		private ICollectionView userClassificationFilesView;

	#endregion

	#region ctor

		private ClassificationFiles() { }

	#endregion

	#region public properties

		public static ClassificationFiles Instance => instance.Value;


		public bool Initialized { get; set; }

		/// <summary>
		/// folder path to the local classification file
		/// </summary>
		// public string UserClassificationFolderPath => ConfigFileSupport.UserClassificationFolderPath;
		public string UserClassificationFolderPath => UserClassfFolderPath;

		public bool UserClassificationFolderPathExists => Directory.Exists(UserClassificationFolderPath);

		public  ObservableCollection<ClassificationFile> UserClassificationFiles
		{
			get => userClassificationFiles;
			private set
			{
				userClassificationFiles = value;
				OnPropertyChange();
			}
		}

		public ICollectionView View
		{
			get => userClassificationFilesView;
			// mst be public
			set
			{
				userClassificationFilesView = value;

				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

		private string UserClassfFolderPath => MachSettings.Path.SettingFolderPath +
			ClassificationFileAssist.USER_STORAGE_FOLDER;

	#endregion

	#region public methods

		public void Initialize()
		{
			Initialized = true;

			UpdateCollection();

			OnPropertyChange("Initialized");

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
			foreach (string file in Directory.EnumerateFiles(UserClassificationFolderPath,
				ClassificationFileAssist.USER_STORAGE_PATTERN, SearchOption.AllDirectories))
			{
				ClassificationFile userFile =
					new ClassificationFile(file);

				if (!userFile.IsValid) continue;

				userClassificationFiles.Add(userFile);
			}

			return userClassificationFiles.Count > 0;
		}

		private void UpdateProperties()
		{
			OnPropertyChange("UserClassificationFolderPath");
			OnPropertyChange("UserClassificationFolderPathExists");
		}

		private void UpdateViewProperties()
		{
			OnPropertyChange("UserClassificationFiles");
			OnPropertyChange("View");
		}

		private void UpdateView()
		{
			View = CollectionViewSource.GetDefaultView(userClassificationFiles);

			View.SortDescriptions.Clear();
			View.SortDescriptions.Add(new SortDescription("FileName", ListSortDirection.Ascending));

			View.GroupDescriptions.Clear();
			View.GroupDescriptions.Add(new PropertyGroupDescription("UserName"));
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
			return "this is ConfigSeedLocal";
		}

	#endregion
	}
}