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
using StoreAndRead.TestClasses;

namespace StoreAndRead.Windows
{
	public partial class MainWindow : Window
	{
		List<BaseClass> baseClasses = new List<BaseClass>();

		public MainWindow()
		{
			InitializeComponent();

			BaseClass.bc.Init();

			for (int i = 0; i < 3; i++)
			{
				baseClasses.Add(new Derived1("D1-"+i));
			}

			for (int i = 0; i < 3; i++)
			{
				baseClasses.Add(new Derived2("D2-"+i));
			}
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			BaseClass.bc.RaiseCommonFunctionEvent();

		}
	}
}
