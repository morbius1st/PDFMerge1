using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using _TemplateProject.SettingsSupport;
using UtilityLibrary;

// username: jeffs
// created:  3/23/2025 9:11:15 AM

namespace _TemplateProject.Windows.Support
{
	public class MwProcessManager
	{
	#region fields

		private SuiteSettingLib ssl;

	#endregion

	#region ctor

		public MwProcessManager()
		{
			init();
		}

		private void init()
		{
			DM.Start0();

			ssl = new SuiteSettingLib();

			CsFlowDocManager.Instance.AddDescTextTb($"<green>{nameof(MwProcessManager)} initialized</green>");

			DM.End0();
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

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
			return $"this is {nameof(MwProcessManager)}";
		}

	#endregion
	}
}