using System.Windows;
using System.Windows.Controls;


namespace Sylvester
{
	/// <summary>
	/// Interaction logic for HeaderControl.xaml
	/// </summary>
	public partial class HeaderControl : UserControl
	{
		public HeaderControl()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string),
				typeof(HeaderControl),
				new PropertyMetadata("")
				);

		public string Title
		{
			get { return (string) GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

	}
}
