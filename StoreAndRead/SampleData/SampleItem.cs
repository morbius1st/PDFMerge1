#region using directives

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion


// projname: $projectname$
// itemname: SampleItem
// username: jeffs
// created:  4/11/2020 9:51:32 AM

namespace StoreAndRead.SampleData
{
	public class SampleItem : INotifyPropertyChanged
	{
	#region private fields

		private ObservableCollection<SampleItem> leaves = null;
		private string name;
		private string data;

	#endregion

	#region ctor

		public SampleItem(string name, string data)
		{
			this.name = name;
			this.data = data;
		}

	#endregion

	#region public properties

		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChange();
			}
		}

		public string Data
		{
			get => data;
			set
			{
				data = value;
				OnPropertyChange();
			}
		}

		public ObservableCollection<SampleItem> Leaves
		{
			get => leaves;
			set
			{ 
				leaves = value;
				OnPropertyChange();
			}
		}

		public bool IsBranch
		{
			get => leaves != null;

		}


	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleItem";
		}

	#endregion
	}
}