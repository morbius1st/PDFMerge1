using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using StoreAndRead.DataMgr;
using StoreAndRead.SampleData;
using UtilityLibrary;



// itemname: MainWindow
// username: jeffs
// created:  4/11/2020 6:23:30 AM

namespace StoreAndRead.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
	#region private fields

		private const string DATA_NAME1 = "(SD1)";
		private const string DATA_NAME2 = "(SD2)";

		public static SampleDataManager sd1x { get; set; } = new SampleDataManager(DATA_NAME1);
		public static SampleDataManager sd2x { get; set; } = new SampleDataManager(DATA_NAME1);
		private SampleDataManager sd1;

		private string file1;
		private string file2;

		private DataManager1 dm1;
		private DataManager2 dm2;


	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			SD1 = new SampleDataManager(DATA_NAME1);
//			SD2 = new SampleDataManager2(DATA_NAME2);

			file1 = CsUtilities.AssemblyDirectory + "\\file1.xml";
			file2 = CsUtilities.AssemblyDirectory + "\\file2.xml";
		}

	#endregion

	#region public properties

		public SampleDataManager SD1
		{
			get => sd1;
			private set
			{
				sd1 = value;
				OnPropertyChange();
			}
		}
		

		public SampleDataManager SD2
		{
			get => sd1;
			private set
			{
				sd1 = value;
				OnPropertyChange();
			}
		}

		public DataManager1 Dm1
		{
			get => dm1;
			private set
			{
				dm1 = value;
				OnPropertyChange();
			}
		}
		
		public DataManager2 Dm2
		{
			get => dm2;
			private set
			{
				dm2 = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region control events

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{

			ObservableCollection<SampleItem> r = sd1.Root;

			SampleItem si = r[0];

			si.Name = "Item 0 New Name";

			si = r[1];
			si = r[2];
			si = r[3];



			Debug.WriteLine("At Button Click");
		}


	#endregion

	#region event processing

		private void mainWin_Loaded(object sender, RoutedEventArgs e)
		{
			Dm1 = new DataManager1();

			dm1.Configure(@"B:\Programming\VisualStudioProjects\StoreAndRead\StoreAndRead",
				"DataManager1.xml"
				);

			dm1.Read();
//			dm1.LoadSampleData();
//			dm1.Write();

			Dm2 = new DataManager2();

			dm2.Configure(@"B:\Programming\VisualStudioProjects\StoreAndRead\StoreAndRead",
				"DataManager2.xml"
				);

//			dm2.LoadSampleData();
//			dm2.Write();

			dm2.Read();


		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


		#endregion

	}
}

