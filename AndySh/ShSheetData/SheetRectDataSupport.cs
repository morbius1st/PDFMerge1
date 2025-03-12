using System;
using System.Collections.Generic;
using System.Text;
using UtilityLibrary;
using static ShSheetData.SheetRectType;
using static ShSheetData.SheetRectId;


// user name: jeffs
// created:   5/11/2024 1:22:54 PM

namespace ShSheetData
{
	[Flags]
	public enum SheetRectType
	{
		SRT_NA              = 0,    // 0 - not available
		SRT_LOCATION        = 2<<0, // 1 - location (boundary - e.g. scanning zone)
		SRT_TEXT            = 2<<1, // 2 - has extra text - record text information
		SRT_LINK            = 2<<2, // 4 - has link text - does not use text information
		SRT_BOX             = 2<<3, // 8 - record box information
		SRT_TEXT_N_BOX      = SRT_TEXT | SRT_BOX,
		SRT_LINK_N_BOX      = SRT_LINK | SRT_BOX,  
		SRT_TEXT_LINK_N_BOX = SRT_TEXT |SRT_LINK| SRT_BOX,
	}

	public enum SheetRectId
	{
		SM_NA              = -1,
		SM_SHT             = 0 ,
		SM_XREF                ,

		SM_SHT_NUM             ,
		SM_SHT_NUM_FIND        ,
		SM_SHT_TITLE           ,

		SM_AUTHOR              ,
		SM_DISCLAIMER          ,
		SM_FOOTER              ,

		SM_BANNER_1ST          ,
		SM_BANNER_2ND          ,
		SM_BANNER_3RD          ,
		SM_BANNER_4TH          ,

		SM_WATERMARK1          ,
		SM_WATERMARK2          ,

		SM_OPT0                ,
		SM_OPT1                ,
		SM_OPT2                ,
		SM_OPT3                ,
		SM_OPT4                ,
		SM_OPT5                ,
		SM_OPT6                ,
		SM_OPT7                ,
		SM_OPT8                ,
		SM_OPT9                ,
		SM_OPT10               ,

		SM_PAGE_TITLE			// to add a lable on a created page
	}

	public class TextDecorations
	{
		public static int NORMAL { get; } = 0;
		public static int UNDERLINE { get; } = 1 << 1;
		public static int LINETHROUGH { get; } = 1 << 2;

		public static bool HasUnderline(int decoration)
		{
			return (decoration & UNDERLINE) > 0;
		}

		public static bool HasLinethrough(int decoration)
		{
			return (decoration & LINETHROUGH) > 0;
		}

		public static string FormatTextDeco(int deco)
		{
			if (deco == NORMAL) { return nameof(NORMAL); }

			StringBuilder result = new StringBuilder();

			if (HasUnderline(deco)) result.Append(nameof(UNDERLINE));
			if (HasLinethrough(deco)) result.Append(nameof(LINETHROUGH));

			return result.ToString();
		}
	}

	public class SheetRectConfigData<T>
	{
		public SheetRectType Type { get; private set; }
		public T Id { get; private set; }

		public SheetRectConfigData(SheetRectType type, T id)
		{
			Type = type;
			Id = id;
		}
	}

	public class SheetRectDataSupport
	{
		// common

		public static SheetRectType GetRecType(string name, out SheetRectId id)
		{
			DM.InOut0();

			id = SM_NA;

			if (OptRectIdXref.ContainsKey(name))
			{
				id = OptRectIdXref[name].Id;
				return OptRectIdXref[name].Type;
			}

			if (!ShtRectIdXref.ContainsKey(name)) return SRT_NA;

			id = ShtRectIdXref[name].Id;
			return ShtRectIdXref[name].Type;
		}

		// sheet rects
		public static int ShtRectsMinQty => ShtRectIdXref.Count - 1 - 3 - 1; // don't count NA, incl only 1 banner and only 1 watermark

		public static SheetRectId GetShtRectId(string name)
		{
			if (!ShtRectIdXref.ContainsKey(name)) return SheetRectId.SM_NA;

			return ShtRectIdXref[name].Id;
		}

		public static string GetShtRectName(SheetRectId id)
		{
			foreach (KeyValuePair<string, SheetRectConfigData<SheetRectId>> kvp in ShtRectIdXref)
			{
				if (id == kvp.Value.Id) return kvp.Key;
			}

			return null;
		}

		public static SheetRectType GetShtRectType(string name)
		{
			if (!ShtRectIdXref.ContainsKey(name)) return SheetRectType.SRT_NA;

			return ShtRectIdXref[name].Type;
		}

		public static Dictionary<string, SheetRectConfigData<SheetRectId>> ShtRectIdXref { get; } = new Dictionary<string, SheetRectConfigData<SheetRectId>>()
		{
			{ "Not Available"    , new SheetRectConfigData<SheetRectId>(SRT_NA,SM_NA) }                     , // 0  default / not configured
			{ "Sheet Boundary"   , new SheetRectConfigData<SheetRectId>(SRT_NA,SM_SHT) }                    , // 1  size of sheet
			{ "SHEET XREF"       , new SheetRectConfigData<SheetRectId>(SRT_BOX,SM_XREF) }                  , // 2  the box style for sheet xrefs
			{ "SHEET NUMBER FIND", new SheetRectConfigData<SheetRectId>(SRT_LOCATION,SM_SHT_NUM_FIND) }     , // 3  limits to find the sheet number
			{ "SHEET TITLE"      , new SheetRectConfigData<SheetRectId>(SRT_LOCATION,SM_SHT_TITLE) }        , // 4  where to find the title of the sheet

			{ "SHEET NUMBER"     , new SheetRectConfigData<SheetRectId>(SRT_LINK_N_BOX,SM_SHT_NUM) }        , // 5  where to place the sheet number box
			{ "AUTHOR"           , new SheetRectConfigData<SheetRectId>(SRT_LINK_N_BOX,SM_AUTHOR) }         , // 6  the information for the author box
			{ "DISCLAIMER"       , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_DISCLAIMER) }, // 7  the information for the disclaimer
			{ "FOOTER"           , new SheetRectConfigData<SheetRectId>(SRT_TEXT_N_BOX,SM_FOOTER) }         , // 8  the information for the footer
			{ "FIRST BANNER"     , new SheetRectConfigData<SheetRectId>(SRT_TEXT_N_BOX,SM_BANNER_1ST) }     , // 9  the information for the banner
			{ "SECOND BANNER"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_N_BOX,SM_BANNER_2ND) }     , // 10 ditto
			{ "THIRD BANNER"     , new SheetRectConfigData<SheetRectId>(SRT_TEXT_N_BOX,SM_BANNER_3RD) }     , // 11 ditto
			{ "FOURTH BANNER"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_N_BOX,SM_BANNER_4TH) }     , // 12 ditto
			{ "WATERMARK1"       , new SheetRectConfigData<SheetRectId>(SRT_TEXT,SM_WATERMARK1) }           , // 13 the information for the main watermark
			{ "WATERMARK2"       , new SheetRectConfigData<SheetRectId>(SRT_TEXT,SM_WATERMARK2) }           , // 14 the information for the title block watermark
		};

		// optional rectangles
		public static int OptRectsQty => OptRectIdXref.Count - 1; // don't count NA

		public static SheetRectId GetOptRectId(string name)
		{
			if (!OptRectIdXref.ContainsKey(name)) return SM_NA;

			return OptRectIdXref[name].Id;
		}

		public static string GetOptRectName(SheetRectId id)
		{
			foreach (KeyValuePair<string, SheetRectConfigData<SheetRectId>> kvp in OptRectIdXref)
			{
				if (id == kvp.Value.Id) return kvp.Key;
			}

			return null;
		}

		public static SheetRectType GetOptRectType(string name)
		{
			if (!OptRectIdXref.ContainsKey(name)) return SheetRectType.SRT_NA;

			return OptRectIdXref[name].Type;
		}

		public static Dictionary<string, SheetRectConfigData<SheetRectId>> OptRectIdXref { get; } = new Dictionary<string, SheetRectConfigData<SheetRectId>>()
		{
			{ "Not Available" , new SheetRectConfigData<SheetRectId>(SRT_NA,SM_NA) }               , // default / not configured
			{ "OPTIONAL 0"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT0) }, // the information for an optional box
			{ "OPTIONAL 1"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT1) }, // ditto
			{ "OPTIONAL 2"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT2) }, // ditto
			{ "OPTIONAL 3"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT3) }, // ditto
			{ "OPTIONAL 4"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT4) }, // ditto
			{ "OPTIONAL 5"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT5) }, // ditto
			{ "OPTIONAL 6"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT6) }, // ditto
			{ "OPTIONAL 7"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT7) }, // ditto
			{ "OPTIONAL 8"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT8) }, // ditto
			{ "OPTIONAL 9"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT9) }, // ditto
			{ "OPTIONAL10"    , new SheetRectConfigData<SheetRectId>(SRT_TEXT_LINK_N_BOX,SM_OPT10) }, // ditto
		};

		public override string ToString()
		{
			return $"this is {nameof(SheetRectDataSupport)}";
		}
	}

}