#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using AndyShared.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  6/19/2020 7:09:57 PM

// ConfigSuite - the SuiteSettingFile configuration routines
// (as a  static instances)
// properties
//  initialized
//  the root path to the site file (stored)
//  the name of the site setting file (stored)
//  flag: suite setting file exists
//  SuiteSettingsFile's info
// methods
//  initialize
//  read()
//  write()
//  GetSiteSeedFolderPath
// event
//  site folder changed
// ** move select folder to shared
// ** remove configsite


namespace AndyShared.ConfigMgr
{
	public class ConfigSuite : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<ConfigSuite> instance =
			new Lazy<ConfigSuite>(() => new ConfigSuite());

	#endregion

	#region ctor

		private ConfigSuite() { }

	#endregion

	#region public properties

		public static ConfigSuite Instance => instance.Value;

		public static bool Initalized { get; private set; }

		public string SuiteSettingPath => SuiteSettings.Path.SettingPath;
		public string SuiteSettingFileName => SuiteSettings.Path.FileName;

		public bool SuiteSettingsFileExists => SuiteSettings.Path.Exists;

		public string SiteSettingsRootPath => SuiteSettings.Data.SiteRootPath;

		public bool SiteSettingsRootPathIsValid => !SiteSettingsRootPath.IsVoid();

		public SuiteSettingInfo70<SuiteSettingData70> Info => SuiteSettings.Info;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			Initalized = true;

			UpdateProperties();

			SuiteSettings.Admin.Read();
		}

		public void Write()
		{
			SuiteSettings.Admin.Write();
		}

		public void GetSiteSettingFolder()
		{
			string siteSettingFolder = SelectSettingFolder();

			if (siteSettingFolder.IsVoid()) return;

			SuiteSettings.Data.SiteRootPath = siteSettingFolder;

			Write();

			IssuePathChangeEvent();

			UpdateProperties();

			// bool isFilePath;
			// bool isFolderPath;
			//
			// bool exists = FilePathUtil.Exists(siteSettingFolder, out isFolderPath, out isFilePath);
			//
			// Site.SiteSettingsRootPath = siteSettingFolder;
		}

		public string SelectSettingFolder()
		{
			string siteSettingFolder;

			using (CommonOpenFileDialog cfd = new CommonOpenFileDialog("Select Site Setting Folder - "
				+ MainWindow.TITLE))
			{
				cfd.IsFolderPicker = true;
				cfd.Multiselect = false;
				cfd.ShowPlacesList = true;
				cfd.AllowNonFileSystemItems = false;

				cfd.AllowPropertyEditing = false;

				CommonFileDialogResult result = cfd.ShowDialog();

				if (result != CommonFileDialogResult.Ok) return null;

				siteSettingFolder = cfd.FileName;
			}

			return siteSettingFolder;
		}

		

	#endregion

	#region private methods

		private void UpdateProperties()
		{
			OnPropertyChange("Initalized");
			OnPropertyChange("SuiteSettingPath");
			OnPropertyChange("SuiteSettingsFileExists");
			OnPropertyChange("SiteRootPath");
			OnPropertyChange("SiteSettingsRootPathIsValid");
		}

		private void IssuePathChangeEvent()
		{
			PathChangedEventArgs e = new PathChangedEventArgs() {Path = SiteSettingsRootPath};

			RaiseOnSiteRootPathChangedEvent(e);
		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public delegate void OnSiteRootPathChangedEventHandler(object sender, PathChangedEventArgs e);

		public event OnSiteRootPathChangedEventHandler OnSiteRootPathChanged;

		protected virtual void RaiseOnSiteRootPathChangedEvent(PathChangedEventArgs e)
		{
			OnSiteRootPathChanged?.Invoke(this, e);
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ConfigSuite";
		}

	#endregion
	}

	public class RootPathValidator : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string info = (string) value;

			if (info.IsVoid())
			{
				new ValidationResult(false, "cannot be empty");
			}

			return ValidationResult.ValidResult;
		}
	}

	public class PathChangedEventArgs : EventArgs
	{
		public  bool HasFileName { get; set; }
		public string Path { get; set; }
		public string FileName { get; set; }
	}
}