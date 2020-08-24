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

namespace WpfShared.Dialogs
{
	/// <summary>
	/// Interaction logic for DialogRequestValues.xaml
	/// </summary>
	public partial class DialogRequestValues : Window, INotifyPropertyChanged
	{
		public string nl = Environment.NewLine;

		public string[] TestStrings { get; private set; } = new [] {"string 1", "string 2"};

		public string[] ResultStrings { get; set; } = new string[2];

		public DialogRequestValues()
		{
			InitializeComponent();
		}

		// public static readonly DependencyProperty TextMessageProperty = DependencyProperty.Register(
		// 	"TextMessage", typeof(string), typeof(DialogRequestValues), new PropertyMetadata(default(string)));
		//
		// public string TextMessage
		// {
		// 	get { return (string) GetValue(TextMessageProperty); }
		// 	set { SetValue(TextMessageProperty, value); }
		// }


		public string TextBlockText { get; set; }

	#region TextBoxMessage

		public static readonly DependencyProperty TextBoxMessageProperty = DependencyProperty.RegisterAttached(
			"TextBoxMessage", typeof(string), typeof(DialogRequestValues), new PropertyMetadata(default(string)));

		public static void SetTextBoxMessage(UIElement obj, string value)
		{
			obj.SetValue(TextBoxMessageProperty, value);
		}

		public static string GetTextBoxMessage(UIElement obj)
		{
			return (string) obj.GetValue(TextBoxMessageProperty);
		}

	#endregion

		// this occurs before the value is put into the array
		private void TxBox_OnLostFocus(object sender, RoutedEventArgs e)
		{
			TextBox tbx = (TextBox) sender;

			string msg = "parse failed";
			int idx;
			bool result = int.TryParse((string) tbx.Tag, out idx);

			if (result)
			{
				msg = "  parse worked| text from array| " + ResultStrings[idx];

			}

			TextBlockText = "lost focus| tag| " + tbx.Tag
				+ "  text| " + tbx.Text
				+ msg + nl;

			OnPropertyChange("TextBlockText");

		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
