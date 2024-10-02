#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static Test4.Support.SheetIdentifiers3.ShtIdType2;
using static Test4.Support.SheetIdentifiers3.ShtIdClass2;

#endregion

// user name: jeffs
// created:   9/12/2024 11:22:17 PM

namespace Test4.Support
{
	public class SheetIdentifiers3
	{
		static SheetIdentifiers3()
		{
			initialized = true;

			_fileNameSheetIdentifiers = new SheetIdentifiers3();

			// string a = ShtIds.SheetNumberComponentTitles[0].Name;
		}

		private static bool initialized = false;

		private static SheetIdentifiers3 _fileNameSheetIdentifiers;

		public static SheetIdentifiers3 ShtIds2
		{
			get
			{
				if (!initialized)
				{
					initialized = true;

					_fileNameSheetIdentifiers = new SheetIdentifiers3();
				}

				return _fileNameSheetIdentifiers;
			}
		}

		public enum FileTypeSheetPdf
		{
			INVALID = -1,
			UNASSIGNED = 0,
			OTHER,
			NON_SHEET_PDF,
			SHEET_PDF,
		}

		public static Dictionary<string, int> ShtNumComponentsXref = new Dictionary<string, int>()
		{
			    { CN_PHBLD    , SI_PHBLDG       },
				{ CN_SEPPB    , SI_PBSEP        },
				{ CN_DISCP    , SI_DISCIPLINE   },
				{ CN_SEP0     , SI_SEP0         },
				{ CN_CAT      , SI_CATEGORY     },
				{ CN_SEP1     , SI_SEP1         },
				{ CN_SUBCAT   , SI_SUBCATEGORY  },
				{ CN_SEP2     , SI_SEP2         },
				{ CN_MOD      , SI_MODIFIER     },
				{ CN_SEP3     , SI_SEP3         },
				{ CN_SUBMOD   , SI_SUBMODIFIER  },
				{ CN_SEP4     , SI_SEP4         },
				{ CN_IDF      , SI_IDENTIFIER   },
				{ CN_SEP5     , SI_SEP5         },
				{ CN_SUBIDF   , SI_SUBIDENTIFIER},
				{ CN_SHTID    , SI_SHTID        },
				{ CN_SHTTITLE , SI_SHTTITLE     },

		};

		// @formatter:off — disable formatter after this line
	#pragma warning disable format
		public static SortedDictionary<int, ShtNumComps2> SheetNumComponents { get; } =
			new SortedDictionary<int, ShtNumComps2>()
			{
			{ SI_PHBLDG       , new ShtNumComps2(CN_PHBLDG  , CN_PHBLD   , new NeumonicString2(            "(" , "A", " x.x-xz)"), "Ph/Bld"  , SI_PHBLDG          , ST_IS_PHBLD  , SC_COMP_ID_TITLE)}, // 0
			{ SI_PBSEP        , new ShtNumComps2(CN_SEPPB   , CN_SEPPB   , new NeumonicString2("" , "", "")                      , "spb"     , SI_PBSEP           , ST_NA        , SC_IGNORE)}, // 
			{ SI_DISCIPLINE   , new ShtNumComps2(CN_DISCP   , CN_DISCP   , new NeumonicString2(          "(x " , "A", "x.x-xz)") , "Disc"    , SI_DISCIPLINE      , ST_NOT_PHBLD , SC_COMP_ID_TITLE)}, // 2
			{ SI_SEP0         , new ShtNumComps2(CN_SEP0    , CN_SEP0    , new NeumonicString2("" , "", "")                      , "s0"      , SI_SEP0            , ST_NA        , SC_COMP_ID_SEP)}, // 
			{ SI_CATEGORY     , new ShtNumComps2(CN_CAT     , CN_CAT     , new NeumonicString2(         "(x y" , "0", ".x-xz)")  , "Cat"     , SI_CATEGORY        , ST_TYPE01    , SC_COMP_ID_TITLE)}, // 4
			{ SI_SEP1         , new ShtNumComps2(CN_SEP1    , CN_SEP1    , new NeumonicString2("" , "", "")                      , "s1"      , SI_SEP1            , ST_NA        , SC_COMP_ID_SEP)}, // 
			{ SI_SUBCATEGORY  , new ShtNumComps2(CN_SUBCAT  , CN_SUBCAT  , new NeumonicString2(       "(x yx." , "0", "-xz)")    , "SubCat"  , SI_SUBCATEGORY     , ST_TYPE02    , SC_COMP_ID_TITLE)}, // 6
			{ SI_SEP2         , new ShtNumComps2(CN_SEP2    , CN_SEP2    , new NeumonicString2("" , "", "")                      , "s2"      , SI_SEP2            , ST_NA        , SC_COMP_ID_SEP)}, // 
			{ SI_MODIFIER     , new ShtNumComps2(CN_MOD     , CN_MOD     , new NeumonicString2(     "(x yx.x-" , "0", ")")       , "Mod"     , SI_MODIFIER        , ST_TYPE03    , SC_COMP_ID_TITLE)}, // 8
			{ SI_SEP3         , new ShtNumComps2(CN_SEP3    , CN_SEP3    , new NeumonicString2("" , "", "")                      , "s3"      , SI_SEP3            , ST_NA        , SC_COMP_ID_SEP)}, // 
			{ SI_SUBMODIFIER  , new ShtNumComps2(CN_SUBMOD  , CN_SUBMOD  , new NeumonicString2    ("(x yx.x-x" , "A", ")")       , "SubMod"  , SI_SUBMODIFIER     , ST_TYPE04    , SC_COMP_ID_TITLE)}, // 10
			{ SI_SEP4         , new ShtNumComps2(CN_SEP4    , CN_SEP4    , new NeumonicString2("" , "", "")                      , "s4"      , SI_SEP4            , ST_NA        , SC_COMP_ID_SEP)}, // 
			{ SI_IDENTIFIER   , new ShtNumComps2(CN_IDF     , CN_IDF     , new NeumonicString2(  "(x yx.x-xz(" , "A", "))")      , "Idf"     , SI_IDENTIFIER      , ST_TYPE05    , SC_COMP_ID_TITLE)}, // 12
			{ SI_SEP5         , new ShtNumComps2(CN_SEP5    , CN_SEP5    , new NeumonicString2("" , "", "")                      , "s5"      , SI_SEP5            , ST_NA        , SC_COMP_ID_SEP)}, // 
			{ SI_SUBIDENTIFIER, new ShtNumComps2(CN_SUBIDF  , CN_SUBIDF  , new NeumonicString2("(x yx.x-xz(x/" , "A", "))")      , "SubId"   , SI_SUBIDENTIFIER   , ST_TYPE06    , SC_COMP_ID_TITLE)}, // 14
			{ SI_SHTNUM       , new ShtNumComps2(CN_SHTNUM  , CN_SHTNUM  , new NeumonicString2("" , "", "")                      , "ShtNum"  , SI_SHTNUM          , ST_NA        , SC_IGNORE )}, 
			{ SI_SHTID        , new ShtNumComps2(CN_SHTID   , CN_SHTID   , new NeumonicString2("" , "", "")                      , "ShtId"   , SI_SHTID           , ST_NA        , SC_IGNORE )}, 
			{ SI_SHTTITLE     , new ShtNumComps2(CN_SHTTITLE, CN_SHTTITLE, new NeumonicString2("" , "", "")                      , "Title"   , SI_SHTTITLE        , ST_NA        , SC_IGNORE )}, 
			};
	#pragma warning restore format
		// @formatter:on — enable formatter after this line

		public const int SI_PHBLDG        = 0;  // 0
		public const int SI_PBSEP         = 1;  //
		public const int SI_DISCIPLINE    = 2;  // 1
		public const int SI_SEP0          = 3;  //
		public const int SI_CATEGORY      = 4;  // 2
		public const int SI_SEP1          = 5;  //
		public const int SI_SUBCATEGORY   = 6;  // 3
		public const int SI_SEP2          = 7;  //
		public const int SI_MODIFIER      = 8;  // 4
		public const int SI_SEP3          = 9;  // 
		public const int SI_SUBMODIFIER   = 10; // 5
		public const int SI_SEP4          = 11; // 
		public const int SI_IDENTIFIER    = 12; // 6
		public const int SI_SEP5          = 13; // 
		public const int SI_SUBIDENTIFIER = 14; // 7

		public const int SI_SHTNUM        = 21;
		public const int SI_SHTID         = 22;
		public const int SI_SHTTITLE      = 23;

		public const int SI_MIN = 0;
		public const int SI_MAX = SI_SHTTITLE;


		public const string CN_PHBLD    = "PhBldgid";
		public const string CN_PHBLDG   = "Phase/Building";
		public const string CN_DISCP    = "Discipline";
		public const string CN_CAT      = "Category";
		public const string CN_SUBCAT   = "SubCategory";
		public const string CN_MOD      = "Modifier";
		public const string CN_SUBMOD   = "SubModifier";
		public const string CN_IDF      = "Identifier";
		public const string CN_SUBIDF   = "SubIdentifier";

		public const string CN_SHTNUM   = "SheetNum";
		public const string CN_SHTID    = "SheetId";
		public const string CN_SHTTITLE = "SheetTitle";

		public const string CN_SEP0  = "sep0";
		public const string CN_SEP1  = "sep1";
		public const string CN_SEP2  = "sep2";
		public const string CN_SEP3  = "sep3";
		public const string CN_SEP4  = "sep4";
		public const string CN_SEP5  = "sep5";

		public const string CN_SEPPB = "spb";

		public const string STR_PBSEP = " ";


		public enum ShtIdType2
		{
			ST_NA     = -1,

			ST_NOT_PHBLD = 0,

			// 01 to 06 - non bldg / phase
			ST_TYPE01	= 1,	// Discipline + Category  (e.g. A1, or A 1 or A.1 or A-1)
			ST_TYPE02	= 2,	// type01 + Sub-Category
			ST_TYPE03	= 3,	// type02 + Modifier
			ST_TYPE04	= 4,	// type03 + Sub-Modifier
			ST_TYPE05	= 5,	// type04 + Identifier
			ST_TYPE06	= 6,	// type05 + Sub-Identifier

			ST_IS_PHBLD	= 10,

			// 11 to 16 - is bldg / phase
			ST_TYPE11	= 11, // Discipline + Category  (e.g. A1, or A 1 or A.1 or A-1)
			ST_TYPE12	= 12, // type01 + Sub-Category
			ST_TYPE13	= 13, // type02 + Modifier
			ST_TYPE14	= 14, // type03 + Sub-Modifier
			ST_TYPE15	= 15, // type04 + Identifier
			ST_TYPE16	= 16, // type05 + Sub-Identifier
		}

		public enum ShtIdClass2
		{
			SC_IGNORE,
			SC_COMP_ID_TITLE,
			SC_COMP_ID_SEP,
		}


		public struct ShtNumComps2
		{
			public string Name { get; private set; }
			public string GroupId { get; private set; }
			public NeumonicString2 Neumonic { get; private set; }
			public string AbbrevName { get; private set; }
			public int Index { get; private set; }
			public ShtIdType2 SheetIdType { get; private set; }
			public ShtIdClass2 ItemClass { get; private set; }

			public ShtNumComps2(string name, string grpId, NeumonicString2 neumonicStr, string abbrevName,
				int idx, ShtIdType2 shtType, ShtIdClass2 iClass)
			{
				Name = name;
				GroupId = grpId;
				Neumonic = neumonicStr;
				AbbrevName = abbrevName;
				Index = idx;
				SheetIdType = shtType;
				ItemClass = iClass;

			}
		}

		public struct NeumonicString2
		{
			public string Preface { get; private set; }
			public string Body { get; private set; }
			public string Suffix { get; private set; }

			public NeumonicString2(string preface, string body, string suffix)
			{
				Preface = preface;
				Body = body;
				Suffix = suffix;
			}
		}

	}
}