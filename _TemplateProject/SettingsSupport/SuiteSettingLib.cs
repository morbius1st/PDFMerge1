using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettingsManager;
using UtilityLibrary;


// user name: jeffs
// created:   3/23/2025 7:56:55 AM

namespace _TemplateProject.SettingsSupport
{
	public class SuiteSettingLib
	{
		// to access suite settings
		// include SuiteSettings.cs in the project in the folder Settings
		// include a link to SettingsManager.cs in the project
		// add the symbol "SUITE_SETTINGS" to the project

		private SettingsMgr<SuiteSettingPath, SuiteSettingInfo<SuiteSettingDataFile>, SuiteSettingDataFile> sus;

		public SuiteSettingLib()
		{
			DM.Start0();

			init();

			DM.End0();
		}

		public void init()
		{
			DM.Start0();

			CsFlowDocManager.Instance.AddDescTextLineTb($"<green>{nameof(SuiteSettingLib)} initialized</green>");

			sus = SuiteSettings.Admin;

			DM.End0();
		}
		
	#region public methods

	#endregion


	#region system overrides

		public override string ToString()
		{
			return $"I am {nameof(SuiteSettingLib)}";
		}

	#endregion
	}
}
