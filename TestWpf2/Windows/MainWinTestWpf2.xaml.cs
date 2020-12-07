#region using

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TestWpf2.Data;

#endregion

// projname: TestWpf2
// itemname: MainWindow
// username: jeffs
// created:  12/6/2020 12:28:56 PM

namespace TestWpf2.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWinTestWpf2 : Window, INotifyPropertyChanged
	{
	#region private fields

		private static string tbxMessage = null;

		private static MainWinTestWpf2 me;

		private double pb1Value;
		private double pb1MaxValue;
		private Progress<double> pb1ProgressValue;
		private Progress<string> message;

		private IProgress<double> progressDouble;
		private IProgress<string> progressString;

	#endregion

	#region ctor

		public MainWinTestWpf2()
		{
			InitializeComponent();

			me = this;

			pb1ProgressValue = new Progress<double>(value => Pb1Value = value);
			message = new Progress<string>(value => TbxMessage += value);

			// pb1ProgressValue = new Progress<double>(value => Pb1.Value = value);
			// message = new Progress<string>(value => Tbx1.Text += value);

			progressDouble = pb1ProgressValue;
			progressString = message;

			Pb1Value = 0;
			Pb1MaxValue = 100;
		}

	#endregion

	#region public properties

		public string TbxMessage
		{
			get => tbxMessage;
			set
			{
				tbxMessage = value;
				OnPropertyChange();
			}
		}

		public double Pb1Value
		{
			get => pb1Value;
			set
			{
				pb1Value = value;
				OnPropertyChange();
			}
		}

		public double Pb1MaxValue
		{
			get => pb1MaxValue;
			set
			{
				pb1MaxValue = value;
				OnPropertyChange();
			}
		}

		public static SampleData Sd { get; set; } = new SampleData();

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

		public async void TestProgressWin(double qty)
		{
			await Task.Run(() =>
				{
					workTask(qty);
				}
				);
		}

		private void workTask(double qty)
		{
			for (double i = 0; i < qty; i++)
			{
				Debug.WriteLine("work task item| " + i);

				progressDouble.Report(i+1);
				progressString.Report("working on| " + (i+1) + "\n");

				Thread.Sleep(50);
			}
		}

		private void test1()
		{
			Modify.Modify m = new Modify.Modify(me, pb1ProgressValue, 
				message);

			Pb1Value = 0;
			Pb1MaxValue = 20;

			m.TestProgress(Pb1MaxValue);
		}

		private void test2()
		{
			Pb1Value = 0;
			Pb1MaxValue = 20;

			TestProgressWin(Pb1MaxValue);
		}

	#endregion

	#region event consuming

		private void BtnTestProgress_OnClick(object sender, RoutedEventArgs e)
		{
			test1();

			// test2();

		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@ Debug");
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