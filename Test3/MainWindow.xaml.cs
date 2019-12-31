using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test3
{

	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public static bool ShowEventMesssage = false;

		public static string nl = System.Environment.NewLine;

		public enum SelectStatus
		{
			UNCHECK,
			CHECK,
			PARTIAL,
			UNCHECK_WAS_CHECK,
			UNCHECK_WAS_PARTIAL
		}

		public const int MAX = 12;


//		public static string nlx { get; } = System.Environment.NewLine;

		public static MainWindow Instance { get; private set; }
		public static MainWindow MainWin => Instance;

		private List<EventTest2> ev2 = new List<EventTest2>();

		public EventTestMgr evMgr { get; private set; }

		public MainWindow()
		{
			InitializeComponent();

			Instance = this;

			MakeEventTests(MAX);

			SetEventTests();

			
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnTest1_OnClick(object sender, RoutedEventArgs e)
		{
			TriStateTest(SelectStatus.CHECK);

			OnTriState(TriStateProperty);

			TriStateProperty = null;

			TriStateProperty += ev2[4].TriStateTestMethod;
			TriStateProperty += ev2[6].TriStateTestMethod;

		}

		private void BtnCheckOneOne_OnClick(object sender, RoutedEventArgs e)
		{
			tbk2.AppendText("********  check [1][1] ********\n");
			evMgr.CheckOneOne();
		}

		private void BtnUnCheckOneOne_OnClick(object sender, RoutedEventArgs e)
		{
			tbk2.AppendText("********  uncheck [1][1] ********\n");
			evMgr.UnCheckOneOne();
		}

		private void BtnCheckOne_OnClick(object sender, RoutedEventArgs e)
		{
			tbk2.AppendText("********  check [1] ********\n");
			evMgr.CheckOne();
		}
		
		private void BtnCheckOneOneTv2_OnClick(object sender, RoutedEventArgs e)
		{
			tbk2.AppendText("********  check [2] ********\n");
			evMgr.CheckOneOne();
		}
		
		private void BtnUnCheckOneOneTv2_OnClick(object sender, RoutedEventArgs e)
		{
			tbk2.AppendText("********  uncheck [2] ********\n");
			evMgr.UnCheckOneOne();
		}

		private void BtnResetTree_OnClick(object sender, RoutedEventArgs e)
		{
			tbk2.Clear();
			tbk2.AppendText("********  tree reset ********\n");
			evMgr.ResetTree();

			OnPropertyChange("evMgr");
		}

		private void BtnMakeTree_OnClick(object sender, RoutedEventArgs e)
		{
			tbk2.Clear();
			tbk2.AppendText("********  tree made ********\n");
			evMgr = new EventTestMgr();
			OnPropertyChange("evMgr");
		}

		private void BtnListTree_OnClick(object sender, RoutedEventArgs e)
		{
			tbk2.AppendText("********  listing reset ********\n");
			evMgr.ListTree();
		}

		private void BtnClearScreen_OnClick(object sender, RoutedEventArgs e)
		{
			tbk2.Clear();
			tbk1.Clear();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("at debug");
		}

		private void MakeEventTests(int qty)
		{
			for (int i = 0; i < qty; i++)
			{
				ev2.Add(new EventTest2("", 0));

				EventTest2 evnt2 = new EventTest2(((SelectStatus) (i % 4)).ToString(), i);
				evnt2.Status = ((SelectStatus) (i % 4));
				ev2[i] = evnt2;
			}
		}

		private void SetEventTests()
		{
			for (int i = 0; i < MAX; i++)
			{
				if (i % 3 == 0)
				{
					ev2[i].Status =
						(SelectStatus) ((int) ev2[i].Status) + 1;

					TriStateStatusProperty += ev2[i].EventTestMethod;

					TriStateProperty += ev2[i].TriStateTestMethod;
				}
			}
		}


		public void AppendLineTbk1(string msg)
		{
			AppendMessageTbk1(msg + nl);
		}

		public void AppendMessageTbk1(string msg)
		{
			tbk1.Text += msg;
		}
		
		public void AppendLineTbk2(string msg)
		{
			AppendMessageTbk2(msg + nl);
		}

		public void AppendMessageTbk2(string msg)
		{
			tbk2.Text += msg;
		}

		// create a delegate
		public delegate void TriStateType(SelectStatus status);

		// create the property / delegate holder
		public TriStateType TriStateStatusProperty { get; set; }

		// the routine to fire the event
		private void TriStateTest(SelectStatus status)
		{
			if (TriStateStatusProperty != null) TriStateStatusProperty(status);
		}


		public delegate void TriState(TriState t);

		public TriState TriStateProperty { get; set; }

		private void OnTriState(TriState t)
		{
			if (TriStateProperty != null) TriStateProperty(t);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		private void CheckBox_LostFocus(object sender, RoutedEventArgs e)
		{
			CheckBox cbx = (CheckBox) sender;

			EventTest4 et4 = (EventTest4) cbx.DataContext;

			et4.TriStateReset();
		}

		private void CheckBox_LostFocus5(object sender, RoutedEventArgs e)
		{
			CheckBox cbx = (CheckBox) sender;

			TreeNode5 et = (TreeNode5) cbx.DataContext;

			et.TriStateReset();
		}
	}
}
