using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AndyShared.Support
{
	public static class ObservableCollectionSupport
	{
		public static T Find<T>(this ObservableCollection<T> collection, string search, int elements = Int32.MaxValue)
			where T : IObservCollMember
		{
			int count = elements < collection.Count ? elements : collection.Count;

			for (var i = 0; i < collection.Count; i++)
			{
				T item = collection[i];

				if (item.Key.Equals(search)) return item;
			}

			return default;
		}
	}
}
