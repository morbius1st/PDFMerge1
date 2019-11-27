#region + Using Directives

using System.Collections.Generic;
using Felix.FileListManager;
using Felix.OutlineManager.DebugSupport;
using Felix.Settings;
using Felix.Windows;

#endregion


// projname: Tests2.OutlineManager
// itemname: OutlineMgr
// username: jeffs
// created:  11/20/2019 10:02:50 PM


namespace Felix.OutlineManager
{
	public class OutlineMgr
	{
		private OutlineItems ois;

		public List<string> UnMatched { get; private set; }

	#if DEBUG
		private OutlineDebugSupport ods = new OutlineDebugSupport();
	#endif

	#region ctor

		public static OutlineMgr Instance => new OutlineMgr();

		static OutlineMgr() { }

		private OutlineMgr()
		{
			Initialize();
		}

	#endregion

		public bool Initialized { get; private set; } = false;

		public bool Initialize()
		{
			if (Initialized) return true;

			Configure();

			Initialized = true;

			return true;
		}

		public void Reset()
		{
			Initialized = false;

			Initialize();
		}

		private bool Configure()
		{
			ois = new OutlineItems();

			MainWinManager.MessageClear();

		#if DEBUG
			MainWinManager.MessageAppendLine("@outline mgr| initial list");
			ods.ListAppOutlineItems();
		#endif

			LoadOutlineItems();

		#if DEBUG
			ods.ListUserOutlineItems();
//			ods.AddDebugData(ois);
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

			foreach (FileItem fi in FileItems.Instance)
			{
				found = fi.OutlinePath.FileWithoutExtension;

				if (fi.OutlinePath.HasFileName)
				{
					foreach (OutlineItem oi in ois.Vue)
					{
						if (fi.OutlinePath.FileWithoutExtension.StartsWith(oi.Pattern))
						{
							fi.OutlinePath.Prepend(oi.OutlinePath);
							fi.SequenceCode = oi.SequenceCode;
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
			UserSettings.Admin.Read();

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
