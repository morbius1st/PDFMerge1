
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AndyShared.ClassificationDataSupport.SheetSupport;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using UtilityLibrary;
using TreeNode = AndyShared.ClassificationDataSupport.TreeSupport.TreeNode;


// user name: jeffs
// created:   6/11/2025 7:29:32 PM

namespace SuiteInfoEditor.Support
{
	public class ClassifFilesMgr
	{
		private CsFlowDocManager w = CsFlowDocManager.Instance;

		ClassificationFiles cfs = ClassificationFiles.Instance;

		private const string SHOW_COL = "70";
		private const string SHOW_COL_B = "40";
		private const int MARGIN_WIDTH = 3;

		private const int DEP_MARG_COLA_DEF = 2;

		private int depthFactor;
		private int depthMargin_ColA = DEP_MARG_COLA_DEF;
		private int depth = 0;
		private int totalItems = 0;
		private int nodeCount = 0;



		public void ShowFileInfo()
		{
			// beginning outermost and top level

			// GeneralSettingsMgr.ShowSetgColumn = 60; // is local now

			showPrefaceInfo("Classification Files");

			// w.AddDescTextLineFd(GeneralSettingsMgr.ShowIntro(, null, 45, ["<dimgray>", "</dimgray>"], ["<lawngreen>", "</lawngreen>"], ["", ""]));
			//
			// depthFactor = 0;
			// depth = 0;
			//
			// cfs.Initialize();
			//
			// w.StartFd();
			//
			// showSetting("Is Init?", $"{cfs.Initialized}");
			// showSetting("Global Folder Path", $"{cfs.AllClassifFolderPath}");
			// showSetting("User Folder Path", $"{cfs.UserClassfFolderPath}");
			// showSetting("User Folder Path Exists?", $"{cfs.UserClassificationFolderPathExists}");
			// showSetting("Classif File Count", $"{(cfs.UserClassificationFiles?.Count ?? 0)}");
			//
			// w.AddLineBreaksFd(1);

			showFileList();

			w.AddLineBreaksFd(1);

			w.AddDescTextFd("<indent spaces ='4'/>");

			foreach (ClassificationFile cf in cfs.UserClassificationFiles)
			{
				showFileInfo(cf);
				// w.AddDescTextLineFd("<repeat text='-' tocolumn='85'/>");
				//
				// w.AddDescTextLineFd($"<magenta>{cf.FileId}  <dimgray>(File Id)</dimgray></magenta>");
				// w.AddDescTextLineFd($"<magenta>{cf.UserName}  <dimgray>(UserName)</dimgray></magenta>");
				//
				// w.AddDescTextLineFd("<repeat text='-' tocolumn='85'/>");

				cf.Initialize();

				w.AddDescTextFd($"<indent spaces ='+4'/>");
				
				showInfo(cf);

				depthFactor = 0;
				depthMargin_ColA = DEP_MARG_COLA_DEF;

				w.AddDescTextFd($"<indent spaces ='-4'/>");
			}

			w.AddDescTextLineFd("<indent/>");

			w.AddDescTextLineFd("<repeat text='-' tocolumn='85'/>");
		}


		public void ShowFileInfoSummary()
		{
			showPrefaceInfo("Classification FilesSummary");

			w.AddDescTextFd("<indent spaces ='4'/>");

			foreach (ClassificationFile cf in cfs.UserClassificationFiles)
			{
				showFileInfo(cf);

				w.AddLineBreaksFd(1);

				showNodeSummary(cf.TreeBase.RootParent);

				w.AddLineBreaksFd(1);
			}

			w.AddDescTextFd("<indent/>");
		}


		// support methods

		private void showSetting(string title, string value, string pf = "darkgray", string sf = "cyan")
		{
			string m = margin0();

			w.AddDescTextLineFd($"{m}<{pf}>{title} <repeat text='.' tocolumn='{SHOW_COL}'/></{pf}> <{sf}>{value}</{sf}>");
		}

		private int ix = 0;

		private string margin0()
		{
			string marg;

			// int repeat = depthFactor > 1 ? depthMargin_ColA - depth : 0;

			// string marg =  "|  ".Repeat(depth);

			// string marg =  "⁞  ".Repeat(depth);


			// marg =  ":  ".Repeat(depth);
			marg =  "·  ".Repeat(depth);
			// if (ix < 250) marg =  "│  ".Repeat(depth);
			// if (ix < 200) marg =  "∙  ".Repeat(depth);
			// if (ix < 150) marg =  "꞉  ".Repeat(depth);
			// if (ix < 100) marg =  "∙  ".Repeat(depth);
			// if (ix < 50) marg =  ".  ".Repeat(depth);

			ix++;

			string marg2 = margin2(0);

			return $"<dimgray>{marg}</dimgray>{marg2}";
		}

		// for node title only
		private string margin1()
		{
			return "   ".Repeat(depth - 1);
		}

		private string margin2(int d)
		{
			if (depthFactor - 1 <= 0) return "";
			// if (depthMargin_ColA - d <= 1) depthMargin_ColA += 2;
			//
			// int repeat = depthFactor > 0 ? depthMargin_ColA - d : 0;

			// return "   ".Repeat(repeat);
			return "   ".Repeat(depthFactor - 1 + d);
		}

		private string margin3()
		{
			return $"{margin1()}{margin2(1)}";
		}

		private void showDepthInfo(string pos)
		{
			int len0 = margin0().Length;
			int len1 = margin1().Length;
			int len2 = margin2(0).Length;
			int len3 = margin3().Length;

			w.AddTextLineFd($"@{pos,-4}| depthFactor {depthFactor} | depth {depth} | depthMar_ColA {depthMargin_ColA} | lengths 0, 1, 2, 3 | {len0}, {len1}, {len2}, {len3}, ");
		}

		private void showPrefaceInfo(string title)
		{
			w.AddDescTextLineFd(GeneralSettingsMgr.ShowIntro(title, null, 45, ["<dimgray>", "</dimgray>"], ["<lawngreen>", "</lawngreen>"], ["", ""]));

			depthFactor = 0;
			depth = 0;

			cfs.Initialize();

			w.StartFd();

			showSetting("Is Init?", $"{cfs.Initialized}");
			showSetting("Global Folder Path", $"{cfs.AllClassifFolderPath}");
			showSetting("User Folder Path", $"{cfs.UserClassfFolderPath}");
			showSetting("User Folder Path Exists?", $"{cfs.UserClassificationFolderPathExists}");
			showSetting("Classif File Count", $"{(cfs.UserClassificationFiles?.Count ?? 0)}");
			
			w.AddLineBreaksFd(1);
		}

		private void showFileInfo(ClassificationFile cf)
		{
			w.AddDescTextLineFd("<repeat text='-' tocolumn='85'/>");

			w.AddDescTextLineFd($"<magenta>{cf.FileId}  <dimgray>(File Id)</dimgray></magenta>");
			w.AddDescTextLineFd($"<magenta>{cf.UserName}  <dimgray>(UserName)</dimgray></magenta>");

			w.AddDescTextLineFd("<repeat text='-' tocolumn='85'/>");

			cf.Initialize();

			w.AddDescTextFd($"<indent spaces ='+4'/>");

			// do work here

			depthFactor = 0;
			depthMargin_ColA = DEP_MARG_COLA_DEF;

			w.AddDescTextFd($"<indent spaces ='-4'/>");
		}

		// support methods - detailed info

		private void showFileList()
		{
			string fileType;

			w.AddDescTextFd("<indent spaces ='4'/>");

			w.AddDescTextLineFd("List of Classification Files");

			w.AddDescTextLineFd("<repeat text='-' tocolumn='85'/>");
			
			w.AddDescTextFd("<indent spaces ='+4'/>");

			foreach (ClassificationFile cf in cfs.UserClassificationFiles)
			{
				fileType = cf.IsUserClassfFile ? "User" : "Default";

				w.AddDescTextFd($"<magenta>{cf.FileId}</magenta>");
				w.AddDescTextFd($" <repeat text='.' tocolumn='{SHOW_COL_B}'/>");
				w.AddDescTextFd($" {fileType,-9}  ");
				w.AddDescTextFd($" <lawngreen>{cf.FileName,-36}</lawngreen>");
				w.AddDescTextFd($" <dimgray>({cf.FolderPath})</dimgray>\n");
			}

			w.AddDescTextFd("<indent spaces ='-8'/>");

			// w.AddDescTextLineFd("<repeat text='-' tocolumn='105'/>");
		}

		private void showInfo(ClassificationFile cf)
		{
			// outermost level just below header

			totalItems = 1;

			showSetting("Full Path", cf.FullFilePath);

			showSetting("HeaderDescFromFile", cf.HeaderDescFromFile);
			showSetting("HeaderDescFromMemory", cf.HeaderDescFromMemory);
			showSetting("HeaderNote", cf.HeaderNote);
			showSetting("HeaderVersion", cf.HeaderVersion);

			showSetting("SampleFilePath", cf.SampleFilePath);
			showSetting("CanEdit", cf.CanEdit.ToString());
			showSetting("CanSave", cf.CanSave.ToString());
			showSetting("IsInitialized", cf.IsInitialized.ToString());
			showSetting("IsModified", cf.IsModified.ToString());
			showSetting("IsValid", cf.IsValid.ToString());
			showSetting("TreeBase Id", cf.TreeBase.ID);
			showSetting("TreeBase Child Count", cf.TreeBase.ExtChildCount.ToString());

			nodeCount = 0;
			
			depth = 0;  // this will set the tree node margin width as well as all other margins

			showTreeNode(cf.TreeBase.RootParent);

			depth = 0;

			w.AddLineBreaksFd(1);
		}

		private void showTreeNode(TreeNode node)
		{
			// second level.  but this is hierarchical, so it can will be a variable depth
			
			depthFactor = 1;

			nodeCount++;

			// if (nodeCount > 10) return;

			w.AddLineBreaksFd(1);

			if (node == null)
			{
				w.AddDescTextLineFd("<lawngreen>Node is NULL</lawngreen>");
				return;
			}

			depth++;

			// showDepthInfo("A");

			w.AddDescTextLineFd($"{margin1()}<lawngreen>{node.ID}</lawngreen> <dimgray>(Node / ID)</dimgray><repeat text=' ' tocolumn='{SHOW_COL}'/><magenta> {nodeCount}</magenta>");

			showSetting("Parent ID", (node.Parent?.ID ?? "base of tree"));
			showSetting("Depth", node.Depth.ToString());
			showSetting("Chk State", node.CheckedState.ToString());
			showSetting("3-Chk State", node.TriState.ToString());
			showSetting("Is Exp", node.IsExpanded.ToString());
			showSetting("Child Count", node.Children.Count.ToString());

			if (node.Item != null)
			{
				totalItems++;
				showItem(node.Item);
			}
			else
			{
				showSetting("Item",  "is <red>NULL</red>");
			}

			depthFactor = 1;

			if ((node.Children?.Count ?? 0) > 0)
			{
				
				foreach (TreeNode child in node.Children)
				{
					showTreeNode(child);
				}
				
			}

			depthFactor = 1;

			depth--;
		}

		private void showItem(SheetCategory sc)
		{
			depthFactor = 2;

			w.AddLineBreaksFd(1);

			if (sc == null)
			{
				w.AddDescTextLineFd("<lawngreen>Sheet Category is NULL</lawngreen>");
				return;
			}

			// showDepthInfo("B");

			w.AddDescTextLineFd($"{margin3()}<lawngreen>{sc.ID}</lawngreen> <dimgray>(SheetCategory / ID)</dimgray>");

			showSetting("Title", sc.Title);
			showSetting("Description", sc.Description);
			showSetting("ItemClass", sc.ItemClass.ToString());
			showSetting("SortCode", sc.SortCode);
			showSetting("Parent", (sc.Parent?.Item?.Title ?? "<red>NULL</red>"));
			showSetting("CompareOps Count", (sc.CompareOps?.Count -1).ToString());
			showSetting("IsFixed?", sc.IsFixed.ToString());
			showSetting("IsLocked?", sc.IsLocked.ToString());
			showSetting("CompOp Count?", sc.CompOpCount.ToString());


			if ((sc.CompareOps?.Count ?? 0)>0)
			{
				showCompOpps(sc.CompareOps);
			}

			depthFactor = 2;
		}

		private void showCompOpps(ObservableCollection<ComparisonOperation> cos)
		{
			depthFactor = 3;

			if (cos == null)
			{
				w.AddDescTextFd("<lawngreen>List of ComparisonOperations is NULL</lawngreen>");
				return;
			}

			foreach (ComparisonOperation co in cos)
			{
				showCompOp(co);
			}

			depthFactor = 3;
		}

		private void showCompOp(ComparisonOperation co)
		{
			w.AddLineBreaksFd(1);

			if (co == null)
			{
				w.AddDescTextLineFd("<lawngreen>ComparisonOperation is NULL</lawngreen>");
				return;
			}

			// showDepthInfo("C");

			w.AddDescTextLineFd($"{margin3()}<lawngreen>{co.ID}</lawngreen> <dimgray>(ComparisonOperation / ID)</dimgray>");

			showSetting("ID", co.ID);
			showSetting("CompareComponent", $"{co.CompareComponentIndex} ({co.CompareComponentName})");
			showSetting("LogicalComparisonOpCode", co.LogicalComparisonOpCode.ToString());
			showSetting("ValueComparisonOpCode", co.ValueComparisonOpCode.ToString());
			showSetting("CompareValue", co.CompareValue);
			showSetting("IsDisabled", co.IsDisabled.ToString());

		}

		// support methods - summary info

		private void showNodeSummary(TreeNode node)
		{
			nodeCount++;

			string toCol;
			string msg;
			int margin;

			if (node == null)
			{
				w.AddDescTextLineFd("<lawngreen>NODE is NULL</lawngreen>");
				return;
			}

			if (node.Item == null)
			{
				w.AddDescTextLineFd("<lawngreen>Node ITEM is NULL</lawngreen>");
				return;
			}

			if (node.Parent != null)
			{
				msg = ". ".Repeat(node.Depth - 1);
				margin = msg.Length + 5;

				w.AddDescTextFd($"<dimgray>{msg}</dimgray>");

				if (node.Item.Description != null && !node.Item.Description.Equals("node description"))
				{
					msg = node.Item.Description;
					margin += msg.Length;

					if (margin >= 35)
					{
						toCol = "55";
						margin = 55;
					}
					else
					{
						toCol = "35";
						margin = 35;
					}

					w.AddDescTextFd($"{node.Item.Description} <repeat text='.' tocolumn='{toCol}'/> ");
				}

				margin += 28;

				toCol = margin > 35+28 ? "88" : "68";

				w.AddDescTextFd($"<gray>({node.Item.Title})</gray> <repeat text='.' tocolumn='{toCol}'/> [ ");

				showCompOps(node.Item);

				w.AddDescTextFd($" ] <linebreak/>");
			}

			if (node.Children?.Count > 0)
			{
				foreach (TreeNode child in node.Children)
				{
					showNodeSummary(child);
				}
			}

		}

		private void showCompOps(SheetCategory item)
		{
			if (item == null || item.CompareOps == null || item.CompareOps.Count == 0)
			{
				w.AddDescTextFd("empty");
				return;
			}

			int idx = 0;


			foreach (ComparisonOperation co in item.CompareOps)
			{
				if (idx > 0) w.AddDescTextFd($" <magenta>{co.LogicalCompareSymbol}</magenta> ");

				showCompOpSummary(co);

				idx++;
			}
		}

		private void showCompOpSummary(ComparisonOperation co)
		{
			w.AddDescTextFd($"{co.CompareComponentName} <magenta>{co.ValueCompareSymbol}</magenta> {co.CompareValue}");
		}


		public override string ToString()
		{
			return $"this is {nameof(ClassifFilesMgr)}";
		}
	}
}
