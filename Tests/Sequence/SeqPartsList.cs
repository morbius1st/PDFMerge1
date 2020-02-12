#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

#endregion


// projname: Tests.Sequence
// itemname: SqePartsList
// username: jeffs
// created:  11/8/2019 10:37:42 PM


namespace Tests.Sequence
{
	public class SeqPartsList : INotifyPropertyChanged
	{
		public SeqPartsList()
		{
			PartsList = new Dictionary<string, SeqParts>();

			TestData();

			Vue = CollectionViewSource.GetDefaultView(PartsList);

			OnPropertyChange("Vue");
		}

		private static SeqPartsList _instance;

		public static SeqPartsList Instance
		{
			get
			{
				if ( _instance == null) _instance = new SeqPartsList();

				return _instance;
			}
		}

		private void TestData()
		{
			SeqParts parts;
			SeqPart sp;

			parts = new SeqParts("test A");

			sp = new SeqPart("A1", "titleA1", "descriptionA1");
			parts.AddPart(sp);

			sp = new SeqPart("A2", "titleA2", "descriptionA2");
			parts.AddPart(sp);

			PartsList.Add("P1", parts);


			parts = new SeqParts("test B");

			sp = new SeqPart("B1", "titleB1", "descriptionB1");
			parts.AddPart(sp);

			sp = new SeqPart("B2", "titleB2", "descriptionB2");
			parts.AddPart(sp);

			PartsList.Add("P2", parts);

			OnPropertyChange("PartsList");

		}

		public ICollectionView Vue { get; set; }

		public Dictionary<string, SeqParts> PartsList { get; set; }

		public void Add(SeqParts parts)
		{
			PartsList.Add(parts.Name, parts);
		}

		public void ClrFilter()
		{
			Vue.Filter = null;

			OnPropertyChange("Vue");
		}

		public string FilterName { get; set; } = "Architectural";

		public void SetFilter(string filterName)
		{
			FilterName = filterName;

			Vue.Filter = Filter;

			OnPropertyChange("Vue");
		}

		private bool Filter(object item)
		{
			KeyValuePair<string, SeqParts> kvp =
				(KeyValuePair < string, SeqParts > ) item;

			return (kvp.Key.Equals(FilterName));

		}

		public void Clear()
		{
			PartsList.Clear();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}



	}
}
