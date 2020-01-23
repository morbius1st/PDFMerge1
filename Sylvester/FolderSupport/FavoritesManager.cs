#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



#endregion


// projname: Sylvester.FolderSupport
// itemname: FavoritesManager
// username: jeffs
// created:  1/20/2020 8:55:27 PM


namespace Sylvester.FolderSupport
{
	public class FavoritesManager
	{
		private int index;

		public FavoritesManager(int index)
		{
			this.index = index;
		}

		public bool HasFavorites { get; private set; } = false;

	}
}
