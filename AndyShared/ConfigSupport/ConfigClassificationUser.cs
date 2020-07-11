using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;
using SettingsManager;
using UtilityLibrary;

namespace AndyShared.ConfigSupport
{
	public class ConfigClassificationUser : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<ConfigClassificationUser> instance =
			new Lazy<ConfigClassificationUser>(() => new ConfigClassificationUser());

		private  ObservableCollection<ConfigFileClassificationUser> userClassificationFiles =
			new ObservableCollection<ConfigFileClassificationUser>();

		private ICollectionView userClassificationFilesView;

	#endregion

	#region ctor

		private ConfigClassificationUser() {}

	#endregion

	#region public properties

		public static ConfigClassificationUser Instance => instance.Value;
		

		public bool Initialized { get; set; }

		/// <summary>
		/// path to the local seed folder
		/// </summary>
		public string UserClassificationFolderPath => ConfigFileSupport.UserClassificationFolderPath;

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

		// return the full filepath to the requested 
		// classification file
		public string Find(string userName, string fileId)
		{

			foreach (CollectionViewGroup viewGroup in View.Groups)
			{
				if (((ConfigFileClassificationUser)viewGroup.Items[0]).UserName.Equals(userName))
				{
					foreach (ConfigFileClassificationUser userCfg in viewGroup.Items)
					{
						if (userCfg.FileId.Equals(fileId))
						{
							return userCfg.GetFullFilePath;
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
