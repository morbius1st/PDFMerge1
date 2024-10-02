using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
// using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using DebugCode;
using JetBrains.Annotations;
using Test4.SheetMgr;
using Test4.Windows;


namespace Test4
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged, IWin
	{
		// private static MainWindow mw;

		private MainWinSupport ms;
		private SheetPdfManager sm;


		private static string messageBox;

		public MainWindow()
		{
			InitializeComponent();

			DM.Iw = (IWin) this;

			init();
		}

		public MainWinSupport MainWinSupport => ms;

		public string MessageBox
		{
			get => messageBox;
			set
			{
				if (value == messageBox) return;
				messageBox = value;
				OnPropertyChanged();
			}
		}

		private void init()
		{
			bool result;

			ms = new MainWinSupport();
			sm = SheetPdfManager.Instance;

			if (ms.GetClassifFile("PdfSample 1"))
			{
				Debug.WriteLine("get classification file worked");
			}
			else
			{
				Debug.WriteLine("get classification file failed");
			}

			OnPropertyChanged(nameof(MainWinSupport));

			// if (ms.ParseSamples())
			// {
			// 	Debug.WriteLine("parse samples worked");
			// }
			// else
			// {
			// 	Debug.WriteLine("parse samples failed");
			// }
		}

		public void DebugMsg(string msg)
		{
			MessageBox += msg;
		}

		public void DebugMsgLine(string msg)
		{
			MessageBox += msg + "\n";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		// public static event PropertyChangedEventHandler PropertyChanged_S;
		//
		// [DebuggerStepThrough]
		// [NotifyPropertyChangedInvocator]
		// private static void OnPropertyChanged_S([CallerMemberName] string memberName = "")
		// {
		// 	PropertyChanged_S?.Invoke(MainWindow.mw, new PropertyChangedEventArgs(memberName));
		// }

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		
		private void btnClassifyTest_OnClick(object sender, RoutedEventArgs e)
		{
			if (ms.FilterSamples("PdfSample 4"))
			{
				Debug.WriteLine("filter worked");
				OnPropertyChanged(nameof(ms.ClassificationFile));
			}
			else
			{
				Debug.WriteLine("filter failed");
				return;
			}
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			int idx0 = 3;
			int idx1 = 3;
			int idx2 = 0;
			int idxs = 0;

			BaseOfTree b = ms.ClassificationFile.TreeBase;
			ObservableCollection<TreeNode> c0 = b.Children;
			ObservableCollection<TreeNode> c1 = c0[idx0].Children;
			ObservableCollection<TreeNode> c2 = c1[idx1].Children;

			SheetCategory s = c2[idx2].Item;
			ObservableCollection<ComparisonOperation> o = s.CompareOps;
			ComparisonOperation op = o[idxs];
			FileNameSheetIdentifiers.ShtNumComps2 nd = op.CompNameData;
			LogicalComparisonOp lcop= op.LogicalComparisonOpCode;
			ValueComparisonOp vcop = op.ValueComparisonOpCode;
			LogicalCompareOp lcp = op.LogicalCompareOp;
			ValueCompareOp Vcp = op.ValueCompareOp;

			int A = 1 + 1;
		}

    }

	// public sealed class BubbleScrollEvent : Behavior<UIElement>
	// {
	// 	protected override void OnAttached()
	// 	{
	// 		base.OnAttached();
	// 		AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
	// 	}
	//
	// 	protected override void OnDetaching()
	// 	{
	// 		AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
	// 		base.OnDetaching();
	// 	}
	//
	// 	void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	// 	{
	// 		if (!e.Handled)
	// 		{
	// 			e.Handled = true;
	// 			var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta) { RoutedEvent = UIElement.MouseWheelEvent };
	// 			AssociatedObject.RaiseEvent(e2);
	// 		}
	// 	}
	// }
}