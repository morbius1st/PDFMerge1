// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SheetIdBase.cs
// Created:      -- ()

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Sylvester.FileSupport {

	public class SheetIdTest : INotifyPropertyChanged
	{
		protected const string SEARCH_PATTERN = @"[.-]";
		protected const string SUBST_PATTERN = @" ";

		public static bool ForceUpperCaseName { get; set; } = true;

		protected Route fullFileName;
		protected string sheetNumber;
		protected string separator;
		protected string sheetName;
		protected string comment;
		protected string adjustedSheetNumber;

		protected Regex regex = new Regex(SEARCH_PATTERN, RegexOptions.Compiled | RegexOptions.Singleline);

		public SheetIdTest()
		{
			FullFileName = Route.Invalid;
			SheetNumber = "";
			SheetName = "";
			Comment = "";

		}

		public Route FullFileName
		{
			get => fullFileName;
			set
			{
				fullFileName = value;
				OnPropertyChange();
			}
		}

		public string FilePath => fullFileName.Path;
		public string FileName => fullFileName.FileName;

		public string SheetNumber
		{
			get => sheetNumber;
			set
			{
				sheetNumber = value;
				OnPropertyChange();

				AdjustedSheetNumber = AdjustSheetNumber(sheetNumber);
			}
		}

		public string AdjustedSheetNumber
		{
			get => adjustedSheetNumber;
			set
			{
				adjustedSheetNumber = value;
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
			get => sheetName;
			set
			{
				sheetName = value;
				if (ForceUpperCaseName)
				{
					sheetName = sheetName.ToUpper();
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
			return regex.Replace(sheetId, SUBST_PATTERN) + "-test";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}