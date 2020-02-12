#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion


// projname: Tests.Debug
// itemname: TestCollection
// username: jeffs
// created:  11/9/2019 6:53:54 AM


namespace Tests.Debug
{
	public class TestCollection : INotifyPropertyChanged
	{
		public ObservableCollection<Tx> tItems { get; set; }

		public TestCollection()
		{
			tItems = new ObservableCollection<Tx>();

			tItems.Add(new Tx("item1a", "item1b"));
			tItems.Add(new Tx("item2a", "item2b"));
			tItems.Add(new Tx("item3a", "item3b"));
			tItems.Add(new Tx("item4a", "item4b"));
			tItems.Add(new Tx("item5a", "item5b"));
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this,  new PropertyChangedEventArgs(memberName));
		}
	}


	public class Tx : INotifyPropertyChanged
	{
		private string item1;
		private string item2;

		public Tx(string item1, string item2)
		{
			Item1 = item1;
			Item2 = item2;
		}

		public string Item1
		{
			get => item1;
			set
			{
				OnPropertyChange();
				item1 = value;
			}
		}

		public string Item2
		{
			get => item2;
			set
			{
				OnPropertyChange();
				item2 = value;
			}
		}

		
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}
}