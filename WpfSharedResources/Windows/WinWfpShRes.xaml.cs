#region using
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

#endregion

// projname: WpfSharedResources
// itemname: MainWindow
// username: jeffs
// created:  8/8/2020 7:14:27 AM

namespace WpfSharedResources.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class WinWfpShRes : Window, INotifyPropertyChanged
	{
		#region private fields

		#endregion

		#region ctor

		public WinWfpShRes()
		{
			InitializeComponent();
		}

		#endregion

		#region public properties



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


	}
}
