#region using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CSLibraryIo;
using CSLibraryIo.CommonFileFolderDialog;
using UtilityLibrary;

using Microsoft.IdentityModel;

#endregion

// projname: AndyFavsAndHistory
// itemname: MainWindow
// username: jeffs
// created:  1/10/2021 6:45:04 PM

namespace AndyFavsAndHistory.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class FavsAndHistory : Window, INotifyPropertyChanged
	{
	#region private fields

		private FilePath<FileNameSimple> location;


	#endregion

	#region ctor

		public FavsAndHistory(FilePath<FileNameSimple> location)
		{
			InitializeComponent();

			this.location = location;
		}

	#endregion

	#region public properties

		

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Config(string username)
		{

		}

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
	}
}