using System.Windows;
using System.Windows.Controls;

namespace Sylvester
{
	/// <summary>
	/// Interaction logic for lvTestControl.xaml
	/// </summary>
	public partial class lvTestControl : UserControl
	{
		public lvTestControl()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty CheckBoxVisibilityProperty = DependencyProperty.Register(
			"CheckBoxVisibility", typeof(Visibility), typeof(lvTestControl), new PropertyMetadata(Visibility.Visible));

		public Visibility CheckBoxVisibility
		{
			get { return (Visibility) GetValue(CheckBoxVisibilityProperty); }
			set { SetValue(CheckBoxVisibilityProperty, value); }
		}

		public static readonly DependencyProperty CanSelectProperty = DependencyProperty.Register(
			"CannotSelect", typeof(bool), typeof(lvTestControl), new PropertyMetadata(true) );

		public bool CanSelect
		{
			get { return (bool) GetValue(CanSelectProperty); }
			set { SetValue(CanSelectProperty, value); }
		}
	}
}