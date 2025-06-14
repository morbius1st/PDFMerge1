using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AAndyLibrary.SettingsSupport;
using UtilityLibrary;

// username: jeffs
// created:  3/23/2025 9:11:15 AM

namespace AAndyLibrary.Windows.Support
{
	public class MwSetgProcessManager
	{

	#region fields

		private IFdFmt tw;

		private SuiteSettingLib ssl;

	#endregion

	#region ctor

		public MwSetgProcessManager(IFdFmt tw)
		{
			this.tw = tw;

			init();
		}

		private void init()
		{
			DM.Start0();

			ssl = new SuiteSettingLib(tw);

			tw.TblkFmtdLine($"<green>{nameof(MwSetgProcessManager)} initialized</green>");

			DM.End0();
		}

	#endregion






	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(MwSetgProcessManager)}";
		}

	#endregion

	}
}