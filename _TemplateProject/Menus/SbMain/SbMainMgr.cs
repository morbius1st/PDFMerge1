using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilityLibrary;
using static _TemplateProject.Menus.SbPdfPrimeManager;


// projname: $projectname$
// itemname: SbMainMgr
// username: jeffs
// created:  5/26/2025 5:18:26 PM

namespace _TemplateProject.Menus.SbMain
{
	public class SbMainMgr
	{
		private const string PROCESS_MSG = "<dimgray>process as <red>{3}</red>|got choice <limegreen>{0}</limegreen> from manager <white>{1}</white> and menu <white>{2}</white></dimgray>";

	#region private fields

		private static readonly Lazy<SbMainMgr> instance =
			new Lazy<SbMainMgr>(() => new SbMainMgr());

		private CsFlowDocManager fdMgr;

	#endregion

	#region ctor

		private SbMainMgr()
		{
			DM.Start0();

			fdMgr = CsFlowDocManager.Instance;

			DM.End0();
		}

	#endregion

	#region public properties

		public static SbMainMgr Instance => instance.Value;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Config()
		{
			DM.Start0();
			fdMgr.RegisterMenuManager(SbPdfPrimeManager.MENU_NAME, SbPdfPrimeManager.Instance);
			fdMgr.ConfigMenuMgr(SbPdfPrimeManager.MENU_NAME);
			DM.End0();
		}

		public void Start()
		{
			DM.Start0();

			fdMgr.GetMenuSelection(SbPdfPrimeManager.MENU_NAME, SbPdfPrimeManager.SbPdfPrime.MENU_NAME, OnMenuChoice);

			DM.End0();
		}

		private void OnMenuChoice(string menuMgrName, string menuName, string choice)
		{
			DM.Start0();

			fdMgr.AddTextLineFd($"got menu choice| {choice}");


			if (menuName.Equals(SbPdfPrime.MENU_NAME))
			{
				switch (choice)
				{
				case "I1":
				case "I2":
				case "I3":
				case "I4":
					{
						processInit(choice, menuMgrName, menuName );
						break;
					}
				case "R1":
				case "R2":
				case "R3":
					{
						processReset(choice, menuMgrName, menuName );
						break;
					}
				// case "Alpha":
				case "Beta":
				case "Delta":
					{
						processPrimeButton(choice, menuMgrName, menuName );
						break;
					}
				}
			}
			else if (menuName.Equals(SbPdfRead.MENU_NAME))
			{
				switch (choice)
				{
				case "RD1":
				case "RD2":
					{
						processRead(choice, menuMgrName, menuName );
						break;
					}
				}
			}
			else if (menuName.Equals(SbPdfSelect.MENU_NAME))
			{
				switch (choice)
				{
				case "SE1":
				case "SE2":
				case "SE3":
					{
						processSelect(choice, menuMgrName, menuName );
						break;
					}
				}
			}
			else if (menuName.Equals(SbPdfScan.MENU_NAME))
			{
				switch (choice)
				{
				case "SC1":
				case "SC2":
					{
						processScan(choice, menuMgrName, menuName );
						break;
					}
				}
			}
			else if (menuName.Equals(SbPdfBtnTest.MENU_NAME))
			{
				switch (choice)
				{
				case "A":
				case "B":
					{
						processTest(choice, menuMgrName, menuName );
						break;
					}
				}
			}
			else
			{
				CsFlowDocManager.Instance.AddDescTextLineTb(string.Format(PROCESS_MSG, choice, menuMgrName, menuName, "NONE"));
			}

			DM.End0();
		}

	#endregion

	#region private methods

		private void processInit(string choice, string mgr, string mnu)
		{
			CsFlowDocManager.Instance.AddDescTextLineTb(string.Format(PROCESS_MSG, choice, mgr, mnu, "INIT"));
		}

		private void processPrimeButton(string choice, string mgr, string mnu)
		{
			CsFlowDocManager.Instance.AddDescTextLineTb(string.Format(PROCESS_MSG, choice, mgr, mnu, "PRIME BUTTON"));
		}

		private void processReset(string choice, string mgr, string mnu)
		{
			CsFlowDocManager.Instance.AddDescTextLineTb(string.Format(PROCESS_MSG, choice, mgr, mnu, "RESET"));
		}

		private void processRead(string choice, string mgr, string mnu)
		{
			CsFlowDocManager.Instance.AddDescTextLineTb(string.Format(PROCESS_MSG, choice, mgr, mnu, "READ"));
		}

		private void processSelect(string choice, string mgr, string mnu)
		{
			CsFlowDocManager.Instance.AddDescTextLineTb(string.Format(PROCESS_MSG, choice, mgr, mnu, "SELECT"));
		}

		private void processScan(string choice, string mgr, string mnu)
		{
			CsFlowDocManager.Instance.AddDescTextLineTb(string.Format(PROCESS_MSG, choice, mgr, mnu, "SCAN"));
		}

		private void processTest(string choice, string mgr, string mnu)
		{
			CsFlowDocManager.Instance.AddDescTextLineTb(string.Format(PROCESS_MSG, choice, mgr, mnu, "TEST"));
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(SbMainMgr)}";
		}

	#endregion
	}
}