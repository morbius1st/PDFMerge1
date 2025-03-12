#region using

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

using JetBrains.Annotations;
using UtilityLibrary;

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
	public partial class MainWindow : Window, INotifyPropertyChanged, IWin
	{
		private string messageBox;

		public const string DATA_FILE_FOLDER =
			@"C:\Users\jeffs\Documents\Programming\VisualStudioProjects\PDF SOLUTIONS\_Samples\";

		public const string SHEET_DATA_FILE  = @"SheetData.xml";

	#region private fields

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			init();
		}

		private void init() { }

	#endregion

	#region public properties

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

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void TblkMsgLine(string msg)
		{
			DebugMsgLine(msg);
		}
		public void TblkMsg(string msg)
		{
			DebugMsg(msg);
		}

		public void DebugMsgLine(string msg)
		{
			MessageBox += msg + "\n";
		}
		public void DebugMsg(string msg)
		{
			MessageBox += msg;
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			
		}

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
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