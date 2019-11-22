#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AODeliverable.FileSelection;

#endregion


// projname: AODeliverable.OutlineSupport
// itemname: OutlineMgr
// username: jeffs
// created:  11/3/2019 1:06:16 PM


namespace AODeliverable.OutlineSupport
{
	// manage the names of the outlines versus
	// the folder names
	public class OutlineMgr
	{
		private OutlineMgr()
		{
			fileMgr = SelectFilesMgr.Instance;
			os = new OutlineSettings();
		}

		private static OutlineMgr instance = null;

		private SelectFilesMgr fileMgr;
		private OutlineSettings os;

		public static OutlineMgr Instance
		{
			get
			{
				if (instance == null) instance = new OutlineMgr();

				return instance;
			}
		}

		public void AdjustOutlinePaths()
		{
			for (int i = 0; i < fileMgr.ItemCount; i++)
			{
				FileItem fi = fileMgr[i];

				fi = os.AdjustOutlineByFilter(fi);
			}
		}

	}
}
