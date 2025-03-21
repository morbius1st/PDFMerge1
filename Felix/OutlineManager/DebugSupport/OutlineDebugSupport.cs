﻿#region + Using Directives

using System.Collections.Generic;
using Felix.Settings;
using Felix.Windows;

#endregion


// projname: Tests2.OutlineManager.DebugSupport
// itemname: OutlineDebugSupport
// username: jeffs
// created:  11/24/2019 9:48:15 AM


namespace Felix.OutlineManager.DebugSupport
{
	public class OutlineDebugSupport
	{
		public void AddDebugData(OutlineItems oi)
		{
			oi.Add(
				new List<OutlineItem>()
				{
					new OutlineItem("07002", "A A2", "072 Architectural\\Plans",
						"Architectural Plan Sheets",  
						true),
					new OutlineItem("070", "A A", "070 Architectural",
						"All Architectural Sheets",  
						true),
					new OutlineItem("0700201", "A A2.1", "072 Architectural\\Plans\\Floor Plans",
						"Architectural Plan Sheets",  
						true)
				}
				);

			oi.Sort();
		}

		public void ListAppOutlineItems()
		{
			MainWinManager.MessageAppendLine("");

			MainWinManager.MessageAppendLine("@outline mgr| app element count| "
				+ AppSettings.Data.DefaultOutlineItems.Count);

			foreach (OutlineItem otl in AppSettings.Data.DefaultOutlineItems)
			{
				MainWinManager.MessageAppendLine("@outline mgr| " + otl.Pattern + " :: "
					+ otl.OutlinePath + " :: " +  otl.Description);
			}
		}

		public void ListUserOutlineItems()
		{
			MainWinManager.MessageAppendLine("");

			MainWinManager.MessageAppendLine("@outline mgr| user element count| " + UserSettings.Data.OutlineItems.Count);

			foreach (OutlineItem otl in UserSettings.Data.OutlineItems)
			{
				MainWinManager.MessageAppendLine("@outline mgr| " + otl.Pattern + " :: "
					+ otl.OutlinePath + " :: " +  otl.Description);
			}
		}

		private void ListOutlineItems(OutlineItems oi)
		{
			MainWinManager.MessageAppendLine("");

			MainWinManager.MessageAppendLine("@outline mgr| outline count| " + oi);

			foreach (OutlineItem otl in oi)
			{
				MainWinManager.MessageAppendLine("@outline mgr| " + otl.Pattern + " :: "
					+ otl.OutlinePath + " :: " +  otl.Description);
			}
		}

		public void ListOutlineItemsVue(OutlineItems oi)
		{
			MainWinManager.MessageAppendLine("");

			MainWinManager.MessageAppendLine("@outline mgr| outline count| " + oi.Count);

			foreach (OutlineItem otl in oi.Vue)
			{
				MainWinManager.MessageAppendLine("@outline mgr| " + otl.Pattern + " :: "
					+ otl.OutlinePath + " :: " +  otl.Description);
			}
		}
	}
}
