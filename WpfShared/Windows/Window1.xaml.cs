using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UtilityLibrary;
using WpfShared.Dialogs.DialogSupport;

namespace WpfShared.Windows
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window, INotifyPropertyChanged
	{
		enum EventStat
		{
			PathChg,
			SelectFldr,
			History,
			Favorite
		}

		private string[] eventStatus = new [] {"", "", "", "" };

		private bool clicked = true;

		private string message;

		public Window1()
		{
			InitializeComponent();

			flipCkicked("None");

			FolderRoute.OnFavoritesPressed += FolderRouteOnOnFavoritesPressed;
			FolderRoute.OnHistoryPressed += FolderRouteOnOnHistoryPressed;
			FolderRoute.OnSelectFolderRequested += FolderRouteOnOnSelectFolderRequested;
			FolderRoute.OnPathChange += FolderRouteOnOnPathChange;
			// FolderRoute
		}

		public string Message
		{
			get => message;
			set
			{
				message = value;
				OnPropertyChange();
			}
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


		private void Skb01_OnClick(object sender, RoutedEventArgs e)
		{
			flipCkicked("clicked");

			Debug.WriteLine("win1 clicked");
		}

		private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
		{
			flipCkicked("clicked 1");

			Debug.WriteLine("win1 clicked1");
		}
		
		private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
		{
			flipCkicked("clicked 2");

			Debug.WriteLine("win1 clicked2");
		}

		
		private void ButtonBase_OnClick3(object sender, RoutedEventArgs e)
		{
			flipCkicked("clicked 3");

			Debug.WriteLine("win1 clicked1");
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


		private void flipCkicked(string who)
		{
			if (clicked)
			{
				clicked = false;
				Message = "not " + who;
			}
			else
			{
				clicked = true;
				Message = "is " + who;
			}
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

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


		
	}
}
