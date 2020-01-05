// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SheetIdBase.cs
// Created:      -- ()

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static Sylvester.Support.Support;

namespace Sylvester.FileSupport
{
	public abstract class SheetId : INotifyPropertyChanged
	{
		protected const string SEARCH_PATTERN_A = @"[.-]| +";
		protected const string SUBST_PATTERN_A = @" ";

		protected const string SEARCH_PATTERN_B = @"(?<=[A-Z])([ -]+)(?=[0-9])";
		protected const string SUBST_PATTERN_B = @"";

		protected KeyValuePair<Regex, string>[] ReplacePatterns =
		{
			new KeyValuePair<Regex, string>(
				new Regex(SEARCH_PATTERN_A, RegexOptions.Compiled | RegexOptions.Singleline),
				SUBST_PATTERN_A
				),
			new KeyValuePair<Regex, string>(
				new Regex(SEARCH_PATTERN_B, RegexOptions.Compiled | RegexOptions.Singleline),
				SUBST_PATTERN_B
				)
		};

		protected Regex regex = new Regex(SEARCH_PATTERN_A, RegexOptions.Compiled | RegexOptions.Singleline);

		public static bool ForceUpperCaseName { get; set; } = false;
		public static bool ForceWordCapName { get; set; } = true;

		protected Route fullFileRoute;
		protected string phaseBldg;
		protected string phaseBldgSep;
		protected string sheetID;
		protected string separator;
		protected string sheetName;
		protected string comment;
		protected string adjustedSheetID;


		public SheetId()
		{
			fullFileRoute = Route.Invalid;

			phaseBldg = "";
			phaseBldgSep = "";
			sheetID = "";
			separator = "";
			sheetName = "";
			comment = "";
			adjustedSheetID = "";
		}

		public Route FullFileRoute
		{
			get => fullFileRoute;
			set
			{
				fullFileRoute = value;
				OnPropertyChange();

				Support.Support.ParseFile(this);
			}
		}

		public string SheetID
		{
			get => sheetID;
			set
			{
				sheetID = value;
				OnPropertyChange();

				SheetNumber = value;

				AdjustedSheetID = AdjustSheetNumber(sheetID);
			}
		}

		public string FilePath => fullFileRoute.Path;
		public string FileName => fullFileRoute.FileName;

		public string PhaseBldg
		{
			get => phaseBldg;
			set
			{
				phaseBldg = value;
				SheetNumber = value;
			}
		}

		public string PhaseBldgSep
		{
			get => phaseBldgSep;
			set
			{
				phaseBldgSep = value;
				SheetNumber = value;
			}
		}

		public string SheetNumber
		{
			get { return phaseBldg + phaseBldgSep + (sheetID ?? "n/a"); }
			set
			{
				// value is not needed - just ignore
				OnPropertyChange();
			}
		}

		public string AdjustedSheetID
		{
			get => adjustedSheetID;
			set
			{
				adjustedSheetID = value;
				OnPropertyChange();
			}
		}

		public string Separator
		{
			get => separator;
			set => separator = value;
		}

		public string SheetName
		{
			get => sheetName ?? "n/a";
			set
			{
				sheetName = value;
				if (ForceUpperCaseName)
				{
					sheetName = sheetName.ToUpper();
				}
				else if (ForceWordCapName)
				{
					sheetName = ToCapEachWord(sheetName);
				}

				OnPropertyChange();
			}
		}

		public string Comment
		{
			get => comment;
			set
			{
				comment = value;
				OnPropertyChange();
			}
		}

		protected string AdjustSheetNumber(string sheetId)
		{
			string result = sheetId;

			for (int i = 0; i < ReplacePatterns.Length; i++)
			{
				result =
					ReplacePatterns[i].Key.Replace(result, ReplacePatterns[i].Value).Trim();
			}

			return result;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return phaseBldg.PadRight(3)
				+ " | " + phaseBldgSep.PadRight(4)
				+ " | " + sheetID.PadRight(10)
				+ (" (" + adjustedSheetID + ")").PadRight(10)
				+ " | " + separator.PadRight(6)
				+ " | " + sheetName
				+ " ( " + comment + ")";
		}
	}
}