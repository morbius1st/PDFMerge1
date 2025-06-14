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
	public class MwProcessManager
	{

	#region fields

		private IFdFmt tw;

		private MwSettings mws;

		#endregion

		#region ctor

		public MwProcessManager(IFdFmt tw)
		{
			DM.Start0();
			this.tw = tw;

			init();

			DM.End0();
		}

		private void init()
		{
			DM.Start0();

			tw.TblkFmtdLine($"<green>{nameof(MwProcessManager)} initialized</green>");

			DM.End0();
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void ShowSettings()
		{
			DM.Start0();

			tw.TblkFmtdLine($"<green>showing settings</green>", SHOW_WHICH.SW_TBLK1);

			mws = new MwSettings(tw);
			mws.Show();

			DM.End0();
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
			return $"this is {nameof(MwProcessManager)}";
		}

	#endregion

	}
}