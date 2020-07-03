using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AndyShared.Support
{
	public static class ObservableCollectionSupport
	{
		public static T Find<T>(this ObservableCollection<T> collection, string search)
			where T : IObservCollMember
		{
			foreach (T item in collection)
			{
				if (item.Key.Equals(search)) return item;
			}

			return default;
		}
	}
}
