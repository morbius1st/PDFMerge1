﻿#region using
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using UtilityLibrary;
using UtilityLibrary;

#endregion

// projname: WpfShared
// itemname: MainWindow
// username: jeffs
// created:  7/12/2020 12:32:07 PM

namespace WpfShared.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		#region private fields

		#endregion

		#region ctor

		public MainWindow()
		{
			InitializeComponent();

			FilePath<FileNameSimple> f = FilePath<FileNameSimple>.Invalid;
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
