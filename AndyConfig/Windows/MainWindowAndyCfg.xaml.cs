#region using

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AndyConfig.ConfigMgr;
using AndyShared.ConfigMgr;
using SettingsManager;
using AndyShared.ClassificationFileSupport;
using AndyShared.ConfigMgrShared;
using AndyShared.ConfigSupport;

#endregion

// projname: AndyConfig
// itemname: MainWindowAndyCfg
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
 * 4. setup the LocalSeedFilesList variable
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
 // objects 
   // ConfigSuite - the SuiteSettingsFile
   // ConfigInstalledSeedFiles - the Installed Seed Folder and Files
   // ConfigSite - the SiteSettingsFile
   // ConfigSiteSeedFiles - the Site Seed Folder and Files
   // ConfigLocalSeedFiles - the Local Seed Folder and Files + 
   //		the composite list of seed files (site + local)
   
 // information
   // installedSeed => has installed seed files
   // siteSeed => has site seed files  + has list of selected installed seed files
   // localSeed => has local seed files + compsite list of selectes seed files + local seed files
 *
 *
*/

namespace AndyConfig.Windows
{
	public partial class MainWindowAndyCfg : Window, INotifyPropertyChanged
	{
		public static string TITLE = "Andy Configurator";

	#region private fields

		private const string SEED_PATTERN = @"*.seed.xml";

		private ConfigManager cfgMgr = new ConfigManager();

		private ConfigSeedFile selInstalledSeedFile;

		private string userNameSelected;

	#endregion

	#region ctor

		public MainWindowAndyCfg()
		{
			InitializeComponent();


			// ClassfFile<ClassificationFileData>
			// 	ClsFile1 = new ClassfFile<ClassificationFileData>();

			BaseDataFile<ClassificationFileData>
				ClsFile1 = new BaseDataFile<ClassificationFileData>();

			ClsFile1.Configure(@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs", "(jeffs) PdfSample 1.xml");
		}

	#endregion

	#region public properties

		public ConfigSuite Suite => cfgMgr?.Suite ?? null;
		public ConfigSite Site => cfgMgr?.Site ?? null;
		public ConfigSeedInstalled SeedInstalled => cfgMgr?.SeedInstalled ?? null;
		public ConfigSeedSite SeedSite => cfgMgr?.SeedSite ?? null;
		public ConfigSeedLocal SeedLocal => cfgMgr?.SeedLocal ?? null;
		public ClassificationFiles ClassificationFiles => cfgMgr?.ClassificationFiles ?? null;

		public string SiteRootPath => SuiteSettings.Data.SiteRootPath;

		public string SiteSettingFile => SiteSettings.Path.FileName;

		public string UserNameSelected
		{
			get => userNameSelected;
			private set
			{
				userNameSelected = value;
				OnPropertyChange();
			}
		}

		#endregion

		#region private properties

		#endregion

		#region public methods

		#endregion

		#region private methods

		#endregion

		#region event processing

		private void BtnSeedSaveApply_OnClick(object sender, RoutedEventArgs e)
		{
			// Debug.WriteLine("At Debug");

			// 1. configure site config file - record selected / not selected
			// 2. copy selected to local folder
			// 3. configure local, suite config file (keep local)

			// take the selected files from this list and...
			// cfgMgr.SeedInstalled.InstalledSeedFileList.FoundFiles

			// and place the configuration settings into this config file
			// and copy them into this folder
			// which will allow local seed files
			// cfgMgr.Suite.


			// cfgMgr.Seed.SaveSeedFileList();

			SeedInstalled.Apply();
			
		}

		private void Btn_SiteSeedApply_OnClick(object sender, RoutedEventArgs e)
		{
			// ConfigSeedSite a =
			// 	ConfigSeedSite.Instance;
			//
			// ICollectionView v = a.View;

			SeedSite.Apply();

			Debug.WriteLine("At Debug");
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			// Debug.WriteLine("user classification config file| " +
			// 	ClassificationUser.Find("jeffs", "PdfSample 2") ?? "is null");

			Debug.WriteLine("At Debug");
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
			if (selInstalledSeedFile == null ||
				e.OriginalSource.GetType() == typeof(CheckBox)) return;
			
			selInstalledSeedFile.SelectedSeed =
				!selInstalledSeedFile.SelectedSeed;
		}

		private void Dg1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			Debug.WriteLine("at Selected Changed");

			selInstalledSeedFile = 
				Dg1.Items[Dg1.SelectedIndex] as ConfigSeedFile;

			Dg1.CurrentCell = new DataGridCellInfo(selInstalledSeedFile, Dg1.Columns[0]);
			Dg1.BeginEdit();
		}


		private void expdr06_Expanded(object sender, RoutedEventArgs e)
		{
			UserNameSelected = (string) ((Expander) sender).Tag;

			// if want to get the child textblock with the user name:
			// TextBlock tbk = (TextBlock) ((Expander) sender).FindName("tbkUsrName");


			// string path = UserSettings.Path.SettingFolderPath;
		}

		private void expdr06_Collapsed(object sender, RoutedEventArgs e)
		{
			UserNameSelected = null;
		}

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			cfgMgr = new ConfigManager();

			cfgMgr.Initialize();

			OnPropertyChange("Suite");
			OnPropertyChange("Site");
			OnPropertyChange("SeedInstalled");
			OnPropertyChange("SeedSite");
			OnPropertyChange("SeedLocal");
			OnPropertyChange("User");
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


		#endregion

		private void Dg4_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

	}

	[ValueConversion(typeof(bool), typeof(string))]
	public class BoolToMessage : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null) return "";

			string[] p = new string[((StringCollection) parameter).Count];
			((StringCollection) parameter).CopyTo(p, 0);

			return (bool) value ? p[0] : p[1];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

	[ValueConversion(typeof(string), typeof(string))]
	public class NullStringToMessage : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null) parameter = "is null";

			return (string) value ?? (string) parameter;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

	[ValueConversion(typeof(bool), typeof(string))]
	public class EnumToMessage : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null || value == null) return "";

			string[] p = new string[((StringCollection) parameter).Count];
			((StringCollection) parameter).CopyTo(p, 0);

			SeedFileStatus status = (SeedFileStatus) value;

			if (status == SeedFileStatus.IGNORE) return "";

			return p[(int) status];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
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


	// [ValueConversion(typeof(bool), typeof(string))]
	// public class BoolToString : IValueConverter
	// {
	// 	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	// 	{
	// 		return (bool) value ? "True" : "False";
	// 	}
	//
	// 	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	// 	{
	// 		return null;
	// 	}
	// }


}