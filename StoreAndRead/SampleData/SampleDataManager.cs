#region using directives

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

// projname: $projectname$
// itemname: SampleDataManager
// username: jeffs
// created:  4/11/2020 10:08:15 AM

namespace StoreAndRead.SampleData
{
	public class SampleDataManager : INotifyPropertyChanged
	{
	#region private fields

		private static SampleData sd;

		private ObservableCollection<SampleItem> root;


	#endregion

	#region ctor

		public SampleDataManager(string dataName)
		{
			sd = new SampleData(dataName);

			Root = sd.LoadSampleData();

		}

	#endregion

	#region public properties

		public ObservableCollection<SampleItem> Root
		{
			get => root;
			private set
			{
				root = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event processing

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleDataManager";
		}

	#endregion
	}
}