#region using

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

		private static string tbxMessage = null;
		private static MainWinTestWpf1 me;

	#endregion

	#region ctor

		public MainWinTestWpf1()
		{
			InitializeComponent();

			me = this;
		}

	#endregion

	#region public properties

		public string TbxMessage => tbxMessage;

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

		public static void SetMsg(string msg)
		{
			tbxMessage = msg;
			OnPropertyChangeStatic("TbxMessage");
		}

		public static void AppendMsg(string msg)
		{
			tbxMessage += msg;
			OnPropertyChangeStatic("TbxMessage");
		}
		
		public static void AppendMsgLine(string msg)
		{
			tbxMessage += msg + "\n";
			OnPropertyChangeStatic("TbxMessage");
		}


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
						
		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			SampleData sd = Sd;

			Debug.WriteLine("@debug");
		}

		
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

		public static event PropertyChangedEventHandler PropertyChangedStatic;

		private static void OnPropertyChangeStatic([CallerMemberName] string memberName = "")
		{
			PropertyChangedStatic?.Invoke(me, new PropertyChangedEventArgs(memberName));
		}

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