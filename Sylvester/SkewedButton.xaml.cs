using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sylvester
{
    /// <summary>
    /// Interaction logic for SkewedButton.xaml
    /// </summary>
    public partial class SkewedButton : UserControl
    {
        public SkewedButton()
        {
            InitializeComponent();
        }

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(SkewedButton), new PropertyMetadata(default(string)));

		public string Text
		{
			get { return (string) GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
			"Index", typeof(int), typeof(SkewedButton), new PropertyMetadata(-1));

		public int Index
		{
			get { return (int) GetValue(IndexProperty); }
			set { SetValue(IndexProperty, value); }
		}

		private void InnerButton_OnClick(object sender, RoutedEventArgs e) { }
	}
}
