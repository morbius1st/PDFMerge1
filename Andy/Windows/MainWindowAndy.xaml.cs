#region using
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using UtilityLibrary;
using Andy.Windows.ResourceFiles.XamlResources;
#endregion

// projname: Andy
// itemname: MainWindow
// username: jeffs
// created:  12/28/2020 7:31:15 PM


namespace Andy.Windows
{


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindowAndy : Window, INotifyPropertyChanged
	{
		#region private fields

			private int btnType = 0;


		#endregion

		#region ctor

		public MainWindowAndy()
		{
			InitializeComponent();

			// FldrRoute.Path = new FilePath<FileNameSimple>(@"P:\2099-900 Sample Project\Publish\Bulletins\2017-07-00 flat");
			// FldrRoute.ProposedObliqueButtonType = btnType;


		}

		#endregion

		#region public properties

			public int BtnType
			{
				get => btnType;
				set
				{
					btnType = value;
					OnPropertyChange();

					// FldrRoute.ProposedObliqueButtonType = value;
				}
			}
		#endregion

		#region private properties

		#endregion

		#region public methods

		#endregion

		#region private methods

		#endregion

		#region event consuming

		#endregion

		#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		#endregion

		#region system overrides

		public override string ToString()
		{
			return "this is MainWindow";
		}

		#endregion

			private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
			{
				 ++BtnType;
			}
	}
}
