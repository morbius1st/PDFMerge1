#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using AndyFavsAndHistory.Windows;
using UtilityLibrary;
using WpfShared.Dialogs.DialogSupport;
using WpfShared.Windows;
using static WpfTests.Windows.MainWindow.EventStat;
using SettingsManager;

using AndyShared.Support;

// using AndyFavsAndHistory.Windows;

#endregion

// projname: WpfTests
// itemname: MainWindow
// username: jeffs
// created:  1/3/2021 7:42:14 AM


namespace WpfTests.Windows
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{

	public class HouseInfo
	{
		public string House
		{
			get;
			set;
		}

		public ObservableCollection<String> Details
		{
			get;
			set;
		}
	}




	#region private fields

		public enum EventStat
		{
			PathChg,
			SelectFldr,
			History,
			Favorite
		}

		private string[] eventStatus = new [] {"", "", "", "" };

		private string message;

		private  bool isConfigured = true;
		private string userName = "Jeffs";
		private string projNumber = "2099-900";

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			LoadHouseInfo();

			FolderRoute.OnFavoritesPressed += FolderRouteOnOnFavoritesPressed;
			FolderRoute.OnHistoryPressed += FolderRouteOnOnHistoryPressed;
			FolderRoute.OnSelectFolderRequested += FolderRouteOnOnSelectFolderRequested;
			FolderRoute.OnPathChange += FolderRouteOnOnPathChange;

			WriteLine("pgm setting| isconfigured| " + isConfigured.ToString());
			WriteLine("pgm setting|     username| " + userName);
			WriteLine("pgm setting|      projnum| " + projNumber);

			WriteLine("suite setg| root folder| " + SuiteSettings.Path.RootFolderPath);
			WriteLine("suite setg| setg folder| " + SuiteSettings.Path.SettingFolderPath);
			WriteLine("suite setg| site root path| " + SuiteSettings.Data.SiteRootPath);

			
		}

	#endregion

	#region public properties

		public string Message
		{
			get => message;
			set
			{
				message = value;
				OnPropertyChange();
			}
		}

		public void Write(string msg)
		{
			Message += msg;
		}

		public void WriteLine(string msg)
		{
			Message += msg + "\n";
		}


		public string Favorite
		{
			get => eventStatus[(int) EventStat.Favorite];
			set
			{
				eventStatus[(int) EventStat.Favorite] = value;
				OnPropertyChange();
			}
		}

		public string History
		{
			get => eventStatus[(int) EventStat.History];
			set
			{
				eventStatus[(int) EventStat.History] = value;
				OnPropertyChange();
			}
		}

		public string SelectFldr
		{
			get => eventStatus[(int) EventStat.SelectFldr];
			set
			{
				eventStatus[(int) EventStat.SelectFldr] = value;
				OnPropertyChange();
			}
		}

		public string PathChg
		{
			get => eventStatus[(int) EventStat.PathChg];
			set
			{
				eventStatus[(int) EventStat.PathChg] = value;
				OnPropertyChange();
			}
		}

		
		public string UserName
		{
			get => userName;
			set
			{
				userName = value;
				OnPropertyChange();
			}
		}

		public string ProjNumber
		{
			get => projNumber;
			set
			{
				projNumber = value;
				OnPropertyChange();
			}
		}

		private ObservableCollection<HouseInfo> houseInfos;

		public ObservableCollection<HouseInfo> HouseInfos
		{
			get => houseInfos;
			set
			{
				houseInfos = value;
				OnPropertyChange();
			}
		}


	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure(string userName, string projectNumber)
		{
			UserName = userName;
			ProjNumber = projectNumber;
		}


	#endregion

	#region private methods

		private void LoadHouseInfo()
		{
			HouseInfos = new ObservableCollection<HouseInfo>();

			HouseInfo hi = new HouseInfo();

			hi.Details = new ObservableCollection<string>();

			hi.Details.Add("sub 1.1");
			hi.Details.Add("sub 1.2");
			hi.Details.Add("sub 1.3");

			hi.House = "menu 1";

			houseInfos.Add(hi);

			hi = new HouseInfo();
			hi.Details = new ObservableCollection<string>();

			hi.Details.Add("sub 2.1");
			hi.Details.Add("sub 2.2");
			hi.Details.Add("sub 2.3");

			hi.House = "menu 2";

			houseInfos.Add(hi);

			hi = new HouseInfo();
			hi.Details = new ObservableCollection<string>();

			hi.Details.Add("sub 3.1");
			hi.Details.Add("sub 3.2");
			hi.Details.Add("sub 3.3");

			hi.House = "menu 3";

			houseInfos.Add(hi);

			OnPropertyChange("HouseInfos");
		}


		private void FolderRouteOnOnPathChange(object sender, PathChangeArgs e)
		{
			EventMessage("Changed", EventStat.PathChg);
		}

		private void FolderRouteOnOnSelectFolderRequested(object sender, EventArgs e)
		{
			EventMessage("Pressed", EventStat.SelectFldr);
		}

		private void FolderRouteOnOnHistoryPressed(object sender, EventArgs e)
		{
			EventMessage("Pressed", EventStat.History);
		}

		private void FolderRouteOnOnFavoritesPressed(object sender, EventArgs e)
		{
			EventMessage("Pressed", EventStat.Favorite);
		}

		private void EventMessage(string msg, EventStat idx)
		{
			eventStatus[(int) idx] = msg;
			string e = idx.ToString();
			OnPropertyChange(e);

			Timer t = new Timer(ClearEventMessage, idx, 500, 0);
		}


		private void ClearEventMessage(object state)
		{
			EventStat idx = (EventStat) state;
			string e = idx.ToString();

			eventStatus[(int) idx] = "";

			OnPropertyChange(e);
		}

	#endregion

	#region event consuming

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@debug");
		}

		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			SuiteSettings.Admin.Read();

			var s = 
				SuiteSettings.Admin;

			FavsAndHistory fav = new FavsAndHistory();

			fav.Configure(Environment.UserName);

			fav.ShowDialog();

		}

		private void btnAdd_OnClick(object sender, RoutedEventArgs e)
		{
			FolderRoute.SetPath(new FilePath<FileNameSimple>(
				@"P:\2099-900 Sample Project\Publish\Bulletins\2017-07-00 one level"));
		}

		private void BtnClr_OnClick(object sender, RoutedEventArgs e)
		{
			FolderRoute.ClearPath();
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

[ValueConversion(typeof(double), typeof(double))]
public class HeightWidthConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		double width = (double) values[0];
		double height = (double) values[1];

		return height - width <= 0 ? height : width;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		return null;
	}
}
}