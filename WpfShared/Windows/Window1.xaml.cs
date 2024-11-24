using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using JetBrains.Annotations;
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
		private bool popupIsOpen;
		private static ObservableCollection<string> cbxItems;

		static Window1()
		{
			addItems();
		}

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
				OnPropertyChanged();
			}
		}

		public string Favorite
		{
			get => eventStatus[(int) EventStat.Favorite];
			set
			{
				eventStatus[(int) EventStat.Favorite] = value;
				OnPropertyChanged();
			}
		}

		public string History
		{
			get => eventStatus[(int) EventStat.History];
			set
			{
				eventStatus[(int) EventStat.History] = value;
				OnPropertyChanged();
			}
		}

		public string SelectFldr
		{
			get => eventStatus[(int) EventStat.SelectFldr];
			set
			{
				eventStatus[(int) EventStat.SelectFldr] = value;
				OnPropertyChanged();
			}
		}

		public string PathChg
		{
			get => eventStatus[(int) EventStat.PathChg];
			set
			{
				eventStatus[(int) EventStat.PathChg] = value;
				OnPropertyChanged();
			}
		}

		public static ObservableCollection<string> CbxItems
		{
			get => cbxItems;
			set
			{
				if (Equals(value, cbxItems)) return;
				cbxItems = value;

			}
		}

		private static void addItems()
		{
			cbxItems = new ObservableCollection<string>();

			cbxItems.Add("Item 1a");
			cbxItems.Add("Item 2b");
			cbxItems.Add("Item 3c");
			cbxItems.Add("Item 4d");
			cbxItems.Add("Item 5e");

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
			OnPropertyChanged(e);

			Timer t = new Timer(ClearEventMessage, idx, 500, 0);
		}


		private void ClearEventMessage(object state)
		{
			EventStat idx = (EventStat) state;
			string e = idx.ToString();

			eventStatus[(int) idx] = "";

			OnPropertyChanged(e);

		}

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public bool PopupIsOpen
		{
			get => popupIsOpen;
			set
			{
				if (value == popupIsOpen) return;
				popupIsOpen = value;
				OnPropertyChanged();
			}
		}

		private void Cbx2_OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			Debug.WriteLine("combobox | mouse up");

			PopupIsOpen = true;
		}

		private void Popup_OnClosed(object sender, EventArgs e)
		{
			Debug.WriteLine("combobox | popup is closed");

			PopupIsOpen = false;
		}

		// private void Cbx2_OnLostFocus(object sender, RoutedEventArgs e)
		// {
		// 	Debug.WriteLine("combobox | lost focus");
		//
		// 	PopupIsOpen = false;
		// }
		// private void Cbx2_OnMouseLeave(object sender, MouseEventArgs e)
		// {
		// 		Debug.WriteLine("combobox | mouse leave");
		// 	
		// 		PopupIsOpen = false;
		// }
	}
}
