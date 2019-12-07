#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Permissions;

#endregion


// projname: Tests2.PDFMergeManager.IndexList
// itemname: IndexedList
// username: jeffs
// created:  12/4/2019 12:07:13 AM


namespace Tests2.PDFMergeManager.IndexList
{

	public class IndexedList<T1, T2> : List<KeyValuePair<T1, T2>>
	{
		List<KeyValuePair<T1, T2>> t = new List<KeyValuePair<T1, T2>>();
		public bool ContainsKey(T1 key)
		{
			foreach (KeyValuePair<T1, T2> kvp in this)
			{
				if (kvp.Key.Equals(key)) return true;
			}



			return false;
		}

		public bool TryGetValue(T1 key, out T2 value)
		{
			

			foreach (KeyValuePair<T1, T2> kvp in this)
			{
				if (kvp.Key.Equals(key))
				{
					value = kvp.Value;
					return true;
				}
			}

			value = default(T2);

			return false;
		}



	}
}
