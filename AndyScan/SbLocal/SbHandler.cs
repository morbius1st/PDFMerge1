using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AndyScan.Samples;
using UtilityLibrary;
using static AndyScan.SbLocal.SbMainMnuItemExt;
using static AndyScan.SbSystem.SbmnuItems;
using AndyScan.SbSystem;

// username: jeffs
// created:  3/16/2025 7:58:03 AM

namespace AndyScan.SbLocal
{
	public class SbHandler
	{
	#region private fields

		private IMenu menu;

		private ITblkFmt iw;

		private  Dictionary<string, Tuple<SbMnuItemId, List<string>,
			List<string>, List<string>, int, List<int>>>[] Menus;

		private int menuIdx;

		private SbMnuItemId menuChoice;

		private Sample sampleSet;

	#endregion

	#region ctor

		public SbHandler(ITblkFmt iw, IMenu im)
		{
			DM.Start0();
			this.iw = iw;
			menu = im;

			Menus = menu.Menus;

			DM.End0();
		}

	#endregion

	#region public properties

		public Sample SampleSet
		{
			get => sampleSet;
			set => sampleSet = value;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool? ProcessMenuChoice(int menuItem, string menuKey, out int menuIndex)
		{
			DM.Start0();

			menuIdx = menuItem;

			menuChoice = Menus[menuIdx][menuKey].Item1;

			// Debug.WriteLine($"process menu choice| menu = {menu} | item = {menuChoice}");
			bool? result = true;

			switch (menuIdx)
			{
			case 0:
				{
					result = processMenu0();
					break;
				}
			case 1:
				{
					result = processMenu1();
					break;
				}
			case 2:
				{
					result = processMenu2();
					break;
				}
			default:
				{
					string key = "default";
					iw.TblkMsgLine($"got choice '{key}'");
					DM.Stat0($"got choice '{key}'");
					
					break;
				}
			}

			menuIndex = menuIdx;

			DM.End0();

			return result;
		}

	#endregion

	#region private methods

		private bool? processMenu0()
		{
			DM.Start0();
			bool? result = true;

			if (menuChoice.Value == SBMI_EXIT.Value)
			{
				result = menuExit();
				menuIdx = -2;
			}
			else if (menuChoice.Value == MMI_INIT_1.Value)
			{
				result = menu0Init1();
			}
			else if (menuChoice.Value == MMI_INIT_2.Value)
			{
				result = menu0Init2();
			}
			else if (menuChoice.Value == MMI_RESET_0.Value)
			{
				result = menu0Reset0();
			}
			else if (menuChoice.Value == MMI_RESET_2.Value)
			{
				result = menu0Reset2();
			}
			else if (menuChoice.Value == MMI_RESET_4.Value)
			{
				result = menu0Reset4();
			}
			else if (menuChoice.Value == MMI_RESET_6.Value)
			{
				result = menu0Reset6();
			}
			else if (menuChoice.Value == MMI_RESET_8.Value)
			{
				result = menu0Reset8();
			}
			else if (menuChoice.Value == MMI_ADD_2.Value)
			{
				result = menu0Add2();
			}
			else if (menuChoice.Value == MMI_ADD_M.Value)
			{
				result = menu0AddM();
			}
			else if (menuChoice.Value == MMI_DATA_SEL1.Value)
			{
				result = menu0Sel1();
			}
			else if (menuChoice.Value == MMI_DATA_READ1.Value)
			{
				result = menu0Read();
				if (result == false) menuIdx = 0;
			}
			else if (menuChoice.Value == MMI_OP_SCAN1.Value)
			{
				result = menu0Scan1();
			}
			else
			{
				iw.TblkMsgLine($"got un-planned choice {menuChoice}");
			}

			DM.End0();
			return result;
		}

		private bool? processMenu1()
		{
			DM.Start0();

			bool? result = true;

			if (menuChoice.Value == MMI_OP_SCAN1.Value)
			{
				result = menu1Scan2();
			}
			else if (menuChoice.Value == SBMI_EXIT.Value)
			{
				result = menuExit();
				menuIdx = 0;
			}

			DM.End0();

			return result;
		}

		private bool? processMenu2()
		{
			DM.Start0();

			bool? result = true;

			if (menuChoice.Value == SBMI_EXIT.Value || 
				menuChoice.Value != MMI_OP_2_SCDATA.Value)
			{
				result = menuExit();
				menuIdx = 0;
			}
			else
			{
				result = menu2ScanData();

				if (result == true)
				{
					showInScanData();
				}
			}

			DM.End0();
			return result;
		}

		// general procedures

		private bool? menuExit()
		{
			DM.Stat0($"got choice 'X-{menuIdx}'");

			iw.TblkMsgLine($"got choice 'X-{menuIdx}'");

			return null;
		}

		// menu 0 procedures

		private bool? menu0Init1()
		{
			string key = "Init";
			iw.TblkMsgLine($"got choice '{key}-1'");
			DM.Stat0($"got choice '{key}-1'");

			return true;
		}

		private bool? menu0Init2()
		{
			string key = "Init";
			iw.TblkMsgLine($"got choice '{key}-2'");
			DM.Stat0($"got choice '{key}-2'");

			return true;
		}

		private bool? menu0Reset0()
		{
			string key = "Reset";
			iw.TblkMsgLine($"got choice '{key}-0'");
			DM.Stat0($"got choice '{key}-0'");

			return true;
		}

		private bool? menu0Reset2()
		{
			string key = "Reset";
			iw.TblkMsgLine($"got choice '{key}-2'");
			DM.Stat0($"got choice '{key}-2'");

			return true;
		}

		private bool? menu0Reset4()
		{
			string key = "Reset";
			iw.TblkMsgLine($"got choice '{key}-4'");
			DM.Stat0($"got choice '{key}-4'");

			return true;
		}

		private bool? menu0Reset6()
		{
			string key = "Reset";
			iw.TblkMsgLine($"got choice '{key}-6'");
			DM.Stat0($"got choice '{key}-6'");

			return true;
		}

		private bool? menu0Reset8()
		{
			string key = "Reset";
			iw.TblkMsgLine($"got choice '{key}-8'");
			DM.Stat0($"got choice '{key}-8'");

			return true;
		}

		private bool? menu0Add2()
		{
			string key = "Add";
			iw.TblkMsgLine($"got choice '{key}-2'");
			DM.Stat0($"got choice '{key}-2'");

			return true;
		}

		private bool? menu0AddM()
		{
			string key = "Add";
			iw.TblkMsgLine($"got choice '{key}-M'");
			DM.Stat0($"got choice '{key}-M'");

			return true;
		}

		private bool? menu0Sel1()
		{
			string key = "Sel";
			iw.TblkMsgLine($"got choice '{key}-1'");
			DM.Stat0($"got choice '{key}-1'");

			return true;
		}

		private bool? menu0Read()
		{
			iw.TblkMsgLine("got choice 'RD'");
			DM.Stat0("got choice 'RD'");

			return false;
		}

		private bool? menu0Scan1()
		{
			iw.TblkMsgLine("got choice 'SC-1'");
			DM.Stat0("got choice 'SC-1'");

			return true;
		}


		// menu 1 procedures

		private bool? menu1Scan2()
		{
			iw.TblkMsgLine("got choice 'SC-2'");
			DM.Stat0("got choice 'SC-2'");

			return true;
		}


		// menu 2 procedures

		private bool? menu2ScanData()
		{
			bool? result = true;
			int option = menuChoice.SubValue;

			result = Samples.SampleData.SampleScanData.TryGetValue(option, out sampleSet);

			return result;
		}

		private void showInScanData()
		{
			iw.TblkMsgLine("Scan Data");
			iw.TblkMsgLine($"{sampleSet.Description}");
		}

		#endregion

		#region event consuming

		#endregion

		#region event publishing

		#endregion

		#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(SbHandler)}";
		}

	#endregion
	}
}