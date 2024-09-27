#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

			// string a = ShtIds.SheetNumberComponentTitles[0].Name;
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

		// value index is not being used - spare
		public List<ShtNumberCompName> SheetNumberComponentTitles { get; } = new List<ShtNumberCompName>()
		{
			new ShtNumberCompName(PHBLDG, new NeumonicString(            "(" , "A", " x.x-xz)"), "Ph/Bld", 5),
			new ShtNumberCompName(DISCP , new NeumonicString(          "(x " , "A", "x.x-xz)") , "Disc"  , 10),
			new ShtNumberCompName(CAT   , new NeumonicString(         "(x y" , "0", ".x-xz)")  , "Cat"   , 15),
			new ShtNumberCompName(SUBCAT, new NeumonicString(       "(x yx." , "0", "-xz)")    , "SubCat", 20),
			new ShtNumberCompName(MOD   , new NeumonicString(     "(x yx.x-" , "0", ")")       , "Mod"   , 25),
			new ShtNumberCompName(SUBMOD, new NeumonicString    ("(x yx.x-x" , "A", ")")       , "SubMod", 30),
			new ShtNumberCompName(ID    , new NeumonicString(  "(x yx.x-xz(" , "A", "))")      , "Id"    , 35),
			new ShtNumberCompName(SUBID , new NeumonicString("(x yx.x-xz(x/" , "A", "))")      , "SubId" , 40),
		};
		// new ShtNumberCompName("Sheet Number Type", "Number Type", 0),

		public const string PHBLD  = "PhBldgid";
		public const string PHBLDG = "Phase/Building";
		public const string DISCP  = "Discipline";
		public const string CAT    = "Category";
		public const string SUBCAT = "SubCategory";
		public const string MOD    = "Modifier";
		public const string SUBMOD = "SubModifier";
		public const string ID     = "Identifier";
		public const string SUBID  = "SubIdentifier";

		public const string SEP0  = "sep0";
		public const string SEP1  = "sep1";
		public const string SEP2  = "sep2";
		public const string SEP3  = "sep3";
		public const string SEP4  = "sep4";
		public const string SEP5  = "sep5";

		// public const string SHTNAME= "SheetName";

		public const string SHTID = "SheetId";
		public const string SHTTITLE = "SheetTitle";

		private const string SEP   = "sep";
		private const string SEPR  = "Separator";
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

		// shorthand access to the indices
		internal const int SHTID_COMP_IDX = 2;
		internal const int SHTNAME_COMP_IDX = 3;

		// the index in the final component array: ShtCompList
		internal const int PHBLDG_COMP_IDX = 0; //
		internal const int PBSEP_COMP_IDX = 1;  //

		// // the index in the final component value array: FileNameSheetPdf.SheetComps
		public const int PHBLDG_VALUE_IDX        = 0;  // 0
		public const int PBSEP_VALUE_IDX         = 1;  //
		public const int DISCIPLINE_VALUE_IDX    = 2;  // 1
		public const int SEP0_VALUE_IDX          = 3;  //
		public const int CATEGORY_VALUE_IDX      = 4;  // 2
		public const int SEP1_VALUE_IDX          = 5;  //
		public const int SUBCATEGORY_VALUE_IDX   = 6;  // 3
		public const int SEP2_VALUE_IDX          = 7;  //
		public const int MODIFIER_VALUE_IDX      = 8;  // 4
		public const int SEP3_VALUE_IDX          = 9;  // 
		public const int SUBMODIFIER_VALUE_IDX   = 10; // 5
		public const int SEP4_VALUE_IDX          = 11; // 
		public const int IDENTIFIER_VALUE_IDX    = 12; // 6
		public const int SEP5_VALUE_IDX          = 13; // 
		public const int SUBIDENTIFIER_VALUE_IDX = 14; // 7
		public const int SEP6_VALUE_IDX          = 15; // 

		// public const int SHEET_NAME_VALUE_IDX    = 16; // 8

		internal const int VALUE_IDX_COUNT = SEP6_VALUE_IDX; // 28 (req'd # // +1 - zero based
		internal const int VALUE_IDX_MIN = PHBLDG_VALUE_IDX;


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

		public static readonly CompNameInfo CNI_SHTID     = new CompNameInfo(SHTID, -1, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SHTTITLE  = new CompNameInfo(SHTTITLE, -1, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_PHBLD     = new CompNameInfo(PHBLD , PHBLDG_VALUE_IDX       , ShtIdType.ST_PHBLD);
		public static readonly CompNameInfo CNI_DISCP     = new CompNameInfo(DISCP , DISCIPLINE_VALUE_IDX   , ShtIdType.ST_NOT_PHBLD);
		public static readonly CompNameInfo CNI_CAT       = new CompNameInfo(CAT   , CATEGORY_VALUE_IDX     , ShtIdType.ST_TYPE01);
		public static readonly CompNameInfo CNI_SUBCAT    = new CompNameInfo(SUBCAT, SUBIDENTIFIER_VALUE_IDX, ShtIdType.ST_TYPE02);
		public static readonly CompNameInfo CNI_MOD       = new CompNameInfo(MOD   , MODIFIER_VALUE_IDX     , ShtIdType.ST_TYPE03);
		public static readonly CompNameInfo CNI_SUBMOD    = new CompNameInfo(SUBMOD, SUBMODIFIER_VALUE_IDX  , ShtIdType.ST_TYPE04);
		public static readonly CompNameInfo CNI_ID        = new CompNameInfo(ID    , IDENTIFIER_VALUE_IDX   , ShtIdType.ST_TYPE05);
		public static readonly CompNameInfo CNI_SUBID     = new CompNameInfo(SUBID , SUBIDENTIFIER_VALUE_IDX, ShtIdType.ST_TYPE06);
		public static readonly CompNameInfo CNI_SEP0      = new CompNameInfo(SEP0, SEP0_VALUE_IDX, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SEP1      = new CompNameInfo(SEP1, SEP1_VALUE_IDX, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SEP2      = new CompNameInfo(SEP2, SEP2_VALUE_IDX, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SEP3      = new CompNameInfo(SEP3, SEP3_VALUE_IDX, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SEP4      = new CompNameInfo(SEP4, SEP4_VALUE_IDX, ShtIdType.ST_NA);
		public static readonly CompNameInfo CNI_SEP5      = new CompNameInfo(SEP5, SEP5_VALUE_IDX, ShtIdType.ST_NA);

	


		internal static Dictionary<int, CompNameInfo> CompNames = new Dictionary<int, CompNameInfo>()
		{
			{PHBLDG_VALUE_IDX       , CNI_PHBLD},		// 0	0
			// {PBSEP_VALUE_IDX        , PBSEP},		//		1
			{DISCIPLINE_VALUE_IDX   , CNI_DISCP},		// 1	2
			{SEP0_VALUE_IDX         , CNI_SEP0},		//		3
			{CATEGORY_VALUE_IDX     , CNI_CAT},			// 2	4
			{SEP1_VALUE_IDX         , CNI_SEP1},		//		5
			{SUBCATEGORY_VALUE_IDX  , CNI_SUBCAT},		// 3	6
			{SEP2_VALUE_IDX         , CNI_SEP2},		//		7
			{MODIFIER_VALUE_IDX     , CNI_MOD},			// 4	8
			{SEP3_VALUE_IDX         , CNI_SEP3},		//		9
			{SUBMODIFIER_VALUE_IDX  , CNI_SUBMOD},		// 5	10
			{SEP4_VALUE_IDX         , CNI_SEP4},		//		11
			{IDENTIFIER_VALUE_IDX   , CNI_ID},			// 6	12
			{SEP5_VALUE_IDX         , CNI_SEP5},		//		13
			{SUBIDENTIFIER_VALUE_IDX, CNI_SUBID},		// 7	14
			// {SEP6_VALUE_IDX         , $"{SEP}6"},	// 	    15
			// {SHEET_NAME_VALUE_IDX   , SHTNAME},		//		16

		};

		internal static Dictionary<string, int> CompNums = new Dictionary<string, int>()
		{
			{PHBLD    , PHBLDG_VALUE_IDX       },
			// {PBSEP    , PBSEP_VALUE_IDX        },
			{DISCP    , DISCIPLINE_VALUE_IDX   },
			{SEP0     , SEP0_VALUE_IDX         },
			{SEP1     , SEP1_VALUE_IDX         },
			{SEP2     , SEP2_VALUE_IDX         },
			{SEP3     , SEP3_VALUE_IDX         },
			{SEP4     , SEP4_VALUE_IDX         },
			{SEP5     , SEP5_VALUE_IDX         },
			{CAT      , CATEGORY_VALUE_IDX     },
			{SUBCAT   , SUBCATEGORY_VALUE_IDX  },
			{MOD      , MODIFIER_VALUE_IDX     },
			{SUBMOD   , SUBMODIFIER_VALUE_IDX  },
			{ID       , IDENTIFIER_VALUE_IDX   },
			{SUBID    , SUBIDENTIFIER_VALUE_IDX},
			// {$"{SEP}6", SEP6_VALUE_IDX         },
			// {SHTNAME  , SHEET_NAME_VALUE_IDX   },

		};



		// internal static string[] CompNames = new []
		// {
		// 	nameof(PHBLDG_VALUE_IDX),
		// 	nameof(PBSEP_VALUE_IDX        ),
		// 	nameof(DISCIPLINE_VALUE_IDX   ),
		// 	nameof(SEP0_VALUE_IDX         ),
		// 	nameof(CATEGORY_VALUE_IDX     ),
		// 	nameof(SEP1_VALUE_IDX         ),
		// 	nameof(SUBCATEGORY_VALUE_IDX  ),
		// 	nameof(SEP2_VALUE_IDX         ),
		// 	nameof(MODIFIER_VALUE_IDX     ),
		// 	nameof(SEP3_VALUE_IDX         ),
		// 	nameof(SUBMODIFIER_VALUE_IDX  ),
		// 	nameof(SEP4_VALUE_IDX         ),
		// 	nameof(IDENTIFIER_VALUE_IDX   ),
		// 	nameof(SEP5_VALUE_IDX         ),
		// 	nameof(SUBIDENTIFIER_VALUE_IDX),
		// 	nameof(SEP6_VALUE_IDX         )
		// };



		internal static int[] IDX_XLATE = new[]
		{
			PHBLDG_VALUE_IDX, DISCIPLINE_VALUE_IDX, CATEGORY_VALUE_IDX, SUBCATEGORY_VALUE_IDX,
			MODIFIER_VALUE_IDX, SUBMODIFIER_VALUE_IDX, IDENTIFIER_VALUE_IDX, SUBIDENTIFIER_VALUE_IDX
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

		public string CompGrpName2(ShtCompTypes type, int compListIdx)
		{
			return ShtCompList2[(int) type].ShtCompInfo[compListIdx].GrpName;
		}

		public string CompTitle2(ShtCompTypes type, int compListIdx)
		{
			return ShtCompList2[(int) type].ShtCompInfo[compListIdx].Title;
		}

		public int CompValueIdx2(ShtCompTypes type, int compListIdx)
		{
			return ShtCompList2[(int) type].ShtCompInfo[compListIdx].ValueIndex;
		}

		public bool CompIsUsed2(ShtCompTypes type, int compListIdx)
		{
			return ShtCompList2[(int) type].ShtCompInfo[compListIdx].IsUsed;
		}

		public SeqCtrlUse2 CompSeqCtrl2(ShtCompTypes type, int compListIdx)
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
						PHBLDG_VALUE_IDX, grpNname: PHBLD, title: PHBLDG),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.END),
						PBSEP_VALUE_IDX, grpNname: "pbsep", title: "PB" + SEPR),
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
						DISCIPLINE_VALUE_IDX , grpNname: DISCP + "10" , title: DISCP),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.SKIP, SeqCtrlProceedReqd.CONTINUE),
						SEP0_VALUE_IDX , grpNname: SEP + 0 , title: SEPR + 0),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						CATEGORY_VALUE_IDX , grpNname: CAT + "10" , title: CAT),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						SEP1_VALUE_IDX , grpNname: SEP + 11 , title: SEPR + 11),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						SUBCATEGORY_VALUE_IDX , grpNname: SUBCAT + "10", title: SUBCAT),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						SEP2_VALUE_IDX , grpNname: SEP + 12 , title: SEPR + 12),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE) ,
						MODIFIER_VALUE_IDX , grpNname: MOD + "10" , title: MOD),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.CONTINUE),
						SEP3_VALUE_IDX , grpNname: SEP + 13 , title: SEPR + 13),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.END, SeqCtrlNextOpt.REQ_IF_PRIOR),
						SUBMODIFIER_VALUE_IDX , grpNname: SUBMOD + "10", title: SUBMOD),
				}),
			new SheetCompList2("TYPE20", "Green",
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						DISCIPLINE_VALUE_IDX , grpNname: DISCP + "20" , title: DISCP),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						SEP0_VALUE_IDX , grpNname: SEP + 21 , title: SEPR + 21),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.END),
						CATEGORY_VALUE_IDX , grpNname: CAT + "20" , title: CAT),
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SEP1_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SUBCATEGORY_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SEP2_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), MODIFIER_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SEP3_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SUBMODIFIER_VALUE_IDX ) ,
				}),
			new SheetCompList2("TYPE30", "Categorized",
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						DISCIPLINE_VALUE_IDX , grpNname: DISCP + "30" , title: DISCP),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.SKIP, SeqCtrlProceedReqd.CONTINUE),
						SEP0_VALUE_IDX , grpNname: SEP + 0 , title: SEPR + 0),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						CATEGORY_VALUE_IDX , grpNname: CAT + "30" , title: CAT),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						SEP1_VALUE_IDX , grpNname: SEP + 31 , title: SEPR + 31),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.END),
						SUBCATEGORY_VALUE_IDX , grpNname: SUBCAT + "30", title: SUBCAT),
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SEP2_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), MODIFIER_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SEP3_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SUBMODIFIER_VALUE_IDX ) ,
				}),
			new SheetCompList2("TYPE40", "Traditional",
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						DISCIPLINE_VALUE_IDX , grpNname: DISCP + "40" , title: DISCP),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						SEP0_VALUE_IDX , grpNname: SEP + 41 , title: SEPR + 41),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.END),
						CATEGORY_VALUE_IDX , grpNname: CAT + "40" , title: CAT),
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SEP1_VALUE_IDX ),
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SUBCATEGORY_VALUE_IDX ),
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SEP2_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), MODIFIER_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SEP3_VALUE_IDX ) ,
					new SheetCompInfo2(new SeqCtrlReqd(SeqCtrlUse2.NOT_USED), SUBMODIFIER_VALUE_IDX ) ,
				}),
			new SheetCompList2("IDENT", null,
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.CONTINUE, SeqCtrlNextOpt.SEQ_START),
						SEP4_VALUE_IDX , grpNname: SEP + 4 , title: SEPR + 4),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.CONTINUE, SeqCtrlNextOpt.REQ_IF_PRIOR),
						IDENTIFIER_VALUE_IDX , grpNname: ID , title: ID),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.CONTINUE),
						SEP5_VALUE_IDX , grpNname: SEP + 5 , title: SEPR + 5),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.END, SeqCtrlNextOpt.REQ_IF_PRIOR),
						SUBIDENTIFIER_VALUE_IDX , grpNname: SUBID , title: SUBID),
					// new SheetCompInfo2(
					// 	new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.END, SeqCtrlNextOpt.SEQ_END_SEQ_REQ),
					// 	SEP6_VALUE_IDX , grpNname: SEP + 6 , title: SEPR + 6),
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

	public struct ShtNumberCompName
	{
		public string Name { get; private set; }
		public NeumonicString Neumonic { get; private set; }
		public string AbbrevName { get; private set; }
		public int ValueIndex { get; private set; }

		public ShtNumberCompName(   string name, NeumonicString neumonicStr, string abbrevName, int valueIdx)
		{
			Name = name;
			Neumonic = neumonicStr;
			AbbrevName = abbrevName;
			ValueIndex = valueIdx;
		}
	}

	public struct NeumonicString
	{
		public string Preface { get; private set; }
		public string Body { get; private set; }
		public string Suffix { get; private set; }

		public NeumonicString(string preface, string body, string suffix)
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
}