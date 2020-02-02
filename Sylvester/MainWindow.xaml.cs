using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Sylvester.Process;
using Sylvester.Settings;

namespace Sylvester
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private bool compare = false;
		private bool go = false;

		public static Window MainWin;

		public ProcessManager pm { get; private set; } = new ProcessManager();

		public MainWindow()
		{
			InitializeComponent();

			UserSettings.Admin.Read();

			MainWin = this;
		}

	#region public properties

		public bool SetFocus
		{
			set { SetFocusComparison(); }
		}

		public bool Compare
		{
			get => compare;
			private set
			{
				compare = value;
				OnPropertyChange();
			}
		}

		public bool Go
		{
			get => go;
			private set
			{
				go = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region public methods

		public void SetFocusComparison()
		{
			lvComparison.Focus();
		}

	#endregion

	#region window events

		private void BtnTest1_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@test1");
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@debug");
		}

		private void BtnReset_OnClick(object sender, RoutedEventArgs e)
		{
			pm.Reset();

			OnPropertyChange("pm");
		}

		private void BtnReadCurrent_OnClick(object sender, RoutedEventArgs e)
		{
			if (!pm.ReadBase()) return;

			OnPropertyChange("pm");
		}

		private void BtnReadRevision_OnClick(object sender, RoutedEventArgs e)
		{
			if (!pm.ReadTest()) return;

			OnPropertyChange("pm");
		}

		private void BtnRead_OnClick(object sender, RoutedEventArgs e)
		{
			pm.Read();

			OnPropertyChange("pm");
		}

		private void BtnCompare_OnClick(object sender, RoutedEventArgs e)
		{
			Go = pm.Process();

			SetFocusComparison();

			OnPropertyChange("pm");
		}

		private void BtnGo_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@go");
			pm.RenameFiles();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void rbtn_OnClick(object sender, RoutedEventArgs e)
		{
			SetFocusComparison();
		}

		private void Mainwin_Loaded(object sender, RoutedEventArgs e)
		{
			pm = new ProcessManager(HdrBase, HdrTest);

			OnPropertyChange("pm");
		}

	#endregion

	#region events

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}
}