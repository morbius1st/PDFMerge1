#region using

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using JetBrains.Annotations;
using SuiteInfoEditor.Support;
using UtilityLibrary;

#endregion

// projname: SuiteInfoEditor
// itemname: MainWindow
// username: jeffs
// created:  11/28/2024 1:00:00 PM

namespace SuiteInfoEditor.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class WmSuiteInfo : Window, INotifyPropertyChanged, ITblkFmt, IWin
	{
		private string messages;

	#region private fields

		private Operations1 op1;

	#endregion

	#region ctor

		public WmSuiteInfo()
		{
			InitializeComponent();

			op1 = new Operations1(this);
			Identifiers.Instance.Init(this);

		}

	#endregion

	#region public properties

		public string Messages
		{
			get => messages;
			set
			{
				if (value == messages) return;
				messages = value;
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
			Messages += msg + "\n";
		}
		public void DebugMsg(string msg)
		{
			Messages += msg;
		}

		public void TblkFmtdLine(string msg)
		{
			DebugMsgLine(msg);
		}
		public void TblkFmtd(string msg)
		{
			DebugMsg(msg);
		}
		public void TblkMsgClear()
		{
			Messages = null;
		}

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

		private void BtnLocations_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsManager.FileLocationSupport.ShowLocations(this);
		}

		private void BtnCompConsts_OnClick(object sender, RoutedEventArgs e)
		{
			Identifiers.Instance.showShtNumComponentConstants1();
		}
		
		private void BtnCompConsts2_OnClick(object sender, RoutedEventArgs e)
		{
			Identifiers.Instance.showShtNumComponentConstants2();
		}
		
		private void BtnCompNameInfo1_OnClick(object sender, RoutedEventArgs e)
		{
			Identifiers.Instance.ShowCompNameInfo1();
		}

		private void BtnSheetNumComponentData_OnClick(object sender, RoutedEventArgs e)
		{
			Identifiers.Instance.ShowSheetNumComponentData();
		}

	}
}