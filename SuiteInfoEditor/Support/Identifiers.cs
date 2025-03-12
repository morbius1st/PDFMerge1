#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;

using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;

#endregion


// projname: $projectname$
// itemname: Identifiers
// username: jeffs
// created:  11/29/2024 5:17:51 PM

namespace SuiteInfoEditor.Support
{
	public class Identifiers
	{
	#region private fields

		private const int SUBJECT_WIDTH = -20;
		private const int VALUE_WIDTH_1 = -20;
		private const int VALUE_WIDTH_2 = -16;
		private const int VALUE_WIDTH_3 = -10;

		private ITblkFmt w;


		private static readonly Lazy<Identifiers> instance =
			new Lazy<Identifiers>(() => new Identifiers());

	#endregion

	#region ctor

		private Identifiers() { }

		public void Init(ITblkFmt w)
		{
			this.w = w;
		}

	#endregion

	#region public properties

		public static Identifiers Instance => instance.Value;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void showShtNumComponentConstants1()
		{
			w.TblkMsgLine("Showing sheet number component names");

			w.TblkMsgLine($"\t{"N_PHBLD "    , SUBJECT_WIDTH} | {N_PHBLD }");
			w.TblkMsgLine($"\t{"N_PHBLDG"    , SUBJECT_WIDTH} | {N_PHBLDG}");
			w.TblkMsgLine($"\t{"N_DISCP "    , SUBJECT_WIDTH} | {N_DISCP }");
			w.TblkMsgLine($"\t{"N_CAT   "    , SUBJECT_WIDTH} | {N_CAT   }");
			w.TblkMsgLine($"\t{"N_SUBCAT"    , SUBJECT_WIDTH} | {N_SUBCAT}");
			w.TblkMsgLine($"\t{"N_MOD   "    , SUBJECT_WIDTH} | {N_MOD   }");
			w.TblkMsgLine($"\t{"N_SUBMOD"    , SUBJECT_WIDTH} | {N_SUBMOD}");
			w.TblkMsgLine($"\t{"N_ID    "    , SUBJECT_WIDTH} | {N_ID    }");
			w.TblkMsgLine($"\t{"N_SUBID "    , SUBJECT_WIDTH} | {N_SUBID }");
			w.TblkMsgLine("\n");

			w.TblkMsgLine($"\t{"N_PBSEP "    , SUBJECT_WIDTH} |>{N_PBSEP }<");
			w.TblkMsgLine("\n");


			w.TblkMsgLine($"\t{"N_SEP0"    , SUBJECT_WIDTH} |>{N_SEP0}<");
			w.TblkMsgLine($"\t{"N_SEP1"    , SUBJECT_WIDTH} |>{N_SEP1}<");
			w.TblkMsgLine($"\t{"N_SEP2"    , SUBJECT_WIDTH} |>{N_SEP2}<");
			w.TblkMsgLine($"\t{"N_SEP3"    , SUBJECT_WIDTH} |>{N_SEP3}<");
			w.TblkMsgLine($"\t{"N_SEP4"    , SUBJECT_WIDTH} |>{N_SEP4}<");
			w.TblkMsgLine($"\t{"N_SEP5"    , SUBJECT_WIDTH} |>{N_SEP5}<");
			w.TblkMsgLine("\n");

			w.TblkMsgLine($"\t{"N_SHTNUM  "    , SUBJECT_WIDTH} | {N_SHTNUM  }");
			w.TblkMsgLine($"\t{"N_SHTID   "    , SUBJECT_WIDTH} | {N_SHTID   }");
			w.TblkMsgLine($"\t{"N_SHTTITLE"    ,  SUBJECT_WIDTH} | {N_SHTTITLE}");
			w.TblkMsgLine("\n");

			// private
			// w.DebugMsgLine($"\t{"N_SEP "    , SUBJECT_WIDTH} | {N_SEP }");
			// w.DebugMsgLine($"\t{"N_SEPR"    , SUBJECT_WIDTH} | {N_SEPR}");
			// w.DebugMsgLine($"\t{"PBSEP "    , SUBJECT_WIDTH} | {PBSEP }");
		}

		public void showShtNumComponentConstants2()
		{
			w.TblkMsgLine("Showing sheet number array const's");

			w.TblkMsgLine($"\t{"VI_PHBLDG       "    , SUBJECT_WIDTH} | {VI_PHBLDG       }");
			w.TblkMsgLine($"\t{"VI_PBSEP        "    , SUBJECT_WIDTH} | {VI_PBSEP        }");
			w.TblkMsgLine($"\t{"VI_DISCIPLINE   "    , SUBJECT_WIDTH} | {VI_DISCIPLINE   }");
			w.TblkMsgLine($"\t{"VI_SEP0         "    , SUBJECT_WIDTH} | {VI_SEP0         }");
			w.TblkMsgLine($"\t{"VI_CATEGORY     "    , SUBJECT_WIDTH} | {VI_CATEGORY     }");
			w.TblkMsgLine($"\t{"VI_SEP1         "    , SUBJECT_WIDTH} | {VI_SEP1         }");
			w.TblkMsgLine($"\t{"VI_SUBCATEGORY  "    , SUBJECT_WIDTH} | {VI_SUBCATEGORY  }");
			w.TblkMsgLine($"\t{"VI_SEP2         "    , SUBJECT_WIDTH} | {VI_SEP2         }");
			w.TblkMsgLine($"\t{"VI_MODIFIER     "    , SUBJECT_WIDTH} | {VI_MODIFIER     }");
			w.TblkMsgLine($"\t{"VI_SEP3         "    , SUBJECT_WIDTH} | {VI_SEP3         }");
			w.TblkMsgLine($"\t{"VI_SUBMODIFIER  "    , SUBJECT_WIDTH} | {VI_SUBMODIFIER  }");
			w.TblkMsgLine($"\t{"VI_SEP4         "    , SUBJECT_WIDTH} | {VI_SEP4         }");
			w.TblkMsgLine($"\t{"VI_IDENTIFIER   "    , SUBJECT_WIDTH} | {VI_IDENTIFIER   }");
			w.TblkMsgLine($"\t{"VI_SEP5         "    , SUBJECT_WIDTH} | {VI_SEP5         }");
			w.TblkMsgLine($"\t{"VI_SUBIDENTIFIER"    , SUBJECT_WIDTH} | {VI_SUBIDENTIFIER}");
			w.TblkMsgLine($"\t{"VI_SORT_SUFFIX  "    , SUBJECT_WIDTH} | {VI_SORT_SUFFIX  }");
			w.TblkMsgLine($"\t{"VI_SHTNUM       "    , SUBJECT_WIDTH} | {VI_SHTNUM       }");
			w.TblkMsgLine($"\t{"VI_SHTID        "    , SUBJECT_WIDTH} | {VI_SHTID        }");
			w.TblkMsgLine($"\t{"VI_SHTTITLE     "    , SUBJECT_WIDTH} | {VI_SHTTITLE     }");

			w.TblkMsgLine("\n");

			w.TblkMsgLine($"\t{"VI_COMP_COUNT" , SUBJECT_WIDTH} | {VI_COMP_COUNT, VALUE_WIDTH_1}  (number of sheet components [up to & incl. VI_SUBIDENTIFIER] )");
			w.TblkMsgLine($"\t{"VI_COUNT"      , SUBJECT_WIDTH} | {VI_COUNT     , VALUE_WIDTH_1}  (maximum component value in this list + 1 [equal to VI_SHTTITLE + 1] ))");
			w.TblkMsgLine($"\t{"VI_MIN"        , SUBJECT_WIDTH} | {VI_MIN       , VALUE_WIDTH_1}  (minimum component value in this list [equal to VI_PHBLDG] )");

		}

		public void ShowCompNameInfo1()
		{
			w.TblkMsgLine("Showing component name info (individual data elements)");

			w.TblkMsgLine($"\t{"CNI_SHTID   "    , SUBJECT_WIDTH} | {CNI_SHTID   .Index, 4}, {CNI_SHTID   .Name, VALUE_WIDTH_1}, {CNI_SHTID   .Type}");
			w.TblkMsgLine($"\t{"CNI_SHTTITLE"    , SUBJECT_WIDTH} | {CNI_SHTTITLE.Index, 4}, {CNI_SHTTITLE.Name, VALUE_WIDTH_1}, {CNI_SHTTITLE.Type}");
			w.TblkMsgLine($"\t{"CNI_PHBLD   "    , SUBJECT_WIDTH} | {CNI_PHBLD   .Index, 4}, {CNI_PHBLD   .Name, VALUE_WIDTH_1}, {CNI_PHBLD   .Type}");
			w.TblkMsgLine($"\t{"CNI_DISCP   "    , SUBJECT_WIDTH} | {CNI_DISCP   .Index, 4}, {CNI_DISCP   .Name, VALUE_WIDTH_1}, {CNI_DISCP   .Type}");
			w.TblkMsgLine($"\t{"CNI_CAT     "    , SUBJECT_WIDTH} | {CNI_CAT     .Index, 4}, {CNI_CAT     .Name, VALUE_WIDTH_1}, {CNI_CAT     .Type}");
			w.TblkMsgLine($"\t{"CNI_SUBCAT  "    , SUBJECT_WIDTH} | {CNI_SUBCAT  .Index, 4}, {CNI_SUBCAT  .Name, VALUE_WIDTH_1}, {CNI_SUBCAT  .Type}");
			w.TblkMsgLine($"\t{"CNI_MOD     "    , SUBJECT_WIDTH} | {CNI_MOD     .Index, 4}, {CNI_MOD     .Name, VALUE_WIDTH_1}, {CNI_MOD     .Type}");
			w.TblkMsgLine($"\t{"CNI_SUBMOD  "    , SUBJECT_WIDTH} | {CNI_SUBMOD  .Index, 4}, {CNI_SUBMOD  .Name, VALUE_WIDTH_1}, {CNI_SUBMOD  .Type}");
			w.TblkMsgLine($"\t{"CNI_ID      "    , SUBJECT_WIDTH} | {CNI_ID      .Index, 4}, {CNI_ID      .Name, VALUE_WIDTH_1}, {CNI_ID      .Type}");
			w.TblkMsgLine($"\t{"CNI_SUBID   "    , SUBJECT_WIDTH} | {CNI_SUBID   .Index, 4}, {CNI_SUBID   .Name, VALUE_WIDTH_1}, {CNI_SUBID   .Type}");
			w.TblkMsgLine($"\t{"CNI_SEP0    "    , SUBJECT_WIDTH} | {CNI_SEP0    .Index, 4}, {CNI_SEP0    .Name, VALUE_WIDTH_1}, {CNI_SEP0    .Type}");
			w.TblkMsgLine($"\t{"CNI_SEP1    "    , SUBJECT_WIDTH} | {CNI_SEP1    .Index, 4}, {CNI_SEP1    .Name, VALUE_WIDTH_1}, {CNI_SEP1    .Type}");
			w.TblkMsgLine($"\t{"CNI_SEP2    "    , SUBJECT_WIDTH} | {CNI_SEP2    .Index, 4}, {CNI_SEP2    .Name, VALUE_WIDTH_1}, {CNI_SEP2    .Type}");
			w.TblkMsgLine($"\t{"CNI_SEP3    "    , SUBJECT_WIDTH} | {CNI_SEP3    .Index, 4}, {CNI_SEP3    .Name, VALUE_WIDTH_1}, {CNI_SEP3    .Type}");
			w.TblkMsgLine($"\t{"CNI_SEP4    "    , SUBJECT_WIDTH} | {CNI_SEP4    .Index, 4}, {CNI_SEP4    .Name, VALUE_WIDTH_1}, {CNI_SEP4    .Type}");
			w.TblkMsgLine($"\t{"CNI_SEP5    "    , SUBJECT_WIDTH} | {CNI_SEP5    .Index, 4}, {CNI_SEP5    .Name, VALUE_WIDTH_1}, {CNI_SEP5    .Type}");


			w.TblkMsgLine("\nShowing same info in the Dictionary: 'CompNames'");

			int idx = 0;

			w.TblkMsgLine($"\t     | key  | idx | name                | type");

			foreach (KeyValuePair<int, CompNameInfo> cni in CompNames)
			{
				w.TblkMsgLine($"\t {idx++,-3} | {cni.Key, 4} | {cni.Value.Index, 4}, {cni.Value.Name, VALUE_WIDTH_1}, {cni.Value.Type}");
			}

		}

		public void ShowSheetNumComponentData()
		{
			int idx = 0;

			foreach (KeyValuePair<int, ShtNumComps2> snc in SheetNumComponentData)
			{
				w.TblkMsg($"\t {idx++,-3} | {snc.Key, 4} | {snc.Value.Index, 4} {snc.Value.Name, VALUE_WIDTH_2}");
				w.TblkMsg($" {snc.Value.AbbrevName , VALUE_WIDTH_3}");
				w.TblkMsg($" {snc.Value.ItemClass  , VALUE_WIDTH_1}");
				w.TblkMsg($" {snc.Value.GroupId    , VALUE_WIDTH_2}");

				if (!snc.Value.Neumonic.ToString().IsVoid())
				{
					w.TblkMsg($" {snc.Value.Neumonic   , VALUE_WIDTH_1}");
					w.TblkMsgLine($" ( >{snc.Value.Neumonic.Preface}< >{snc.Value.Neumonic.Body}< >{snc.Value.Neumonic.Suffix}< )");
				} else
				{
					w.TblkMsgLine("---");
				}
			}
		}


	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(Identifiers)}";
		}

	#endregion
	}
}