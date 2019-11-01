#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
		private static DebugSupport _instance;

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

			foreach (FileItem fi in fl)
			{
				logMsgDbLn2("");
				logMsgDbLn2("********", "*******");
				logMsgDbLn2("name", fi.getName());
				logMsgDbLn2("outline path", fi.outlinePath);
				logMsgDbLn2("directory", fi.getDirectory());
				logMsgDbLn2("outline directory", fi.getOutlineDirectory());

				logMsgDbLn2("");

				fi.outlinePath = AdjustOutlinePerFile(fi.outlinePath, fi.getName());
				logMsgDbLn2("revised outline directory", fi.getOutlineDirectory());

			}

			return fl;
		}

		private string Arch   = @"^[A-Z] ?A";
		private string Struct = @"^[A-Z] ?S";
		private string Mech   = @"^[A-Z] ?M";
		private string Plumb  = @"^[A-Z] ?P";
		private string Title  = @"^[A-Z] ?T";
		private string Cover  = @"^[A-Z] ?C";
		private string Civil  = @"^C ?\d";

		internal struct MatchItem
		{
			public Regex Pattern { get; set; }
			public string BookmarkTitle { get; set; }

			public MatchItem(string pattern, string title)
			{
				Pattern = new Regex(pattern, RegexOptions.Compiled);
				BookmarkTitle = title;
			}
		}

		private List<MatchItem> BookmarkTitles = new List<MatchItem>();

		internal void MatchList()
		{
			BookmarkTitles.Add(new MatchItem(Arch, "070 Arch"));
			BookmarkTitles.Add(new MatchItem(Struct, "110 Struct"));
			BookmarkTitles.Add(new MatchItem(Cover, "000 Cover"));
			BookmarkTitles.Add(new MatchItem(Title, "000 Title"));
			BookmarkTitles.Add(new MatchItem(Civil, "010 Civil"));
			BookmarkTitles.Add(new MatchItem(Plumb, "130 Plumb"));
			BookmarkTitles.Add(new MatchItem(Mech, "150 Mech"));
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