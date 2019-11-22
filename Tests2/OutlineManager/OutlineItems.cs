#region + Using Directives

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

#endregion


// projname: Tests
// itemname: OutlineItems
// username: jeffs
// created:  11/6/2019 10:13:37 PM


namespace Tests2.OutlineManager
{
	public class OutlineItems : INotifyPropertyChanged
	{
		private ObservableCollection<OutlineItem> outlineItems  { get; set; }

		public ICollectionView Vue { get; set; }

		public OutlineItems() { }

		public void Add(List<OutlineItem> items)
		{
			outlineItems = new ObservableCollection<OutlineItem>(items);
		}

		public void Sort()
		{
			Vue = CollectionViewSource.GetDefaultView(outlineItems);
			Vue.SortDescriptions.Add(new SortDescription("SequenceCodeString", ListSortDirection.Ascending));
			
			OnPropertyChange("Vue");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
