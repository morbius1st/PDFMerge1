#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;

using AndyShared.FileSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.MergeSupport;
using AndyShared.Settings;
using JetBrains.Annotations;
using SettingsManager;

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


		// objects

		private BaseOfTree root;

		private Classify cls4;

		private MainWindow mw;
		private bool treeItemModified;
		private bool treeModified;

	#endregion

	#region ctor

		public MainWinSupport(MainWindow mw)
		{
			this.mw = mw;
		}

		private void init()
		{
			
		}

	#endregion

	#region public properties

		public ClassificationFile ClassificationFile { get; set; }

	#endregion

	#region private properties

	#endregion

	#region process methods

	#endregion

	#region public methods

		public bool FilterSamples(string fileid)
		{
			if (!GetClassifFile(fileid)) return false;

			cls4 = new Classify();

			bool result = SheetPdfManager.Instance.
				FilterSheetNames4(ClassificationFile, cls4);

			return result;
		}

		public bool ParseSamples()
		{
			return SheetPdfManager.Instance.ParseSheetNames4();
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
				ClassificationFile = ClassificationFile.Create();

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
				if (ClassificationFile.Exists(fileId))
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
			return ClassificationFile.Create(fileId);
		} 

		public void ReadClassfFile()
		{
			ClassificationFile = ClassificationFile.GetUserClassfFile(UserSettings.Data.LastClassificationFileId);

			int count;
			string path = @"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\(jeffs) PdfSample 1.xml";

			count = CsXmlUtilities.CountXmlElements(path, "TreeNode");

			ClassificationFile.Initialize();

			OnPropertyChanged(nameof(ClassificationFile));

		}

		public string GetNodePath(TreeNode node)
		{
			TreeNode tempNode = node;
			string namePath = node.Item.Title;

			for (int i = 1; i < node.Depth; i++)
			{
				tempNode = tempNode.Parent;
				namePath = $"{tempNode.Item.Title} > {namePath}";
			}

			return namePath;
		}

		// public static void AddPrelimCompOp(SheetCategory item)
		// {
		// 	ComparisonOp vco = new ComparisonOp(LOGICAL_AND, DOES_NOT_EQUAL, "1", 2);
		// 	vco.Parent = item;
		// 	vco.CompOpModified = true;
		//
		// 	item.CompareOps.Add(vco);
		// }


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