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

namespace AndyShared.ConfigMgr
{
	public class ConfigSuite : INotifyPropertyChanged
	{
	#region private fields

	#endregion

	#region ctor

//		public ConfigSuite() { }

	#endregion

	#region public properties

		public static bool Initalized { get; private set; }

		public bool SuiteSettingsFileExists => SuiteSettings.Path.Exists;

		public string SiteSettingsRootPath => SuiteSettings.Data.SiteRootPath;

		public bool SiteSettingsRootPathIsValid => !SiteSettingsRootPath.IsVoid();

		public string SuiteSettingFileName => SuiteSettings.Path.FileName;

		public SuiteSettingInfo70<SuiteSettingData70> Info => SuiteSettings.Info;

		public ConfigSite Site { get; private set; } = new ConfigSite();

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Read()
		{
			SuiteSettings.Admin.Read();

			Initalized = true;

			updateProperties();

			if (!SuiteSettings.Data.SiteRootPath.IsVoid())
			{
				Site.SiteSettingsRootPath = SuiteSettings.Data.SiteRootPath;

				Site.Read();
			}
		}

		public void Write()
		{
			SuiteSettings.Admin.Write();
		}

		public void GetSiteSettingFolder()
		{
			string selected = SelectSettingFolder();

			if (selected.IsVoid()) return;

			SuiteSettings.Data.SiteRootPath = selected;

			Write();

			Site.SiteSettingsRootPath = selected;

			Site.Read();

			updateProperties();
		}

		public string SelectSettingFolder()
		{
			string selected; 

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

				selected = cfd.FileName;
			}

			return selected;
		}

	#endregion

	#region private methods

		private void updateProperties()
		{
			OnPropertyChange("Initalized");
			OnPropertyChange("SuiteSettingsFileExists");
			OnPropertyChange("SiteRootPath");
			OnPropertyChange("SiteSettingsRootPathIsValid");
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
}