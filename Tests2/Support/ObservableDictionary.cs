#region + Using Directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests2.PDFMergeManager;

#endregion


// projname: Tests2.Support
// itemname: ObservableDictionary
// username: jeffs
// created:  12/9/2019 8:56:37 PM


namespace Tests2.Support
{
//	public class ObservableDictionary<T1, T2> : ObservableCollection<KeyValuePair<T1, T2>> , IList<KeyValuePair<T1, T2>>
	public class ObservableDictionary<T1, T2> : IList<KeyValuePair<T1, T2>>
	{
		private ObservableCollection<KeyValuePair<T1, T2>> list;

		public ObservableDictionary()
		{
			list = new ObservableCollection<KeyValuePair<T1, T2>>();
		}

		public ObservableDictionary(IEnumerable<KeyValuePair<T1, T2>> collection)
		{
			list = new ObservableCollection<KeyValuePair<T1, T2>>(collection);
		}

		public new void Add(KeyValuePair<T1, T2> item)
		{
			list.Add(item);
		}

		public void Clear()
		{
			list.Clear();
		}
		public bool Contains(KeyValuePair<T1, T2> item)
		{
			return list.Contains(item);
		}

		public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex)
		{
			list.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<T1, T2> item)
		{
			return list.Remove(item);
		}

		public int Count => list.Count;

		public bool IsReadOnly => false;

		public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		void ICollection<KeyValuePair<T1, T2>>.Add(KeyValuePair<T1, T2> item)
		{
			Add(item);
		}

		public int IndexOf(KeyValuePair<T1, T2> item)
		{
			return list.IndexOf(item);
		}

		public void Insert(int index, KeyValuePair<T1, T2> item)
		{
			list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			list.RemoveAt(index);
		}

		public KeyValuePair<T1, T2> this[int index]
		{
			get => list[index];
			set => list[index] = value;
		}
	}

}
