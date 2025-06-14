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
	public class ProcessManager
	{
	#region fields

		private ITblkFmt tw;

		private SuiteSettingLib ssl;

	#endregion

	#region ctor

		public ProcessManager(ITblkFmt tw)
		{
			this.tw = tw;

			init();
		}

		private void init()
		{
			DM.Start0();

			ssl = new SuiteSettingLib(tw);

			tw.TblkFmtdLine($"</green>{nameof(ProcessManager)} initialized</green>", SHOW_WHICH.SW_TBLK1);

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
			return $"this is {nameof(ProcessManager)}";
		}

	#endregion

	}
}