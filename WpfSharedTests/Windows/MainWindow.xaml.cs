#region using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfShared.Windows;

using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

#endregion

// projname: WpfSharedTests
// itemname: MainWindow
// username: jeffs
// created:  9/10/2020 6:53:54 AM

namespace WpfSharedTests.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
	#region private fields

		private string message = "this is the initial message";

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();
		}

	#endregion

	#region public properties

		public string Message
		{
			get => message;
			set
			{
				message = value;
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

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion


		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			WpfShared.Windows.ClassificationFileSelector d = new ClassificationFileSelector();

			bool? result = d.ShowDialog();

			Message += nl + "dialog result| " + result.ToString();

			if (result == true)
			{
				Message += "\nelected is| " + (d.Selected?.FileName ?? "is null");
				Message += "\nsample file is| " + (d.Selected?.SampleFileName ?? "is null");
			}
			else
			{
				Message += "\ncanceled";
				Message += "\nnothing selected|";
			}

			Message += "\n";
		}
	}
}