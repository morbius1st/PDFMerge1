using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.WindowsAPICodePack.Shell;

namespace Sylvester.CsMessageBox
{
	/// <summary>
	/// Interaction logic for CsMessageBox.xaml
	/// </summary>
	public partial class CsMessageBox : Window, INotifyPropertyChanged
	{
	#region private fields
		private string message;
		private string caption;

		private MessageBoxResult result;
	#endregion

	#region ctor

		

		public CsMessageBox(string msg, string caption = null)
		{
			initalize();

			Message = msg;
			Caption = caption;

		}

		private CsMessageBox()
		{
			initalize();
		}

		private void initalize()
		{
			InitializeComponent();
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

		public string Caption
		{
			get => caption;

			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					caption = Application.Current.MainWindow?.Title ?? "Message Box";
					return;
				}
				caption = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region public methods

		public new MessageBoxResult Show()
		{
			CsMessageBox me = this;

			SetDialogLocation(me);

			me.ShowDialog();

			return result;
		}

	#endregion

	#region private methods

		private void SetDialogLocation(CsMessageBox me)
		{
			try
			{
				double ownerTop = Application.Current.MainWindow.Top;
				double ownerLeft = Application.Current.MainWindow.Left;
				double ownerWidth = Application.Current.MainWindow.ActualWidth;
				double ownerHeight = Application.Current.MainWindow.ActualHeight;

				double top = ownerTop + (ownerHeight - me.Height) / 2;
				double left = ownerLeft + (ownerWidth - me.Width) / 2;

				me.Top = top;
				me.Left = left;
			}
			catch
			{
				me.Top = 400;
				me.Left = 400;
			}
		}

	#endregion

	#region window events

		private void BtnOK_OnClick(object sender, RoutedEventArgs e)
		{
			result = MessageBoxResult.OK;

			this.Close();
		}

		

	#endregion

	#region event handling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion


	}
}