#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

#endregion


// projname: Tests.Sequence
// itemname: SeqItems
// username: jeffs
// created:  11/8/2019 7:23:53 PM


namespace Tests.Sequence
{
	public class SeqItems : INotifyPropertyChanged
	{
		private static SeqItems _instance;

		public SeqItems()
		{
			Items = new Dictionary<string, SeqItem>();

			TestData();
		}

		public void TestData()
		{
			SeqPart sp;
			List<SeqPart> Parts;
			SeqItem si;


			Parts = new List<SeqPart>(2);

			sp = new SeqPart("A1", "Title A1", "desc A1");
			sp.Test = "John";
			Parts.Add(sp);

			sp = new SeqPart("A2", "Title A2", "desc A2");
			sp.Test = "Jane";
			Parts.Add(sp);

			si = new SeqItem("test");
			si.AddItem(Parts);

			Items.Add("k1", si);

			Parts = new List<SeqPart>(2);

			sp = new SeqPart("B1", "Title B1", "desc B1");
			sp.Test = "Chuck";
			Parts.Add(sp);

			sp = new SeqPart("B2", "Title B2", "desc B2");
			sp.Test = "Bob";
			Parts.Add(sp);

			si = new SeqItem("test");
			si.AddItem(Parts);

			Items.Add("k2", si);

			OnPropertyChange("Items");
		}

		public static SeqItems Instance
		{
			get
			{
				if (_instance == null) _instance = new SeqItems();

				return _instance;
			}
		}

		public Dictionary<string, SeqItem> Items { get; private set; }

		public void AddItem(SeqItem item)
		{
			Items.Add(item.MakeID(), item);

			OnPropertyChange("Items");
		}

		public void Clear()
		{
			Items.Clear();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}