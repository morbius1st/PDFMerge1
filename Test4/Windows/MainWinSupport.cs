#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AndyShared.ClassificationFileSupport;
using AndyShared.FileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.Settings;
using JetBrains.Annotations;
using SettingsManager;
using Test4.Support;
using Test4.SheetMgr;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  9/21/2024 5:03:59 PM

namespace Test4.Windows
{
	public class MainWinSupport : INotifyPropertyChanged
	{
	#region private fields

		private int last;
		private int start;
		private int end;


	#endregion

	#region ctor

		public MainWinSupport() { }

		private void init()
		{
			
		}

	#endregion

	#region public properties

		public ClassificationFile ClassificationFile { get; set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool ProcessSamples()
		{
			MakeSamples();
			ConfigFileNameParser();
			return ParseSampleFileNames();
		}

		public void MakeSamples()
		{
			Samples4.MakeSamples();
		}

		public void ConfigFileNameParser()
		{
			FileNameSheetParser3.Instance.Config();
			FileNameSheetParser3.Instance.CreateSpecialDisciplines(SheetPdfManager.Instance.SpecialDisciplines);
			FileNameSheetParser3.Instance.CreateFileNamePattern();
		}

		public bool ParseSampleFileNames()
		{
			bool result = true;

			last = Samples4.Sheets.Count;
			start = 0;
			end = last;

			for (var i = start; i < end; i++)
			{
				Samples4.Sheets[i].SheetPdf3 =
					new FilePath<FileNameSheetPdf3>(Samples4.Sheets[i].FileName);

				result &= Samples4.Sheets[i].SheetPdf3.FileNameObject.ShtNumber.IsParseGood;
			}

			return result;
		}


		public void UserSettingsWrite()
		{
			UserSettings.Admin.Write();
		}

		public void UserSettingsRead()
		{
			if (!UserSettings.Admin.Path.Exists) UserSettings.Admin.Create();

			UserSettings.Admin.Read();

			UserSettings.Data.IsRead = true;

		}



		public bool GetClassifFile(string fileId = null)
		{
			if (!InitClassfFile(fileId))
			{
				// prior does not exist - create
				ClassificationFile = ClassificationFileAssist.Create();

				UserSettings.Data.LastClassificationFileId = ClassificationFile.FileId;

				UserSettingsWrite();

				ClassificationFile.Initialize();

				OnPropertyChanged(nameof(ClassificationFile));
			}

			return ClassificationFile.FilePathLocal.Exists;
		}

		public bool InitClassfFile(string fileId = null)
		{
			if (!UserSettings.Data.IsRead) UserSettingsRead();

			// use cases
			// case a: fileid provided as an argument to this routine
			//		open this file - return true;
			// case b: fileid saved in user settings
			//		open this file - return true
			// case c: neither of the above
			//		return false - allow calling routine determine how to proceed

			if (!fileId.IsVoid())
			{
				if (ClassificationFileAssist.Exists(fileId))
				{
					UserSettings.Data.LastClassificationFileId = fileId;

					UserSettingsWrite();

					ReadClassfFile();

					return true;
				}

			}
			else
			{
				if (!UserSettings.Data.LastClassificationFileId.IsVoid())
				{
					ReadClassfFile();

					return true;
				}
			}


			return false;
		}

		public ClassificationFile CreateClassifFile(string fileId = null)
		{
			return ClassificationFileAssist.Create(fileId);
		} 

		public void ReadClassfFile()
		{
			ClassificationFile = ClassificationFileAssist.GetUserClassfFile(UserSettings.Data.LastClassificationFileId);
			ClassificationFile.Initialize();

			OnPropertyChanged(nameof(ClassificationFile));
		}



		// public void TestInfo()
		// {
		// 	UserSettingPath fu = UserSettings.Path;
		// 	MachSettingPath fm = MachSettings.Path;
		// 	SuiteSettingPath fs = SuiteSettings.Path;
		// 	
		// 	
		// 	// " default" folder for the classification files
		// 	FilePath<FileNameSimple> f1 = SettingsSupport.AllClassifFolderPath;
		// 	
		// 	FilePath<FileNameSimple> f2 = SettingsSupport.UserClassifFolderPath;
		// 	
		// 	ClassificationFile cf1 = ClassificationFileAssist.GetUserClassfFile("PdfSample 1");
		// 	cf1.Initialize();
		// 	
		// 	ClassificationFile cf2 = ClassificationFileAssist.Create();
		//
		// 	ClassificationFile cf3;
		// }

	#endregion

	#region private methods

		private bool configClassfFilePath(string fileId = null)
		{
			if (!UserSettings.Data.IsRead) UserSettingsRead();

			if (!fileId.IsVoid())
			{
				UserSettings.Data.LastClassificationFileId = fileId;

				UserSettingsWrite();
			}
			else
			if (UserSettings.Data.LastClassificationFileId.IsVoid())
			{
				return false;
			}

			return true;
		}



	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}




	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(MainWinSupport)}";
		}

	#endregion
	}
}