
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndyShared.ClassificationDataSupport.SheetSupport;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using UtilityLibrary;


// user name: jeffs
// created:   6/11/2025 7:29:32 PM

namespace SuiteInfoEditor.Support
{
	public class ClassifFilesMgr
	{
		private CsFlowDocManager w = CsFlowDocManager.Instance;

		ClassificationFiles cfs = ClassificationFiles.Instance;

		public void GetFileInfo()
		{
			GeneralSettingsMgr.ShowSetgColumn = 60;

			cfs.Initialize();

			GeneralSettingsMgr.ShowSetting("Is Init?", $"{cfs.Initialized}");
			GeneralSettingsMgr.ShowSetting("Global Folder Path", $"{cfs.AllClassifFolderPath}");
			GeneralSettingsMgr.ShowSetting("User Folder Path", $"{cfs.UserClassfFolderPath}");
			GeneralSettingsMgr.ShowSetting("User Folder Path Exists?", $"{cfs.UserClassificationFolderPathExists}");
			GeneralSettingsMgr.ShowSetting("User Classif File Count", $"{(cfs.UserClassificationFiles?.Count ?? 0)}");


			

			w.AddLineBreaks(1);

			w.StartTb("<indent spaces ='4'/>");

			foreach (ClassificationFile cf in cfs.UserClassificationFiles)
			{
				w.AddDescTextLineTb("<repeat text='-' tocolumn='85'/>");

				w.AddDescTextLineTb($"{cf.FileId}  (File Id)");
				w.AddDescTextLineTb($"{cf.UserName}  (UserName)");

				w.AddDescTextLineTb("<repeat text='-' tocolumn='85'/>");

				cf.Initialize();

				w.StartTb("<indent spaces ='+4'/>");
				
				showInfo(cf);

				w.StartTb("<indent spaces ='-4'/>");

			}

			w.AddDescTextLineTb("<indent/>");

			w.AddDescTextLineTb("<repeat text='-' tocolumn='85'/>");
		}

		private int depth = 0;
		private int totalItems = 0;
		private int nodeCount = 0;

		private void showInfo(ClassificationFile cf)
		{
			depth = 0;
			totalItems = 1;

			

			GeneralSettingsMgr.ShowSetting("Full Path", cf.FullFilePath);
			// GeneralSettingsMgr.ShowSetting("FolderPath", cf.FolderPath);
			// GeneralSettingsMgr.ShowSetting("FolderPathLocal", cf.FolderPathLocal);


			GeneralSettingsMgr.ShowSetting("HeaderDescFromFile", cf.);

			GeneralSettingsMgr.ShowSetting("HeaderDescFromFile", cf.HeaderDescFromFile);
			GeneralSettingsMgr.ShowSetting("HeaderDescFromMemory", cf.HeaderDescFromMemory);
			GeneralSettingsMgr.ShowSetting("HeaderNote", cf.HeaderNote);

			GeneralSettingsMgr.ShowSetting("SampleFilePath", cf.SampleFilePath);
			GeneralSettingsMgr.ShowSetting("CanEdit", cf.CanEdit.ToString());
			GeneralSettingsMgr.ShowSetting("CanSave", cf.CanSave.ToString());
			GeneralSettingsMgr.ShowSetting("IsInitialized", cf.IsInitialized.ToString());
			GeneralSettingsMgr.ShowSetting("IsModified", cf.IsModified.ToString());
			GeneralSettingsMgr.ShowSetting("IsValid", cf.IsValid.ToString());
			GeneralSettingsMgr.ShowSetting("TreeBase Id", cf.TreeBase.ID);
			GeneralSettingsMgr.ShowSetting("TreeBase Child Count", cf.TreeBase.ExtChildCount.ToString());

			w.StartTb("<indent spaces ='+4'/>");

			nodeCount = 0;

			showTreeNode(cf.TreeBase.RootParent);

			w.StartTb("<indent spaces ='-4'/>");

		}



		private void showTreeNode(TreeNode node)
		{
			if (nodeCount > 10) return;

			if (node == null)
			{
				w.AddDescTextTb("<lawngreen>Node is NULL</lawngreen>");
				return;
			}

			int item = 0;

			foreach (TreeNode child in node.Children)
			{
				item++;
				totalItems++;

				w.AddLineBreaks(1);

				showNode(child.Item);

				w.AddLineBreaks(1);

				if (child.Children.Count > 0)
				{
					depth++;
					
					showTreeNode(child);
					
					depth--;
				}
			}
		}

		private void showNode(SheetCategory sc)
		{
			nodeCount++;

			w.AddLineBreaks(1);

			if (sc == null)
			{
				w.AddDescTextTb("<lawngreen>Sheet Category is NULL</lawngreen>");
				return;
			}


			w.AddDescTextTb($"<lawngreen>{sc.ID}</lawngreen> <dimgray>(SheetCategory / ID)</dimgray>");

			w.StartTb("<indent spaces ='+4'/>");

			GeneralSettingsMgr.ShowSetting("ID", sc.ID);
			GeneralSettingsMgr.ShowSetting("Title", sc.Title);
			GeneralSettingsMgr.ShowSetting("Description", sc.Description);
			GeneralSettingsMgr.ShowSetting("ItemClass", sc.ItemClass.ToString());
			GeneralSettingsMgr.ShowSetting("SortCode", sc.SortCode);
			GeneralSettingsMgr.ShowSetting("Parent", sc.Parent.Item.Title);
			GeneralSettingsMgr.ShowSetting("CompareOps Count", (sc.CompareOps?.Count -1).ToString());
			GeneralSettingsMgr.ShowSetting("IsFixed?", sc.IsFixed.ToString());
			GeneralSettingsMgr.ShowSetting("IsLocked?", sc.IsLocked.ToString());
			GeneralSettingsMgr.ShowSetting("CompOp Count?", sc.CompOpCount.ToString());

			if ((sc.CompareOps?.Count ?? 0)>0)
			{
				w.AddLineBreaks(1);

				showCompOpps(sc.CompareOps);
			}

			w.StartTb("<indent spaces ='-4'/>");

			w.AddLineBreaks(1);
		}

		private void showCompOpps(ObservableCollection<ComparisonOperation> cos)
		{
			if (cos == null)
			{
				w.AddDescTextTb("<lawngreen>List of ComparisonOperations is NULL</lawngreen>");
				return;
			}

			foreach (ComparisonOperation co in cos)
			{
				
				showCompOp(co);
				
			}
		}

		private void showCompOp(ComparisonOperation co)
		{
			if (co == null)
			{
				w.AddDescTextTb("<lawngreen>ComparisonOperation is NULL</lawngreen>");
				return;
			}


			w.AddDescTextTb($"<lawngreen>{co.ID}</lawngreen> <dimgray>(ComparisonOperation / ID)</dimgray>");

			GeneralSettingsMgr.ShowSetting("ID", co.ID);

			w.StartTb("<indent spaces ='+4'/>");

			GeneralSettingsMgr.ShowSetting("CompareComponentIndex", co.CompareComponentIndex.ToString());
			GeneralSettingsMgr.ShowSetting("LogicalComparisonOpCode", co.LogicalComparisonOpCode.ToString());
			GeneralSettingsMgr.ShowSetting("ValueComparisonOpCode", co.ValueComparisonOpCode.ToString());
			GeneralSettingsMgr.ShowSetting("CompareValue", co.CompareValue);
			GeneralSettingsMgr.ShowSetting("IsDisabled", co.IsDisabled.ToString());

			w.StartTb("<indent spaces ='-4'/>");
		}


		public override string ToString()
		{
			return $"this is {nameof(ClassifFilesMgr)}";
		}
	}
}
