#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers.SeqCtrl;

#endregion

// user name: jeffs
// created:   10/1/2020 7:16:21 AM

namespace AndyShared.FileSupport.SheetPDF
{
	public class FileNameSheetIdentifiers
	{
		public enum FileTypeSheetPdf
		{
			INVALID     = -1,
			UNASSIGNED  = 0,
			OTHER,
			NON_SHEET_PDF,
			SHEET_PDF,
			// SHEET_PDF_UNASSIGNED_TYPE,
			// SHEET_PDF_PB_UNASSIGNED_TYPE,
			// SHEET_PDF_PB = 0,
			// SHEET_PDF_ID = 5,
			// SHEET_PDF_TYPE10   = 1,
			// SHEET_PDF_TYPE20,
			// SHEET_PDF_TYPE30,
			// SHEET_PDF_TYPE40,
			// COUNT               = (int) SHEET_PDF_TYPE40 - (int) SHEET_PDF_TYPE10 + 1,
		}


		// internal string CompGrpName(ShtCompTypes type, ShtCompsGrpIdx grpIdx)
		// {
		// 	return SheetComponentNames[(int) type][(int) grpIdx, 0];
		// }


		// public enum ShtCompsPhaseBldg
		// {
		// 	PHBLDG  ,
		// 	PBSEP   ,
		// 	SHTID1  ,
		// 	SHTID2  ,
		// 	COUNTPB
		// }

		// the index in the final 
		// sheet component array
		public enum pbCompsIdx
		{
			// phase / bldg only
			CI_PHBLDG       = 0, // 0 
			CI_PBSEP        ,    // 1 
			CI_PBCOUNT              // 2
		}

		public const int SHTIDIDX = 2;
		public const int SHTNAMEIDX = 3;

		// the index in the final 
		// sheet component array
		public enum ShtIdCompsIdx
		{
			CI_DISCIPLINE   = 0, // 0
			CI_SEP0         ,    // 1
			CI_CATEGORY     ,    // 2
			CI_SEP1         ,    // 3
			CI_SUBCATEGORY  ,    // 4
			CI_SEP2         ,    // 5
			CI_MODIFIER     ,    // 6
			CI_SEP3         ,    // 7
			CI_SUBMODIFIER  ,    // 8
			CI_TYPECOUNT         // 10
		}

		// the index in the final 
		// sheet component array
		public enum identCompsIdx
		{
			CI_SEP4         = 0, // 0
			CI_IDENTIFIER   ,    // 1
			CI_SEP5         ,    // 2
			CI_SUBIDENTIFIER,    // 3
			CI_SEP6         ,    // 4
			CI_IDENTCOUNT        // 5
		}

		public const int PBSTART = (int) pbCompsIdx.CI_PHBLDG;
		public const int PBEND = (int) pbCompsIdx.CI_PBSEP + 1;

		public const int TYPESTART = (int) ShtIdCompsIdx.CI_DISCIPLINE;
		public const int TYPEEND = (int) ShtIdCompsIdx.CI_SUBMODIFIER + 1;

		public const int IDENTSTART = (int) identCompsIdx.CI_SEP4;
		public const int IDENTEND = (int) identCompsIdx.CI_SEP6 + 1;


		// shorthand access to the indicies
		internal const int PHBLDGIDX          = (int) pbCompsIdx.CI_PHBLDG        ;
		internal const int PBSEPIDX           = (int) pbCompsIdx.CI_PBSEP        	;
		internal const int DISCIPLINEIDX	  = (int) ShtIdCompsIdx.CI_DISCIPLINE   	;
		internal const int SEP0IDX			  = (int) ShtIdCompsIdx.CI_SEP0         	;
		internal const int CATEGORYIDX		  = (int) ShtIdCompsIdx.CI_CATEGORY     	;
		internal const int SEP1IDX			  = (int) ShtIdCompsIdx.CI_SEP1         	;
		internal const int SUBCATEGORYIDX	  = (int) ShtIdCompsIdx.CI_SUBCATEGORY  	;
		internal const int SEP2IDX			  = (int) ShtIdCompsIdx.CI_SEP2         	;
		internal const int MODIFIERIDX		  = (int) ShtIdCompsIdx.CI_MODIFIER     	;
		internal const int SEP3IDX			  = (int) ShtIdCompsIdx.CI_SEP3         	;
		internal const int SUBMODIFIERIDX	  = (int) ShtIdCompsIdx.CI_SUBMODIFIER  	;
		internal const int SEP4IDX			  = (int) identCompsIdx.CI_SEP4;
		internal const int IDENTIFIERIDX	  = (int) identCompsIdx.CI_IDENTIFIER;
		internal const int SEP5IDX      	  = (int) identCompsIdx.CI_SEP5;
		internal const int SUBIDENTIFIERIDX	  = (int) identCompsIdx.CI_SUBIDENTIFIER;
		internal const int SEP6IDX      	  = (int) identCompsIdx.CI_SEP6;

		// indicates the sheet component type and 
		// is the index in the SheetComponentNames array
		// is the index in the master "ShtCompList" array
		public enum ShtCompTypes
		{
			UNASSIGNED  = -1,
			PHBLDG      = 0,
			TYPE10      = 1,
			TYPE20      = 2,
			TYPE30      = 3,
			TYPE40      = 4,
			IDENT       = 5,
			COUNT
		}


		// the index of the group name in the
		// master "ShtCompList" array
		// public enum ShtCompsGrpIdx
		// {
		// 	PHBLDG0      = 0,
		// 	PBSEP0       ,
		// 	SHTID0       ,
		// 	SHTNAME      ,
		// 	DISCIPLINE10 = 0,
		// 	CATEGORY10   ,
		// 	SEP11        ,
		// 	SUBCATEGORY10,
		// 	SEP12        ,
		// 	MODIFIER10   ,
		// 	SEP13        ,
		// 	SUBMODIFIER10,
		// 	COUNT10      ,
		// 	DISCIPLINE20 = 0,
		// 	SEP21        ,
		// 	CATEGORY20    ,
		// 	COUNT20      ,
		// 	DISCIPLINE30 = 0,
		// 	CATEGORY30   ,
		// 	SEP31        ,
		// 	SUBCATEGORY30,
		// 	COUNT30      ,
		// 	DISCIPLINE40 = 0,
		// 	SEP41        ,
		// 	CATEGORY40   ,
		// 	COUNT40      ,
		// 	SEP01         = 0,
		// 	IDENTIFIER0   ,
		// 	SEP02         ,
		// 	SUBIDENTIFIER0,
		// 	SEP03         ,
		// 	COUNTID0
		// }

		//
		// // index A: the sheet component type
		// // index 1 & 2:
		// // index B, the sheet component (SheetComponentsType1, etc.)
		// // index B,1, the sheet component, regex group name 
		// // index B,2, the sheet component, human name
		//
		// public static string[][,] SheetComponentNames = new []
		// {
		// 	// index = 0
		// 	new string[,]
		// 	{
		// 		// building-phase
		// 		{"bldgid"     , PHBLD},
		// 		{"pbsep"      , SEPR},
		// 		{"shtid"      , "Sheet Id"},
		// 		{"shtname"    , "Sheet Name"},
		// 	},
		//
		// 	// index = 1
		// 	new string[,]
		// 	{
		// 		// TYPE1 (ie. typical  A A2.1-1)
		// 		{DISCP + "10" , DISCP},
		// 		{CAT + "10"   , CAT},
		// 		{SEP + 11     , SEPR + 11},
		// 		{SUBCAT + "10", SUBCAT},
		// 		{SEP + 12     , SEPR + 12},
		// 		{MOD + "10"   , MOD},
		// 		{SEP + 13     , SEPR + 13},
		// 		{SUBMOD + "10", SUBMOD},
		// 	},
		// 	// index = 2
		// 	new string[,]
		// 	{
		// 		// TYPE2 (ie. 'GRN.1'
		// 		{DISCP + "20" , DISCP},
		// 		{SEP + 21     , SEPR + 21},
		// 		{CAT + "20"   , CAT},
		// 	},
		// 	// index = 3
		// 	new string[,]
		// 	{
		// 		// TYPE3
		// 		{DISCP + "30" , DISCP},
		// 		{CAT + "30"   , CAT},
		// 		{SEP + 31     , SEPR + 31},
		// 		{SUBCAT + "30", SUBCAT},
		// 	},
		// 	// index = 4
		// 	new string[,]
		// 	{
		// 		// TYPE4
		// 		{DISCP + "40" , DISCP},
		// 		{SEP + 41     , SEPR + 41},
		// 		{CAT + "40"   , CAT},
		// 	},
		//
		// 	// index = 5
		// 	new string[,]
		// 	{
		// 		// identifier
		// 		{SEP + 1      , SEPR + 1},
		// 		{ID           , ID},
		// 		{SEP + 2      , SEPR + 2},
		// 		{SUBID        , SUBID},
		// 		{SEP + 3      , SEPR + 3}
		// 	}
		// };


		public const string PHBLD  = "Bldg/Phase";
		public const string DISCP  = "Discipline";
		public const string CAT    = "Category";
		public const string SUBCAT = "SubCategory";
		public const string MOD    = "Modifier";
		public const string SUBMOD = "SubModifier";
		public const string ID     = "Identifier";
		public const string SUBID  = "SubIdentifier";

		public const string SEP    = "sep";
		public const string SEPR   = "Separator";


		public static int PhBldgCount = 0;
		public static int Type10Count = 0;
		public static int Type20Count = 0;
		public static int Type30Count = 0;
		public static int Type40Count = 0;
		public static int IdentCount = 0;

		public string CompGrpName(ShtCompTypes type, int idx)
		{
			return ShtCompList[(int) type].ShtCompNames[idx].GrpName;
		}

		public string CompTitle(ShtCompTypes type, int  idx)
		{
			return ShtCompList[(int) type].ShtCompNames[idx].Title;
		}

		public int CompIdx(ShtCompTypes type, int  idx)
		{
			return ShtCompList[(int) type].ShtCompNames[idx].Index;
		}

		public bool CompIsUsed(ShtCompTypes type, int  idx)
		{
			return ShtCompList[(int) type].ShtCompNames[idx].IsUsed;
		}

		public SeqCtrl CompSeqCtrl(ShtCompTypes type, int  idx)
		{
			return ShtCompList[(int) type].ShtCompNames[idx].SeqCtrl;
		}


		public static List<SheetCompList> ShtCompList = new List<SheetCompList>()
		{
			new SheetCompList("PhBldg",
				new List<SheetCompNames>()
				{
					new SheetCompNames(true, REQUIRED_CONTINUE, PhBldgCount++, grpNname: "bldgid" , title: PHBLD),
					new SheetCompNames(true, REQUIRED_CONTINUE, PhBldgCount++, grpNname: "pbsep"  , title: "PB"+SEPR),
					new SheetCompNames(true, NOT_USED, PhBldgCount++, grpNname: "shtid"  , title: "Sheet Id"),
					new SheetCompNames(true, NOT_USED, PhBldgCount++, grpNname: "shtname", title: "Sheet Name")
				}),
			new SheetCompList("TYPE10",
				new List<SheetCompNames>()
				{
					new SheetCompNames(true,  REQUIRED_CONTINUE, DISCIPLINEIDX, grpNname: DISCP + "10" , title: DISCP),
					new SheetCompNames(false, SKIP_CONTINUE, SEP0IDX),
					new SheetCompNames(true,  REQUIRED_CONTINUE, CATEGORYIDX, grpNname: CAT + "10"   , title: CAT),
					new SheetCompNames(true,  REQUIRED_CONTINUE, SEP1IDX, grpNname: SEP + 11     , title: SEPR + 11),
					new SheetCompNames(true,  REQUIRED_CONTINUE, SUBCATEGORYIDX, grpNname: SUBCAT + "10", title: SUBCAT),
					new SheetCompNames(true,  REQUIRED_CONTINUE, SEP2IDX, grpNname: SEP + 12     , title: SEPR + 12),
					new SheetCompNames(true,  REQUIRED_CONTINUE, MODIFIERIDX, grpNname: MOD + "10"   , title: MOD),
					new SheetCompNames(true,  OPTIONAL_SKIP_NEXT, SEP3IDX, grpNname: SEP + 13     , title: SEPR + 13),
					new SheetCompNames(true,  OPTIONAL_END, SUBMODIFIERIDX, grpNname: SUBMOD + "10", title: SUBMOD),
				}),
			new SheetCompList("TYPE20",
				new List<SheetCompNames>()
				{
					new SheetCompNames(true,  REQUIRED_CONTINUE, DISCIPLINEIDX, grpNname: DISCP + "20" , title: DISCP),
					new SheetCompNames(true,  REQUIRED_CONTINUE, SEP0IDX, grpNname: SEP + 21     , title: SEPR + 21),
					new SheetCompNames(true,  REQUIRED_END, CATEGORYIDX, grpNname: CAT + "20"   , title: CAT),
					new SheetCompNames(false, NOT_USED, SEP1IDX       ),
					new SheetCompNames(false, NOT_USED, SUBCATEGORYIDX),
					new SheetCompNames(false, NOT_USED, SEP2IDX       ),
					new SheetCompNames(false, NOT_USED, MODIFIERIDX   ),
					new SheetCompNames(false, NOT_USED, SEP3IDX       ),
					new SheetCompNames(false, NOT_USED, SUBMODIFIERIDX),
				}),

			new SheetCompList("TYPE30",
				new List<SheetCompNames>()
				{
					new SheetCompNames(true,  REQUIRED_CONTINUE, DISCIPLINEIDX, grpNname: DISCP + "30" , title: DISCP),
					new SheetCompNames(false, SKIP_CONTINUE, SEP0IDX       ),
					new SheetCompNames(true,  REQUIRED_CONTINUE, CATEGORYIDX, grpNname: CAT + "30"   , title: CAT),
					new SheetCompNames(true,  REQUIRED_CONTINUE, SEP1IDX, grpNname: SEP + 31     , title: SEPR + 31),
					new SheetCompNames(true,  REQUIRED_END, SUBCATEGORYIDX, grpNname: SUBCAT + "30", title: SUBCAT),
					new SheetCompNames(false, NOT_USED, SEP2IDX       ),
					new SheetCompNames(false, NOT_USED, MODIFIERIDX   ),
					new SheetCompNames(false, NOT_USED, SEP3IDX       ),
					new SheetCompNames(false, NOT_USED, SUBMODIFIERIDX),
				}),
			new SheetCompList("TYPE40",
				new List<SheetCompNames>()
				{
					// identifier
					new SheetCompNames(true,  REQUIRED_CONTINUE, DISCIPLINEIDX, grpNname: DISCP + "40" , title: DISCP),
					new SheetCompNames(true,  REQUIRED_CONTINUE, SEP0IDX, grpNname: SEP + 41     , title: SEPR + 41),
					new SheetCompNames(true,  REQUIRED_END, CATEGORYIDX, grpNname: CAT + "40"   , title: CAT),
					new SheetCompNames(false, NOT_USED, SEP1IDX       ),
					new SheetCompNames(false, NOT_USED, SUBCATEGORYIDX),
					new SheetCompNames(false, NOT_USED, SEP2IDX       ),
					new SheetCompNames(false, NOT_USED, MODIFIERIDX   ),
					new SheetCompNames(false, NOT_USED, SEP3IDX       ),
					new SheetCompNames(false, NOT_USED, SUBMODIFIERIDX),
				}),
			new SheetCompList("IDENT",
				new List<SheetCompNames>()
				{
					// identifier
					new SheetCompNames(true, OPTIONAL_END_OR_NEXT_REQD_START_OPT_SEQ, SEP4IDX, grpNname: SEP + 4      , title: SEPR + 4),
					new SheetCompNames(true, REQUIRED_CONTINUE, IDENTIFIERIDX, grpNname: ID           , title: ID),
					new SheetCompNames(true, OPTIONAL_SKIP_NEXT, SEP5IDX, grpNname: SEP + 5      , title: SEPR + 5),
					new SheetCompNames(true, REQUIRED_CONTINUE, SUBIDENTIFIERIDX, grpNname: SUBID        , title: SUBID),
					new SheetCompNames(true, REQUIRED_END_AND_OPT_SEQ_END, SEP6IDX, grpNname: SEP + 6      , title: SEPR + 6),
				})
		};


		// not applicable
		// required - continue
		// required - end of seq
		// skip
		// optional / seq end if not present
		// optional / seq end if not present / next is required if present
		// optional / seq end if not present / next is required if present // start of optional seq
		// required / end of optional seq

		public enum SeqCtrl
		{
			NOT_USED                                  = -2,
			SKIP_CONTINUE                             = -1,
			REQUIRED_CONTINUE                         = 0,
			REQUIRED_END                              = 1,
			REQUIRED_END_AND_OPT_SEQ_END              = 2,
			OPTIONAL                                  = 100,
			OPTIONAL_END                              = 101,
			OPTIONAL_END_OR_NEXT_REQD_START_OPT_SEQ   = 102,
			OPTIONAL_SKIP_NEXT                        = 103,
		}



		public class SheetCompList
		{
			public string TypeName { get; private set; }

			public int Count
			{
				get => ShtCompNames.Count;
			}

			public List<SheetCompNames> ShtCompNames { get; private set; }

			public SheetCompList(  string typeName, List<SheetCompNames> compNames)
			{
				TypeName = typeName;
				ShtCompNames = compNames;
			}
		}

		public struct SheetCompNames
		{
			public int Index { get; private set; }
			public string GrpName { get; private set; }
			public string Title { get; private set; }
			public bool IsUsed { get; private set; }
			public SeqCtrl SeqCtrl { get; private set; }

			public SheetCompNames(    bool isUsed, SeqCtrl seqCtrl, int index, string grpNname = "", string title = "")
			{
				IsUsed = isUsed;
				SeqCtrl = seqCtrl;
				Index = index;
				GrpName = grpNname;
				Title = title;
			}
		}
	}
}