#region + Using Directives

using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Xml;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers.SeqCtrlUse;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;

#endregion

// user name: jeffs
// created:   10/1/2020 7:16:21 AM

namespace AndyShared.FileSupport.FileNameSheetPDF
{
	public class FileNameSheetIdentifiers
	{
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
			UNASSIGNED = -1,
			PHBLDG = 0,
			TYPE10 = 1,
			TYPE20 = 2,
			TYPE30 = 3,
			TYPE40 = 4,
			IDENT = 5,
			COUNT
		}


		// shorthand access to the indicies
		internal const int SHTID_COMP_IDX = 2;
		internal const int SHTNAME_COMP_IDX = 3;

		// the index in the final component array: ShtCompList
		internal const int PHBLDG_COMP_IDX = 0; //
		internal const int PBSEP_COMP_IDX = 1;  //

		internal const int COMP_PB_IDX_MIN = PHBLDG_COMP_IDX;
		internal const int COMP_PB_IDX_COUNT = PBSEP_COMP_IDX + 1;

		internal const int DISCIPLINE_COMP_IDX = 0;  //
		internal const int SEP0_COMP_IDX = 1;        //
		internal const int CATEGORY_COMP_IDX = 2;    //
		internal const int SEP1_COMP_IDX = 3;        //
		internal const int SUBCATEGORY_COMP_IDX = 4; //
		internal const int SEP2_COMP_IDX = 5;        //
		internal const int MODIFIER_COMP_IDX = 6;    //
		internal const int SEP3_COMP_IDX = 7;        //
		internal const int SUBMODIFIER_COMP_IDX = 8; //

		internal const int COMP_SHTID_IDX_MIN = DISCIPLINE_COMP_IDX;
		internal const int COMP_SHTID_IDX_COUNT = SUBMODIFIER_COMP_IDX + 1;

		internal const int SEP4_COMP_IDX = 0;          //
		internal const int IDENTIFIER_COMP_IDX = 1;    //
		internal const int SEP5_COMP_IDX = 2;          //
		internal const int SUBIDENTIFIER_COMP_IDX = 3; //
		internal const int SEP6_COMP_IDX = 4;          //

		internal const int COMP_IDENT_IDX_MIN = SEP4_COMP_IDX;
		internal const int COMP_IDENT_IDX_COUNT = SEP6_COMP_IDX + 1;


		private const int SHTID_VALUE_IDX = -1;
		private const int SHTNAME_VALUE_IDX = -1;

		// // the index in the final component value array: FileNameSheetPdf.SheetComps
		private const int PHBLDG_VALUE_IDX = 0;         // 0
		private const int PBSEP_VALUE_IDX = 1;          //
		private const int DISCIPLINE_VALUE_IDX = 2;     // 1
		private const int SEP0_VALUE_IDX = 3;           //
		private const int CATEGORY_VALUE_IDX = 4;       // 2
		private const int SEP1_VALUE_IDX = 5;           //
		private const int SUBCATEGORY_VALUE_IDX = 6;    // 3
		private const int SEP2_VALUE_IDX = 7;           //
		private const int MODIFIER_VALUE_IDX = 8;       // 4
		private const int SEP3_VALUE_IDX = 9;           // 
		private const int SUBMODIFIER_VALUE_IDX = 10;   // 5
		private const int SEP4_VALUE_IDX = 11;          // 
		private const int IDENTIFIER_VALUE_IDX = 12;    // 6
		private const int SEP5_VALUE_IDX = 13;          // 
		private const int SUBIDENTIFIER_VALUE_IDX = 14; // 7
		private const int SEP6_VALUE_IDX = 15;          // 

		internal const int VALUE_IDX_COUNT = SEP6_VALUE_IDX + 1; // 28 (req'd # + extra
		internal const int VALUE_IDX_MIN = PHBLDG_VALUE_IDX;

		internal static int[] IDX_XLATE = new[]
		{
			PHBLDG_VALUE_IDX, DISCIPLINE_VALUE_IDX, CATEGORY_VALUE_IDX, SUBCATEGORY_VALUE_IDX,
			MODIFIER_VALUE_IDX, SUBMODIFIER_VALUE_IDX, IDENTIFIER_VALUE_IDX, SUBIDENTIFIER_VALUE_IDX
		};

		internal int IDX_XLATE_MIN = 0;

		internal int IDX_XLATE_MAX = IDX_XLATE.Length - 1;

		public enum SeqCtrlUse
		{
			NOT_USED = -2,
			SKIP_CONTINUE = -1,
			REQUIRED_CONTINUE = 0,
			REQUIRED_END = 1,
			REQUIRED_END_AND_OPT_SEQ_END = 2,
			OPTIONAL = 100,
			OPTIONAL_END = 101,
			OPTIONAL_END_OR_NEXT_REQD_START_OPT_SEQ = 102,
			OPTIONAL_SKIP_NEXT = 103,
		}

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


		private const string PHBLD = "PhBldgid";
		private const string DISCP = "Discipline";
		private const string CAT = "Category";
		private const string SUBCAT = "SubCategory";
		private const string MOD = "Modifier";
		private const string SUBMOD = "SubModifier";
		private const string ID = "Identifier";
		private const string SUBID = "SubIdentifier";
		private const string SEP = "sep";
		private const string SEPR = "Separator";


		//
		// public string CompGrpName(ShtCompTypes type, int compListIdx)
		// {
		// 	return ShtCompList[(int) type].ShtCompInfo[compListIdx].GrpName;
		// }
		//
		// public string CompTitle(ShtCompTypes type, int compListIdx)
		// {
		// 	return ShtCompList[(int) type].ShtCompInfo[compListIdx].Title;
		// }
		//
		// public int CompValueIdx(ShtCompTypes type, int compListIdx)
		// {
		// 	return ShtCompList[(int) type].ShtCompInfo[compListIdx].ValueIndex;
		// }
		//
		// public bool CompIsUsed(ShtCompTypes type, int compListIdx)
		// {
		// 	return ShtCompList[(int) type].ShtCompInfo[compListIdx].IsUsed;
		// }
		//
		// public SeqCtrlUse CompSeqCtrl(ShtCompTypes type, int compListIdx)
		// {
		// 	return ShtCompList[(int) type].ShtCompInfo[compListIdx].SeqCtrlUseUse;
		// }
		//
		// public SheetCompList ShtCompLst(ShtCompTypes type) => ShtCompList[(int) type];
		//
		// public string SheetNumTypeTitle(ShtCompTypes type) => ShtCompLst(type).Title;
		//
		// public List<SheetCompList> ShtCompList = new List<SheetCompList>()
		// {
		// 	new SheetCompList("PhBldg", null,
		// 		new List<SheetCompInfo>()
		// 		{
		// 			new SheetCompInfo(true, REQUIRED_CONTINUE,  PHBLDG_VALUE_IDX        , grpNname: PHBLD ,
		// 				title: "Phase/Bldg"),
		// 			new SheetCompInfo(true, REQUIRED_END,       PBSEP_VALUE_IDX         , grpNname: "pbsep"  ,
		// 				title: "PB" + SEPR),
		// 			new SheetCompInfo(true, NOT_USED,           SHTID_VALUE_IDX         , grpNname: "shtid"  ,
		// 				title: "Sheet Id"),
		// 			new SheetCompInfo(true, NOT_USED,           SHTNAME_VALUE_IDX       , grpNname: "shtname",
		// 				title: "Sheet Name")
		// 		}),
		// 	new SheetCompList("TYPE10", "Categorized-Extended",
		// 		new List<SheetCompInfo>()
		// 		{
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , DISCIPLINE_VALUE_IDX   , grpNname: DISCP + "10" ,
		// 				title: DISCP),
		// 			new SheetCompInfo(false, SKIP_CONTINUE     , SEP0_VALUE_IDX         , grpNname: SEP + 0     ,
		// 				title: SEPR + 0),
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , CATEGORY_VALUE_IDX     , grpNname: CAT + "10"   ,
		// 				title: CAT),
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , SEP1_VALUE_IDX         , grpNname: SEP + 11     ,
		// 				title: SEPR + 11),
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , SUBCATEGORY_VALUE_IDX  , grpNname: SUBCAT + "10",
		// 				title: SUBCAT),
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , SEP2_VALUE_IDX         , grpNname: SEP + 12     ,
		// 				title: SEPR + 12),
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , MODIFIER_VALUE_IDX     , grpNname: MOD + "10"   ,
		// 				title: MOD),
		// 			new SheetCompInfo(true,  OPTIONAL_SKIP_NEXT, SEP3_VALUE_IDX         , grpNname: SEP + 13     ,
		// 				title: SEPR + 13),
		// 			new SheetCompInfo(true,  OPTIONAL_END      , SUBMODIFIER_VALUE_IDX  , grpNname: SUBMOD + "10",
		// 				title: SUBMOD),
		// 		}),
		// 	new SheetCompList("TYPE20", "Green",
		// 		new List<SheetCompInfo>()
		// 		{
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , DISCIPLINE_VALUE_IDX   , grpNname: DISCP + "20" ,
		// 				title: DISCP),
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , SEP0_VALUE_IDX         , grpNname: SEP + 21     ,
		// 				title: SEPR + 21),
		// 			new SheetCompInfo(true,  REQUIRED_END      , CATEGORY_VALUE_IDX     , grpNname: CAT + "20"    ,
		// 				title: CAT),
		// 			new SheetCompInfo(false, NOT_USED          , SEP1_VALUE_IDX          ) ,
		// 			new SheetCompInfo(false, NOT_USED          , SUBCATEGORY_VALUE_IDX   ) ,
		// 			new SheetCompInfo(false, NOT_USED          , SEP2_VALUE_IDX          ) ,
		// 			new SheetCompInfo(false, NOT_USED          , MODIFIER_VALUE_IDX      ) ,
		// 			new SheetCompInfo(false, NOT_USED          , SEP3_VALUE_IDX          ) ,
		// 			new SheetCompInfo(false, NOT_USED          , SUBMODIFIER_VALUE_IDX   ) ,
		// 		}),
		//
		// 	new SheetCompList("TYPE30", "Categorized",
		// 		new List<SheetCompInfo>()
		// 		{
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , DISCIPLINE_VALUE_IDX   , grpNname: DISCP + "30" ,
		// 				title: DISCP),
		// 			new SheetCompInfo(false, SKIP_CONTINUE     , SEP0_VALUE_IDX          ),
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , CATEGORY_VALUE_IDX     , grpNname: CAT + "30"   ,
		// 				title: CAT),
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , SEP1_VALUE_IDX         , grpNname: SEP + 31     ,
		// 				title: SEPR + 31),
		// 			new SheetCompInfo(true,  REQUIRED_END      , SUBCATEGORY_VALUE_IDX  , grpNname: SUBCAT + "30",
		// 				title: SUBCAT),
		// 			new SheetCompInfo(false, NOT_USED          , SEP2_VALUE_IDX          ),
		// 			new SheetCompInfo(false, NOT_USED          , MODIFIER_VALUE_IDX      ),
		// 			new SheetCompInfo(false, NOT_USED          , SEP3_VALUE_IDX          ),
		// 			new SheetCompInfo(false, NOT_USED          , SUBMODIFIER_VALUE_IDX   ),
		// 		}),
		// 	new SheetCompList("TYPE40", "Traditional",
		// 		new List<SheetCompInfo>()
		// 		{
		// 			// identifier
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , DISCIPLINE_VALUE_IDX    , grpNname: DISCP + "40" ,
		// 				title: DISCP),
		// 			new SheetCompInfo(true,  REQUIRED_CONTINUE , SEP0_VALUE_IDX          , grpNname: SEP + 41     ,
		// 				title: SEPR + 41),
		// 			new SheetCompInfo(true,  REQUIRED_END      , CATEGORY_VALUE_IDX      , grpNname: CAT + "40"   ,
		// 				title: CAT),
		// 			new SheetCompInfo(false, NOT_USED          , SEP1_VALUE_IDX           ),
		// 			new SheetCompInfo(false, NOT_USED          , SUBCATEGORY_VALUE_IDX    ),
		// 			new SheetCompInfo(false, NOT_USED          , SEP2_VALUE_IDX           ),
		// 			new SheetCompInfo(false, NOT_USED          , MODIFIER_VALUE_IDX       ),
		// 			new SheetCompInfo(false, NOT_USED          , SEP3_VALUE_IDX           ),
		// 			new SheetCompInfo(false, NOT_USED          , SUBMODIFIER_VALUE_IDX    ),
		// 		}),
		// 	new SheetCompList("IDENT", null,
		// 		new List<SheetCompInfo>()
		// 		{
		// 			// identifier
		// 			new SheetCompInfo(true, OPTIONAL_END_OR_NEXT_REQD_START_OPT_SEQ , SEP4_VALUE_IDX           ,
		// 				grpNname: SEP + 4  , title: SEPR + 4),
		// 			new SheetCompInfo(true, REQUIRED_CONTINUE                       , IDENTIFIER_VALUE_IDX     ,
		// 				grpNname: ID       , title: ID),
		// 			new SheetCompInfo(true, OPTIONAL_SKIP_NEXT                      , SEP5_VALUE_IDX           ,
		// 				grpNname: SEP + 5  , title: SEPR + 5),
		// 			new SheetCompInfo(true, REQUIRED_CONTINUE                       , SUBIDENTIFIER_VALUE_IDX ,
		// 				grpNname: SUBID    , title: SUBID),
		// 			new SheetCompInfo(true, REQUIRED_END_AND_OPT_SEQ_END            , SEP6_VALUE_IDX           ,
		// 				grpNname: SEP + 6  , title: SEPR + 6),
		// 		})
		// };


		public class SheetCompList
		{
			public string TypeName { get; private set; }

			public string Title { get; private set; }

			public int Count
			{
				get => ShtCompInfo.Count;
			}

			public List<SheetCompInfo> ShtCompInfo { get; private set; }

			public SheetCompList(string typeName, string title, List<SheetCompInfo> compInfo)
			{
				TypeName = typeName;
				Title = title;
				ShtCompInfo = compInfo;
			}
		}

		public class SheetCompInfo
		{
			public int ValueIndex { get; private set; }
			public string GrpName { get; private set; }
			public string Title { get; private set; }
			public bool IsUsed { get; private set; }
			public SeqCtrlUse SeqCtrlUseUse { get; private set; }

			public SheetCompInfo(bool isUsed, SeqCtrlUse seqCtrlUseUse,
				int valueIndex, string grpNname = "", string title = "")
			{
				IsUsed = isUsed;
				SeqCtrlUseUse = seqCtrlUseUse;
				ValueIndex = valueIndex;
				GrpName = grpNname;
				Title = title;
			}
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


			public bool IsUseOK => (int) SeqCtrlUse >= 0;

			public bool IsReqdProceedOK
			{
				get
				{
					// SeqCtrlProceedReqd test = GetProceedReqd();
					// int result = test.CompareTo(SeqCtrlProceedReqd.NOT_USED);
					//
					return (int) GetProceedReqd() >= 0;
				}
			}

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


		public List<SheetCompList2> ShtCompList2 = new List<SheetCompList2>()
		{
			new SheetCompList2("PhBldg", null,
				new List<SheetCompInfo2>()
				{
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.CONTINUE),
						PHBLDG_VALUE_IDX, grpNname: PHBLD, title: "Phase/Bldg"),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.REQUIRED, SeqCtrlProceedReqd.END),
						PBSEP_VALUE_IDX, grpNname: "pbsep", title: "PB" + SEPR),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.NOT_USED),
						SHTID_VALUE_IDX , grpNname: "shtid" , title: "Sheet Id"),
					new SheetCompInfo2(
						new SeqCtrlReqd(SeqCtrlUse2.NOT_USED),
						SHTNAME_VALUE_IDX , grpNname: "shtname", title: "Sheet Name")
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
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.CONTINUE, SeqCtrlNextOpt.REQ_IF_PRIOR),
						SUBIDENTIFIER_VALUE_IDX , grpNname: SUBID , title: SUBID),
					new SheetCompInfo2(
						new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.END, SeqCtrlNextOpt.SEQ_END_SEQ_REQ),
						SEP6_VALUE_IDX , grpNname: SEP + 6 , title: SEPR + 6),
				})
		};


		//
		// public SeqItemReqd x1 = new SeqItemReqd("TEST", 1);
		// public SeqItemOpt x2 = new SeqItemOpt("TEST", 1);
		//
		// public abstract class SeqItem : IEquatable<SeqItem>
		// {
		// 	public abstract int Ctrl { get; }
		//
		// 	public string Name { get; set; }
		// 	public int Seq { get; set; }
		//
		// 	public SeqItem() {}
		//
		// 	public SeqItem(string name, int seq)
		// 	{
		// 		Name = name;
		// 		Seq = seq;
		// 	}
		//
		// 	public static bool operator== (SeqItem s1, SeqItem s2)
		// 	{
		// 		return s1.Equals(s2);
		// 	}
		//
		// 	public static bool operator!= (SeqItem s1, SeqItem s2)
		// 	{
		// 		return !s1.Equals(s2);
		// 	}
		//
		// 	public bool Equals(SeqItem other)
		// 	{
		// 		return (
		// 			other != null &&
		// 			Name != null &&
		// 			other.Ctrl == Ctrl &&
		// 			other.Name != null &&
		// 			other.Seq == Seq &&
		// 			other.Name.Equals(Name));
		// 	}
		//
		// 	public override string ToString()
		// 	{
		// 		return Name;
		// 	}
		// }
		//
		// public class SeqItemReqd : SeqItem
		// {
		// 	public override int Ctrl { get; } = 1;
		//
		// 	public SeqItemReqd(string name, int seq) : base(name, seq) {}
		//
		// 	// public override bool Equals(SeqItem other)
		// 	// {
		// 	// 	return (
		// 	// 		Name != null &&
		// 	// 		other.Name != null &&
		// 	// 		other.Seq == Seq &&
		// 	// 		other.Name.Equals(Name));
		// 	// }
		// }
		//
		// public class SeqItemOpt : SeqItem
		// {
		// 	public override int Ctrl { get; } = 2;
		//
		// 	public SeqItemOpt(string name, int seq) : base(name, seq) {}
		//
		// 	// public override bool Equals(SeqItem other)
		// 	// {
		// 	// 	return (
		// 	// 		other != null &&
		// 	// 		base.Name != null &&
		// 	// 		other.Ctrl == Ctrl && 
		// 	// 		other.Name != null &&
		// 	// 		other.Seq == base.Seq && 
		// 	// 		other.Name.Equals(Name));
		// 	// }
		// }


		// public void test()
		// {
		// 	SheetCompInfo2 t1 =
		// 		new SheetCompInfo2(new SeqCtrlOpt(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedOpt.END, SeqCtrlNextOpt.NOT_USED),
		// 			1, grpNname: "", title: "");
		// 	SheetCompInfo2 t2 = new SheetCompInfo2(
		// 		new SeqCtrlReqd(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedReqd.CONTINUE, SeqCtrlNextReqd.NOT_USED), 1,
		// 		grpNname: "",
		// 		title: "");

		// SheetCompInfo2 t1 = new SheetCompInfo2(true,
		// 	new SeqCtrl<SeqCtrlNextOpt>(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedReqd.END,
		// 		SeqCtrlNextOpt.NOT_USED), 1, "group", "title");
		//
		// SheetCompInfo2 t2 = new SheetCompInfo2(true,
		// 	new SeqCtrl<SeqCtrlNextReqd>(SeqCtrlUse2.OPTIONAL, SeqCtrlProceedReqd.END,
		// 		SeqCtrlNextReqd.NOT_USED),1, "group", "title");
		// }


		// public class CiList
		// {
		// public List<ASeqCtl> SciList = new List<ASeqCtl>()
		// {
		// 	new SeqCtrl<SeqCtrlNextOptional>(SeqCtrlUse2.OPTIONAL, SeqCtrlProceed.END, SeqCtrlNextOptional.NOT_USED),
		// 	new SeqCtrl<SeqCtrlNextRequired>(SeqCtrlUse2.OPTIONAL, SeqCtrlProceed.END, SeqCtrlNextRequired.NOT_USED)
		// };

		// public CiList(ASeqCtrl A, ASeqCtrl B)
		// {
		// 	C = A;
		// 	D = B;
		// }
		//
		// public ASeqCtrl C;
		// public ASeqCtrl D;
		//
		// public ASeqCtrl Z = new SeqCtrl<SeqCtrlNextOptional>(SeqCtrlUse2.OPTIONAL, SeqCtrlProceed.END,
		// 	SeqCtrlNextOptional.NOT_USED);
		//
		// public SeqCtrlNextOptional GetNextAsOptional()
		// {
		// 	SeqCtrlNextOptional s = SeqCtrlNextOptional.NOT_USED;
		//
		// 	Type t =Z.Type;
		//
		// 	if (t == typeof(SeqCtrlNextOptional))
		// 	{
		// 		s = ((SeqCtrl<SeqCtrlNextOptional>) Z).Next;
		// 	}
		//
		// 	return s;
		// }
		//
		//
		// CiList cil = new CiList(
		// 		new SeqCtrl<SeqCtrlNextOptional>(SeqCtrlUse2.OPTIONAL, SeqCtrlProceed.END, SeqCtrlNextOptional.NOT_USED),
		// 		new SeqCtrl<SeqCtrlNextRequired>(SeqCtrlUse2.OPTIONAL, SeqCtrlProceed.END, SeqCtrlNextRequired.OPT_SEQ_END)
		// 	);


		// public SeqCtrlNextOptional GetNextAsOptional(int idx)
		// {
		// 	SeqCtrlNextOptional s = SeqCtrlNextOptional.NOT_USED;
		//
		// 	Type t = SciList[idx].Type;
		//
		// 	if (t == typeof(SeqCtrlNextOptional))
		// 	{
		// 		s = ((SeqCtrl<SeqCtrlNextOptional>) SciList[idx]).Next;
		// 	}
		//
		// 	return s;
		// }
		// }
	}
}