#region using

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TestWpf1.Adjust;
using TestWpf1.Data;

#endregion

// projname: TestWpf1
// itemname: MainWindow
// username: jeffs
// created:  11/29/2020 11:58:44 AM

namespace TestWpf1.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWinTestWpf1 : Window, INotifyPropertyChanged
	{
	#region private fields

		private int testCount;

	#endregion

	#region ctor

		public MainWinTestWpf1()
		{
			InitializeComponent();
		}

	#endregion

	#region public properties

		public int TestCount
		{
			get => testCount;
			set
			{
				testCount = value;
				OnPropertyChange();
			}
		}

		public static SampleData Sd { get; set; } = new SampleData();

		public int ExtendedCount => Sd.TreeRoot.ExtCount;

		public int ExtendedMergeCount => Sd.TreeRoot.ExtMergeCount;

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void UpdateCounts()
		{
			
			Sd.TreeRoot.exCount();
			Sd.TreeRoot.exMergeCount();

			OnPropertyChange("ExtendedCount");
			OnPropertyChange("ExtendedMergeCount");
		}

	#endregion

	#region event consuming
		
		private void BtnClear_OnClick(object sender, RoutedEventArgs e)
		{
			Sd.TreeRoot.ClrMergeItems();
			UpdateCounts();
		}
				
		private void BtnCount_OnClick(object sender, RoutedEventArgs e)
		{
			UpdateCounts();
		}

		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			AdjustMergeItems adj = new AdjustMergeItems();

			TestCount = adj.Configure(Sd);

			adj.Process();

			UpdateCounts();
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