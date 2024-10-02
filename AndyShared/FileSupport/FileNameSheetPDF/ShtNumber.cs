#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AndyShared.FileSupport.FileNameSheetPDF;
using JetBrains.Annotations;
using UtilityLibrary;
using static AndyShared.FileSupport.FileNameParseStatusCodes;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers.ShtIdType2;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers.ShtIdClass2;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;

#endregion

// user name: jeffs
// created:   9/14/2024 10:40:18 PM

namespace AndyShared.FileSupport.FileNameSheetPDF
{
	
	public class ShtNumber : INotifyPropertyChanged
	{
		// private static int idx = 0;

		private string origSheetNumber;

		private string parsedSheetNumber;
		private string parsedSheetId;

		private FileNameSheetIdentifiers.ShtIdType2 shtIdType;
		private FileNameParseStatusCodes statusCode = SC_NONE;

		public ShtNumber(string shtNum, bool parse = false)
		{
			// Index = idx++;

			ConfigFileNameComps();

			OrigSheetNumber = shtNum;

			if (parse)
			{
				ParseShtNumber();
			}
		}

		// public int Index { get; }

		// used
		public string OrigSheetNumber
		{
			get => origSheetNumber ?? " ";
			private set
			{
				origSheetNumber = value;
				OnPropertyChange();
			}
		}

		// used
		public string ParsedSheetNumber => parsedSheetNumber ?? "";

		// used
		public string ParsedSheetId => parsedSheetId ?? "";

		// used
		public string SheetNumberFromComps
		{
			get
			{
				getParsedShtNum();

				return parsedSheetNumber;
			}
		}

		// used
		public string SheetIdFromComps
		{
			get
			{
				getParsedShtId();

				return parsedSheetId;
			}
		}

		// used
		public FileNameSheetIdentifiers.ShtIdType2 SheetIdType 
		{
			get => shtIdType;
			private set
			{
				shtIdType = value;
				OnPropertyChange();
			}
		}

		// used
		public FileNameParseStatusCodes StatusCode
		{
			get => statusCode;
			private set
			{
				if (value == statusCode) return;
				statusCode = value;
				OnPropertyChange();

			}
		}

		// used
		public bool IsPhaseBldg => !PhaseBldg.IsVoid();

		// used
		public bool IsParseGood => parsedSheetNumber?.Equals(origSheetNumber) ?? false;

		// used
		// sheet number components
		public List<string> ShtNumComps { get; private set; }

		// all used
		public string PhaseBldg      => ShtNumComps[VI_PHBLDG]        ;
		// public string PhaseBldgSep   => ShtNumComps[VI_PBSEP
		public string Discipline     => ShtNumComps[VI_DISCIPLINE]    ;
		public string Seperator0     => ShtNumComps[VI_SEP0]          ;
		public string Category       => ShtNumComps[VI_CATEGORY]      ;
		public string Seperator1     => ShtNumComps[VI_SEP1]          ;
		public string Subcategory    => ShtNumComps[VI_SUBCATEGORY]   ;
		public string Seperator2     => ShtNumComps[VI_SEP2]          ;
		public string Modifier       => ShtNumComps[VI_MODIFIER]      ;
		public string Seperator3     => ShtNumComps[VI_SEP3]          ;
		public string Submodifier    => ShtNumComps[VI_SUBMODIFIER]   ;
		public string Seperator4     => ShtNumComps[VI_SEP4]          ;
		public string Identifier     => ShtNumComps[VI_IDENTIFIER]    ;
		public string Seperator5     => ShtNumComps[VI_SEP5]          ;
		public string Subidentifier  => ShtNumComps[VI_SUBIDENTIFIER] ;

		// public


		private bool ParseShtNumber()
		{
			if (!okToParse()) return false;

			Match match = FileNameSheetParser.Instance.MatchShtNumber(origSheetNumber);

			if (!match.Success)
			{
				StatusCode = FileNameParseStatusCodes.SC_PARSER_MATCH_FAILED;
				return false;
			}

			GroupCollection g = match.Groups;

			parsedSheetNumber = g[N_SHTNUM]?.Value ?? "";
			parsedSheetId = g[N_SHTID]?.Value ?? "";

			if (!FileNameSheetParser.Instance.extractShtNumComps3(this, g))
			{
				StatusCode = FileNameParseStatusCodes.SC_PARSER_COMP_EXTRACT_FAILED;
				return false;
			}

			return true;
		}

		private bool okToParse()
		{
			if (origSheetNumber.IsVoid())
			{
				StatusCode = FileNameParseStatusCodes.SC_VOID_SHTNUM;
				return false;
			}

			if (!FileNameSheetParser.Instance.IsConfigd)
			{
				StatusCode = FileNameParseStatusCodes.SC_PARSER_NOT_CONFIGD;
				return false;
			}

			return true;
		}

		public void SetShtIdType(int value)
		{
			if (IsPhaseBldg) value += (int) FileNameSheetIdentifiers.ShtIdType2.ST_IS_PHBLD;

			SheetIdType = (FileNameSheetIdentifiers.ShtIdType2) value;
		}

		public void NotifyChanges()
		{
			OnPropertyChange(nameof(ParsedSheetNumber));
			OnPropertyChange(nameof(ParsedSheetId));

			OnPropertyChange(nameof(SheetNumberFromComps));
			OnPropertyChange(nameof(SheetIdFromComps));
			OnPropertyChange(nameof(IsPhaseBldg));
			OnPropertyChange(nameof(IsParseGood));
			OnPropertyChange(nameof(ShtNumComps));

			// PB
			OnPropertyChange(nameof(PhaseBldg));

			// sheet Id
			OnPropertyChange(nameof(Discipline));
			OnPropertyChange(nameof(Seperator0));
			OnPropertyChange(nameof(Category));
			OnPropertyChange(nameof(Seperator1));
			OnPropertyChange(nameof(Subcategory));
			OnPropertyChange(nameof(Seperator2));
			OnPropertyChange(nameof(Modifier));
			OnPropertyChange(nameof(Seperator3));
			OnPropertyChange(nameof(Submodifier));
			OnPropertyChange(nameof(Seperator4));
			OnPropertyChange(nameof(Identifier));
			OnPropertyChange(nameof(Seperator5));
			OnPropertyChange(nameof(Subidentifier));
		}

		// private 

		private void ConfigFileNameComps()
		{
			ShtNumComps = new List<string>();

			for (int i = VI_MIN; i < VI_COUNT; i++)
			{
				ShtNumComps.Add(null);
			}

		}

		private void getParsedShtNum()
		{
			if (!parsedSheetNumber.IsVoid()) return;

			StringBuilder result = new StringBuilder("");

			if (!PhaseBldg.IsVoid()) result.Append(PhaseBldg).Append(N_PBSEP);

			getParsedShtId();

			result.Append(parsedSheetId);

			parsedSheetNumber = result.ToString();
		}

		private void getParsedShtId()
		{
			if (!parsedSheetId.IsVoid()) return;

			StringBuilder result = new StringBuilder("");

			for (int i = VI_DISCIPLINE; i < VI_SUBIDENTIFIER+1; i++)
			{
				if (ShtNumComps[i].IsVoid()) break;
				result.Append(ShtNumComps[i]);
			}

			parsedSheetId = result.ToString();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		
		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return $"this is {nameof(OrigSheetNumber)}";
		}
	}
}
