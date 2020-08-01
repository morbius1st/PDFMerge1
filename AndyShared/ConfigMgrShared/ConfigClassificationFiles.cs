using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;
using AndyShared.ConfigSupport;
using SettingsManager;
using UtilityLibrary;


// all user classification files
// all stored on the local machine in
// a common folder which is a sub-folder
// of the machine setting file


namespace AndyShared.ConfigMgrShared
{
	public class ConfigClassificationFiles : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<ConfigClassificationFiles> instance =
			new Lazy<ConfigClassificationFiles>(() => new ConfigClassificationFiles());

		private  ObservableCollection<ConfigFileClassificationUser> userClassificationFiles =
			new ObservableCollection<ConfigFileClassificationUser>();

		private ICollectionView userClassificationFilesView;

	#endregion

	#region ctor

		private ConfigClassificationFiles() { }

	#endregion

	#region public properties

		public static ConfigClassificationFiles Instance => instance.Value;


		public bool Initialized { get; set; }

		/// <summary>
		/// folder path to the local classification file
		/// </summary>
		// public string UserClassificationFolderPath => ConfigFileSupport.UserClassificationFolderPath;
		public string UserClassificationFolderPath => UserClassfFolderPath;

		public bool UserClassificationFolderPathExists => Directory.Exists(UserClassificationFolderPath);

		public  ObservableCollection<ConfigFileClassificationUser> UserClassificationFiles
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
			ConfigFileSupport.USER_STORAGE_FOLDER;

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
			userClassificationFiles = new ObservableCollection<ConfigFileClassificationUser>();

			if (!GetFiles()) return;

			UpdateView();

			UpdateViewProperties();
		}

		// return the folderpath to the requested 
		// classification file
		public ConfigFileClassificationUser Find(string userName, string fileId)
		{
			foreach (CollectionViewGroup viewGroup in View.Groups)
			{
				if (((ConfigFileClassificationUser) viewGroup.Items[0]).UserName.Equals(userName))
				{
					foreach (ConfigFileClassificationUser userCfg in viewGroup.Items)
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
				ConfigFileSupport.USER_STORAGE_PATTERN, SearchOption.AllDirectories))
			{
				ConfigFileClassificationUser userFile =
					new ConfigFileClassificationUser(file);

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