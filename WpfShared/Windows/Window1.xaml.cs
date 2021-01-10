using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
using System.Windows.Shapes;

namespace WpfShared.Windows
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window, INotifyPropertyChanged
	{
		private int idx = 0;
		private bool clicked = true;

		private string message;

		public Window1()
		{
			InitializeComponent();

			flipCkicked("None");
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
#pragma warning disable CS1061 // 'FolderRoute' does not contain a definition for 'AddPathButton' and no accessible extension method 'AddPathButton' accepting a first argument of type 'FolderRoute' could be found (are you missing a using directive or an assembly reference?)
			folderRoute.AddPathButton("Path| " + idx++);
#pragma warning restore CS1061 // 'FolderRoute' does not contain a definition for 'AddPathButton' and no accessible extension method 'AddPathButton' accepting a first argument of type 'FolderRoute' could be found (are you missing a using directive or an assembly reference?)
		}

		private void BtnClr_OnClick(object sender, RoutedEventArgs e)
		{

#pragma warning disable CS1061 // 'FolderRoute' does not contain a definition for 'ClearPath' and no accessible extension method 'ClearPath' accepting a first argument of type 'FolderRoute' could be found (are you missing a using directive or an assembly reference?)
			folderRoute.ClearPath();
#pragma warning restore CS1061 // 'FolderRoute' does not contain a definition for 'ClearPath' and no accessible extension method 'ClearPath' accepting a first argument of type 'FolderRoute' could be found (are you missing a using directive or an assembly reference?)
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

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


		
	}
}
