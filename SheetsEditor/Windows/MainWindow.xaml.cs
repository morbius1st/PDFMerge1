#region using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

#endregion

// projname: SheetsEditor
// itemname: MainWindow
// username: jeffs
// created:  11/23/2024 3:58:37 PM

namespace SheetsEditor.Windows
{



	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public const string DATA_FILE_FOLDER = @"C:\Users\jeffs\Documents\Programming\VisualStudioProjects\PDF SOLUTIONS\_Samples\";
		public const string SHEET_DATA_FILE  = @"SheetData.xml";

	#region private fields

	#endregion

	#region ctor

		public MainWindow()
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

	#region event consuming

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

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
	}
}