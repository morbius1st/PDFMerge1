#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers.ShtIdType2;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers.ShtIdClass2;
#endregion

// user name: jeffs
// created:   10/1/2020 7:16:21 AM

namespace AndyShared.FileSupport.FileNameSheetPDF
{
	public class FileNameSheetIdentifiers
	{
		static FileNameSheetIdentifiers()
		{
			initialized = true;

			_fileNameSheetIdentifiers = new FileNameSheetIdentifiers();

			_fileNameSheetIdentifiers.makeCollectView();
		}

		private static bool initialized = false;

		private static FileNameSheetIdentifiers _fileNameSheetIdentifiers;

		public static FileNameSheetIdentifiers ShtIds
		{
			get
			{
				if (!initialized)
				{
					initialized = true;

					_fileNameSheetIdentifiers = new FileNameSheetIdentifiers();
				}

				return _fileNameSheetIdentifiers;
			}
		}

		// // value index is not being used - spare
		// private List<ShtNumberCompName> SheetNumberComponentTitles { get; } = new List<ShtNumberCompName>()
		// {
		// 	new ShtNumberCompName(N_PHBLDG, new NeumonicString(            "(" , "A", " x.x-xz)"), "Ph/Bld", 5),
		// 	new ShtNumberCompName(N_DISCP , new NeumonicString(          "(x " , "A", "x.x-xz)") , "Disc"  , 10),
		// 	new ShtNumberCompName(N_CAT   , new NeumonicString(         "(x y" , "0", ".x-xz)")  , "Cat"   , 15),
		// 	new ShtNumberCompName(N_SUBCAT, new NeumonicString(       "(x yx." , "0", "-xz)")    , "SubCat", 20),
		// 	new ShtNumberCompName(N_MOD   , new NeumonicString(     "(x yx.x-" , "0", ")")       , "Mod"   , 25),
		// 	new ShtNumberCompName(N_SUBMOD, new NeumonicString    ("(x yx.x-x" , "A", ")")       , "SubMod", 30),
		// 	new ShtNumberCompName(N_ID    , new NeumonicString(  "(x yx.x-xz(" , "A", "))")      , "Id"    , 35),
		// 	new ShtNumberCompName(N_SUBID , new NeumonicString("(x yx.x-xz(x/" , "A", "))")      , "SubId" , 40),
		// };


		// public Dictionary<int, ShtNumComps2> SheetNumComponents { get; } = new Dictionary<int, ShtNumComps2>()
		// {
		// 	SheetNumComponentData[0]
		// };

		private void makeCollectView()
		{
			lcv = (ICollectionView) CollectionViewSource.GetDefaultView(SheetNumComponentData);

			lcv.Filter = new Predicate<object>(c => ((KeyValuePair<int, ShtNumComps2>) c).Value.SheetIdType != ST_NA);
		}

		public ICollectionView lcv { get; private set; }
		

			#pragma warning disable format
		public static SortedDictionary<int, ShtNumComps2> SheetNumComponentData { get; } =
			new SortedDictionary<int, ShtNumComps2>()
			{
				// 0
				{ VI_PHBLDG       , new ShtNumComps2(N_PHBLDG  , N_PHBLD   , new NeumonicString2(            "(" , "A", " x.x-xz)"), "Ph/Bld"  , VI_PHBLDG          , ST_IS_PHBLD  , SC_COMP_ID_TITLE)}, // 0
				{ VI_PBSEP        , new ShtNumComps2(N_SEPPB   , N_SEPPB   , new NeumonicString2("" , "", "")                      , "spb"     , VI_PBSEP           , ST_NA        , SC_IGNORE)}, // 
				// 1 
				{ VI_DISCIPLINE   , new ShtNumComps2(N_DISCP   , N_DISCP   , new NeumonicString2(          "(x " , "A", "x.x-xz)") , "Disc"    , VI_DISCIPLINE      , ST_NOT_PHBLD , SC_COMP_ID_TITLE)}, // 2
				{ VI_SEP0         , new ShtNumComps2(N_SEP0    , N_SEP0    , new NeumonicString2("" , "", "")                      , "s0"      , VI_SEP0            , ST_NA        , SC_COMP_ID_SEP)}, // 
				// 2
				{ VI_CATEGORY     , new ShtNumComps2(N_CAT     , N_CAT     , new NeumonicString2(         "(x y" , "0", ".x-xz)")  , "Cat"     , VI_CATEGORY        , ST_TYPE01    , SC_COMP_ID_TITLE)}, // 4
				{ VI_SEP1         , new ShtNumComps2(N_SEP1    , N_SEP1    , new NeumonicString2("" , "", "")                      , "s1"      , VI_SEP1            , ST_NA        , SC_COMP_ID_SEP)}, // 
				// 3
				{ VI_SUBCATEGORY  , new ShtNumComps2(N_SUBCAT  , N_SUBCAT  , new NeumonicString2(       "(x yx." , "0", "-xz)")    , "SubCat"  , VI_SUBCATEGORY     , ST_TYPE02    , SC_COMP_ID_TITLE)}, // 6
				{ VI_SEP2         , new ShtNumComps2(N_SEP2    , N_SEP2    , new NeumonicString2("" , "", "")                      , "s2"      , VI_SEP2            , ST_NA        , SC_COMP_ID_SEP)}, // 
				// 4
				{ VI_MODIFIER     , new ShtNumComps2(N_MOD     , N_MOD     , new NeumonicString2(     "(x yx.x-" , "0", ")")       , "Mod"     , VI_MODIFIER        , ST_TYPE03    , SC_COMP_ID_TITLE)}, // 8
				{ VI_SEP3         , new ShtNumComps2(N_SEP3    , N_SEP3    , new NeumonicString2("" , "", "")                      , "s3"      , VI_SEP3            , ST_NA        , SC_COMP_ID_SEP)}, // 
				// 5
				{ VI_SUBMODIFIER  , new ShtNumComps2(N_SUBMOD  , N_SUBMOD  , new NeumonicString2    ("(x yx.x-x" , "A", ")")       , "SubMod"  , VI_SUBMODIFIER     , ST_TYPE04    , SC_COMP_ID_TITLE)}, // 10
				{ VI_SEP4         , new ShtNumComps2(N_SEP4    , N_SEP4    , new NeumonicString2("" , "", "")                      , "s4"      , VI_SEP4            , ST_NA        , SC_COMP_ID_SEP)}, // 
				// 6
				{ VI_IDENTIFIER   , new ShtNumComps2(N_ID      , N_ID      , new NeumonicString2(  "(x yx.x-xz(" , "A", "))")      , "Idf"     , VI_IDENTIFIER      , ST_TYPE05    , SC_COMP_ID_TITLE)}, // 12
				{ VI_SEP5         , new ShtNumComps2(N_SEP5    , N_SEP5    , new NeumonicString2("" , "", "")                      , "s5"      , VI_SEP5            , ST_NA        , SC_COMP_ID_SEP)}, // 
				// 7
				{ VI_SUBIDENTIFIER, new ShtNumComps2(N_SUBID   , N_SUBID   , new NeumonicString2("(x yx.x-xz(x/" , "A", "))")      , "SubId"   , VI_SUBIDENTIFIER   , ST_TYPE06    , SC_COMP_ID_TITLE)}, // 14
				{ VI_SHTNUM       , new ShtNumComps2(N_SHTNUM  , N_SHTNUM  , new NeumonicString2("" , "", "")                      , "ShtNum"  , VI_SHTNUM          , ST_NA        , SC_IGNORE )}, 
				{ VI_SHTID        , new ShtNumComps2(N_SHTID   , N_SHTID   , new NeumonicString2("" , "", "")                      , "ShtId"   , VI_SHTID           , ST_NA        , SC_IGNORE )}, 
				{ VI_SHTTITLE     , new ShtNumComps2(N_SHTTITLE, N_SHTTITLE, new NeumonicString2("" , "", "")                      , "Title"   , VI_SHTTITLE        , ST_NA        , SC_IGNORE )}, 
			};
	#pragma warning restore format

		// new ShtNumberCompName("Sheet Number Type", "Number Type", 0),

		public const string N_PHBLD  = "PhBldgid";
		public const string N_PHBLDG = "Phase/Building";
		public const string N_DISCP  = "Discipline";
		public const string N_CAT    = "Category";
		public const string N_SUBCAT = "SubCategory";
		public const string N_MOD    = "Modifier";
		public const string N_SUBMOD = "SubModifier";
		public const string N_ID     = "Identifier";
		public const string N_SUBID  = "SubIdentifier";

		public const string N_PBSEP = " ";

		public const string N_SEP0  = "sep0";
		public const string N_SEP1  = "sep1";
		public const string N_SEP2  = "sep2";
		public const string N_SEP3  = "sep3";
		public const string N_SEP4  = "sep4";
		public const string N_SEP5  = "sep5";

		public const string N_SEPPB = "spb";

		// public const string SHTNAME= "SheetName";

		public const string N_SHTNUM   = "SheetNum";
		public const string N_SHTID    = "SheetId";
		public const string N_SHTTITLE = "SheetTitle";

		private const string N_SEP   = "sep";
		private const string N_SEPR  = "Separator";
		private const string PBSEP   = "pbsep";

		public enum FileTypeSheetPdf
		{
			INVALID = -1,
			UNASSIGNED = 0,
			OTHER,
			NON_SHEET_PDF,
			SHEET_PDF,
		}

		// indicates the sheet component type and 
		// is the index in the SheetComponentNames array
		// is the index in the master "ShtCompList" array
		public enum ShtCompTypes
		{
			UNASSIGNED = 0,
			PHBLDG = 1,
			TYPE10 = 2,
			TYPE20 = 3,
			TYPE30 = 4,
			TYPE40 = 5,
			IDENT  = 6,
			COUNT
		}

		public enum ShtIdType
		{
			ST_NA     = -1,

			ST_NOT_PHBLD = 0,
			// 01 to 06 - non bldg / phase
			ST_TYPE01 = 1,  // Discipline + Category  (e.g. A1, or A 1 or A.1 or A-1)
			ST_TYPE02 = 2,  // type01 + Sub-Category
			ST_TYPE03 = 3,  // type02 + Modifier
			ST_TYPE04 = 4,  // type03 + Sub-Modifier
			ST_TYPE05 = 5,  // type04 + Identifier
			ST_TYPE06 = 6,  // type05 + Sub-Identifier

			ST_PHBLD  = 10,

			// 11 to 16 - is bldg / phase
			ST_TYPE11 = 11, // Discipline + Category  (e.g. A1, or A 1 or A.1 or A-1)
			ST_TYPE12 = 12,  // type01 + Sub-Category
			ST_TYPE13 = 13,  // type02 + Modifier
			ST_TYPE14 = 14,  // type03 + Sub-Modifier
			ST_TYPE15 = 15,  // type04 + Identifier
			ST_TYPE16 = 16,  // type05 + Sub-Identifier
		}

		public enum ShtIdType2
		{
			UNASSIGNED = -2,
			ST_NA     = -1,

			ST_NOT_PHBLD = 0,

			// 01 to 06 - non bldg / phase
			ST_TYPE01	= 1, // Discipline + Category  (e.g. A1, or A 1 or A.1 or A-1)
			ST_TYPE02	= 2, // type01 + Sub-Category
			ST_TYPE03	= 3, // type02 + Modifier
			ST_TYPE04	= 4, // type03 + Sub-Modifier
			ST_TYPE05	= 5, // type04 + Identifier
			ST_TYPE06	= 6, // type05 + Sub-Identifier

			ST_IS_PHBLD	= 10,

			// 11 to 16 - is bldg / phase
			ST_TYPE11	= 11, // Discipline + Category  (e.g. A1, or A 1 or A.1 or A-1)
			ST_TYPE12	= 12, // type01 + Sub-Category
			ST_TYPE13	= 13, // type02 + Modifier
			ST_TYPE14	= 14, // type03 + Sub-Modifier
			ST_TYPE15	= 15, // type04 + Identifier
			ST_TYPE16	= 16, // type05 + Sub-Identifier
		}

		// shorthand access to the indices
		internal const int CI_SHTID = 2;
		internal const int CI_SHTNAME = 3;

		// the index in the final component array: ShtCompList
		internal const int CI_PHBLDG = 0; //
		internal const int CI_PBSEP = 1;  //

		// // the index in the final component value array: FileNameSheetPdf.SheetComps
		public const int VI_PHBLDG        = 0;  // 0
		public const int VI_PBSEP         = 1;  //
		public const int VI_DISCIPLINE    = 2;  // 1
		public const int VI_SEP0          = 3;  //
		public const int VI_CATEGORY      = 4;  // 2
		public const int VI_SEP1          = 5;  //
		public const int VI_SUBCATEGORY   = 6;  // 3
		public const int VI_SEP2          = 7;  //
		public const int VI_MODIFIER      = 8;  // 4
		public const int VI_SEP3          = 9;  // 
		public const int VI_SUBMODIFIER   = 10; // 5
		public const int VI_SEP4          = 11; // 
		public const int VI_IDENTIFIER    = 12; // 6
		public const int VI_SEP5          = 13; // 
		public const int VI_SUBIDENTIFIER = 14; // 7

		public const int VI_SORT_SUFFIX   = 15; // 8

		public const int VI_SHTNUM        = 15;
		public const int VI_SHTID         = 16;
		public const int VI_SHTTITLE      = 17;
		// public const int VI_SEP6          = 15; // 


		internal const int VI_COMP_COUNT = VI_SUBIDENTIFIER +1; // 28 (req'd # // +1 - zero based
		internal const int VI_COUNT = VI_SHTTITLE +1;      // 18 (req'd # // +1 - zero based
		internal const int VI_MIN = VI_PHBLDG;


		public struct CompNameInfo
		{
			public string Name { get; set; }
			public int Index { get; set; }
			public ShtIdType Type { get; set; }

			public CompNameInfo(string name, int index, ShtIdType type)
			{
				Name = name;
				Index = index;
				Type = type;
			}
		}

		public static readonly CompNameInfo CNI_SHTID     = new CompNameInfo(N_SHTID, -1, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SHTTITLE  = new CompNameInfo(N_SHTTITLE, -1, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_PHBLD     = new CompNameInfo(N_PHBLD , VI_PHBLDG       , ShtIdType.ST_PHBLD);
		public static readonly CompNameInfo CNI_DISCP     = new CompNameInfo(N_DISCP , VI_DISCIPLINE   , ShtIdType.ST_NOT_PHBLD);
		public static readonly CompNameInfo CNI_CAT       = new CompNameInfo(N_CAT   , VI_CATEGORY     , ShtIdType.ST_TYPE01);
		public static readonly CompNameInfo CNI_SUBCAT    = new CompNameInfo(N_SUBCAT, VI_SUBIDENTIFIER, ShtIdType.ST_TYPE02);
		public static readonly CompNameInfo CNI_MOD       = new CompNameInfo(N_MOD   , VI_MODIFIER     , ShtIdType.ST_TYPE03);
		public static readonly CompNameInfo CNI_SUBMOD    = new CompNameInfo(N_SUBMOD, VI_SUBMODIFIER  , ShtIdType.ST_TYPE04);
		public static readonly CompNameInfo CNI_ID        = new CompNameInfo(N_ID    , VI_IDENTIFIER   , ShtIdType.ST_TYPE05);
		public static readonly CompNameInfo CNI_SUBID     = new CompNameInfo(N_SUBID , VI_SUBIDENTIFIER, ShtIdType.ST_TYPE06);
		public static readonly CompNameInfo CNI_SEP0      = new CompNameInfo(N_SEP0, VI_SEP0, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SEP1      = new CompNameInfo(N_SEP1, VI_SEP1, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SEP2      = new CompNameInfo(N_SEP2, VI_SEP2, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SEP3      = new CompNameInfo(N_SEP3, VI_SEP3, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SEP4      = new CompNameInfo(N_SEP4, VI_SEP4, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SEP5      = new CompNameInfo(N_SEP5, VI_SEP5, ShtIdType.ST_NA);

	


		internal static Dictionary<int, CompNameInfo> CompNames = new Dictionary<int, CompNameInfo>()
		{
			{VI_PHBLDG       , CNI_PHBLD},		// 0	0
			// {VI_PBSEP        , PBSEP},		//		1
			{VI_DISCIPLINE   , CNI_DISCP},		// 1	2
			{VI_SEP0         , CNI_SEP0},		//		3
			{VI_CATEGORY     , CNI_CAT},			// 2	4
			{VI_SEP1         , CNI_SEP1},		//		5
			{VI_SUBCATEGORY  , CNI_SUBCAT},		// 3	6
			{VI_SEP2         , CNI_SEP2},		//		7
			{VI_MODIFIER     , CNI_MOD},			// 4	8
			{VI_SEP3         , CNI_SEP3},		//		9
			{VI_SUBMODIFIER  , CNI_SUBMOD},		// 5	10
			{VI_SEP4         , CNI_SEP4},		//		11
			{VI_IDENTIFIER   , CNI_ID},			// 6	12
			{VI_SEP5         , CNI_SEP5},		//		13
			{VI_SUBIDENTIFIER, CNI_SUBID},		// 7	14
			// {VI_SEP6         , $"{N_SEP}6"},	// 	    15
			// {SHEET_NAME_VALUE_IDX   , SHTNAME},		//		16

		};

		internal static Dictionary<string, int> CompNums = new Dictionary<string, int>()
		{
			{N_PHBLD    , VI_PHBLDG       },
			// {PBSEP    , VI_PBSEP        },
			{N_DISCP    , VI_DISCIPLINE   },
			{N_SEP0     , VI_SEP0         },
			{N_SEP1     , VI_SEP1         },
			{N_SEP2     , VI_SEP2         },
			{N_SEP3     , VI_SEP3         },
			{N_SEP4     , VI_SEP4         },
			{N_SEP5     , VI_SEP5         },
			{N_CAT      , VI_CATEGORY     },
			{N_SUBCAT   , VI_SUBCATEGORY  },
			{N_MOD      , VI_MODIFIER     },
			{N_SUBMOD   , VI_SUBMODIFIER  },
			{N_ID       , VI_IDENTIFIER   },
			{N_SUBID    , VI_SUBIDENTIFIER},
			// {$"{N_SEP}6", VI_SEP6         },
			// {SHTNAME  , SHEET_NAME_VALUE_IDX   },

		};



		// internal static string[] CompNames = new []
		// {
		// 	nameof(VI_PHBLDG),
		// 	nameof(VI_PBSEP        ),
		// 	nameof(VI_DISCIPLINE   ),
		// 	nameof(VI_SEP0         ),
		// 	nameof(VI_CATEGORY     ),
		// 	nameof(VI_SEP1         ),
		// 	nameof(VI_SUBCATEGORY  ),
		// 	nameof(VI_SEP2         ),
		// 	nameof(VI_MODIFIER     ),
		// 	nameof(VI_SEP3         ),
		// 	nameof(VI_SUBMODIFIER  ),
		// 	nameof(VI_SEP4         ),
		// 	nameof(VI_IDENTIFIER   ),
		// 	nameof(VI_SEP5         ),
		// 	nameof(VI_SUBIDENTIFIER),
		// 	nameof(VI_SEP6         )
		// };



		internal static int[] IDX_XLATE = new[]
		{
			VI_PHBLDG, VI_DISCIPLINE, VI_CATEGORY, VI_SUBCATEGORY,
			VI_MODIFIER, VI_SUBMODIFIER, VI_IDENTIFIER, VI_SUBIDENTIFIER
		};




		internal int IDX_XLATE_MIN = 0;

		internal int IDX_XLATE_MAX = IDX_XLATE.Length - 1;

		public enum SeqCtrlUse2
		{
			NOT_USED = 0,
			REQUIRED = 1, // this means always required
			OPTIONAL = 2, // this means may or may not be needed
			SKIP = 3,
		}

		public enum SeqCtrlProceedOpt
		{
			INVALID = -1,
			NOT_USED = 0,
			END = 1,
			CONTINUE = 2,
		}

		public enum SeqCtrlNextOpt
		{
			INVALID = -1,
			NOT_USED = 0,
			SEQ_START = 2, // process or skip depending on existence
			SEQ_END = 3,
			SEQ_END_SEQ_REQ = 4,
			REQ_IF_PRIOR = 5
		}

		public enum SeqCtrlProceedReqd
		{
			INVALID = -1,
			NOT_USED = 0,
			END = 1,
			CONTINUE = 2,
		}

		public enum SeqCtrlNextReqd
		{
			INVALID = -1,
			NOT_USED = 0,
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
			public FileNameSheetIdentifiers.ShtIdType2 SheetIdType { get; private set; }
			public ShtIdClass2 ItemClass { get; private set; }

			public ShtNumComps2(string name, string grpId, NeumonicString2 neumonicStr, string abbrevName,
				int idx, FileNameSheetIdentifiers.ShtIdType2 shtType, ShtIdClass2 iClass)
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

			public override string ToString()
			{
				return $"{Preface} {Body} {Suffix}";
			}
		}

		/*
		public string CompGrpName2(ShtIdType2 type, int compListIdx)
		{
			return ShtCompList2[(int) type].ShtCompInfo[compListIdx].GrpName;
		}

		public string CompTitle2(ShtIdType2 type, int compListIdx)
		{
			return ShtCompList2[(int) type].ShtCompInfo[compListIdx].Title;
		}

		public int CompValueIdx2(ShtIdType2 type, int compListIdx)
		{
			return ShtCompList2[(int) type].ShtCompInfo[compListIdx].ValueIndex;
		}

		public bool CompIsUsed2(ShtIdType2 type, int compListIdx)
		{
			return ShtCompList2[(int) type].ShtCompInfo[compListIdx].IsUsed;
		}

		public SeqCtrlUse2 CompSeqCtrl2(ShtIdType2 type, int compListIdx)
		{
			return ShtCompList2[(int) type].ShtCompInfo[compListIdx].SeqCtrlUse;
		}


		public List<SheetCompList2> ShtCompList2 { get; } = new List<SheetCompList2>()
		{
			new SheetCompList2("UNASSIGNED", "Unassigned",
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.NOT_USED),
						-1 , grpNname: "shtname", title: "Sheet Name")
				}),
			new SheetCompList2("PhBldg", "Phase-Building",
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_PHBLDG, grpNname: N_PHBLD, title: N_PHBLDG),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.END),
						VI_PBSEP, grpNname: "pbsep", title: "PB" + N_SEPR),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.NOT_USED),
						-1 , grpNname: "shtid" , title: "Sheet Id"),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.NOT_USED),
						-1 , grpNname: "shtname", title: "Sheet Name")
				}),
			new SheetCompList2("TYPE10", "Categorized-Extended",
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_DISCIPLINE , grpNname: N_DISCP + "10" , title: N_DISCP),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.SKIP, SeqCtrlProceedReqd.CONTINUE),
						VI_SEP0 , grpNname: N_SEP + 0 , title: N_SEPR + 0),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_CATEGORY , grpNname: N_CAT + "10" , title: N_CAT),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_SEP1 , grpNname: N_SEP + 11 , title: N_SEPR + 11),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_SUBCATEGORY , grpNname: N_SUBCAT + "10", title: N_SUBCAT),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_SEP2 , grpNname: N_SEP + 12 , title: N_SEPR + 12),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE) ,
						VI_MODIFIER , grpNname: N_MOD + "10" , title: N_MOD),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.CONTINUE),
						VI_SEP3 , grpNname: N_SEP + 13 , title: N_SEPR + 13),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.END, SeqCtrlNextOpt.REQ_IF_PRIOR),
						VI_SUBMODIFIER , grpNname: N_SUBMOD + "10", title: N_SUBMOD),
				}),
			new SheetCompList2("TYPE20", "Green",
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_DISCIPLINE , grpNname: N_DISCP + "20" , title: N_DISCP),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_SEP0 , grpNname: N_SEP + 21 , title: N_SEPR + 21),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.END),
						VI_CATEGORY , grpNname: N_CAT + "20" , title: N_CAT),
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SEP1 ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SUBCATEGORY ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SEP2 ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_MODIFIER ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SEP3 ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SUBMODIFIER ) ,
				}),
			new SheetCompList2("TYPE30", "Categorized",
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_DISCIPLINE , grpNname: N_DISCP + "30" , title: N_DISCP),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.SKIP, SeqCtrlProceedReqd.CONTINUE),
						VI_SEP0 , grpNname: N_SEP + 0 , title: N_SEPR + 0),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_CATEGORY , grpNname: N_CAT + "30" , title: N_CAT),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_SEP1 , grpNname: N_SEP + 31 , title: N_SEPR + 31),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.END),
						VI_SUBCATEGORY , grpNname: N_SUBCAT + "30", title: N_SUBCAT),
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SEP2 ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_MODIFIER ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SEP3 ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SUBMODIFIER ) ,
				}),
			new SheetCompList2("TYPE40", "Traditional",
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_DISCIPLINE , grpNname: N_DISCP + "40" , title: N_DISCP),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						VI_SEP0 , grpNname: N_SEP + 41 , title: N_SEPR + 41),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.END),
						VI_CATEGORY , grpNname: N_CAT + "40" , title: N_CAT),
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SEP1 ),
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SUBCATEGORY ),
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SEP2 ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_MODIFIER ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SEP3 ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), VI_SUBMODIFIER ) ,
				}),
			new SheetCompList2("IDENT", null,
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.CONTINUE, SeqCtrlNextOpt.SEQ_START),
						VI_SEP4 , grpNname: N_SEP + 4 , title: N_SEPR + 4),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.CONTINUE, SeqCtrlNextOpt.REQ_IF_PRIOR),
						VI_IDENTIFIER , grpNname: N_ID , title: N_ID),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.CONTINUE),
						VI_SEP5 , grpNname: N_SEP + 5 , title: N_SEPR + 5),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.END, SeqCtrlNextOpt.REQ_IF_PRIOR),
						VI_SUBIDENTIFIER , grpNname: N_SUBID , title: N_SUBID),
					// new SheetCompInfo2(
					// 	new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.END, SeqCtrlNextOpt.SEQ_END_SEQ_REQ),
					// 	VI_SEP6 , grpNname: N_SEP + 6 , title: N_SEPR + 6),
				})
		};


		public class SheetCompList2
		{
			public string TypeName { get; private set; }

			public string Title { get; private set; }

			public int Count
			{
				get => ShtCompInfo.Count;
			}

			public List<SheetCompInfo2> ShtCompInfo { get; private set; }

			public SheetCompList2(string typeName, string title, List<SheetCompInfo2> compInfo)
			{
				TypeName = typeName;
				Title = title;
				ShtCompInfo = compInfo;
			}
		}




		public class SheetCompInfo2
		{
			public int ValueIndex { get; private set; }
			public string GrpName { get; private set; }
			public string Title { get; private set; }

			public ASeqCtrl SeqCtrl { get; set; }

			public SheetCompInfo2(ASeqCtrl seqctrl,
				int valueIndex, string grpNname = "", string title = "")
			{
				SeqCtrl = seqctrl;
				ValueIndex = valueIndex;
				GrpName = grpNname;
				Title = title;
			}

			public bool IsUsed => SeqCtrl.Use == SeqCtrlUse2.OPTIONAL ||
				SeqCtrl.Use == SeqCtrlUse2.REQUIRED;

			public SeqCtrlUse2 SeqCtrlUse => SeqCtrl.Use;

			public bool IsEnd => GetProceedReqd() == SeqCtrlProceedReqd.END || GetProceedOpt() == SeqCtrlProceedOpt.END;

			public bool IsUseOK => (int) SeqCtrlUse >= 0;

			public bool IsReqdProceedOK => (int) GetProceedReqd() >= 0;

			public bool IsReqdNextOK => (int) GetNextReqd() >= 0;

			public bool IsOptProceedOK => (int) GetProceedOpt() >= 0;

			public bool IsOptNextOK => (int) GetNextOpt() >= 0;

			public SeqCtrlProceedOpt GetProceedOpt()
			{
				if (SeqCtrl.TypeProceed != typeof(SeqCtrlProceedOpt))
				{
					return SeqCtrlProceedOpt.INVALID;
				}

				return ((SeqCtrlOpt) SeqCtrl).Proceed;
			}

			public SeqCtrlNextOpt GetNextOpt()
			{
				if (SeqCtrl.TypeNext != typeof(SeqCtrlNextOpt))
				{
					return SeqCtrlNextOpt.INVALID;
				}

				return ((SeqCtrlOpt) SeqCtrl).Next;
			}

			public SeqCtrlProceedReqd GetProceedReqd()
			{
				if (SeqCtrl.TypeProceed != typeof(SeqCtrlProceedReqd))
				{
					return SeqCtrlProceedReqd.INVALID;
				}

				return ((SeqCtrlReqd) SeqCtrl).Proceed;
			}

			public SeqCtrlNextReqd GetNextReqd()
			{
				if (SeqCtrl.TypeNext != typeof(SeqCtrlNextReqd))
				{
					return SeqCtrlNextReqd.INVALID;
				}

				return ((SeqCtrlReqd) SeqCtrl).Next;
			}
		}
		*/


		public abstract class ASeqCtrl
		{
			internal abstract Type TypeProceed { get; }
			internal abstract Type TypeNext { get; }

			internal SeqCtrlUse2 Use { get; set; }
		}

		public class SeqCtrl<T, U> : ASeqCtrl where T : Enum where U : Enum
		{
			public U Next { get; set; }

			public T Proceed { get; set; }

			internal override Type TypeProceed => typeof(T);
			internal override Type TypeNext => typeof(U);

			public SeqCtrl() { }

			public SeqCtrl(SeqCtrlUse2 use, T proceedReqd, U next)
			{
				Use = use;
				Proceed = proceedReqd;
				Next = next;
			}
		}

		public class SeqCtrlOpt : SeqCtrl<SeqCtrlProceedOpt, SeqCtrlNextOpt>
		{
			public SeqCtrlOpt(SeqCtrlUse2 use,
				SeqCtrlProceedOpt proceed = SeqCtrlProceedOpt.NOT_USED,
				SeqCtrlNextOpt next = SeqCtrlNextOpt.NOT_USED) : base(use, proceed, next) { }
		}

		public class SeqCtrlReqd : SeqCtrl<SeqCtrlProceedReqd, SeqCtrlNextReqd>
		{
			public SeqCtrlReqd(SeqCtrlUse2 use,
				SeqCtrlProceedReqd proceed = SeqCtrlProceedReqd.NOT_USED,
				SeqCtrlNextReqd next = SeqCtrlNextReqd.NOT_USED) : base(use, proceed, next) { }
		}
	}

	// public struct ShtNumberCompName
	// {
	// 	public string Name { get; private set; }
	// 	public NeumonicString Neumonic { get; private set; }
	// 	public string AbbrevName { get; private set; }
	// 	public int ValueIndex { get; private set; }
	//
	// 	public ShtNumberCompName(   string name, NeumonicString neumonicStr, string abbrevName, int valueIdx)
	// 	{
	// 		Name = name;
	// 		Neumonic = neumonicStr;
	// 		AbbrevName = abbrevName;
	// 		ValueIndex = valueIdx;
	// 	}
	// }
	//
	// public struct NeumonicString
	// {
	// 	public string Preface { get; private set; }
	// 	public string Body { get; private set; }
	// 	public string Suffix { get; private set; }
	//
	// 	public NeumonicString(string preface, string body, string suffix)
	// 	{
	// 		Preface = preface;
	// 		Body = body;
	// 		Suffix = suffix;
	// 	}
	//
	// 	public override string ToString()
	// 	{
	// 		return $"{Preface} {Body} {Suffix}";
	// 	}
	// }
}