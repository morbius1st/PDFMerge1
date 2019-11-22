#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests2.Settings;

#endregion


// projname: Tests2.OutlineManager
// itemname: OutlineMgr
// username: jeffs
// created:  11/20/2019 10:02:50 PM


namespace Tests2.OutlineManager
{
	public class OutlineMgr
	{
		private OutlineItems oi;

		public void Initalize()
		{
			oi = new OutlineItems();

			Configure();
		}

		private void Configure()
		{
			Debug.WriteLine("@outline mgr| " + UserSettings.Data.OutlineItems[0].Description + " :: " +
				UserSettings.Data.OutlineItems[0].Title + " :: " + UserSettings.Data.OutlineItems[0].Pattern);

			Debug.WriteLine("@outline mgr| " + UserSettings.Data.OutlineItems[1].Description + " :: " +
				UserSettings.Data.OutlineItems[1].Title + " :: " + UserSettings.Data.OutlineItems[1].Pattern);

			Debug.WriteLine("@outline mgr| " + UserSettings.Data.OutlineItems[2].Description + " :: " +
				UserSettings.Data.OutlineItems[2].Title + " :: " + UserSettings.Data.OutlineItems[2].Pattern);

			UserSettings.Admin.Save();
		}
	}
}
