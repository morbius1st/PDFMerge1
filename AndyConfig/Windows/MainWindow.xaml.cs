#region using

using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using AndyConfig.ConfigMgr;
using AndyConfig.FilesSupport;
using AndyShared.FilesSupport;
using ClassifierEditor.ConfigSupport;
using SettingsManager;
using UtilityLibrary;

#endregion

// projname: AndyConfig
// itemname: MainWindow
// username: jeffs
// created:  6/14/2020 5:30:07 PM


/*
 * config file hierarchy
 *
 * primary config file
 *  SuiteSettings
 *	+-> provides the path to the SiteSettings file
 *
 * purpose / operation
 * start from scratch
 * 1. create the SuiteSettings file
 * 2. get the path to the SiteSettings file from the user
 * 3. save the path to the setting file
 * 4. setup the LocalSeedFiles variable
 *		+-> use bogus info for first element
 * 5. save the path to the setting file
 * 6. verify that path for the SiteSettings is configured
 *		+-> that is, has the correct sub-folder for the seed files
 *		+-> has at least one seed file "xxx.orgseed"
 *
 * run after configuration
 * list info
 * -> path and name for the suite setting file
 * -> path and name for the site setting file
 * -> the seed files found
 * -> all of the UserOrganizationFiles stored
 * allow
 * -> (later) change SiteSettings location / reconfirm it is valid
 * -> (later) edit the UserOrganizationFiles list
 *
 * process
 * start
 * read (and possible create) the SuiteSettings file
 * got Site Path?
 * v   v
 * no  yes -> to validate SiteSettings file / info
 * v
 * get location from user
 * v          v
 * provided   not provided -> CannotProceed -> exit
 * v
 * has seed folder / files
 * v   v
 * no  yes -> info display mode / allow edit
 * v
 * copy seed files from install folder
 * v                      v
 * found / can proceed    not found -> CannotProceed -> exit
 * v
 * load into SiteSetting folder
 * v
 * config SiteSetting with seed file info
 * v
 *  info display mode / allow edit
 *
*/


namespace AndyConfig.Windows
{
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public static string TITLE = "Andy Configurator";

	#region private fields

		private const string SEED_PATTERN = @"*.seed.xml";

		private FolderAndFileSupport seedFiles = null;

		private ConfigManager cfgMgr = new ConfigManager();

		private FilePath<FileNameSimpleSelectable> selectedInstalledSeedFile;

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();
		}

	#endregion

	#region public properties

		public ConfigSuite Suite => cfgMgr.Suite;
		public ConfigSite Site => cfgMgr.Site;
		public ConfigSeed Seed => cfgMgr.Seed;
		public ConfigSeedInstalled SeedInstalled => cfgMgr.SeedInstalled;

		public string SiteRootPath => SuiteSettings.Data.SiteRootPath;

		public string SiteSettingFile => SiteSettings.Path.FileName;

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private bool Initalize()
		{
			SuiteSettings.Admin.Read();

			if (SuiteSettings.Data.SiteRootPath.IsVoid()) return false;

			SiteSettings.Path.RootPath = SuiteSettings.Data.SiteRootPath;

			SiteSettings.Admin.Read();

			return true;
		}

		private bool SetupConfiguration()
		{
			// step 1: request site setting folder
			string siteSettingFolder = GetSiteSettingFolder();

			if (siteSettingFolder == null)
			{
				CannotProceed("Without a folder for the site\n" +
					"settings, I cannot proceed.\n"
					+ "Exiting");
				return false;
			}

			// step 2: save the folder to the SuiteSettings file
			SuiteSettings.Data.SiteRootPath = siteSettingFolder;
			SuiteSettings.Admin.Write();

			// step 3: assign the SiteSetting file location
			SiteSettings.Path.RootPath = siteSettingFolder;

			// step 4: read (create) the SiteSetting file
			SiteSettings.Admin.Read();

			if (SiteSettings.Data.InstSeedFiles == null ||
				SiteSettings.Data.InstSeedFiles.Count == 0)
			{
				// step 5: setup the SiteSetting file / Seed File sub-folder / etc.
				if (!SetupSeedFiles()) return false;
			}

			return true;
		}

		// need to configure the SiteSetting file
		private bool SetupSeedFiles()
		{
			// initialize 
			if (SiteSettings.Data.InstSeedFiles == null)
			{
				SiteSettings.Data.InstSeedFiles = new Dictionary<string, ConfigSeedFileSetting>();
				SiteSettings.Admin.Write();
			}

			// configure
			seedFiles.Folder = new FilePath<FileNameSimpleSelectable>(SiteSettings.Path.SettingPath +
				@"\Seed Files");

			// step 6 does the seed directory exist?
			if (!seedFiles.FolderExists)
			{
				// folder does not exist
				// create folder and copy files from install
				// folder

				if (!MakeSeedFolder())
				{
					CannotProceed("Unable to create the seed folder. \n "
						+ "Without a folder for the seed\n" +
						"files, I cannot proceed.\n"
						+ "Exiting");
					return false;
				}
			}

			// seed older exists

			// step 7: check for files

			seedFiles.GetFiles();

			// step 8: does it have any seed files
			if (!seedFiles.HasFiles)
			{
				// nope - copy from install folder
				if (loadSeedFilesFromInstall() == -1)
				{
					CannotProceed("Unable to copy the seed files. \n "
						+ "into the seed folder in the Site\n" +
						"Settings folder. I cannot proceed.\n"
						+ "Exiting");
					return false;
				}

				StoreSeedFilesInSiteSettings();
				SiteSettings.Admin.Write();
			}

			return true;
		}

		private bool MakeSeedFolder()
		{
			DirectoryInfo di = Directory.CreateDirectory(seedFiles.Folder.GetFullPath);

			return di.Exists;
		}

		private bool StoreSeedFilesInSiteSettings()
		{
			seedFiles.GetFiles();

			foreach (FilePath<FileNameSimpleSelectable> file in seedFiles.FoundFiles)
			{
				string filename = file.GetFileNameWithoutExtension;
				string path = file.GetPath;

				string seedFile = file.GetFullPath;
				string datFile = path + @"\" + filename + ".dat";

				ConfigSeedFileSetting config =
					new ConfigSeedFileSetting(
						file.GetFileNameWithoutExtension, Heading.SuiteName,
						false, true, file.GetPath, file.GetFileNameWithoutExtension, "");

				string key = SiteSettings.Data.MakeKey(Heading.SuiteName, file.GetFileNameWithoutExtension);

				SiteSettings.Data.InstSeedFiles.Add(key, config);
			}

			return true;
		}

		private int loadSeedFilesFromInstall()
		{
		#if DEBUG
			string installSourceFolder =
				@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\.sample\Seed Files";
		#else
			string sourceFolder = Assembly.GetExecutingAssembly().Location + @"\Seed Files";
		#endif

			FolderAndFileSupport installSourceSeedFiles = new FolderAndFileSupport(installSourceFolder, SEED_PATTERN);

			if (!installSourceSeedFiles.FolderExists) return -1;

			installSourceSeedFiles.GetFiles();

			if (!installSourceSeedFiles.HasFiles) return -1;

			// gotten here:
			// the local seed folder exists
			// this folder has some files of the correct extension
			// need to copy files from install folder to seed folder
			// that is, from installFolder to seedFiles.Folder

			// source folder:
			// sourceFolder
			// destination folder:
			// seedfiles.folder.getfullpath

			return CopySeedFiles(installSourceSeedFiles);
		}

		private int CopySeedFiles(FolderAndFileSupport sourceSeedFiles)
		{
			// source folder:
			// sourceFolder
			// destination folder:
			// seedfiles.folder.getfullpath

			int count = 0;

			try
			{
				foreach (FilePath<FileNameSimpleSelectable> file in sourceSeedFiles.FoundFiles)
				{
					string sourceSeed = file.GetFullPath;
					string destSeed = seedFiles.Folder.GetFullPath + @"\" + file.GetFileName;

					string sourceDat = file.GetPath + @"\" + file.GetFileNameWithoutExtension + ".dat";

					string destDat = seedFiles.Folder.GetFullPath + @"\" + file.GetFileNameWithoutExtension + ".dat";

					if (!File.Exists(destSeed) && !File.Exists(destDat))
					{
						File.Copy(sourceSeed, destSeed);
						File.Copy(sourceDat, destDat);

						count++;
					}
				}
			}
			catch
			{
				// file processing error
				// failed
				return -1;
			}

			return count;
		}


		private string GetSiteSettingFolder()
		{
			using (CommonOpenFileDialog cfd = new CommonOpenFileDialog("Select Site Setting Folder - "
				+ TITLE))
			{
				cfd.IsFolderPicker = true;
				cfd.Multiselect = false;
				cfd.ShowPlacesList = true;
				cfd.AllowNonFileSystemItems = false;

				cfd.AllowPropertyEditing = false;

				CommonFileDialogResult	result = cfd.ShowDialog();

				if (result != CommonFileDialogResult.Ok)
				{
					return null;
				}

				return cfd.FileName;
			}

		#pragma warning disable CS0162 // Unreachable code detected
			return null;
		#pragma warning restore CS0162 // Unreachable code detected
		}

		private void CannotProceed(string message)
		{
			MessageBoxResult result = MessageBox.Show(message,
				"Setup Cannot Proceed",
				MessageBoxButton.OK,
				MessageBoxImage.Error);

			this.Close();
		}

	#endregion

	#region event processing

		private void BtnSeedSaveApply_OnClick(object sender, RoutedEventArgs e)
		{
			// Debug.WriteLine("At Debug");

			// 1. configure site config file - record selected / not selected
			// 2. copy selected to local folder
			// 3. configure local, suite config file (keep local)

			// take the selected files from this list and...
			// cfgMgr.SeedInstalled.InstalledSeedFiles.FoundFiles

			// and place the configuration settings into this config file
			// and copy them into this folder
			// which will allow local seed files
			// cfgMgr.Suite.

			cfgMgr.Seed.SaveSeedFileList();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			// Debug.WriteLine("At Debug");
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnSelectSiteFolder_OnClick(object sender, RoutedEventArgs e)
		{
			Suite.GetSiteSettingFolder();
		}

		private void Dg1_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

			if (e.OriginalSource.GetType() == typeof(CheckBox)) return;

			selectedInstalledSeedFile.GetFileNameObject.Selected =
				!selectedInstalledSeedFile.GetFileNameObject.Selected;
		}

		private void Dg1SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			Debug.WriteLine("at Selected Changed");

			selectedInstalledSeedFile = 
				Dg1.Items[Dg1.SelectedIndex] as FilePath<FileNameSimpleSelectable>;

			Dg1.CurrentCell = new DataGridCellInfo(selectedInstalledSeedFile, Dg1.Columns[0]);
			Dg1.BeginEdit();
		}


		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
//			try
//			{
//				File.Delete(
//					@"D:\Users\Jeff\OneDrive\Prior Folders\Office Stuff\CAD\Copy Y Drive & Office Standards\AppData\CyberStudio\Andy\Andy.Site.setting.xml");
//			File.Delete(@"C:\Users\jeffs\AppData\Roaming\CyberStudio\Andy\Andy.suite.setting.xml");
//			}
//			catch { }

			Suite.Read();

//			seedFiles = new FolderAndFileSupport(null, SEED_PATTERN);
//
//			if (!Initalize())
//			{
//				// no site path file in SuiteSettings
//				if (!SetupConfiguration())
//				{
//					CannotProceed("Critical information cannot be located.\n" +
//						"I cannot proceed without this information.");
//
//					this.Close();
//				}
//
//				MessageBox.Show("Success!", TITLE, MessageBoxButton.OK, MessageBoxImage.Asterisk);
//
//			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


		#endregion


			
	}

	//	[ValueConversion(typeof(bool), typeof(string))]
	//	public class BoolToFileFoundConverter : IValueConverter
	//	{
	//		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	//		{
	//			string description = (string) parameter;
	//
	//
	//			return (bool) value ? description + " has been found" : description + " has not been found";
	//		}
	//
	//		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	//		{
	//			return null;
	//		}
	//	}

	[ValueConversion(typeof(bool), typeof(string))]
	public class BoolToMessage : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string[] p = new string[((StringCollection) parameter).Count];
			((StringCollection) parameter).CopyTo(p, 0);

			return (bool) value ? p[0] : p[1];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}