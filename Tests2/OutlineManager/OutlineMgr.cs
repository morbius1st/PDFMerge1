#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;
using Tests2.FileListManager;
using Tests2.OutlineManager.DebugSupport;
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
		private OutlineItems ois;

		private OutlineDebugSupport ods = new OutlineDebugSupport();

		public List<string> UnMatched { get; private set; }

		public bool Initalize()
		{
			ois = new OutlineItems();

			Configure();

			return true;
		}

		private bool Configure()
		{
			MainWinManager.MessageClear();

		#if DEBUG
			MainWinManager.MessageAppendLine("@outline mgr| initial list");
			ods.ListAppOutlineItems();
		#endif

			LoadOutlineItems();

		#if DEBUG
			ods.ListUserOutlineItems();
			ods.AddDebugData(ois);
		#endif

			SaveUserOutlineItems();

		#if DEBUG
			ods.ListUserOutlineItems();
			ods.ListOutlineItemsVue(ois);
		#endif

			return true;
		}

		public int ApplyOutlineSettings()
		{
			UnMatched = new List<string>();

			string found = null;

			foreach (FileItem fi in FileList.Instance.FileListItems)
			{
				found = fi.OutlinePath.FileWithoutExtension;

				if (fi.OutlinePath.HasFileName)
				{
					foreach (OutlineItem oi in ois.Vue)
					{
						if (fi.OutlinePath.FileWithoutExtension.StartsWith(oi.Pattern))
						{
							fi.OutlinePath.Prepend(oi.OutlinePath);
							found = null;
							break;
						}
					}
				}

				if (found != null)
				{
					UnMatched.Add(found);
				}
			}

			return UnMatched.Count;
		}

		private void SaveUserOutlineItems()
		{
			UserSettings.Data.OutlineItems.Clear();

			foreach (OutlineItem oli in ois)
			{
				UserSettings.Data.OutlineItems.Add(oli.Clone());
			}

			UserSettings.Admin.Save();
		}

		private void LoadOutlineItems()
		{
			if (UserSettings.Data.OutlineItems.Count == 0) ReadAppOutlineItems();

			ois = new OutlineItems();

			ois.Add(UserSettings.Data.OutlineItems);

			ois.Sort();

		}

		private void ReadAppOutlineItems()
		{
			foreach (OutlineItem otl in AppSettings.Data.DefaultOutlineItems)
			{
				UserSettings.Data.OutlineItems.Add(otl.Clone());
			}
			UserSettings.Admin.Save();
		}


	}
}
