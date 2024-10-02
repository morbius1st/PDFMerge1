#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtilityLibrary;

using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;

#endregion

// username: jeffs
// created:  9/29/2024 7:52:22 AM

namespace AndyShared.FileSupport.FileNameSheetPDF
{
	public class FileExtensionPdfClassifier : FileExtensionClassifier
	{
		public override string[] fileTypes { get; set; } = {"pdf"};
	}

	public class FileNameSheetParser
	{
	#region private fields

		private List<int> compLengths;

		private Regex shtFileNamePattern;

		private static readonly Lazy<FileNameSheetParser> instance =
			new Lazy<FileNameSheetParser>(() => new FileNameSheetParser());

	#endregion

	#region ctor

		private FileNameSheetParser()
		{
			Config();
		}

	#endregion

	#region public properties

		public static FileNameSheetParser Instance => instance.Value;

		public string PdfFileNamePattern { get; private set; }

		public string SpecialDisciplines { get; set; }

		public bool IsConfigd => !PdfFileNamePattern.IsVoid();

		public List<int> CompLengths
		{
			get => compLengths;
			set => compLengths = value;
		}

	#endregion

	#region primary methods

		// version 3 - use this
		public Match MatchShtNumber(string  shtNum)
		{
			return shtFileNamePattern.Match(shtNum);
		}

		public bool extractShtNumComps3(ShtNumber shtNumber, GroupCollection g)
		{
			Group grp;
			string grpName;
			int idx;

			try
			{
				foreach (KeyValuePair<int, ShtNumComps2> kvp in FileNameSheetIdentifiers.SheetNumComponentData)
				{
					if (kvp.Value.ItemClass == ShtIdClass2.SC_IGNORE) continue;

					idx = kvp.Key;
					grpName = kvp.Value.GroupId;

					grp = g[grpName];

					if (idx > VI_SEP0 && grp.Value.IsVoid() || idx == VI_SUBIDENTIFIER)
					{
						shtNumber.SetShtIdType((idx - 2) / 2);

						if (idx != VI_SUBIDENTIFIER) break;
					}

					shtNumber.ShtNumComps[idx] = grp.Value;

					updateCompLength(idx, grp.Value);
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

	#endregion

	#region public methods

		public void Config()
		{
			initCompLengths(FileNameSheetIdentifiers.VI_COMP_COUNT);
		}

		public void CreateFileNamePattern()
		{
			string patternPrefix =
				@"^(?<SheetNum>(?<PhBldgid>(?>(?:[0-9]{0,3}[A-Z]{0,3}(?= [^0-9])))?) ?(?<SheetId>(?<Discipline>(?>";
			string patternSuffix =
				@"[A-Z]{0,3}(?=[\-\. 0-9]|$)))(?<sep0>[\-\. ]?)(?<Category>\w*)(?<sep1>[\-\.]?)(?<SubCategory>\w*)(?<sep2>[\-\.]?)(?<Modifier>(?>\w*?(?=[\-\.]|$))*)(?<sep3>[\-\.]?)(?<SubModifier>(?>\w*?(?=[\-\.]|$))*)(?<sep4>[\-\.]?)(?<Identifier>(?>\w*?(?=[\-\.]|$))*)(?<sep5>[\-\.]?)(?<SubIdentifier>\w*)))(?>\s-\s(?<SheetTitle>.*))*";

			PdfFileNamePattern = $"{patternPrefix}{SpecialDisciplines}{patternSuffix}";

			shtFileNamePattern =
				new Regex(PdfFileNamePattern, RegexOptions.Compiled | RegexOptions.Singleline);
		}

		public void CreateSpecialDisciplines(List<string> specialDisciplines)
		{
			SpecialDisciplines = "";

			if (specialDisciplines == null || specialDisciplines.Count == 0) return;

			StringBuilder sb = new StringBuilder("");

			foreach (string s in specialDisciplines)
			{
				sb.Append(s).Append("|");
			}

			SpecialDisciplines = sb.ToString();
		}

		public bool SplitFileName(FileNameSimple file, out string shtNumber, out string shtTitle)
		{
			shtNumber = "";
			shtTitle = "";

			if (file == null) return false;

			string[] parts = file.FileNameNoExt.Split(new [] { " - " }, StringSplitOptions.None);

			if (parts.Length != 2 ) return false;

			shtNumber = parts[0];
			shtTitle = parts[1];

			return true;
		}

	#endregion

	#region private methods

		private void updateCompLength(int idx, string compStr)
		{
			if (compStr.Length > CompLengths[idx]) CompLengths[idx] = compStr.Length;
		}

		private void initCompLengths(int count)
		{
			CompLengths = new List<int>(count);

			for (int i = 0; i < count; i++)
			{
				CompLengths.Add(-1);
			}
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(FileNameSheetParser)}";
		}

	#endregion
	}
}