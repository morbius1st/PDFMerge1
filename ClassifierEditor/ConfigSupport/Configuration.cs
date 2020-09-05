#region using directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using UtilityLibrary;
using SettingsManager;

using AndyShared.Settings;

#endregion

// username: jeffs
// created:  6/11/2020 6:14:19 AM

namespace ClassifierEditor.ConfigSupport
{
	// configures the location of the information (folders and file names)
	// and handles creating a default setup

	public class Configuration : INotifyPropertyChanged
	{
	#region private fields

		// private ClassificationFiles classificationFiles = ClassificationFiles.Instance;


		// private ClassificationFile classificationFile;

	#endregion

	#region ctor

		public Configuration()
		{
			UserSettings.Admin.Read();
			UserSettings.Admin.Write();

			MachSettings.Admin.Read();
			MachSettings.Admin.Write();

			// string n = MachSettings.Data.LastClassificationFileId;
			// string rp = MachSettings.Path.RootFolderPath;
			// string sp = MachSettings.Path.SettingFolderPath;

			// classificationFiles.Initialize();

			// getLastClassificationFile(UserSettings.Data.LastClassificationFileId);

			// SuiteSettings.Admin.Read();
			//
			// SiteSettings.Path.RootFolderPath = SuiteSettings.Data.SiteRootPath;
			//
			// SiteSettings.Admin.Read();
			//
			// AppSettings.Admin.Read();


			// AddUserOrgFile("PdfSample", "jeffs");
		}

	#endregion

	#region public properties

		// public ClassificationFile ClassfFile => classificationFile;
		//
		// public string LastEditedClassificationFilePath => classificationFile.FullFilePath;
		// // ConfigFileSupport.MakeClassificationFileName(
		// // Environment.UserName, UserSettings.Data.LastClassificationFileId);
		//
		// public string LastEditedSampleFilePath => classificationFile.SampleFilePath;
		// // ConfigFileSupport.GetSampleFile(LastEditedClassificationFolderPath, LastEditedClassificationFilePath, false);
		//
		// public string LastEditedClassificationFolderPath => classificationFile.FolderPath;
		// // classificationUser.Find(Environment.UserName, UserSettings.Data.LastClassificationFileId);
		//
		// public string LastEditedClassificationFileName => classificationFile.FileName;
		//
		// public string LastEditedClassificationFileDescription => classificationFile.DescriptionFromFile;

	#endregion

	#region private properties

	#endregion

	#region public methods

		// public void AddUserOrgFile(string id, string username)
		// {
		//
		// 	string file = ConfigFileSupport.MakeClassificationFileName(id, username);
		//
		// 	ConfigFile<FileNameSimple> cfs =
		// 		new ConfigFile<FileNameSimple>(
		// 			id,
		// 			username,
		// 			ConfigFileSupport.UserClassificationFolderPath,
		// 			file);
		//
		// 	SuiteSettings.Data.UsersOrganizationFiles.Add(cfs);
		// 	SuiteSettings.Admin.Write();
		// }

		// todo: fix
		// public void ConfigureCategories( SheetCategoryDataManager dm)
		// {
		// 	dm.Configure(UserSettings.Data.CatConfigFolder,
		// 		UserSettings.Data.CatConfigFile);
		// }

	#endregion

	#region private methods

		// private void getLastClassificationFile(string fileId)
		// {
		// 	classificationFile = ClassificationFileAssist.GetUserClassfFile(fileId);
		// 	classificationFile.Read();
		// 	
		//
		// 	// classificationFile = classificationFiles.Find(Environment.UserName, fieldId);
		// }

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
			return "this is Configuration";
		}

	#endregion
	}
}