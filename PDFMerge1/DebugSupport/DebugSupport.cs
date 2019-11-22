#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PDFMerge1.Support;
using static UtilityLibrary.MessageUtilities;

#endregion


// projname: PDFMerge1.DebugSupport
// itemname: DebugSupport
// username: jeffs
// created:  10/31/2019 11:13:38 PM


namespace PDFMerge1.DebugSupport
{
	public class DebugSupport
	{
		private static DebugSupport _instance = null;

		public static DebugSupport Instance
		{
			get
			{
				if (_instance == null) _instance = new DebugSupport();

				return _instance;
			}
		}

		private DebugSupport()
		{
			MatchList();



		}



		internal FileList ModifyFileList(FileList fl)
		{
			logMsgDbLn2("");
			logMsgDbLn2("********", "*******");
			logMsgDbLn2("modifyfile list");



			for (int i = 0; i < fl.GrossCount; i++)
			{
				FileItem fi = fl[i];

				logMsgDbLn2("");
				logMsgDbLn2("********", "*******");
				logMsgDbLn2("name", fi.getName());
				logMsgDbLn2("outline path", fi.outlinePath);
				logMsgDbLn2("directory", fi.getDirectory());
				logMsgDbLn2("outline directory", fi.getOutlineDirectory());

				logMsgDbLn2("");

				fi = AdjustOutlinePerFile(fi);

				fi.outlinePath = AdjustOutlinePerFile(fi.outlinePath, fi.getName());
				logMsgDbLn2("revised outline directory", fi.getOutlineDirectory());

			}

			return fl;
		}

		private MatchItem miArchGen = new MatchItem(
			new Regex(@"^[A-Z] ?A", RegexOptions.Compiled), 
			"070 Arch", 0);

		private MatchItem miArchSite = new MatchItem(
			new Regex(@"^[A-Z] ?A1", RegexOptions.Compiled), 
			"070 Arch\\01 Site", 1);
		private MatchItem miArchPlans = new MatchItem(
			new Regex(@"^[A-Z] ?A2", RegexOptions.Compiled), 
			"070 Arch\\02 Plans", 1);
		private MatchItem miArchVert = new MatchItem(
			new Regex(@"^[A-Z] ?A3", RegexOptions.Compiled), 
			"070 Arch\\03 Vert Circ", 1);


		private MatchItem miStruct = new MatchItem(
			new Regex(@"^[A-Z] ?S", RegexOptions.Compiled), 
			"110 Struct", 0);

		private MatchItem miMech = new MatchItem(
			new Regex(@"^[A-Z] ?M", RegexOptions.Compiled), 
			"150 Mech", 0);

		private MatchItem miPlumb = new MatchItem(
			new Regex(@"^[A-Z] ?P", RegexOptions.Compiled), 
			"130 Plumb", 0);

		private MatchItem miTitle = new MatchItem(
			new Regex(@"^[A-Z] ?T", RegexOptions.Compiled), 
			"000 Title", 0);

		private MatchItem miCover = new MatchItem(
			new Regex(@"^[A-Z] ?C", RegexOptions.Compiled), 
			"000 Cover", 0);

		private MatchItem miCivil = new MatchItem(
			new Regex(@"^C ?\d", RegexOptions.Compiled), 
			"010 Civil", 0);

		public struct MatchItem : IComparable<MatchItem>
		{
			public Regex Pattern { get; set; }
			public string BookmarkTitle { get; set; }
			public int DepthAdjust { get; set; }

			public MatchItem(Regex pattern, string title, int depthAdjust)
			{
				Pattern = pattern;
				BookmarkTitle = title;
				DepthAdjust = depthAdjust;
			}

			public int CompareTo(MatchItem other)
			{
				return other.DepthAdjust - DepthAdjust;
			}
		}

		public List<MatchItem> BookmarkTitles { get; } = new List<MatchItem>();

		public void MatchList()
		{
			BookmarkTitles.Add(miArchGen);

			BookmarkTitles.Add(miArchSite);
			BookmarkTitles.Add(miArchPlans);
			BookmarkTitles.Add(miArchVert);


			BookmarkTitles.Add(miStruct);
			BookmarkTitles.Add(miCover);
			BookmarkTitles.Add(miTitle);
			BookmarkTitles.Add(miCivil);
			BookmarkTitles.Add(miPlumb);
			BookmarkTitles.Add(miMech);

			BookmarkTitles.Sort();

			
		}

		internal FileItem AdjustOutlinePerFile(FileItem fi)
		{
			logMsgDbLn2("");

			string firstDir = fi.outlinePath.GetFirstDirectoryName();
//			string firstDir2 = fi.outlinePath.GetSubDirectoryPath(0);
//			string firstDir3 = fi.outlinePath.GetSubDirectoryName(0);
			string remainDir = fi.outlinePath.Substring(firstDir.Length);

			string newPath = fi.outlinePath;

//			logMsgDbLn2("firstdir", firstDir);
////			logMsgDbLn2("firstdir2", firstDir2);
////			logMsgDbLn2("firstdir3", firstDir3);
//			logMsgDbLn2("remaindir", remainDir);

			for (int i = 0; i < BookmarkTitles.Count; i++)
			{
				logMsgDbLn2("test pattern", BookmarkTitles[i].Pattern);
				if (BookmarkTitles[i].Pattern.IsMatch(fi.getName()))
				{
					newPath = "\\" + BookmarkTitles[i].BookmarkTitle
						+ remainDir;
					logMsgDbLn2("found| new path", newPath);

					fi.outlinePath = newPath;

					logMsgDbLn2("");

					break;
				}
			}

			return fi;

		}


		internal string AdjustOutlinePerFile(string outlinePath, string fileName)
		{
			string firstDir = outlinePath.GetFirstDirectoryName();
			string remainDir = outlinePath.Substring(firstDir.Length);

			string newPath = outlinePath;

//			logMsgDbLn2("first dir", firstDir);
//			logMsgDbLn2("remain dir", remainDir);

			for (int i = 0; i < BookmarkTitles.Count; i++)
			{
				if (BookmarkTitles[i].Pattern.IsMatch(fileName))
				{
					newPath = "\\" + BookmarkTitles[i].BookmarkTitle
						+ remainDir;


				}
			}

			return newPath;
		}



		internal void ModifyMergeTree(PdfMergeTree tree)
		{
			logMsgDbLn2("********", "*******");
			logMsgDbLn2("modify merge items");
			ModifyMergeTree(tree.GetMergeItems, 0);
		}

		private void ModifyMergeTree(List<MergeItem> mergeitems, int depth)
		{
			foreach (MergeItem mi in mergeitems)
			{
				ListMI(mi);

				if (mi.mergeItems != null)
				{
					ModifyMergeTree(mi.mergeItems, depth + 1);
				}
			}
		}


		private void ListMI(MergeItem mi)
		{
			logMsgDbLn2("");

			if (mi.depth == 0)
			{
				logMsgDbLn2("modify| depth is", "zero / root");

			}

			if (mi.bookmarkType == bookmarkType.LEAF)
			{
				logMsgDbLn2("modify| item is", "LEAF");
				logMsgDbLn2("modify| path", mi.fileItem.outlinePath);
			}
			else if (mi.bookmarkType == bookmarkType.BRANCH)
			{
				logMsgDbLn2("modify| item is", "BRANCH");

			}
			else
			{
				logMsgDbLn2("modify| item is", "INVALID");
				return;
			}

			logMsgDbLn2("modify| bktitle", mi.bookmarkTitle);
		}
	}
}