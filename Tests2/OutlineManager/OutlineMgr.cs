#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests2.Settings;
using Tests2.Windows;

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
			MainWinManager.MessageClear();

//			MainWinManager.MessageAppendLine("@outline mgr| initial list");

//			ListAppOutlineItems();
			LoadOutlineItems();
//			ListUserOutlineItems();

			AddDebugData();
//			SaveUserOutlineItems();
//			ListUserOutlineItems();
			ListOutlineItemsVue();
		}

		private void SaveUserOutlineItems()
		{
			UserSettings.Data.OutlineItems.Clear();

			foreach (OutlineItem oli in oi)
			{
				UserSettings.Data.OutlineItems.Add(oli.Clone());
			}

			UserSettings.Admin.Save();
		}

		private void LoadOutlineItems()
		{
			if (UserSettings.Data.OutlineItems.Count == 0) ReadAppOutlineItems();

			oi = new OutlineItems();

			oi.Add(UserSettings.Data.OutlineItems);

			oi.Sort();

		}

		private void ReadAppOutlineItems()
		{
			foreach (OutlineItem otl in AppSettings.Data.DefaultOutlineItems)
			{
				UserSettings.Data.OutlineItems.Add(otl.Clone());
			}
			UserSettings.Admin.Save();
		}


		private void AddDebugData()
		{
			oi.Add(
				new List<OutlineItem>()
				{
					new OutlineItem("07002", "A A2", "072 Architectural - Plans",
						"Architectural Plan Sheets",  
						true),
					new OutlineItem("070", "A A", "070 Architectural",
						"All Architectural Sheets",  
						true),
					new OutlineItem("0700201", "A A2.1", "072 Architectural - Floor Plans",
						"Architectural Plan Sheets",  
						true)
				}
				);

			oi.Sort();
		}


		private void ListAppOutlineItems()
		{
			MainWinManager.MessageAppendLine("");

			MainWinManager.MessageAppendLine("@outline mgr| app element count| " + AppSettings.Data.DefaultOutlineItems.Count);

			foreach (OutlineItem otl in AppSettings.Data.DefaultOutlineItems)
			{
				MainWinManager.MessageAppendLine("@outline mgr| " + otl.Pattern + " :: "
					+ otl.Title + " :: " +  otl.Description);
			}
		}

		private void ListUserOutlineItems()
		{
			MainWinManager.MessageAppendLine("");

			MainWinManager.MessageAppendLine("@outline mgr| user element count| " + UserSettings.Data.OutlineItems.Count);

			foreach (OutlineItem otl in UserSettings.Data.OutlineItems)
			{
				MainWinManager.MessageAppendLine("@outline mgr| " + otl.Pattern + " :: "
					+ otl.Title + " :: " +  otl.Description);
			}
		}

		private void ListOutlineItems()
		{
			MainWinManager.MessageAppendLine("");

			MainWinManager.MessageAppendLine("@outline mgr| outline count| " + oi.Count);

			foreach (OutlineItem otl in oi)
			{
				MainWinManager.MessageAppendLine("@outline mgr| " + otl.Pattern + " :: "
					+ otl.Title + " :: " +  otl.Description);
			}
		}

		private void ListOutlineItemsVue()
		{
			MainWinManager.MessageAppendLine("");

			MainWinManager.MessageAppendLine("@outline mgr| outline count| " + oi.Count);

			foreach (OutlineItem otl in oi.Vue)
			{
				MainWinManager.MessageAppendLine("@outline mgr| " + otl.Pattern + " :: "
					+ otl.Title + " :: " +  otl.Description);
			}
		}
	}
}
