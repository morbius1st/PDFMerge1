#region + Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Data;
using Tests2.Settings;
using Tests2.Windows;

#endregion


// projname: Tests
// itemname: OutlineItems
// username: jeffs
// created:  11/6/2019 10:13:37 PM


namespace Tests2.OutlineManager
{
	public class OutlineItems : INotifyPropertyChanged, IEnumerable<OutlineItem>
	{
		private ObservableCollection<OutlineItem> outlineItems  { get; set; }

		public OutlineItems()
		{
			outlineItems = new ObservableCollection<OutlineItem>();
		}

		public int Count => outlineItems.Count;

		public ICollectionView Vue { get; set; }

		public void Add(List<OutlineItem> items)
		{
			if (items == null || items.Count == 0) return;

			foreach (OutlineItem item in items)
			{
				string temp = ValidateOutlineItem(item);

				// skip all failures
				if (temp != null)
				{
					item.OutlinePath = temp;

					outlineItems.Add(item);
				}
			}
		}

		public void Sort()
		{
			Vue = CollectionViewSource.GetDefaultView(outlineItems);
			Vue.SortDescriptions.Add(new SortDescription("SequenceCode", ListSortDirection.Descending));
			
			OnPropertyChange("Vue");
		}

		private string ValidateOutlineItem(OutlineItem oi)
		{

			Regex rx = new Regex(@"^\\w*");

			if (rx.IsMatch(oi.OutlinePath)) return oi.OutlinePath;

			rx = new Regex(@"^\w*");

			if (rx.IsMatch(oi.OutlinePath)) return "\\" + oi.OutlinePath;

			rx = new Regex(@"^\\\w*");

			if (rx.IsMatch(oi.OutlinePath))
			{
				return oi.OutlinePath.Substring(1);
			}

			return null;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public IEnumerator<OutlineItem> GetEnumerator()
		{
			return outlineItems.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}
}
